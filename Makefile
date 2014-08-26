all:
	g++ src/Main.cpp -c -Wall -o Main.o -lglui -lm -lGL -lglut -lGLU -g
	g++ src/classes.cpp -c -Wall -o classes.o 
	g++ src/Plot.cpp -c -o Plot.o -Wall -lglui -lglut -lGL -lm -lGLU
	g++ -o bin/RUNME Main.o Plot.o classes.o -Wall -lglui -lglut -lGL -lm -lGLU
	rm Main.o Plot.o classes.o
	./bin/RUNME -g
#	nvcc RK4.cu card.cpp -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -ldl -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64
#	nvcc RK4.cu -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64 -ldl

remove:
	g++ src/USPL.cpp -c -Wall -o USPL.o -lglui -lm -lGL -lglut -lGLU 
	nvcc -I ~/NVIDIA_GPU_Computing_SDK/C/common/inc src/RK4.cu -c -o RK4.o -lglui -lglut -lGL -lm -lcublas -arch=sm_13 -lGLU -Xcompiler -fopenmp -lgomp -ldl
	nvcc -I ~/NVIDIA_GPU_Computing_SDK/C/common/inc src/PlasmaDecay.cu -c -o Decay.o -lglui -lglut -lGL -lm -lcublas -arch=sm_13 -lGLU 
	nvcc -I ~/NVIDIA_GPU_Computing_SDK/C/common/inc -o RUNME RK4.o USPL.o Decay.o -lglui -lglut -lGL -lm -lcublas -arch=sm_13 -lGLU -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64 -Xcompiler -fopenmp -lgomp -ldl -g
	rm RK4.o USPL.o Decay.o
	rm -r DataOutput/640r640t100.000dz10.010dz2100z1\ \ 0z20.00pf
	./RUNME -g
#	nvcc RK4.cu card.cpp -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -ldl -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64
#	nvcc RK4.cu -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64 -ldl
