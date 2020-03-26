using System;
using System.Collections.Generic;
using System.Text;

namespace Virtual_AGV
{
    public class ConfigData
    {
        // Base Info
        public string Type { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }

        // Log Info
        public string LogPath { get; set; }
        public int LogDateLimit { get; set; }
    }
}
