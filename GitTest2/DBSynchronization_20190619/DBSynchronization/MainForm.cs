using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoreLib;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace DBSynchronization
{
    public partial class MainForm : Form
    {
        // 변수
        private ConfigData config = new ConfigData();
        private List<TableInfo> tableList = new List<TableInfo>();

        // 생성자
        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            // Config ini
            string fileName = Application.StartupPath.TrimEnd('\\') + "\\Config.ini";
            config.Database1IP = CoreIni.Read(fileName, "DATABASE_1", "IP");
            config.Database1Name = CoreIni.Read(fileName, "DATABASE_1", "NAME");
            config.Database1ID = CoreIni.Read(fileName, "DATABASE_1", "ID");
            config.Database1Pwd = CoreIni.Read(fileName, "DATABASE_1", "PWD");
            config.Database1Security = CoreIni.Read(fileName, "DATABASE_1", "SECURITY");
            config.Database1Type = CoreIni.Read(fileName, "DATABASE_1", "TYPE");
            config.Database2IP = CoreIni.Read(fileName, "DATABASE_2", "IP");
            config.Database2Name = CoreIni.Read(fileName, "DATABASE_2", "NAME");
            config.Database2ID = CoreIni.Read(fileName, "DATABASE_2", "ID");
            config.Database2Pwd = CoreIni.Read(fileName, "DATABASE_2", "PWD");
            config.Database2Security = CoreIni.Read(fileName, "DATABASE_2", "SECURITY");
            config.Database2Type = CoreIni.Read(fileName, "DATABASE_2", "TYPE");
            config.insertInterval = int.Parse(CoreIni.Read(fileName, "INSERT_INTERVAL", "INTERVAL"));

            // ini Table Info
            while (true)
            {
                string tableNumb = string.Format("TABLE_{0}", tableList.Count + 1);
                string tableName = CoreIni.Read(fileName, tableNumb, "TABLE_NAME");
                if (tableName.Equals(string.Empty)) break;
                TableInfo table = new TableInfo();
                table.Name = CoreIni.Read(fileName, tableNumb, "TABLE_NAME");
                table.Key = new List<string>();
                table.Key.Add(CoreIni.Read(fileName, tableNumb, "KEY1"));
                table.Key.Add(CoreIni.Read(fileName, tableNumb, "KEY2"));
                table.Key.Add(CoreIni.Read(fileName, tableNumb, "KEY3"));
                table.Key.Add(CoreIni.Read(fileName, tableNumb, "KEY4"));
                table.Key.Add(CoreIni.Read(fileName, tableNumb, "KEY5"));
                table.SaveType = CoreIni.Read(fileName, tableNumb, "SAVE_TYPE");

                // Table정보 저장
                tableList.Add(table);
            }
            
            // TreeNode UI
            // ini 테이블 확인
            string strPath = Application.StartupPath.TrimEnd('\\').Replace("bin\\Debug", "image\\");

            // 노드 이미지 추가
            ImageList imgList = new ImageList();
            imgList.Images.Add(Bitmap.FromFile(string.Concat(strPath, "table.jpg")));
            imgList.Images.Add(Bitmap.FromFile(string.Concat(strPath, "Column.jpg")));
            imgList.Images.Add(Bitmap.FromFile(string.Concat(strPath, "PKcolumn.jpg")));
            tvwTables.ImageList = imgList;

            // DB 연결
            string sql2ConnectStr = string.Format("DATA SOURCE={0}; INITIAL CATALOG={1}; UID={2};PWD={3}; INTEGRATED SECURITY={4}; CONNECTION TIMEOUT=1",
                                         config.Database1IP, config.Database1Name, config.Database1ID, config.Database1Pwd, config.Database1Security);
            CoreSql sql1 = new CoreSql();
            if (!sql1.Connect(sql2ConnectStr))
            {
                sql1.Disconnect();
                WriteLog(Color.Red, "Error : CAN NOT CONNECT DATABASE.");
                return;
            }

            // 노드 추가
            for (int i = 0; i < tableList.Count; i++)
            {
                // 테이블노드 추가
                string nodeText = string.Format("{0} (SaveType : {1})", tableList[i].Name, tableList[i].SaveType);
                TreeNode node = new TreeNode(nodeText);
                node.Name = tableList[i].Name;
                tvwTables.Nodes.Add(node);
                tvwTables.Nodes[i].ImageIndex = 0;

                // 컬럼 조회
                string selectColumn = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS "
                    + "WHERE TABLE_NAME = '{0}'", tableList[i].Name);
                SqlDataReader readColumn = sql1.ExecuteReader(selectColumn);
                if (readColumn == null)
                {
                    sql1.Disconnect();
                    WriteLog(Color.Red, string.Format("Error : CAN NOT PK Position EXECUTE READER."));
                    return;
                }

                // 컬럼노드 추가
                while (readColumn.Read())
                {
                    TreeNode nodeColumn = new TreeNode(readColumn.GetString(0));
                    nodeColumn.Name = readColumn.GetString(0);
                    tvwTables.Nodes[i].Nodes.Add(nodeColumn);
                    tvwTables.Nodes[i].Nodes[nodeColumn.Name].ImageIndex = 1;
                    tvwTables.Nodes[i].Nodes[nodeColumn.Name].SelectedImageIndex = 1;
                }
                readColumn.Close();

                // DB PK조회
                string selectKey = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE "
                    + "WHERE TABLE_NAME='{0}' ORDER BY ORDINAL_POSITION ASC", tableList[i].Name);
                SqlDataReader readerKey = sql1.ExecuteReader(selectKey);
                if (readerKey == null)
                {
                    sql1.Disconnect();
                    WriteLog(Color.Red, "Error : CAN NOT PK EXECUTE READER.");
                    return;
                }

                // DB PK읽기
                tableList[i].KeyName = new List<string>();
                while (readerKey.Read())
                {
                    for (int j = 0; j < tvwTables.Nodes[i].Nodes.Count; j++)
                    {
                        if (tvwTables.Nodes[i].Nodes[j].Name == readerKey.GetString(0))
                        {
                            tvwTables.Nodes[i].Nodes[j].ImageIndex = 2;
                            tvwTables.Nodes[i].Nodes[j].SelectedImageIndex = 2;
                        }
                    }
                }
                readerKey.Close();
            }
            sql1.Disconnect();
            tvwTables.ExpandAll();

            tvwTables.Nodes[0].EnsureVisible();
            tvwTables.Nodes[0].ExpandAll();
            
            // 타이머 시작
            tmrDbCopy.Interval = config.insertInterval;
            tmrDbCopy.Start();
        }

        // 타이머
        private void tmrDbCopy_Tick(object sender, EventArgs e)
        {
            // DB 연결 쿼리
            string sql1ConnectStr = string.Format("DATA SOURCE={0}; INITIAL CATALOG={1}; UID={2};PWD={3}; INTEGRATED SECURITY={4}; CONNECTION TIMEOUT=1",
                                                     config.Database1IP, config.Database1Name, config.Database1ID, config.Database1Pwd, config.Database1Security);
            string sql2ConnectStr = string.Format("DATA SOURCE={0}; INITIAL CATALOG={1}; UID={2};PWD={3}; INTEGRATED SECURITY={4}; CONNECTION TIMEOUT=1",
                                         config.Database2IP, config.Database2Name, config.Database2ID, config.Database2Pwd, config.Database2Security);
            CoreSql sql1 = new CoreSql();
            CoreSql sql2 = new CoreSql();

            // TABLE 동기화
            for (int i = 0; i < tableList.Count; i++)
            {
                // DB 연결
                if (!sql1.Connect(sql1ConnectStr) || !sql2.Connect(sql2ConnectStr))
                {
                    sql1.Disconnect();
                    sql2.Disconnect();
                    WriteLog(Color.Red, "CAN NOT CONNECT DATABASE");
                    return;
                }

                switch (tableList[i].SaveType)
                {
                    case "INSERT":
                        // PK List 초기화
                        if (tableList[i].KeyName != null) tableList[i].KeyName.Clear();
                        if (tableList[i].KeyIndex != null) tableList[i].KeyIndex.Clear();

                        // DB PK조회
                        string selectKey = string.Format("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE "
                            + "WHERE TABLE_NAME='{0}' ORDER BY ORDINAL_POSITION ASC", tableList[i].Name);
                        SqlDataReader readerKey = sql1.ExecuteReader(selectKey);
                        if (readerKey == null)
                        {
                            sql1.Disconnect();
                            sql2.Disconnect();
                            WriteLog(Color.Red, "CAN NOT PK EXECUTE READER");
                            continue;
                        }

                        // DB PK읽기
                        tableList[i].KeyName = new List<string>();
                        while (readerKey.Read())
                        {
                            tableList[i].KeyName.Add(readerKey.GetString(0));
                        }
                        readerKey.Close();

                        // DB PK Index 조회
                        string selectKeyPosition = string.Format("SELECT COLUMN_NAME,ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS "
                            + "WHERE TABLE_NAME = '{0}'", tableList[i].Name);
                        SqlDataReader readKeyIndex = sql1.ExecuteReader(selectKeyPosition);
                        if (readKeyIndex == null)
                        {
                            sql1.Disconnect();
                            sql2.Disconnect();
                            WriteLog(Color.Red, "CAN NOT PK Position EXECUTE READER");
                            continue;
                        }

                        // DB PK Index 읽기
                        tableList[i].KeyIndex = new List<int>();
                        while (readKeyIndex.Read())
                        {
                            string columnName = readKeyIndex.GetString(0);
                            int columnIndex = readKeyIndex.GetInt32(1);
                            for (int j = 0; j < tableList[i].KeyName.Count; j++)
                            {
                                if (tableList[i].KeyName[j] == columnName)
                                {
                                    tableList[i].KeyIndex.Add(columnIndex);
                                    break;
                                }
                            }
                        }
                        readKeyIndex.Close();

                        // DB 조회 쿼리
                        string selectQuery = string.Empty;
                        switch (tableList[i].KeyName.Count)
                        {
                            case 1:
                                selectQuery = string.Format("SELECT TOP 1000 * FROM {0} WHERE ({1} > '{2}')", tableList[i].Name, tableList[i].KeyName[0], tableList[i].Key[0]);
                                break;
                            case 2:
                                selectQuery = string.Format("SELECT TOP 1000 * FROM {0} WHERE ({1} > '{2}')"
                                + "OR ({1} = '{2}' AND {3} > '{4}') ORDER BY {1},{3}"
                                , tableList[i].Name, tableList[i].KeyName[0], tableList[i].Key[0], tableList[i].KeyName[1], tableList[i].Key[1]);
                                break;
                            case 3:
                                selectQuery = string.Format("SELECT TOP 1000 * FROM {0} WHERE ({1} > '{2}')"
                                + " OR ({1} = '{2}' AND {3} > '{4}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} > '{6}') ORDER BY {1},{3},{5}"
                                , tableList[i].Name, tableList[i].KeyName[0], tableList[i].Key[0], tableList[i].KeyName[1], tableList[i].Key[1], tableList[i].KeyName[2], tableList[i].Key[2]);
                                break;
                            case 4:
                                selectQuery = string.Format("SELECT TOP 1000 * FROM {0} WHERE ({1} > '{2}')"
                                + " OR ({1} = '{2}' AND {3} > '{4}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} > '{6}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} = '{6}' AND {7} > '{8}') ORDER BY {1},{3},{5},{7}"
                                , tableList[i].Name, tableList[i].KeyName[0], tableList[i].Key[0], tableList[i].KeyName[1], tableList[i].Key[1], tableList[i].KeyName[2], tableList[i].Key[2]
                                , tableList[i].KeyName[3], tableList[i].Key[3]);
                                break;
                            case 5:
                                selectQuery = string.Format("SELECT TOP 1000 * FROM {0} WHERE ({1} > '{2}')"
                                + " OR ({1} = '{2}' AND {3} > '{4}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} > '{6}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} = '{6}' AND {7} > '{8}')"
                                + " OR ({1} = '{2}' AND {3} = '{4}' AND {5} = '{6}' AND {7} = '{8}' AND {9} > '{10}') ORDER BY {1},{3},{5},{7},{9}"
                                , tableList[i].Name, tableList[i].KeyName[0], tableList[i].Key[0], tableList[i].KeyName[1], tableList[i].Key[1], tableList[i].KeyName[2], tableList[i].Key[2]
                                , tableList[i].KeyName[3], tableList[i].Key[3], tableList[i].KeyName[4], tableList[i].Key[4]);
                                break;
                        }

                        // DB 조회
                        SqlDataReader selectReader = sql1.ExecuteReader(selectQuery);
                        if (selectReader == null)
                        {
                            sql1.Disconnect();
                            sql2.Disconnect();
                            WriteLog(Color.Red, "CAN NOT EXECUTE READER.");
                            continue;
                        }

                        // DB 읽기
                        int insertCount = 0;
                        int InsertMissCount = 0;
                        while (selectReader.Read())
                        {
                            // Insert 쿼리
                            StringBuilder insertQuery = new StringBuilder(string.Format("INSERT INTO {0} VALUES(", tableList[i].Name));
                            for (int j = 0; j < selectReader.FieldCount; j++)
                            {
                                insertQuery.Append(string.Format("'{0}',", selectReader.GetString(j)));

                                // KEY값 저장
                                if (tableList[i].KeyIndex.Contains(j + 1))
                                {
                                    int index = tableList[i].KeyIndex.IndexOf(j + 1);
                                    tableList[i].Key[index] = selectReader.GetString(j);
                                }
                            }
                            insertQuery.Replace(",", ")", insertQuery.Length - 1, 1);

                            // DB Insert
                            int sqlResult = sql2.ExecuteNonQuery(insertQuery.ToString());
                            if (sqlResult == 1) insertCount++;
                            else InsertMissCount++;
                        }
                        selectReader.Close();

                        // 동기화 체크
                        string sqlqueryCount = string.Format("SELECT ROWS FROM SYS.SYSINDEXES WHERE ID = OBJECT_ID('{0}') AND INDID <2", tableList[i].Name);
                        SqlDataReader sqlCount1 = sql1.ExecuteReader(sqlqueryCount);
                        sqlCount1.Read();
                        int table1Count = sqlCount1.GetInt32(0);
                        sqlCount1.Close();
                        SqlDataReader sqlCount2 = sql2.ExecuteReader(sqlqueryCount);
                        sqlCount2.Read();
                        int table2Count = sqlCount2.GetInt32(0);
                        sqlCount2.Close();
                        if (insertCount > 0)
                        {
                            WriteLog(Color.Black, string.Format("[{0}] INSERT : {1}", tableList[i].Name, insertCount));
                        }
                        if (InsertMissCount > 0)
                        {
                            WriteLog(Color.Red, string.Format("[{0}] CAN NOT INSERT : {1}", tableList[i].Name, InsertMissCount));
                        }

                        if (table1Count == table2Count)
                        {

                        }

                        // KEY값 ini저장
                        string fileName = Application.StartupPath.TrimEnd('\\') + "\\Config.ini";
                        string iniSection = string.Format("TABLE_{0}", i + 1);
                        CoreIni.Write(fileName, iniSection, "KEY1", tableList[i].Key[0]);
                        CoreIni.Write(fileName, iniSection, "KEY2", tableList[i].Key[1]);
                        CoreIni.Write(fileName, iniSection, "KEY3", tableList[i].Key[2]);
                        CoreIni.Write(fileName, iniSection, "KEY4", tableList[i].Key[3]);
                        CoreIni.Write(fileName, iniSection, "KEY5", tableList[i].Key[4]);
                        break;

                    case "BULK":

                        // 조회
                        string copySelectQuery = string.Format("select * from {0}", tableList[i].Name);
                        DataTable bulkTable = sql1.ExecuteDataTable(copySelectQuery);
                        if (bulkTable == null)
                        {
                            sql1.Disconnect();
                            sql2.Disconnect();
                            WriteLog(Color.Red, "Error : CAN NOT EXECUTE DATATABLE.");
                            continue;
                        }
                        if (bulkTable.Rows.Count > 10000)
                        {
                            sql1.Disconnect();
                            sql2.Disconnect();
                            WriteLog(Color.Red, string.Format("[{0}] ROW 10000 COUNT OVER", tableList[i].Name));
                            continue;
                        }

                        // Table Delete
                        string tableDeleteQuery = string.Format("TRUNCATE TABLE {0}", tableList[i].Name);
                        int deleteResult = sql2.ExecuteNonQuery(tableDeleteQuery);

                        // Bulk Insert
                        int bulkCopyResult = sql2.BulkCopy(bulkTable, tableList[i].Name);
                        if (bulkCopyResult == 1)
                        {
                            WriteLog(Color.Black, string.Format("[{0}] BULK : OK", tableList[i].Name));
                        }
                        else
                        {
                            WriteLog(Color.Red, string.Format("[{0}] DATATABLE BULK 실패", tableList[i].Name));
                            continue;
                        }

                        // BULK KEY값 "" ini저장
                        string filepath = Application.StartupPath.TrimEnd('\\') + "\\Config.ini";
                        string section = string.Format("TABLE_{0}", i + 1);
                        CoreIni.Write(filepath, section, "KEY1", string.Empty);
                        CoreIni.Write(filepath, section, "KEY2", string.Empty);
                        CoreIni.Write(filepath, section, "KEY3", string.Empty);
                        CoreIni.Write(filepath, section, "KEY4", string.Empty);
                        CoreIni.Write(filepath, section, "KEY5", string.Empty);
                        break;
                }
            }

            // DB 해제
            sql1.Disconnect();
            sql2.Disconnect();
        }

        // Log UI
        private void WriteLog(Color color, string logText)
        {
            // 메시지
            string msg = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + logText + "\r\n";

            // UI 출력
            rtbLog.Select(rtbLog.Text.Length, 0);
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(msg);
            rtbLog.ScrollToCaret();
            rtbLog.Update();

            // Log
            CoreLog log = new CoreLog();
            log.LogPath = Application.StartupPath.TrimEnd('\\') + "\\Log";
            log.WriteLog(logText);
            log.Dispose();
        }

        // 메뉴 버튼
        private void openIniCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = Application.StartupPath.TrimEnd('\\') + "\\Config.ini";
            System.Diagnostics.Process.Start(fileName);
        }
        private void viewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = Application.StartupPath.TrimEnd('\\') + "\\Log";
            System.Diagnostics.Process.Start(fileName);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void LogClaenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }
    }
}
