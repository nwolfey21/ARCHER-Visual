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
using System.Diagnostics;

namespace ObesePhantomGenerator
{
    // main form
    public partial class FormObesePhantomGenerator : Form
    {
        // +++++++++++++++++++++++++++++++++++ variables ++++++++++++++++++++++++++++++++++++++++
        bool isInvalidCharacter = false;
        bool tooManyDecimalPeriod = false;
        bool tooManyDecimalDigit = false;
        bool textValid = false;
        double maxBMI = 46.00;
        double minBMI = 38.00;
        double maxHeight = 1.88;
        double minHeight = 1.65;
        double maxWeight = 0;
        double minWeight = 0;
        string completedText = null;
        const double standardHeight = 1.76; // [m]
        const double standardDensity = 1.07 * 1000.0; // [kg/m^3]
        const double epsilon = 1e-6;

        List<List<Coordinate>> holeMesh = new List<List<Coordinate>>();
        List<List<Coordinate>> bulletMesh = new List<List<Coordinate>>();
        List<List<Coordinate>> newMesh = new List<List<Coordinate>>();

        // +++++++++++++++++++++++++++++++++++ constructor ++++++++++++++++++++++++++++++++++++++++
        public FormObesePhantomGenerator()
        {
            InitializeComponent();

            // variables
            maxWeight = maxBMI * maxHeight * maxHeight;
            minWeight = minBMI * minHeight * minHeight;

            // form
            this.StartPosition = FormStartPosition.CenterScreen;

            // initialize trackBarBMI
            trackBarBMI.Maximum = Convert.ToInt32(maxBMI * 100);
            trackBarBMI.Minimum = Convert.ToInt32(minBMI * 100);
            trackBarBMI.LargeChange = 100;
            trackBarBMI.SmallChange = 1;
            trackBarBMI.TickFrequency = 100;
            //trackBarBMI.TickStyle = TickStyle.Both;
            // initialize textBoxBMI
            textBoxBMI.Text = minBMI.ToString();

            // initialize trackBarHeight
            trackBarHeight.Maximum = Convert.ToInt32(maxHeight * 100);
            trackBarHeight.Minimum = Convert.ToInt32(minHeight * 100);
            trackBarHeight.LargeChange = 2;
            trackBarHeight.SmallChange = 1;
            trackBarHeight.TickFrequency = 2;
            //trackBarHeight.TickStyle = TickStyle.Both;
            // initialize textBoxHeight
            textBoxHeight.Text = minHeight.ToString();

            // initialize trackBarWeight
            trackBarWeight.Maximum = Convert.ToInt32(maxWeight * 100);
            trackBarWeight.Minimum = Convert.ToInt32(minWeight * 100);
            trackBarWeight.LargeChange = 500;
            trackBarWeight.SmallChange = 50;
            trackBarWeight.TickFrequency = 500;
            //trackBarWeight.TickStyle = TickStyle.Both;
            trackBarWeight.Enabled = false;
            // initialize textBoxWeight
            textBoxWeight.Text = minWeight.ToString();
            textBoxWeight.Enabled = false;

            // initialize button
            buttonGenerate.Enabled = false;
            buttonExportObj.Enabled = false;

            // initialize labels
            labelMinBMI.Text = string.Format("{0:##}", minBMI);
            labelMaxBMI.Text = string.Format("{0:##}", maxBMI);
            labelMinHeight.Text = string.Format("{0:#.##}",minHeight);
            labelMaxHeight.Text = string.Format("{0:#.##}", maxHeight);
            labelMinWeight.Text = string.Format("{0:###.##}", minWeight);
            labelMinWeight.Enabled = false;
            labelMaxWeight.Text = string.Format("{0:###.##}", maxWeight);
            labelMaxWeight.Enabled = false;

            // tabpage
            // note: System.Drawing.Color.Transparent and System.Drawing.SystemColors.Control
            tabPageRegular.BackColor = SystemColors.Control;
            tabPageFree.BackColor = SystemColors.Control;

        }

        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        // ------------------------------------------------
        //                    buttons
        // ------------------------------------------------
        private void buttonReset_Click(object sender, EventArgs e)
        {
            trackBarBMI.Value = trackBarBMI.Minimum;
            trackBarHeight.Value = trackBarHeight.Minimum;
            trackBarWeight.Value = trackBarWeight.Minimum;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ------------------------------------------------
        //                    trackbars
        // ------------------------------------------------
        // for BMI, height and weight trackbars
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar sd = (TrackBar)sender;

            // get current BMI & height, and calculate the corresponding weight
            double BMI = Convert.ToDouble(trackBarBMI.Value) / 100.00;
            double height = Convert.ToDouble(trackBarHeight.Value) / 100.00;
            double weight = BMI * height * height;
            trackBarWeight.Value = Convert.ToInt32(weight * 100);

            if (sd.Name == "trackBarBMI")
            {
                // update the BMI textbox
                textBoxBMI.Text = BMI.ToString();
                // update the weight trackbar
                trackBarWeight.Value = Convert.ToInt32(weight * 100);
            }
            else if (sd.Name == "trackBarHeight")
            {
                textBoxHeight.Text = height.ToString();
                trackBarWeight.Value = Convert.ToInt32(weight * 100);
            }
            else if (sd.Name == "trackBarWeight")
            {
                textBoxWeight.Text = weight.ToString();
            }
        }

        // ------------------------------------------------
        //                    textboxes
        // ------------------------------------------------
        // for BMI, height textboxes
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox sd = (TextBox)sender;

            // get and then set the pre-specified min and max value
            double minValue = 0;
            double maxValue = 0;
            if (sd.Name == "textBoxBMI")
            {
                minValue = minBMI;
                maxValue = maxBMI;
            }
            else if (sd.Name == "textBoxHeight")
            {
                minValue = minHeight;
                maxValue = maxHeight;
            }

            // check if text is non-empty
            if (sd.Text != "")
            {
                completedText = sd.Text;

                // convert .XX to 0.XX
                if (sd.Text[0] == '.')
                {
                    completedText = '0' + sd.Text;
                }

                // check if the number entered is valid
                double enteredValue = Convert.ToDouble(completedText);
                textValid = (enteredValue >= minValue) &&
                            (enteredValue <= maxValue);


                // update the trackbars
                if (textValid == true)
                {
                    if (sd.Name == "textBoxBMI")
                    {
                        trackBarBMI.Value = Convert.ToInt32(enteredValue * 100);
                    }
                    else if (sd.Name == "textBoxHeight")
                    {
                        trackBarHeight.Value = Convert.ToInt32(enteredValue * 100);
                    }// end if
                }// end if
            }// end if
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox sd = (TextBox)sender; // conversion of obj type into textbox type, why it works?
            isInvalidCharacter = false;
            tooManyDecimalPeriod = false;
            tooManyDecimalDigit = false;

            // always allow entering of backspace
            if (e.KeyCode != Keys.Back)
            {
                // check if a non-number is entered
                isInvalidCharacter = (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) &&
                                     (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) &&
                                     (e.KeyCode != Keys.OemPeriod) ||
                                     Control.ModifierKeys == Keys.Shift;

                // check if there is already one decimal point
                if (sd.Text.Contains('.') == true)
                {
                    // check if one more decimal point is entered
                    tooManyDecimalPeriod = e.KeyCode == Keys.OemPeriod;

                    int decimalPointPosition = sd.Text.IndexOf('.');
                    // check if the current cursor is at the decimal part
                    if (sd.SelectionStart > decimalPointPosition)
                    {
                        // check if there has already been 2 decimal digits
                        // i.e check if the cursor is at the position of the 3rd decimal digit
                        tooManyDecimalDigit = decimalPointPosition == (sd.Text.Length - 3);
                    }
                }
            }

            isInvalidCharacter = isInvalidCharacter || tooManyDecimalPeriod || tooManyDecimalDigit;
        }

        // when keypress event occurs, this associated method will be used
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // prevent non-number characters being entered
            if (isInvalidCharacter == true)
            {
                //If the event is not handled, it will be sent to the operating system for default processing. Set Handled to true to cancel the KeyPress event.
                e.Handled = true;
            }
            //MessageBox.Show(e.Handled.ToString());

        }

        // ------------------------------------------------
        //                    buttons
        // ------------------------------------------------
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // disable the generate button
            buttonGenerate.Enabled = false;

            // variables
            bool validInput = false;
            // check if values in the textbox and trackbar are the same
            validInput = Convert.ToDouble(textBoxBMI.Text) == Convert.ToDouble(trackBarBMI.Value) / 100.0
                && Convert.ToDouble(textBoxHeight.Text) == Convert.ToDouble(trackBarHeight.Value) / 100.0;
                //&& Convert.ToDouble(textBoxWeight.Text) == Convert.ToDouble(trackBarWeight.Value) / 100.0;
                // note: the textbox and trackbar of weight cannot be the same whether the trackbar value is devided by 100 or 10000. they must be rounded.

            if (validInput == true)
            {

                // get desiredVolume derived from specified BMI and standard height
                double desiredVolume = 1.0e6 * standardHeight * standardHeight / standardDensity
                    * Convert.ToDouble(trackBarBMI.Value) / 100.00;

                // first, perform same-height-different-BMI interpolation
                newMesh = Interpolation.SameHeight_DifferentBMI(bulletMesh, holeMesh, desiredVolume, epsilon);


                // get desiredVolume derived from specified BMI and specified height
                double currentHeight = Convert.ToDouble(trackBarHeight.Value)/100.00;
                desiredVolume = 1.0e6 * currentHeight*currentHeight / standardDensity
                    * Convert.ToDouble(trackBarBMI.Value) / 100.00;

                // get the ratio of height
                double heightScale = currentHeight / standardHeight;

                // second, perform same-BMI-different-height interpolation
                newMesh = Interpolation.SameBMI_DifferentHeight(newMesh, desiredVolume, heightScale, epsilon);

                stopwatch.Stop();

                TimeSpan ts = stopwatch.Elapsed;

                MessageBox.Show(ts.TotalSeconds.ToString());
                
                // enable the export button
                buttonExportObj.Enabled = true;
            }
            else
            {
                string exceptionReport = "Invalid input parameters";
                MessageBox.Show(exceptionReport);
            }

        }

        private void buttonImportObj_Click(object sender, EventArgs e)
        {
            bulletMesh = FileIO.ImportSampleObj(@"D:\study\deformation\Obese Phantom Interpolation\task3\Skin142kg.obj");
            holeMesh = FileIO.ImportSampleObj(@"D:\study\deformation\Obese Phantom Interpolation\task3\dismembered_hole_Skin142kg_Skin117kg.obj");
            buttonImportObj.Enabled = false;
            buttonGenerate.Enabled = true;
        }// end buttonImportObj_Click

        private void buttonExportObj_Click(object sender, EventArgs e)
        {
            FileIO.ExportObj(newMesh);
        }

        private void buttonFreelyScale_Click(object sender, EventArgs e)
        {
            List<Coordinate> vertexNormals = new List<Coordinate>();
            double xScaleFactor = Convert.ToDouble(textBoxXScaleFactor.Text);
            double yScaleFactor = Convert.ToDouble(textBoxYScaleFactor.Text);
            double zScaleFactor = Convert.ToDouble(textBoxZScaleFactor.Text);
            
            // import bulletMesh
            bulletMesh = FileIO.ImportSampleObj(@"D:\study\deformation\Obese Phantom Interpolation\task3\Skin142kg.obj");
            
            // scale the vertices
            newMesh.Add(UpdateVertices.ProportionalScaler(bulletMesh[0], xScaleFactor, yScaleFactor, zScaleFactor));
            
            // add the faces
            newMesh.Add(bulletMesh[1]);
            
            // calculate vertex normals
            vertexNormals = CalculateMesh.CalculateVertexNormals(newMesh[0], bulletMesh[1]);

            // add vertex normals
            newMesh.Add(vertexNormals);
        }

        private void buttonImportObj2_Click(object sender, EventArgs e)
        {
            bulletMesh = FileIO.ImportSampleObj(@"D:\study\deformation\Obese Phantom Interpolation\task3\Skin142kg.obj");
        }


    }// end form
}// end namespace
