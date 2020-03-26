using System;
using System.Text;
using System.Windows.Forms;

namespace Virtual_AGV
{
    public partial class TestForm : Form
    {
        // 변수 선언
        private ushort[] mainWord { get; set; }

        // 생성자
        public TestForm()
        {
            InitializeComponent();
        }

        // 외부함수
        public void wordMemoryInput(ushort[] wordArray)
        {
            this.mainWord = wordArray;
        }

        // PosSet Test
        private void btnWordComm_Click(object sender, EventArgs e)
        {
            ushort posWayPoint = ushort.Parse(txtWayPoint.Text);
            ushort posDestination = ushort.Parse(txtDestination.Text);

            mainWord[312] = posWayPoint;
            mainWord[313] = posDestination;
        }

        //Destination Test
        private void btnDestinationOn_Click(object sender, EventArgs e)
        {
            mainWord[310] = 1;
        }
        private void btnDestinationOff_Click(object sender, EventArgs e)
        {
            mainWord[310] = 0;
        }

        //Resume Test
        private void btnResumeOn_Click(object sender, EventArgs e)
        {
            mainWord[320] = 1;
        }
        private void btnResumeOff_Click(object sender, EventArgs e)
        {
            mainWord[320] = 0;
        }

        //Pause Test
        private void btnPauseOn_Click(object sender, EventArgs e)
        {
            mainWord[330] = 1;
        }
        private void btnPauseOff_Click(object sender, EventArgs e)
        {
            mainWord[330] = 0;
        }
        
    }
}
