using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace CIM_Log_Analysis
{
    public partial class frmCimLogAnalysis : Form
    {
        /// <summary>
        /// 
        ///  2017-07-23
        ///  
        /// </summary>

        private List<string> m_listLogFile = new List<string>();

        private List<string> m_listAnalysis = new List<string>();

        private List<string> m_listCELL_ID = new List<string>();

        private int m_nLogItemCnt = 0;

        private bool m_bOnlyCountInfo = false;

        private string m_strEQ_IP = string.Empty;

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        public frmCimLogAnalysis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private bool Analysis_Init()
        {
            cbEQPID.Items.Add(CONST.COMBO_INIT);
            cbEQPID.Items.Add(CONST.EQ_01_01_LOCAL);
            cbEQPID.Items.Add(CONST.COMBO_INIT);
            cbEQPID.SelectedIndex = 0;

            cbMessage.Items.Add(CONST.COMBO_INIT);
            cbMessage.Items.Add(CONST.MSG_S6F11);             //~~~ EQ State
            cbMessage.Items.Add(CONST.COMBO_INIT);
            cbMessage.Items.Add(CONST.MSG_S5F1);              //~~~ Alarm
            cbMessage.Items.Add(CONST.COMBO_INIT);
            cbMessage.SelectedIndex = 0;

            cbMessageSub.Items.Add(CONST.COMBO_INIT);
            cbMessageSub.Items.Add(CONST.MSG_CEID_401);   //~~~ Cell In
            cbMessageSub.Items.Add(CONST.MSG_CEID_406);   //~~~ Cell Out
            cbMessageSub.Items.Add(CONST.COMBO_INIT);
            cbMessageSub.Items.Add(CONST.MSG_CEID_101);   //~~~ EQ State Change
            cbMessageSub.Items.Add(CONST.MSG_CEID_606);   //~~~ TPM Loss
            cbMessageSub.Items.Add(CONST.COMBO_INIT);
            cbMessageSub.Items.Add(CONST.MSG_CEID_200);   //~~~ Material Change
            cbMessageSub.Items.Add(CONST.MSG_CEID_215);   //~~~ Material Assemble Process
            cbMessageSub.Items.Add(CONST.MSG_CEID_222);   //~~~ Material NG Process
            cbMessageSub.Items.Add(CONST.COMBO_INIT);
            cbMessageSub.SelectedIndex = 0;

            rbtFixed.Checked = true;
            tbRangeStart.Text = "1";
            tbRangeEnd.Text = "100";

            ckbNonRealtime.Checked = true;

            return true;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

            nPos = strDate.LastIndexOf("_20");
            if (nPos >= 0)
            {
                strDate = strDate.Substring(nPos + 1);
            }

            //~~~ Date Info Parsing

            strReturn = string.Format("{0:0000}-{1:00}-{2:00}", strDate.Substring(0, 4), strDate.Substring(4, 2), strDate.Substring(6, 2));

            return strReturn;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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


        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private bool LoadLogFileList()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Multiselect = true;
            ofd.InitialDirectory = "\\\\" + m_strEQ_IP + "\\" + CONST.CIM_LOG_FOLDER;

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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void frmCimLogAnalysis_Load(object sender, EventArgs e)
        {
            Analysis_Init();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void cbMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbMessage.Text)
            {
                case CONST.MSG_S6F11:
                    cbMessageSub.SelectedIndex = 0;
                    cbMessageSub.Enabled = true;
                    break;

                case CONST.MSG_S5F1:
                    cbMessageSub.SelectedIndex = 0;
                    cbMessageSub.Enabled = false;
                    break;

                default:
                    cbMessageSub.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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
                strData = string.Format("#{0}> {1}{2}", i + 1, m_listLogFile[i], CONST.CR_LF);

                tbLogFile.AppendText(strData);

                m_listAnalysis.Add("\t\t\t\t" + strData);
            }

            m_listAnalysis.Add(CONST.CR_LF);

            tbLogAnalysis.Clear();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            m_listAnalysis.Clear();

            tbLogAnalysis.Clear();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnCount_Click(object sender, EventArgs e)
        {
            m_bOnlyCountInfo = true;

            Analysis_Proc();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnCimLogAnalysis_Click(object sender, EventArgs e)
        {
            m_bOnlyCountInfo = false;

            Analysis_Proc();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private bool ExportToExcel()
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

                streamWriter.Write(strData);
            }

            streamWriter.Close();

            Process.Start("Excel.exe", strPath + "\\" + strFile);

            return true;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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
                strData = string.Format("\t\t\t\t*** {0} : {1} : ({2}) {3}", cbMessage.Text, cbMessageSub.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CONST.CR_LF);
            }
            else
            {
                strData = string.Format("\t\t\t\t*** {0} : ({1}) {2}", cbMessage.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CONST.CR_LF);
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
                strData = string.Format("\t\t\t\t*** {0} : {1} : ({2}) : {3} ea {4}", cbMessage.Text, cbMessageSub.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_nLogItemCnt, CONST.CR_LF + CONST.CR_LF);
            }
            else
            {
                strData = string.Format("\t\t\t\t*** {0} : ({1}) : {2} ea {3}", cbMessage.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), m_nLogItemCnt, CONST.CR_LF + CONST.CR_LF);
            }

            m_listAnalysis.Add(strData);

            tbLogAnalysis.AppendText(strData);

            tbLogItemCount.Text = string.Format("{0} EA", m_nLogItemCnt);

            return true;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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
            strAnalysis += LogParsing("<", "", listLogData[CONST.IDX_MSG_START]);   //~~~ <S5F1 W Alarm Report Send

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
                for (int i = CONST.IDX_MSG_START + 1; i < listLogData.Count; ++i)
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

            m_listAnalysis.Add(strAnalysis + CONST.CR_LF);

            if (ckbNonRealtime.Checked == false)
            {
                tbLogAnalysis.AppendText(strAnalysis);
                tbLogAnalysis.AppendText(CONST.CR_LF);
            }

            return true;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
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

            strLogData = listLogData[CONST.IDX_MSG_START];

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

            if (strMsg == CONST.MSG_S6F11)         //~~~ EQ State
            {
                strLogData = listLogData[CONST.IDX_MSG_START + 3];
                nPos = strLogData.IndexOf(strMsg_Sub);
                if (nPos < 0)
                {
                    return false;
                }
            }

            Analysis_Proc_Result(strDate, listLogData, strMsg, strMsg_Sub);

            return true;
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void cbEQPID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nPos = -1;

            if (cbEQPID.Text == CONST.COMBO_INIT)
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

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnExplorer_Click(object sender, EventArgs e)
        {
            string strArgument = "\\\\" + m_strEQ_IP;

            Process.Start("explorer", strArgument);
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnRemote_Click(object sender, EventArgs e)
        {
            string strArgument = "/v:" + m_strEQ_IP;

            Process.Start("mstsc", strArgument);
        }

        /// <summary>
        /// 
        ///  2017-07-23
        /// 
        /// </summary>
        private void btnPing_Click(object sender, EventArgs e)
        {
            string strArgument = "-t " + m_strEQ_IP;

            Process.Start("ping", strArgument);
        }

    }
}
