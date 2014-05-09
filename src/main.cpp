//
//  main.cpp
//  importOBJ
//
//  Created by Noah Wolfe on 5/8/14.
//  Copyright (c) 2014 Noah Wolfe. All rights reserved.
//

#include "classes.h"

using namespace std;

//These variables will need to be passed into the function

int main(int argc, const char * argv[])
{
    meshOBJ heartWall("/home/noah/Documents/ARCHER/ARCHER-Visual/data/73_adult_male/073kg/087.obj");  //Instantiate Heart Wall mesh object giving file path
    heartWall.loadOBJ();                                            //load object data from file
//    heartWall.print();                                              //printf the data to the screen
    return 0;
}

