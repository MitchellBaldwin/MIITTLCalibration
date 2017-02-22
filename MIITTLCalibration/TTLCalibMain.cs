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
        
        private Label[] maxPipDisplayLabels = new Label[10];
        private TextBox[] pipTextBoxes = new TextBox[10];
        private Label[] minPipDisplayLabels = new Label[10];

        private string dataPath = Path.Combine(Application.ExecutablePath, @"\Data");
        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }
        
        private string pVLFilePath = Path.Combine(Application.ExecutablePath, @"\PVL");
        public string PVLFilePath
        {
            get { return pVLFilePath; }
            set { pVLFilePath = value; }
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
            InitializeExcelWorksheets();

            if (ExcelOK)
            {
                // Read and display compliance setting values and Pip limits
                // (may move to a helper function - same functionality needed for changing lung model & type)

            }
        }

        private void TTLCalibMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ExcelApp != null)
            {
                ExcelApp.Quit();
            }
        }

        private void LungTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton ltrb = sender as RadioButton;
            if (ltrb != null)
            {
                SNPrefix = ltrb.Tag.ToString();

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
                MessageBox.Show("MS Excel is not installed on this computer", "MS Excel error");
                return;
            }
            else
            {
                //MS Excel is installed
                try
                {
                    ExcelApp = new Excel.Application();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Microsoft Excel error");
                    ExcelOK = false;
                    return;
                }

                //MS Excel is started; open the PV Cal workbook
                try
                {
                    string wbPath = Path.Combine(DataPath, PVCalFileName);
                    PVCalWorkbook = ExcelApp.Workbooks.Open(wbPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Microsoft Excel error");
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

                    ActiveCalWorksheet = SL0CalWorksheet;

                    ExcelOK = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Microsoft Excel error");
                    ExcelOK = false;
                    return;
                }
            }
        }

        #endregion Helper functions

    }
}
