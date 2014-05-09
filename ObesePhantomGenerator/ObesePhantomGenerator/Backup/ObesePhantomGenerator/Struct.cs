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
    // declare a struct to record the coordinate of vertices, vertexnomals and faces
    public struct Coordinate
    {
        public double x, y, z;
    }

}// end namespace