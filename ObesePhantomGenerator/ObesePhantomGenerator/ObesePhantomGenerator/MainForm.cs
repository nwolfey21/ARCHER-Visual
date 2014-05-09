using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using System.Globalization; // for use of Double.Parse(String, NumberStyles)
using System.IO;
using System.Diagnostics;
// solution explorer->references->add reference->System.Windows.Forms.DataVisualization
// all the methods are non-static!!
using System.Windows.Forms.DataVisualization.Charting;


namespace ObesePhantomGenerator
{
    // main form
    public partial class FormObesePhantomGenerator : Form
    {
        // +++++++++++++++++++++++++++++++++++ variables ++++++++++++++++++++++++++++++++++++++++
        bool isInvalidCharacter = false;
        double maxBMI = 0;
        double minBMI = 0;
        double maxHeight = 1.859;
        double minHeight = 1.668;
        double maxWeight = 0;
        double minWeight = 0;
        double desiredBMI = 0;
        double desiredHeight = 0;
        double desiredWeight = 0;

        List<EachMeshInfo> phantomInfo = new List<EachMeshInfo>();
        List<List<Coordinate>> newMesh = new List<List<Coordinate>>();
        List<List<Coordinate>> oldMesh = new List<List<Coordinate>>();

        //import holeMesh and bulletMesh
        List<List<Coordinate>> holeMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\Skin_117kg.obj");
        List<List<Coordinate>> bulletMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\Skin_142kg.obj");

        // +++++++++++++++++++++++++++++++++++ methods ++++++++++++++++++++++++++++++++++++++++
        public FormObesePhantomGenerator()
        {
            InitializeComponent();
        }

        private void FormObesePhantomGenerator_Load(object sender, EventArgs e)
        {
            #region calculate the weight of holeMesh and bulletMesh
            //variables
            double weight_holeMesh = 0;
            double weight_bulletMesh = 0;
            double volume_holeMesh = 0;
            double volume_bulletMesh = 0;
            double aSkin_holeMesh = 0;
            double aSkin_bulletMesh = 0;
            double vSAT_holeMesh = 0;
            double vSAT_bulletMesh = 0;
            double vVAT_holeMesh = 0;
            double vVAT_bulletMesh = 0;
            double v150_standard = 0; //SAT
            double v151_standard = 0; //VAT
            double v72_standard = 0; //stomach wall
            double v73_standard = 0; //stomach content
            double v95_standard = 0; //liver
            double v76_86_standard = 0; //large intestine
            double v75_standard = 0; //small intestine
            double vResidualTissue_holeMesh = 0;
            double vResidualTissue_bulletMesh = 0;

            //calculate holeMesh weight
            //import phantom infor
            phantomInfo = FileIO.GetPhantomInfo();

            //import SAT and VAT mesh
            List<List<Coordinate>> SATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\150.obj");
            List<List<Coordinate>> VATMesh = FileIO.ImportObjWithoutDialog(Application.StartupPath + @"\Input\151.obj");

            volume_holeMesh = CalculateMesh.CalculateVolume(holeMesh[0], holeMesh[1]); //skin
            volume_bulletMesh = CalculateMesh.CalculateVolume(bulletMesh[0], bulletMesh[1]); //skin
            v150_standard = CalculateMesh.CalculateVolume(SATMesh[0], SATMesh[1]); //SAT
            v151_standard = CalculateMesh.CalculateVolume(VATMesh[0], VATMesh[1]); //VAT
            v72_standard = FileIO.GetMeshInfo(phantomInfo, 72, 72, "volume"); //stomach wall
            v73_standard = FileIO.GetMeshInfo(phantomInfo, 73, 73, "volume"); //stomach content
            v95_standard = FileIO.GetMeshInfo(phantomInfo, 95, 95, "volume"); //liver
            v76_86_standard = FileIO.GetMeshInfo(phantomInfo, 76, 86, "volume"); //large intestine
            v75_standard = FileIO.GetMeshInfo(phantomInfo, 75, 75, "volume"); //small intestine
            vSAT_holeMesh = 0.8 * (volume_holeMesh - v150_standard);
            vSAT_bulletMesh = 0.8 * (volume_bulletMesh - v150_standard);
            vVAT_holeMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95_standard - v76_86_standard - v75_standard;
            vVAT_bulletMesh = v151_standard - 0.8 * v72_standard - v73_standard - v95_standard - v76_86_standard - v75_standard;
            vResidualTissue_holeMesh = volume_holeMesh - vSAT_holeMesh - vVAT_holeMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume");
            vResidualTissue_bulletMesh = volume_bulletMesh - vSAT_bulletMesh - vVAT_bulletMesh - FileIO.GetMeshInfo(phantomInfo, 1, 138, "volume");
            aSkin_holeMesh = CalculateMesh.CalculateArea(holeMesh[0], holeMesh[1]);
            aSkin_bulletMesh = CalculateMesh.CalculateArea(bulletMesh[0], bulletMesh[1]);

            weight_holeMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass")
                + GlobalConstant.densitySAT * vSAT_holeMesh
                + GlobalConstant.densityVAT * vVAT_holeMesh
                + GlobalConstant.densityResidualTissue * vResidualTissue_holeMesh
                + GlobalConstant.densitySkin * aSkin_holeMesh * GlobalConstant.thicknessSkin;
            weight_bulletMesh = FileIO.GetMeshInfo(phantomInfo, 1, 138, "mass")
                + GlobalConstant.densitySAT * vSAT_bulletMesh
                + GlobalConstant.densityVAT * vVAT_bulletMesh
                + GlobalConstant.densityResidualTissue * vResidualTissue_bulletMesh
                + GlobalConstant.densitySkin * aSkin_bulletMesh * GlobalConstant.thicknessSkin;
            weight_holeMesh = weight_holeMesh / 1000; //convert from g to kg
            weight_bulletMesh = weight_bulletMesh / 1000; //convert from g to kg

            #endregion

            //set min and max of BMI
            minBMI = weight_holeMesh / GlobalConstant.standardHeight / GlobalConstant.standardHeight; //lower limit needs to be ceiled up
            minBMI = Math.Ceiling(minBMI * 100.00) / 100;
            maxBMI = weight_bulletMesh / GlobalConstant.standardHeight / GlobalConstant.standardHeight; //upper limit needs to floored down
            maxBMI = Math.Floor(maxBMI * 100.00) / 100;

            //calculate min and max of weight
            minWeight = minBMI * minHeight * minHeight;
            maxWeight = maxBMI * maxHeight * maxHeight;

            //--------------------trackBar--------------------
            //BMI
            trackBarBMI.Maximum = Convert.ToInt32(maxBMI * 100);
            trackBarBMI.Minimum = Convert.ToInt32(minBMI * 100);
            trackBarBMI.LargeChange = 100; // press the PAGE UP or PAGE DOWN key or click the track bar on either side of the scroll box
            trackBarBMI.SmallChange = 1; // press one of the arrow keys
            trackBarBMI.TickFrequency = 100; // delta between ticks
            //trackBarBMI.TickStyle = TickStyle.Both;
            //height
            trackBarHeight.Maximum = Convert.ToInt32(maxHeight * 1000);
            trackBarHeight.Minimum = Convert.ToInt32(minHeight * 1000);
            trackBarHeight.LargeChange = 20;
            trackBarHeight.SmallChange = 1;
            trackBarHeight.TickFrequency = 20;
            //trackBarHeight.TickStyle = TickStyle.Both;
            //weight
            // initialize trackBarWeight
            trackBarWeight.Maximum = Convert.ToInt32(maxWeight * 100);
            trackBarWeight.Minimum = Convert.ToInt32(minWeight * 100);
            trackBarWeight.LargeChange = 500;
            trackBarWeight.SmallChange = 1;
            trackBarWeight.TickFrequency = 500;
            //trackBarWeight.TickStyle = TickStyle.Both;

            //--------------------textBox--------------------
            textBoxBMI.Text = minBMI.ToString("#.##");
            textBoxHeight.Text = minHeight.ToString("#.###");
            textBoxWeight.Text = minWeight.ToString("#.##");
            textBoxOriginalVolume.Enabled = false;

            //--------------------button--------------------
            buttonExportObj.Enabled = false;
            buttonDeform.Enabled = false;
            buttonExportObj2.Enabled = false;

            //--------------------radio buttons--------------------
            radioButtonSpecifyScaleFactor.Checked = true;
            radioButtonAlongCentroidVertexVector.Checked = true;

            //--------------------tabpage--------------------
            tabPageStandardDeformer.BackColor = SystemColors.Control;
            tabPageFreeDeformer.BackColor = SystemColors.Control;

            //-------------------- labels--------------------
            labelMinBMI.Text = string.Format("{0:#.##}", minBMI);
            labelMaxBMI.Text = string.Format("{0:#.##}", maxBMI);
            labelMinHeight.Text = string.Format("{0:#.###}", minHeight);
            labelMaxHeight.Text = string.Format("{0:#.###}", maxHeight);
            labelMinWeight.Text = string.Format("{0:#.##}", minWeight);
            //labelMinWeight.Enabled = false;
            labelMaxWeight.Text = string.Format("{0:#.##}", maxWeight);
            //labelMaxWeight.Enabled = false;

        }

        // ------------------------------------------------
        //                    buttons
        // ------------------------------------------------
        private void buttonReset_Click(object sender, EventArgs e)
        {
            trackBarBMI.Value = trackBarBMI.Minimum;
            trackBarHeight.Value = trackBarHeight.Minimum;
            trackBarWeight.Value = trackBarWeight.Minimum;

            textBoxBMI.Text = minBMI.ToString("#.##");
            textBoxHeight.Text = minHeight.ToString("#.###");
            textBoxWeight.Text = minWeight.ToString("#.##");

            textBoxComputationTime.Text = null;

            buttonExportObj.Enabled = false;
        }

        private void buttonReset2_Click(object sender, EventArgs e)
        {
            buttonImportObj2.Enabled = true;
            buttonDeform.Enabled = false;
            buttonExportObj2.Enabled = false;
            radioButtonSpecifyScaleFactor.Checked = true;
            radioButtonAlongCentroidVertexVector.Checked = false;
            radioButtonAlongVertexNormal.Checked = false;
            textBoxOriginalVolume.Text = null;
            textBoxDeformedVolume.Text = null;
            textBoxScaleFactorX.Text = null;
            textBoxScaleFactorY.Text = null;
            textBoxScaleFactorZ.Text = null;
            textBoxScaleFactorK.Text = null;
            textBoxComputationTime2.Text = null;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            // disable the generate button
            buttonGenerate.Enabled = false;

            //start counting time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool validInput = false;
            // check if values in the textbox and trackbar are the same
            validInput = Convert.ToDouble(textBoxBMI.Text) == Convert.ToDouble(trackBarBMI.Value) / 100.0
                && Convert.ToDouble(textBoxHeight.Text) == Convert.ToDouble(trackBarHeight.Value) / 1000.0;
            //&& Convert.ToDouble(textBoxWeight.Text) == Convert.ToDouble(trackBarWeight.Value) / 100.0;

            if (validInput == true)
            {
                //--------------------------step 1 interpolation--------------------------

                //get desired BMI
                desiredBMI = Convert.ToDouble(trackBarBMI.Value) / 100.0;

                //get the desired weight at the standard height (this is not what the user wants)
                desiredWeight = desiredBMI * GlobalConstant.standardHeight * GlobalConstant.standardHeight;

                //perform same-height-different-BMI interpolation
                newMesh = Interpolation.SameHeight_DifferentBMI(bulletMesh, holeMesh, desiredWeight);

                //--------------------------step 2 scaling--------------------------
                //get desired height
                desiredHeight = Convert.ToDouble(trackBarHeight.Value) / 1000.00;

                //update the desired weight (this is waht the user actually wants)
                desiredWeight = desiredBMI * desiredHeight * desiredHeight;

                //perform same-BMI-different-height scaling
                newMesh = Interpolation.SameBMI_DifferentHeight(newMesh, desiredWeight, desiredHeight);

                //calculate the waist circumference and waist hip ratio
                double waistCircumference = CalculateMesh.CalculateCircumference(newMesh, desiredHeight, "waist");
                textBoxWaistCircumference.Text = waistCircumference.ToString("#.##");
                double waistHipRatio = waistCircumference / CalculateMesh.CalculateCircumference(newMesh, desiredHeight, "hip");
                textBoxWaistHipRatio.Text = waistHipRatio.ToString("#.####");

                //display computation time
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                textBoxComputationTime.Text = ts.TotalSeconds.ToString("#.##");

                // enable the buttons
                buttonGenerate.Enabled = true;
                buttonExportObj.Enabled = true;
            }
            else
            {
                string exceptionReport = "Invalid input parameters";
                MessageBox.Show(exceptionReport);
            }

        }

        private void buttonExportObj_Click(object sender, EventArgs e)
        {
            //FileIO.ExportObj(newMesh, "Output");
            FileIO.ExportObjBatch(newMesh, desiredBMI, desiredHeight);
        }

        private void buttonDeform_Click(object sender, EventArgs e)
        {
            //start counting time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //disable button
            buttonDeform.Enabled = false;

            // if specify scale factor
            if (radioButtonSpecifyScaleFactor.Checked == true)
            {
                if (radioButtonAlongCentroidVertexVector.Checked == true)
                {
                    if (textBoxScaleFactorK.Text != "")
                    {
                        double scaleFactorK = Convert.ToDouble(textBoxScaleFactorK.Text);
                        newMesh = Interpolation.DeformAlongCentroidVector_SpecifyScaleFactor(oldMesh, scaleFactorK);
                    }
                }
                else if (radioButtonAlongVertexNormal.Checked == true)
                {
                    if (textBoxScaleFactorK.Text != "")
                    {
                        double scaleFactorK = Convert.ToDouble(textBoxScaleFactorK.Text);
                        newMesh = Interpolation.DeformAlongVertexNormal_SpecifyScaleFactor(oldMesh, scaleFactorK);
                    }
                }
                else if (radioButtonAlongXYZ.Checked == true)
                {
                    if (textBoxScaleFactorX.Text != "" && textBoxScaleFactorY.Text != "" && textBoxScaleFactorZ.Text != "")
                    {
                        double scaleFactorX = Convert.ToDouble(textBoxScaleFactorX.Text);
                        double scaleFactorY = Convert.ToDouble(textBoxScaleFactorY.Text);
                        double scaleFactorZ = Convert.ToDouble(textBoxScaleFactorZ.Text);
                        newMesh = Interpolation.DeformAlongXYZ_SpecifyScaleFactor(oldMesh, scaleFactorX, scaleFactorY, scaleFactorZ);
                    }
                }
            }
            //if specify volume
            else if (radioButtonSpecifyVolume.Checked == true)
            {
                if (textBoxDeformedVolume.Text != "") //desired volume is specified
                {
                    if (radioButtonAlongCentroidVertexVector.Checked == true)
                    {
                        double scaleFactorK = 0;
                        double desiredVolume = Convert.ToDouble(textBoxDeformedVolume.Text);
                        newMesh = Interpolation.DeformAlongCentroidVector_SpecifyVolume(oldMesh, desiredVolume, out scaleFactorK);
                        textBoxScaleFactorK.Text = scaleFactorK.ToString("#.####");
                    }
                    else if (radioButtonAlongVertexNormal.Checked == true)
                    {
                        double scaleFactorK = 0;
                        double desiredVolume = Convert.ToDouble(textBoxDeformedVolume.Text);
                        newMesh = Interpolation.DeformAlongVertexNormal_SpecifyVolume(oldMesh, desiredVolume, out scaleFactorK);
                        textBoxScaleFactorK.Text = scaleFactorK.ToString("#.####");
                    }
                    else if (radioButtonAlongXYZ.Checked == true)
                    {
                        double scaleFactorX = 0;
                        double scaleFactorY = 0;
                        double scaleFactorZ = 0;
                        double desiredVolume = Convert.ToDouble(textBoxDeformedVolume.Text);
                        newMesh = Interpolation.DeformAlongXYZ_SpecifyVolume(oldMesh, desiredVolume, out scaleFactorX, out scaleFactorY, out scaleFactorZ);
                        textBoxScaleFactorX.Text = scaleFactorX.ToString("#.####");
                        textBoxScaleFactorY.Text = scaleFactorY.ToString("#.####");
                        textBoxScaleFactorZ.Text = scaleFactorZ.ToString("#.####");
                    }//end if
                }//end if
            }//end if

            //enable button
            buttonDeform.Enabled = true;
            buttonExportObj2.Enabled = true;

            //display time elapsed
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            textBoxComputationTime2.Text = ts.TotalSeconds.ToString("#.##");

        }//end method

        private void buttonImportObj2_Click(object sender, EventArgs e)
        {
            //import original mesh
            oldMesh = FileIO.ImportObjWithDialog();

            //if import successfully
            if (oldMesh.Count != 0)
            {
                //calculate and display volume
                double oldVolume = CalculateMesh.CalculateVolume(oldMesh[0], oldMesh[1]);
                textBoxOriginalVolume.Text = oldVolume.ToString("#.####");

                //enable button
                buttonDeform.Enabled = true;
            }
        }

        // ------------------------------------------------
        //                    radio buttons
        // ------------------------------------------------
        private void radioButtonChooseTaskMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSpecifyScaleFactor.Checked == true && radioButtonAlongCentroidVertexVector.Checked == true)
            {
                textBoxDeformedVolume.Enabled = false;
                textBoxScaleFactorX.Enabled = false;
                textBoxScaleFactorY.Enabled = false;
                textBoxScaleFactorZ.Enabled = false;
                textBoxScaleFactorK.Enabled = true;
            }
            else if (radioButtonSpecifyScaleFactor.Checked == true && radioButtonAlongVertexNormal.Checked == true)
            {
                textBoxDeformedVolume.Enabled = false;
                textBoxScaleFactorX.Enabled = false;
                textBoxScaleFactorY.Enabled = false;
                textBoxScaleFactorZ.Enabled = false;
                textBoxScaleFactorK.Enabled = true;
            }
            else if (radioButtonSpecifyScaleFactor.Checked == true && radioButtonAlongXYZ.Checked == true)
            {
                textBoxDeformedVolume.Enabled = false;
                textBoxScaleFactorX.Enabled = true;
                textBoxScaleFactorY.Enabled = true;
                textBoxScaleFactorZ.Enabled = true;
                textBoxScaleFactorK.Enabled = false;
            }
            else if (radioButtonSpecifyVolume.Checked == true && radioButtonAlongCentroidVertexVector.Checked == true)
            {
                textBoxDeformedVolume.Enabled = true;
                textBoxScaleFactorX.Enabled = false;
                textBoxScaleFactorY.Enabled = false;
                textBoxScaleFactorZ.Enabled = false;
                textBoxScaleFactorK.Enabled = false;
            }
            else if (radioButtonSpecifyVolume.Checked == true && radioButtonAlongVertexNormal.Checked == true)
            {
                textBoxDeformedVolume.Enabled = true;
                textBoxScaleFactorX.Enabled = false;
                textBoxScaleFactorY.Enabled = false;
                textBoxScaleFactorZ.Enabled = false;
                textBoxScaleFactorK.Enabled = false;
            }
            else if (radioButtonSpecifyVolume.Checked == true && radioButtonAlongXYZ.Checked == true)
            {
                textBoxDeformedVolume.Enabled = true;
                textBoxScaleFactorX.Enabled = false;
                textBoxScaleFactorY.Enabled = false;
                textBoxScaleFactorZ.Enabled = false;
                textBoxScaleFactorK.Enabled = false;
            }
        }

        // ------------------------------------------------
        //                    trackbars
        // ------------------------------------------------
        // triggered when BMI, height and weight trackbars' value change
        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
            TrackBar sd = (TrackBar)sender;

            // get current BMI & height, and calculate the corresponding weight
            double BMI = Convert.ToDouble(trackBarBMI.Value) / 100.00;
            double height = Convert.ToDouble(trackBarHeight.Value) / 1000.00;
            double weight = BMI * height * height;
            trackBarWeight.Value = Convert.ToInt32(weight * 100); //no need to disable trackBarWeight any more because of this command

            if (sd.Name == "trackBarBMI")
            {
                // update the BMI textbox
                textBoxBMI.Text = BMI.ToString("#.##");
            }
            else if (sd.Name == "trackBarHeight")
            {
                textBoxHeight.Text = height.ToString("#.###");
            }
            else if (sd.Name == "trackBarWeight")
            {
                textBoxWeight.Text = weight.ToString("#.##");
            }
        }

        // ------------------------------------------------
        //                    textboxes
        // ------------------------------------------------
        // triggered when BMI, height and weight textboxes' text change
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
            else if (sd.Name == "textBoxWeight")
            {
                minValue = minWeight;
                maxValue = maxWeight;
            }

            // check if text is non-empty
            if (sd.Text != "")
            {
                // entire text of the textbox
                string completedText = sd.Text;

                // convert .XX to 0.XX
                if (sd.Text[0] == '.')
                {
                    completedText = '0' + sd.Text;
                }

                // check if the number entered is valid
                double enteredValue = Convert.ToDouble(completedText);
                bool textValid = (enteredValue >= minValue) &&
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
                        trackBarHeight.Value = Convert.ToInt32(enteredValue * 1000);
                    }// end if
                }// end if
            }// end if
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox sd = (TextBox)sender; // conversion of obj type into textbox type, why it works?
            
            int decimalDigitsAllowed = 0;
            if (sd == textBoxBMI)
            {
                decimalDigitsAllowed = 2;
            }
            else if (sd == textBoxHeight)
            {
                decimalDigitsAllowed = 3;
            }

            bool tooManyDecimalPeriod = false;
            bool tooManyDecimalDigit = false;

            // always allow entering of backspace
            if (e.KeyCode != Keys.Back)
            {
                // check if a non-number is entered
                isInvalidCharacter = (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9) && // Determine whether the keystroke is a number from the top of the keyboard
                                     (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9) && // Determine whether the keystroke is a number from the keypad
                                     (e.KeyCode != Keys.OemPeriod) ||
                                     Control.ModifierKeys == Keys.Shift; //If shift key was pressed, it's not a number

                // check if there is already one decimal point
                if (sd.Text.Contains('.') == true)
                {
                    // check if one more decimal point is entered
                    tooManyDecimalPeriod = e.KeyCode == Keys.OemPeriod;

                    int decimalPointPosition = sd.Text.IndexOf('.');

                    // check if the current cursor is at the decimal part
                    if (sd.SelectionStart > decimalPointPosition)
                    {
                        // check if there has already been decimalDigitsAllowed decimal digits
                        // i.e check if the cursor is at the position of the decimalDigitsAllowed+1 decimal digit
                        tooManyDecimalDigit = ((sd.Text.Length - decimalPointPosition) > decimalDigitsAllowed);
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
                //reset isInvalidCharacter status
                isInvalidCharacter = false;
            }
        }

        //
        private void buttonTrial_Click(object sender, EventArgs e)
        {
            /*Chart dataChart = new Chart();
            double x = Convert.ToDouble(textBoxTrial.Text);
            textBoxTrial.Text= dataChart.DataManipulator.Statistics.NormalDistribution(x).ToString();*/
        }

        //
        private void textBoxPreventEnteringCharacters_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //convert from .xx to 0.xx
        private void textBoxAdjustNumber_TextChanged(object sender, EventArgs e)
        {
            TextBox sd = (TextBox)sender;
            if (sd.Text != "")
            {
                if (sd.Text[0] == '.')
                {
                    sd.Text = '0' + sd.Text;

                }
            }
        }

    }// end form
}// end namespace
