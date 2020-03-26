using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Reflection;
using ADGV;
using userLogin;

namespace MFQCIMLogChecker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Material
        private string[] logInfo;
        ACF materialPort1ACF = new ACF();
        ACF materialPort2ACF = new ACF();
        COF materialPort1COF = new COF();
        COF materialPort2COF = new COF();
        FRAME_COF materialPort1FrameCOF = new FRAME_COF();
        FRAME_COF materialPort2FrameCOF = new FRAME_COF();
        FPC materialPort1FPC = new FPC();
        FPC materialPort2FPC = new FPC();
        StringBuilder resultText = new StringBuilder();
        System.Data.DataTable mainTable = new System.Data.DataTable();
        List<string> issueList = new List<string>();

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Setting.
            cmbEQSelect.SelectedIndex = 0;
            cmbLogVerSelect.SelectedIndex = 0;
            txtLogPath.Text = "";
            string materialLogInfo = "Date,Time,LogType,CEID,CELL_ID,MT_BATCH_ID,MT_BATCH_NAME,MT_ID,MT_PORT,MT_ST,MT_STATE,MT_TOTAL,MT_TYPE,USE,PRODUCT,ASSEMBLE,NG,P_SUPPLY,P_USE,P_NG,P_ASSEMBLE,REMAIN,MT_NO";
            logInfo = materialLogInfo.Split(',');
            chkACFCheck.Checked = false;
            GridBuffer(dgvMemory);
            GridBuffer(adgvFilter);
            GridBuffer(adgvTKData);
            GridBuffer(adgvTKAverage);
            tabControl1.Enabled = false;

            // Analysis set.
            Analysis_Init();

        }

        // Grid view speed.
        public void GridBuffer(DataGridView dgv)
        {
            Type dgvType = dgvMemory.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgvMemory, true, null);
        }
        public void GridBuffer(AdvancedDataGridView dgv)
        {
            Type dgvType = dgvMemory.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgvMemory, true, null);
        }


        // log path set.
        private void btnLogFilePathSet_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLogPath.Text = openFileDialog1.FileName.ToString();
            }
        }

        // Log check.
        private void btnLogCheck_Click(object sender, EventArgs e)
        {
            issueList.Clear();
            this.adgvFilter.DataSource = null;
            this.Enabled = false;
            prgStatus.Value = 0;
            mainTable.Clear();
            mainTable.Columns.Clear();
            resultText.Clear();
            txtReuslt.Clear();
            string filePath = txtLogPath.Text;
            double prgCount = 0;

            if (txtLogPath.Text == string.Empty)
            {
                MessageBox.Show("Log 경로를 설정해주세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            FileInfo fileInfo = new FileInfo(txtLogPath.Text);
            if (!fileInfo.Exists)
            {
                MessageBox.Show("해당 경로에 Log 파일이 존재하지 않습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }

            string[] logArray = File.ReadAllLines(filePath);
            double temp = logArray.Length;

            // Gridview.
            ViewUpdate(logArray);
            string eqType = cmbEQSelect.Text;

            switch (eqType)
            {
                case "COG":
                    for (int i = cmbLogVerSelect.SelectedIndex == 0 ? 0 : 1; i < logArray.Length; i++)
                    {
                        prgCount = i / temp;
                        prgCount = prgCount * 100;
                        if (prgCount > prgStatus.Value)
                        {
                            prgStatus.PerformStep();
                        }

                        string[] logLineArray = logArray[i].Split(',');

                        // String check.
                        CharCheck(i, logLineArray);

                        // Length check.
                        LengthCheck(i, logLineArray);

                        // Batch id check.
                        BatchIDCheck(i, logLineArray);

                        string sCEID = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[3] : logLineArray[1];

                        switch (sCEID)
                        {
                            // Material Assemble CEID 215.
                            case "215":
                                COGAssembleCheck(i, logLineArray);
                                break;

                            // Material NG CEID 222.
                            case "222":
                                COGNGCheck(i, logLineArray);
                                break;

                            // Material Kitting Cancel CEID 219.
                            case "219":
                                COGCancelCheck(i, logLineArray);
                                break;

                            // Material Kitting CEID 221.
                            case "221":
                                COGKittngCheck(i, logLineArray);
                                break;

                            // Material supply CEID 225.
                            case "225":
                                COGSupplyCheck(i, logLineArray);
                                break;
                        }
                    }
                    break;

                case "FOG":
                    for (int i = cmbLogVerSelect.SelectedIndex == 0 ? 0 : 1; i < logArray.Length; i++)
                    {
                        prgCount = i / temp;
                        prgCount = prgCount * 100;
                        if (prgCount > prgStatus.Value)
                        {
                            prgStatus.PerformStep();
                        }
                        string[] logLineArray = logArray[i].Split(',');

                        // String check.
                        CharCheck(i, logLineArray);

                        // Length check.
                        LengthCheck(i, logLineArray);

                        // Batch id check.
                        BatchIDCheck(i, logLineArray);

                        string sCEID = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[3] : logLineArray[1];

                        switch (sCEID)
                        {
                            // Material Assemble CEID 215.
                            case "215":
                                FOGAssembleCheck(i, logLineArray);
                                break;

                            // Material NG CEID 222.
                            case "222":
                                FOGNGCheck(i, logLineArray);
                                break;

                            // Material Kitting Cancel CEID 219.
                            case "219":
                                FOGCancelCheck(i, logLineArray);
                                break;

                            // Material Kitting CEID 221.
                            case "221":
                                FOGKittngCheck(i, logLineArray);
                                break;

                            // Material 공급완료 CEID 225.
                            case "225":
                                FOGSupplyCheck(i, logLineArray);
                                break;
                        }
                    }
                    break;

                default:
                    MessageBox.Show("설비 유형이 존재하지 않습니다.\r\n 예시) COG or FOG", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    break;

            }
            txtReuslt.Text = resultText.ToString();
            this.Enabled = true;
            bindingSource1.DataSource = adgvFilter.DataSource;
            bindingSource1.Filter = "";
        }

        // Special characters check.
        private void CharCheck(int line, string[] logLineArray)
        {
            for (int i = cmbLogVerSelect.SelectedIndex == 0 ? 0 : 1; i < logLineArray.Length; i++)
            {
                foreach (char temp in logLineArray[i].ToString())
                {
                    Regex engRegex = new Regex(@"[a-zA-Z]");
                    Boolean ismatch = engRegex.IsMatch(temp.ToString());
                    if (Char.IsNumber(temp))
                    {
                        continue;
                    }
                    else if (ismatch)
                    {
                        continue;
                    }
                    else if (temp == '_' || temp == '-' || temp == ':' || temp == '.' || temp == ' ' || temp.ToString() == string.Empty)
                    {
                        continue;
                    }
                    else
                    {
                        IssueWrite(line + 1, string.Format("{0} 항목에 특수문자가 포함되어 있습니다.", logInfo[i].ToString().Replace("logMsg.s", "")));
                        ViewIssueColor(line, i);
                        continue;
                    }
                }
            }
        }

        // length check.
        private void LengthCheck(int line, string[] logLineArray)
        {
            int logver = cmbLogVerSelect.SelectedIndex;
            string sCEID = string.Empty;
            string materialType = string.Empty;
            int materialIDLength = 30;

            switch (logver)
            {
                case 0:
                    sCEID = logLineArray[3];
                    materialType = logLineArray[12];

                    if(materialType == "FRAME_COF")
                    {
                        materialIDLength = 15;
                    }

                    switch (sCEID)
                    {
                        case "215":
                            if (logLineArray[4].Length != 16)
                            {
                                IssueWrite(line + 1, "CELL ID 길이가 맞지 않습니다. 16자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요",materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            if (logLineArray[6].Length != 20)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 20자 수정필요");
                                ViewIssueColor(line, 6);
                            }
                            if (logLineArray[7].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 7);
                            }
                            break;

                        case "219":
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            if (logLineArray[6].Length != 20)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 20자 수정필요");
                                ViewIssueColor(line, 6);
                            }
                            if (logLineArray[7].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 7);
                            }
                            break;
                        case "221":
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            if (logLineArray[6].Length != 20)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 20자 수정필요");
                                ViewIssueColor(line, 6);
                            }
                            if (logLineArray[7].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 7);
                            }
                            break;
                        case "222":
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            if (logLineArray[6].Length != 20)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 20자 수정필요");
                                ViewIssueColor(line, 6);
                            }
                            if (logLineArray[7].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 7);
                            }
                            break;
                        case "225":
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            if (logLineArray[6].Length != 20)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 20자 수정필요");
                                ViewIssueColor(line, 6);
                            }
                            if (logLineArray[7].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 7);
                            }
                            break;
                    }
                    break;

                case 1:
                    sCEID = logLineArray[1];
                    materialType = logLineArray[6];

                    if (materialType == "FRAME_COF")
                    {
                        materialIDLength = 15;
                    }

                    switch (sCEID)
                    {
                        case "215":
                            if (logLineArray[2].Length != 16)
                            {
                                IssueWrite(line + 1, "CELL ID 길이가 맞지 않습니다. 16자 수정필요");
                                ViewIssueColor(line, 2);
                            }
                            if (logLineArray[3].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 3);
                            }
                            if (logLineArray[4].Length != 10)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 10자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            break;

                        case "219":
                            if (logLineArray[3].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 3);
                            }
                            if (logLineArray[4].Length != 10)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 10자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            break;
                        case "221":
                            if (logLineArray[3].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 3);
                            }
                            if (logLineArray[4].Length != 10)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 10자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            break;
                        case "222":
                            if (logLineArray[3].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 3);
                            }
                            if (logLineArray[4].Length != 10)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 10자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            break;
                        case "225":
                            if (logLineArray[3].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("BATCH ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 3);
                            }
                            if (logLineArray[4].Length != 10)
                            {
                                IssueWrite(line + 1, "BATCH NAME 길이가 맞지 않습니다. 10자 수정필요");
                                ViewIssueColor(line, 4);
                            }
                            if (logLineArray[5].Length != materialIDLength)
                            {
                                IssueWrite(line + 1, string.Format("Material ID 길이가 맞지 않습니다. {0}자 수정필요", materialIDLength));
                                ViewIssueColor(line, 5);
                            }
                            break;
                    }
                    break;
            }
        }

        // MT Batch ID == MT ID , MT Bactch Name.
        private void BatchIDCheck(int line, string[] logLineArray)
        {
            int logver = cmbLogVerSelect.SelectedIndex;
            string strMTBatchID = string.Empty;
            string strMTBatchName = string.Empty;
            string strMTID = string.Empty;

            switch (logver)
            {
                case 0:
                    strMTBatchID = logLineArray[5];
                    strMTBatchName = logLineArray[6];
                    strMTID = logLineArray[7];
                    if (!strMTBatchID.Equals(strMTID))
                    {
                        IssueWrite(line + 1, "Material_Batch_ID와 Material_ID 가 일치하지 않습니다.");
                        ViewIssueColor(line, 5);
                        ViewIssueColor(line, 7);
                    }
                    if (strMTBatchID.Length > 20)
                    {
                        string strMTBatchNameTemp = strMTBatchID.Remove(20);
                        if (!strMTBatchName.Equals(strMTBatchNameTemp))
                        {
                            IssueWrite(line + 1, "Material_Batch_Name 값 이상");
                            ViewIssueColor(line, 6);
                        }
                    }
                    break;

                case 1:
                    strMTBatchID = logLineArray[3];
                    strMTBatchName = logLineArray[4];
                    strMTID = logLineArray[5];
                    if (!strMTBatchID.Equals(strMTID))
                    {
                        IssueWrite(line + 1, "Material_Batch_ID와 Material_ID 가 일치하지 않습니다.");
                        ViewIssueColor(line, 3);
                        ViewIssueColor(line, 5);
                    }
                    if (strMTBatchID.Length > 20)
                    {
                        string strMTBatchNameTemp = strMTBatchID.Remove(10);
                        if (!strMTBatchName.Equals(strMTBatchNameTemp))
                        {
                            IssueWrite(line + 1, "Material_Batch_Name 값 이상");
                            ViewIssueColor(line, 4);
                        }
                    }
                    break;
            }
        }

        #region COG

        // FRAME_COF Check
        private void cmbEQSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbEQSelect.SelectedIndex == 0)
            {
                chkFrameCOFCheck.Enabled = true;
            }
            else
            {
                chkFrameCOFCheck.Enabled = false;
            }
        }

        // Material Assemble CEID 215.
        private void COGAssembleCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF") && !materialType.Equals("FRAME_COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            switch (materialType)
            {
                // Material ACF.
                case "ACF":

                    if (cmbLogVerSelect.SelectedIndex == 0)
                    {
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material ID change check.
                                if (materialPort1ACF.materialID != tempMaterialID)
                                {
                                    materialPort1ACF.setCheck = false;
                                }
                                if (cmbLogVerSelect.SelectedIndex == 0)
                                {
                                    if (materialPort1ACF.setCheck == true)
                                    {
                                        int tempTotalQty = int.Parse(logLineArray[11]);
                                        int tempUseQty = int.Parse(logLineArray[13]);
                                        int tempAssembleQty = int.Parse(logLineArray[15]);
                                        int tempTotalNGQty = int.Parse(logLineArray[16]);
                                        int temppUseQty = int.Parse(logLineArray[18]);
                                        int temppNGQty = int.Parse(logLineArray[19]);
                                        int temppAssembleQty = int.Parse(logLineArray[20]);
                                        int tempRemainQty = int.Parse(logLineArray[21]);

                                        // Material not rekitting => hold.
                                        if (tempTotalQty != materialPort1ACF.totalQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                            ViewIssueColor(line, 11);
                                        }

                                        bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;

                                        // Add 1.
                                        if (flagUseQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                            ViewIssueColor(line, 13);
                                        }

                                        bool flagAssembleQty = (tempAssembleQty - materialPort1ACF.assembleQty) != 1 ? true : false;

                                        // Add 1.
                                        if (flagAssembleQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                            ViewIssueColor(line, 15);
                                        }

                                        bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 0 ? true : false;

                                        // Not change 0.
                                        if (flagTotalNGQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                            ViewIssueColor(line, 16);
                                        }

                                        // 1.
                                        if (temppUseQty != 1)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 p_Use Count 이상");
                                            ViewIssueColor(line, 18);
                                        }

                                        if (temppNGQty != 0)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 215보고간 NG Count");
                                            ViewIssueColor(line, 19);
                                        }

                                        // 1.
                                        if (temppAssembleQty != 1)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 이상");
                                            ViewIssueColor(line, 20);
                                        }

                                        bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;

                                        // 1.
                                        if (flagRemainQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                            ViewIssueColor(line, 21);
                                        }
                                    }
                                    materialPort1ACF.materialID = logLineArray[7];
                                    materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                                    materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                                    materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                                    materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                                    materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                                    materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                                    materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                    materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                                    materialPort1ACF.setCheck = true;
                                }
                            }
                        }
                    }
                    else if (cmbLogVerSelect.SelectedIndex == 1)
                    {
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort1ACF.materialID != tempMaterialID)
                                {
                                    materialPort1ACF.setCheck = false;
                                }
                                if (cmbLogVerSelect.SelectedIndex == 0)
                                {
                                    if (materialPort1ACF.setCheck == true)
                                    {
                                        int tempTotalQty = int.Parse(logLineArray[10]);
                                        int tempUseQty = int.Parse(logLineArray[11]);
                                        int tempAssembleQty = int.Parse(logLineArray[12]);
                                        int tempTotalNGQty = int.Parse(logLineArray[13]);
                                        int temppUseQty = int.Parse(logLineArray[16]);
                                        int temppNGQty = int.Parse(logLineArray[18]);
                                        int temppAssembleQty = int.Parse(logLineArray[17]);
                                        int tempRemainQty = int.Parse(logLineArray[14]);

                                        // Material not rekitting => hold.
                                        if (tempTotalQty != materialPort1ACF.totalQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                            ViewIssueColor(line, 10);
                                        }

                                        bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;

                                        // Add 1.
                                        if (flagUseQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                            ViewIssueColor(line, 11);
                                        }

                                        bool flagAssembleQty = (tempAssembleQty - materialPort1ACF.assembleQty) != 1 ? true : false;

                                        // Add 1.
                                        if (flagAssembleQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                            ViewIssueColor(line, 12);
                                        }

                                        bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 0 ? true : false;

                                        // Not change.
                                        if (flagTotalNGQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                            ViewIssueColor(line, 13);
                                        }

                                        // 1.
                                        if (temppUseQty != 1)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 p_Use Count 이상");
                                            ViewIssueColor(line, 15);
                                        }

                                        // 0.
                                        if (temppNGQty != 0)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 215보고간 NG Count");
                                            ViewIssueColor(line, 17);
                                        }

                                        // 1.
                                        if (temppAssembleQty != 1)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 이상");
                                            ViewIssueColor(line, 16);
                                        }

                                        // 1.
                                        bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                        if (flagRemainQty)
                                        {
                                            IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                            ViewIssueColor(line, 14);
                                        }
                                    }
                                    materialPort1ACF.materialID = logLineArray[5];
                                    materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                                    materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                                    materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                                    materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                                    materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                                    materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                                    materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                    materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                                    materialPort1ACF.setCheck = true;
                                }
                            }
                        }
                    }
                    break;

                // Material COF.
                case "COF":
                    if (cmbLogVerSelect.SelectedIndex == 0)
                    {
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];

                            // Material id change Logic pass.
                            if (materialPort1COF.materialID != tempMaterialID)
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);
                                int tempUseQty = int.Parse(logLineArray[13]);
                                int tempAssembleQty = int.Parse(logLineArray[15]);
                                int tempTotalNGQty = int.Parse(logLineArray[16]);
                                int temppUseQty = int.Parse(logLineArray[18]);
                                int temppNGQty = int.Parse(logLineArray[19]);
                                int temppAssembleQty = int.Parse(logLineArray[20]);
                                int tempRemainQty = int.Parse(logLineArray[21]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort1COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort1COF.assembleQty) != 1 ? true : false;

                                // Add 1.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 0 ? true : false;

                                // Not Change
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 0.
                                if (temppNGQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT1 215보고간 NG Count");
                                    ViewIssueColor(line, 19);
                                }

                                // 1.
                                if (temppAssembleQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 p_Assemble Count 이상");
                                    ViewIssueColor(line, 20);
                                }

                                // 1.
                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort1COF.materialID = logLineArray[7];
                            materialPort1COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort1COF.useQty = int.Parse(logLineArray[13]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];

                            // Material id change Logic pass.
                            if (materialPort2COF.materialID != tempMaterialID)
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);
                                int tempUseQty = int.Parse(logLineArray[13]);
                                int tempAssembleQty = int.Parse(logLineArray[15]);
                                int tempTotalNGQty = int.Parse(logLineArray[16]);
                                int temppUseQty = int.Parse(logLineArray[18]);
                                int temppNGQty = int.Parse(logLineArray[19]);
                                int temppAssembleQty = int.Parse(logLineArray[20]);
                                int tempRemainQty = int.Parse(logLineArray[21]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort2COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort2COF.assembleQty) != 1 ? true : false;

                                // Add 1.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 0 ? true : false;

                                // Not change.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 0.
                                if (temppNGQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT2 215보고간 NG Count");
                                    ViewIssueColor(line, 19);
                                }

                                // 1.
                                if (temppAssembleQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 p_Assemble Count 이상");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 1 ? true : false;
                                
                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort2COF.materialID = logLineArray[7];
                            materialPort2COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort2COF.useQty = int.Parse(logLineArray[13]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort2COF.setCheck = true;
                        }
                    }
                    else if (cmbLogVerSelect.SelectedIndex == 1)
                    {
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];

                            // Material id change Logic pass.
                            if (materialPort1COF.materialID != tempMaterialID)
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);
                                int tempUseQty = int.Parse(logLineArray[11]);
                                int tempAssembleQty = int.Parse(logLineArray[12]);
                                int tempTotalNGQty = int.Parse(logLineArray[13]);
                                int temppUseQty = int.Parse(logLineArray[16]);
                                int temppNGQty = int.Parse(logLineArray[18]);
                                int temppAssembleQty = int.Parse(logLineArray[17]);
                                int tempRemainQty = int.Parse(logLineArray[14]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort1COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort1COF.assembleQty) != 1 ? true : false;

                                // Add 1.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 0 ? true : false;
                                
                                // Not change.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 13);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 0.
                                if (temppNGQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT1 215보고간 NG Count");
                                    ViewIssueColor(line, 18);
                                }

                                // 1.
                                if (temppAssembleQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 p_Assemble Count 이상");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort1COF.materialID = logLineArray[5];
                            materialPort1COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort1COF.useQty = int.Parse(logLineArray[11]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];

                            // Material id change Logic pass.
                            if (materialPort2COF.materialID != tempMaterialID)
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);
                                int tempUseQty = int.Parse(logLineArray[11]);
                                int tempAssembleQty = int.Parse(logLineArray[12]);
                                int tempTotalNGQty = int.Parse(logLineArray[13]);
                                int temppUseQty = int.Parse(logLineArray[16]);
                                int temppNGQty = int.Parse(logLineArray[18]);
                                int temppAssembleQty = int.Parse(logLineArray[17]);
                                int tempRemainQty = int.Parse(logLineArray[14]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort2COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort2COF.assembleQty) != 1 ? true : false;

                                // Add 1.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 0 ? true : false;
                                
                                // Not change.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 13);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 0.
                                if (temppNGQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT2 215보고간 NG Count");
                                    ViewIssueColor(line, 18);
                                }

                                // 1.
                                if (temppAssembleQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 p_Assemble Count 이상");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort2COF.materialID = logLineArray[5];
                            materialPort2COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort2COF.useQty = int.Parse(logLineArray[11]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort2COF.setCheck = true;
                        }
                    }
                    break;

                // Material FRAME_COF.
                case "FRAME_COF":
                    if (cmbLogVerSelect.SelectedIndex == 0)
                    {
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material id change Logic pass.
                                if (materialPort1FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);
                                    int tempUseQty = int.Parse(logLineArray[13]);
                                    int tempAssembleQty = int.Parse(logLineArray[15]);
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);
                                    int temppUseQty = int.Parse(logLineArray[18]);
                                    int temppNGQty = int.Parse(logLineArray[19]);
                                    int temppAssembleQty = int.Parse(logLineArray[20]);
                                    int tempRemainQty = int.Parse(logLineArray[21]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort1FrameCOF.assembleQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 0 ? true : false;

                                    // Not Change
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 0.
                                    if (temppNGQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 215보고간 NG Count");
                                        ViewIssueColor(line, 19);
                                    }

                                    // 1.
                                    if (temppAssembleQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 p_Assemble Count 이상");
                                        ViewIssueColor(line, 20);
                                    }

                                    // 1.
                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }

                                materialPort1FrameCOF.materialID = logLineArray[7];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material id change Logic pass.
                                if (materialPort2FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);
                                    int tempUseQty = int.Parse(logLineArray[13]);
                                    int tempAssembleQty = int.Parse(logLineArray[15]);
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);
                                    int temppUseQty = int.Parse(logLineArray[18]);
                                    int temppNGQty = int.Parse(logLineArray[19]);
                                    int temppAssembleQty = int.Parse(logLineArray[20]);
                                    int tempRemainQty = int.Parse(logLineArray[21]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort2FrameCOF.assembleQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 0 ? true : false;

                                    // Not change.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 0.
                                    if (temppNGQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 215보고간 NG Count");
                                        ViewIssueColor(line, 19);
                                    }

                                    // 1.
                                    if (temppAssembleQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 p_Assemble Count 이상");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2FrameCOF.materialID = logLineArray[7];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                        else if (cmbLogVerSelect.SelectedIndex == 1)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort1FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);
                                    int tempUseQty = int.Parse(logLineArray[11]);
                                    int tempAssembleQty = int.Parse(logLineArray[12]);
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);
                                    int temppUseQty = int.Parse(logLineArray[16]);
                                    int temppNGQty = int.Parse(logLineArray[18]);
                                    int temppAssembleQty = int.Parse(logLineArray[17]);
                                    int tempRemainQty = int.Parse(logLineArray[14]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort1FrameCOF.assembleQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 0 ? true : false;

                                    // Not change.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 0.
                                    if (temppNGQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 215보고간 NG Count");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 1.
                                    if (temppAssembleQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 p_Assemble Count 이상");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }

                                materialPort1FrameCOF.materialID = logLineArray[5];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort2FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);
                                    int tempUseQty = int.Parse(logLineArray[11]);
                                    int tempAssembleQty = int.Parse(logLineArray[12]);
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);
                                    int temppUseQty = int.Parse(logLineArray[16]);
                                    int temppNGQty = int.Parse(logLineArray[18]);
                                    int temppAssembleQty = int.Parse(logLineArray[17]);
                                    int tempRemainQty = int.Parse(logLineArray[14]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort2FrameCOF.assembleQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 0 ? true : false;

                                    // Not change.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 0.
                                    if (temppNGQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 215보고간 NG Count");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 1.
                                    if (temppAssembleQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 p_Assemble Count 이상");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2FrameCOF.materialID = logLineArray[5];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                    }
                    break;
            }
        }

        // Material NG CEID 222.
        private void COGNGCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF") && !materialType.Equals("FRAME_COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF.
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material id change Logic pass.
                                if (materialPort1ACF.materialID != tempMaterialID)
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);
                                    int tempUseQty = int.Parse(logLineArray[13]);
                                    int tempAssembleQty = int.Parse(logLineArray[15]);
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);
                                    int temppUseQty = int.Parse(logLineArray[18]);
                                    int temppNGQty = int.Parse(logLineArray[19]);
                                    int temppAssembleQty = int.Parse(logLineArray[20]);
                                    int tempRemainQty = int.Parse(logLineArray[21]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1ACF.totalQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    
                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    
                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 19);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[7];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material COF.
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];

                            // Material id change Logic pass.
                            if (materialPort1COF.materialID != tempMaterialID)
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);
                                int tempUseQty = int.Parse(logLineArray[13]);
                                int tempAssembleQty = int.Parse(logLineArray[15]);
                                int tempTotalNGQty = int.Parse(logLineArray[16]);
                                int temppUseQty = int.Parse(logLineArray[18]);
                                int temppNGQty = int.Parse(logLineArray[19]);
                                int temppAssembleQty = int.Parse(logLineArray[20]);
                                int tempRemainQty = int.Parse(logLineArray[21]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort1COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1COF.assembleQty ? true : false;

                                // Not change.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 1 ? true : false;

                                // Add 1.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 222보고간 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 1.
                                if (temppNGQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 NG Count 이상");
                                    ViewIssueColor(line, 19);
                                }

                                // 0.
                                if (temppAssembleQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT1 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort1COF.materialID = logLineArray[7];
                            materialPort1COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort1COF.useQty = int.Parse(logLineArray[13]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];

                            // Material id change Logic pass.
                            if (materialPort2COF.materialID != tempMaterialID)
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);
                                int tempUseQty = int.Parse(logLineArray[13]);
                                int tempAssembleQty = int.Parse(logLineArray[15]);
                                int tempTotalNGQty = int.Parse(logLineArray[16]);
                                int temppUseQty = int.Parse(logLineArray[18]);
                                int temppNGQty = int.Parse(logLineArray[19]);
                                int temppAssembleQty = int.Parse(logLineArray[20]);
                                int tempRemainQty = int.Parse(logLineArray[21]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort2COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2COF.assembleQty ? true : false;

                                // Not change.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 1 ? true : false;

                                // Add 1.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 222보고간 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 1.
                                if (temppNGQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 NG Count 이상");
                                    ViewIssueColor(line, 19);
                                }

                                // 0.
                                if (temppAssembleQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT2 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort2COF.materialID = logLineArray[7];
                            materialPort2COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort2COF.useQty = int.Parse(logLineArray[13]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort2COF.setCheck = true;
                        }
                        break;

                    // Material FRAME_COF.
                    case "FRAME_COF":

                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material id change Logic pass.
                                if (materialPort1FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);
                                    int tempUseQty = int.Parse(logLineArray[13]);
                                    int tempAssembleQty = int.Parse(logLineArray[15]);
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);
                                    int temppUseQty = int.Parse(logLineArray[18]);
                                    int temppNGQty = int.Parse(logLineArray[19]);
                                    int temppAssembleQty = int.Parse(logLineArray[20]);
                                    int tempRemainQty = int.Parse(logLineArray[21]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1FrameCOF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 19);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }

                                materialPort1FrameCOF.materialID = logLineArray[7];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];

                                // Material id change Logic pass.
                                if (materialPort2FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);
                                    int tempUseQty = int.Parse(logLineArray[13]);
                                    int tempAssembleQty = int.Parse(logLineArray[15]);
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);
                                    int temppUseQty = int.Parse(logLineArray[18]);
                                    int temppNGQty = int.Parse(logLineArray[19]);
                                    int temppAssembleQty = int.Parse(logLineArray[20]);
                                    int tempRemainQty = int.Parse(logLineArray[21]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2FrameCOF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 NG Count 이상");
                                        ViewIssueColor(line, 19);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2FrameCOF.materialID = logLineArray[7];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF.
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort1ACF.materialID != tempMaterialID)
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);
                                    int tempUseQty = int.Parse(logLineArray[11]);
                                    int tempAssembleQty = int.Parse(logLineArray[12]);
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);
                                    int temppUseQty = int.Parse(logLineArray[16]);
                                    int temppNGQty = int.Parse(logLineArray[18]);
                                    int temppAssembleQty = int.Parse(logLineArray[17]);
                                    int tempRemainQty = int.Parse(logLineArray[14]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1ACF.totalQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[5];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material COF.
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];

                            // Material id change Logic pass.
                            if (materialPort1COF.materialID != tempMaterialID)
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);
                                int tempUseQty = int.Parse(logLineArray[11]);
                                int tempAssembleQty = int.Parse(logLineArray[12]);
                                int tempTotalNGQty = int.Parse(logLineArray[13]);
                                int temppUseQty = int.Parse(logLineArray[16]);
                                int temppNGQty = int.Parse(logLineArray[18]);
                                int temppAssembleQty = int.Parse(logLineArray[17]);
                                int tempRemainQty = int.Parse(logLineArray[14]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort1COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1COF.assembleQty ? true : false;

                                // Not change.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 1 ? true : false;

                                // Add 1.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 222보고간 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppNGQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT1 NG Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 0.
                                if (temppAssembleQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT1 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort1COF.materialID = logLineArray[5];
                            materialPort1COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort1COF.useQty = int.Parse(logLineArray[11]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];

                            // Material id change Logic pass.
                            if (materialPort2COF.materialID != tempMaterialID)
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);
                                int tempUseQty = int.Parse(logLineArray[11]);
                                int tempAssembleQty = int.Parse(logLineArray[12]);
                                int tempTotalNGQty = int.Parse(logLineArray[13]);
                                int temppUseQty = int.Parse(logLineArray[16]);
                                int temppNGQty = int.Parse(logLineArray[18]);
                                int temppAssembleQty = int.Parse(logLineArray[17]);
                                int tempRemainQty = int.Parse(logLineArray[14]);

                                // Material not rekitting => hold.
                                if (tempTotalQty != materialPort2COF.totalQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;

                                // Add 1.
                                if (flagUseQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2COF.assembleQty ? true : false;

                                // Not change.
                                if (flagAssembleQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 1 ? true : false;

                                // Add 1.
                                if (flagTotalNGQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                // 1.
                                if (temppUseQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 222보고간 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                // 1.
                                if (temppNGQty != 1)
                                {
                                    IssueWrite(line + 1, "COF PORT2 NG Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                // 0.
                                if (temppAssembleQty != 0)
                                {
                                    IssueWrite(line + 1, "COF PORT2 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 1 ? true : false;

                                // 1.
                                if (flagRemainQty)
                                {
                                    IssueWrite(line + 1, "COF PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort2COF.materialID = logLineArray[5];
                            materialPort2COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort2COF.useQty = int.Parse(logLineArray[11]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort2COF.setCheck = true;
                        }
                        break;


                    // Material FRAME_COF.
                    case "FRAME_COF":
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {

                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort1FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);
                                    int tempUseQty = int.Parse(logLineArray[11]);
                                    int tempAssembleQty = int.Parse(logLineArray[12]);
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);
                                    int temppUseQty = int.Parse(logLineArray[16]);
                                    int temppNGQty = int.Parse(logLineArray[18]);
                                    int temppAssembleQty = int.Parse(logLineArray[17]);
                                    int tempRemainQty = int.Parse(logLineArray[14]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1FrameCOF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }

                                materialPort1FrameCOF.materialID = logLineArray[5];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];

                                // Material id change Logic pass.
                                if (materialPort2FrameCOF.materialID != tempMaterialID)
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);
                                    int tempUseQty = int.Parse(logLineArray[11]);
                                    int tempAssembleQty = int.Parse(logLineArray[12]);
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);
                                    int temppUseQty = int.Parse(logLineArray[16]);
                                    int temppNGQty = int.Parse(logLineArray[18]);
                                    int temppAssembleQty = int.Parse(logLineArray[17]);
                                    int tempRemainQty = int.Parse(logLineArray[14]);

                                    // Material not rekitting => hold.
                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagUseQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2FrameCOF.assembleQty ? true : false;

                                    // Not change.
                                    if (flagAssembleQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 1 ? true : false;

                                    // Add 1.
                                    if (flagTotalNGQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    // 1.
                                    if (temppUseQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    // 1.
                                    if (temppNGQty != 1)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 NG Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    // 0.
                                    if (temppAssembleQty != 0)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 1 ? true : false;

                                    // 1.
                                    if (flagRemainQty)
                                    {
                                        IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2FrameCOF.materialID = logLineArray[5];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                        break;
                }
            }
        }

        // Material Kitting Cancel CEID 219.
        private void COGCancelCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF") && !materialType.Equals("FRAME_COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF.
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1ACF.setCheck = false;
                            }
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1COF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                if (tempTotalQty != materialPort1COF.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 TOTAL Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 USE Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1COF.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 Assemble Count 변경");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 NG Count 변경");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 p_Assemble 변경");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 Remain Count 변경");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort1COF.setCheck = false;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2COF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                if (tempTotalQty != materialPort2COF.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 TOTAL Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 USE Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2COF.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 Assemble Count 변경");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 NG Count 변경");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 p_Assemble 변경");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 Remain Count 변경");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort2COF.setCheck = false;
                        }
                        break;
                    // Material FRAME_COF
                    case "FRAME_COF":
                        
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1FrameCOF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1FrameCOF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1FrameCOF.setCheck = false;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2FrameCOF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 TOTAL Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 USE Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2FrameCOF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 Assemble Count 변경");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 p_Use Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 NG Count 변경");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 p_Assemble 변경");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 Remain Count 변경");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2FrameCOF.setCheck = false;
                            }
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1ACF.setCheck = false;
                            }
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1COF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1COF.setCheck = false;
                            }

                            if (materialPort1COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                if (tempTotalQty != materialPort1COF.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 TOTAL Count 변경");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1COF.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 USE Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1COF.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 Assemble Count 변경");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1COF.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 NG Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 p_Assemble 변경");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1COF.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT1 Remain Count 변경");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort1COF.setCheck = false;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2COF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2COF.setCheck = false;
                            }

                            if (materialPort2COF.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                if (tempTotalQty != materialPort2COF.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 TOTAL Count 변경");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2COF.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 USE Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2COF.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 Assemble Count 변경");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2COF.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 NG Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 p_Assemble 변경");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2COF.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel COF PORT2 Remain Count 변경");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort2COF.setCheck = false;
                        }
                        break;

                    // Material FRAME_COF
                    case "FRAME_COF":
                                                
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1FrameCOF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1FrameCOF.setCheck = false;
                                }

                                if (materialPort1FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                    if (tempTotalQty != materialPort1FrameCOF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1FrameCOF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1FrameCOF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1FrameCOF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1FrameCOF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1FrameCOF.setCheck = false;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2FrameCOF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2FrameCOF.setCheck = false;
                                }

                                if (materialPort2FrameCOF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                    if (tempTotalQty != materialPort2FrameCOF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 TOTAL Count 변경");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2FrameCOF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 USE Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2FrameCOF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 Assemble Count 변경");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2FrameCOF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 p_Use Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 NG Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 p_Assemble 변경");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2FrameCOF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel FRAME_COF PORT2 Remain Count 변경");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2FrameCOF.setCheck = false;
                            }
                        }
                        break;
                }
            }
        }

        // Material Kitting CEID 221
        private void COGKittngCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF") && !materialType.Equals("FRAME_COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 7);
                                }
                            }
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1COF.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "COF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 7);
                            }
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2COF.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "COF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 7);
                            }
                        }
                        break;

                    // Material FRAME_COF
                    case "FRAME_COF":
                                                
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1FrameCOF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 7);
                                }
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2FrameCOF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 7);
                                }
                            }
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 5);
                                }
                            }
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1COF.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "COF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 5);
                            }
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2COF.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "COF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 5);
                            }
                        }
                        break;

                    // Material FRAME_COF
                    case "FRAME_COF":
                                                                        
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1FrameCOF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 5);
                                }
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2FrameCOF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 5);
                                }
                            }
                        }
                        break;
                }
            }
        }

        // Material 공급완료 CEID 225
        private void COGSupplyCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF") && !materialType.Equals("FRAME_COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 15);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 20);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort1ACF.materialID = logLineArray[7];
                            materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                            materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                            materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                            materialPort1ACF.setCheck = true;
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "COF PORT1 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 15);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 19);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 20);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "COF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 21);
                            }
                            materialPort1COF.materialID = logLineArray[7];
                            materialPort1COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort1COF.useQty = int.Parse(logLineArray[13]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "COF PORT2 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 15);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 19);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 20);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "COF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 21);
                            }
                            materialPort2COF.materialID = logLineArray[7];
                            materialPort2COF.totalQty = int.Parse(logLineArray[11]);
                            materialPort2COF.useQty = int.Parse(logLineArray[13]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[21]);
                            materialPort2COF.setCheck = true;
                        }
                        break;

                    // Material FRAME_COF
                    case "FRAME_COF":
                                                                                                
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 15);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 20);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 21);
                                }
                                materialPort1FrameCOF.materialID = logLineArray[7];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 15);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 20);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 21);
                                }
                                materialPort2FrameCOF.materialID = logLineArray[7];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 10);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 12);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 17);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort1ACF.materialID = logLineArray[5];
                            materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                            materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                            materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                            materialPort1ACF.setCheck = true;
                        }
                        break;

                    // Material COF
                    case "COF":
                        if (materialPort == "1")
                        {
                            int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "COF PORT1 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 10);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 12);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "COF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 17);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "COF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 14);
                            }
                            materialPort1COF.materialID = logLineArray[5];
                            materialPort1COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort1COF.useQty = int.Parse(logLineArray[11]);
                            materialPort1COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort1COF.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "COF PORT2 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 10);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 12);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "COF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 17);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "COF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 14);
                            }
                            materialPort2COF.materialID = logLineArray[5];
                            materialPort2COF.totalQty = int.Parse(logLineArray[10]);
                            materialPort2COF.useQty = int.Parse(logLineArray[11]);
                            materialPort2COF.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2COF.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2COF.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2COF.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2COF.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2COF.remainQty = int.Parse(logLineArray[14]);
                            materialPort2COF.setCheck = true;
                        }
                        break;

                    // Material FRAME_COF
                    case "FRAME_COF":
                                                                                                                        
                        bool checkFrameCOF = chkFrameCOFCheck.Checked;

                        if (checkFrameCOF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 10);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 12);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 17);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 14);
                                }
                                materialPort1FrameCOF.materialID = logLineArray[5];
                                materialPort1FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort1FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1FrameCOF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 10);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 12);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 17);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "FRAME_COF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 14);
                                }
                                materialPort2FrameCOF.materialID = logLineArray[5];
                                materialPort2FrameCOF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2FrameCOF.useQty = int.Parse(logLineArray[11]);
                                materialPort2FrameCOF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2FrameCOF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2FrameCOF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2FrameCOF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2FrameCOF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2FrameCOF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2FrameCOF.setCheck = true;
                            }
                        }
                        break;
                }
            }
        }
        #endregion

        #region FOG
        // Material Assemble CEID 215
        private void FOGAssembleCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("FPC"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 1씩 증가
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 1
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort1ACF.assembleQty) != 1 ? true : false;
                                    if (flagAssembleQty)                                                                // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 0 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 215보고간 NG Count");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 1)                                                          // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 이상");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[7];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 1씩 증가
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 1
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort2ACF.assembleQty) != 1 ? true : false;
                                    if (flagAssembleQty)                                                                // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 0 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 215보고간 NG Count");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 1)                                                          // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 p_Assemble Count 이상");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2ACF.materialID = logLineArray[7];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 1씩 증가
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 1
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort1FPC.assembleQty) != 1 ? true : false;
                                if (flagAssembleQty)                                                                // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 0 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT1 215보고간 NG Count");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 1)                                                          // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 p_Assemble Count 이상");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort1FPC.materialID = logLineArray[7];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort1FPC.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 1씩 증가
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 1
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort2FPC.assembleQty) != 1 ? true : false;
                                if (flagAssembleQty)                                                                // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 0 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 p_Use Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT2 215보고간 NG Count");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 1)                                                          // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 p_Assemble Count 이상");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort2FPC.materialID = logLineArray[7];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 1씩 증가
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 1
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort1ACF.assembleQty) != 1 ? true : false;
                                    if (flagAssembleQty)                                                                // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 0 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 215보고간 NG Count");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 1)                                                          // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 이상");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[5];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 1씩 증가
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 1
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = (tempAssembleQty - materialPort2ACF.assembleQty) != 1 ? true : false;
                                    if (flagAssembleQty)                                                                // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 0 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 215보고간 NG TOTAL Count 수량 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 215보고간 NG Count");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 1)                                                          // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 p_Assemble Count 이상");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2ACF.materialID = logLineArray[5];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 1씩 증가
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 1
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort1FPC.assembleQty) != 1 ? true : false;
                                if (flagAssembleQty)                                                                // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 0 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT1 215보고간 NG Count");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 1)                                                          // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 p_Assemble Count 이상");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort1FPC.materialID = logLineArray[5];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort1FPC.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 1씩 증가
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 1
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = (tempAssembleQty - materialPort2FPC.assembleQty) != 1 ? true : false;
                                if (flagAssembleQty)                                                                // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 0 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 215보고간 NG TOTAL Count 수량 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 p_Use Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT2 215보고간 NG Count");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 1)                                                          // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 p_Assemble Count 이상");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort2FPC.materialID = logLineArray[5];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
        }

        // Material NG CEID 222
        private void FOGNGCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("FPC"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }
            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 1씩 증가
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 1
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 1)                                                                // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[7];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 1씩 증가
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 1
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 1)                                                                // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 NG Count 이상");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2ACF.materialID = logLineArray[7];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2ACF.setCheck = true;
                            }

                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 1씩 증가
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 1
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 222보고간 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 1)                                                                // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 NG Count 이상");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT1 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort1FPC.materialID = logLineArray[7];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort1FPC.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 1씩 증가
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 1
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 USE Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 222보고간 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 1)                                                                // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 NG Count 이상");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT2 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 21);
                                }
                            }

                            materialPort2FPC.materialID = logLineArray[7];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 1씩 증가
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 1
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 1)                                                                // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 NG Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT1 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1ACF.materialID = logLineArray[5];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 1씩 증가
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 1
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 TOTAL Count 이상");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 USE Count 이상");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Assemble Count 이상");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 1씩 증가
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 NG TOTAL Count 이상");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 1)                                                               // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 222보고간 p_Use Count 이상");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 1)                                                                // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 NG Count 이상");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 222보고간 p_Assemble Count");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 1 ? true : false;
                                    if (flagRemainQty)                                                                  // 1
                                    {
                                        IssueWrite(line + 1, "ACF PORT2 Remain Count 이상");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2ACF.materialID = logLineArray[5];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2ACF.setCheck = true;
                            }

                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 1씩 증가
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 1
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT1 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 222보고간 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 1)                                                                // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 NG Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT1 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT1 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort1FPC.materialID = logLineArray[5];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort1FPC.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material 재 Kitting 아닌이상 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 1씩 증가
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 1씩 증가
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 1
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 1
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 1씩 감소

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // Material 재 Kitting 아닌이상 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 TOTAL Count 이상");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 USE Count 이상");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Assemble Count 이상");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 1씩 증가
                                {
                                    IssueWrite(line + 1, "FPC PORT2 NG TOTAL Count 이상");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 1)                                                               // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 222보고간 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 1)                                                                // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 NG Count 이상");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "FPC PORT2 222보고간 p_Assemble Count");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 1 ? true : false;
                                if (flagRemainQty)                                                                  // 1
                                {
                                    IssueWrite(line + 1, "FPC PORT2 Remain Count 이상");
                                    ViewIssueColor(line, 14);
                                }
                            }

                            materialPort2FPC.materialID = logLineArray[5];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
        }

        // Material Kitting Cancel CEID 219
        private void FOGCancelCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("FPC"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }
            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort1ACF.setCheck = false;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 TOTAL Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 USE Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 Assemble Count 변경");
                                        ViewIssueColor(line, 15);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 p_Use Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 NG Count 변경");
                                        ViewIssueColor(line, 19);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 p_Assemble 변경");
                                        ViewIssueColor(line, 20);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 Remain Count 변경");
                                        ViewIssueColor(line, 21);
                                    }
                                }
                                materialPort2ACF.setCheck = false;
                            }

                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 TOTAL Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 USE Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 Assemble Count 변경");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 NG Count 변경");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 p_Assemble 변경");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 Remain Count 변경");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort1FPC.setCheck = false;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 고정

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 TOTAL Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 USE Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 Assemble Count 변경");
                                    ViewIssueColor(line, 15);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 p_Use Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 NG Count 변경");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 p_Assemble 변경");
                                    ViewIssueColor(line, 20);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 Remain Count 변경");
                                    ViewIssueColor(line, 21);
                                }
                            }
                            materialPort2FPC.setCheck = false;
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort1ACF.setCheck = false;
                                }

                                if (materialPort1ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                    if (tempTotalQty != materialPort1ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 TOTAL Count 변경");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort1ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 USE Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort1ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Assemble Count 변경");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort1ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Use Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 NG Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 p_Assemble 변경");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort1ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT1 Remain Count 변경");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort1ACF.setCheck = false;
                            }
                            else if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2ACF.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                                {
                                    materialPort2ACF.setCheck = false;
                                }

                                if (materialPort2ACF.setCheck == true)
                                {
                                    int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                    int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                    int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                    int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                    int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                    int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                    int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                    int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                    if (tempTotalQty != materialPort2ACF.totalQty)                                      // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 TOTAL Count 변경");
                                        ViewIssueColor(line, 10);
                                    }

                                    bool flagUseQty = (tempUseQty - materialPort2ACF.useQty) != 1 ? true : false;
                                    if (flagUseQty)                                                                     // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 USE Count 변경");
                                        ViewIssueColor(line, 11);
                                    }

                                    bool flagAssembleQty = tempAssembleQty != materialPort2ACF.assembleQty ? true : false;
                                    if (flagAssembleQty)                                                                // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 Assemble Count 변경");
                                        ViewIssueColor(line, 12);
                                    }

                                    bool flagTotalNGQty = (tempTotalNGQty - materialPort2ACF.totalNGQty) != 1 ? true : false;
                                    if (flagTotalNGQty)                                                                 // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 NG TOTAL Count 변경");
                                        ViewIssueColor(line, 13);
                                    }

                                    if (temppUseQty != 0)                                                               // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 p_Use Count 변경");
                                        ViewIssueColor(line, 16);
                                    }

                                    if (temppNGQty != 0)                                                                // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 NG Count 변경");
                                        ViewIssueColor(line, 18);
                                    }

                                    if (temppAssembleQty != 0)                                                          // 0
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 p_Assemble 변경");
                                        ViewIssueColor(line, 17);
                                    }

                                    bool flagRemainQty = (materialPort2ACF.remainQty - tempRemainQty) != 0 ? true : false;
                                    if (flagRemainQty)                                                                  // 고정
                                    {
                                        IssueWrite(line + 1, "Kitting Cancel ACF PORT2 Remain Count 변경");
                                        ViewIssueColor(line, 14);
                                    }
                                }
                                materialPort2ACF.setCheck = false;
                            }

                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort1FPC.setCheck = false;
                            }

                            if (materialPort1FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                if (tempTotalQty != materialPort1FPC.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 TOTAL Count 변경");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort1FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 USE Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort1FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 Assemble Count 변경");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort1FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 NG Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 p_Assemble 변경");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort1FPC.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT1 Remain Count 변경");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort1FPC.setCheck = false;
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2FPC.materialID != tempMaterialID)                                      // MaterialID 자재변경시 Logic 건너뜀
                            {
                                materialPort2FPC.setCheck = false;
                            }

                            if (materialPort2FPC.setCheck == true)
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // 고정
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 고정
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 고정
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 고정
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 고정

                                if (tempTotalQty != materialPort2FPC.totalQty)                                      // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 TOTAL Count 변경");
                                    ViewIssueColor(line, 10);
                                }

                                bool flagUseQty = (tempUseQty - materialPort2FPC.useQty) != 1 ? true : false;
                                if (flagUseQty)                                                                     // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 USE Count 변경");
                                    ViewIssueColor(line, 11);
                                }

                                bool flagAssembleQty = tempAssembleQty != materialPort2FPC.assembleQty ? true : false;
                                if (flagAssembleQty)                                                                // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 Assemble Count 변경");
                                    ViewIssueColor(line, 12);
                                }

                                bool flagTotalNGQty = (tempTotalNGQty - materialPort2FPC.totalNGQty) != 1 ? true : false;
                                if (flagTotalNGQty)                                                                 // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 NG TOTAL Count 변경");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 p_Use Count 변경");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 NG Count 변경");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 p_Assemble 변경");
                                    ViewIssueColor(line, 17);
                                }

                                bool flagRemainQty = (materialPort2FPC.remainQty - tempRemainQty) != 0 ? true : false;
                                if (flagRemainQty)                                                                  // 고정
                                {
                                    IssueWrite(line + 1, "Kitting Cancel FPC PORT2 Remain Count 변경");
                                    ViewIssueColor(line, 14);
                                }
                            }
                            materialPort2FPC.setCheck = false;
                        }
                        break;
                }
            }
        }

        // Material Kitting CEID 221
        private void FOGKittngCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("FPC"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort1ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 7);
                                }
                            }
                            if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[7];
                                if (materialPort2ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 7);
                                }
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort1FPC.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "FPC PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 7);
                            }
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[7];
                            if (materialPort2FPC.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "FPC PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 7);
                            }
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort1ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 5);
                                }
                            }
                            if (materialPort == "2")
                            {
                                string tempMaterialID = logLineArray[5];
                                if (materialPort2ACF.materialID == tempMaterialID)
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                    ViewIssueColor(line, 5);
                                }
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort1FPC.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "FPC PORT1 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 5);
                            }
                        }
                        else if (materialPort == "2")
                        {
                            string tempMaterialID = logLineArray[5];
                            if (materialPort2FPC.materialID == tempMaterialID)
                            {
                                IssueWrite(line + 1, "FPC PORT2 Cancel 된 Material ID와 Kitting 된 Material ID 중복");
                                ViewIssueColor(line, 5);
                            }
                        }
                        break;
                }
            }
        }

        // Material 공급완료 CEID 225
        private void FOGSupplyCheck(int line, string[] logLineArray)
        {
            string materialPort = logLineArray[8];
            if (!materialPort.Equals("1") && !materialPort.Equals("2"))
            {
                IssueWrite(line + 1, "Port 값 이상");
                ViewIssueColor(line, 8);
            }
            string materialType = cmbLogVerSelect.SelectedIndex == 0 ? logLineArray[12] : logLineArray[6];
            if (!materialType.Equals("ACF") && !materialType.Equals("COF"))
            {
                IssueWrite(line + 1, "Material Type 값 이상");
                int index = cmbLogVerSelect.SelectedIndex == 0 ? 12 : 6;
                ViewIssueColor(line, index);
            }

            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 15);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 20);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 21);
                                }
                                materialPort1ACF.materialID = logLineArray[7];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT2 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 15);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 19);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 20);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 21);
                                }
                                materialPort2ACF.materialID = logLineArray[7];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[15]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[19]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[20]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[21]);
                                materialPort2ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "FPC PORT1 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 15);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 19);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 20);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "FPC PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 21);
                            }
                            materialPort1FPC.materialID = logLineArray[7];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort1FPC.setCheck = true;
                        }
                        else if (materialPort == "2")
                        {
                            int tempTotalQty = int.Parse(logLineArray[11]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[13]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[15]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[16]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[18]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[19]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[20]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[21]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "FPC PORT2 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 15);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 19);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 20);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "FPC PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 21);
                            }
                            materialPort2FPC.materialID = logLineArray[7];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[15]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[19]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[20]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[21]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                switch (materialType)
                {
                    // Material ACF
                    case "ACF":
                        bool checkACF = chkACFCheck.Checked;

                        if (checkACF)
                        {
                            if (materialPort == "1")
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT1 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 10);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 12);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 17);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 14);
                                }
                                materialPort1ACF.materialID = logLineArray[5];
                                materialPort1ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort1ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort1ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort1ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort1ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort1ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort1ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort1ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort1ACF.setCheck = true;
                            }
                            else if (materialPort == "2")
                            {
                                int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                                int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                                int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                                int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                                int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                                int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                                int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                                int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                                if (tempTotalQty == 0)                                      // Material Ktting 값
                                {
                                    IssueWrite(line + 1, "ACF PORT2 TOTAL Count가 0값 입니다.");
                                    ViewIssueColor(line, 10);
                                }

                                if (tempUseQty != 0)                                                                     // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 11);
                                }

                                if (tempAssembleQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 12);
                                }

                                if (tempTotalNGQty != 0)                                                                 // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 13);
                                }

                                if (temppUseQty != 0)                                                               // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 16);
                                }

                                if (temppNGQty != 0)                                                                // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 18);
                                }

                                if (temppAssembleQty != 0)                                                          // 0
                                {
                                    IssueWrite(line + 1, "ACF PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                    ViewIssueColor(line, 17);
                                }

                                if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                                {
                                    IssueWrite(line + 1, "ACF PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                    ViewIssueColor(line, 14);
                                }
                                materialPort2ACF.materialID = logLineArray[5];
                                materialPort2ACF.totalQty = int.Parse(logLineArray[10]);
                                materialPort2ACF.useQty = int.Parse(logLineArray[11]);
                                materialPort2ACF.assembleQty = int.Parse(logLineArray[12]);
                                materialPort2ACF.totalNGQty = int.Parse(logLineArray[13]);
                                materialPort2ACF.pUseQty = int.Parse(logLineArray[16]);
                                materialPort2ACF.pNGQty = int.Parse(logLineArray[18]);
                                materialPort2ACF.pAssembleQty = int.Parse(logLineArray[17]);
                                materialPort2ACF.remainQty = int.Parse(logLineArray[14]);
                                materialPort2ACF.setCheck = true;
                            }
                        }
                        break;

                    // Material FPC
                    case "FPC":
                        if (materialPort == "1")
                        {
                            int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "FPC PORT1 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 10);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 12);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "FPC PORT1 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 17);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "FPC PORT1 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 14);
                            }
                            materialPort1FPC.materialID = logLineArray[5];
                            materialPort1FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort1FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort1FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort1FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort1FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort1FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort1FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort1FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort1FPC.setCheck = true;
                        }                        
                        else if (materialPort == "2")
                        {
                            int tempTotalQty = int.Parse(logLineArray[10]);                                     // Material Kitting 값
                            int tempUseQty = int.Parse(logLineArray[11]);                                       // 0
                            int tempAssembleQty = int.Parse(logLineArray[12]);                                  // 0
                            int tempTotalNGQty = int.Parse(logLineArray[13]);                                   // 0
                            int temppUseQty = int.Parse(logLineArray[16]);                                      // 0
                            int temppNGQty = int.Parse(logLineArray[18]);                                       // 0
                            int temppAssembleQty = int.Parse(logLineArray[17]);                                 // 0
                            int tempRemainQty = int.Parse(logLineArray[14]);                                    // 남은 수량 Material Kitting 값 동일

                            if (tempTotalQty == 0)                                      // Material Ktting 값
                            {
                                IssueWrite(line + 1, "FPC PORT2 TOTAL Count가 0값 입니다.");
                                ViewIssueColor(line, 10);
                            }

                            if (tempUseQty != 0)                                                                     // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 USE Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 11);
                            }

                            if (tempAssembleQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 12);
                            }

                            if (tempTotalNGQty != 0)                                                                 // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 NG TOTAL Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 13);
                            }

                            if (temppUseQty != 0)                                                               // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 p_Use Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 16);
                            }

                            if (temppNGQty != 0)                                                                // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 NG Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 18);
                            }

                            if (temppAssembleQty != 0)                                                          // 0
                            {
                                IssueWrite(line + 1, "FPC PORT2 p_Assemble Count 자재 장착 후 0이 아닙니다.");
                                ViewIssueColor(line, 17);
                            }

                            if (tempRemainQty != tempTotalQty)                                                                  // 남은 수량 Material Kitting 값 동일
                            {
                                IssueWrite(line + 1, "FPC PORT2 Remain Count 자재 장착 후 TotalQty와 동일해야합니다.");
                                ViewIssueColor(line, 14);
                            }
                            materialPort2FPC.materialID = logLineArray[5];
                            materialPort2FPC.totalQty = int.Parse(logLineArray[10]);
                            materialPort2FPC.useQty = int.Parse(logLineArray[11]);
                            materialPort2FPC.assembleQty = int.Parse(logLineArray[12]);
                            materialPort2FPC.totalNGQty = int.Parse(logLineArray[13]);
                            materialPort2FPC.pUseQty = int.Parse(logLineArray[16]);
                            materialPort2FPC.pNGQty = int.Parse(logLineArray[18]);
                            materialPort2FPC.pAssembleQty = int.Parse(logLineArray[17]);
                            materialPort2FPC.remainQty = int.Parse(logLineArray[14]);
                            materialPort2FPC.setCheck = true;
                        }
                        break;
                }
            }
        }
        #endregion

        // Issue Result
        private void IssueWrite(int line, string text)
        {
            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                string strTemp = string.Format("Log Line : {0} 줄, 내용 : {1}\r\n", line.ToString(), text);
                resultText.Append(strTemp);
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                string strTemp = string.Format("Log Line : {0} 줄, 내용 : {1}\r\n", (line - 1).ToString(), text);
                resultText.Append(strTemp);
            }
        }

        // 그리드뷰
        private void ViewUpdate(string[] logArray)
        {
            int logver = cmbLogVerSelect.SelectedIndex;
            DataGridViewTopLeftHeaderCell topheaderCell = new DataGridViewTopLeftHeaderCell();

            switch (logver)
            {
                case 0:
                    for (int i = 0; i < logInfo.Length; i++)
                    {
                        mainTable.Columns.Add(logInfo[i]);
                    }
                    for (int i = 0; i < logArray.Length; i++)
                    {
                        string[] logLineArray = logArray[i].Split(',');
                        mainTable.Rows.Add(logLineArray);
                    }
                    adgvFilter.DataSource = mainTable;

                    topheaderCell.Value = "No";
                    adgvFilter.TopLeftHeaderCell = topheaderCell;
                    for (int i = 0; i < adgvFilter.Rows.Count; i++)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.HeaderCell.Value = (i + 1).ToString();
                        row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        adgvFilter.Rows[i].HeaderCell = row.HeaderCell;
                    }
                    adgvFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    //SetDoNotSort(adgvFilter);
                    break;

                case 1:
                    string[] logColumnArray = logArray[0].Split(',');
                    for (int i = 0; i < logColumnArray.Length; i++)
                    {
                        mainTable.Columns.Add(logColumnArray[i]);
                    }
                    for (int i = 1; i < logArray.Length; i++)
                    {
                        string[] logLineArray = logArray[i].Split(',');
                        mainTable.Rows.Add(logLineArray);
                    }
                    adgvFilter.DataSource = mainTable;
                    topheaderCell.Value = "No";
                    adgvFilter.TopLeftHeaderCell = topheaderCell;
                    for (int i = 0; i < adgvFilter.Rows.Count; i++)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.HeaderCell.Value = (i + 1).ToString();
                        row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                        adgvFilter.Rows[i].HeaderCell = row.HeaderCell;
                    }
                    adgvFilter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    break;

            }
        }
        private void ViewIssueColor(int line, int cellIndex)
        {
            if (cmbLogVerSelect.SelectedIndex == 0)
            {
                adgvFilter.Rows[line].Cells[cellIndex].Style.ForeColor = Color.Red;
                issueList.Add(string.Format("{0},{1}", line, cellIndex));
            }
            else if (cmbLogVerSelect.SelectedIndex == 1)
            {
                adgvFilter.Rows[line - 1].Cells[cellIndex].Style.ForeColor = Color.Red;
                issueList.Add(string.Format("{0},{1}", line - 1, cellIndex));
            }
        }
        private void SetDoNotSort(DataGridView dgv) // Sort 막음
        {
            foreach (DataGridViewColumn i in dgv.Columns)
            {
                i.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        // View Issue Line 이동
        private void txtReuslt_Click(object sender, EventArgs e)
        {
            if (bindingSource1.Filter != string.Empty)
            {
                MessageBox.Show("LOG View 필터 사용간 Issue Line 이동이 불가능합니다.\r\n필터를 풀어주세요.", "Issue Line Focus Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtReuslt.Text == string.Empty) return;
            adgvFilter.ClearSelection();
            int focus = txtReuslt.GetLineFromCharIndex(txtReuslt.SelectionStart);
            if (txtReuslt.Lines.Length == (focus + 1)) return;
            int index = txtReuslt.Lines[focus].IndexOf(" 줄,");
            int line = int.Parse(txtReuslt.Lines[focus].Substring(0, index).Replace("Log Line : ", "")) - 1;
            DataGridViewCell _dgvCell = adgvFilter.Rows[line].Cells[0];
            adgvFilter.FirstDisplayedCell = _dgvCell;
            adgvFilter.CurrentCell = _dgvCell;
            adgvFilter.Rows[line].Selected = true;
            string port = adgvFilter.Rows[line].Cells[8].Value.ToString();
            string materialType = adgvFilter.Rows[line].Cells[12].Value.ToString();

            // 해당 Port Material Type 선택 Color
            for (int i = 0; i < adgvFilter.Rows.Count - 1; i++)
            {
                string tempPort = adgvFilter.Rows[i].Cells[8].Value.ToString();
                string tempMaterialType = adgvFilter.Rows[i].Cells[12].Value.ToString();
                string tempCEID = adgvFilter.Rows[i].Cells[3].Value.ToString();
                if (tempCEID != "406") //                                                                    CEID 406 제외
                {
                    if (port.Equals(tempPort) && materialType.Equals(tempMaterialType))
                    {
                        adgvFilter.Rows[i].DefaultCellStyle.BackColor = Color.YellowGreen;
                    }
                    else
                    {
                        adgvFilter.Rows[i].DefaultCellStyle.BackColor = Color.Empty;
                    }
                }
            }
        }
        // Filter
        private void adgvFilter_FilterStringChanged(object sender, EventArgs e)
        {
            bindingSource1.Filter = adgvFilter.FilterString;

            if (bindingSource1.Filter == "")
            {
                adgvFilter.DataSource = mainTable;
                for (int i = 0; i < issueList.Count; i++)
                {
                    string[] strTemp = issueList[i].Split(',');
                    int row = int.Parse(strTemp[0].ToString());
                    int cell = int.Parse(strTemp[1].ToString());
                    adgvFilter.Rows[row].Cells[cell].Style.ForeColor = Color.Red;
                }
                DataGridViewTopLeftHeaderCell topheaderCell = new DataGridViewTopLeftHeaderCell();
                topheaderCell.Value = "No";
                adgvFilter.TopLeftHeaderCell = topheaderCell;
                for (int i = 0; i < adgvFilter.Rows.Count; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.HeaderCell.Value = (i + 1).ToString();
                    row.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    adgvFilter.Rows[i].HeaderCell = row.HeaderCell;
                }
            }
            else
            {
                // FilterLogNumber();
            }

        }
        private void adgvFilter_SortStringChanged(object sender, EventArgs e)
        {
            //this.bindingSource1.Sort = this.adgvFilter.SortString;
            int tempIndex = adgvFilter.SortString.IndexOf(']');
            string columnName = adgvFilter.SortString.Substring(1, tempIndex - 1);
            DataGridViewColumn sortColumn = adgvFilter.Columns[columnName];
            string sort = adgvFilter.SortString.Substring(tempIndex + 2);
            if (sort == "ASC")
            {
                adgvFilter.Sort(sortColumn, ListSortDirection.Ascending);
            }
            else
            {
                adgvFilter.Sort(sortColumn, ListSortDirection.Descending);
            }
        }

        private void btnFilerCancel_Click(object sender, EventArgs e)
        {
            bindingSource1.Filter = "";
            adgvFilter.ClearFilter();
            adgvFilter_FilterStringChanged(sender, e);
        }
        private void FilterLogNumber() // 필터시 기존 행번호 및 검사 색상 나오게하는 메서드...너무오래걸려...
        {
            DataTable tb = new DataTable();
            tb = (DataTable)bindingSource1.DataSource;
            adgvFilter.DataSource = tb;

            for (int i = 0; i < adgvFilter.Rows.Count; i++)
            {
                string time = adgvFilter.Rows[i].Cells[1].Value.ToString();
                string ceid = adgvFilter.Rows[i].Cells[3].Value.ToString();
                string cellId = adgvFilter.Rows[i].Cells[4].Value.ToString();
                string materialType = adgvFilter.Rows[i].Cells[12].Value.ToString();
                for (int j = 0; j < mainTable.Rows.Count; j++)
                {
                    string timeTemp = mainTable.Rows[j][1].ToString();
                    string ceidTemp = mainTable.Rows[j][3].ToString();
                    string cellIdTemp = mainTable.Rows[j][4].ToString();
                    string materialTypeTemp = mainTable.Rows[j][12].ToString();
                    string matchTemp1 = string.Concat(time, "_", ceid, "_", cellId, "_", materialType);
                    string matchTemp2 = string.Concat(timeTemp, "_", ceidTemp, "_", cellIdTemp, "_", materialTypeTemp);
                    if (matchTemp1 == matchTemp2)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.HeaderCell.Value = (j + 1).ToString();
                        adgvFilter.Rows[i].HeaderCell = row.HeaderCell;
                    }
                }
                for (int j = 0; j < issueList.Count; j++)
                {
                    string[] strTemp = issueList[j].Split(',');
                    string row = strTemp[0].ToString();
                    int cell = int.Parse(strTemp[1].ToString());
                    string rowTemp = adgvFilter.Rows[i].HeaderCell.Value.ToString();
                    if (row == rowTemp)
                    {
                        adgvFilter.Rows[i].Cells[cell].Style.ForeColor = Color.Red;
                    }
                }
            }
        }


        // Excel File Export 기능
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            ExportToExcel();
            this.Enabled = true;
        }
        private void ExportToExcel()
        {
            if (adgvFilter.DataSource != null)
            {
                Excel.Application excelApp;
                Excel._Workbook workBook;
                Excel._Worksheet workSheet;

                // Start Excel and get Application object
                excelApp = new Excel.Application();

                // Get a new workbook
                workBook = (Excel._Workbook)(excelApp.Workbooks.Add(Missing.Value));
                workSheet = (Excel._Worksheet)workBook.ActiveSheet;

                // Add table headers going cell by cell
                int k = 0;
                string[] colHeader = new string[adgvFilter.ColumnCount];
                for (int i = 0; i < adgvFilter.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1] = adgvFilter.Columns[i].HeaderText;
                    k = i + 65;
                    colHeader[i] = Convert.ToString((char)k);
                }

                // Format A1:D1 as bold.vertical alignment center.
                workSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
                workSheet.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // Create an array to multiple values at once
                object[,] saNames = new object[adgvFilter.RowCount, adgvFilter.ColumnCount];

                string tp;
                for (int i = 0; i < adgvFilter.RowCount; i++)
                {
                    for (int j = 0; j < adgvFilter.ColumnCount; j++)
                    {
                        tp = adgvFilter.Rows[i].Cells[j].ValueType.Name;
                        string value = string.Empty;
                        if (adgvFilter.Rows[i].Cells[j].Value != null)
                            value = adgvFilter.Rows[i].Cells[j].Value.ToString();

                        if (tp == "String") // 2000-01-01 형태의 날짜 필터하기 위함(숫자로 변환 방식)
                        {
                            saNames[i, j] = string.Concat("'", value);
                        }
                        else
                        {
                            saNames[i, j] = value;
                        }
                    }
                }

                // Fill A2:B6 With an array of values (First and Last Names)
                // workSheet.get_Range("A2", "B6").Value2 = saNames;
                workSheet.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] + (adgvFilter.RowCount + 1)).Value2 = saNames;


                // Excel To Export시 Color는 속도 개선 필요
                if (chkExportToExcelColor.Checked == true)
                {
                    for (int i = 2; i < adgvFilter.RowCount; i++)
                    {
                        Color fillcolor = adgvFilter.Rows[i].DefaultCellStyle.BackColor;
                        if (fillcolor.Name != "0")
                        {
                            workSheet.get_Range(string.Concat("A", i + 2), string.Concat("W", i + 2)).Interior.Color = System.Drawing.ColorTranslator.ToOle(fillcolor);
                        }
                        for (int j = 0; j < adgvFilter.ColumnCount; j++)
                        {
                            string cell = string.Concat(DecToAlphabet(j), i.ToString());
                            Color color = adgvFilter[j, i - 2].Style.ForeColor;
                            if (color.Name != "0")
                            {
                                workSheet.get_Range(cell, Missing.Value).Font.Color = System.Drawing.ColorTranslator.ToOle(color);
                            }
                        }
                    }
                }

                excelApp.Visible = true;
                excelApp.UserControl = true;
                workBook = null;
                excelApp = null;

                // EXCEL.EXE 프로세스 제거
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
            }
            else
            {
                MessageBox.Show("검사 결과가 없어 Export가 불가능합니다.", "Export Excel Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string DecToAlphabet(int num)
        {
            int rest; //나눗셈 계산에 사용될 나머지 값
            string alphabet; //10진수에서 알파벳으로 변환될 값

            byte[] asciiA = Encoding.ASCII.GetBytes("A"); // 0=>A
            rest = num % 26; // A~Z 26자
            asciiA[0] += (byte)rest; // num 0일 때 A, num 4일 때 A+4 => E

            alphabet = Encoding.ASCII.GetString(asciiA); //변환된 알파벳 저장

            num = num / 26 - 1; // 그 다음 자리의 알파벳 계산을 재귀하기 위해, 받은 수/알파벳수 -1 (0은 A라는 문자값이 있으므로 -1을 기준으로 계산함)
            if (num > -1)
            {
                alphabet = alphabet.Insert(0, DecToAlphabet(num)); //재귀 호출하며 결과를 앞자리에 insert
            }
            return alphabet; // 최종값 return
        }// 숫자 엑셀 알파벳 컬럼값 변환

        // Login 기능
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
                MessageBox.Show("ID를 입력하세요!", "Login ID Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (string.IsNullOrEmpty(txtPW.Text))
                MessageBox.Show("PassWord를 입력하세요!!", "Login PW Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                LogInConfirmCheck();
        }
        // 사용자 ID와 Password 일치 여부 확인
        public void LogInConfirmCheck()
        {
            UserLogin login = new UserLogin();
            

            if (txtID.Text != login.UserID() )
            {
                MessageBox.Show("아이디가 존재하지 않습니다.", "Login ID Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtID.Clear();
            }
            else
            {
                if (txtPW.Text == login.UserPW())
                {
                    this.btnExportExcel.Enabled = true;
                    this.btnLogCheck.Enabled = true;
                    this.btnLogFilePathSet.Enabled = true;
                    this.btnTKExportExcel.Enabled = true;
                    this.btnTKInLogSet.Enabled = true;
                    this.btnTKOutLogSet.Enabled = true;
                    this.btnTKLogCheck.Enabled = true;

                    pnlLogin.Visible = false;
                    tabControl1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("비밀번호가 틀렸습니다.", "Login PW Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPW.Clear();
                }
            }
        }
        private void btnLoginCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void txtPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                object a = new object();
                EventArgs b = new EventArgs();
                btnLogin_Click(a, b);
            }
        }

        // Log Change
        private void cmbLogSet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Track In / Out

        // 로그경로설정
        private void btnTKInLogSet_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string temp = openFileDialog1.FileName.ToString();
                if (temp.Contains("CELL_TRACK_IN_LOG"))
                {
                    txtTKInLogPath.Text = openFileDialog1.FileName.ToString();
                }
                else
                {
                    MessageBox.Show("CELL_TARCK_IN_LOG가 아닙니다.", "Track IN Log Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnTKOutLogSet_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string temp = openFileDialog1.FileName.ToString();
                if (temp.Contains("CELL_TRACK_OUT_LOG"))
                {
                    txtTKOutLogPath.Text = openFileDialog1.FileName.ToString();
                }
                else
                {
                    MessageBox.Show("CELL_TARCK_OUT_LOG가 아닙니다.", "Track Out Log Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 로그검사
        private void btnTKLogCheck_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            if (txtTKInLogPath.Text == string.Empty || txtTKOutLogPath.Text == string.Empty)
            {
                MessageBox.Show("Track In/Out Log 경로를 설정해주세요.", "Track In/Out Log Path Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
            prgTKStatus.Value = 0;
            adgvTKAverage.DataSource = null;
            adgvTKData.DataSource = null;
            string[] logTKInArray = File.ReadAllLines(txtTKInLogPath.Text);
            string[] logTKOutArray = File.ReadAllLines(txtTKOutLogPath.Text);

            #region TK IN 과 OUT간 시간차 index
            // 1번째 행 10열 TK IN CELLID 값을 TK OUT CELLID 행 index 찾는다.
            int indexTKInStart = 0;
            string tkInCellID = logTKInArray[1].Split(',')[8];
            int indexCount = 0;
            for (int i = 1; i < logTKOutArray.Length; i++)
            {
                string tempTKOutCellID = logTKOutArray[i].Split(',')[8];
                if (tkInCellID == tempTKOutCellID)
                {
                    indexTKInStart = i;
                    break;
                }
                if (i == logTKOutArray.Length && indexTKInStart == 0)
                {
                    indexCount++;
                    tkInCellID = logTKInArray[indexCount].Split(',')[8];
                    i = 0;
                }
            }
            prgTKStatus.PerformStep();
            // TK OUT CELLID 마지막행값 TK IN CELLID 행 index 찾는다.
            int indexTKOutEnd = 0;
            string tkOutCellID = logTKOutArray[logTKOutArray.Length - 1].Split(',')[8];
            for (int i = logTKInArray.Length - 1; i > 1; i--)
            {
                string tempTKInCellID = logTKInArray[i].Split(',')[8];
                if (tkOutCellID == tempTKInCellID)
                {
                    indexTKOutEnd = i;
                    break;
                }
            }
            prgTKStatus.PerformStep();
            #endregion

            #region Table Setting
            DataTable tableTKDataView = new DataTable();
            DataTable tableTKIn = new DataTable();
            string[] tempInColumn = logTKInArray[0].Split(',');
            for (int i = 0; i < tempInColumn.Length; i++)
            {
                tableTKIn.Columns.Add(tempInColumn[i]);
                tableTKDataView.Columns.Add(string.Concat("TK_IN_", tempInColumn[i]));
            }
            tableTKDataView.Columns.Add("TK OUT Check");
            tableTKDataView.Columns.Add("TK IN Check");
            prgTKStatus.PerformStep();
            DataTable tableTKOut = new DataTable();
            string[] tempOutColumn = logTKOutArray[0].Split(',');
            for (int i = 0; i < tempOutColumn.Length; i++)
            {
                tableTKOut.Columns.Add(tempOutColumn[i]);
                tableTKDataView.Columns.Add(string.Concat("TK_OUT_", tempOutColumn[i]));
            }
            prgTKStatus.PerformStep();
            int rowCount = logTKInArray.Length > logTKOutArray.Length ? (logTKInArray.Length - 1) : (logTKOutArray.Length - 1);
            for (int i = 0; i < rowCount; i++)
            {
                tableTKDataView.Rows.Add();
            }
            for (int i = 1; i < logTKInArray.Length; i++)
            {
                string[] rowData = logTKInArray[i].Split(',');
                tableTKIn.Rows.Add(rowData);
                for (int j = 0; j < 14; j++)
                {
                    tableTKDataView.Rows[i - 1][j] = rowData[j];
                }
            }
            prgTKStatus.PerformStep();
            for (int i = 1; i < logTKOutArray.Length; i++)
            {
                string[] rowData = logTKOutArray[i].Split(',');
                tableTKOut.Rows.Add(rowData);
                for (int j = 0; j < 16; j++)
                {
                    tableTKDataView.Rows[i - 1][j + 16] = rowData[j];
                }
            }

            #endregion
            prgTKStatus.PerformStep();
            #region CELL ID 누락 중복 체크
            StringBuilder issueTKSb = new StringBuilder();
            // TK OUT
            double trackOutCount = 0;
            double trackOutMissCount = 0;
            double trackOutSameCount = 0;
            for (int i = 0; i < indexTKOutEnd; i++)
            {
                string cellID = tableTKIn.Rows[i][8].ToString();
                DataRow[] rows = tableTKOut.Select(string.Format("CELLID='{0}'", cellID));

                if (cellID == string.Empty)
                {
                    tableTKDataView.Rows[i][14] = 0;
                    continue;
                }
                if (rows.Length != 1)
                {
                    if (rows.Length > 1)
                    {
                        issueTKSb.AppendLine(string.Format("Track_In Line={0}, Track_Out {1}회, CELLID={2} 중복", i, rows.Length, cellID));
                    }
                    else if (rows.Length == 0)
                    {
                        issueTKSb.AppendLine(string.Format("Track_In CELLID={0}, Track_Out 누락", cellID));
                    }
                }
                //if(rows.Length == 1)
                //{
                //    trackOutCount = trackOutCount + rows.Length;
                //}
                //else if (rows.Length == 0)
                //{
                //    trackOutMissCount++;
                //}
                tableTKDataView.Rows[i][14] = rows.Length;
            }
            prgTKStatus.PerformStep();

            trackOutMissCount = tableTKDataView.Select("[TK OUT Check]='0'").Length;
            trackOutCount = tableTKDataView.Select("[TK OUT Check]='1'").Length;
            trackOutSameCount = tableTKDataView.Select("[TK OUT Check]>'1'").Length;

            // TK IN
            double trackInCount = 0;
            double trackInMissCount = 0;
            double trackInSameCount = 0;
            for (int i = indexTKInStart - 1; i < logTKOutArray.Length - 1; i++)
            {
                string cellID = tableTKOut.Rows[i][8].ToString();
                DataRow[] rows = tableTKIn.Select(string.Format("CELLID='{0}'", cellID));
                if (cellID == string.Empty)
                {
                    tableTKDataView.Rows[i][15] = 0;
                    continue;
                }
                if (rows.Length != 1)
                {
                    if (rows.Length > 1)
                    {
                        issueTKSb.AppendLine(string.Format("Track_Out Line={0}, Track_In {1}회, CELLID={2} 중복", i, rows.Length, cellID));
                    }
                    else if (rows.Length == 0)
                    {
                        issueTKSb.AppendLine(string.Format("Track_Out CELLID={0}, Track_In 누락", cellID));
                    }
                }
                //if (rows.Length == 1)
                //{
                //    trackInCount = trackInCount + rows.Length;
                //}
                //else if (rows.Length == 0)
                //{
                //    trackInMissCount++;
                //}
                tableTKDataView.Rows[i][15] = rows.Length;
            }
            prgTKStatus.PerformStep();

            trackInMissCount = tableTKDataView.Select("[TK IN Check]='0'").Length;
            trackInCount = tableTKDataView.Select("[TK IN Check]='1'").Length;
            trackInSameCount = tableTKDataView.Select("[TK IN Check]>'1'").Length;

            double tkTotalCount = tableTKDataView.Select("[TK_IN_CELLID]<>''").Length;

            // 중복= (전체수량 - 1) - (1인값 + 0인값 = 0)
            //trackOutSameCount = (logTKOutArray.Length - 1) - (trackOutCount + trackOutMissCount);

            trackOutSameCount = (tableTKDataView.Select("[TK OUT Check]<>''").Length) - (trackOutCount + trackOutMissCount);

            // 중복= ((전체수량 - TK In/Out 차) - TKOutStart) - (1인값 + 0인값 = 0)
            //double temp = logTKInArray.Length - logTKOutArray.Length;
            //trackInSameCount = ((logTKInArray.Length - temp) - indexTKInStart) - (trackInCount + trackInMissCount);
            trackInSameCount = (tableTKDataView.Select("[TK IN Check]<>''").Length) - (trackInCount + trackInMissCount);
            #endregion

            #region 정합성 공식
            // CELL 정합성 출력
            // TK IN OUT 전체 총수량, 중복, 누락, 정상
            // 정합성 백분율 : (중복 + 누락) / 정상
            double averageTKIn = (1 - ((trackInSameCount + trackInMissCount) / trackInCount)) * 100;
            double averageTKOut = (1 - ((trackOutSameCount + trackOutMissCount) / trackOutCount)) * 100;
            // 평균 정합률 (in 정합성 + out 정합성) / 2
            double totalaverageTK = (averageTKIn + averageTKOut) / 2;
            #endregion
            prgTKStatus.PerformStep();

            #region 정합성 그리드뷰
            DataTable averageTKTable = new DataTable();
            averageTKTable.Columns.Add("구분");
            averageTKTable.Columns.Add("전체 총 수량");
            averageTKTable.Columns.Add("중복");
            averageTKTable.Columns.Add("누락");
            averageTKTable.Columns.Add("정상");
            averageTKTable.Columns.Add("정합성");
            averageTKTable.Rows.Add();
            averageTKTable.Rows.Add();
            averageTKTable.Rows.Add();

            averageTKTable.Rows[1][0] = "Track IN";
            averageTKTable.Rows[1][1] = tkTotalCount;
            averageTKTable.Rows[1][2] = trackInSameCount;
            averageTKTable.Rows[1][3] = trackInMissCount;
            averageTKTable.Rows[1][4] = trackInCount;
            averageTKTable.Rows[1][5] = string.Format("{0:f2}%", averageTKIn);

            averageTKTable.Rows[2][0] = "Track OUT";
            averageTKTable.Rows[2][1] = tkTotalCount;
            averageTKTable.Rows[2][2] = trackOutSameCount;
            averageTKTable.Rows[2][3] = trackOutMissCount;
            averageTKTable.Rows[2][4] = trackOutCount;
            averageTKTable.Rows[2][5] = string.Format("{0:f2}%", averageTKOut);

            averageTKTable.Rows[0][0] = "Track IN/OUT";
            averageTKTable.Rows[0][1] = tkTotalCount * 2;
            averageTKTable.Rows[0][2] = trackInSameCount + trackOutSameCount;
            averageTKTable.Rows[0][3] = trackInMissCount + trackOutMissCount;
            averageTKTable.Rows[0][4] = trackInCount + trackOutCount;
            averageTKTable.Rows[0][5] = string.Format("{0:f2}%", (averageTKIn + averageTKOut) / 2);

            adgvTKAverage.DataSource = averageTKTable;
            adgvTKAverage.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            bindingSource2.DataSource = adgvTKAverage.DataSource;
            #endregion

            adgvTKData.DataSource = tableTKDataView;
            adgvTKData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            bindingSource3.DataSource = adgvTKData.DataSource;

            txtTKResult.Text = issueTKSb.ToString();
            prgTKStatus.PerformStep();
            this.Enabled = true;
        }

        // Track In/Out Filter
        private void adgvTKAverage_FilterStringChanged(object sender, EventArgs e)
        {
            bindingSource2.Filter = adgvTKAverage.FilterString;
        }
        private void adgvTKAverage_SortStringChanged(object sender, EventArgs e)
        {
            bindingSource2.Sort = adgvTKAverage.SortString;
        }
        private void adgvTKData_FilterStringChanged(object sender, EventArgs e)
        {
            bindingSource3.Filter = adgvTKData.FilterString;
        }
        private void adgvTKData_SortStringChanged(object sender, EventArgs e)
        {
            bindingSource3.Sort = adgvTKData.SortString;
        }
        private void btnTKExportExcel_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            TKExportToExcel();
            this.Enabled = true;
        }
        private void TKExportToExcel()
        {
            //MessageBox.Show("기능 추가중입니다. - 심민수D.", "Export Excel Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //return;

            if (adgvTKAverage.DataSource == null)
            {
                MessageBox.Show("검사 결과가 없어 Export가 불가능합니다.", "Export Excel Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (adgvTKData.DataSource == null)
            {
                MessageBox.Show("검사 결과가 없어 Export가 불가능합니다.", "Export Excel Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Excel.Application excelApp;
            Excel._Workbook workBook;
            Excel._Worksheet workSheet1;
            Excel._Worksheet workSheet2;
            Excel._Worksheet workSheet3;

            // Start Excel and get Application object
            excelApp = new Excel.Application();

            // Get a new workbook
            workBook = (Excel._Workbook)excelApp.Workbooks.Add();
            workSheet1 = workBook.Worksheets.get_Item(1) as Excel.Worksheet;
            workSheet1.Name = "정합성";

            // Add table headers going cell by cell
            int k = 0;
            string[] colHeader = new string[adgvTKAverage.ColumnCount];
            for (int i = 0; i < adgvTKAverage.Columns.Count; i++)
            {
                workSheet1.Cells[1, i + 1] = adgvTKAverage.Columns[i].HeaderText;
                k = i + 65;
                colHeader[i] = Convert.ToString((char)k);
            }

            // Format A1:D1 as bold.vertical alignment center.
            workSheet1.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
            workSheet1.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            // Create an array to multiple values at once
            object[,] saNames = new object[adgvTKAverage.RowCount, adgvTKAverage.ColumnCount];

            string tp;
            for (int i = 0; i < adgvTKAverage.RowCount; i++)
            {
                for (int j = 0; j < adgvTKAverage.ColumnCount; j++)
                {
                    tp = adgvTKAverage.Rows[i].Cells[j].ValueType.Name;
                    string value = string.Empty;
                    if (adgvTKAverage.Rows[i].Cells[j].Value != null)
                        value = adgvTKAverage.Rows[i].Cells[j].Value.ToString();

                    saNames[i, j] = value;
                }
            }

            // Fill A2:B6 With an array of values (First and Last Names)
            // workSheet.get_Range("A2", "B6").Value2 = saNames;
            workSheet1.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] + (adgvTKAverage.RowCount + 1)).Value2 = saNames;

            // Get a new workbook
            workSheet2 = workBook.Worksheets.get_Item(2) as Excel.Worksheet;
            workSheet2.Name = "TrackInOutLog";

            // Add table headers going cell by cell
            k = 0;
            colHeader = null;
            colHeader = new string[adgvTKData.ColumnCount];
            for (int i = 0; i < adgvTKData.Columns.Count; i++)
            {
                workSheet2.Cells[1, i + 1] = adgvTKData.Columns[i].HeaderText;
                if (i < 26) k = i + 65;
                else k = (i - 26) + 65;
                if (i < 26) colHeader[i] = Convert.ToString((char)k);
                else colHeader[i] = Convert.ToString("A" + (char)k);
            }

            // Format A1:D1 as bold.vertical alignment center.
            workSheet2.get_Range("A1", colHeader[colHeader.Length - 1] + "1").Font.Bold = true;
            workSheet2.get_Range("A1", colHeader[colHeader.Length - 1] + "1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            // Create an array to multiple values at once
            saNames = null;
            saNames = new object[adgvTKData.RowCount, adgvTKData.ColumnCount];

            tp = string.Empty;
            for (int i = 0; i < adgvTKData.RowCount; i++)
            {
                for (int j = 0; j < adgvTKData.ColumnCount; j++)
                {
                    tp = adgvTKData.Rows[i].Cells[j].ValueType.Name;
                    string value = string.Empty;
                    if (adgvTKData.Rows[i].Cells[j].Value != null)
                        value = adgvTKData.Rows[i].Cells[j].Value.ToString();

                    saNames[i, j] = value;
                }
            }

            // Fill A2:B6 With an array of values (First and Last Names)
            // workSheet.get_Range("A2", "B6").Value2 = saNames;
            workSheet2.get_Range(colHeader[0] + "2", colHeader[colHeader.Length - 1] + (adgvTKData.RowCount + 1)).Value2 = saNames;

            workSheet3 = workBook.Worksheets.get_Item(3) as Excel.Worksheet;
            workSheet3.Name = "LOG검사결과";

            for (int i = 0; i < txtTKResult.Lines.Length; i++)
            {
                workSheet3.Cells[i + 1, 1] = txtTKResult.Lines[i].ToString();
            }

            excelApp.Visible = true;
            excelApp.UserControl = true;
            workBook = null;
            excelApp = null;

            // EXCEL.EXE 프로세스 제거
            GC.GetTotalMemory(false);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.GetTotalMemory(true);

        }
        #endregion

        #region PLC Memory
        Thread readMemoryPLCThread;
        bool bPlcConnection = false;
        //System.Timers.Timer _Timer;
        ActUtlTypeLib.ActUtlType _ActUtlType;
        DataTable readMemoryPLCTable = new DataTable();

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtDevice.Text == string.Empty || txtStationNo.Text == string.Empty)
            {
                MessageBox.Show("PLC Station Number, Device 주소를 입력해주세요. ", "PLC Station, Device Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int szIndex = int.Parse(txtDevice.Text.Substring(1, txtDevice.Text.Length - 1));
            if (btnPLCConnect.Text == "Connect")
            {
                dgvMemory.Rows.Clear();
                for (int i = 0; i < 100; i++)
                {
                    dgvMemory.Rows.Add();
                    string szDA = string.Concat(txtDevice.Text.Substring(0, 1), szIndex.ToString());
                    dgvMemory[0, i].Value = szDA;
                    szIndex++;
                }
                readMemoryPLCThread = new Thread(new ThreadStart(ReadMemoryPLCData));
                _ActUtlType = new ActUtlTypeLib.ActUtlType();

                //Logical Station number를 입력할 TextBox가 String.Empty이거나, Null이 아니면 진행되도록.
                if (String.IsNullOrEmpty(txtStationNo.Text).Equals(false))
                {
                    _ActUtlType.ActLogicalStationNumber = Convert.ToInt32(txtStationNo.Text);
                    if (_ActUtlType.Open().Equals(0))
                    {
                        bPlcConnection = true;

                        string deviceAddress = txtDevice.Text;

                        //PLC의 M0를 접점시키는 코드
                        //if (_ActUtlType.WriteDeviceBlock(deviceAddress, 1, 1).Equals(0))
                        //{
                        readMemoryPLCThread.Start();
                        //}
                        btnPLCConnect.Text = "Disconnect";
                        btnPLCSet.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("PLC 연결실패.", "PLC Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                if (bPlcConnection.Equals(true))
                {
                    //PLC의 M0를 접점을 해제시키는 코드
                    _ActUtlType.Close();
                    btnPLCSet.Enabled = false;
                }

                bPlcConnection = false;
                btnPLCConnect.Text = "Connect";
            }
        }

        public void ReadMemoryPLCData()
        {
            int index = 0;
            String szDevice = string.Empty;

            while (bPlcConnection)
            {
                if (index == 100) index = 0;

                Invoke(new MethodInvoker(delegate()
                    {
                        szDevice = dgvMemory[0, index].Value.ToString();
                    }));

                int lSize = 1; //읽어올 PLC Device의 Word 수

                int[] lplData = new int[lSize]; //읽어온 PLC Device Word 값을 저장할 변수

                //_ActUtlType.ReadDeviceBlock 함수의 리턴 값
                int nResult = _ActUtlType.ReadDeviceBlock(szDevice, lSize, out lplData[0]);

                if (nResult.Equals(0)) //정상적으로 리턴받으면 0을 받는다.
                {
                    Invoke(new MethodInvoker(delegate()
                    {
                        if (lplData[0] >= 0 && lplData[0] <= 127)
                        {
                            dgvMemory[1, index].Value = lplData[0];
                            dgvMemory[2, index].Value = Convert.ToString(lplData[0], 2).PadLeft(8, '0');
                            dgvMemory[3, index].Value = Convert.ToString(lplData[0], 16);
                            dgvMemory[4, index].Value = Convert.ToChar(lplData[0]);
                        }
                        else
                        {
                            dgvMemory[1, index].Value = lplData[0];
                            dgvMemory[2, index].Value = "";
                            dgvMemory[3, index].Value = "";
                            dgvMemory[4, index].Value = "";
                        }
                    }));
                }
                index++;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // PLC 연결 쓰레드 해제
            bPlcConnection = false;

            // PLC 연결 해제
            if (_ActUtlType != null)
            {
                _ActUtlType.Close();
            }
        }

        private void btnPLCSet_Click(object sender, EventArgs e)
        {
            if (btnPLCConnect.Text == "Disconnect")
            {
                if (txtModifyDevice.Text == string.Empty || txtPLCSetValue.Text == string.Empty)
                {
                    MessageBox.Show("PLC Device 주소 및 Set Value를 입력해주세요. ", "PLC Device Set Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _ActUtlType.ActLogicalStationNumber = Convert.ToInt32(txtStationNo.Text);
                bPlcConnection = true;

                string deviceAddress = txtModifyDevice.Text;
                int setValue = int.Parse(txtPLCSetValue.Text);


                if (DialogResult.No == MessageBox.Show("PLC 메모리 영역에 강제 Data 입력을 하시겠습니까?", "경고!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    MessageBox.Show("PLC Value Set 취소하였습니다.", "PLC Value Set Cancel.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //PLC의 M0를 접점시키는 코드
                if (_ActUtlType.WriteDeviceBlock(deviceAddress, 1, setValue).Equals(0))
                {

                }
                else
                {
                    MessageBox.Show("PLC Value Set 실패하였습니다.", "PLC Value Set Error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        private void InputOnlyNumber(KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtStationNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputOnlyNumber(e);
        }

        private void txtPLCSetValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            InputOnlyNumber(e);
        }

        #region High Log Check

        // SECS2 Log Read (기존 Analysis 사용하면 됨)
        private void logRead()
        {
            string testFilePath = string.Empty;
            List<string> logLine = new List<string>();

            testFilePath = "D:\0.내재화CIM\\log\\SDCCIM_Log_2017012013.log";

            StreamReader stRead = new StreamReader(testFilePath);
        }
        


        #endregion\

        #region CIM Log Analysis

        #region Define
        public const int IDX_MSG_START = 1; // URCIS or SYSWIN log(syswin Log 상단에 Syswin 남아 구분 가능) syswin 시 0 및 MSG_CEID 쪽 "," 사이에 추가 필요 아마도?(동관 사업장 내 완료 버전 있음)

        public const string EQ_01_01_LOCAL = "Local : 127.0.0.1";

        public const string CIM_LOG_FOLDER = "SDCCIM_LOG";

        public const string CR_LF = "\r\n";

        public const string COMBO_INIT = "---------------";

        public const string MSG_S6F11 = "S6F11";                //~~~ EQ State
        public const string MSG_S5F1 = "S5F1";                 //~~~ Alarm
        public const string MSG_INPUT = "S-F-";                 //~~~ User Input

        public const string MSG_CEID_401 = "CEID '401'";       //~~~ Cell In
        public const string MSG_CEID_406 = "CEID '406'";       //~~~ Cell Out
        public const string MSG_CEID_101 = "CEID '101'";       //~~~ EQ State Change
        public const string MSG_CEID_200 = "CEID '200'";       //~~~ Material Change
        public const string MSG_CEID_215 = "CEID '215'";       //~~~ Material Assemble Process
        public const string MSG_CEID_222 = "CEID '222'";       //~~~ Material NG Process
        public const string MSG_CEID_223 = "CEID '223'";       //~~~ Material Warning Process
        public const string MSG_CEID_224 = "CEID '224'";       //~~~ Material Shortage Process
        public const string MSG_CEID_606 = "CEID '606'";       //~~~ TPM Loss

        #endregion Define End

        private List<string> m_listLogFile = new List<string>();

        private List<string> m_listAnalysis = new List<string>();

        private List<string> m_listCELL_ID = new List<string>();

        private int m_nLogItemCnt = 0;

        private bool m_bOnlyCountInfo = false;

        private string m_strEQ_IP = string.Empty;

        private bool Analysis_Init()
        {
            cbEQPID.Items.Add(COMBO_INIT);
            cbEQPID.Items.Add(EQ_01_01_LOCAL);
            cbEQPID.Items.Add(COMBO_INIT);
            cbEQPID.SelectedIndex = 0;

            cbMessage.Items.Add(COMBO_INIT);
            cbMessage.Items.Add(MSG_S6F11);             //~~~ EQ State
            cbMessage.Items.Add(COMBO_INIT);
            cbMessage.Items.Add(MSG_S5F1);              //~~~ Alarm
            cbMessage.Items.Add(COMBO_INIT);
            cbMessage.SelectedIndex = 0;

            cbMessageSub.Items.Add(COMBO_INIT);
            cbMessageSub.Items.Add(MSG_CEID_401);   //~~~ Cell In
            cbMessageSub.Items.Add(MSG_CEID_406);   //~~~ Cell Out
            cbMessageSub.Items.Add(COMBO_INIT);
            cbMessageSub.Items.Add(MSG_CEID_101);   //~~~ EQ State Change
            cbMessageSub.Items.Add(MSG_CEID_606);   //~~~ TPM Loss
            cbMessageSub.Items.Add(COMBO_INIT);
            cbMessageSub.Items.Add(MSG_CEID_200);   //~~~ Material Change
            cbMessageSub.Items.Add(MSG_CEID_215);   //~~~ Material Assemble Process
            cbMessageSub.Items.Add(MSG_CEID_222);   //~~~ Material NG Process
            cbMessageSub.Items.Add(MSG_CEID_223);   //~~~ Material Warning Process
            cbMessageSub.Items.Add(MSG_CEID_224);   //~~~ Material Shortage Process
            cbMessageSub.Items.Add(COMBO_INIT);
            cbMessageSub.SelectedIndex = 0;

            rbtFixed.Checked = true;
            tbRangeStart.Text = "1";
            tbRangeEnd.Text = "100";

            ckbNonRealtime.Checked = true;

            return true;
        }

        private string LogParsing(string strParsing_Start, string strParsing_End, string strLog)
        {
            int nPos_Start = -999;
            int nPos_End = -999;
            string strData = string.Empty;
            string strReturn = string.Empty;

            if (strParsing_Start == "")
            {
                nPos_Start = 0;
            }
            else
            {
                nPos_Start = strLog.IndexOf(strParsing_Start);
            }

            if (strParsing_End == "")
            {
                nPos_End = strLog.Length - 1;
            }
            else
            {
                nPos_End = strLog.IndexOf(strParsing_End, nPos_Start + 1);
            }

            strData = strLog.Substring(nPos_Start, (nPos_End - nPos_Start) + 1);

            strReturn = string.Format("{0}\t", strData);

            return strReturn;
        }

        private void tbRangeStart_TextChanged(object sender, EventArgs e)
        {
            int nValue = 0;

            if (tbRangeStart.Text.Length < 1)
            {
                tbRangeStart.Text = "1";
            }

            nValue = int.Parse(tbRangeStart.Text);
            nValue += 99;

            tbRangeEnd.Text = string.Format("{0}",nValue);
        }

        private string GetLogDate(string strLogFile)
        {
            string strReturn = string.Empty;
            string strDate = string.Empty;
            int nPos = -999;

            //~~~ strLogFile => "D:\SDCCIM_LOG\SDCCIM_Log_2016092023.log"

            nPos = strLogFile.LastIndexOf("\\");
            if (nPos >= 0)
            {
                strDate = strLogFile.Substring(nPos + 1);
            }

            if (strDate.Length < "yyyyMMddHH.log".Length)
            {
                return string.Empty;
            }

            //~~~ strDate => "SDCCIM_Log_2016092023.log"  or  "2016092023.log"

            nPos = strDate.LastIndexOf("_Log_");
            if (nPos >= 0)
            {
                strDate = strDate.Substring(11);
            }

            //~~~ Date Info Parsing

            strReturn = string.Format("{0:0000}-{1:00}-{2:00}", strDate.Substring(0, 4), strDate.Substring(4, 2), strDate.Substring(6, 2));

            return strReturn;
        }

        private string Check_Cell_ID(string strCELL_ID)
        {
            string strReturn = string.Empty;
            string strData = string.Empty;

            strData = strCELL_ID;
            strData = strData.Replace(" ", "");

            strReturn += strData;

            strData = strData.Replace("'", "");
            strData = strData.Replace("\t", "");

            if (strData.Length > 0)
            {
                if (m_listCELL_ID.Contains(strData) == true)         //~~~ CELL_ID Duplicate
                {
                    strReturn += "--- Duplicate : ";
                    strReturn += strData;
                    strReturn += " --- \t";
                }
                else
                {
                    strReturn += "\t";
                    m_listCELL_ID.Add(strData);
                }
            }
            else
            {
                strReturn += "=== Empty === \t";
            }

            return strReturn;
        }

        private bool LoadLogFileList()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;
            ofd.InitialDirectory = "\\\\" + m_strEQ_IP + "\\" + CIM_LOG_FOLDER;

            ofd.ShowDialog();

            if (ofd.FileNames.Length < 1)
            {
                return false;
            }

            m_listLogFile.Clear();

            for (int i = 0; i < ofd.FileNames.Length; ++i)
            {
                m_listLogFile.Add(ofd.FileNames[i]);
            }

            return true;
        }

        private void frmCimLogAnalysis_Load(object sender, EventArgs e)
        {
            Analysis_Init();
        }

        private void rbtFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtFixed.Checked == true)
            {
                cbMessage.Enabled = true;
                cbMessageSub.Enabled = true;

                tbMessage.Enabled = false;
                tbRangeStart.Enabled = false;
                tbRangeEnd.Enabled = false;
            }
        }

        private void rbtUnfixed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtUnfixed.Checked == true)
            {
                cbMessage.Enabled = false;
                cbMessageSub.Enabled = false;

                tbMessage.Enabled = true;
                tbRangeStart.Enabled = true;
                tbRangeEnd.Enabled = true;
            }
        }

        private void cbMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbMessage.Text)
            {
                case MSG_S6F11:
                    cbMessageSub.SelectedIndex = 0;
                    cbMessageSub.Enabled = true;
                    break;

                case MSG_S5F1:
                    cbMessageSub.SelectedIndex = 0;
                    cbMessageSub.Enabled = false;
                    break;

                default:
                    cbMessageSub.Enabled = false;
                    break;
            }
        }

        private void btnSelectLogFile_Click(object sender, EventArgs e)
        {
            bool bResult = false;
            string strData = string.Empty;

            bResult = LoadLogFileList();
            if (bResult == false)
            {
                return;
            }

            m_listAnalysis.Clear();

            tbLogFile.Clear();

            for (int i = 0; i < m_listLogFile.Count; ++i)
            {
                strData = string.Format("#{0}> {1}{2}", i + 1, m_listLogFile[i], CR_LF);

                tbLogFile.AppendText(strData);

                m_listAnalysis.Add("\t\t\t\t" + strData);
            }

            m_listAnalysis.Add(CR_LF);

            tbLogAnalysis.Clear();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            m_listAnalysis.Clear();

            tbLogAnalysis.Clear();
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            m_bOnlyCountInfo = true;

            Analysis_Proc();
        }

        private void btnCimLogAnalysis_Click(object sender, EventArgs e)
        {
            m_bOnlyCountInfo = false;

            Analysis_Proc();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            AnalysisExportToExcel(sender, e);
        }

        private bool AnalysisExportToExcel(object sender, EventArgs e)
        {
            string strPath = "D:\\temp\\CimLogAnalysis";
            string strFile;
            string strData;
            bool bResult;

            bResult = Directory.Exists(strPath);
            if (bResult == false)
            {
                Directory.CreateDirectory(strPath);
            }

            strFile = string.Format("{0:0000}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}.csv",
                DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            StreamWriter streamWriter = new StreamWriter(strPath + "\\" + strFile);

            foreach (string str in m_listAnalysis)
            {
                strData = str.Replace(",", " ");
                strData = str.Replace("\t", ",");

                #region easyReadColurm - 2019-11-06
                if(strData.Contains("S6F11 : CEID '401' "))
                {
                    strData = strData.Replace("\r\n", "");
                    strData = string.Concat(strData, ",,,,,,,EQ,,,,,,,,,,CELLID,,,,,,,,,,,READERID\r\n");
                }
                else if (strData.Contains("S6F11 : CEID '406' "))
                {
                    strData = strData.Replace("\r\n", "");
                    strData = string.Concat(strData, ",,,,,,,EQ,,,,,,,,,,CELLID,,,,,,,,,,,READERID,,,MT-1,,,,,,,,,,,,,,,,,,DV,,,,,,,,,,,,,,,,,,,,,,,,,,JUDGE\r\n");
                }

                #endregion

                streamWriter.Write(strData);
            }

            streamWriter.Close();

            Process.Start("Excel.exe", strPath + "\\" + strFile);

            return true;
        }

        private bool Analysis_Proc()
        {
            string strData = string.Empty;

            //
            //~~~ Start
            //

            m_nLogItemCnt = 0;

            tbLogItemCount.Clear();
            tbLogItemCount.Update();

            m_listCELL_ID.Clear();

            if (rbtFixed.Checked == true)
            {
                strData = string.Format("\t\t\t\t*** {0} : {1} : ({2}) {3}", cbMessage.Text, cbMessageSub.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CR_LF);
            }
            else
            {
                strData = string.Format("\t\t\t\t*** {0} : ({1}) {2}", cbMessage.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CR_LF);
            }

            m_listAnalysis.Add(strData);

            tbLogAnalysis.AppendText(strData);

            //
            //~~~ Log Analysis
            //

            for (int i = 0; i < m_listLogFile.Count; ++i)
            {
                Analysis_Proc_File(m_listLogFile[i]);
            }

            //
            //~~~ Finish
            //

            if (rbtFixed.Checked == true)
            {
                strData = string.Format("\t\t\t\t*** {0} : {1} : ({2}) : {3} ea {4}", cbMessage.Text, cbMessageSub.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_nLogItemCnt, CR_LF + CR_LF);
            }
            else
            {
                strData = string.Format("\t\t\t\t*** {0} : ({1}) : {2} ea {3}", cbMessage.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_nLogItemCnt, CR_LF + CR_LF);
            }

            m_listAnalysis.Add(strData);

            tbLogAnalysis.AppendText(strData);

            tbLogItemCount.Text = string.Format("{0} EA", m_nLogItemCnt);

            return true;
        }

        private bool Analysis_Proc_File(string strLogFile)
        {
            bool bResult = false;
            string strDate = string.Empty;
            string strLogData = string.Empty;
            List<string> listLogData = new List<string>();

            bResult = File.Exists(strLogFile);
            if (bResult == false)
            {
                return false;
            }

            strDate = GetLogDate(strLogFile);

            StreamReader streamReader = new StreamReader(strLogFile);
            if (streamReader == null)
            {
                return false;
            }

            while (true)
            {
                strLogData = streamReader.ReadLine();
                if (strLogData == null)
                {
                    Analysis_Proc_MSG(strDate, listLogData);
                    listLogData.Clear();
                    break;
                }
                if (strLogData.Substring(0, 1) == "[")
                {
                    Analysis_Proc_MSG(strDate, listLogData);
                    listLogData.Clear();
                }

                listLogData.Add(strLogData);
            }

            streamReader.Close();

            return true;
        }

        private bool Analysis_Proc_Result(string strDate, List<string> listLogData, string strMsg, string strMsg_Sub)
        {
            string strData = string.Empty;
            string strTemp = string.Empty;
            string strAnalysis = string.Empty;
            int nTemp = 0;
            int nStart = 0;
            int nEnd = 0;

            m_nLogItemCnt += 1;

            if (m_bOnlyCountInfo == true)
            {
                return true;
            }

            strAnalysis += LogParsing("", "", "[" + strDate + "]");                 //~~~ [2016-09-27]
            strAnalysis += LogParsing("[", "]", listLogData[0]);                    //~~~ [14:41:08.999] [SECS2]Send - [0002ADB7]
            strAnalysis += LogParsing("", "", "[" + strMsg + "]");                  //~~~ S6F11
            strAnalysis += LogParsing("", "", "[" + strMsg_Sub + "]");              //~~~ CEID '401'
            strAnalysis += LogParsing("<", "", listLogData[IDX_MSG_START]);   //~~~ <S5F1 W Alarm Report Send

            if (rbtUnfixed.Checked == true)
            {
                nStart = int.Parse(tbRangeStart.Text);
                nEnd = int.Parse(tbRangeEnd.Text);
            }
            else
            {
                nStart = int.MinValue;
                nEnd = int.MaxValue;
            }

            if ((m_nLogItemCnt >= nStart) && (m_nLogItemCnt <= nEnd))
            {
                for (int i = IDX_MSG_START + 1; i < listLogData.Count; ++i)
                {
                    strData = listLogData[i];

                    if (strData.IndexOf("<A") >= 0)
                    {
                        strAnalysis += LogParsing("<", "", listLogData[i]);

                        if (strData.Contains("<A 40 CELLID '") == true)
                        {
                            strTemp = strData;

                            nTemp = strTemp.IndexOf("'", 0);
                            if (nTemp >= 0)
                            {
                                strTemp = strTemp.Substring(nTemp + 1);
                            }

                            nTemp = strTemp.IndexOf("'", 0);
                            if (nTemp >= 0)
                            {
                                strTemp = strTemp.Substring(0, nTemp);
                            }

                            strTemp = strTemp.Trim();
                            strAnalysis += LogParsing("", "", strTemp);

                            strTemp = string.Format("LEN : {0}", strTemp.Length);
                            strAnalysis += LogParsing("", "", strTemp);
                        }
                    }
                }
            }

            m_listAnalysis.Add(strAnalysis + CR_LF);

            if (ckbNonRealtime.Checked == false)
            {
                tbLogAnalysis.AppendText(strAnalysis);
                tbLogAnalysis.AppendText(CR_LF);
            }

            return true;
        }

        private bool Analysis_Proc_MSG(string strDate, List<string> listLogData)
        {
            int nPos = -999;
            string strLogData = string.Empty;

            string strMsg = string.Empty;
            string strMsg_Sub = string.Empty;

            string strMsgCheck_1 = string.Empty;
            string strMsgCheck_2 = string.Empty;
            
            //
            //~~~ Validation Check
            // 

            if (listLogData.Count < 2)
            {
                return false;
            }
            else if (listLogData[0].Substring(0, 1) != "[")
            {
                return false;
            }

            if (rbtUnfixed.Checked == true)
            {
                tbMessage.Text = tbMessage.Text.ToUpper();
                strMsg = tbMessage.Text;
                strMsg_Sub = "";
            }
            else
            {
                strMsg = cbMessage.Text;
                strMsg_Sub = cbMessageSub.Text;
            }

            strMsgCheck_1 = strMsg + " ";
            strMsgCheck_2 = strMsg + "W";

            strLogData = listLogData[IDX_MSG_START];

            nPos = strLogData.IndexOf(strMsgCheck_1);
            if (nPos < 0)
            {
                nPos = strLogData.IndexOf(strMsgCheck_2);
                if (nPos < 0)
                {
                    return false;
                }
            }

            //
            //~~~ Log Analysis
            // 

            if (strMsg == MSG_S6F11)         //~~~ EQ State
            {
                strLogData = listLogData[IDX_MSG_START + 3];
                nPos = strLogData.IndexOf(strMsg_Sub);
                if (nPos < 0)
                {
                    return false;
                }
            }

            Analysis_Proc_Result(strDate, listLogData, strMsg, strMsg_Sub);

            return true;
        }

        private void cbEQPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nPos = -1;

            if (cbEQPID.Text == COMBO_INIT)
            {
                m_strEQ_IP = "";

                btnExplorer.Enabled = false;
                btnPing.Enabled = false;
                btnRemote.Enabled = false;
            }
            else
            {
                nPos = cbEQPID.Text.LastIndexOf(" : ");
                m_strEQ_IP = cbEQPID.Text.Substring(nPos + 3);

                btnExplorer.Enabled = true;
                btnPing.Enabled = true;
                btnRemote.Enabled = true;
            }
        }

        private void btnExplorer_Click(object sender, EventArgs e)
        {
            string strArgument = "\\\\" + m_strEQ_IP;

            Process.Start("explorer", strArgument);
        }

        private void btnRemote_Click(object sender, EventArgs e)
        {
            string strArgument = "/v:" + m_strEQ_IP;

            Process.Start("mstsc", strArgument);
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            string strArgument = "-t " + m_strEQ_IP;

            Process.Start("ping", strArgument);
        }

        private void btnALLAnalysis_Click(object sender, EventArgs e)
        {
            cbMessage.Text = "S6F11";
            cbMessageSub.Text = "CEID '401'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '406'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '101'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '606'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '215'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '222'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '223'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessageSub.Text = "CEID '224'";
            btnCimLogAnalysis_Click(sender, e);
            cbMessage.Text = "S5F1";
            cbMessageSub.Text = string.Empty;
            btnCimLogAnalysis_Click(sender, e);
        }
        #endregion CIM Log Analysis End
        
        
    }
}
