using System;
using System.Collections.Generic;
using System.Text;

namespace CIM_Log_Analysis
{
    public static class CONST
    {
        public const int IDX_MSG_START = 1;

        //public const string EQ_01_01_LOCAL = "Local : 192.168.0.40";
        public const string EQ_01_01_LOCAL = "Local : 127.0.0.1";

        public const string CIM_LOG_FOLDER = "SDCCIM_LOG";

        public const string CR_LF = "\r\n";

        public const string COMBO_INIT = "---------------";

        public const string MSG_S6F11 = "S6F11";                //~~~ EQ State
        public const string MSG_S5F1  = "S5F1";                 //~~~ Alarm
        public const string MSG_INPUT = "S-F-";                 //~~~ User Input

        public const string MSG_CEID_401 = "CEID '401'";       //~~~ Cell In
        public const string MSG_CEID_406 = "CEID '406'";       //~~~ Cell Out
        public const string MSG_CEID_101 = "CEID '101'";       //~~~ EQ State Change
        public const string MSG_CEID_200 = "CEID '200'";       //~~~ Material Change
        public const string MSG_CEID_215 = "CEID '215'";       //~~~ Material Assemble Process
        public const string MSG_CEID_222 = "CEID '222'";       //~~~ Material NG Process
        public const string MSG_CEID_606 = "CEID '606'";       //~~~ TPM Loss
    }
}
