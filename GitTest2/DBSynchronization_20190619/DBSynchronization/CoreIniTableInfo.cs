using System;
using System.Collections.Generic;
using System.Text;

namespace DBSynchronization
{
    class TableInfo
    {
        // Table Info
        public string Name { get; set; }

        // Table Key
        public List<string> Column { get; set; }
        public List<string> KeyName { get; set; }
        public List<string> Key { get; set; }
        public List<int> KeyIndex { get; set; }
        public string SaveType { get; set; }
    }
}
