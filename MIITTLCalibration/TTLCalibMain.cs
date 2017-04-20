using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace MIITTLCalibration
{
    public partial class TTLCalibMain : Form
    {
        #region Form level properties and variables
        private static Excel.Application ExcelApp = null;
        private static Excel.Workbook PVCalWorkbook = null;
        private static Excel.Worksheet SL0CalWorksheet = null;
        private static Excel.Worksheet AIICalWorksheet = null;
        private static Excel.Worksheet AIACalWorksheet = null;
        private static Excel.Worksheet DALCalWorksheet = null;
        private static Excel.Worksheet DARCalWorksheet = null;

        private static Excel.Worksheet ActiveCalWorksheet = null;

        private Boolean excelOK = false;
        public Boolean ExcelOK
        {
            get { return excelOK; }
            set { excelOK = value; }
        }

        private Label[] cSettingDisplayLabels = new Label[10];
        private Label[] maxPipDisplayLabels = new Label[10];
        private TextBox[] pipTextBoxes = new TextBox[10];
        private Label[] minPipDisplayLabels = new Label[10];

        private Boolean appIsClosing = false;
        public Boolean AppIsClosing
        {
            get { return appIsClosing; }
            set { appIsClosing = value; }
        }
        
        private string dataPath = Path.Combine(Application.StartupPath, "Data");
        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }
        
        private string pVLFilePath = Path.Combine(Application.StartupPath, "PVL");
        public string PVLFilePath
        {
            get { return pVLFilePath; }
            set { pVLFilePath = value; }
        }

        private string pVLFileName = "SL00000.pvl";
        public string PVLFileName
        {
            get 
            { 
                // Build & return file name based on lung model type and serial number
                pVLFileName = SNPrefix + serialNumberTextBox.Text + ".pvl";
                return pVLFileName; 
            }
            set { pVLFileName = value; }
        }

        private string sNPrefix = "SL0";
        public string SNPrefix
        {
            get { return sNPrefix; }
            set { sNPrefix = value; }
        }

        private string pVCalFileName = "PV Cal.xlsx";
        public string PVCalFileName
        {
            get { return pVCalFileName; }
            set { pVCalFileName = value; }
        }

        #endregion

        public TTLCalibMain()
        {
            InitializeComponent();

            cSettingDisplayLabels[0] = c10Label;
            cSettingDisplayLabels[1] = c20Label;
            cSettingDisplayLabels[2] = c30Label;
            cSettingDisplayLabels[3] = c40Label;
            cSettingDisplayLabels[4] = c50Label;
            cSettingDisplayLabels[5] = c60Label;
            cSettingDisplayLabels[6] = c70Label;
            cSettingDisplayLabels[7] = c80Label;
            cSettingDisplayLabels[8] = c90Label;
            cSettingDisplayLabels[9] = c100Label;

            maxPipDisplayLabels[0] = c10MaxPipDisplayLabel;
            maxPipDisplayLabels[1] = c20MaxPipDisplayLabel;
            maxPipDisplayLabels[2] = c30MaxPipDisplayLabel;
            maxPipDisplayLabels[3] = c40MaxPipDisplayLabel;
            maxPipDisplayLabels[4] = c50MaxPipDisplayLabel;
            maxPipDisplayLabels[5] = c60MaxPipDisplayLabel;
            maxPipDisplayLabels[6] = c70MaxPipDisplayLabel;
            maxPipDisplayLabels[7] = c80MaxPipDisplayLabel;
            maxPipDisplayLabels[8] = c90MaxPipDisplayLabel;
            maxPipDisplayLabels[9] = c100MaxPipDisplayLabel;

            pipTextBoxes[0] = c10PipTextBox;
            pipTextBoxes[1] = c20PipTextBox;
            pipTextBoxes[2] = c30PipTextBox;
            pipTextBoxes[3] = c40PipTextBox;
            pipTextBoxes[4] = c50PipTextBox;
            pipTextBoxes[5] = c60PipTextBox;
            pipTextBoxes[6] = c70PipTextBox;
            pipTextBoxes[7] = c80PipTextBox;
            pipTextBoxes[8] = c90PipTextBox;
            pipTextBoxes[9] = c100PipTextBox;

            minPipDisplayLabels[0] = c10MinPipDisplayLabel;
            minPipDisplayLabels[1] = c20MinPipDisplayLabel;
            minPipDisplayLabels[2] = c30MinPipDisplayLabel;
            minPipDisplayLabels[3] = c40MinPipDisplayLabel;
            minPipDisplayLabels[4] = c50MinPipDisplayLabel;
            minPipDisplayLabels[5] = c60MinPipDisplayLabel;
            minPipDisplayLabels[6] = c70MinPipDisplayLabel;
            minPipDisplayLabels[7] = c80MinPipDisplayLabel;
            minPipDisplayLabels[8] = c90MinPipDisplayLabel;
            minPipDisplayLabels[9] = c100MinPipDisplayLabel;

        }

        #region Form level event handlers
        private void TTLCalibMain_Load(object sender, EventArgs e)
        {
            // Show splash screen while Excel loads
            LoadWorkbookSplashSscreen lwss = new LoadWorkbookSplashSscreen();
            lwss.Show();

            InitializeExcelWorksheets();

            PVCalWorkbook.BeforeClose += PVCalWorkbookBeforeClose;

            // Close splash screen
            lwss.Close();

            // Check whether PVL folder location has been set;
            // if it has, then load it
            // if it has not, then initialize it to the default location
            string tempPVLPath = MIITTLCalibration.Properties.Settings.Default.PVLFolder;
            if (tempPVLPath == "none")
            {
                MIITTLCalibration.Properties.Settings.Default.PVLFolder = PVLFilePath;
                MIITTLCalibration.Properties.Settings.Default.Save();
            }
            else
            {
                PVLFilePath = MIITTLCalibration.Properties.Settings.Default.PVLFolder;
                if (ExcelOK)
                {
                    buildPVLFileButton.Enabled = true;
                }
            }
        }

        private void TTLCalibMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExcelApp != null)
            {
                AppIsClosing = true;
                // Close the PV Cal workbook - do NOT save changes
                // Check whether Close(true) saves the file - YES
                PVCalWorkbook.Close(false);
                ExcelApp.Quit();
            }
        }

        private void showDataNormalizationFileButtonCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExcelOK)
            {
                ExcelApp.Visible = showDataNormalizationFileButtonCheckBox.Checked;
            }
            else
            {
                // Re-initialize whatever aspects of Excel and PV Cal.xlsx found missing
                // No longer necessary - implemented BeforeClose event handler on workbook to prevent 
                //the user closing Excel outside the program

            }
        }

        private void LungTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton ltrb = sender as RadioButton;
            if (ltrb != null)
            {
                SNPrefix = ltrb.Tag.ToString();
                snPrefixDisplayLabel.Text = SNPrefix;

                // Use helper function to initialize text on controls:
                SetActiveCalWorksheet();

                #region Obsolete code
                //    // Select worksheet associated with the selected lung model & type
                //    if (ltrb == singleRadioButton)
                //    {
                //        ActiveCalWorksheet = SL0CalWorksheet;
                //    }
                //    else if (ltrb == aiInfantRadioButton)
                //    {
                //        ActiveCalWorksheet = AIICalWorksheet;
                //    }
                //    else if (ltrb == aiAdultRadioButton)
                //    {
                //        ActiveCalWorksheet = AIACalWorksheet;
                //    }
                //    else if (ltrb == daLeftRadioButton)
                //    {
                //        ActiveCalWorksheet = DALCalWorksheet;
                //    }
                //    else if (ltrb == daRightRadioButton)
                //    {
                //        ActiveCalWorksheet = DARCalWorksheet;
                //    }

                //    // Set compliance setting labels and controls as appropriate for infant or adult lung:
                //    System.Array cSettingVals = ActiveCalWorksheet.get_Range("C7", "L7").Cells.Value;
                //    if (ActiveCalWorksheet == AIICalWorksheet)
                //    {
                //        for (int i = 0; i < 10; ++i)
                //        {
                //            cSettingDisplayLabels[i].Text = ((double)cSettingVals.GetValue(1, i + 1)).ToString("0.000");
                //        }
                //        // c70 & c90 data points no longer used:
                //        //c70Label.Visible = false;
                //        //c70MaxPipDisplayLabel.Visible = false;
                //        //c70PipTextBox.Visible = false;
                //        //c70MinPipDisplayLabel.Visible = false;
                //        //c90Label.Visible = false;
                //        //c90MaxPipDisplayLabel.Visible = false;
                //        //c90PipTextBox.Visible = false;
                //        //c90MinPipDisplayLabel.Visible = false;
                //    }
                //    else
                //    {
                //        for (int i = 0; i < 10; ++i)
                //        {
                //            cSettingDisplayLabels[i].Text = ((double)cSettingVals.GetValue(1, i + 1)).ToString("0.00");
                //        }
                //        // c70 & c90 data points no longer used:
                //        //c70Label.Visible = true;
                //        //c70MaxPipDisplayLabel.Visible = true;
                //        //c70PipTextBox.Visible = true;
                //        //c70MinPipDisplayLabel.Visible = true;
                //        //c90Label.Visible = true;
                //        //c90MaxPipDisplayLabel.Visible = true;
                //        //c90PipTextBox.Visible = true;
                //        //c90MinPipDisplayLabel.Visible = true;
                //    }
                #endregion
            }
            //buildPVLFileButton.Text = "Build " + PVLFileName;
            
        }

        private void serialNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            buildPVLFileButton.Text = "Build " + PVLFileName;
        }

        private void buildPVLFileButton_Click(object sender, EventArgs e)
        {
            double[] pipVals = new double[10];
            double[] ccVals = new double[4];
            string[] pvlLines = new string[40];

            for (int i = 0; i < 10; ++i)
            {
                pipVals[i] = Convert.ToDouble(pipTextBoxes[i].Text);
            }

            // Various ways to specifying cell ranges:
            //Excel.Range startCell = (Excel.Range)ActiveCalWorksheet.Cells[10, 3];
            //Excel.Range endCell = (Excel.Range)ActiveCalWorksheet.Cells[10, 12];
            //Excel.Range startCell = (Excel.Range)ActiveCalWorksheet.get_Range("C10");
            //Excel.Range endCell = (Excel.Range)ActiveCalWorksheet.get_Range("L10");
            //Excel.Range pipRange = ActiveCalWorksheet.Range[startCell, endCell];
            
            // Write the entered values for Pip measurements @ 1000 mL (100 mL for infant lungs) to the PV Cal worksheet:
            Excel.Range pipRange = ActiveCalWorksheet.get_Range("C30", "L30");
            pipRange.Value2 = pipVals;

            // Write the entered values for Pip measurements at other injection volumes to the active worksheet:
            ActiveCalWorksheet.get_Range("C21").Value2 = Convert.ToDouble(c10Pip100TextBox.Text);
            ActiveCalWorksheet.get_Range("C25").Value2 = Convert.ToDouble(c10Pip500TextBox.Text);

            ActiveCalWorksheet.get_Range("D21").Value2 = Convert.ToDouble(c20Pip100TextBox.Text);
            ActiveCalWorksheet.get_Range("D25").Value2 = Convert.ToDouble(c20Pip500TextBox.Text);
            ActiveCalWorksheet.get_Range("D35").Value2 = Convert.ToDouble(c20Pip1500TextBox.Text);
            ActiveCalWorksheet.get_Range("D40").Value2 = Convert.ToDouble(c20Pip2000TextBox.Text);

            ActiveCalWorksheet.get_Range("G21").Value2 = Convert.ToDouble(c50Pip100TextBox.Text);
            ActiveCalWorksheet.get_Range("G25").Value2 = Convert.ToDouble(c50Pip500TextBox.Text);
            ActiveCalWorksheet.get_Range("G35").Value2 = Convert.ToDouble(c50Pip1500TextBox.Text);
            ActiveCalWorksheet.get_Range("G40").Value2 = Convert.ToDouble(c50Pip2000TextBox.Text);

            ActiveCalWorksheet.get_Range("L21").Value2 = Convert.ToDouble(c100Pip100TextBox.Text);
            ActiveCalWorksheet.get_Range("L25").Value2 = Convert.ToDouble(c100Pip500TextBox.Text);
            ActiveCalWorksheet.get_Range("L35").Value2 = Convert.ToDouble(c100Pip1500TextBox.Text);
            ActiveCalWorksheet.get_Range("L40").Value2 = Convert.ToDouble(c100Pip2000TextBox.Text);
            
            // Read resultant compliance coefficient values as strings and write to a new PVL file
            for (int j=0; j<10; ++j)
            {
                //Excel.Range ccRange = ActiveCalWorksheet.get_Range("Y22", "Y25");
                Excel.Range startCell = (Excel.Range)ActiveCalWorksheet.Cells[j * 17 + 30, 25];
                Excel.Range endCell = (Excel.Range)ActiveCalWorksheet.Cells[j * 17 + 33, 25];
                Excel.Range ccRange = ActiveCalWorksheet.Range[startCell, endCell];
                for (int i = 0; i < 4; ++i)
                {
                    var a = ccRange.Cells[i + 1, 1].Value2;
                    ccVals[i] = ccRange.Cells[i + 1, 1].Value2;
                }
                for (int i = 0; i < 4; ++i)
                {
                    pvlLines[j * 4 + i] = ccVals[3 - i].ToString("0.000000000000");
                }
            }

            string pvlFilePathAndName = Path.Combine(PVLFilePath, PVLFileName);
            
            // Check whether file already exists and if so allow user to enter a different file name / path:
            DialogResult result = DialogResult.OK;
            bool pvlFileExists = File.Exists(pvlFilePathAndName);
            if (pvlFileExists)
            {
                result = MessageBox.Show("PVL file: " + PVLFileName + " already exists, overwrite?",
                                          "Confirm overwrite",
                                          MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    pvlFileDialog.InitialDirectory = PVLFilePath;
                    pvlFileDialog.FileName = PVLFileName;
                    result = pvlFileDialog.ShowDialog();
                }
            }
            if (result == DialogResult.OK)
            {
                using (StreamWriter pvlWriter = new StreamWriter(pvlFilePathAndName))
                {
                    foreach (string line in pvlLines)
                    {
                        pvlWriter.WriteLine(line);
                    }
                }
                // Display message box indicating the PVL file was successfully written:
                MessageBox.Show("Successfully created: " + pvlFilePathAndName, "Created PVL File");
            }

            // Save a copy of the Excel worksheet with the data and results for this lung
            // First delete all the worksheets except the active one:
            ExcelApp.DisplayAlerts = false;
            foreach (Excel.Worksheet ws in PVCalWorkbook.Sheets)
            {
                if (ws != ActiveCalWorksheet)
                {
                    ws.Delete();
                }
            }
            ExcelApp.DisplayAlerts = true;
            // Build the path and file name and save:
            string xlsxFileName = Path.ChangeExtension(PVLFileName, ".xlsx");
            PVCalWorkbook.SaveAs(Path.Combine(PVLFilePath, xlsxFileName));
            // Close workbook and re-open the original:
            PVCalWorkbook.Close();
            OpenPVCalWorkbook();
            //string wbPath = Path.Combine(DataPath, PVCalFileName);
            //PVCalWorkbook = ExcelApp.Workbooks.Open(wbPath);
            //int n = ExcelApp.Workbooks.Count;     // Test code

        }

        private void selectOutputFolderButton_Click(object sender, EventArgs e)
        {
            selectOutputFolderDialog.SelectedPath = PVLFilePath;
            DialogResult result = selectOutputFolderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (IsDirectoryWritable(selectOutputFolderDialog.SelectedPath))
                {
                    PVLFilePath = selectOutputFolderDialog.SelectedPath;
                    MIITTLCalibration.Properties.Settings.Default.PVLFolder = PVLFilePath;
                    MIITTLCalibration.Properties.Settings.Default.Save();
                    if (ExcelOK)
                    {
                        buildPVLFileButton.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("Operating System denied write access to the selected folder", "Access Denied");
                    buildPVLFileButton.Enabled = false;
                }
            }
        }

        #endregion Form level event handlers

        #region Helper functions
        private void InitializeExcelWorksheets()
        {
            Type officeType = Type.GetTypeFromProgID("Excel.Application");
            if (officeType == null)
            {
                //MS Excel is not installed
                ExcelOK = false;
                MessageBox.Show("Microsoft Excel is not installed on this computer", "Microsoft Excel error");
                return;
            }
            else
            {
                //MS Excel is installed
                try
                {
                    ExcelApp = new Excel.Application();
                    //ExcelApp.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Microsoft Excel error");
                    ExcelApp = null;
                    ExcelOK = false;
                    return;
                }

                //MS Excel is started; open the PV Cal workbook
                OpenPVCalWorkbook();

            }
        }

        private void OpenPVCalWorkbook()
        {
            // Check for existance of PVCal.xlsx at the proper location; allow user to browse
            //to it before opening if not found:
            string wpPathAndFileName = Path.Combine(DataPath, PVCalFileName);
            if (!File.Exists(wpPathAndFileName))
            {
                pvcwbFileDialog.InitialDirectory = DataPath;
                pvcwbFileDialog.FileName = PVCalFileName;
                pvcwbFileDialog.ShowDialog();

                wpPathAndFileName = pvcwbFileDialog.FileName;
                PVCalFileName = Path.GetFileName(wpPathAndFileName);
                DataPath = Path.GetDirectoryName(wpPathAndFileName);
            }

            try
            {
                PVCalWorkbook = ExcelApp.Workbooks.Open(wpPathAndFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Microsoft Excel error");
                PVCalWorkbook = null;
                if (ExcelApp != null)
                {
                    ExcelApp.Quit();
                    ExcelApp = null;
                }
                ExcelOK = false;
                return;
            }
            
            //The PV Cal workbook is open; load the worksheets associated with each lung model & type
            try
            {
                SL0CalWorksheet = PVCalWorkbook.Sheets[1];
                AIICalWorksheet = PVCalWorkbook.Sheets[2];
                AIACalWorksheet = PVCalWorkbook.Sheets[3];
                DALCalWorksheet = PVCalWorkbook.Sheets[4];
                DARCalWorksheet = PVCalWorkbook.Sheets[5];

                ExcelOK = true;

                // Select active worksheet based in lung type radio button states
                SetActiveCalWorksheet();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Microsoft Excel error");
                ExcelOK = false;
                return;
            }
        }

        private void PVCalWorkbookBeforeClose(ref bool Cancel)
        {
            if (AppIsClosing)
            {
                Cancel = false;
            }
            else
            {
                Cancel = true;
            }
        }
        
        private void SetActiveCalWorksheet()
        {
            RadioButton selectedLungTypeRadioButton = singleRadioButton;
            foreach (RadioButton rb in selectLungModelTypeGroupBox.Controls)
            {
                if (rb.Checked)
                {
                    selectedLungTypeRadioButton = rb;
                }
            }
            // Select worksheet associated with the selected lung model & type
            if (selectedLungTypeRadioButton == singleRadioButton)
            {
                ActiveCalWorksheet = SL0CalWorksheet;
            }
            else if (selectedLungTypeRadioButton == aiInfantRadioButton)
            {
                ActiveCalWorksheet = AIICalWorksheet;
            }
            else if (selectedLungTypeRadioButton == aiAdultRadioButton)
            {
                ActiveCalWorksheet = AIACalWorksheet;
            }
            else if (selectedLungTypeRadioButton == daLeftRadioButton)
            {
                ActiveCalWorksheet = DALCalWorksheet;
            }
            else if (selectedLungTypeRadioButton == daRightRadioButton)
            {
                ActiveCalWorksheet = DARCalWorksheet;
            }
                
            // Set compliance setting labels and controls as appropriate for infant or adult lung:
            System.Array cSettingVals = ActiveCalWorksheet.get_Range("C7", "L7").Cells.Value;
            if (ActiveCalWorksheet == AIICalWorksheet)
            {
                for (int i = 0; i < 10; ++i)
                {
                    cSettingDisplayLabels[i].Text = ((double)cSettingVals.GetValue(1, i + 1)).ToString("0.000");
                    measuredPip100Label.Text = "Measured Pip @ 10 mL:";
                    measuredPip500Label.Text = "Measured Pip @ 50 mL:";
                    maxPipLabel .Text = "Maximum Pip @ 100 mL:";
                    measuredPipLabel.Text = "Measured Pip @ 100 mL:";
                    minPipLabel.Text = "Minimum Pip @ 100 mL:";
                    measuredPip1500Label.Text = "Measured Pip @ 150 mL:";
                    measuredPip2000Label.Text = "Measured Pip @ 200 mL:";
                }
                #region Obsolete code
		        // c70 & c90 data points no longer used:
                //c70Label.Visible = false;
                //c70MaxPipDisplayLabel.Visible = false;
                //c70PipTextBox.Visible = false;
                //c70MinPipDisplayLabel.Visible = false;
                //c90Label.Visible = false;
                //c90MaxPipDisplayLabel.Visible = false;
                //c90PipTextBox.Visible = false;
                //c90MinPipDisplayLabel.Visible = false;
 
	            #endregion
            }
            else
            {
                for (int i = 0; i < 10; ++i)
                {
                    cSettingDisplayLabels[i].Text = ((double)cSettingVals.GetValue(1, i + 1)).ToString("0.00");
                    measuredPip100Label.Text = "Measured Pip @ 100 mL:";
                    measuredPip500Label.Text = "Measured Pip @ 500 mL:";
                    maxPipLabel.Text = "Maximum Pip @ 1000 mL:";
                    measuredPipLabel.Text = "Measured Pip @ 1000 mL:";
                    minPipLabel.Text = "Minimum Pip @ 1000 mL:";
                    measuredPip1500Label.Text = "Measured Pip @ 1500 mL:";
                    measuredPip2000Label.Text = "Measured Pip @ 2000 mL:";
                }
                #region Obsolete code
		        // c70 & c90 data points no longer used:
                //c70Label.Visible = true;
                //c70MaxPipDisplayLabel.Visible = true;
                //c70PipTextBox.Visible = true;
                //c70MinPipDisplayLabel.Visible = true;
                //c90Label.Visible = true;
                //c90MaxPipDisplayLabel.Visible = true;
                //c90PipTextBox.Visible = true;
                //c90MinPipDisplayLabel.Visible = true;
 
	            #endregion            
            }
            
            buildPVLFileButton.Text = "Build " + PVLFileName;

            if (ExcelOK)
            {
                // Read and display compliance setting values and Pip limits:
                System.Array maxPipVals = ActiveCalWorksheet.get_Range("C9", "L9").Cells.Value;
                System.Array nomPipVals = ActiveCalWorksheet.get_Range("C8", "L8").Cells.Value;
                System.Array minPipVals = ActiveCalWorksheet.get_Range("C11", "L11").Cells.Value;
                for (int i = 0; i < 10; ++i)
                {
                    //cSettingDisplayLabels[i].Text = ((double)cSettingVals.GetValue(1, i + 1)).ToString("0.00");
                    maxPipDisplayLabels[i].Text = ((double)maxPipVals.GetValue(1, i + 1)).ToString("0.00");
                    pipTextBoxes[i].Text = ((double)nomPipVals.GetValue(1, i + 1)).ToString("0.00");
                    minPipDisplayLabels[i].Text = ((double)minPipVals.GetValue(1, i + 1)).ToString("0.00");
                }

                c10Pip100TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("C21").Cells.Value2)).ToString("0.00");
                c10Pip500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("C25").Cells.Value2)).ToString("0.00");

                c20Pip100TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("D21").Cells.Value2)).ToString("0.00");
                c20Pip500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("D25").Cells.Value2)).ToString("0.00");
                c20Pip1500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("D35").Cells.Value2)).ToString("0.00");
                c20Pip2000TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("D40").Cells.Value2)).ToString("0.00");

                c50Pip100TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("G21").Cells.Value2)).ToString("0.00");
                c50Pip500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("G25").Cells.Value2)).ToString("0.00");
                c50Pip1500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("G35").Cells.Value2)).ToString("0.00");
                c50Pip2000TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("G40").Cells.Value2)).ToString("0.00");

                c100Pip100TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("L21").Cells.Value2)).ToString("0.00");
                c100Pip500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("L25").Cells.Value2)).ToString("0.00");
                c100Pip1500TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("L35").Cells.Value2)).ToString("0.00");
                c100Pip2000TextBox.Text = ((double)(ActiveCalWorksheet.get_Range("L40").Cells.Value2)).ToString("0.00");

            }

        }
        
        public bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            try
            {
                using (FileStream fs = File.Create(Path.Combine(dirPath, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                {
                }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        #endregion Helper functions

    }
}
