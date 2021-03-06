//
//  classes.cpp
//  importOBJ
//
//  Created by Noah Wolfe on 5/8/14.
//  Copyright (c) 2014 Noah Wolfe. All rights reserved.
//

#include "classes.h"
//#include <iostream>

//--------vertex Class----------//
vertex::vertex()
{
    p[0] = 0;
    p[1] = 0;
    p[2] = 0;
}
vertex::vertex(float p1, float p2, float p3)
{
    p[0] = p1;
    p[1] = p2;
    p[2] = p3;
}
vertex::vertex(vertex const &v)
{
    p[0] = v.p[0];
    p[1] = v.p[1];
    p[2] = v.p[2];
}
void vertex::print()
{
    std::cout << p[0] << " " << p[1] << " " << p[2] << std::endl;
}
float vertex::getX()
{
	return p[0];
}
float vertex::getY()
{
	return p[1];
}
float vertex::getZ()
{
	return p[2];
}
//--------face Class----------//
face::face()
{
    v[0] = *new vertex(0,0,0);
    v[1] = *new vertex(0,0,0);
    v[2] = *new vertex(0,0,0);
    vN[0] = *new vertex(0,0,0);
    vN[1] = *new vertex(0,0,0);
    vN[2] = *new vertex(0,0,0);
}
face::face(vertex v1, vertex v2, vertex v3, vertex vN1, vertex vN2, vertex vN3)
{
    v[0] = v1;
    v[1] = v2;
    v[2] = v3;
    vN[0] = vN1;
    vN[1] = vN2;
    vN[2] = vN3;
}
face::face(face const &f)
{
    v[0] = f.v[0];
    v[1] = f.v[1];
    v[2] = f.v[2];
    vN[0] = f.vN[0];
    vN[1] = f.vN[1];
    vN[2] = f.vN[2];

}
void face::printVertex()
{
    std::cout << "  vertex1: ";
    v[0].print();
    std::cout << "  vertex2: ";
    v[1].print();
    std::cout << "  vertex3: ";
    v[2].print();
}
void face::printVertexNormal()
{
    std::cout << "  vertexNormal1: ";
    vN[0].print();
    std::cout << "  vertexNormal2: ";
    vN[1].print();
    std::cout << "  vertexNormal3: ";
    vN[2].print();
}
vertex* face::getNormals()
{
	return vN;
}
vertex* face::getVertices()
{
	return v;
}
//--------meshOBJ Class----------//
meshOBJ::meshOBJ()
{
//	faces[0] = *new face();
	filePath = "not initialized";
}
meshOBJ::meshOBJ(std::string path)
{
    filePath = path;
}
void meshOBJ::setPath(std::string path)
{
	filePath = path;
}
void meshOBJ::addFace(face newFace)
{
    faces.push_back(newFace);
}
unsigned long meshOBJ::getSize()
{
    return faces.size();
}
void meshOBJ::print()
{
	std::cout << "size: " << faces.size() << std::endl;
    for (unsigned int i=0;i<faces.size();i++)
    {
        std::cout << "Face" << i << ": ";
        faces.at(i).printVertex();
        faces.at(i).printVertexNormal();
    }
}
void meshOBJ::load()
{
    std::string dataLine, first;
    std::vector<vertex> v1,n1;
    float tp1,tp2,tp3,tn1,tn2,tn3;
    int f1,f2,f3;
    std::ifstream myFile;
    myFile.open(filePath.c_str());
	if (!myFile.is_open()) 
	{
		 std::cout << "Error opening file " << filePath << std::endl;
		 valid = 0;
  	}
	else
	{
		valid = 1;
	}
    
    while(getline(myFile,dataLine))
    {
        std::istringstream iss(dataLine);
        iss >> first;
        //Check for vertex
        if(first == "v")
        {
            iss >> tp1 >> tp2 >> tp3;
            v1.push_back(*new vertex(tp1,tp2,tp3));
        }
        //Check for vertex normal
        if(first == "vn")
        {
            iss >> tn1 >> tn2 >> tn3;
            n1.push_back(*new vertex(tn1,tn2,tn3));
        }
        //check for faces
        if(first == "f")
        {
            iss >> f1;
            iss.ignore(256,' ');
            iss >> f2;
            iss.ignore(256,' ');
            iss >> f3;
            this->addFace(*new face(v1.at(f1-1),v1.at(f2-1),v1.at(f3-1),n1.at(f1-1),n1.at(f2-1),n1.at(f3-1)));
        }
    }
	std::cout << "finished loading " << filePath << std::endl;
}
int meshOBJ::isValidated()
{
	return valid;
}
vertex* meshOBJ::getNormals(int faceIndex)
{
	return faces.at(faceIndex).getNormals();
}
vertex* meshOBJ::getVertices(int faceIndex)
{
	return faces.at(faceIndex).getVertices();
}
//--------particle Class----------//
particle::particle()
{
//	faces[0] = *new face();
	filePath = "not initialized";
}
particle::particle(std::string path)
{
    filePath = path;
}
void particle::setPath(std::string path)
{
	filePath = path;
}
unsigned long particle::getSize()
{
    return position.size();
}
/*void particle::print()
{
	std::cout << "size: " << faces.size() << std::endl;
    for (unsigned int i=0;i<faces.size();i++)
    {
        std::cout << "Face" << i << ": ";
        faces.at(i).printVertex();
        faces.at(i).printVertexNormal();
    }
}
*/void particle::load()
{
    std::string dataLine, first;
    std::vector<vertex> v1,n1;
    float tp1,tp2,tp3;
    std::ifstream myFile;
    myFile.open(filePath.c_str());
	if (!myFile.is_open()) 
	{
		 std::cout << "Error opening file " << filePath << std::endl;
  	}
    
    while(getline(myFile,dataLine))
    {
        std::istringstream iss(dataLine);
        iss >> tp1 >> tp2 >> tp3;
		position.push_back(*new vertex(tp1,tp2,tp3));
    }
	std::cout << "finished loading " << filePath << std::endl;
}
vertex particle::getPosition(int indx)
{
	return position.at(indx);
}
