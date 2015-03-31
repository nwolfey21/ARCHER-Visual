ARCHER-Visual User Manual
=======================================================================

Application High Level Description
-----------------------------------------------------------------------
This application was developed for the purpose of visualizing the
ARCHER Monte Carlo Computed Tomography (CT) Radiation Dose Simulation.
The application loads in the phantom OBJ file and parses the data into 
objects that are rendered to the screen. The application can also load
particle trajectory data (computed and saved from the ARCHER CT simul-
ation) from file, where it is also parsed into objects to be rendered.

Using the Configuration GUI, the user can chose to statically overlay
the particles to show their trajectory through the phantom domain or 
the user can choose to animate the particles. The animation iterates 
through all available particles and displays each position at each 
step. The steps have no relation to physical time. Each position along
each particle trajectory simply indicates a time in the simulation at 
which the position was sampled.

Application Low Level Description
-----------------------------------------------------------------------
All rendering is done using OpenGL. The GLUT and GLUI frameworks are 
used to assist with the OpenGL development as they provide simple 
functions for built in buttons, windows, lists, etc. for OpenGL. 

Compiling and Running
-----------------------------------------------------------------------
  - Using the command line, change directory to ARCHER-Visual.
  - type make and hit enter. This will compile and execute the code.

Required Libraries/Packages
-----------------------------------------------------------------------
This application requires OpenGL, GLUT and GLUI to be installed.
If errors are returned during the compilation regarding either of
the three, then install the packages accordingly.

  - Scientific Linux (mc-rrmdg server)
    ~ GLUT: sudo yum install freeglut freeglut-devel
    ~ GLUI: sudo yum install glui
  - Ubuntu Linux 
    ~ GLUT: sudo apt-get install freeglut freeglut-devel
    ~ GLUI: sudo apt-get install glui
