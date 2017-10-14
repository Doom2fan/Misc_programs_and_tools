EE_BIN = ps2ctrl.elf
EE_CFLAGS = -g
EE_OBJS = main.o
EE_LIBS = -lpad -lps2ip -ldebug -lc

all: $(EE_BIN)

clean:
	rm -f *.elf *.o *.a

# Include settings for ps2lib/newlib
include $(PS2LIB)/ee/Makefile.include
include $(PS2SDK)/Makefile.pref
include $(PS2SDK)/Makefile.eeglobal