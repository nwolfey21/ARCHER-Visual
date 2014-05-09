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
    static class Interpolation
    {
        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // -------------------------------------------------------------------------
        //                    same height, different BMI
        // -------------------------------------------------------------------------
        // input: bullet and hole mesh, desired BMI, tolerance epsilon
        // output: new mesh
        // external functions: CalculateMeshVolume
        public static List<List<Coordinate>> SameHeight_DifferentBMI
            (List<List<Coordinate>> bulletMesh,
            List<List<Coordinate>> holeMesh,
            double desiredVolume, double epsilon)
        {
            // initialize variables
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            List<Coordinate> vertexNormals = new List<Coordinate>();
            double optimumFactor = 0;
            double newVolume = 0;
            double minOptimumFactor = 0; // corresponding to bulletMesh
            double maxOptimumFactor = 1; // corresponding to holeMesh

            // first guess of the optimumFactor
            optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;

            // update the coordinates of new vertices resulting from the optimumFactor
            newMesh.Add(UpdateVertices.SameH_DifferentBMI(bulletMesh[0], holeMesh[0], optimumFactor));

            // calculate the volume of new mesh
            newVolume = CalculateMesh.CalculateVolume(newMesh[0], bulletMesh[1]);

            // binary approximation method
            while (Math.Abs(newVolume - desiredVolume) > epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (newVolume < desiredVolume)
                {
                    maxOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }
                else if (newVolume > desiredVolume)
                {
                    minOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }

                // clear newMesh
                newMesh.Clear();

                // update the coordinates of new vertices corresponding to the optimumFactor
                newMesh.Add(UpdateVertices.SameH_DifferentBMI(bulletMesh[0], holeMesh[0], optimumFactor));

                // calculate the volume of new mesh
                newVolume = CalculateMesh.CalculateVolume(newMesh[0], bulletMesh[1]);

            }// end while

            // add face information, which is the same with bullet and hole mesh
            newMesh.Add(bulletMesh[1]);

            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], newMesh[1]);

            // add vertex normals
            newMesh.Add(vertexNormals);

            // display statistics
            /*MessageBox.Show("same heigh, different BMI interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;
        } // end method

        // -------------------------------------------------------------------------
        //                    same BMI, different height
        // -------------------------------------------------------------------------
        // input: bullet and hole mesh, desired BMI, tolerance epsilon
        // output: new mesh
        // external functions: CalculateMeshVolume
        public static List<List<Coordinate>> SameBMI_DifferentHeight
            (List<List<Coordinate>> mesh, double desiredVolume, double heightScale, double epsilon)
        {
            // initialize variables
            List<Coordinate> vertexNormals = new List<Coordinate>();
            List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
            double optimumFactor = 0;
            double newVolume = 0;
            double minOptimumFactor = 0; // 0: shrinking to a singularity, 1: same size
            double maxOptimumFactor = 1e10;
            
            // first guess of the optimumFactor
            optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;

            // add new vertices
            newMesh.Add(UpdateVertices.ProportionalScaler(mesh[0], optimumFactor, heightScale, optimumFactor));

            // calculate the volume of new mesh
            newVolume = CalculateMesh.CalculateVolume(newMesh[0], mesh[1]);

            // binary approximation method
            while (Math.Abs(newVolume - desiredVolume) > epsilon)
            {
                //MessageBox.Show("delta = "+Math.Abs(newVolume - desiredVolume).ToString());
                if (newVolume < desiredVolume)
                {
                    minOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }
                else if (newVolume > desiredVolume)
                {
                    maxOptimumFactor = optimumFactor;
                    optimumFactor = (minOptimumFactor + maxOptimumFactor) / 2.0;
                }

                // clear mesh
                newMesh.Clear();

                // add new vertices
                newMesh.Add(UpdateVertices.ProportionalScaler(mesh[0], optimumFactor, heightScale, optimumFactor));

                // calculate the volume of new mesh
                newVolume = CalculateMesh.CalculateVolume(newMesh[0], mesh[1]);

            }// end while

            // add faces
            newMesh.Add(mesh[1]);

            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], mesh[1]);

            // add vertex normals
            newMesh.Add(vertexNormals);

            // display statistics
            /*MessageBox.Show("same BMI, different height interpolation:"
                + "\n\n" + "optimum factor = " + optimumFactor.ToString()
                + "\n" + "volume of the new mesh = " + newVolume.ToString());*/

            return newMesh;
        } // end method



    } // end class
}// end namespace