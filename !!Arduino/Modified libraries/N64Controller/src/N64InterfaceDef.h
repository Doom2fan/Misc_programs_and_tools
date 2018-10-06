#ifdef PORTLETTER

#define CAT(x, y) CAT_(x, y)
#define CAT_(x, y) x ## y
#define CLASSNAME CAT(N64Interface_PIN, PORTLETTER)

class CLASSNAME : public N64Interface {
public:
    CLASSNAME(unsigned char pincode) : N64Interface(pincode) {};
    virtual void init();
    virtual void send(unsigned char * buffer, char length);
    virtual void get();
};

#undef CLASSNAME
#undef CAT
#undef CAT_

#endif