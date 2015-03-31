all:
	g++ src/Main.cpp -c -Wall -o Main.o -lglui -lm -lGL -lglut -lGLU -g
	g++ src/classes.cpp -c -Wall -o classes.o 
	g++ src/Plot.cpp -c -o Plot.o -Wall -lglui -lglut -lGL -lm -lGLU
	g++ -o bin/RUNME Main.o Plot.o classes.o -Wall -lglui -lglut -lGL -lGLU
	rm Main.o Plot.o classes.o
	./bin/RUNME -g
#	nvcc RK4.cu card.cpp -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -ldl -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64
#	nvcc RK4.cu -o RK4 -arch=sm_13 -lm -Xcompiler -fopenmp -lgomp -I/home/uspl/NVIDIA_GPU_Computing_SDK/C/common/inc -I/home/uspl/install/include -lcublas -lcufft -lcudart -L/home/uspl/NVIDIA_GPU_Computing_SDK/C/lib -lcutil_x86_64 -ldl
