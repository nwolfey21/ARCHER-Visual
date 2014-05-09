using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization; // for use of Double.Parse(String, NumberStyles)
using System.IO;

namespace ObesePhantomGenerator
{
    static class CalculateMesh
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // ------------------------------------------------
        //                    calculate mesh volume
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: volume
        // external functions: none
        public static double CalculateVolume( List<Coordinate>vertices, List<Coordinate>faces)
        {
            double volume = 0;
            double determinant = 0;
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            double p11;
            double p12;
            double p13;
            double p21;
            double p22;
            double p23;
            double p31;
            double p32;
            double p33;

            for (int i = 0; i < faces.Count; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                // get the determinant
                p11 = point2.x - point1.x;
                p12 = point2.y - point1.y;
                p13 = point2.z - point1.z;
                p21 = point3.x - point2.x;
                p22 = point3.y - point2.y;
                p23 = point3.z - point2.z;
                p31 = -point3.x;
                p32 = -point3.y;
                p33 = -point3.z;

                determinant = p11 * (p22 * p33 - p23 * p32)
                    - p12 * (p21 * p33 - p23 * p31)
                    + p13 * (p21 * p32 - p22 * p31);

                // update the volume
                volume = volume + 1.0 / 6.0 * determinant;
            }// end for

            volume = Math.Abs(volume);
            return volume;
        }// end class

        // ------------------------------------------------
        //                    calculate face normals
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: face normals
        // external functions: none
        public static List<Coordinate> CalculateFaceNormals(List<Coordinate> vertices, List<Coordinate> faces)
        {
            List<Coordinate> faceNormals = new List<Coordinate>();
            int point1_index = 0;
            int point2_index = 0;
            int point3_index = 0;
            Coordinate point1 = new Coordinate();
            Coordinate point2 = new Coordinate();
            Coordinate point3 = new Coordinate();
            Coordinate crossProduct = new Coordinate();
            double p11;
            double p12;
            double p13;
            double p21;
            double p22;
            double p23;
            double norm;

            for (int i = 0; i < faces.Count; i++)
            {
                // get the coordinates (the vertices number) of a certain face
                point1_index = Convert.ToInt32(faces[i].x);
                point2_index = Convert.ToInt32(faces[i].y);
                point3_index = Convert.ToInt32(faces[i].z);

                // get the coordinates of each vertex belonging to that face
                // vertex 1
                point1.x = vertices[point1_index - 1].x;
                point1.y = vertices[point1_index - 1].y;
                point1.z = vertices[point1_index - 1].z;

                // vertex 2
                point2.x = vertices[point2_index - 1].x;
                point2.y = vertices[point2_index - 1].y;
                point2.z = vertices[point2_index - 1].z;

                // vertex 3
                point3.x = vertices[point3_index - 1].x;
                point3.y = vertices[point3_index - 1].y;
                point3.z = vertices[point3_index - 1].z;

                // get the cross product
                p11 = point2.x - point1.x;
                p12 = point2.y - point1.y;
                p13 = point2.z - point1.z;
                p21 = point3.x - point2.x;
                p22 = point3.y - point2.y;
                p23 = point3.z - point2.z;

                crossProduct.x = p12 * p23 - p13 * p22;
                crossProduct.y = -(p11 * p23 - p13 * p21);
                crossProduct.z = p11 * p22 - p12 * p21;

                //normalize
                norm = Math.Sqrt(crossProduct.x * crossProduct.x
                    + crossProduct.y * crossProduct.y
                    + crossProduct.z * crossProduct.z);
                crossProduct.x = crossProduct.x / norm;
                crossProduct.y = crossProduct.y / norm;
                crossProduct.z = crossProduct.z / norm;

                faceNormals.Add(crossProduct);
            }

            return faceNormals;
        }//end class

        // ------------------------------------------------
        //                    calculate vertex normals
        // ------------------------------------------------
        // input: vertices and faces of the mesh
        // output: vertex normals
        // external functions: CalculateFaceNormals
        public static List<Coordinate> CalculateVertexNormals(List<Coordinate> vertices, List<Coordinate> faces)
        {
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<Coordinate> faceNormals = new List<Coordinate>();
            Coordinate point = new Coordinate();
            Coordinate temp = new Coordinate();
            double norm;

            // calculate face normals
            faceNormals.AddRange(CalculateFaceNormals(vertices,faces));

            // initialize vertex normals
            point.x = 0;
            point.y = 0;
            point.z = 0;
            for (int i=0; i<vertices.Count;i++)
            {
                vertexNormals.Add(point);
            }

            for ( int i = 0; i < faces.Count; i++)
            {
                // pay attention to the way of parameter passing!
                // vertex 1
                // faces[i].x: index of the 1st vetex of the ith face
                temp = vertexNormals[Convert.ToInt32(faces[i].x) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].x) - 1] = temp;

                // vertex 2
                temp = vertexNormals[Convert.ToInt32(faces[i].y) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].y) - 1] = temp;

                // vertex 3
                temp = vertexNormals[Convert.ToInt32(faces[i].z) - 1];
                temp.x = temp.x + faceNormals[i].x;
                temp.y = temp.y + faceNormals[i].y;
                temp.z = temp.z + faceNormals[i].z;
                vertexNormals[Convert.ToInt32(faces[i].z) - 1] = temp;
            }

            //normalize
            for (int i = 0; i < vertices.Count; i++)
            {
                temp = vertexNormals[i];
                norm = Math.Sqrt( temp.x * temp.x + temp.y * temp.y + temp.z * temp.z);
                temp.x = temp.x / norm;
                temp.y = temp.y / norm;
                temp.z = temp.z / norm;
                vertexNormals[i] = temp;
            }

            return vertexNormals;
        }//end class

    }// end class
}// end namespace