/*
# _____     ___ ____     ___ ____
#  ____|   |    ____|   |        | |____|
# |     ___|   |____ ___|    ____| |    \    PS2DEV Open Source Project.
#-----------------------------------------------------------------------
# Copyright 2001-2004, ps2dev - http://www.ps2dev.org
# Licenced under Academic Free License version 2.0
# Review ps2sdk README & LICENSE files for further details.
#
# $Id: echo.c 1152 2005-06-12 17:49:50Z oopo $
*/

#include <tamtypes.h>
#include <sifcmd.h>
#include <kernel.h>
#include <sifrpc.h>
#include <debug.h>
#include <stdio.h>
#include <loadfile.h>
#include <string.h>
#include <malloc.h>
#include <fileio.h>

#include "libpad.h"
#include "ps2ip.h"

#define ROM_PADMAN

const char *ipConf = "192.168.0.10 255.255.255.0 192.168.0.1";

typedef struct ControlInfo {
    unsigned char x,  y,   // Left analog stick.
                  z, rz;   // Right analog stick.
    __uint32_t    buttons; // Buttons. Bitfield. { 1: Triangle, 2: Circle, 3: Cross, 4: Square, 5: L1, 6: R1, 7: L2, 8: R2, 9: L3, 10: R3, 11: Select, 12: Start }
    unsigned char pov;     // D-pad. Bitfield. { 1: Up, 2: Right, 3: Down, 4: Left }
} ControlInfo;

void serverThread ();

int main (int argc, char *argv[]) {
    SifInitRpc (0);
    fioInit();

    init_scr ();

    SifLoadModule ("mc0:/BOOT/IOPTRAP.IRX", 0, NULL);
    SifLoadModule ("mc0:/BOOT/PS2DEV9.IRX", 0, NULL);
    SifLoadModule ("mc0:/BOOT/PS2SMAP.IRX", strlen (ipConf), ipConf);
    SifLoadModule ("rom0:SIO2MAN", 0, NULL);
    SifLoadModule ("rom0:MCMAN", 0, NULL);
    SifLoadModule ("rom0:MCSERV", 0, NULL);
    SifLoadModule ("host:DNS.IRX", 0, NULL);
    SifLoadModule ("host:PS2IPS.IRX", 0, NULL);
    
    nprintf ("1\n");

    if (ps2ip_init () < 0) {
        nprintf ("ERROR: ps2ip_init falied!\n");
        SleepThread ();
    }

    serverThread ();
    return 0;
}

static char padBuf [256] __attribute__ ((aligned (64)));
static char actAlign[6];
static int actuators;

void loadModules () {
    int ret;


#ifdef ROM_PADMAN
    ret = SifLoadModule ("rom0:SIO2MAN", 0, NULL);
#else
    ret = SifLoadModule ("rom0:XSIO2MAN", 0, NULL);
#endif
    if (ret < 0) {
        nprintf ("sifLoadModule sio failed: %d\n", ret);
        SleepThread ();
    }

#ifdef ROM_PADMAN
    ret = SifLoadModule ("rom0:PADMAN", 0, NULL);
#else
    ret = SifLoadModule ("rom0:XPADMAN", 0, NULL);
#endif 
    if (ret < 0) {
        nprintf ("sifLoadModule pad failed: %d\n", ret);
        SleepThread ();
    }
}
int waitPadReady (int port, int slot) {
    int state;
    int lastState;
    char stateString [16];

    state = padGetState (port, slot);
    lastState = -1;
    while ((state != PAD_STATE_STABLE) && (state != PAD_STATE_FINDCTP1)) {
        if (state != lastState) {
            padStateInt2String (state, stateString);
            nprintf ("Please wait, pad(%d,%d) is in state %s\n",
                        port, slot, stateString);
        }
        lastState = state;
        state = padGetState (port, slot);
    }
    if (lastState != -1) { // Were the pad ever 'out of sync'?
        nprintf ("Pad OK!\n");
    }

    return 0;
}
int initializePad (int port, int slot) {

    int ret;
    int modes;
    int i;

    waitPadReady (port, slot);

    // How many different modes can this device operate in?
    // i.e. get # entrys in the modetable
    modes = padInfoMode (port, slot, PAD_MODETABLE, -1);
    nprintf ("The device has %d modes\n", modes);

    if (modes > 0) {
        nprintf ("( ");
        for (i = 0; i < modes; i++) {
            nprintf ("%d ", padInfoMode (port, slot, PAD_MODETABLE, i));
        }
        nprintf (")");
    }

    nprintf ("It is currently using mode %d\n",
                padInfoMode (port, slot, PAD_MODECURID, 0));

    // If modes == 0, this is not a Dual shock controller 
    // (it has no actuator engines)
    if (modes == 0) {
        nprintf ("This is a digital controller?\n");
        return 1;
    }

    // Verify that the controller has a DUAL SHOCK mode
    i = 0;
    do {
        if (padInfoMode (port, slot, PAD_MODETABLE, i) == PAD_TYPE_DUALSHOCK)
            break;
        i++;
    } while (i < modes);
    if (i >= modes) {
        nprintf ("This is no Dual Shock controller\n");
        return 1;
    }

    // If ExId != 0x0 => This controller has actuator engines
    // This check should always pass if the Dual Shock test above passed
    ret = padInfoMode (port, slot, PAD_MODECUREXID, 0);
    if (ret == 0) {
        nprintf ("This is no Dual Shock controller??\n");
        return 1;
    }

    nprintf ("Enabling dual shock functions\n");

    // When using MMODE_LOCK, user cant change mode with Select button
    padSetMainMode (port, slot, PAD_MMODE_DUALSHOCK, PAD_MMODE_LOCK);

    waitPadReady (port, slot);
    nprintf ("infoPressMode: %d\n", padInfoPressMode (port, slot));

    waitPadReady (port, slot);
    nprintf ("enterPressMode: %d\n", padEnterPressMode (port, slot));

    waitPadReady (port, slot);
    actuators = padInfoAct (port, slot, -1, 0);
    nprintf ("# of actuators: %d\n", actuators);

    if (actuators != 0) {
        actAlign [0] = 0;   // Enable small engine
        actAlign [1] = 1;   // Enable big engine
        actAlign [2] = 0xff;
        actAlign [3] = 0xff;
        actAlign [4] = 0xff;
        actAlign [5] = 0xff;

        waitPadReady (port, slot);
        nprintf ("padSetActAlign: %d\n",
                    padSetActAlign (port, slot, actAlign));
    } else {
        nprintf ("Did not find any actuators.\n");
    }

    waitPadReady (port, slot);

    return 1;
}

char buffer [1];
ControlInfo *ctrlInfo;
unsigned char sendBuffer [sizeof (ControlInfo)];

int HandleClient (int cs) {
    int rcvSize, sntSize;

    rcvSize = recv (cs, buffer, 1, 0);
    if (rcvSize <= 0) {
        nprintf ("PS2CONTROLLER: recv returned %i\n", rcvSize);
        return -1;
    }
    if (buffer [0] == 0x7F) { // Received polling byte. Send the controller data
       #ifdef SAFER_SBUFFER
        char *ctrlInfoBytes = (char *) ctrlInfo;
        size_t i;
        for (i = 0; i < sizeof (ControlInfo); i++)
            sendBuffer [i] = ctrlInfoBytes [i];
        sntSize = send (cs, sendBuffer, sizeof (ControlInfo), 0);
        #else
        sntSize = send (cs, ctrlInfo, sizeof (ControlInfo), 0);
        #endif
    } else
        nprintf ("PS2CONTROLLER: Got a value that wasn't 0x7F. wtf.\n"); // Just silently ignore it .-.

    return 0;
}

void serverThread () {
    
    nprintf ("1\n");
    loadModules ();

    padInit (0);

    ctrlInfo = malloc (sizeof (ControlInfo));

    int sh;
    int cs;
    struct sockaddr_in echoServAddr;
    struct sockaddr_in echoClntAddr;
    int clntLen;
    int rc;
    fd_set active_rd_set;
    fd_set rd_set;
    int ret;
    int port, slot;
    u32 paddata;
    u32 old_pad = 0;
    u32 new_pad;
    struct padButtonStatus buttons;

    port = 0; slot = 0;

    if ((ret = padPortOpen (port, slot, padBuf)) == 0) {
        nprintf ("padOpenPort failed: %d\n", ret);
        SleepThread ();
    }

    if (!initializePad (port, slot)) {
        nprintf ("pad initalization failed!\n");
        SleepThread ();
    }

    nprintf ("PS2CONTROLLER: Server Thread Started.\n");


    sh = socket (AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (sh < 0) {
        nprintf ("PS2CONTROLLER: Socket failed to create.\n");
        SleepThread ();
    }

    nprintf ("PS2CONTROLLER: Got socket.. %i\n", sh);


    memset (&echoServAddr, 0, sizeof (echoServAddr));
    echoServAddr.sin_family = AF_INET;
    echoServAddr.sin_addr.s_addr = htonl (INADDR_ANY);
    echoServAddr.sin_port = htons (5120);

    rc = bind (sh, (struct sockaddr *) &echoServAddr, sizeof (echoServAddr));
    if (rc < 0) {
        nprintf ("PS2CONTROLLER: Socket failed to bind.\n");
        SleepThread ();
    }

    nprintf ("PS2CONTROLLER: bind returned %i\n", rc);


    rc = listen (sh, 2);
    if (rc < 0) {
        nprintf ("PS2CONTROLLER: listen failed.\n");
        SleepThread ();
    }

    nprintf ("PS2CONTROLLER: listen returned %i\n", rc);

    FD_ZERO (&active_rd_set);
    FD_SET (sh, &active_rd_set);
    while (1) {
        ret = padGetState (port, slot);
        if ((ret != PAD_STATE_STABLE) && (ret != PAD_STATE_FINDCTP1)) { // Do nothing. Fek this, this is just quick and dirty.
        } else {
            ret = padRead(port, slot, &buttons); // port, slot, buttons
            paddata = 0xffff ^ buttons.btns;
            new_pad = paddata & ~old_pad;

            // Axes
            ctrlInfo->x = buttons.ljoy_h; ctrlInfo->y  = buttons.ljoy_v;
            ctrlInfo->z = buttons.rjoy_h; ctrlInfo->rz = buttons.rjoy_v;
            // Buttons
            ctrlInfo->buttons = ((new_pad & PAD_START) == PAD_START)   << 11 |
                                ((new_pad & PAD_SELECT) == PAD_SELECT) << 10 |
                                ((new_pad & PAD_R3) == PAD_R3)         << 9 |
                                ((new_pad & PAD_L3) == PAD_L3)         << 8 |
                                ((new_pad & PAD_R2) == PAD_R2)         << 7 |
                                ((new_pad & PAD_L2) == PAD_L2)         << 6 |
                                ((new_pad & PAD_R1) == PAD_R1)         << 5 |
                                ((new_pad & PAD_L1) == PAD_L1)         << 4 |
                                ((new_pad & PAD_SQUARE) == PAD_SQUARE) << 3 |
                                ((new_pad & PAD_CROSS) == PAD_CROSS)   << 2 |
                                ((new_pad & PAD_CIRCLE) == PAD_CIRCLE) << 1 |
                                ((new_pad & PAD_TRIANGLE) == PAD_TRIANGLE);
            // POV
            ctrlInfo->pov = ((new_pad & PAD_LEFT) == PAD_LEFT)   << 3 |
                            ((new_pad & PAD_DOWN) == PAD_DOWN)   << 2 |
                            ((new_pad & PAD_RIGHT) == PAD_RIGHT) << 1 |
                            ((new_pad & PAD_UP) == PAD_UP);
        }

        int i;
        clntLen = sizeof (echoClntAddr);
        rd_set = active_rd_set;
        if (select (FD_SETSIZE, &rd_set, NULL, NULL, NULL) < 0) {
            nprintf ("PS2CONTROLLER: Select failed.\n");
            SleepThread ();
        }

        for (i = 0; i < FD_SETSIZE; i++) {
            if (FD_ISSET (i, &rd_set)) {
                if (i == sh) {
                    cs = accept (sh, (struct sockaddr *)&echoClntAddr, &clntLen);
                    if (cs < 0) {
                        nprintf ("PS2CONTROLLER: accept failed.\n");
                        SleepThread ();
                    }
                    FD_SET (cs, &active_rd_set);
                    nprintf ("PS2CONTROLLER: accept returned %i.\n", cs);
                } else {
                    if (HandleClient (i) < 0) {
                        FD_CLR (i, &active_rd_set);
                        disconnect (i);
                    }
                }
            }
        }
    }

    SleepThread ();
}
