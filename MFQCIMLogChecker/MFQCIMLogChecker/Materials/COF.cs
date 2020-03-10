using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFQCIMLogChecker
{
    class COF
    {
        public string materialID { get; set; }
        public bool setCheck { get; set; }
        public string materialType = "COF";
        public int totalQty { get; set; }
        public int useQty { get; set; }
        public int assembleQty { get; set; }
        public int totalNGQty { get; set; }
        public int pUseQty { get; set; }
        public int pNGQty { get; set; }
        public int pAssembleQty { get; set; }
        public int remainQty { get; set; }
    }
}
