#ifdef USEGPU
#define GPU_THREADS 1
#endif

#ifndef GPU_THREADS
#define GPU_THREADS 1
#endif

#ifndef THREADS
#define THREADS 3
#endif

#undef I_SIZE
#define I_SIZE 15