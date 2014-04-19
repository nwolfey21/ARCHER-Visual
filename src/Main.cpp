/*******************************************************************

Rensselaer Polytechnic Institute

********************************************************************

Discription:
  This is the main project code for the visualization of ARCHER-CT
  simulations.

Version: 1.0

Currently: 04/19/2014
  Initial Code base

*******************************************************************/
/* Propagation Code Headers */
#include <stdlib.h>
#include <stdio.h>
#include <math.h>
#include <time.h>
#include <string.h>
#include <complex.h>

/* OpenGL Code Headers */
#include <GL/glui.h>

/*************External Functions***************/
// Updated
void runSim(int crap);
void runSimNoGUI(int crap);
float * initialize(int *sn, int *prints, int propDistance, int rDomain, int xDomain, int yDomain, int tDomain, int zDomain, int iflag, int inlambda0, int inrmax, int intmax, int show_cart, int inLCR, int taup, int w0, float inEnergy, int inpf, float inchirp, char *outDirectory);
void finalize();
void drawTemporal( int x1, float x2, float y1, float y2, int flag);
float drawSpatial(int N, int flag);
void drawSpectral( int N, int flag);
void drawSpaceTime( int N, int flag);
void drawMaxIntensity( int N, int flag);
float drawBeamWaist( int N, int flag);
void plasmaDecay(int nr, int nt, int step_num, double intmax, double inconsp3, char *directory);

/*************Internal Functions***************/
void realtimeDisplay( void );
void draw_temporal_axes(float scale);
void spatialReshape( int x, int y );
void spatialDisplay( void );
void spectralDisplayInit( void );
void spectralDisplay( void );
void spacetimeDisplayInit( void );
void spacetimeDisplay( void );
void maxIntensityDisplayInit( void );
void maxIntensityDisplay( void );
void beamWaistDisplayInit( void );
void beamWaistDisplay( void );

float elapsed_cpu;

float version = 3.93;
float xy_aspect;
int   last_x, last_y;
float rotationX = 0.0, rotationY = 0.0;

/** Time Variables **/
clock_t start, end, endi, intermediate;

/** Propagation variables **/
int flag[12];
int prints[10];
int propDistance = 100;
int rDomain = 640;
int xDomain = 0;
int yDomain = 0;
int tDomain = 640;
int zDomain = 1/*10000*/;
int inrmax = 20;
int intmax = 200;
int inlambda0 = 800;
int inLCR = 60;
float inchirp = -5.75;
float inEnergy = 35;
int inpf = 0;
int inw0 = 10;
int intaup = 55;
int mt;			// current z step
int flagInit = 0;
float *values;
float powr = 0;
float ener = 0;
float diameter = 0;
float tPlasMax = 1.67;
int tPlasDomain = 100;
float tDecay = 2.77;
char *Directory;

/** These are the live variables passed into GLUI ***/
int   wireframe = 0;
int   obj_type = 1;
int   segments = 8;
int   segments2 = 8;
int   light0_enabled = 1;
int   light1_enabled = 1;
float light0_intensity = 1.0;
float light1_intensity = .4;
int   temporalWindow;
int   spatialWindow;
int   spectralWindow;
int   spacetimeWindow;
int   maxIntensityWindow;
int   beamWaistWindow;
int   realtimeWindow;
float scale = 1.0;
int   show_sphere=1;
int   show_torus=1;
int   show_axes = 1;
int   show_text = 1;
int   show_diffraction=1;
int   show_stf=0;
int   show_dispersion=0;
int   show_spm=0;
int   show_ss=0;
int   show_mpi=0;
int   show_plasma=0;
int   show_lens=0;
int   show_raman=0;
int   show_avalanche=0;
int   show_cyl=1;
int   show_cart=0;
int   show_spatial=0;
int   show_temporal=0;
int   show_space=0;
int   show_spectral=0;
int   show_beam=1;
int   show_max=1;
int   print_spatDom=1;
int   print_plasDom=1;
int   print_spatVid=1;
int   print_plasVid=1;
int   print_plasSim=1;
int   input_pf=0;
int   input_energy=1;
float spatial_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_spatial_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float spatial_pos[] = { 0.0, 0.0, 0.0 };
float spectral_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_spectral_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float spectral_pos[] = { 0.0, 0.0, 0.0 };
float spacetime_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_spacetime_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float spacetime_pos[] = { 0.0, 0.0, 0.0 };
float maxIntensity_pos[] = { 0.0, 0.0, 0.0 };
float beamWaist_pos[] = { 0.0, 0.0, 0.0 };
float maxIntensity_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_maxIntensity_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float beamWaist_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_beamWaist_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float temporal_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_temporal_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float temporal_pos[] = { 0.0, 0.0, 0.0 };
float sphere_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float torus_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float view_rotate[16] = { 1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };
float obj_pos[] = { 0.0, 0.0, 0.0 };
char  strDistance[17] = "Distance: 0cm";
char  strEnergy[20] = "Energy: 0.0 mJ";
char  strPower[20] = "Power: 0.0 GW";
char  strIntensity[26] = "Intensity: 0.0 arb";
char  strDiameter[20] = "Beam Waist: cm";
char  strTimeLeft[28] = "Time Remaining: 0 secs";
char  strTimeRunning[30] = "Elapsed Time: 0 secs";
char pulseString_list[15] = "Gaussian";
char wavelengthString_list[10] = "800";
int   curr_string = 0;

/** Pointers to the windows and some of the controls we'll create **/
GLUI            *spectralRightSubWindow, *spatialSubWindow, *spectralSubWindow, *temporalBottomSubWIndow, *controlWindow, *spacetimeSubWindow, *maxIntensitySubWindow, *propSubWindow, *beamWaistSubWindow;
GLUI            *realtimeTopSubWindow, *realtimeBottomSubWindow;
GLUI_Spinner    *light0_spinner, *light1_spinner;
GLUI_StaticText *text, *energy, *power, *intensity, *energyval, *powerval, *intensityval;
GLUI_StaticText *indistance, *distanceval, *radius, *radiusval;
GLUI_RadioGroup *radio;
GLUI_Panel      *obj_panel, *import_panel, *control_panel;
GLUI_Panel      *realtime_top_panel, *realtime_bottom_panel;
GLUI_EditText   *fwhmText;

/********** User IDs for callbacks ********/
#define LIGHT0_ENABLED_ID    200
#define LIGHT1_ENABLED_ID    201
#define LIGHT0_INTENSITY_ID  250
#define LIGHT1_INTENSITY_ID  260
#define ENABLE_ID            300
#define DISABLE_ID           301
#define SHOW_ID              302
#define HIDE_ID              303
#define INITIALIZE           231
#define PROPAGATE	     232
#define PAUSE		     233
#define DECAY	     	     234

/********** Miscellaneous global variables **********/
GLfloat light0_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light0_diffuse[] =  {.6f, .6f, 1.0f, 1.0f};
GLfloat light0_position[] = {.5f, .5f, 1.0f, 0.0f};

GLfloat light1_ambient[] =  {0.1f, 0.1f, 0.3f, 1.0f};
GLfloat light1_diffuse[] =  {.9f, .6f, 0.0f, 1.0f};
GLfloat light1_position[] = {-1.0f, -1.0f, 1.0f, 0.0f};

GLfloat lights_rotation[16] = {1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1 };


/***********************************************************************/
/*                          control_cb()                               */
/*                      GLUI control callback                          */
/***********************************************************************/
void control_cb( int control )
{
	printf("control_cb \n");
	if ( control == LIGHT0_ENABLED_ID ) 
	{
		if ( light0_enabled ) 
		{
			glEnable( GL_LIGHT0 );
			light0_spinner->enable();
		}
		else 
		{
			glDisable( GL_LIGHT0 ); 
			light0_spinner->disable();
		}
	}
	else if ( control == LIGHT1_ENABLED_ID ) 
	{
		if ( light1_enabled ) 
		{
			glEnable( GL_LIGHT1 );
			light1_spinner->enable();
		}
		else 
		{
			glDisable( GL_LIGHT1 ); 
			light1_spinner->disable();
		}
	}
	else if ( control == LIGHT0_INTENSITY_ID ) 
	{
		float v[] = {light0_diffuse[0], light0_diffuse[1], light0_diffuse[2], light0_diffuse[3]};
		v[0] *= light0_intensity;
		v[1] *= light0_intensity;
		v[2] *= light0_intensity;
		glLightfv(GL_LIGHT0, GL_DIFFUSE, v );
	}
	else if ( control == LIGHT1_INTENSITY_ID ) 
	{
		float v[] = {light1_diffuse[0], light1_diffuse[1], light1_diffuse[2], light1_diffuse[3]};
		v[0] *= light1_intensity;
		v[1] *= light1_intensity;
		v[2] *= light1_intensity;
		glLightfv(GL_LIGHT1, GL_DIFFUSE, v );
	}
	else if ( control == ENABLE_ID )
	{
		temporalBottomSubWIndow->enable();			// Enables temporalBottomSubWindow (controls)
	}
	else if ( control == DISABLE_ID )
	{
		temporalBottomSubWIndow->disable();			// disables temporalBottomSubWindow (controls)
	}
	else if ( control == SHOW_ID )
	{
		temporalBottomSubWIndow->show();			// disables temporalBottomSubWindow (controls)
	}
	else if ( control == HIDE_ID )
	{
		temporalBottomSubWIndow->hide();			// disables temporalBottomSubWindow (controls)
	}
	else if ( control == INITIALIZE )
	{
		flagInit = 1;
		flag[0] = show_diffraction;
		flag[1] = show_spm;
		flag[2] = show_dispersion;
		flag[3] = show_mpi;
		flag[4] = show_ss;
		flag[5] = show_plasma;
		flag[6] = show_lens;
		flag[7] = 0;
		flag[8] = input_pf;
		flag[9] = input_energy;
		prints[0] = print_spatDom;	//Print Spatial Domain
		prints[1] = print_plasDom;	//Print Plasma Domain
		prints[2] = print_spatVid;	//Print Spatial Video
		prints[3] = print_plasVid;	//Print Plasma Video
		prints[4] = print_plasSim;	//Print Plasma Lifecycle
//		values = initialize(flag, prints, propDistance, rDomain, xDomain, yDomain, tDomain, zDomain, 1, inlambda0, inrmax, intmax, show_cyl, inLCR, intaup, inw0, inEnergy, inpf, inchirp, Directory);
		powr = values[0];
		ener = values[1];
		sprintf(strPower, "Power: %.2f GW", powr*1e-9);
		sprintf(strEnergy, "Energy: %.2f mJ", ener*1e3);
		glutSetWindow(spatialWindow);
		spatialDisplay();
		glutSetWindow(realtimeWindow);
		realtimeDisplay();		
	}
	else if ( control == PROPAGATE ) 	// avalanche
	{
		printf("\n\n--------------Propagate------------\n");
		int mod;
		int mod2;
		int mod2Rate = 50;
		float avgtime;
		float timeleft;
		start = clock();
		intermediate = clock();
		for( mt = 1; mt <= zDomain; mt++)
		{
			mod2 = mt % mod2Rate;
			if(mod2 == 0)
			{
				endi = clock();
				avgtime = (double)(endi-start) / mt;
				timeleft = ((zDomain - (float)mt)) * avgtime;
			    	sprintf(strTimeLeft, "Time Remaining: %.2f secs", timeleft/CLOCKS_PER_SEC);
				intermediate = endi;
			}
			mod = mt % 1;
			if(mod == 0)
			{
				sprintf(strDistance, "Distance: %.2fcm", (float)mt*((float)propDistance)/zDomain);
			}
			glutSetWindow(temporalWindow);
//			runSim(mt);
			if( control == PAUSE )
			{
				break;
			}
			mod = mt % 1;
			if(mod == 0)
			{
				end = clock();	
				elapsed_cpu = ((double) (end - start)) / CLOCKS_PER_SEC*2.0;
				sprintf(strTimeRunning, "Elapsed Time: %.2f secs", elapsed_cpu);	
				glutSetWindow(spatialWindow);
				spatialDisplay();
				glutSetWindow(realtimeWindow);
				realtimeDisplay();
			}
//	printf("Crap1\n");
		}
		printf("elapsed time for matrix exponential on cpu = %f seconds \n", elapsed_cpu);		
//		finalize();			
	}
	else if ( control == DECAY ) 	// avalanche
	{
		printf("\n\n--------------Plasma Decay------------\n");
		start = clock();
		intermediate = clock();
//		plasmaDecay(rDomain, tDomain, zDomain, tPlasMax, tDecay, Directory); 
	}
}


/***********************************************************************/
/*                         myGlutKeyboard()                            */
/***********************************************************************/
void myGlutKeyboard(unsigned char Key, int x, int y)
{
	printf("myglutkeyboard \n");
	switch(Key)
	{
		case 27: 
		case 'q':
			exit(0);
		break;
	};
	glutPostRedisplay();
}


/***********************************************************************/
/*                           myGlutMenu()                              */
/***********************************************************************/
void myGlutMenu( int value )
{
	printf("myglutmenu \n");
	myGlutKeyboard( value, 0, 0 );
}


/***********************************************************************/
/*                           myGlutIdle()                              */
/***********************************************************************/
void myGlutIdle( void )
{
	printf("myglutidle \n");
	if ( glutGetWindow() != temporalWindow ) 
		glutSetWindow(temporalWindow);  
	glutPostRedisplay();
}


/***********************************************************************/
/*                           myGlutMouse()                             */
/***********************************************************************/
void myGlutMouse(int button, int button_state, int x, int y )
{
	printf("myglutmouse \n");
}


/***********************************************************************/
/*                           myGlutMotion()                            */
/***********************************************************************/
void myGlutMotion(int x, int y )
{
	printf("myglutmotion \n");
	glutPostRedisplay(); 
}


/***********************************************************************/
/*                           spatialReshape()                           */
/***********************************************************************/
void spatialReshape( int x, int y )
{
	printf("spatialReshape \n");
	glutSetWindow(spatialWindow);
	int tx, ty, tw, th;
	GLUI_Master.get_viewport_area( &tx, &ty, &tw, &th );
	glViewport( tx, ty, tw, th );
	xy_aspect = (float)tw / (float)th;
	glutPostRedisplay();
}


/***********************************************************************/
/*                           draw_spatial_axes()                       */
/*               Disables lighting, then draws RGB axes                */
/***********************************************************************/
void draw_spatial_axes( float scale )
{
	printf("draw_spatial_axes\n");
	glDisable( GL_LIGHTING );
	glPushMatrix();
	glScalef( scale, scale, scale );
	glLineWidth(2.0); 
	glBegin( GL_LINES );
	/* Letter X */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 1.77f, -0.05f, 0.0 );  glVertex3f( 1.9, -0.25f, 0.0 ); 
	glVertex3f( 1.77f, -.25f, 0.0 );  glVertex3f( 1.9, -0.05f, 0.0 );
	/* Letter Y */
	glColor3f( 0.0, 0.0, 0.0 );
	glVertex3f( -0.05f, 1.85f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 ); 
	glVertex3f( -0.15f, 1.85f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 );
	glVertex3f( -0.1f, 1.6f, 0.0 );  glVertex3f( -0.1, 1.74f, 0.0 ); 
	/* X axis */
	glColor3f( 1.0, 0.0, 0.0 );
	glVertex3f( 0.0, 0.0, 0.0 );  glVertex3f( 2.0, 0.0, 0.0 ); 
	/* Y axis */
	glColor3f( 0.0, 0.0, 0.0 );
	glVertex3f( 0.0, 0.0, 0.0 );  glVertex3f( 0.0, 2.0, 0.0 ); 
	glEnd();
	glPopMatrix();
	glEnable( GL_LIGHTING );
}


/***********************************************************************/
/*                           realtimeDisplay                           */
/***********************************************************************/
void realtimeDisplay( void )
{
	printf("realtimeDisplay \n");
	glClearColor( .9f, .9f, .9f, 1.0f );
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	if ( show_text ) 
	{
		glDisable( GL_LIGHTING );  /* Disable lighting while we render text */
		glMatrixMode( GL_PROJECTION );
		glLoadIdentity();
		gluOrtho2D( 0.0, 100.0, 0.0, 11.0 );
		glMatrixMode( GL_MODELVIEW );
		glLoadIdentity();
		glLineWidth(3.0);
		glColor3ub( 0, 0, 0 );
		glRasterPos2i( 20, 9 );
		/*** Render the live character array 'text' ***/
		int i;
		for( i=0; i<(int)strlen( strDistance ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strDistance[i] );
		}
		glRasterPos2i( 55, 9 );			// (x,y)
		for( i=0; i<(int)strlen( strEnergy ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strEnergy[i] );
		}
		glRasterPos2i( 20, 7 );
		for( i=0; i<(int)strlen( strPower ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strPower[i] );
		}
		glRasterPos2i( 55, 7 );
		for( i=0; i<(int)strlen( strIntensity ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strIntensity[i] );
		}
		glRasterPos2i( 20, 5 );
		for( i=0; i<(int)strlen( strDiameter ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strDiameter[i] );
		}
		glRasterPos2i( 55, 5 );
		for( i=0; i<(int)strlen( strTimeLeft ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strTimeLeft[i] );
		}
		glRasterPos2i( 35, 3 );
		for( i=0; i<(int)strlen( strTimeRunning ); i++ )
		{
			glutBitmapCharacter( GLUT_BITMAP_HELVETICA_18, strTimeRunning[i] );
		}
	}
	glLineWidth(2.0);
	glEnable( GL_LIGHTING );
	glutSwapBuffers(); 
}


/***********************************************************************/
/*                           spatialDisplay()                           */
/***********************************************************************/
void spatialDisplay( void )
{
	float intensity = 0;
	printf("spatialDisplay \n");
	glClearColor( .9f, .9f, .9f, 1.0f );
	glClear( GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT );
	glMatrixMode( GL_PROJECTION );

	glLoadIdentity();
	glFrustum( -xy_aspect*.04, xy_aspect*.04, -.04, .04, .1, 15.0 );
	glMatrixMode( GL_MODELVIEW );

	glLoadIdentity();
	glMultMatrixf( lights_rotation );
	glLightfv(GL_LIGHT0, GL_POSITION, light0_position);

	glLoadIdentity();
	glTranslatef( 0.0, 0.0, -2.6f );
	glTranslatef( spatial_pos[0], spatial_pos[1], -spatial_pos[2] ); 
	glMultMatrixf( view_spatial_rotate );

	glScalef( scale, scale, scale );

	glPushMatrix();
	glTranslatef( 0.0, 0.0, 0.0 );
	glMultMatrixf( spatial_rotate );
	if ( flagInit>0 )
	{
		draw_spatial_axes(.52f);
	}

	glTranslatef( 0.0, 0.0, 0.0 );
	glMultMatrixf( spatial_rotate );
	if( flagInit>0 )
	{
		intensity = drawSpatial(mt, show_spatial);
	}
	sprintf(strIntensity, "Intensity: %f arb", intensity);	

	glEnable( GL_LIGHTING );

	glutSwapBuffers(); 
}




/**********************************************************************************/
/*                                    MAIN                                        */
/**********************************************************************************/
int main(int argc, char* argv[])
{
	int i;
	for(i = 1; i < argc; ++i) 
	{
		/***********************************************************************/
		/*                 Initialize GLUT and Create Windows                  */
		/***********************************************************************/
		glutInit(&argc, argv);
		glutInitDisplayMode( GLUT_RGB | GLUT_DOUBLE | GLUT_DEPTH );


		/***********************************************************************/
		/*                      Setup Spatial Window                           */
		/***********************************************************************/
		/*** Create Window/Setup Callbacks***/
		glutInitWindowPosition( 1800, 25 );		// Top Left
		glutInitWindowSize( 580, 500 );
		spatialWindow = glutCreateWindow( "Spatial Intensity" );

		glutDisplayFunc( spatialDisplay );
		GLUI_Master.set_glutReshapeFunc( spatialReshape );  
		GLUI_Master.set_glutKeyboardFunc( myGlutKeyboard );
		GLUI_Master.set_glutSpecialFunc( NULL );
		GLUI_Master.set_glutMouseFunc( myGlutMouse );
		glutMotionFunc( myGlutMotion );
		/*** End Create Window/Setup Callbacks***/

		/*** Add Bottom SubWindow ***/
		spatialSubWindow = GLUI_Master.create_glui_subwindow( spatialWindow, GLUI_SUBWINDOW_BOTTOM );
		spatialSubWindow->set_main_gfx_window( spatialWindow );

		GLUI_Rotation *view_rot_spatial = new GLUI_Rotation(spatialSubWindow, "Rotate", view_spatial_rotate );
		view_rot_spatial->set_spin( 1.0 );
		new GLUI_Column( spatialSubWindow, false );
		GLUI_Translation *trans_xy_spatial = new GLUI_Translation(spatialSubWindow, "XY", GLUI_TRANSLATION_XY, spatial_pos );
		trans_xy_spatial->set_speed( .005 );
		new GLUI_Column( spatialSubWindow, false );
		GLUI_Translation *trans_x_spatial = new GLUI_Translation(spatialSubWindow, "X", GLUI_TRANSLATION_X, spatial_pos );
		trans_x_spatial->set_speed( .005 );
		new GLUI_Column( spatialSubWindow, false );
		GLUI_Translation *trans_y_spatial = new GLUI_Translation(spatialSubWindow, "Y", GLUI_TRANSLATION_Y, &spatial_pos[1]);
		trans_y_spatial->set_speed( .005 );
		new GLUI_Column( spatialSubWindow, false );
		GLUI_Translation *trans_z_spatial = new GLUI_Translation(spatialSubWindow, "Z", GLUI_TRANSLATION_Z, &spatial_pos[2]);
		trans_z_spatial->set_speed( .005 );
		/*** End Bottom SubWindow ***/

		/***** Setup OpenGL lights *****/
		glEnable(GL_LIGHTING);
		glEnable( GL_NORMALIZE );

		glEnable(GL_LIGHT0);
		glLightfv(GL_LIGHT0, GL_AMBIENT, light0_ambient);
		glLightfv(GL_LIGHT0, GL_DIFFUSE, light0_diffuse);
		glLightfv(GL_LIGHT0, GL_POSITION, light0_position);

		glEnable(GL_LIGHT1);
		glLightfv(GL_LIGHT1, GL_AMBIENT, light1_ambient);
		glLightfv(GL_LIGHT1, GL_DIFFUSE, light1_diffuse);
		glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
		/***** End OpenGL lights *****/

		/***** Enable z-buferring *****/
		glEnable(GL_DEPTH_TEST);


		/***********************************************************************/
		/*                      Setup Real-Time Window                         */
		/***********************************************************************/
		/*** Create Window/Setup Callbacks***/
		glutInitWindowPosition( 2500, 292 );		// Bottom Right
		glutInitWindowSize( 580, 233 );
		realtimeWindow = glutCreateWindow( "Real-Time" );

		glutDisplayFunc( realtimeDisplay );
		GLUI_Master.set_glutKeyboardFunc( myGlutKeyboard );
		GLUI_Master.set_glutSpecialFunc( NULL );
		GLUI_Master.set_glutMouseFunc( myGlutMouse );
		/*** End Create Window/Setup Callbacks***/

		realtimeBottomSubWindow = GLUI_Master.create_glui_subwindow( realtimeWindow, GLUI_SUBWINDOW_BOTTOM );
		realtime_bottom_panel = new GLUI_Panel(realtimeBottomSubWindow, "Control" );

		//########## Add Bottom Panel ##########//
		/***** Add Buttons *****/
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		new GLUI_Button( realtime_bottom_panel, "Propagate", PROPAGATE, control_cb );
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		new GLUI_Button( realtime_bottom_panel, "Decay", DECAY, control_cb );
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		realtimeBottomSubWindow->add_column_to_panel(realtime_bottom_panel, false);
		/***** End Buttons *****/
		//########## End Bottom Panel ##########//


		/***********************************************************************/
		/*                      Create Control Window                          */
		/***********************************************************************/
		controlWindow = GLUI_Master.create_glui( "Configuration", 0, 0/*2500*/, 25 );
		control_panel = new GLUI_Panel(controlWindow, "" );
		import_panel = new GLUI_Panel(control_panel, "File Import" );

		/**** Add listbox ****/
		GLUI_Listbox *pulseList = new GLUI_Listbox( import_panel, "Pulse:", &curr_string );

		for( i=0; i<1; i++ )
			pulseList->add_item( i, pulseString_list );
		/**** End listbox ****/

		/**** Add Spinners ****/
		GLUI_Spinner *fwhmSpinner = new GLUI_Spinner( import_panel, "Pulse Width(fs):   ", &intaup);
		fwhmSpinner->set_int_limits( 1, 600 );
		fwhmSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *wavelengthSpinner = new GLUI_Spinner( import_panel, "Wavelength(nm):", &inlambda0);
		wavelengthSpinner->set_int_limits( 1, 1000 );
		wavelengthSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *lcrSpinner = new GLUI_Spinner( import_panel, "Lens Radius(cm):", &inLCR);
		lcrSpinner->set_int_limits( 1, 1000 );
		lcrSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *chirpSpinner = new GLUI_Spinner( import_panel, "Chirp(+or-):", &inchirp);
		chirpSpinner->set_float_limits( -100.0, 1000.0 );
		chirpSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *energySpinner = new GLUI_Spinner( import_panel, "Energy(mJ):", &inEnergy);
		energySpinner->set_float_limits( 0, 10000 );
		energySpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *pfSpinner = new GLUI_Spinner( import_panel, "Power Factor:", &inpf);
		pfSpinner->set_int_limits( 0, 1000 );
		pfSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *w0Spinner = new GLUI_Spinner( import_panel, "Beam Waist(mm):", &inw0);
		w0Spinner->set_int_limits( 0, 1000 );
		w0Spinner->set_alignment( GLUI_ALIGN_LEFT );
		/**** End Spinners ****/

		controlWindow->add_column_to_panel(control_panel, true);

		/*** Add rollouts (checkboxes) ***/
		GLUI_Rollout *terms = new GLUI_Rollout(control_panel, "Terms", true );
		new GLUI_Checkbox( terms, "Diffraction", &show_diffraction );
		new GLUI_Checkbox( terms, "Dispersion", &show_dispersion );
		new GLUI_Checkbox( terms, "Self-Phase Modulation", &show_spm );
		new GLUI_Checkbox( terms, "Self-Steepening", &show_ss );
		new GLUI_Checkbox( terms, "Multi-Photon Ionization", &show_mpi );
		new GLUI_Checkbox( terms, "Plasma/Ionization", &show_plasma );
		new GLUI_Checkbox( terms, "Focussing Lens", &show_lens );
		/*** End rollouts (checkboxes) ***/

		controlWindow->add_column_to_panel(control_panel, true);

		/*** Add rollouts (checkboxes) ***/
		GLUI_Rollout *numerical_panel = new GLUI_Rollout(control_panel, "Numerical Parameters", false );
		GLUI_Spinner *timeSpinner = new GLUI_Spinner( numerical_panel, "T Points:", &tDomain);
		timeSpinner->set_int_limits( 2, 5120 );
		timeSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *rSpinner = new GLUI_Spinner( numerical_panel, "R Points:", &rDomain);
		rSpinner->set_int_limits( 0, 5120 );
		rSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *xSpinner = new GLUI_Spinner( numerical_panel, "X Points:", &xDomain);
		xSpinner->set_int_limits( 0, 5120 );
		xSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *ySpinner = new GLUI_Spinner( numerical_panel, "Y Points:", &yDomain);
		ySpinner->set_int_limits( 0, 5120 );
		ySpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *zstepSpinner = new GLUI_Spinner( numerical_panel, "Z Points:", &zDomain);
		zstepSpinner->set_int_limits( 1, 10000000 );
		zstepSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *distanceSpinner = new GLUI_Spinner( numerical_panel, "Distance(cm):", &propDistance);
		distanceSpinner->set_int_limits( 1, 1000000 );
		distanceSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *rmaxSpinner = new GLUI_Spinner( numerical_panel, "R Max(mm):", &inrmax);
		rmaxSpinner->set_int_limits( 1, 1000000 );
		rmaxSpinner->set_alignment( GLUI_ALIGN_LEFT );
		GLUI_Spinner *tmaxSpinner = new GLUI_Spinner( numerical_panel, "T Max(fs):", &intmax);
		tmaxSpinner->set_int_limits( 1, 1000000 );
		tmaxSpinner->set_alignment( GLUI_ALIGN_LEFT );
		/*** End rollouts (checkboxes) ***/

		controlWindow->add_column_to_panel(control_panel, true);

		/*** Add rollouts (checkboxes) ***/
		GLUI_Rollout *features_panel = new GLUI_Rollout(control_panel, "Program Features", true );
		new GLUI_Checkbox( features_panel, "Cylindrical Coordinates", &show_cyl );
		new GLUI_Checkbox( features_panel, "Cartesian Coordinates", &show_cart );
		new GLUI_Checkbox( features_panel, "Input Power Factor", &input_pf );
		new GLUI_Checkbox( features_panel, "Input Energy", &input_energy );
		new GLUI_Checkbox( features_panel, "Print Spatial Domain", &print_spatDom );
		new GLUI_Checkbox( features_panel, "Print Plasma Domain", &print_plasDom );
		new GLUI_Checkbox( features_panel, "Print Spatial Video", &print_spatVid );
		new GLUI_Checkbox( features_panel, "Print Plasma Video", &print_plasVid );
		new GLUI_Checkbox( features_panel, "Print Plasma Decay", &print_plasSim );
		/*** End rollouts (checkboxes) ***/

		controlWindow->add_column_to_panel(control_panel, true);

		/*** Add rollouts (checkboxes) ***/
		GLUI_Rollout *windows_panel = new GLUI_Rollout(control_panel, "Window Selection", true );
		new GLUI_Checkbox( windows_panel, "Spatial Intensity", &show_spatial );
		new GLUI_Checkbox( windows_panel, "Temporal Intensity", &show_temporal );
		new GLUI_Checkbox( windows_panel, "Spectral Distance", &show_spectral );
		new GLUI_Checkbox( windows_panel, "Max Intensity", &show_max );
		new GLUI_Checkbox( windows_panel, "Beam Waist", &show_beam );
		new GLUI_Checkbox( windows_panel, "Space-Time Intensity", &show_space );
		/*** End rollouts (checkboxes) ***/

		controlWindow->add_column_to_panel(control_panel, true);

		/*** Add rollouts (checkboxes) ***/
		GLUI_Rollout *plasma_panel = new GLUI_Rollout(control_panel, "Plasma Decay Simulation", true );
		GLUI_Spinner *plasmaxSpinner = new GLUI_Spinner( plasma_panel, "Duration(ns):", &tPlasMax);
		plasmaxSpinner->set_float_limits( 2, 5120 );
		GLUI_Spinner *plasdomainSpinner = new GLUI_Spinner( plasma_panel, "T Points:", &tPlasDomain);
		plasdomainSpinner->set_int_limits( 2, 5120 );
		GLUI_Spinner *decaySpinner = new GLUI_Spinner( plasma_panel, "Decay Constant(e-9):", &tDecay);
		decaySpinner->set_float_limits( 2, 5120 );
		/*** End rollouts (checkboxes) ***/

		new GLUI_Button( controlWindow, "Initiate", INITIALIZE, control_cb );

		/***********************************************************************/
		/*                     And The Rest....                                */
		/***********************************************************************/
		printf( "USPL Version: %3.1f\n", version );
		printf("rDomain:%d, tDomain:%d\n", rDomain, tDomain);

		glutMainLoop();
		return EXIT_SUCCESS;
		break;
	}
}
