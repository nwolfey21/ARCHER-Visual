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
    static class Test
    {
        
        public static double method_2(double a, double b)
        {
            double sum;
            sum = method_1(a, b) + a + b;            
            return sum;
        }

        public static double method_1(double a, double b)
        {
            double sum;
            sum = a + b;
            return sum;
        }
    }
}// end namespace