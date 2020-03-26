using System;
using System.Collections.Generic;
using System.Text;

namespace DBSynchronization
{
    class ConfigData
    {
        // DB Info
        public string Database1IP { get; set; }
        public string Database2IP { get; set; }
        public string Database1Name { get; set; }
        public string Database2Name { get; set; }
        public string Database1ID { get; set; }
        public string Database2ID { get; set; }
        public string Database1Pwd { get; set; }
        public string Database2Pwd { get; set; }
        public string Database1Security { get; set; }
        public string Database2Security { get; set; }
        public string Database1Type { get; set; }
        public string Database2Type { get; set; }


        // DB CopyTime Set
        public int insertInterval { get; set; }
    }
}
