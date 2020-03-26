using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using CoreLib;

namespace Virtual_AGV
{
    public partial class MainForm : Form
    {
        // 내부변수
        private ConfigData config = new ConfigData();
        private CoreServer server = new CoreServer();
        private CoreLog log = new CoreLog();
        private int movingCount = 0;
        private byte[] spareBuffer = new byte[0];
        private ushort[] wordArray = new ushort[500];
        private List<PositionData> posInfoList = new List<PositionData>();

        // 생성자
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // 언어설정(영문:en-US/한글:ko-KR);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            // Config.ini
            string fileName = Application.StartupPath.TrimEnd('\\') + "\\Config.ini";
            config.Type = CoreIni.Read(fileName, "INFO", "TYPE");
            config.IP = CoreIni.Read(fileName, "INFO", "IP");
            config.Port = CoreParse.ToInt(CoreIni.Read(fileName, "INFO", "PORT"));
            config.LogPath = CoreIni.Read(fileName, "LOG", "LOG_PATH");
            config.LogDateLimit = CoreParse.ToInt(CoreIni.Read(fileName, "LOG", "LOG_DATE_LIMIT"));

            // Log 설정
            log.LogPath = config.LogPath.TrimEnd('\\');
            log.LogDateLimit = config.LogDateLimit;
            log.FileFormat = "yyyyMMdd_HH" + "'.log'";
            log.HeaderFormat = "";

            #region # Position 정보
            //// Position 정보
            //PositionData pos3200 = new PositionData(3200); posInfoList.Add(pos3200);
            //PositionData pos3210 = new PositionData(3210); posInfoList.Add(pos3210);
            //PositionData pos3220 = new PositionData(3220); posInfoList.Add(pos3220);
            //PositionData pos3230 = new PositionData(3230); posInfoList.Add(pos3230);
            //PositionData pos3240 = new PositionData(3240); posInfoList.Add(pos3240);
            //PositionData pos3290 = new PositionData(3290); posInfoList.Add(pos3290);
            //PositionData pos3350 = new PositionData(3350); posInfoList.Add(pos3350);
            //PositionData pos3360 = new PositionData(3360); posInfoList.Add(pos3360);
            //PositionData pos3370 = new PositionData(3370); posInfoList.Add(pos3370);
            //PositionData pos3380 = new PositionData(3380); posInfoList.Add(pos3380);
            //PositionData pos3400 = new PositionData(3400); posInfoList.Add(pos3400);
            //PositionData pos3490 = new PositionData(3490); posInfoList.Add(pos3490);
            //PositionData pos3600 = new PositionData(3600); posInfoList.Add(pos3600);
            //PositionData pos3610 = new PositionData(3610); posInfoList.Add(pos3610);
            //PositionData pos3620 = new PositionData(3620); posInfoList.Add(pos3620);
            //PositionData pos3630 = new PositionData(3630); posInfoList.Add(pos3630);
            //PositionData pos3640 = new PositionData(3640); posInfoList.Add(pos3640);
            //PositionData pos3690 = new PositionData(3690); posInfoList.Add(pos3690);
            //PositionData pos3800 = new PositionData(3800); posInfoList.Add(pos3800);
            //PositionData pos3890 = new PositionData(3890); posInfoList.Add(pos3890);
            //PositionData pos3900 = new PositionData(3900); posInfoList.Add(pos3900);
            //PositionData pos3910 = new PositionData(3910); posInfoList.Add(pos3910);
            //PositionData pos3920 = new PositionData(3920); posInfoList.Add(pos3920);
            //PositionData pos3930 = new PositionData(3930); posInfoList.Add(pos3930);
            //PositionData pos3940 = new PositionData(3940); posInfoList.Add(pos3940);
            //PositionData pos4000 = new PositionData(4000); posInfoList.Add(pos4000);
            //PositionData pos4010 = new PositionData(4010); posInfoList.Add(pos4010);
            //PositionData pos4020 = new PositionData(4020); posInfoList.Add(pos4020);
            //PositionData pos4030 = new PositionData(4030); posInfoList.Add(pos4030);
            //PositionData pos4040 = new PositionData(4040); posInfoList.Add(pos4040);
            //PositionData pos4090 = new PositionData(4090); posInfoList.Add(pos4090);
            //PositionData pos4100 = new PositionData(4100); posInfoList.Add(pos4100);
            //PositionData pos4200 = new PositionData(4200); posInfoList.Add(pos4200);
            //PositionData pos4290 = new PositionData(4290); posInfoList.Add(pos4290);
            //PositionData pos4300 = new PositionData(4300); posInfoList.Add(pos4300);
            //PositionData pos4310 = new PositionData(4310); posInfoList.Add(pos4310);
            //PositionData pos4311 = new PositionData(4311); posInfoList.Add(pos4311);
            //PositionData pos4312 = new PositionData(4312); posInfoList.Add(pos4312);
            //PositionData pos4313 = new PositionData(4313); posInfoList.Add(pos4313);
            //PositionData pos4314 = new PositionData(4314); posInfoList.Add(pos4314);
            //PositionData pos4315 = new PositionData(4315); posInfoList.Add(pos4315);
            //PositionData pos4316 = new PositionData(4316); posInfoList.Add(pos4316);
            //PositionData pos4317 = new PositionData(4317); posInfoList.Add(pos4317);
            //PositionData pos4318 = new PositionData(4318); posInfoList.Add(pos4318);
            //PositionData pos4319 = new PositionData(4319); posInfoList.Add(pos4319);
            //PositionData pos4320 = new PositionData(4320); posInfoList.Add(pos4320);
            //PositionData pos4321 = new PositionData(4321); posInfoList.Add(pos4321);
            //PositionData pos4322 = new PositionData(4322); posInfoList.Add(pos4322);
            //PositionData pos4323 = new PositionData(4323); posInfoList.Add(pos4323);
            //PositionData pos4324 = new PositionData(4324); posInfoList.Add(pos4324);
            //PositionData pos4325 = new PositionData(4325); posInfoList.Add(pos4325);
            //PositionData pos4326 = new PositionData(4326); posInfoList.Add(pos4326);
            //PositionData pos4327 = new PositionData(4327); posInfoList.Add(pos4327);
            //PositionData pos4328 = new PositionData(4328); posInfoList.Add(pos4328);
            //PositionData pos4329 = new PositionData(4329); posInfoList.Add(pos4329);
            //PositionData pos4330 = new PositionData(4330); posInfoList.Add(pos4330);
            //PositionData pos4331 = new PositionData(4331); posInfoList.Add(pos4331);
            //PositionData pos4332 = new PositionData(4332); posInfoList.Add(pos4332);
            //PositionData pos4333 = new PositionData(4333); posInfoList.Add(pos4333);
            //PositionData pos4334 = new PositionData(4334); posInfoList.Add(pos4334);
            //PositionData pos4335 = new PositionData(4335); posInfoList.Add(pos4335);
            //PositionData pos4336 = new PositionData(4336); posInfoList.Add(pos4336);
            //PositionData pos4337 = new PositionData(4337); posInfoList.Add(pos4337);
            //PositionData pos4338 = new PositionData(4338); posInfoList.Add(pos4338);
            //PositionData pos4339 = new PositionData(4339); posInfoList.Add(pos4339);
            //PositionData pos4340 = new PositionData(4340); posInfoList.Add(pos4340);
            //PositionData pos4341 = new PositionData(4341); posInfoList.Add(pos4341);
            //PositionData pos4342 = new PositionData(4342); posInfoList.Add(pos4342);
            //PositionData pos4343 = new PositionData(4343); posInfoList.Add(pos4343);
            //PositionData pos4400 = new PositionData(4400); posInfoList.Add(pos4400);
            //PositionData pos4410 = new PositionData(4410); posInfoList.Add(pos4410);
            //PositionData pos4411 = new PositionData(4411); posInfoList.Add(pos4411);
            //PositionData pos4412 = new PositionData(4412); posInfoList.Add(pos4412);
            //PositionData pos4413 = new PositionData(4413); posInfoList.Add(pos4413);
            //PositionData pos4414 = new PositionData(4414); posInfoList.Add(pos4414);
            //PositionData pos4415 = new PositionData(4415); posInfoList.Add(pos4415);
            //PositionData pos4416 = new PositionData(4416); posInfoList.Add(pos4416);
            //PositionData pos4417 = new PositionData(4417); posInfoList.Add(pos4417);
            //PositionData pos4418 = new PositionData(4418); posInfoList.Add(pos4418);
            //PositionData pos4419 = new PositionData(4419); posInfoList.Add(pos4419);
            //PositionData pos4420 = new PositionData(4420); posInfoList.Add(pos4420);
            //PositionData pos4421 = new PositionData(4421); posInfoList.Add(pos4421);
            //PositionData pos4422 = new PositionData(4422); posInfoList.Add(pos4422);
            //PositionData pos4423 = new PositionData(4423); posInfoList.Add(pos4423);
            //PositionData pos4424 = new PositionData(4424); posInfoList.Add(pos4424);
            //PositionData pos4425 = new PositionData(4425); posInfoList.Add(pos4425);
            //PositionData pos4426 = new PositionData(4426); posInfoList.Add(pos4426);
            //PositionData pos4427 = new PositionData(4427); posInfoList.Add(pos4427);
            //PositionData pos4428 = new PositionData(4428); posInfoList.Add(pos4428);
            //PositionData pos4429 = new PositionData(4429); posInfoList.Add(pos4429);
            //PositionData pos4430 = new PositionData(4430); posInfoList.Add(pos4430);
            //PositionData pos4431 = new PositionData(4431); posInfoList.Add(pos4431);
            //PositionData pos4432 = new PositionData(4432); posInfoList.Add(pos4432);
            //PositionData pos4433 = new PositionData(4433); posInfoList.Add(pos4433);
            //PositionData pos4434 = new PositionData(4434); posInfoList.Add(pos4434);
            //PositionData pos4435 = new PositionData(4435); posInfoList.Add(pos4435);
            //PositionData pos4436 = new PositionData(4436); posInfoList.Add(pos4436);
            //PositionData pos4437 = new PositionData(4437); posInfoList.Add(pos4437);
            //PositionData pos4438 = new PositionData(4438); posInfoList.Add(pos4438);
            //PositionData pos4439 = new PositionData(4439); posInfoList.Add(pos4439);
            //PositionData pos4440 = new PositionData(4440); posInfoList.Add(pos4440);
            //PositionData pos4441 = new PositionData(4441); posInfoList.Add(pos4441);
            //PositionData pos4442 = new PositionData(4442); posInfoList.Add(pos4442);
            //PositionData pos4443 = new PositionData(4443); posInfoList.Add(pos4443);
            //PositionData pos4490 = new PositionData(4490); posInfoList.Add(pos4490);
            //PositionData pos4500 = new PositionData(4500); posInfoList.Add(pos4500);
            //PositionData pos4600 = new PositionData(4600); posInfoList.Add(pos4600);
            //PositionData pos4690 = new PositionData(4690); posInfoList.Add(pos4690);
            //PositionData pos4700 = new PositionData(4700); posInfoList.Add(pos4700);
            //PositionData pos4710 = new PositionData(4710); posInfoList.Add(pos4710);
            //PositionData pos4711 = new PositionData(4711); posInfoList.Add(pos4711);
            //PositionData pos4712 = new PositionData(4712); posInfoList.Add(pos4712);
            //PositionData pos4713 = new PositionData(4713); posInfoList.Add(pos4713);
            //PositionData pos4714 = new PositionData(4714); posInfoList.Add(pos4714);
            //PositionData pos4715 = new PositionData(4715); posInfoList.Add(pos4715);
            //PositionData pos4716 = new PositionData(4716); posInfoList.Add(pos4716);
            //PositionData pos4717 = new PositionData(4717); posInfoList.Add(pos4717);
            //PositionData pos4718 = new PositionData(4718); posInfoList.Add(pos4718);
            //PositionData pos4719 = new PositionData(4719); posInfoList.Add(pos4719);
            //PositionData pos4800 = new PositionData(4800); posInfoList.Add(pos4800);
            //PositionData pos4810 = new PositionData(4810); posInfoList.Add(pos4810);
            //PositionData pos4811 = new PositionData(4811); posInfoList.Add(pos4811);
            //PositionData pos4890 = new PositionData(4890); posInfoList.Add(pos4890);
            //PositionData pos4900 = new PositionData(4900); posInfoList.Add(pos4900);
            //PositionData pos5000 = new PositionData(5000); posInfoList.Add(pos5000);
            //PositionData pos5090 = new PositionData(5090); posInfoList.Add(pos5090);
            //PositionData pos5100 = new PositionData(5100); posInfoList.Add(pos5100);
            //PositionData pos5110 = new PositionData(5110); posInfoList.Add(pos5110);
            //PositionData pos5111 = new PositionData(5111); posInfoList.Add(pos5111);
            //PositionData pos5200 = new PositionData(5200); posInfoList.Add(pos5200);
            //PositionData pos5210 = new PositionData(5210); posInfoList.Add(pos5210);
            //PositionData pos5211 = new PositionData(5211); posInfoList.Add(pos5211);
            //PositionData pos5290 = new PositionData(5290); posInfoList.Add(pos5290);
            //PositionData pos5300 = new PositionData(5300); posInfoList.Add(pos5300);
            //PositionData pos5400 = new PositionData(5400); posInfoList.Add(pos5400);
            //PositionData pos5490 = new PositionData(5490); posInfoList.Add(pos5490);
            //PositionData pos5500 = new PositionData(5500); posInfoList.Add(pos5500);
            //PositionData pos5510 = new PositionData(5510); posInfoList.Add(pos5510);
            //PositionData pos5511 = new PositionData(5511); posInfoList.Add(pos5511);
            //PositionData pos5600 = new PositionData(5600); posInfoList.Add(pos5600);
            //PositionData pos5610 = new PositionData(5610); posInfoList.Add(pos5610);
            //PositionData pos5611 = new PositionData(5611); posInfoList.Add(pos5611);
            //PositionData pos5690 = new PositionData(5690); posInfoList.Add(pos5690);
            //PositionData pos5700 = new PositionData(5700); posInfoList.Add(pos5700);
            //PositionData pos5800 = new PositionData(5800); posInfoList.Add(pos5800);
            //PositionData pos5890 = new PositionData(5890); posInfoList.Add(pos5890);
            //PositionData pos5900 = new PositionData(5900); posInfoList.Add(pos5900);
            //PositionData pos5910 = new PositionData(5910); posInfoList.Add(pos5910);
            //PositionData pos5911 = new PositionData(5911); posInfoList.Add(pos5911);
            //PositionData pos6000 = new PositionData(6000); posInfoList.Add(pos6000);
            //PositionData pos6010 = new PositionData(6010); posInfoList.Add(pos6010);
            //PositionData pos6011 = new PositionData(6011); posInfoList.Add(pos6011);
            //PositionData pos6090 = new PositionData(6090); posInfoList.Add(pos6090);
            //PositionData pos6100 = new PositionData(6100); posInfoList.Add(pos6100);
            //PositionData pos6200 = new PositionData(6200); posInfoList.Add(pos6200);
            //PositionData pos6290 = new PositionData(6290); posInfoList.Add(pos6290);
            //PositionData pos6300 = new PositionData(6300); posInfoList.Add(pos6300);
            //PositionData pos6310 = new PositionData(6310); posInfoList.Add(pos6310);
            //PositionData pos6311 = new PositionData(6311); posInfoList.Add(pos6311);
            //PositionData pos6400 = new PositionData(6400); posInfoList.Add(pos6400);
            //PositionData pos6410 = new PositionData(6410); posInfoList.Add(pos6410);
            //PositionData pos6411 = new PositionData(6411); posInfoList.Add(pos6411);
            //PositionData pos6490 = new PositionData(6490); posInfoList.Add(pos6490);
            //PositionData pos6500 = new PositionData(6500); posInfoList.Add(pos6500);
            //PositionData pos6600 = new PositionData(6600); posInfoList.Add(pos6600);
            //PositionData pos6690 = new PositionData(6690); posInfoList.Add(pos6690);
            //PositionData pos6700 = new PositionData(6700); posInfoList.Add(pos6700);
            //PositionData pos6710 = new PositionData(6710); posInfoList.Add(pos6710);
            //PositionData pos6711 = new PositionData(6711); posInfoList.Add(pos6711);
            //PositionData pos6800 = new PositionData(6800); posInfoList.Add(pos6800);
            //PositionData pos6810 = new PositionData(6810); posInfoList.Add(pos6810);
            //PositionData pos6811 = new PositionData(6811); posInfoList.Add(pos6811);
            //PositionData pos6890 = new PositionData(6890); posInfoList.Add(pos6890);
            #endregion

            #region # Link 정보
            //// Link 정보
            //pos3200.AddFront(pos3210, pos3400);
            //pos3200.AddLink(pos3210, pos3400);
            //pos3210.AddFront(pos3220);
            //pos3210.AddRear(pos3200);
            //pos3210.AddLink(pos3220, pos3200);
            //pos3220.AddFront(pos3230);
            //pos3220.AddRear(pos3210);
            //pos3220.AddLink(pos3230, pos3210);
            //pos3230.AddFront(pos3240);
            //pos3230.AddRear(pos3220);
            //pos3230.AddLink(pos3240, pos3220);
            //pos3240.AddFront(pos3290);
            //pos3240.AddRear(pos3230);
            //pos3240.AddLink(pos3290, pos3230);
            //pos3290.AddRear(pos3240);
            //pos3290.AddLink(pos3240, pos3490);
            //pos3350.AddFront(pos3360);
            //pos3350.AddLink(pos3360);
            //pos3360.AddFront(pos3370);
            //pos3360.AddRear(pos3350);
            //pos3360.AddLink(pos3370, pos3350);
            //pos3370.AddFront(pos3380);
            //pos3370.AddRear(pos3360);
            //pos3370.AddLink(pos3380, pos3360);
            //pos3380.AddFront(pos3400);
            //pos3380.AddRear(pos3370);
            //pos3380.AddLink(pos3400, pos3370);
            //pos3400.AddFront(pos3600);
            //pos3400.AddRear(pos3380);
            //pos3400.AddLink(pos3200, pos3600, pos3380);
            //pos3490.AddFront(pos3290);
            //pos3490.AddLink(pos3290, pos3690);
            //pos3600.AddFront(pos3610, pos3800);
            //pos3600.AddLink(pos3610, pos3800, pos3400);
            //pos3610.AddFront(pos3620);
            //pos3610.AddRear(pos3600);
            //pos3610.AddLink(pos3620, pos3600);
            //pos3620.AddFront(pos3630);
            //pos3620.AddRear(pos3610);
            //pos3620.AddLink(pos3630, pos3610);
            //pos3630.AddFront(pos3640);
            //pos3630.AddRear(pos3620);
            //pos3630.AddLink(pos3640, pos3620);
            //pos3640.AddFront(pos3690);
            //pos3640.AddRear(pos3630);
            //pos3640.AddLink(pos3690, pos3630);
            //pos3690.AddFront(pos3490);
            //pos3690.AddRear(pos3640);
            //pos3690.AddLink(pos3490, pos3640, pos3890);
            //pos3800.AddFront(pos4000);
            //pos3800.AddLink(pos4000, pos3600);
            //pos3890.AddFront(pos3690);
            //pos3890.AddLink(pos3690, pos4090);
            //pos3900.AddFront(pos3910, pos4100);
            //pos3900.AddLink(pos3910, pos4100);
            //pos3910.AddFront(pos3920);
            //pos3910.AddRear(pos3900);
            //pos3910.AddLink(pos3920, pos3900);
            //pos3920.AddFront(pos3930);
            //pos3920.AddRear(pos3910);
            //pos3920.AddLink(pos3930, pos3910);
            //pos3930.AddFront(pos3940);
            //pos3930.AddRear(pos3920);
            //pos3930.AddLink(pos3940, pos3920);
            //pos3940.AddFront(pos4000);
            //pos3940.AddRear(pos3930);
            //pos3940.AddLink(pos4000, pos3930);
            //pos4000.AddFront(pos4010, pos4200);
            //pos4000.AddRear(pos3940);
            //pos4000.AddLink(pos4010, pos4200, pos3800, pos3940);
            //pos4010.AddFront(pos4020);
            //pos4010.AddRear(pos4000);
            //pos4010.AddLink(pos4020, pos4000);
            //pos4020.AddFront(pos4030);
            //pos4020.AddRear(pos4010);
            //pos4020.AddLink(pos4030, pos4010);
            //pos4030.AddFront(pos4040);
            //pos4030.AddRear(pos4020);
            //pos4030.AddLink(pos4040, pos4020);
            //pos4040.AddFront(pos4090);
            //pos4040.AddRear(pos4030);
            //pos4040.AddLink(pos4090, pos4030);
            //pos4090.AddFront(pos3890);
            //pos4090.AddRear(pos4040);
            //pos4090.AddLink(pos3890, pos4040, pos4290);
            //pos4100.AddFront(pos4300);
            //pos4100.AddLink(pos4300, pos3900);
            //pos4200.AddFront(pos4400);
            //pos4200.AddLink(pos4400, pos4000);
            //pos4290.AddFront(pos4090);
            //pos4290.AddLink(pos4090, pos4490);
            //pos4300.AddFront(pos4310, pos4500);
            //pos4300.AddLink(pos4310, pos4500, pos4100);
            //pos4310.AddFront(pos4311);
            //pos4310.AddRear(pos4300);
            //pos4310.AddLink(pos4311, pos4300);
            //pos4311.AddFront(pos4312);
            //pos4311.AddRear(pos4310);
            //pos4311.AddLink(pos4312, pos4310);
            //pos4312.AddFront(pos4313);
            //pos4312.AddRear(pos4311);
            //pos4312.AddLink(pos4313, pos4311);
            //pos4313.AddFront(pos4314);
            //pos4313.AddRear(pos4312);
            //pos4313.AddLink(pos4314, pos4312);
            //pos4314.AddFront(pos4315);
            //pos4314.AddRear(pos4313);
            //pos4314.AddLink(pos4315, pos4313);
            //pos4315.AddFront(pos4316);
            //pos4315.AddRear(pos4314);
            //pos4315.AddLink(pos4316, pos4314);
            //pos4316.AddFront(pos4317);
            //pos4316.AddRear(pos4315);
            //pos4316.AddLink(pos4317, pos4315);
            //pos4317.AddFront(pos4318);
            //pos4317.AddRear(pos4316);
            //pos4317.AddLink(pos4318, pos4316);
            //pos4318.AddFront(pos4319);
            //pos4318.AddRear(pos4317);
            //pos4318.AddLink(pos4319, pos4317);
            //pos4319.AddFront(pos4320);
            //pos4319.AddRear(pos4318);
            //pos4319.AddLink(pos4320, pos4318);
            //pos4320.AddFront(pos4321);
            //pos4320.AddRear(pos4319);
            //pos4320.AddLink(pos4321, pos4319);
            //pos4321.AddFront(pos4322);
            //pos4321.AddRear(pos4320);
            //pos4321.AddLink(pos4322, pos4320);
            //pos4322.AddFront(pos4323);
            //pos4322.AddRear(pos4321);
            //pos4322.AddLink(pos4323, pos4321);
            //pos4323.AddFront(pos4324);
            //pos4323.AddRear(pos4322);
            //pos4323.AddLink(pos4324, pos4322);
            //pos4324.AddFront(pos4325);
            //pos4324.AddRear(pos4323);
            //pos4324.AddLink(pos4325, pos4323);
            //pos4325.AddFront(pos4326);
            //pos4325.AddRear(pos4324);
            //pos4325.AddLink(pos4326, pos4324);
            //pos4326.AddFront(pos4327);
            //pos4326.AddRear(pos4325);
            //pos4326.AddLink(pos4327, pos4325);
            //pos4327.AddFront(pos4328);
            //pos4327.AddRear(pos4326);
            //pos4327.AddLink(pos4328, pos4326);
            //pos4328.AddFront(pos4329);
            //pos4328.AddRear(pos4327);
            //pos4328.AddLink(pos4329, pos4327);
            //pos4329.AddFront(pos4330);
            //pos4329.AddRear(pos4328);
            //pos4329.AddLink(pos4330, pos4328);
            //pos4330.AddFront(pos4331);
            //pos4330.AddRear(pos4329);
            //pos4330.AddLink(pos4331, pos4329);
            //pos4331.AddFront(pos4332);
            //pos4331.AddRear(pos4330);
            //pos4331.AddLink(pos4332, pos4330);
            //pos4332.AddFront(pos4333);
            //pos4332.AddRear(pos4331);
            //pos4332.AddLink(pos4333, pos4331);
            //pos4333.AddFront(pos4334);
            //pos4333.AddRear(pos4332);
            //pos4333.AddLink(pos4334, pos4332);
            //pos4334.AddFront(pos4335);
            //pos4334.AddRear(pos4333);
            //pos4334.AddLink(pos4335, pos4333);
            //pos4335.AddFront(pos4336);
            //pos4335.AddRear(pos4334);
            //pos4335.AddLink(pos4336, pos4334);
            //pos4336.AddFront(pos4337);
            //pos4336.AddRear(pos4335);
            //pos4336.AddLink(pos4337, pos4335);
            //pos4337.AddFront(pos4338);
            //pos4337.AddRear(pos4336);
            //pos4337.AddLink(pos4338, pos4336);
            //pos4338.AddFront(pos4339);
            //pos4338.AddRear(pos4337);
            //pos4338.AddLink(pos4339, pos4337);
            //pos4339.AddFront(pos4340);
            //pos4339.AddRear(pos4338);
            //pos4339.AddLink(pos4340, pos4338);
            //pos4340.AddFront(pos4341);
            //pos4340.AddRear(pos4339);
            //pos4340.AddLink(pos4341, pos4339);
            //pos4341.AddFront(pos4342);
            //pos4341.AddRear(pos4340);
            //pos4341.AddLink(pos4342, pos4340);
            //pos4342.AddFront(pos4343);
            //pos4342.AddRear(pos4341);
            //pos4342.AddLink(pos4343, pos4341);
            //pos4343.AddFront(pos4400);
            //pos4343.AddRear(pos4342);
            //pos4343.AddLink(pos4400, pos4342);
            //pos4400.AddFront(pos4410, pos4600);
            //pos4400.AddRear(pos4343);
            //pos4400.AddLink(pos4410, pos4600, pos4200, pos4343);
            //pos4410.AddFront(pos4411);
            //pos4410.AddRear(pos4400);
            //pos4410.AddLink(pos4411, pos4400);
            //pos4411.AddFront(pos4412);
            //pos4411.AddRear(pos4410);
            //pos4411.AddLink(pos4412, pos4410);
            //pos4412.AddFront(pos4413);
            //pos4412.AddRear(pos4411);
            //pos4412.AddLink(pos4413, pos4411);
            //pos4413.AddFront(pos4414);
            //pos4413.AddRear(pos4412);
            //pos4413.AddLink(pos4414, pos4412);
            //pos4414.AddFront(pos4415);
            //pos4414.AddRear(pos4413);
            //pos4414.AddLink(pos4415, pos4413);
            //pos4415.AddFront(pos4416);
            //pos4415.AddRear(pos4414);
            //pos4415.AddLink(pos4416, pos4414);
            //pos4416.AddFront(pos4417);
            //pos4416.AddRear(pos4415);
            //pos4416.AddLink(pos4417, pos4415);
            //pos4417.AddFront(pos4418);
            //pos4417.AddRear(pos4416);
            //pos4417.AddLink(pos4418, pos4416);
            //pos4418.AddFront(pos4419);
            //pos4418.AddRear(pos4417);
            //pos4418.AddLink(pos4419, pos4417);
            //pos4419.AddFront(pos4420);
            //pos4419.AddRear(pos4418);
            //pos4419.AddLink(pos4420, pos4418);
            //pos4420.AddFront(pos4421);
            //pos4420.AddRear(pos4419);
            //pos4420.AddLink(pos4421, pos4419);
            //pos4421.AddFront(pos4422);
            //pos4421.AddRear(pos4420);
            //pos4421.AddLink(pos4422, pos4420);
            //pos4422.AddFront(pos4423);
            //pos4422.AddRear(pos4421);
            //pos4422.AddLink(pos4423, pos4421);
            //pos4423.AddFront(pos4424);
            //pos4423.AddRear(pos4422);
            //pos4423.AddLink(pos4424, pos4422);
            //pos4424.AddFront(pos4425);
            //pos4424.AddRear(pos4423);
            //pos4424.AddLink(pos4425, pos4423);
            //pos4425.AddFront(pos4426);
            //pos4425.AddRear(pos4424);
            //pos4425.AddLink(pos4426, pos4424);
            //pos4426.AddFront(pos4427);
            //pos4426.AddRear(pos4425);
            //pos4426.AddLink(pos4427, pos4425);
            //pos4427.AddFront(pos4428);
            //pos4427.AddRear(pos4426);
            //pos4427.AddLink(pos4428, pos4426);
            //pos4428.AddFront(pos4429);
            //pos4428.AddRear(pos4427);
            //pos4428.AddLink(pos4429, pos4427);
            //pos4429.AddFront(pos4430);
            //pos4429.AddRear(pos4428);
            //pos4429.AddLink(pos4430, pos4428);
            //pos4430.AddFront(pos4431);
            //pos4430.AddRear(pos4429);
            //pos4430.AddLink(pos4431, pos4429);
            //pos4431.AddFront(pos4432);
            //pos4431.AddRear(pos4430);
            //pos4431.AddLink(pos4432, pos4430);
            //pos4432.AddFront(pos4433);
            //pos4432.AddRear(pos4431);
            //pos4432.AddLink(pos4433, pos4431);
            //pos4433.AddFront(pos4434);
            //pos4433.AddRear(pos4432);
            //pos4433.AddLink(pos4434, pos4432);
            //pos4434.AddFront(pos4435);
            //pos4434.AddRear(pos4433);
            //pos4434.AddLink(pos4435, pos4433);
            //pos4435.AddFront(pos4436);
            //pos4435.AddRear(pos4434);
            //pos4435.AddLink(pos4436, pos4434);
            //pos4436.AddFront(pos4437);
            //pos4436.AddRear(pos4435);
            //pos4436.AddLink(pos4437, pos4435);
            //pos4437.AddFront(pos4438);
            //pos4437.AddRear(pos4436);
            //pos4437.AddLink(pos4438, pos4436);
            //pos4438.AddFront(pos4439);
            //pos4438.AddRear(pos4437);
            //pos4438.AddLink(pos4439, pos4437);
            //pos4439.AddFront(pos4440);
            //pos4439.AddRear(pos4438);
            //pos4439.AddLink(pos4440, pos4438);
            //pos4440.AddFront(pos4441);
            //pos4440.AddRear(pos4439);
            //pos4440.AddLink(pos4441, pos4439);
            //pos4441.AddFront(pos4442);
            //pos4441.AddRear(pos4440);
            //pos4441.AddLink(pos4442, pos4440);
            //pos4442.AddFront(pos4443);
            //pos4442.AddRear(pos4441);
            //pos4442.AddLink(pos4443, pos4441);
            //pos4443.AddFront(pos4490);
            //pos4443.AddRear(pos4442);
            //pos4443.AddLink(pos4490, pos4442);
            //pos4490.AddFront(pos4290);
            //pos4490.AddRear(pos4443);
            //pos4490.AddLink(pos4290, pos4443, pos4690);
            //pos4500.AddFront(pos4700);
            //pos4500.AddLink(pos4700, pos4300);
            //pos4600.AddFront(pos4800);
            //pos4600.AddLink(pos4800, pos4400);
            //pos4690.AddFront(pos4490);
            //pos4690.AddLink(pos4490, pos4890);
            //pos4700.AddFront(pos4710, pos4900);
            //pos4700.AddLink(pos4710, pos4900, pos4500);
            //pos4710.AddFront(pos4711);
            //pos4710.AddRear(pos4700);
            //pos4710.AddLink(pos4711, pos4700);
            //pos4711.AddFront(pos4712);
            //pos4711.AddRear(pos4710);
            //pos4711.AddLink(pos4712, pos4710);
            //pos4712.AddFront(pos4713);
            //pos4712.AddRear(pos4711);
            //pos4712.AddLink(pos4713, pos4711);
            //pos4713.AddFront(pos4714);
            //pos4713.AddRear(pos4712);
            //pos4713.AddLink(pos4714, pos4712);
            //pos4714.AddFront(pos4715);
            //pos4714.AddRear(pos4713);
            //pos4714.AddLink(pos4715, pos4713);
            //pos4715.AddFront(pos4716);
            //pos4715.AddRear(pos4714);
            //pos4715.AddLink(pos4716, pos4714);
            //pos4716.AddFront(pos4717);
            //pos4716.AddRear(pos4715);
            //pos4716.AddLink(pos4717, pos4715);
            //pos4717.AddFront(pos4718);
            //pos4717.AddRear(pos4716);
            //pos4717.AddLink(pos4718, pos4716);
            //pos4718.AddFront(pos4719);
            //pos4718.AddRear(pos4717);
            //pos4718.AddLink(pos4719, pos4717);
            //pos4719.AddFront(pos4800);
            //pos4719.AddRear(pos4718);
            //pos4719.AddLink(pos4800, pos4718);
            //pos4800.AddFront(pos4810, pos5000);
            //pos4800.AddRear(pos4719);
            //pos4800.AddLink(pos4810, pos5000, pos4719, pos4600);
            //pos4810.AddFront(pos4811);
            //pos4810.AddRear(pos4800);
            //pos4810.AddLink(pos4811, pos4800);
            //pos4811.AddFront(pos4890);
            //pos4811.AddRear(pos4810);
            //pos4811.AddLink(pos4890, pos4810);
            //pos4890.AddFront(pos4690);
            //pos4890.AddRear(pos4811);
            //pos4890.AddLink(pos4690, pos4811, pos5090);
            //pos4900.AddFront(pos5100);
            //pos4900.AddLink(pos5100, pos4700);
            //pos5000.AddFront(pos5200);
            //pos5000.AddLink(pos5200, pos4800);
            //pos5090.AddFront(pos4890);
            //pos5090.AddLink(pos4890, pos5290);
            //pos5100.AddFront(pos5110, pos5300);
            //pos5100.AddLink(pos5110, pos5300, pos4900);
            //pos5110.AddFront(pos5111);
            //pos5110.AddRear(pos5100);
            //pos5110.AddLink(pos5111, pos5100);
            //pos5111.AddFront(pos5200);
            //pos5111.AddRear(pos5110);
            //pos5111.AddLink(pos5200, pos5110);
            //pos5200.AddFront(pos5210, pos5400);
            //pos5200.AddRear(pos5111);
            //pos5200.AddLink(pos5210, pos5400, pos5000, pos5111);
            //pos5210.AddFront(pos5211);
            //pos5210.AddRear(pos5200);
            //pos5210.AddLink(pos5211, pos5200);
            //pos5211.AddFront(pos5290);
            //pos5211.AddRear(pos5210);
            //pos5211.AddLink(pos5290, pos5210);
            //pos5290.AddFront(pos5090);
            //pos5290.AddRear(pos5211);
            //pos5290.AddLink(pos5090, pos5211, pos5490);
            //pos5300.AddFront(pos5500);
            //pos5300.AddLink(pos5500, pos5100);
            //pos5400.AddFront(pos5600);
            //pos5400.AddLink(pos5600, pos5200);
            //pos5490.AddFront(pos5290);
            //pos5490.AddLink(pos5290, pos5690);
            //pos5500.AddFront(pos5510, pos5700);
            //pos5500.AddLink(pos5510, pos5700, pos5300);
            //pos5510.AddFront(pos5511);
            //pos5510.AddRear(pos5500);
            //pos5510.AddLink(pos5511, pos5500);
            //pos5511.AddFront(pos5600);
            //pos5511.AddRear(pos5510);
            //pos5511.AddLink(pos5600, pos5510);
            //pos5600.AddFront(pos5610, pos5800);
            //pos5600.AddRear(pos5511);
            //pos5600.AddLink(pos5610, pos5800, pos5400, pos5511);
            //pos5610.AddFront(pos5611);
            //pos5610.AddRear(pos5600);
            //pos5610.AddLink(pos5611, pos5600);
            //pos5611.AddFront(pos5690);
            //pos5611.AddRear(pos5610);
            //pos5611.AddLink(pos5690, pos5610);
            //pos5690.AddFront(pos5490);
            //pos5690.AddRear(pos5611);
            //pos5690.AddLink(pos5490, pos5611, pos5890);
            //pos5700.AddFront(pos5900);
            //pos5700.AddLink(pos5900, pos5500);
            //pos5800.AddFront(pos6000);
            //pos5800.AddLink(pos6000, pos5600);
            //pos5890.AddFront(pos5690);
            //pos5890.AddLink(pos5690, pos6090);
            //pos5900.AddFront(pos5910, pos6100);
            //pos5900.AddLink(pos5910, pos6100, pos5700);
            //pos5910.AddFront(pos5911);
            //pos5910.AddRear(pos5900);
            //pos5910.AddLink(pos5911, pos5900);
            //pos5911.AddFront(pos6000);
            //pos5911.AddRear(pos5910);
            //pos5911.AddLink(pos6000, pos5910);
            //pos6000.AddFront(pos6010, pos6200);
            //pos6000.AddRear(pos5911);
            //pos6000.AddLink(pos6010, pos6200, pos5800, pos5911);
            //pos6010.AddFront(pos6011);
            //pos6010.AddRear(pos6000);
            //pos6010.AddLink(pos6011, pos6000);
            //pos6011.AddFront(pos6090);
            //pos6011.AddRear(pos6010);
            //pos6011.AddLink(pos6090, pos6010);
            //pos6090.AddFront(pos5890);
            //pos6090.AddRear(pos6011);
            //pos6090.AddLink(pos5890, pos6011, pos6290);
            //pos6100.AddFront(pos6300);
            //pos6100.AddLink(pos6300, pos5900);
            //pos6200.AddFront(pos6400);
            //pos6200.AddLink(pos6400, pos6000);
            //pos6290.AddFront(pos6090);
            //pos6290.AddLink(pos6090, pos6490);
            //pos6300.AddFront(pos6310, pos6500);
            //pos6300.AddLink(pos6310, pos6500, pos6100);
            //pos6310.AddFront(pos6311);
            //pos6310.AddRear(pos6300);
            //pos6310.AddLink(pos6311, pos6300);
            //pos6311.AddFront(pos6400);
            //pos6311.AddRear(pos6310);
            //pos6311.AddLink(pos6400, pos6310);
            //pos6400.AddFront(pos6410, pos6600);
            //pos6400.AddRear(pos6311);
            //pos6400.AddLink(pos6410, pos6600, pos6200, pos6311);
            //pos6410.AddFront(pos6411);
            //pos6410.AddRear(pos6400);
            //pos6410.AddLink(pos6411, pos6400);
            //pos6411.AddFront(pos6490);
            //pos6411.AddRear(pos6410);
            //pos6411.AddLink(pos6490, pos6410);
            //pos6490.AddFront(pos6290);
            //pos6490.AddRear(pos6411);
            //pos6490.AddLink(pos6290, pos6411, pos6690);
            //pos6500.AddFront(pos6700);
            //pos6500.AddLink(pos6700, pos6300);
            //pos6600.AddFront(pos6800);
            //pos6600.AddLink(pos6800, pos6400);
            //pos6690.AddFront(pos6490);
            //pos6690.AddLink(pos6490, pos6890);
            //pos6700.AddFront(pos6710);
            //pos6700.AddLink(pos6710, pos6500);
            //pos6710.AddFront(pos6711);
            //pos6710.AddRear(pos6700);
            //pos6710.AddLink(pos6711, pos6700);
            //pos6711.AddFront(pos6800);
            //pos6711.AddRear(pos6710);
            //pos6711.AddLink(pos6800, pos6710);
            //pos6800.AddFront(pos6810);
            //pos6800.AddRear(pos6711);
            //pos6800.AddLink(pos6810, pos6600, pos6711);
            //pos6810.AddFront(pos6811);
            //pos6810.AddRear(pos6800);
            //pos6810.AddLink(pos6811, pos6800);
            //pos6811.AddFront(pos6890);
            //pos6811.AddRear(pos6810);
            //pos6811.AddLink(pos6890, pos6810);
            //pos6890.AddFront(pos6690);
            //pos6890.AddRear(pos6811);
            //pos6890.AddLink(pos6690, pos6811);
            #endregion

            #region # Test Position 정보
            // Position 정보
            PositionData pos3409 = new PositionData(3409, false); posInfoList.Add(pos3409);
            PositionData pos3410 = new PositionData(3410, false); posInfoList.Add(pos3410);
            PositionData pos3411 = new PositionData(3411, false); posInfoList.Add(pos3411);
            PositionData pos3420 = new PositionData(3420, false); posInfoList.Add(pos3420);
            PositionData pos3421 = new PositionData(3421, false); posInfoList.Add(pos3421);
            PositionData pos3430 = new PositionData(3430, true); posInfoList.Add(pos3430);
            PositionData pos3440 = new PositionData(3440, false); posInfoList.Add(pos3440);
            PositionData pos3600 = new PositionData(3600, false); posInfoList.Add(pos3600);
            PositionData pos3620 = new PositionData(3620, true); posInfoList.Add(pos3620);
            PositionData pos3630 = new PositionData(3630, false); posInfoList.Add(pos3630);
            PositionData pos3640 = new PositionData(3640, false); posInfoList.Add(pos3640);
            PositionData pos3650 = new PositionData(3650, false); posInfoList.Add(pos3650);
            PositionData pos3690 = new PositionData(3690, true); posInfoList.Add(pos3690);
            PositionData pos3800 = new PositionData(3800, false); posInfoList.Add(pos3800);
            PositionData pos3890 = new PositionData(3890, false); posInfoList.Add(pos3890);
            PositionData pos3910 = new PositionData(3910, false); posInfoList.Add(pos3910);
            PositionData pos3911 = new PositionData(3911, false); posInfoList.Add(pos3911);
            PositionData pos3920 = new PositionData(3920, false); posInfoList.Add(pos3920);
            PositionData pos3930 = new PositionData(3930, false); posInfoList.Add(pos3930);
            PositionData pos3940 = new PositionData(3940, false); posInfoList.Add(pos3940);
            PositionData pos4000 = new PositionData(4000, true); posInfoList.Add(pos4000);
            PositionData pos4090 = new PositionData(4090, false); posInfoList.Add(pos4090);
            PositionData pos4200 = new PositionData(4200, false); posInfoList.Add(pos4200);
            PositionData pos4290 = new PositionData(4290, false); posInfoList.Add(pos4290);
            PositionData pos4310 = new PositionData(4310, false); posInfoList.Add(pos4310);
            PositionData pos4311 = new PositionData(4311, false); posInfoList.Add(pos4311);
            PositionData pos4312 = new PositionData(4312, false); posInfoList.Add(pos4312);
            PositionData pos4313 = new PositionData(4313, false); posInfoList.Add(pos4313);
            PositionData pos4314 = new PositionData(4314, false); posInfoList.Add(pos4314);
            PositionData pos4315 = new PositionData(4315, false); posInfoList.Add(pos4315);
            PositionData pos4316 = new PositionData(4316, false); posInfoList.Add(pos4316);
            PositionData pos4317 = new PositionData(4317, false); posInfoList.Add(pos4317);
            PositionData pos4318 = new PositionData(4318, false); posInfoList.Add(pos4318);
            PositionData pos4319 = new PositionData(4319, false); posInfoList.Add(pos4319);
            PositionData pos4320 = new PositionData(4320, false); posInfoList.Add(pos4320);
            PositionData pos4321 = new PositionData(4321, false); posInfoList.Add(pos4321);
            PositionData pos4322 = new PositionData(4322, false); posInfoList.Add(pos4322);
            PositionData pos4323 = new PositionData(4323, false); posInfoList.Add(pos4323);
            PositionData pos4324 = new PositionData(4324, false); posInfoList.Add(pos4324);
            PositionData pos4325 = new PositionData(4325, false); posInfoList.Add(pos4325);
            PositionData pos4326 = new PositionData(4326, false); posInfoList.Add(pos4326);
            PositionData pos4327 = new PositionData(4327, false); posInfoList.Add(pos4327);
            PositionData pos4328 = new PositionData(4328, false); posInfoList.Add(pos4328);
            PositionData pos4329 = new PositionData(4329, false); posInfoList.Add(pos4329);
            PositionData pos4330 = new PositionData(4330, false); posInfoList.Add(pos4330);
            PositionData pos4331 = new PositionData(4331, false); posInfoList.Add(pos4331);
            PositionData pos4332 = new PositionData(4332, false); posInfoList.Add(pos4332);
            PositionData pos4333 = new PositionData(4333, false); posInfoList.Add(pos4333);
            PositionData pos4334 = new PositionData(4334, false); posInfoList.Add(pos4334);
            PositionData pos4335 = new PositionData(4335, false); posInfoList.Add(pos4335);
            PositionData pos4336 = new PositionData(4336, false); posInfoList.Add(pos4336);
            PositionData pos4337 = new PositionData(4337, false); posInfoList.Add(pos4337);
            PositionData pos4338 = new PositionData(4338, false); posInfoList.Add(pos4338);
            PositionData pos4339 = new PositionData(4339, false); posInfoList.Add(pos4339);
            PositionData pos4340 = new PositionData(4340, false); posInfoList.Add(pos4340);
            PositionData pos4341 = new PositionData(4341, false); posInfoList.Add(pos4341);
            PositionData pos4342 = new PositionData(4342, false); posInfoList.Add(pos4342);
            PositionData pos4343 = new PositionData(4343, false); posInfoList.Add(pos4343);
            PositionData pos4400 = new PositionData(4400, true); posInfoList.Add(pos4400);
            PositionData pos4410 = new PositionData(4410, false); posInfoList.Add(pos4410);
            PositionData pos4411 = new PositionData(4411, false); posInfoList.Add(pos4411);
            PositionData pos4412 = new PositionData(4412, false); posInfoList.Add(pos4412);
            PositionData pos4413 = new PositionData(4413, false); posInfoList.Add(pos4413);
            PositionData pos4414 = new PositionData(4414, false); posInfoList.Add(pos4414);
            PositionData pos4415 = new PositionData(4415, false); posInfoList.Add(pos4415);
            PositionData pos4416 = new PositionData(4416, false); posInfoList.Add(pos4416);
            PositionData pos4417 = new PositionData(4417, false); posInfoList.Add(pos4417);
            PositionData pos4418 = new PositionData(4418, false); posInfoList.Add(pos4418);
            PositionData pos4419 = new PositionData(4419, false); posInfoList.Add(pos4419);
            PositionData pos4420 = new PositionData(4420, false); posInfoList.Add(pos4420);
            PositionData pos4421 = new PositionData(4421, false); posInfoList.Add(pos4421);
            PositionData pos4422 = new PositionData(4422, false); posInfoList.Add(pos4422);
            PositionData pos4423 = new PositionData(4423, false); posInfoList.Add(pos4423);
            PositionData pos4424 = new PositionData(4424, false); posInfoList.Add(pos4424);
            PositionData pos4425 = new PositionData(4425, false); posInfoList.Add(pos4425);
            PositionData pos4426 = new PositionData(4426, false); posInfoList.Add(pos4426);
            PositionData pos4427 = new PositionData(4427, false); posInfoList.Add(pos4427);
            PositionData pos4428 = new PositionData(4428, false); posInfoList.Add(pos4428);
            PositionData pos4429 = new PositionData(4429, false); posInfoList.Add(pos4429);
            PositionData pos4430 = new PositionData(4430, false); posInfoList.Add(pos4430);
            PositionData pos4431 = new PositionData(4431, false); posInfoList.Add(pos4431);
            PositionData pos4432 = new PositionData(4432, false); posInfoList.Add(pos4432);
            PositionData pos4433 = new PositionData(4433, false); posInfoList.Add(pos4433);
            PositionData pos4434 = new PositionData(4434, false); posInfoList.Add(pos4434);
            PositionData pos4435 = new PositionData(4435, false); posInfoList.Add(pos4435);
            PositionData pos4436 = new PositionData(4436, false); posInfoList.Add(pos4436);
            PositionData pos4437 = new PositionData(4437, false); posInfoList.Add(pos4437);
            PositionData pos4438 = new PositionData(4438, false); posInfoList.Add(pos4438);
            PositionData pos4439 = new PositionData(4439, false); posInfoList.Add(pos4439);
            PositionData pos4440 = new PositionData(4440, false); posInfoList.Add(pos4440);
            PositionData pos4441 = new PositionData(4441, false); posInfoList.Add(pos4441);
            PositionData pos4442 = new PositionData(4442, false); posInfoList.Add(pos4442);
            PositionData pos4443 = new PositionData(4443, false); posInfoList.Add(pos4443);
            PositionData pos4490 = new PositionData(4490, true); posInfoList.Add(pos4490);
            PositionData pos4600 = new PositionData(4600, false); posInfoList.Add(pos4600);
            PositionData pos4690 = new PositionData(4690, false); posInfoList.Add(pos4690);
            PositionData pos4710 = new PositionData(4710, false); posInfoList.Add(pos4710);
            PositionData pos4711 = new PositionData(4711, false); posInfoList.Add(pos4711);
            PositionData pos4712 = new PositionData(4712, false); posInfoList.Add(pos4712);
            PositionData pos4713 = new PositionData(4713, false); posInfoList.Add(pos4713);
            PositionData pos4714 = new PositionData(4714, false); posInfoList.Add(pos4714);
            PositionData pos4715 = new PositionData(4715, false); posInfoList.Add(pos4715);
            PositionData pos4716 = new PositionData(4716, false); posInfoList.Add(pos4716);
            PositionData pos4717 = new PositionData(4717, false); posInfoList.Add(pos4717);
            PositionData pos4718 = new PositionData(4718, false); posInfoList.Add(pos4718);
            PositionData pos4719 = new PositionData(4719, false); posInfoList.Add(pos4719);
            PositionData pos4720 = new PositionData(4720, false); posInfoList.Add(pos4720);
            PositionData pos4721 = new PositionData(4721, false); posInfoList.Add(pos4721);
            PositionData pos4722 = new PositionData(4722, false); posInfoList.Add(pos4722);
            PositionData pos4723 = new PositionData(4723, false); posInfoList.Add(pos4723);
            PositionData pos4724 = new PositionData(4724, false); posInfoList.Add(pos4724);
            PositionData pos4725 = new PositionData(4725, false); posInfoList.Add(pos4725);
            PositionData pos4726 = new PositionData(4726, false); posInfoList.Add(pos4726);
            PositionData pos4727 = new PositionData(4727, false); posInfoList.Add(pos4727);
            PositionData pos4728 = new PositionData(4728, false); posInfoList.Add(pos4728);
            PositionData pos4729 = new PositionData(4729, false); posInfoList.Add(pos4729);
            PositionData pos4730 = new PositionData(4730, false); posInfoList.Add(pos4730);
            PositionData pos4731 = new PositionData(4731, false); posInfoList.Add(pos4731);
            PositionData pos4732 = new PositionData(4732, false); posInfoList.Add(pos4732);
            PositionData pos4733 = new PositionData(4733, false); posInfoList.Add(pos4733);
            PositionData pos4734 = new PositionData(4734, false); posInfoList.Add(pos4734);
            PositionData pos4735 = new PositionData(4735, false); posInfoList.Add(pos4735);
            PositionData pos4736 = new PositionData(4736, false); posInfoList.Add(pos4736);
            PositionData pos4737 = new PositionData(4737, false); posInfoList.Add(pos4737);
            PositionData pos4738 = new PositionData(4738, false); posInfoList.Add(pos4738);
            PositionData pos4739 = new PositionData(4739, false); posInfoList.Add(pos4739);
            PositionData pos4740 = new PositionData(4740, false); posInfoList.Add(pos4740);
            PositionData pos4741 = new PositionData(4741, false); posInfoList.Add(pos4741);
            PositionData pos4742 = new PositionData(4742, false); posInfoList.Add(pos4742);
            PositionData pos4743 = new PositionData(4743, false); posInfoList.Add(pos4743);
            PositionData pos4800 = new PositionData(4800, true); posInfoList.Add(pos4800);
            PositionData pos4810 = new PositionData(4810, false); posInfoList.Add(pos4810);
            PositionData pos4811 = new PositionData(4811, false); posInfoList.Add(pos4811);
            PositionData pos4890 = new PositionData(4890, true); posInfoList.Add(pos4890);
            PositionData pos5000 = new PositionData(5000, false); posInfoList.Add(pos5000);
            PositionData pos5090 = new PositionData(5090, false); posInfoList.Add(pos5090);
            PositionData pos5110 = new PositionData(5110, false); posInfoList.Add(pos5110);
            PositionData pos5111 = new PositionData(5111, false); posInfoList.Add(pos5111);
            PositionData pos5112 = new PositionData(5112, false); posInfoList.Add(pos5112);
            PositionData pos5113 = new PositionData(5113, false); posInfoList.Add(pos5113);
            PositionData pos5114 = new PositionData(5114, false); posInfoList.Add(pos5114);
            PositionData pos5115 = new PositionData(5115, false); posInfoList.Add(pos5115);
            PositionData pos5116 = new PositionData(5116, false); posInfoList.Add(pos5116);
            PositionData pos5117 = new PositionData(5117, false); posInfoList.Add(pos5117);
            PositionData pos5118 = new PositionData(5118, false); posInfoList.Add(pos5118);
            PositionData pos5119 = new PositionData(5119, false); posInfoList.Add(pos5119);
            PositionData pos5120 = new PositionData(5120, false); posInfoList.Add(pos5120);
            PositionData pos5121 = new PositionData(5121, false); posInfoList.Add(pos5121);
            PositionData pos5122 = new PositionData(5122, false); posInfoList.Add(pos5122);
            PositionData pos5123 = new PositionData(5123, false); posInfoList.Add(pos5123);
            PositionData pos5124 = new PositionData(5124, false); posInfoList.Add(pos5124);
            PositionData pos5125 = new PositionData(5125, false); posInfoList.Add(pos5125);
            PositionData pos5126 = new PositionData(5126, false); posInfoList.Add(pos5126);
            PositionData pos5127 = new PositionData(5127, false); posInfoList.Add(pos5127);
            PositionData pos5128 = new PositionData(5128, false); posInfoList.Add(pos5128);
            PositionData pos5129 = new PositionData(5129, false); posInfoList.Add(pos5129);
            PositionData pos5130 = new PositionData(5130, false); posInfoList.Add(pos5130);
            PositionData pos5131 = new PositionData(5131, false); posInfoList.Add(pos5131);
            PositionData pos5132 = new PositionData(5132, false); posInfoList.Add(pos5132);
            PositionData pos5133 = new PositionData(5133, false); posInfoList.Add(pos5133);
            PositionData pos5134 = new PositionData(5134, false); posInfoList.Add(pos5134);
            PositionData pos5135 = new PositionData(5135, false); posInfoList.Add(pos5135);
            PositionData pos5136 = new PositionData(5136, false); posInfoList.Add(pos5136);
            PositionData pos5137 = new PositionData(5137, false); posInfoList.Add(pos5137);
            PositionData pos5138 = new PositionData(5138, false); posInfoList.Add(pos5138);
            PositionData pos5139 = new PositionData(5139, false); posInfoList.Add(pos5139);
            PositionData pos5140 = new PositionData(5140, false); posInfoList.Add(pos5140);
            PositionData pos5141 = new PositionData(5141, false); posInfoList.Add(pos5141);
            PositionData pos5142 = new PositionData(5142, false); posInfoList.Add(pos5142);
            PositionData pos5143 = new PositionData(5143, false); posInfoList.Add(pos5143);
            PositionData pos5200 = new PositionData(5200, true); posInfoList.Add(pos5200);
            PositionData pos5210 = new PositionData(5210, false); posInfoList.Add(pos5210);
            PositionData pos5211 = new PositionData(5211, false); posInfoList.Add(pos5211);
            PositionData pos5290 = new PositionData(5290, true); posInfoList.Add(pos5290);
            PositionData pos5400 = new PositionData(5400, false); posInfoList.Add(pos5400);
            PositionData pos5490 = new PositionData(5490, false); posInfoList.Add(pos5490);
            PositionData pos5510 = new PositionData(5510, false); posInfoList.Add(pos5510);
            PositionData pos5511 = new PositionData(5511, false); posInfoList.Add(pos5511);
            PositionData pos5512 = new PositionData(5512, false); posInfoList.Add(pos5512);
            PositionData pos5513 = new PositionData(5513, false); posInfoList.Add(pos5513);
            PositionData pos5514 = new PositionData(5514, false); posInfoList.Add(pos5514);
            PositionData pos5515 = new PositionData(5515, false); posInfoList.Add(pos5515);
            PositionData pos5516 = new PositionData(5516, false); posInfoList.Add(pos5516);
            PositionData pos5517 = new PositionData(5517, false); posInfoList.Add(pos5517);
            PositionData pos5518 = new PositionData(5518, false); posInfoList.Add(pos5518);
            PositionData pos5519 = new PositionData(5519, false); posInfoList.Add(pos5519);
            PositionData pos5520 = new PositionData(5520, false); posInfoList.Add(pos5520);
            PositionData pos5521 = new PositionData(5521, false); posInfoList.Add(pos5521);
            PositionData pos5522 = new PositionData(5522, false); posInfoList.Add(pos5522);
            PositionData pos5523 = new PositionData(5523, false); posInfoList.Add(pos5523);
            PositionData pos5524 = new PositionData(5524, false); posInfoList.Add(pos5524);
            PositionData pos5525 = new PositionData(5525, false); posInfoList.Add(pos5525);
            PositionData pos5526 = new PositionData(5526, false); posInfoList.Add(pos5526);
            PositionData pos5527 = new PositionData(5527, false); posInfoList.Add(pos5527);
            PositionData pos5528 = new PositionData(5528, false); posInfoList.Add(pos5528);
            PositionData pos5529 = new PositionData(5529, false); posInfoList.Add(pos5529);
            PositionData pos5530 = new PositionData(5530, false); posInfoList.Add(pos5530);
            PositionData pos5531 = new PositionData(5531, false); posInfoList.Add(pos5531);
            PositionData pos5532 = new PositionData(5532, false); posInfoList.Add(pos5532);
            PositionData pos5533 = new PositionData(5533, false); posInfoList.Add(pos5533);
            PositionData pos5534 = new PositionData(5534, false); posInfoList.Add(pos5534);
            PositionData pos5535 = new PositionData(5535, false); posInfoList.Add(pos5535);
            PositionData pos5536 = new PositionData(5536, false); posInfoList.Add(pos5536);
            PositionData pos5537 = new PositionData(5537, false); posInfoList.Add(pos5537);
            PositionData pos5538 = new PositionData(5538, false); posInfoList.Add(pos5538);
            PositionData pos5539 = new PositionData(5539, false); posInfoList.Add(pos5539);
            PositionData pos5540 = new PositionData(5540, false); posInfoList.Add(pos5540);
            PositionData pos5541 = new PositionData(5541, false); posInfoList.Add(pos5541);
            PositionData pos5542 = new PositionData(5542, false); posInfoList.Add(pos5542);
            PositionData pos5543 = new PositionData(5543, false); posInfoList.Add(pos5543);
            PositionData pos5600 = new PositionData(5600, true); posInfoList.Add(pos5600);
            PositionData pos5610 = new PositionData(5610, false); posInfoList.Add(pos5610);
            PositionData pos5611 = new PositionData(5611, false); posInfoList.Add(pos5611);
            PositionData pos5690 = new PositionData(5690, true); posInfoList.Add(pos5690);
            #endregion

            #region # Test Link 정보
            // Link 정보
            pos3409.AddFront(pos3410, pos3600);
            pos3410.AddFront(pos3411);
            pos3410.AddRear(pos3409);
            pos3411.AddFront(pos3420);
            pos3411.AddRear(pos3410);
            pos3420.AddFront(pos3421);
            pos3420.AddRear(pos3411);
            pos3421.AddFront(pos3430);
            pos3421.AddRear(pos3420);
            pos3430.AddFront(pos3440);
            pos3430.AddRear(pos3421, pos3620);
            pos3440.AddRear(pos3430);
            pos3600.AddFront(pos3800);
            pos3600.AddRear(pos3409);
            pos3620.AddFront(pos3430, pos3630);
            pos3630.AddFront(pos3640);
            pos3630.AddRear(pos3620);
            pos3640.AddFront(pos3650);
            pos3640.AddRear(pos3630);
            pos3650.AddFront(pos3690);
            pos3650.AddRear(pos3640);
            pos3690.AddRear(pos3650, pos3890);
            pos3800.AddFront(pos4000);
            pos3800.AddRear(pos3600);
            pos3890.AddFront(pos3690);
            pos3890.AddRear(pos4090);
            pos3910.AddFront(pos3911);
            pos3911.AddFront(pos3920);
            pos3911.AddRear(pos3910);
            pos3920.AddFront(pos3930);
            pos3920.AddRear(pos3911);
            pos3930.AddFront(pos3940);
            pos3930.AddRear(pos3920);
            pos3940.AddFront(pos4000);
            pos3940.AddRear(pos3930);
            pos4000.AddFront(pos4200);
            pos4000.AddRear(pos3940, pos3800);
            pos4090.AddFront(pos4890);
            pos4090.AddRear(pos4290);
            pos4200.AddFront(pos4400);
            pos4200.AddRear(pos4000);
            pos4290.AddFront(pos4090);
            pos4290.AddRear(pos4490);
            pos4310.AddFront(pos4311);
            pos4311.AddFront(pos4312);
            pos4311.AddRear(pos4310);
            pos4312.AddFront(pos4313);
            pos4312.AddRear(pos4311);
            pos4312.AddFront(pos4313);
            pos4312.AddRear(pos4311);
            pos4313.AddFront(pos4314);
            pos4313.AddRear(pos4312);
            pos4314.AddFront(pos4315);
            pos4314.AddRear(pos4313);
            pos4315.AddFront(pos4316);
            pos4315.AddRear(pos4314);
            pos4316.AddFront(pos4317);
            pos4316.AddRear(pos4315);
            pos4317.AddFront(pos4318);
            pos4317.AddRear(pos4316);
            pos4318.AddFront(pos4319);
            pos4318.AddRear(pos4317);
            pos4319.AddFront(pos4320);
            pos4319.AddRear(pos4318);
            pos4320.AddFront(pos4321);
            pos4320.AddRear(pos4319);
            pos4321.AddFront(pos4322);
            pos4321.AddRear(pos4320);
            pos4322.AddFront(pos4323);
            pos4322.AddRear(pos4321);
            pos4323.AddFront(pos4324);
            pos4323.AddRear(pos4322);
            pos4324.AddFront(pos4325);
            pos4324.AddRear(pos4323);
            pos4325.AddFront(pos4326);
            pos4325.AddRear(pos4324);
            pos4326.AddFront(pos4327);
            pos4326.AddRear(pos4325);
            pos4327.AddFront(pos4328);
            pos4327.AddRear(pos4326);
            pos4328.AddFront(pos4329);
            pos4328.AddRear(pos4327);
            pos4329.AddFront(pos4330);
            pos4329.AddRear(pos4328);
            pos4330.AddFront(pos4331);
            pos4330.AddRear(pos4329);
            pos4331.AddFront(pos4332);
            pos4331.AddRear(pos4330);
            pos4332.AddFront(pos4333);
            pos4332.AddRear(pos4331);
            pos4333.AddFront(pos4334);
            pos4333.AddRear(pos4332);
            pos4334.AddFront(pos4335);
            pos4334.AddRear(pos4333);
            pos4335.AddFront(pos4336);
            pos4335.AddRear(pos4334);
            pos4336.AddFront(pos4337);
            pos4336.AddRear(pos4335);
            pos4337.AddFront(pos4338);
            pos4337.AddRear(pos4336);
            pos4338.AddFront(pos4339);
            pos4338.AddRear(pos4337);
            pos4339.AddFront(pos4340);
            pos4339.AddRear(pos4338);
            pos4340.AddFront(pos4341);
            pos4340.AddRear(pos4339);
            pos4341.AddFront(pos4342);
            pos4341.AddRear(pos4340);
            pos4342.AddFront(pos4343);
            pos4342.AddRear(pos4341);
            pos4343.AddFront(pos4400);
            pos4343.AddRear(pos4342);
            pos4400.AddFront(pos4600, pos4410);
            pos4400.AddRear(pos4200, pos4343);
            pos4410.AddFront(pos4411);
            pos4410.AddRear(pos4400);
            pos4411.AddFront(pos4412);
            pos4411.AddRear(pos4410);
            pos4412.AddFront(pos4413);
            pos4412.AddRear(pos4411);
            pos4413.AddFront(pos4414);
            pos4413.AddRear(pos4412);
            pos4414.AddFront(pos4415);
            pos4414.AddRear(pos4413);
            pos4415.AddFront(pos4416);
            pos4415.AddRear(pos4414);
            pos4416.AddFront(pos4417);
            pos4416.AddRear(pos4415);
            pos4417.AddFront(pos4418);
            pos4417.AddRear(pos4416);
            pos4418.AddFront(pos4419);
            pos4418.AddRear(pos4417);
            pos4419.AddFront(pos4420);
            pos4419.AddRear(pos4418);
            pos4420.AddFront(pos4421);
            pos4420.AddRear(pos4419);
            pos4421.AddFront(pos4422);
            pos4421.AddRear(pos4420);
            pos4422.AddFront(pos4423);
            pos4422.AddRear(pos4421);
            pos4423.AddFront(pos4424);
            pos4423.AddRear(pos4422);
            pos4424.AddFront(pos4425);
            pos4424.AddRear(pos4423);
            pos4425.AddFront(pos4426);
            pos4425.AddRear(pos4424);
            pos4426.AddFront(pos4427);
            pos4426.AddRear(pos4425);
            pos4427.AddFront(pos4428);
            pos4427.AddRear(pos4426);
            pos4428.AddFront(pos4429);
            pos4428.AddRear(pos4427);
            pos4429.AddFront(pos4430);
            pos4429.AddRear(pos4428);
            pos4430.AddFront(pos4431);
            pos4430.AddRear(pos4429);
            pos4431.AddFront(pos4432);
            pos4431.AddRear(pos4430);
            pos4432.AddFront(pos4433);
            pos4432.AddRear(pos4431);
            pos4433.AddFront(pos4434);
            pos4433.AddRear(pos4432);
            pos4434.AddFront(pos4435);
            pos4434.AddRear(pos4433);
            pos4435.AddFront(pos4436);
            pos4435.AddRear(pos4434);
            pos4436.AddFront(pos4437);
            pos4436.AddRear(pos4435);
            pos4437.AddFront(pos4438);
            pos4437.AddRear(pos4436);
            pos4438.AddFront(pos4439);
            pos4438.AddRear(pos4437);
            pos4439.AddFront(pos4440);
            pos4439.AddRear(pos4438);
            pos4440.AddFront(pos4441);
            pos4440.AddRear(pos4439);
            pos4441.AddFront(pos4442);
            pos4441.AddRear(pos4440);
            pos4442.AddFront(pos4443);
            pos4442.AddRear(pos4441);
            pos4443.AddFront(pos4490);
            pos4443.AddRear(pos4442);
            pos4490.AddFront(pos4290);
            pos4490.AddRear(pos4443, pos4690);
            pos4600.AddFront(pos4800);
            pos4600.AddRear(pos4400);
            pos4690.AddFront(pos4490);
            pos4690.AddRear(pos4890);
            pos4710.AddFront(pos4711);
            pos4711.AddFront(pos4712);
            pos4711.AddRear(pos4710);
            pos4712.AddFront(pos4713);
            pos4712.AddRear(pos4711);
            pos4713.AddFront(pos4714);
            pos4713.AddRear(pos4712);
            pos4714.AddFront(pos4715);
            pos4714.AddRear(pos4713);
            pos4715.AddFront(pos4716);
            pos4715.AddRear(pos4714);
            pos4716.AddFront(pos4717);
            pos4716.AddRear(pos4715);
            pos4717.AddFront(pos4718);
            pos4717.AddRear(pos4716);
            pos4718.AddFront(pos4719);
            pos4718.AddRear(pos4717);
            pos4719.AddFront(pos4720);
            pos4719.AddRear(pos4718);
            pos4720.AddFront(pos4721);
            pos4720.AddRear(pos4719);
            pos4721.AddFront(pos4722);
            pos4721.AddRear(pos4720);
            pos4722.AddFront(pos4723);
            pos4722.AddRear(pos4721);
            pos4723.AddFront(pos4724);
            pos4723.AddRear(pos4722);
            pos4724.AddFront(pos4725);
            pos4724.AddRear(pos4723);
            pos4725.AddFront(pos4726);
            pos4725.AddRear(pos4724);
            pos4726.AddFront(pos4727);
            pos4726.AddRear(pos4725);
            pos4727.AddFront(pos4728);
            pos4727.AddRear(pos4726);
            pos4728.AddFront(pos4729);
            pos4728.AddRear(pos4727);
            pos4729.AddFront(pos4730);
            pos4729.AddRear(pos4728);
            pos4730.AddFront(pos4731);
            pos4730.AddRear(pos4729);
            pos4731.AddFront(pos4732);
            pos4731.AddRear(pos4730);
            pos4732.AddFront(pos4733);
            pos4732.AddRear(pos4731);
            pos4733.AddFront(pos4734);
            pos4733.AddRear(pos4732);
            pos4734.AddFront(pos4735);
            pos4734.AddRear(pos4733);
            pos4735.AddFront(pos4736);
            pos4735.AddRear(pos4734);
            pos4736.AddFront(pos4737);
            pos4736.AddRear(pos4735);
            pos4737.AddFront(pos4738);
            pos4737.AddRear(pos4736);
            pos4738.AddFront(pos4739);
            pos4738.AddRear(pos4737);
            pos4739.AddFront(pos4740);
            pos4739.AddRear(pos4738);
            pos4740.AddFront(pos4741);
            pos4740.AddRear(pos4739);
            pos4741.AddFront(pos4742);
            pos4741.AddRear(pos4740);
            pos4742.AddFront(pos4743);
            pos4742.AddRear(pos4741);
            pos4743.AddFront(pos4800);
            pos4743.AddRear(pos4742);
            pos4800.AddFront(pos5000, pos4810);
            pos4800.AddRear(pos4600, pos4743);
            pos4810.AddFront(pos4811);
            pos4810.AddRear(pos4800);
            pos4811.AddFront(pos4890);
            pos4811.AddRear(pos4810);
            pos4890.AddFront(pos4690);
            pos4890.AddRear(pos4811, pos5090);
            pos5000.AddFront(pos5200);
            pos5000.AddRear(pos4800);
            pos5090.AddFront(pos4890);
            pos5090.AddRear(pos5290);
            pos5110.AddFront(pos5111);
            pos5111.AddFront(pos5112);
            pos5111.AddRear(pos5110);
            pos5112.AddFront(pos5113);
            pos5112.AddRear(pos5111);
            pos5113.AddFront(pos5114);
            pos5113.AddRear(pos5112);
            pos5114.AddFront(pos5115);
            pos5114.AddRear(pos5113);
            pos5115.AddFront(pos5116);
            pos5115.AddRear(pos5114);
            pos5116.AddFront(pos5117);
            pos5116.AddRear(pos5115);
            pos5117.AddFront(pos5118);
            pos5117.AddRear(pos5116);
            pos5118.AddFront(pos5119);
            pos5118.AddRear(pos5117);
            pos5119.AddFront(pos5120);
            pos5119.AddRear(pos5118);
            pos5120.AddFront(pos5121);
            pos5120.AddRear(pos5119);
            pos5121.AddFront(pos5122);
            pos5121.AddRear(pos5120);
            pos5122.AddFront(pos5123);
            pos5122.AddRear(pos5121);
            pos5123.AddFront(pos5124);
            pos5123.AddRear(pos5122);
            pos5124.AddFront(pos5125);
            pos5124.AddRear(pos5123);
            pos5125.AddFront(pos5126);
            pos5125.AddRear(pos5124);
            pos5126.AddFront(pos5127);
            pos5126.AddRear(pos5125);
            pos5127.AddFront(pos5128);
            pos5127.AddRear(pos5126);
            pos5128.AddFront(pos5129);
            pos5128.AddRear(pos5127);
            pos5129.AddFront(pos5130);
            pos5129.AddRear(pos5128);
            pos5130.AddFront(pos5131);
            pos5130.AddRear(pos5129);
            pos5131.AddFront(pos5132);
            pos5131.AddRear(pos5130);
            pos5132.AddFront(pos5133);
            pos5132.AddRear(pos5131);
            pos5133.AddFront(pos5134);
            pos5133.AddRear(pos5132);
            pos5134.AddFront(pos5135);
            pos5134.AddRear(pos5133);
            pos5135.AddFront(pos5136);
            pos5135.AddRear(pos5134);
            pos5136.AddFront(pos5137);
            pos5136.AddRear(pos5135);
            pos5137.AddFront(pos5138);
            pos5137.AddRear(pos5136);
            pos5138.AddFront(pos5139);
            pos5138.AddRear(pos5137);
            pos5139.AddFront(pos5140);
            pos5139.AddRear(pos5138);
            pos5140.AddFront(pos5141);
            pos5140.AddRear(pos5139);
            pos5141.AddFront(pos5142);
            pos5141.AddRear(pos5140);
            pos5142.AddFront(pos5143);
            pos5142.AddRear(pos5141);
            pos5143.AddFront(pos5200);
            pos5143.AddRear(pos5142);
            pos5200.AddFront(pos5210, pos5400);
            pos5200.AddRear(pos5000, pos5143);
            pos5210.AddFront(pos5211);
            pos5210.AddRear(pos5200);
            pos5211.AddFront(pos5290);
            pos5211.AddRear(pos5210);
            pos5290.AddFront(pos5090);
            pos5290.AddRear(pos5211, pos5490);
            pos5400.AddFront(pos5600);
            pos5400.AddRear(pos5200);
            pos5490.AddFront(pos5290);
            pos5490.AddRear(pos5690);
            pos5510.AddFront(pos5511);
            pos5511.AddFront(pos5512);
            pos5511.AddRear(pos5510);
            pos5512.AddFront(pos5513);
            pos5512.AddRear(pos5511);
            pos5513.AddFront(pos5514);
            pos5513.AddRear(pos5512);
            pos5514.AddFront(pos5515);
            pos5514.AddRear(pos5513);
            pos5515.AddFront(pos5516);
            pos5515.AddRear(pos5514);
            pos5516.AddFront(pos5517);
            pos5516.AddRear(pos5515);
            pos5517.AddFront(pos5518);
            pos5517.AddRear(pos5516);
            pos5518.AddFront(pos5519);
            pos5518.AddRear(pos5517);
            pos5519.AddFront(pos5520);
            pos5519.AddRear(pos5518);
            pos5520.AddFront(pos5521);
            pos5520.AddRear(pos5519);
            pos5521.AddFront(pos5522);
            pos5521.AddRear(pos5520);
            pos5522.AddFront(pos5523);
            pos5522.AddRear(pos5521);
            pos5523.AddFront(pos5524);
            pos5523.AddRear(pos5522);
            pos5524.AddFront(pos5525);
            pos5524.AddRear(pos5523);
            pos5525.AddFront(pos5526);
            pos5525.AddRear(pos5524);
            pos5526.AddFront(pos5527);
            pos5526.AddRear(pos5525);
            pos5527.AddFront(pos5528);
            pos5527.AddRear(pos5526);
            pos5528.AddFront(pos5529);
            pos5528.AddRear(pos5527);
            pos5529.AddFront(pos5530);
            pos5529.AddRear(pos5528);
            pos5530.AddFront(pos5531);
            pos5530.AddRear(pos5529);
            pos5531.AddFront(pos5532);
            pos5531.AddRear(pos5530);
            pos5532.AddFront(pos5533);
            pos5532.AddRear(pos5531);
            pos5533.AddFront(pos5534);
            pos5533.AddRear(pos5532);
            pos5534.AddFront(pos5535);
            pos5534.AddRear(pos5533);
            pos5535.AddFront(pos5536);
            pos5535.AddRear(pos5534);
            pos5536.AddFront(pos5537);
            pos5536.AddRear(pos5535);
            pos5537.AddFront(pos5538);
            pos5537.AddRear(pos5536);
            pos5538.AddFront(pos5539);
            pos5538.AddRear(pos5537);
            pos5539.AddFront(pos5540);
            pos5539.AddRear(pos5538);
            pos5540.AddFront(pos5541);
            pos5540.AddRear(pos5539);
            pos5541.AddFront(pos5542);
            pos5541.AddRear(pos5540);
            pos5542.AddFront(pos5543);
            pos5542.AddRear(pos5541);
            pos5543.AddFront(pos5600);
            pos5543.AddRear(pos5542);
            pos5600.AddFront(pos5610);
            pos5600.AddRear(pos5400, pos5543);
            pos5610.AddFront(pos5611);
            pos5610.AddRear(pos5600);
            pos5611.AddFront(pos5690);
            pos5611.AddRear(pos5610);
            pos5690.AddFront(pos5490);
            pos5690.AddRear(pos5611);
            #endregion

            // 정보 Setting Test
            //List<PositionData> path2 = SearchPath(pos4500, pos4411);

            // Server 설정
            server.IP = config.IP;
            server.Port = config.Port;
            server.OnConnected += new CoreServer.ConnectedEvent(server_OnConnected);
            server.OnDisconnected += new CoreServer.DisconnectedEvent(server_OnDisconnected);
            server.OnReceived += new CoreServer.ReceivedEvnet(server_OnReceived);

            // AGV 다수 실행
            #region # AGV 다수 실행
            int num = 1;

            while (true)
            {
                bool flag = false;
                string type = string.Format("VIRTUAL AGV {0}호기", num);
                num++;
                Mutex mutex = new Mutex(true, type, out flag);
                base.Text = type;

                if (flag == true)
                {
                    break;
                }
                server.Port = server.Port + 1;
            }
            #endregion

            // 서버 시작
            CoreNetResult serverResult = server.Start();

            if (serverResult != CoreNetResult.OK)
            {
                WriteLog(Color.Red, "[FAIL] Can Not Start Server");
                return;
            }

            // 타이머 시작
            tmrProcess.Enabled = true;

            // Mode 설정
            wordArray[112] = 4;

            // Log
            WriteLog(Color.Black, "[INFO] Start Virtual AGV Program!");

            // AGV 시작 위치 설정
            #region # AGV 시작 위치 설정
            Random ranPos = new Random();
            int agvPos = ranPos.Next(28);
            PositionData startPos = posInfoList[agvPos];
            wordArray[120] = (ushort)startPos.No;
            #endregion

            // 배터리 설정
            wordArray[114] = 80;

            // Status 설정
            wordArray[111] = 1;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 다이얼로그 출력
            DialogResult result = MessageBox.Show("Do you want exit program?", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }

            // Log
            WriteLog(Color.Black, "[INFO] Stop Virtual AGV  Program!");
        }

        // Server 이벤트
        private void server_OnConnected(object sender, CoreNetEventArgs e)
        {
            // 색상변경
            lblServer.BackColor = Color.Blue;

            // Log
            WriteLog(Color.Black, "[INFO] Server Connected!!!");
        }
        private void server_OnDisconnected(object sender, CoreNetEventArgs e)
        {
            // 색상변경
            lblServer.BackColor = Color.Red;

            // Log
            WriteLog(Color.Red, "[INFO] Server Disconnected!!!");
        }
        private void server_OnReceived(object sender, CoreNetEventArgs e)
        {
            // 여분Buffer 붙이기
            byte[] recvBuffer = e.Buffer;
            if (spareBuffer.Length > 0)
            {
                recvBuffer = CoreArray.MemCat(spareBuffer, e.Buffer);
                spareBuffer = new byte[0];
            }

            // Length검사1
            if (recvBuffer.Length < 9)
            {
                spareBuffer = recvBuffer;
                return;
            }

            // Parse
            byte[] subHeader = new byte[] { recvBuffer[0], recvBuffer[1] };
            byte networkNumber = recvBuffer[2];
            byte plcNumber = recvBuffer[3];
            byte[] ioNumber = new byte[] { recvBuffer[4], recvBuffer[5] };
            byte stationNumber = recvBuffer[6];
            short dataLength = BitConverter.ToInt16(recvBuffer, 7);

            // SubHeader 검사
            if ((subHeader[0] != 0x50) || (subHeader[1] != 0x00))
            {
                // Log
                log.WriteLog("[FAIL] Wrong SubHeader!!!" + "\r\n");
                return;
            }

            // Length검사2
            if (recvBuffer.Length < (dataLength + 9))
            {
                spareBuffer = recvBuffer;
                return;
            }

            // ReadWord 처리
            if ((recvBuffer[11] == 0x01) && (recvBuffer[12] == 0x04) && (recvBuffer[13] == 0x00) && (recvBuffer[14] == 0x00))
            {
                // Parsing
                int start = BitConverter.ToInt16(recvBuffer, 15);
                int length = BitConverter.ToInt16(recvBuffer, 19);

                // Buffer
                byte[] buffer = new byte[(length * 2) + 11];
                buffer[0] = 0xd0; // Sub Header
                buffer[1] = 0x00; // Sub Header
                buffer[2] = recvBuffer[2]; // Network Number
                buffer[3] = recvBuffer[3]; // PLC Number 
                buffer[4] = recvBuffer[4]; // IO Number
                buffer[5] = recvBuffer[5]; // IO Number
                buffer[6] = recvBuffer[6]; // Station Number
                CoreArray.MemCpy(buffer, 7, BitConverter.GetBytes((length * 2) + 2), 0, 2); // Data Length
                buffer[9] = 0x00; // Complete Code
                buffer[10] = 0x00; // Complete Code
                for (int i = 0; i < length; i++)
                {
                    int bufferIndex = (i * 2) + 11;
                    int wordIndex = start + i;
                    CoreArray.MemCpy(buffer, bufferIndex, BitConverter.GetBytes(wordArray[wordIndex]), 0, 2);
                }

                // Send
                server.Send(e.RemoteEndPoint, buffer);
            }

            // WriteWord 처리
            if ((recvBuffer[11] == 0x01) && (recvBuffer[12] == 0x14) && (recvBuffer[13] == 0x00) && (recvBuffer[14] == 0x00))
            {
                // Parsing
                int start = BitConverter.ToInt16(recvBuffer, 15);
                int length = BitConverter.ToInt16(recvBuffer, 19);

                // Write
                for (int i = 0; i < length; i++)
                {
                    int recvIndex = (i * 2) + 21;
                    int wordIndex = start + i;
                    wordArray[wordIndex] = BitConverter.ToUInt16(recvBuffer, recvIndex);
                }

                // Buffer
                byte[] buffer = new byte[11];
                buffer[0] = 0xd0; // Sub Header
                buffer[1] = 0x00; // Sub Header
                buffer[2] = recvBuffer[2]; // Network Number
                buffer[3] = recvBuffer[3]; // PLC Number 
                buffer[4] = recvBuffer[4]; // IO Number
                buffer[5] = recvBuffer[5]; // IO Number
                buffer[6] = recvBuffer[6]; // Station Number
                buffer[7] = 0x02; // Data Length
                buffer[8] = 0x00; // Data Length
                buffer[9] = 0x00; // Complete Code
                buffer[10] = 0x00; // Complete Code

                // Send
                server.Send(e.RemoteEndPoint, buffer);
            }
        }

        // 타이머 이벤트
        private void tmrProcess_Tick(object sender, EventArgs e)
        {
            // Scenario
            Scenario_Alive();
            Scenario_DatetimeSet();
            Scenario_DestinationSet();
            Scenario_Resume();
            Scenario_Pause();
            Scenario_Moving();
        }

        // AGV 시나리오
        private void Scenario_Alive()
        {
            // 영역설정
            int plcIndex = 100;

            if (DateTime.Now.Second % 3 == 0)
            {
                wordArray[plcIndex] = 1;
            }
            else
            {
                wordArray[plcIndex] = 0;
            }
        }
        private void Scenario_DatetimeSet()
        {
            // 영역설정
            int cimIndex = 300;
            int plcIndex = 301;

            // DatetimeSet
            if ((wordArray[cimIndex] == 1) && (wordArray[plcIndex] == 0))
            {
                wordArray[plcIndex] = 1;

                // Log
                WriteLog(Color.Green, "[AGV] DatetimeSet Bit On");
            }
            else if ((wordArray[cimIndex] == 0) && (wordArray[plcIndex] == 1))
            {
                wordArray[plcIndex] = 0;

                // Log
                WriteLog(Color.Green, "[AGV] DatetimeSet Bit Off");
            }
        }
        private void Scenario_DestinationSet()
        {
            // 영역설정
            int cimIndex = 310;
            int plcIndex = 311;

            // 변수선언
            ushort posCurrent = wordArray[120];
            ushort setPosWayPoint = wordArray[312];
            ushort setPosDestination = wordArray[313];

            //PosSet
            wordArray[121] = setPosWayPoint;
            wordArray[122] = setPosDestination;

            // DestinationSet
            if ((wordArray[cimIndex] == 1) && (wordArray[plcIndex] == 0))
            {
                wordArray[plcIndex] = 1;

                // Log
                WriteLog(Color.Green, "[AGV] DestinationSet Bit On");
            }
            else if ((wordArray[cimIndex] == 0) && (wordArray[plcIndex] == 1))
            {
                wordArray[plcIndex] = 0;

                // Log
                WriteLog(Color.Green, "[AGV] DestinationSet Bit Off");

                // Pause 확인
                if (wordArray[111] == 32) return;
                
                // 목적지 확인
                if (posCurrent == setPosWayPoint) return;

                //Status 변경
                wordArray[111] = 2;
            }
        }
        private void Scenario_Resume()
        {
            // 영역설정
            int status = 111;
            int cimIndex = 320;
            int plcIndex = 321;

            // Resume
            if ((wordArray[cimIndex] == 1) && (wordArray[plcIndex] == 0))
            {
                wordArray[plcIndex] = 1;

                // Log
                WriteLog(Color.Green, "[AGV] Resume Equipment Bit On");

                // Pause 확인
                if (wordArray[status] == 32)
                {
                    wordArray[status] = 1;
                }
            }
            else if ((wordArray[cimIndex] == 0) && (wordArray[plcIndex] == 1))
            {
                wordArray[plcIndex] = 0;

                // Log
                WriteLog(Color.Green, "[AGV] Resume Equipment Bit Off");
            }
        }
        private void Scenario_Pause()
        {
            // 영역설정
            int cimIndex = 330;
            int plcIndex = 331;

            // Pause
            if ((wordArray[cimIndex] == 1) && (wordArray[plcIndex] == 0))
            {
                wordArray[plcIndex] = 1;

                // Status 변경
                wordArray[111] = 32;

                // Log
                WriteLog(Color.Green, "[AGV] Pause Equipment Bit On");
            }
            else if ((wordArray[cimIndex] == 0) && (wordArray[plcIndex] == 1))
            {
                wordArray[plcIndex] = 0;

                // Log
                WriteLog(Color.Green, "[AGV] Pause Equipment Bit Off");
            }
        }
        private void Scenario_Moving()
        {
            // 변수선언
            int status = wordArray[111];
            int posCurrent = wordArray[120];
            int posWayPoint = wordArray[121];
            int posDestination = wordArray[122];

            // UI 변경
            if (lblStatus.Text != status.ToString()) lblStatus.Text = status.ToString();
            if (lblPosCurrent.Text != posCurrent.ToString()) lblPosCurrent.Text = posCurrent.ToString();
            if (lblPosWayPoint.Text != posWayPoint.ToString()) lblPosWayPoint.Text = posWayPoint.ToString();
            if (lblPosDestination.Text != posDestination.ToString()) lblPosDestination.Text = posDestination.ToString();

            // Status 확인
            if (status != 2) return;

            // Status 변경
            if (posCurrent == posWayPoint)
            {
                wordArray[111] = 1;
                return;
            }

            // 경로 확인
            PositionData posFrom = FindPosition(posCurrent);
            PositionData posTo = FindPosition(posWayPoint);
            List<PositionData> path = SearchPath(posFrom, posTo);

            // AGV 이동
            movingCount++;
            if (movingCount > 1)
            {
                movingCount = 0;
                wordArray[120] = (ushort)path[1].No;

                // Log
                WriteLog(Color.Green, "[AGV] Moving Complete (" + path[1].No + ")");
            }
        }

        // UI 이벤트
        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        // 기타함수
        private void WriteLog(Color color, string logText)
        {
            // 메시지
            string msg = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") + logText + "\r\n";

            // 디스플레이 갯수 확인
            if (rtbLog.Lines.Length > 1000)
            {
                rtbLog.Clear();
            }

            // UI 출력
            rtbLog.Select(rtbLog.Text.Length, 0);
            rtbLog.SelectionColor = color;
            rtbLog.AppendText(msg);
            rtbLog.ScrollToCaret();
            rtbLog.Update();

            // 파일 출력
            log.WriteLog(msg);
        }
        private PositionData FindPosition(int no)
        {
            // Position 찾기
            foreach (PositionData pos in posInfoList)
            {
                if (pos.No == no)
                {
                    return pos;
                }
            }

            return null;
        }
        private List<PositionData> SearchPath(PositionData fromPos, PositionData toPos)
        {
            // 거리값 초기화
            foreach (PositionData pos in posInfoList)
            {
                pos.Distance = int.MaxValue;
            }
            fromPos.Distance = 0;

            // 거리값 계산
            bool isFindToPos = false;
            List<PositionData> searchList = new List<PositionData>();
            searchList.Add(fromPos);
            while (true)
            {
                // 다음거리값 계산
                List<PositionData> nextSearchList = new List<PositionData>();
                foreach (PositionData curPos in searchList)
                {
                    foreach (PositionData frontPos in curPos.FrontList)
                    {
                        int newDistance = curPos.Distance + 1;
                        if (frontPos.Distance > newDistance)
                        {
                            frontPos.Distance = newDistance;
                            if (frontPos == toPos)
                            {
                                isFindToPos = true;
                            }
                            else
                            {
                                nextSearchList.Add(frontPos);
                            }
                        }
                    }
                    foreach (PositionData rearPos in curPos.RearList)
                    {
                        int newDistance = curPos.Distance + 1;
                        if (rearPos.Distance > newDistance)
                        {
                            rearPos.Distance = newDistance;
                            if (rearPos == toPos)
                            {
                                isFindToPos = true;
                            }
                            else
                            {
                                nextSearchList.Add(rearPos);
                            }
                        }
                    }
                }

                // 탐색완료확인
                searchList = nextSearchList;
                if ((isFindToPos == true) || (nextSearchList.Count == 0))
                {
                    break;
                }
            }

            // 경로유무확인
            if (isFindToPos == false)
            {
                return null;
            }

            // 경로추출
            PositionData pathPos = toPos;
            List<PositionData> pathList = new List<PositionData>();
            pathList.Add(pathPos);
            while (true)
            {
                // 경로추가
                foreach (PositionData linkPos in pathPos.LinkList)
                {
                    if (pathPos.Distance == linkPos.Distance + 1)
                    {
                        pathList.Insert(0, linkPos);
                        pathPos = linkPos;
                        break;
                    }
                }

                // 경로완료확인
                if (pathPos == fromPos)
                {
                    break;
                }
            }

            return pathList;
        }

        // AGV Test
        private void btnComm_Click(object sender, EventArgs e)
        {
            TestForm commFrom = new TestForm();
            commFrom.Show();
            commFrom.wordMemoryInput(wordArray);
        }

        // Alarm Test
        private void Scenario_Alarm(object sender, KeyEventArgs e)
        {
            Scenario_Alarm();
        }
        private void Scenario_Alarm(object sender, KeyPressEventArgs e)
        {
            Scenario_Alarm();
        }
        private void Scenario_Alarm(object sender, EventArgs e)
        {
            Scenario_Alarm();
        }
        private void Scenario_Alarm()
        {
            // 알람 영역
            for (int i = 230; i < 250; i++)
            {
                // 변수 선언
                Control valueTmps = grpAlarm.Controls[string.Concat("nud", i)];
                Control labelTemp = grpAlarm.Controls[string.Concat("lbl", i)];

                // 입력 확인
                if (valueTmps.Text != string.Empty)
                {
                    // 숫자 확인
                    for (int j = 0; j < valueTmps.Text.Length; j++)
                    {
                        if (!char.IsNumber(valueTmps.Text, j))
                        {
                            wordArray[i] = 0;
                            labelTemp.BackColor = Color.White;
                            labelTemp.ForeColor = Color.Black;
                            return;
                        }
                    }

                    // 크기 확인
                    long tempSize = long.Parse(valueTmps.Text);
                    if (0 == tempSize || 65535 < tempSize)
                    {
                        wordArray[i] = 0;
                        labelTemp.BackColor = Color.White;
                        labelTemp.ForeColor = Color.Black;
                        continue;
                    }

                    // 알람 변경
                    wordArray[i] = ushort.Parse(valueTmps.Text);
                    labelTemp.BackColor = Color.Red;
                    labelTemp.ForeColor = Color.White;
                }
                else
                {
                    wordArray[i] = 0;
                    labelTemp.BackColor = Color.White;
                    labelTemp.ForeColor = Color.Black;
                }
            }
        }
    }
}
