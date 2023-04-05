using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using Skender.Stock.Indicators;

namespace zCharts
{
    public partial class MainForm : Form
    {

        loader lo = new loader();

        //these seqence# can be used to identify panel names for right expanding panels
        //for each tab
        //we  declare these variables and set these intial values

        //monthly panel setting -- sequence =1
        int PW1 = 0;
        bool Hided1 = false;

        //Daily panel setting - sequence =2
        int PW2 = 0;
        bool Hided2 = false;

        //Daily panel setting - sequence =3
        int PW3 = 0;
        bool Hided3 = false;

        //Minute panel setting- sequence =4
        int PW4 = 0;
        bool Hided4 = false;

        //Dow panel setting - sequence =5
        int PW5 = 0;
        bool Hided5 = false;

        //Nasdaq panel setting - sequence =6
        int PW6 = 0;
        bool Hided6 = false;

        //RSI scann - sequence =7
        int PW7 = 0;
        bool Hided7 = false;

        //MACD scann - sequence =8
        int PW8 = 0;
        bool Hided8 = false;

        //Boll scann 1 - sequence =9
        int PW9 = 0;
        bool Hided9 = false;

        //Boll scann 2 - advanced -- seqence =10
        int PW10 = 0;
        bool Hided10 = false;

        //Z scann -- sequence =11
        int PW11 = 0;
        bool Hided11 = false;

        //Dow Weekly --Sequence = 12
        int PW12 = 0;
        bool Hided12 = false;

        //Dow Monthly -- Sequence = 13
        int PW13 = 0;
        bool Hided13 = false;

        //Nasdaw Weekly -- Sequence = 14
        int PW14 = 0;
        bool Hided14 = false;

        //Nasdaq Monthly -- Sequence = 15
        int PW15 = 0;
        bool Hided15 = false;

        //Business Logic
        string tree = "";
        string path { get; set; }
        private static clsFileHandler oFH = new clsFileHandler();
        string Delimiter = ",";
        int DataRow1 = 1;
        int HeaderRow = 0;
        int MaxRows = 0;
        List<StockPrice> _daily_stockprice = new List<StockPrice>();
        string Symbol { get; set; }
        List<Quote> quote = new List<Quote>();
        List<Quote> quote_weekly = new List<Quote>();
        List<Quote> quote_monthly = new List<Quote>();
        List<Quote> quote_daily = new List<Quote>();
        List<Quote> quote_min = new List<Quote>();
        List<Quote> quote_dow_daily = new List<Quote>();
        List<Quote> quote_dow_weekly = new List<Quote>();
        List<Quote> quote_dow_monthly = new List<Quote>();

        List<Quote> quote_nasdaq_daily = new List<Quote>();
        List<Quote> quote_nasdaq_weekly = new List<Quote>();
        List<Quote> quote_nasdaq_monthly = new List<Quote>();
        #region Main Functions
        public void show_treeview()
        {

            eod_treeView.Nodes.Clear();
            tree = lo.view();
            if (tree.Length > 0)
            {
                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                int i = 0;
                foreach (treeview tr in _tree_view)
                {
                    eod_treeView.Nodes.Add(tr.root);
                    foreach (string node in tr.node)
                    {
                        eod_treeView.Nodes[i].Nodes.Add(node.ToUpper());
                    }
                    i++;
                }
            }
        }
        public MainForm()
        {
            InitializeComponent();
            show_treeview();

            
            splitContainerDaily.Panel1.AutoScroll = true;
            splitContainerWeekly.Panel1.AutoScroll = true;
            splitContainerMonthly.Panel1.AutoScroll = true;
            splitContainer2.Panel1.AutoScroll = true;
            splitContainerDowMonthly.Panel1.AutoScroll = true;
            splitContainerDow.Panel1.AutoScroll = true;
            splitContainerDowWeekly.Panel1.AutoScroll = true;

            string dx = Properties.Settings.Default["Data_location"].ToString();
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.path = dx.Length > 0 ? dx : path;

            //Monthly panels
            PW1 = SPanel1.Width;
            Hided1 = false;
            buttonLeft1.Visible = false;
            buttonTop1.Visible = true;

            //Daily panels
            PW2 = SPanel2.Width;
            Hided2 = false;
            buttonLeft2.Visible = false;
            buttonTop2.Visible = true;

            //weekly settings
            PW3 = SPanel3.Width;
            Hided3 = false;
            buttonLeft3.Visible = false;
            buttonTop3.Visible = true;

            //minutes settings
            PW4 = SPanel4.Width;
            Hided4 = false;
            buttonLeft4.Visible = false;
            buttonTop4.Visible = true;

            //Dow Jones settings
            PW5 = SPanel5.Width;
            Hided5 = false;
            buttonLeft5.Visible = false;
            buttonTop5.Visible = true;

            //Nasdaq settings
            PW6 = SPanel6.Width;
            Hided6 = false;
            buttonLeft6.Visible = false;
            buttonTop6.Visible = true;

            //RSI scan settings
            PW7 = SPanel7.Width;
            Hided7 = false;
            buttonLeft7.Visible = false;
            buttonTop7.Visible = true;

            //MACD scann settings
            PW8 = SPanel8.Width;
            Hided8 = false;
            buttonLeft8.Visible = false;
            buttonTop8.Visible = true;

            //Bollinger Band scan 1
            PW9 = SPanel9.Width;
            Hided9 = false;
            buttonLeft9.Visible = false;
            buttonTop9.Visible = true;


            //Bollinger Band scan 2
            PW10 = SPanel10.Width;
            Hided10 = false;
            buttonLeft10.Visible = false;
            buttonTop10.Visible = true;

            //zScann settings
            PW11 = SPanel11.Width;
            Hided11 = false;
            buttonLeft11.Visible = false;
            buttonTop11.Visible = true;

            //Dow Jones Weekly settings
            PW12 = SPanel12.Width;
            Hided12 = false;
            buttonLeft12.Visible = false;
            buttonTop12.Visible = true;

            //Dow Jones Monthly settings
            PW13 = SPanel13.Width;
            Hided13 = false;
            buttonLeft13.Visible = false;
            buttonTop13.Visible = true;

            //Nasdaq Weekly settings
            PW14 = SPanel14.Width;
            Hided14 = false;
            buttonLeft14.Visible = false;
            buttonTop14.Visible = true;


            //Nasdaq Monthly settings
            PW15 = SPanel15.Width;
            Hided15 = false;
            buttonLeft15.Visible = false;
            buttonTop15.Visible = true;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        //monthly panel settings
        private void buttonTop1_Click(object sender, EventArgs e)
        {
            if (!Hided1)
            {
                buttonLeft1.Text = "M\no\nn\nt\nh\nl\ny\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop1.Visible = false;
                buttonLeft1.Visible = true;
            }

            timerMonthly.Start();
        }

        private void buttonLeft1_Click(object sender, EventArgs e)
        {
            if (Hided1)
            {
                buttonLeft1.Visible = false;
                buttonTop1.Visible = true;
                buttonTop1.Text = "Monthly Settings";
            }

            timerMonthly.Start();
        }

        private void timerMonthly_Tick(object sender, EventArgs e)
        {
            if (Hided1)
            {
                SPanel1.Width = SPanel1.Width + 20;
                if (SPanel1.Width >= PW1)
                {
                    timerMonthly.Stop();
                    Hided1 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel1.Width = SPanel1.Width - 20;
                if (SPanel1.Width <= 0)
                {
                    timerMonthly.Stop();
                    Hided1 = true;
                    this.Refresh();
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        //daily panel control logic
        private void timerDaily_Tick(object sender, EventArgs e)
        {
            if (Hided2)
            {
                SPanel2.Width = SPanel2.Width + 20;
                if (SPanel2.Width >= PW2)
                {
                    timerDaily.Stop();
                    Hided2 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel2.Width = SPanel2.Width - 20;
                if (SPanel2.Width <= 0)
                {
                    timerDaily.Stop();
                    Hided2 = true;
                    this.Refresh();
                }
            }
        }

        private void buttonLeft2_Click(object sender, EventArgs e)
        {
            if (Hided2)
            {
                buttonLeft2.Visible = false;
                buttonTop2.Visible = true;
                buttonTop2.Text = "Daily Settings";
            }

            timerDaily.Start();
        }

        private void buttonTop2_Click(object sender, EventArgs e)
        {
            if (!Hided2)
            {
                buttonLeft2.Text = "D\na\ni\nl\ny\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop2.Visible = false;
                buttonLeft2.Visible = true;
            }

            timerDaily.Start();
        }

        //weekly setting controls
        private void buttonTop3_Click(object sender, EventArgs e)
        {
            if (!Hided3)
            {
                buttonLeft3.Text = "W\ne\ne\nk\nl\ny\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop3.Visible = false;
                buttonLeft3.Visible = true;
            }

            timerWeekly.Start();
        }

        private void buttonLeft3_Click(object sender, EventArgs e)
        {
            if (Hided3)
            {
                buttonLeft3.Visible = false;
                buttonTop3.Visible = true;
                buttonTop3.Text = "Weekly Settings";
            }

            timerWeekly.Start();
        }

        private void timerWeekly_Tick(object sender, EventArgs e)
        {
            if (Hided3)
            {
                SPanel3.Width = SPanel3.Width + 20;
                if (SPanel3.Width >= PW3)
                {
                    timerWeekly.Stop();
                    Hided3 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel3.Width = SPanel3.Width - 20;
                if (SPanel3.Width <= 0)
                {
                    timerWeekly.Stop();
                    Hided3 = true;
                    this.Refresh();
                }
            }
        }

        //Minutes Settings
        private void buttonTop4_Click(object sender, EventArgs e)
        {
            if (!Hided4)
            {
                buttonLeft4.Text = "M\ni\nn\nu\nt\ne\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop4.Visible = false;
                buttonLeft4.Visible = true;
            }

            timerMinute.Start();
        }

        private void buttonLeft4_Click(object sender, EventArgs e)
        {
            if (Hided4)
            {
                buttonLeft4.Visible = false;
                buttonTop4.Visible = true;
                buttonTop4.Text = "Minute Settings";
            }

            timerMinute.Start();
        }

        private void timerMinute_Tick(object sender, EventArgs e)
        {
            if (Hided4)
            {
                SPanel4.Width = SPanel4.Width + 20;
                if (SPanel4.Width >= PW4)
                {
                    timerMinute.Stop();
                    Hided4 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel4.Width = SPanel4.Width - 20;
                if (SPanel4.Width <= 0)
                {
                    timerMinute.Stop();
                    Hided4 = true;
                    this.Refresh();
                }
            }
        }

        //Dow Jones Panel settings
        private void buttonLeft5_Click(object sender, EventArgs e)
        {
            if (Hided5)
            {
                buttonLeft5.Visible = false;
                buttonTop5.Visible = true;
                buttonTop5.Text = "Dow Jones Settings";
            }

            timerDow.Start();
        }

        private void buttontop5_Click(object sender, EventArgs e)
        {
            if (!Hided5)
            {
                buttonLeft5.Text = "\nD\no\nw\n \nJ\no\nn\ne\ns \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop5.Visible = false;
                buttonLeft5.Visible = true;
            }

            timerDow.Start();

        }

        private void timerDow_Tick(object sender, EventArgs e)
        {
            if (Hided5)
            {
                SPanel5.Width = SPanel5.Width + 20;
                if (SPanel5.Width >= PW5)
                {
                    timerDow.Stop();
                    Hided5 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel5.Width = SPanel5.Width - 20;
                if (SPanel5.Width <= 0)
                {
                    timerDow.Stop();
                    Hided5 = true;
                    this.Refresh();
                }
            }
        }

        //Nasdaq Daily Settings
        private void buttonLeft6_Click(object sender, EventArgs e)
        {
            if (Hided6)
            {
                buttonLeft6.Visible = false;
                buttonTop6.Visible = true;
                buttonTop6.Text = "Nasdaq Settings";
            }

            timerNasdaq.Start();
        }

        private void buttonTop6_Click(object sender, EventArgs e)
        {
            if (!Hided6)
            {
                buttonLeft6.Text = "\nN\na\ns\nd\na\nw \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop6.Visible = false;
                buttonLeft6.Visible = true;
            }

            timerNasdaq.Start();
        }

        private void timerNasdaq_Tick(object sender, EventArgs e)
        {
            if (Hided6)
            {
                SPanel6.Width = SPanel6.Width + 20;
                if (SPanel6.Width >= PW6)
                {
                    timerNasdaq.Stop();
                    Hided6 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel6.Width = SPanel6.Width - 20;
                if (SPanel6.Width <= 0)
                {
                    timerNasdaq.Stop();
                    Hided6 = true;
                    this.Refresh();
                }
            }
        }

        //Dow Jones Weekly
        private void buttonLeft12_Click(object sender, EventArgs e)
        {
            if (Hided12)
            {
                buttonLeft12.Visible = false;
                buttonTop12.Visible = true;
                buttonTop12.Text = "Dow Jones Weekly Settings";
            }

            timerDowWeekly.Start();

        }

        private void buttonTop12_Click(object sender, EventArgs e)
        {
            if (!Hided12)
            {
                buttonLeft12.Text = "\nD\no\ns\nw \nJ\no\nn\ne\ns \nW\ne\ne\nk\nl\ny \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop12.Visible = false;
                buttonLeft12.Visible = true;
            }

            timerDowWeekly.Start();
        }

        private void timerDowWeekly_Tick(object sender, EventArgs e)
        {
            if (Hided12)
            {
                SPanel12.Width = SPanel12.Width + 20;
                if (SPanel12.Width >= PW12)
                {
                    timerDowWeekly.Stop();
                    Hided12 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel12.Width = SPanel12.Width - 20;
                if (SPanel12.Width <= 0)
                {
                    timerDowWeekly.Stop();
                    Hided12 = true;
                    this.Refresh();
                }
            }
        }

        //Dow Jones Monthly
        private void buttonLeft13_Click(object sender, EventArgs e)
        {
            if (Hided13)
            {
                buttonLeft13.Visible = false;
                buttonTop13.Visible = true;
                buttonTop13.Text = "Dow Jones Monthly Settings";
            }

            timerDowMonthly.Start();
        }

        private void buttonTop13_Click(object sender, EventArgs e)
        {
            if (!Hided13)
            {
                buttonLeft13.Text = "\nD\no\ns\nw \nJ\no\nn\ne\ns \nM\no\nn\nt\nh\nl\ny \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop13.Visible = false;
                buttonLeft13.Visible = true;
            }

            timerDowMonthly.Start();
        }

        private void timerDowMonthly_Tick(object sender, EventArgs e)
        {
            if (Hided13)
            {
                SPanel13.Width = SPanel13.Width + 20;
                if (SPanel13.Width >= PW13)
                {
                    timerDowMonthly.Stop();
                    Hided13 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel13.Width = SPanel13.Width - 20;
                if (SPanel13.Width <= 0)
                {
                    timerDowMonthly.Stop();
                    Hided13 = true;
                    this.Refresh();
                }
            }
        }

        //Nasdaq Weekly
        private void buttonLeft14_Click(object sender, EventArgs e)
        {
            if (Hided14)
            {
                buttonLeft14.Visible = false;
                buttonTop14.Visible = true;
                buttonTop14.Text = "Nasdaq Weekly Settings";
            }

            timerNasdaqWeekly.Start();
        }

        private void buttonTop14_Click(object sender, EventArgs e)
        {
            if (!Hided14)
            {
                buttonLeft14.Text = "\nN\na\ns\nd \na\nq \nW\ne\ne\nk\nl\ny \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop14.Visible = false;
                buttonLeft14.Visible = true;
            }

            timerNasdaqWeekly.Start();
        }

        private void timerNasdaqWeekly_Tick(object sender, EventArgs e)
        {
            if (Hided14)
            {
                SPanel14.Width = SPanel14.Width + 20;
                if (SPanel14.Width >= PW14)
                {
                    timerNasdaqWeekly.Stop();
                    Hided14 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel14.Width = SPanel14.Width - 20;
                if (SPanel14.Width <= 0)
                {
                    timerNasdaqWeekly.Stop();
                    Hided14 = true;
                    this.Refresh();
                }
            }
        }

        //Nasdaq Monthly
        private void buttonLeft15_Click(object sender, EventArgs e)
        {
            if (Hided15)
            {
                buttonLeft15.Visible = false;
                buttonTop15.Visible = true;
                buttonTop15.Text = "Nasdaq Monthly Settings";
            }

            timerNasdaqMonthly.Start();
        }

        private void buttonTop15_Click(object sender, EventArgs e)
        {
            if (!Hided15)
            {
                buttonLeft15.Text = "\nN\na\ns\nd \na\nq \nM\no\nn\nt\nh\nl\ny \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop15.Visible = false;
                buttonLeft15.Visible = true;
            }

            timerNasdaqMonthly.Start();

        }

        private void timerNasdaqMonthly_Tick(object sender, EventArgs e)
        {
            if (Hided15)
            {
                SPanel15.Width = SPanel15.Width + 20;
                if (SPanel15.Width >= PW15)
                {
                    timerNasdaqMonthly.Stop();
                    Hided15 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel15.Width = SPanel15.Width - 20;
                if (SPanel15.Width <= 0)
                {
                    timerNasdaqMonthly.Stop();
                    Hided15 = true;
                    this.Refresh();
                }
            }
        }


        //RSI scann settings
        private void buttonLeft7_Click(object sender, EventArgs e)
        {
            if (Hided7)
            {
                buttonLeft7.Visible = false;
                buttonTop7.Visible = true;
                buttonTop7.Text = "RSI scann Settings";
            }

            timerRSI.Start();
        }

        private void buttonTop7_Click(object sender, EventArgs e)
        {
            if (!Hided7)
            {
                buttonLeft7.Text = "\nR\nS\nI\n \ns\nc\na\nn\nn \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop7.Visible = false;
                buttonLeft7.Visible = true;
            }

            timerRSI.Start();
        }

        private void timerRSI_Tick(object sender, EventArgs e)
        {
            if (Hided7)
            {
                SPanel7.Width = SPanel7.Width + 20;
                if (SPanel7.Width >= PW7)
                {
                    timerRSI.Stop();
                    Hided7 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel7.Width = SPanel7.Width - 20;
                if (SPanel7.Width <= 0)
                {
                    timerRSI.Stop();
                    Hided7 = true;
                    this.Refresh();
                }
            }
        }

        //MACD Scann Setting
        private void buttonLeft8_Click(object sender, EventArgs e)
        {
            if (Hided8)
            {
                buttonLeft8.Visible = false;
                buttonTop8.Visible = true;
                buttonTop8.Text = "MACD scann";
            }

            timerMACD.Start();
        }

        private void buttonTop8_Click(object sender, EventArgs e)
        {
            if (!Hided8)
            {
                buttonLeft8.Text = "\nM\nA\nC\nD\n \nS\nc\na\nn\nn";
                buttonTop8.Visible = false;
                buttonLeft8.Visible = true;
            }

            timerMACD.Start();
        }

        private void timerMACD_Tick(object sender, EventArgs e)
        {
            if (Hided8)
            {
                SPanel8.Width = SPanel8.Width + 20;
                if (SPanel8.Width >= PW8)
                {
                    timerMACD.Stop();
                    Hided8 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel8.Width = SPanel8.Width - 20;
                if (SPanel8.Width <= 0)
                {
                    timerMACD.Stop();
                    Hided8 = true;
                    this.Refresh();
                }
            }
        }

        //Bollinger Band Scan 1 Panel settings
        private void buttonLeft9_Click(object sender, EventArgs e)
        {
            if (Hided9)
            {
                buttonLeft9.Visible = false;
                buttonTop9.Visible = true;
                buttonTop9.Text = "Bollinger scan 1";
            }

            timerBoll1.Start();
        }

        private void buttonTop9_Click(object sender, EventArgs e)
        {
            if (!Hided9)
            {
                buttonLeft9.Text = "\nB\no\nl\nl\ni\nn\ng\ne\nr\n \nS\nc\na\nn\nn \n1";
                buttonTop9.Visible = false;
                buttonLeft9.Visible = true;
            }

            timerBoll1.Start();
        }

        private void timerBoll1_Tick(object sender, EventArgs e)
        {
            if (Hided9)
            {
                SPanel9.Width = SPanel9.Width + 20;
                if (SPanel9.Width >= PW9)
                {
                    timerBoll1.Stop();
                    Hided9 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel9.Width = SPanel9.Width - 20;
                if (SPanel9.Width <= 0)
                {
                    timerBoll1.Stop();
                    Hided9 = true;
                    this.Refresh();
                }
            }
        }

        //Bollinger Scan 2 panel settings
        private void buttonLeft10_Click(object sender, EventArgs e)
        {
            if (Hided10)
            {
                buttonLeft10.Visible = false;
                buttonTop10.Visible = true;
                buttonTop10.Text = "Bollinger scan 2";
            }

            timerBoll2.Start();
        }

        private void buttonTop10_Click(object sender, EventArgs e)
        {
            if (!Hided10)
            {
                buttonLeft10.Text = "\nB\no\nl\nl\ni\nn\ng\ne\nr\n \nS\nc\na\nn\nn \n2";
                buttonTop10.Visible = false;
                buttonLeft10.Visible = true;
            }

            timerBoll2.Start();
        }

        private void timerBoll2_Tick(object sender, EventArgs e)
        {
            if (Hided10)
            {
                SPanel10.Width = SPanel10.Width + 20;
                if (SPanel10.Width >= PW10)
                {
                    timerBoll2.Stop();
                    Hided10 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel10.Width = SPanel10.Width - 20;
                if (SPanel10.Width <= 0)
                {
                    timerBoll2.Stop();
                    Hided10 = true;
                    this.Refresh();
                }
            }
        }

        //zScann Paenl Settings
        private void buttonLeft11_Click(object sender, EventArgs e)
        {
            if (Hided11)
            {
                buttonLeft11.Visible = false;
                buttonTop11.Visible = true;
                buttonTop11.Text = "zScann Settings";
            }

            timerZScan.Start();
        }

        private void buttonTop11_Click(object sender, EventArgs e)
        {
            if (!Hided11)
            {
                buttonLeft11.Text = "\nz\nS\nC\na\nn\nn\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop11.Visible = false;
                buttonLeft11.Visible = true;
            }

            timerZScan.Start();
        }

        private void timerZScan_Tick(object sender, EventArgs e)
        {
            if (Hided11)
            {
                SPanel11.Width = SPanel11.Width + 20;
                if (SPanel11.Width >= PW11)
                {
                    timerZScan.Stop();
                    Hided11 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel11.Width = SPanel11.Width - 20;
                if (SPanel11.Width <= 0)
                {
                    timerZScan.Stop();
                    Hided11 = true;
                    this.Refresh();
                }
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void addSymbolIntoPortfolioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addSymbolForm sym = new addSymbolForm();
            sym.Show();
            this.Hide();
        }

        private void importSymbolsIntoPortfolioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (eod_treeView.SelectedNode.Text != null)
            {
                importForm from = new importForm(eod_treeView.SelectedNode.Text);
                from.Show();
                this.Hide();
            }
        }

        private void deleteSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(lo.view());
            int i = 0;
            string save_data = @"[";
            foreach (treeview tr in _tree_view)
            {
                save_data += "{'root':'" + tr.root + "','node':[";
                //old node
                int j = 0;
                foreach (string nd in tr.node)
                {
                    if (nd == eod_treeView.SelectedNode.Text)
                    { }
                    else
                    {
                        save_data += "'" + nd + "'" + (j == tr.node.Length - 1 ? "" : ",");
                    }
                    j++;
                }
                save_data += "]}" + (i == tr.root.Length - 1 ? "" : ",");
                i++;
            }
            save_data += "]";
            lo.save(save_data);
            show_treeview();
        }

        private void renameSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renamePortflio rm = new renamePortflio(eod_treeView.SelectedNode.Text, false);
            rm.Show();
            this.Hide();
        }

        private void renamePortfolioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renamePortflio rm = new renamePortflio(eod_treeView.SelectedNode.Text);
            rm.Show();
            this.Hide();
        }

        private void deletePortfolioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(lo.view());
            int i = 0;
            string save_data = @"[";
            foreach (treeview tr in _tree_view)
            {
                if (tr.root != eod_treeView.SelectedNode.Text)
                {
                    save_data += "{'root':'" + tr.root + "','node':[";
                    //old node
                    int j = 0;
                    foreach (string nd in tr.node)
                    { save_data += "'" + nd + "'" + (j == tr.node.Length - 1 ? "" : ","); j++; }
                    //finish old node
                    save_data += "]}" + (i == tr.root.Length - 1 ? "" : ",");
                }

                i++;
            }
            save_data += "]";
            lo.save(save_data);
            show_treeview();
        }

        private void eod_treeView_Click(object sender, EventArgs e)
        {
            var menuItem = eod_treeView.SelectedNode;
            bool __datashow = true;
            if (menuItem != null)
            {
                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                foreach (treeview tr in _tree_view)
                {
                    if (tr.root == menuItem.Text)
                    {
                        __datashow = false;
                    }
                }
                if (__datashow)
                {
                    string __Symbol = menuItem.Text;
                    //MessageBox.Show(__Symbol);
                    this.Symbol = __Symbol;
                    show_eod(__Symbol);
                    // show_csv(__Symbol);
                    Daily_chart();
                    WeeklyChart();
                    MonthlyChart();
                    MinuteChart();
                    //show_daily_chart(__Symbol);
                }
            }
        }


        private void show_eod(string symbol)
        {
            string __path = this.path + "\\\\zLoader\\\\Profile\\\\" + symbol + "_profile.txt";
            if (File.Exists(__path))
                richTextBoxSummary.Text = lo.view(__path);
            else
                richTextBoxSummary.Text = "";
            __path = this.path + "\\\\zLoader\\\\Outlook\\\\" + symbol + "_outlook.txt";
            if (File.Exists(__path))
                richTextBox1.Text = lo.view(__path);
            else
                richTextBox1.Text = "";
        }
        private void eod_treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            eod_treeView.SelectedNode = e.Node;
            if (tree.Length > 0)
            {
                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                foreach (treeview tr in _tree_view)
                {

                    if (tr.root == e.Node.Text)
                    {
                        newPortfolioFolderToolStripMenuItem1.Enabled = true;
                        addSymbolIntoPortfolioToolStripMenuItem1.Enabled = true;
                        importSymbolsIntoPortfolioToolStripMenuItem.Enabled = true;
                        renamePortfolioToolStripMenuItem.Enabled = true;
                        deletePortfolioToolStripMenuItem.Enabled = true;

                        deleteSymbolToolStripMenuItem.Enabled = false;
                        renameSymbolToolStripMenuItem.Enabled = false;
                        //selected_node = e.Node.Text;
                        break;
                    }
                    foreach (string s in tr.node)
                    {
                        if (s == e.Node.Text)
                        {
                            newPortfolioFolderToolStripMenuItem1.Enabled = false;
                            addSymbolIntoPortfolioToolStripMenuItem1.Enabled = false;
                            importSymbolsIntoPortfolioToolStripMenuItem.Enabled = false;
                            renamePortfolioToolStripMenuItem.Enabled = false;
                            deletePortfolioToolStripMenuItem.Enabled = false;

                            deleteSymbolToolStripMenuItem.Enabled = true;
                            renameSymbolToolStripMenuItem.Enabled = true;
                            //selected_node = e.Node.Text;
                            break;
                        }
                    }
                }
            }

        }
        private void eod_treeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStripTreeview.Show(eod_treeView, e.Location);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            show_treeview();
        }

        private void newPortfolioFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            newPortfolioForm file = new newPortfolioForm();
            file.Show();
            this.Hide();
        }

        #endregion



      

        #region Daily Tab
        private static int ddl_p = 0;
        private void priceComboBoxDailyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDailyPrice.ColumnCount + tableLayoutPanelDailyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_p < childIndex || ddl_p == 0)
            {
                ddl_p = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxDailyPrice.Size;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxDailyPrice_Click);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = "p_ddl_" + childIndex;

                TextBox pText = new TextBox();
                pText.Size = textBoxDailyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //add row style here
                tableLayoutPanelDailyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDailyPrice.Controls.Add(pCombo);
                tableLayoutPanelDailyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDailyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelDailyPrice.Controls.SetChildIndex(pText, childIndex + 1);
                Daily_chart();
            }
            
        }
        private void priceComboBoxDailyPrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDailyPrice.ColumnCount + tableLayoutPanelDailyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_p < childIndex || ddl_p == 0)
            {
                ddl_p = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxDailyPrice.Size;
                pCombo.Dock = DockStyle.Fill;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxDailyPrice_Click);
                pCombo.Name = "p_ddl_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxDailyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //add row style here
                tableLayoutPanelDailyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDailyPrice.Controls.Add(pCombo);
                tableLayoutPanelDailyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDailyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelDailyPrice.Controls.SetChildIndex(pText, childIndex + 1);

                Daily_chart();
            }
        }
        private static int ddl_c = 0;
        private void indicatorComboBoxDailyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDailyIndicator.ColumnCount + tableLayoutPanelDailyIndicator.Controls.GetChildIndex((Control)sender);
            ddl_c = childIndex;
            IndicatorComboBox indBox = new IndicatorComboBox();
            indBox.Size = indicatorComboBoxDailyIndicator.Size;
            indBox.Dock = DockStyle.Fill;
            indBox.Name = "ddl_" + ddl_c;
            indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDailyIndicator_Click);

            TextBox pText = new TextBox();
            pText.Dock = DockStyle.Fill;
            pText.Size = textBoxDailyIndicator.Size;

            //add row style here
            tableLayoutPanelDailyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            tableLayoutPanelDailyIndicator.Controls.Add(indBox);
            tableLayoutPanelDailyIndicator.Controls.Add(pText);

            //move to the right location
            tableLayoutPanelDailyIndicator.Controls.SetChildIndex(indBox, childIndex);
            tableLayoutPanelDailyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

            //add ZedGraph to match with the indicators from above

            tableLayoutPanelDailyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            ZedGraphControl zChart = new ZedGraphControl();
            zChart.Dock = DockStyle.Fill;
            zChart.Size = new Size(tableLayoutPanelDailyMain.GetColumnWidths()[0], 200);
            zChart.AutoSize = true;

            indicator_daily(indicatorComboBoxDailyIndicator.SelectedItem.ToString(), zChart);

            tableLayoutPanelDailyMain.Controls.Add(zChart, 0, tableLayoutPanelDailyMain.RowCount - 1);
        } 
        private void indicatorComboBoxDailyIndicator_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDailyIndicator.ColumnCount + tableLayoutPanelDailyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_c < childIndex || ddl_c == 0)
            {
                ddl_c = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxDailyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDailyIndicator_Click);
                indBox.Name = "ddl_" + ddl_c;

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxDailyIndicator.Size;


                //add row style here
                tableLayoutPanelDailyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDailyIndicator.Controls.Add(indBox);
                tableLayoutPanelDailyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDailyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelDailyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above    
                tableLayoutPanelDailyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelDailyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                TableLayoutControlCollection x = tableLayoutPanelDailyIndicator.Controls;
                IndicatorComboBox _x = x.Find("ddl_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                indicator_daily(_x.SelectedItem.ToString(), zChart);

                tableLayoutPanelDailyMain.Controls.Add(zChart, 0, tableLayoutPanelDailyMain.RowCount - 1);
            }
          
        }
        private void comboBoxDailyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            Daily_chart();
        }
        private void Daily_chart()
        {
            if (this.Symbol != string.Empty || this.Symbol != null)
            {
                if (comboBoxDailyChart.SelectedItem != null)
                {
                    MainChart mi = new MainChart();
                    mi.FileName = this.Symbol + "_daily.csv";
                    mi.FolderName = "Daily";
                    mi.Path = this.path;
                    mi.Symbol = this.Symbol;
                    mi.Layout = tableLayoutPanelDailyPrice;
                    mi.Overlay_key = "p_ddl_";
                    string Selected = comboBoxDailyChart.SelectedItem.ToString();
                    quote_daily.Clear();
                    quote_daily.AddRange(mi.Quotes());
                    mi.Chart(Selected, zedGraphDailyTop, quote_daily);

                    //MessageBox.Show("Please Select Symbol." );
                }
                
            }
            else
                MessageBox.Show("Please Select Symbol.");

        }
        private void indicator_daily(string indectorName, ZedGraphControl zgc)
        {
            IndectorPrice ip = new IndectorPrice(quote_daily);
            List<Overlay> ov = ip.indicator_call(indectorName);

            if (quote_daily.Count > 0)
            {
                GraphPane myPane = zgc.GraphPane;
                myPane.Title.Text = indectorName;
                myPane.XAxis.Title.Text = "X";
                myPane.YAxis.Title.Text = "Y";

                foreach (Overlay o in ov)
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

                zgc.AxisChange();
            }
        }
        #endregion

        #region weekly Tab
        private static int ddl_w_p = 0;
        private void priceComboBoxWeeklyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelWeeklyPrice.ColumnCount + tableLayoutPanelWeeklyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_w_p < childIndex || ddl_w_p == 0)
            {
                ddl_w_p = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxWeeklyPrice.Size;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboWeeklyPrice_Click);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = "ddl_w_p_" + childIndex;

                TextBox pText = new TextBox();
                pText.Size = textBoxWeeklyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //add row style here
                tableLayoutPanelWeeklyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelWeeklyPrice.Controls.Add(pCombo);
                tableLayoutPanelWeeklyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelWeeklyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelWeeklyPrice.Controls.SetChildIndex(pText, childIndex + 1);

                //show comments for developer only
                //MessageBox.Show("Add this price indicator into Left side -> Top Price Chart!");
                WeeklyChart();
            }
        }
        private void priceComboWeeklyPrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelWeeklyPrice.ColumnCount + tableLayoutPanelWeeklyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_w_p < childIndex || ddl_w_p == 0)
            {
                ddl_w_p = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxWeeklyPrice.Size;
                pCombo.Dock = DockStyle.Fill;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboWeeklyPrice_Click);
                pCombo.Name = "ddl_w_p_" + childIndex;

                TextBox pText = new TextBox();
                pText.Size = textBoxWeeklyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //add row style here
                tableLayoutPanelWeeklyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelWeeklyPrice.Controls.Add(pCombo);
                tableLayoutPanelWeeklyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelWeeklyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelWeeklyPrice.Controls.SetChildIndex(pText, childIndex + 1);

                WeeklyChart();
            }
        }
        private static int ddl_w_i = 0;
        private void indicatorComboBoxWeeklyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelWeeklyIndicator.ColumnCount + tableLayoutPanelWeeklyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_w_i < childIndex || ddl_w_i == 0)
            {
                ddl_w_i = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxWeeklyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.Name = "ddl_w_i_" + childIndex;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxWeeklyIndicator_Click);

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxWeeklyIndicator.Size;


                tableLayoutPanelWeeklyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelWeeklyIndicator.Controls.Add(indBox);
                tableLayoutPanelWeeklyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelWeeklyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelWeeklyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above
                //
                tableLayoutPanelWeeklyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelWeeklyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                indicator_weekly(indicatorComboBoxWeeklyIndicator.SelectedItem.ToString(), zChart);

                tableLayoutPanelWeeklyMain.Controls.Add(zChart, 0, tableLayoutPanelWeeklyMain.RowCount - 1);

            }
        }
        private void indicator_weekly(string indectorName, ZedGraphControl zgc)
        {
            IndectorPrice ip = new IndectorPrice(quote_weekly);
            List<Overlay> ov = ip.indicator_call(indectorName);

            if (quote_weekly.Count > 0)
            {
                GraphPane myPane = zgc.GraphPane;
                myPane.Title.Text = indectorName;
                myPane.XAxis.Title.Text = "X";
                myPane.YAxis.Title.Text = "Y";

                foreach (Overlay o in ov)
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

                zgc.AxisChange();
            }
        }
        private void indicatorComboBoxWeeklyIndicator_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelWeeklyIndicator.ColumnCount + tableLayoutPanelWeeklyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_w_i < childIndex || ddl_w_i == 0)
            {
                ddl_w_i = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxWeeklyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxWeeklyIndicator_Click);
                indBox.Name = "ddl_w_i_" + childIndex;

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxWeeklyIndicator.Size;

                tableLayoutPanelWeeklyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelWeeklyIndicator.Controls.Add(indBox);
                tableLayoutPanelWeeklyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelWeeklyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelWeeklyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above
                //
                tableLayoutPanelWeeklyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelWeeklyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                TableLayoutControlCollection x = tableLayoutPanelWeeklyIndicator.Controls;
                IndicatorComboBox _x = x.Find("ddl_w_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                indicator_weekly(_x.SelectedItem.ToString(), zChart);

                tableLayoutPanelWeeklyMain.Controls.Add(zChart, 0, tableLayoutPanelWeeklyMain.RowCount - 1);
            }
        }

        private void comboBoxWeeklyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            WeeklyChart();
        }
        private void WeeklyChart()
        {
            if (this.Symbol != string.Empty || this.Symbol != null)
            {
                if (comboBoxWeeklyChart.SelectedItem != null)
                {
                    MainChart mi = new MainChart();
                    mi.FileName = this.Symbol + "_weekly.csv";
                    mi.FolderName = "Weekly";
                    mi.Path = this.path;
                    mi.Symbol = this.Symbol;
                    mi.Layout = tableLayoutPanelWeeklyPrice;
                    mi.Overlay_key = "ddl_w_p_";
                    string WeekliChartSelected = comboBoxWeeklyChart.SelectedItem.ToString();
                    quote_weekly.Clear();
                    quote_weekly.AddRange(mi.Quotes());
                    mi.Chart(WeekliChartSelected, zedGraphControlWeeklyTop, quote_weekly);
                }
            }
            else
                MessageBox.Show("Please Select Symbol.");
        }

        #endregion

        #region Monthly Tab
        private static int ddl_m_p = 0;
        private void priceComboBoxMonthlyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            int childIndex = tableLayoutPanelMonthlyPrice.ColumnCount + tableLayoutPanelMonthlyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_m_p < childIndex || ddl_m_p == 0)
            {
                ddl_m_p = childIndex;

                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxMonthlyPrice.Size;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxMonthlyPrice_Click);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = "ddl_m_p_" + childIndex;

                TextBox pText = new TextBox();
                pText.Size = textBoxMonthlyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                tableLayoutPanelMonthlyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMonthlyPrice.Controls.Add(pCombo);
                tableLayoutPanelMonthlyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMonthlyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelMonthlyPrice.Controls.SetChildIndex(pText, childIndex + 1);
                MonthlyChart();
                //show comments for developer only
                //MessageBox.Show("Add this price indicator into Top Price Chart!");
            }
        }
        private void priceComboBoxMonthlyPrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMonthlyPrice.ColumnCount + tableLayoutPanelMonthlyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_m_p < childIndex || ddl_m_p == 0)
            {
                ddl_m_p = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxMonthlyPrice.Size;
                pCombo.Dock = DockStyle.Fill;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxMonthlyPrice_Click);
                pCombo.Name = "ddl_m_p_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxMonthlyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                //
                tableLayoutPanelMonthlyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMonthlyPrice.Controls.Add(pCombo);
                tableLayoutPanelMonthlyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMonthlyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelMonthlyPrice.Controls.SetChildIndex(pText, childIndex + 1);
                MonthlyChart();
                //show comments for developer only
                // MessageBox.Show("Add this price indicator into Left side -> Top Chart!");
            }
        }
        private static int ddl_m_i = 0;
        private void indicatorComboBoxMonthlyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMonthlyIndicator.ColumnCount + tableLayoutPanelMonthlyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_m_i < childIndex || ddl_m_i == 0)
            {
                ddl_m_i = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxMonthlyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.Name = "ddl_m_i_" + childIndex;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxMonthlyIndicator_Click);

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxMonthlyIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelMonthlyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMonthlyIndicator.Controls.Add(indBox);
                tableLayoutPanelMonthlyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMonthlyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelMonthlyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above
                //
                tableLayoutPanelMonthlyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelMonthlyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                indicator_monthly(indicatorComboBoxMonthlyIndicator.SelectedItem.ToString(), zChart);

                tableLayoutPanelMonthlyMain.Controls.Add(zChart, 0, tableLayoutPanelMonthlyMain.RowCount - 1);
            }
        }
        private void indicator_monthly(string indectorName, ZedGraphControl zgc)
        {
            IndectorPrice ip = new IndectorPrice(quote_monthly);
            List<Overlay> ov = ip.indicator_call(indectorName);

            if (quote_monthly.Count > 0)
            {
                GraphPane myPane = zgc.GraphPane;
                myPane.Title.Text = indectorName;
                myPane.XAxis.Title.Text = "X";
                myPane.YAxis.Title.Text = "Y";

                foreach (Overlay o in ov)
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

                zgc.AxisChange();
            }
        }
        private void indicatorComboBoxMonthlyIndicator_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMonthlyIndicator.ColumnCount + tableLayoutPanelMonthlyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_m_i < childIndex || ddl_m_i == 0)
            {
                ddl_m_i = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxMonthlyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.Name = "ddl_m_i_" + childIndex;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxMonthlyIndicator_Click);

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxMonthlyIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelMonthlyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMonthlyIndicator.Controls.Add(indBox);
                tableLayoutPanelMonthlyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMonthlyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelMonthlyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above    
                tableLayoutPanelMonthlyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelMonthlyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                TableLayoutControlCollection x = tableLayoutPanelMonthlyIndicator.Controls;
                IndicatorComboBox _x = x.Find("ddl_m_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                //MessageBox.Show(_x.SelectedItem.ToString());
                indicator_monthly(_x.SelectedItem.ToString(), zChart);

                tableLayoutPanelMonthlyMain.Controls.Add(zChart, 0, tableLayoutPanelMonthlyMain.RowCount - 1);
            }
        }
        private void comboBoxMonthlyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            MonthlyChart();
        }
        private void MonthlyChart()
        {
            if (this.Symbol != string.Empty || this.Symbol != null)
            {
                if (comboBoxMonthlyChart.SelectedItem != null)
                {
                    MainChart mi = new MainChart();
                    mi.FileName = this.Symbol + "_monthly.csv";
                    mi.FolderName = "Monthly";
                    mi.Path = this.path;
                    mi.Symbol = this.Symbol;
                    mi.Layout = tableLayoutPanelMonthlyPrice;
                    mi.Overlay_key = "ddl_m_p_";
                    string WeekliChartSelected = comboBoxMonthlyChart.SelectedItem.ToString();
                    quote_monthly.Clear();
                    quote_monthly.AddRange(mi.Quotes());
                    mi.Chart(WeekliChartSelected, zedGraphControlMonthlyTop, quote_monthly);
                }
            }
            else
                MessageBox.Show("Please Select Symbol.");
        }
        #endregion

        #region Minute tab
        private static int ddl_min_p_ = 0;
        private void priceComboBoxMinutePrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMinutePrice.ColumnCount + tableLayoutPanelMinutePrice.Controls.GetChildIndex((Control)sender);
            if (ddl_min_p_ < childIndex || ddl_min_p_ == 0)
            {
                ddl_min_p_ = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxMinutePrice.Size;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxMinutePrice_Click);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = "ddl_min_p_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxMinutePrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                //
                tableLayoutPanelMinutePrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMinutePrice.Controls.Add(pCombo);
                tableLayoutPanelMinutePrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMinutePrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelMinutePrice.Controls.SetChildIndex(pText, childIndex + 1);
                MinuteChart();
                //show comments for developer only
                // MessageBox.Show("Add this price indicator into Left side->Top Price Chart!");
            }
        }
        private void priceComboBoxMinutePrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMinutePrice.ColumnCount + tableLayoutPanelMinutePrice.Controls.GetChildIndex((Control)sender);
            if (ddl_min_p_ < childIndex || ddl_min_p_ == 0)
            {
                ddl_min_p_ = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxMinutePrice.Size;
                pCombo.Dock = DockStyle.Fill;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxMinutePrice_Click);
                pCombo.Name = "ddl_min_p_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxMinutePrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                //
                tableLayoutPanelMinutePrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMinutePrice.Controls.Add(pCombo);
                tableLayoutPanelMinutePrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMinutePrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelMinutePrice.Controls.SetChildIndex(pText, childIndex + 1);
                MinuteChart();
            }
            //show comments for developer only
            //MessageBox.Show("Add this price indicator into Left side -> Top Chart!");
        }
        private static int ddl_min_i_ = 0;
        private void indicatorComboBoxMinuteIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMinuteIndicator.ColumnCount + tableLayoutPanelMinuteIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_min_i_ < childIndex || ddl_min_i_ == 0)
            {
                ddl_min_i_ = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxMinuteIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.Name = "ddl_min_i_" + childIndex;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxMinuteIndicator_Click);

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxMinuteIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelMinuteIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMinuteIndicator.Controls.Add(indBox);
                tableLayoutPanelMinuteIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMinuteIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelMinuteIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above
                //
                tableLayoutPanelMinuteMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelDailyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;
                indicator_min(indicatorComboBoxMinuteIndicator.SelectedItem.ToString(), zChart,quote_min);
                tableLayoutPanelMinuteMain.Controls.Add(zChart, 0, tableLayoutPanelMinuteMain.RowCount - 1);
            }
        }   
        private void indicatorComboBoxMinuteIndicator_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelMinuteIndicator.ColumnCount + tableLayoutPanelMinuteIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_min_i_ < childIndex || ddl_min_i_ == 0)
            {
                ddl_min_i_ = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxMinuteIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxMinuteIndicator_Click);
                indBox.Name = "ddl_min_i_" + childIndex;
                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxMinuteIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelMinuteIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelMinuteIndicator.Controls.Add(indBox);
                tableLayoutPanelMinuteIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelMinuteIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelMinuteIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above    
                tableLayoutPanelMinuteMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelMinuteMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;


                TableLayoutControlCollection x = tableLayoutPanelMinuteIndicator.Controls;
                IndicatorComboBox _x = x.Find("ddl_min_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                //MessageBox.Show(_x.SelectedItem.ToString());
                indicator_min(_x.SelectedItem.ToString(), zChart,quote_min);

                tableLayoutPanelMinuteMain.Controls.Add(zChart, 0, tableLayoutPanelMinuteMain.RowCount - 1);
            }
        }
        private void comboBoxMinuteChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            MinuteChart();
        }
        private void MinuteChart()
        {
            if (this.Symbol != string.Empty || this.Symbol != null)
            {
                if (comboBoxMinuteChart.SelectedItem != null)
                {
                    MainChart mi = new MainChart();
                    mi.FileName = this.Symbol + "_15min.csv";
                    mi.FolderName = "15min";
                    mi.Path = this.path;
                    mi.Symbol = this.Symbol;
                    mi.Layout = tableLayoutPanelMinutePrice;
                    mi.Overlay_key = "ddl_min_p_";
                    string MinuteChartSelected = comboBoxMinuteChart.SelectedItem.ToString();
                    quote_min.Clear();
                    quote_min.AddRange(mi.Quotes());
                    mi.Chart(MinuteChartSelected, zedGraphControlMinuteTop, quote_min);
                }
            }
            else
                MessageBox.Show("Please Select Symbol.");
        }
        #endregion


        #region DowDaily
        OnChangeHandeller DowDaily_Hendal = new OnChangeHandeller();
        private void comboBoxDowDailyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowDailyChart();
        }
        private void DowDailyChart()
        {
            if (comboBoxDowDailyChart.SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "DOW_daily.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelDowDailyPrice;
                mi.Overlay_key = "ddl_dow_day_p_";
                string Selected = comboBoxDowDailyChart.SelectedItem.ToString();
                quote_dow_daily.Clear();
                quote_dow_daily.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlDowDailyTop, quote_dow_daily);
            }

            //foreach (Control p in tableLayoutPanelDowDailyPrice.Controls)
            //{
            //    if (p.Name.Contains("ddl_dow_day_p_"))
            //    {
            //        TableLayoutControlCollection x = tableLayoutPanelDowDailyPrice.Controls;
            //        PriceComboBox _x = x.Find(p.Name, false).FirstOrDefault() as PriceComboBox;
            //        MessageBox.Show(_x.SelectedItem.ToString());
            //    }
            //}
        }
        private void priceComboBoxDowDailyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowDaily_Hendal.Price_Name = "ddl_dow_day_p_";
            DowDaily_Hendal.ComboBox = priceComboBoxDowDailyPrice;
            DowDaily_Hendal.OnChangeClick = priceComboBoxDowDailyPrice_Click;
            DowDaily_Hendal.OverlayLayout_Price = tableLayoutPanelDowDailyPrice;
            DowDaily_Hendal.Price_sender = sender;
            DowDaily_Hendal.Price_textBox = textBoxDowDailyPrice;
            DowDaily_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            DowDailyChart();

        }
        private void priceComboBoxDowDailyPrice_Click(object sender, EventArgs e)
        {
            DowDaily_Hendal.Price_Name = "ddl_dow_day_p_";
            DowDaily_Hendal.ComboBox = priceComboBoxDowDailyPrice;
            DowDaily_Hendal.OnChangeClick = priceComboBoxDowDailyPrice_Click;
            DowDaily_Hendal.OverlayLayout_Price = tableLayoutPanelDowDailyPrice;
            DowDaily_Hendal.Price_sender = sender;
            DowDaily_Hendal.Price_textBox = textBoxDowDailyPrice;
            DowDaily_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            DowDailyChart();
        }

        //Dow Daily Indicator tab
        private void indicatorComboBoxDowDailyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowDaily_Hendal.IndectorLayout = tableLayoutPanelDowDailyIndicator;
            DowDaily_Hendal.Indector_Combobox = indicatorComboBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxDowDailyIndicator_Click;
            DowDaily_Hendal.Indector_Name = "ddl_dow_day_i_";
            DowDaily_Hendal.Indector_sender = sender;
            DowDaily_Hendal.Indector_TextBox = textBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_MainLayout = tableLayoutPanelDowDailyMain;
            DowDaily_Hendal.Quotes = quote_dow_daily;
            DowDaily_Hendal.Selected = true;
            DowDaily_Hendal.SelectedIteam = indicatorComboBoxDowDailyIndicator.SelectedItem.ToString();
            DowDaily_Hendal.OnIndectorChange();



            ////this 2 is the column count
            //int childIndex = tableLayoutPanelDowDailyIndicator.ColumnCount + tableLayoutPanelDowDailyIndicator.Controls.GetChildIndex((Control)sender);
            //if (ddl_min_i_ < childIndex || ddl_min_i_ == 0)
            //{
            //    ddl_min_i_ = childIndex;
            //    IndicatorComboBox indBox = new IndicatorComboBox();
            //    indBox.Size = indicatorComboBoxDowDailyIndicator.Size;
            //    indBox.Dock = DockStyle.Fill;
            //    indBox.Name = "ddl_min_i_" + childIndex;
            //    indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDowDailyIndicator_Click);

            //    TextBox pText = new TextBox();
            //    pText.Dock = DockStyle.Fill;
            //    pText.Size = textBoxDowDailyIndicator.Size;

            //    //
            //    //add row style here
            //    //
            //    tableLayoutPanelDowDailyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            //    tableLayoutPanelDowDailyIndicator.Controls.Add(indBox);
            //    tableLayoutPanelDowDailyIndicator.Controls.Add(pText);

            //    //move to the right location
            //    tableLayoutPanelDowDailyIndicator.Controls.SetChildIndex(indBox, childIndex);
            //    tableLayoutPanelDowDailyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

            //    //add ZedGraph to match with the indicators from above
            //    //
            //    tableLayoutPanelDowDailyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            //    ZedGraphControl zChart = new ZedGraphControl();
            //    zChart.Dock = DockStyle.Fill;
            //    zChart.Size = new Size(tableLayoutPanelDailyMain.GetColumnWidths()[0], 200);
            //    zChart.AutoSize = true;
            //    indicator_min(indicatorComboBoxDowDailyIndicator.SelectedItem.ToString(), zChart);
            //    tableLayoutPanelDowDailyMain.Controls.Add(zChart, 0, tableLayoutPanelDowDailyMain.RowCount - 1);
            //}
        }

        private void indicatorComboBoxDowDailyIndicator_Click(object sender, EventArgs e)
        {
            DowDaily_Hendal.IndectorLayout = tableLayoutPanelDowDailyIndicator;
            DowDaily_Hendal.Indector_Combobox = indicatorComboBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxDowDailyIndicator_Click;
            DowDaily_Hendal.Indector_Name = "ddl_dow_day_i_";
            DowDaily_Hendal.Indector_sender = sender;
            DowDaily_Hendal.Indector_TextBox = textBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_MainLayout = tableLayoutPanelDowDailyMain;
            DowDaily_Hendal.Quotes = quote_dow_daily;
            DowDaily_Hendal.Selected = false;
            //DowDaily_Hendal.SelectedIteam = indicatorComboBoxDowDailyIndicator.SelectedItem.ToString();
            DowDaily_Hendal.OnIndectorChange();
            ////this 2 is the column count
            //int childIndex = tableLayoutPanelDowDailyIndicator.ColumnCount + tableLayoutPanelDowDailyIndicator.Controls.GetChildIndex((Control)sender);
            //if (ddl_min_i_ < childIndex || ddl_min_i_ == 0)
            //{
            //    ddl_min_i_ = childIndex;
            //    IndicatorComboBox indBox = new IndicatorComboBox();
            //    indBox.Size = indicatorComboBoxDowDailyIndicator.Size;
            //    indBox.Dock = DockStyle.Fill;
            //    indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDowDailyIndicator_Click);
            //    indBox.Name = "ddl_min_i_" + childIndex;
            //    TextBox pText = new TextBox();
            //    pText.Dock = DockStyle.Fill;
            //    pText.Size = textBoxDowDailyIndicator.Size;

            //    //
            //    //add row style here
            //    //
            //    tableLayoutPanelDowDailyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            //    tableLayoutPanelDowDailyIndicator.Controls.Add(indBox);
            //    tableLayoutPanelDowDailyIndicator.Controls.Add(pText);

            //    //move to the right location
            //    tableLayoutPanelDowDailyIndicator.Controls.SetChildIndex(indBox, childIndex);
            //    tableLayoutPanelDowDailyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

            //    //add ZedGraph to match with the indicators from above    
            //    tableLayoutPanelDowDailyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            //    ZedGraphControl zChart = new ZedGraphControl();
            //    zChart.Dock = DockStyle.Fill;
            //    zChart.Size = new Size(tableLayoutPanelDowDailyMain.GetColumnWidths()[0], 200);
            //    zChart.AutoSize = true;


            //    TableLayoutControlCollection x = tableLayoutPanelDowDailyIndicator.Controls;
            //    IndicatorComboBox _x = x.Find("ddl_min_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
            //    //MessageBox.Show(_x.SelectedItem.ToString());
            //    indicator_min(_x.SelectedItem.ToString(), zChart);

            //    tableLayoutPanelDowDailyMain.Controls.Add(zChart, 0, tableLayoutPanelDowDailyMain.RowCount - 1);
            //}
        }

        #endregion

        #region DowWeekly
        OnChangeHandeller DowWeekly_Hendal = new OnChangeHandeller();
        private void comboBoxDowWeeklyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowWeeklyChart();
        }
        private void DowWeeklyChart()
        {
            if (comboBoxDowWeeklyChart .SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "DOW_weekly.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelDowWeeklyPrice;
                mi.Overlay_key = "ddl_dow_week_p_";
                string Selected = comboBoxDowWeeklyChart.SelectedItem.ToString();
                quote_dow_weekly.Clear();
                quote_dow_weekly.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlDowWeeklyTop, quote_dow_weekly);
            }
        }
        private void priceComboBoxDowWeeklyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowWeekly_Hendal.Price_Name = "ddl_dow_week_p_";
            DowWeekly_Hendal.ComboBox = priceComboBoxDowWeeklyPrice;
            DowWeekly_Hendal.OnChangeClick = priceComboBoxDowWeeklyPrice_Click;
            DowWeekly_Hendal.OverlayLayout_Price = tableLayoutPanelDowWeeklyPrice;
            DowWeekly_Hendal.Price_sender = sender;
            DowWeekly_Hendal.Price_textBox = textBoxDowWeeklyPrice;
            DowWeekly_Hendal.OnChange();
            DowWeeklyChart();
        }
        private void priceComboBoxDowWeeklyPrice_Click(object sender, EventArgs e)
        {
            DowWeekly_Hendal.Price_Name = "ddl_dow_week_p_";
            DowWeekly_Hendal.ComboBox = priceComboBoxDowWeeklyPrice;
            DowWeekly_Hendal.OnChangeClick = priceComboBoxDowWeeklyPrice_Click;
            DowWeekly_Hendal.OverlayLayout_Price = tableLayoutPanelDowWeeklyPrice;
            DowWeekly_Hendal.Price_sender = sender;
            DowWeekly_Hendal.Price_textBox = textBoxDowWeeklyPrice;
            DowWeekly_Hendal.OnChange();
            DowWeeklyChart();
        }
        //Dow Weekly Indicator Tab
        private void indicatorComboBoxDowWeeklyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowWeekly_Hendal.IndectorLayout = tableLayoutPanelDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_Combobox = indicatorComboBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowWeeklyIndicator_Click;
            DowWeekly_Hendal.Indector_Name = "ddl_dow_week_i_";
            DowWeekly_Hendal.Indector_sender = sender;
            DowWeekly_Hendal.Indector_TextBox = textBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_MainLayout = tableLayoutPanelDowWeeklyMain;
            DowWeekly_Hendal.Quotes = quote_dow_weekly;
            DowWeekly_Hendal.Selected = true;
            DowWeekly_Hendal.SelectedIteam = indicatorComboBoxDowWeeklyIndicator.SelectedItem.ToString();
            DowWeekly_Hendal.OnIndectorChange();

        }
        private void indicatorComboBoxDowWeeklyIndicator_Click(object sender, EventArgs e)
        {
            DowWeekly_Hendal.IndectorLayout = tableLayoutPanelDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_Combobox = indicatorComboBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowWeeklyIndicator_Click;
            DowWeekly_Hendal.Indector_Name = "ddl_dow_week_i_";
            DowWeekly_Hendal.Indector_sender = sender;
            DowWeekly_Hendal.Indector_TextBox = textBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_MainLayout = tableLayoutPanelDowWeeklyMain;
            DowWeekly_Hendal.Quotes = quote_dow_weekly;
            DowWeekly_Hendal.Selected = false;
            DowWeekly_Hendal.SelectedIteam = indicatorComboBoxDowWeeklyIndicator.SelectedItem.ToString();
            DowWeekly_Hendal.OnIndectorChange();
        }
        #endregion

        #region DowMonthly
        private void comboBoxDowMonthlyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowMonthlyChat();
        }

        private void DowMonthlyChat()
        {
            if (comboBoxDowMonthlyChart.SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "DOW_monthly.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelDowMonthlyPrice;
                mi.Overlay_key = "ddl_dow_month_p_";
                string Selected = comboBoxDowMonthlyChart.SelectedItem.ToString();
                quote_dow_monthly.Clear();
                quote_dow_monthly.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlDowMonthlyTop, quote_dow_monthly);
            }
        }

        private void indicator_min(string indectorName, ZedGraphControl zgc,List<Quote> quotes)
        {
            IndectorPrice ip = new IndectorPrice(quotes);
            List<Overlay> ov = ip.indicator_call(indectorName);

            if (quotes.Count > 0)
            {
                GraphPane myPane = zgc.GraphPane;
                myPane.Title.Text = indectorName;
                myPane.XAxis.Title.Text = "X";
                myPane.YAxis.Title.Text = "Y";

                foreach (Overlay o in ov)
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

                zgc.AxisChange();
            }
        }

        int ddl_dow_month_p_ = 0;
        //Dow Monthly tab
        private void priceComboBoxDowMonthlyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDowMonthlyPrice.ColumnCount + tableLayoutPanelDowMonthlyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_dow_month_p_ < childIndex || ddl_dow_month_p_ == 0)
            {
                ddl_dow_month_p_ = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxDowMonthlyPrice.Size;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxDowMonthlyPrice_Click);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = "ddl_dow_month_p_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxDowMonthlyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                //
                tableLayoutPanelDowMonthlyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDowMonthlyPrice.Controls.Add(pCombo);
                tableLayoutPanelDowMonthlyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pText, childIndex + 1);

                //DowMonthlyChart();
                //show comments for developer only
                // MessageBox.Show("Add this price indicator into Left side->Top Price Chart!");
            }
            DowMonthlyChat();
        }

        private void priceComboBoxDowMonthlyPrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDowMonthlyPrice.ColumnCount + tableLayoutPanelDowMonthlyPrice.Controls.GetChildIndex((Control)sender);
            if (ddl_dow_month_p_ < childIndex || ddl_dow_month_p_ == 0)
            {
                ddl_min_p_ = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = priceComboBoxDowMonthlyPrice.Size;
                pCombo.Dock = DockStyle.Fill;
                pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxDowMonthlyPrice_Click);
                pCombo.Name = "ddl_dow_month_p_" + childIndex;
                TextBox pText = new TextBox();
                pText.Size = textBoxDowMonthlyPrice.Size;
                pText.Dock = DockStyle.Fill;

                //
                //add row style here
                //
                tableLayoutPanelDowMonthlyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDowMonthlyPrice.Controls.Add(pCombo);
                tableLayoutPanelDowMonthlyPrice.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pCombo, childIndex);
                tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pText, childIndex + 1);

                //DowMonthlyChart();
            }
            DowMonthlyChat();
            //show comments for developer only
            //MessageBox.Show("Add this price indicator into Left side -> Top Chart!");
        }

        int ddl_dow_month_i_ = 0;
        //Dow Monthly Indicator Tab
        private void indicatorComboBoxDowMonthlyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDowMonthlyIndicator.ColumnCount + tableLayoutPanelDowMonthlyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_dow_month_i_ < childIndex || ddl_dow_month_i_ == 0)
            {
                ddl_dow_month_i_ = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxDowMonthlyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.Name = "ddl_dow_month_i_" + childIndex;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDowMonthlyIndicator_Click);

                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxDowMonthlyIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelDowMonthlyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDowMonthlyIndicator.Controls.Add(indBox);
                tableLayoutPanelDowMonthlyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDowMonthlyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelDowMonthlyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above
                tableLayoutPanelDowMonthlyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelDailyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;
                indicator_min(indicatorComboBoxDowMonthlyIndicator.SelectedItem.ToString(), zChart, quote_dow_monthly);

                tableLayoutPanelDowMonthlyMain.Controls.Add(zChart, 0, tableLayoutPanelDowMonthlyMain.RowCount - 1);
            }
        }

        private void indicatorComboBoxDowMonthlyIndicator_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            int childIndex = tableLayoutPanelDowMonthlyIndicator.ColumnCount + tableLayoutPanelDowMonthlyIndicator.Controls.GetChildIndex((Control)sender);
            if (ddl_dow_month_i_ < childIndex || ddl_dow_month_i_ == 0)
            {
                ddl_dow_month_i_ = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = indicatorComboBoxDowMonthlyIndicator.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxDowMonthlyIndicator_Click);
                indBox.Name = "ddl_dow_month_i_" + childIndex;
                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = textBoxDowMonthlyIndicator.Size;

                //
                //add row style here
                //
                tableLayoutPanelDowMonthlyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                tableLayoutPanelDowMonthlyIndicator.Controls.Add(indBox);
                tableLayoutPanelDowMonthlyIndicator.Controls.Add(pText);

                //move to the right location
                tableLayoutPanelDowMonthlyIndicator.Controls.SetChildIndex(indBox, childIndex);
                tableLayoutPanelDowMonthlyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above    
                tableLayoutPanelDowMonthlyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(tableLayoutPanelDowMonthlyMain.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;


                TableLayoutControlCollection x = tableLayoutPanelDowMonthlyIndicator.Controls;
                IndicatorComboBox _x = x.Find("ddl_dow_month_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;

                //MessageBox.Show(_x.SelectedItem.ToString());
                indicator_min(_x.SelectedItem.ToString(), zChart,quote_dow_monthly);

                tableLayoutPanelDowMonthlyMain.Controls.Add(zChart, 0, tableLayoutPanelDowMonthlyMain.RowCount - 1);
            }
        }
        #endregion


        #region NasdaqDaily
        OnChangeHandeller NasdaDaily_Hendal = new OnChangeHandeller();
        private void comboBoxNasdaqDailyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaDailyChat();
        }


        private void NasdaDailyChat()
        {
            if (comboBoxNasdaqDailyChart.SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "NASDAQ_daily.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelNasdaqDailyPrice;
                mi.Overlay_key = "ddl_nasdaq_day_p_";
                string Selected = comboBoxNasdaqDailyChart.SelectedItem.ToString();
                quote_nasdaq_daily.Clear();
                quote_nasdaq_daily.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlNasdaqDailyTop, quote_nasdaq_daily);
            }
        }


        //Nasdaq Daily Price Tab
        private void priceComboBoxNasdaqDailyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaDaily_Hendal.Price_Name = "ddl_nasdaq_day_p_";
            NasdaDaily_Hendal.ComboBox = priceComboBoxNasdaqDailyPrice;
            NasdaDaily_Hendal.OnChangeClick = priceComboBoxNasdaqDailyPrice_Click;
            NasdaDaily_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqDailyPrice;
            NasdaDaily_Hendal.Price_sender = sender;
            NasdaDaily_Hendal.Price_textBox = textBoxNasdaqDailyPrice;
            NasdaDaily_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaDailyChat();
            
        }

        private void priceComboBoxNasdaqDailyPrice_Click(object sender, EventArgs e)
        {
            NasdaDaily_Hendal.Price_Name = "ddl_nasdaq_day_p_";
            NasdaDaily_Hendal.ComboBox = priceComboBoxNasdaqDailyPrice;
            NasdaDaily_Hendal.OnChangeClick = priceComboBoxNasdaqDailyPrice_Click;
            NasdaDaily_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqDailyPrice;
            NasdaDaily_Hendal.Price_sender = sender;
            NasdaDaily_Hendal.Price_textBox = textBoxNasdaqDailyPrice;
            NasdaDaily_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaDailyChat();
        }

        //Nasdaq Daily Indicator Tab
        private void indicatorComboBoxNasdaqDailyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaDaily_Hendal.IndectorLayout = tableLayoutPanelNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_Combobox = indicatorComboBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqDailyIndicator_Click;
            NasdaDaily_Hendal.Indector_Name = "ddl_nasdaq_day_i_";
            NasdaDaily_Hendal.Indector_sender = sender;
            NasdaDaily_Hendal.Indector_TextBox = textBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqDailyMain;
            NasdaDaily_Hendal.Quotes = quote_nasdaq_daily;
            NasdaDaily_Hendal.Selected = true;
            NasdaDaily_Hendal.SelectedIteam = indicatorComboBoxNasdaqDailyIndicator.SelectedItem.ToString();
            NasdaDaily_Hendal.OnIndectorChange();



        }

        private void indicatorComboBoxNasdaqDailyIndicator_Click(object sender, EventArgs e)
        {
            NasdaDaily_Hendal.IndectorLayout = tableLayoutPanelNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_Combobox = indicatorComboBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqDailyIndicator_Click;
            NasdaDaily_Hendal.Indector_Name = "ddl_nasdaq_day_i_";
            NasdaDaily_Hendal.Indector_sender = sender;
            NasdaDaily_Hendal.Indector_TextBox = textBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqDailyMain;
            NasdaDaily_Hendal.Quotes = quote_nasdaq_daily;
            NasdaDaily_Hendal.Selected = false;
            NasdaDaily_Hendal.SelectedIteam = indicatorComboBoxNasdaqDailyIndicator.SelectedItem.ToString();
            NasdaDaily_Hendal.OnIndectorChange();
        }
        #endregion

        #region Nasda Weekly
        OnChangeHandeller NasdaqWeekly_Hendal = new OnChangeHandeller();
        private void comboBoxNasdaqWeeklyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaqWeekChat();
        }
        private void NasdaqWeekChat()
        {
            if (comboBoxNasdaqWeeklyChart.SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "NASDAQ_weekly.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelNasdaqWeeklyPrice;
                mi.Overlay_key = "ddl_nasdaq_week_p_";
                string Selected = comboBoxNasdaqWeeklyChart.SelectedItem.ToString();
                quote_nasdaq_weekly.Clear();
                quote_nasdaq_weekly.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlNasdaqWeeklyTop, quote_nasdaq_weekly);
            }
        }
        //Nasdaq Weekly Price tab
        private void priceComboBoxNasdaqWeeklyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {

            NasdaqWeekly_Hendal.Price_Name = "ddl_nasdaq_week_p_";
            NasdaqWeekly_Hendal.ComboBox = priceComboBoxNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.OnChangeClick = priceComboBoxNasdaqWeeklyPrice_Click;
            NasdaqWeekly_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.Price_sender = sender;
            NasdaqWeekly_Hendal.Price_textBox = textBoxNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaqWeekChat();
        }
        private void priceComboBoxNasdaqWeeklyPrice_Click(object sender, EventArgs e)
        {

            NasdaqWeekly_Hendal.Price_Name = "ddl_nasdaq_week_p_";
            NasdaqWeekly_Hendal.ComboBox = priceComboBoxNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.OnChangeClick = priceComboBoxNasdaqWeeklyPrice_Click;
            NasdaqWeekly_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.Price_sender = sender;
            NasdaqWeekly_Hendal.Price_textBox = textBoxNasdaqWeeklyPrice;
            NasdaqWeekly_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaqWeekChat();
        }


        //Nasdaq Weekly Indicator Tab
        private void indicatorComboBoxNasdaqWeeklyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaqWeekly_Hendal.IndectorLayout = tableLayoutPanelNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_Combobox = indicatorComboBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqWeeklyIndicator_Click;
            NasdaqWeekly_Hendal.Indector_Name = "ddl_nasdaq_week_i_";
            NasdaqWeekly_Hendal.Indector_sender = sender;
            NasdaqWeekly_Hendal.Indector_TextBox = textBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqWeeklyMain;
            NasdaqWeekly_Hendal.Quotes = quote_nasdaq_weekly;
            NasdaqWeekly_Hendal.Selected = true;
            NasdaqWeekly_Hendal.SelectedIteam = indicatorComboBoxNasdaqWeeklyIndicator.SelectedItem.ToString();
            NasdaqWeekly_Hendal.OnIndectorChange();
            ////this 2 is the column count
            //int childIndex = tableLayoutPanelNasdaqWeeklyIndicator.ColumnCount + tableLayoutPanelNasdaqWeeklyIndicator.Controls.GetChildIndex((Control)sender);
            //if (ddl_nasdaq_month_i_ < childIndex || ddl_nasdaq_month_i_ == 0)
            //{
            //    ddl_nasdaq_month_i_ = childIndex;
            //    IndicatorComboBox indBox = new IndicatorComboBox();
            //    indBox.Size = indicatorComboBoxNasdaqWeeklyIndicator.Size;
            //    indBox.Dock = DockStyle.Fill;
            //    indBox.Name = "ddl_nasdaq_month_i_" + childIndex;
            //    indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxNasdaqWeeklyIndicator_Click);

            //    TextBox pText = new TextBox();
            //    pText.Dock = DockStyle.Fill;
            //    pText.Size = textBoxNasdaqWeeklyIndicator.Size;

            //    //
            //    //add row style here
            //    //
            //    tableLayoutPanelNasdaqWeeklyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.Add(indBox);
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.Add(pText);

            //    //move to the right location
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.SetChildIndex(indBox, childIndex);
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

            //    //add ZedGraph to match with the indicators from above
            //    //
            //    tableLayoutPanelNasdaqWeeklyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            //    ZedGraphControl zChart = new ZedGraphControl();
            //    zChart.Dock = DockStyle.Fill;
            //    zChart.Size = new Size(tableLayoutPanelNasdaqWeeklyMain.GetColumnWidths()[0], 200);
            //    zChart.AutoSize = true;
            //    indicator_min(indicatorComboBoxNasdaqWeeklyIndicator.SelectedItem.ToString(), zChart, quote_nasdaq_weekly);
            //    tableLayoutPanelNasdaqWeeklyMain.Controls.Add(zChart, 0, tableLayoutPanelNasdaqWeeklyMain.RowCount - 1);
            //}
        }
        private void indicatorComboBoxNasdaqWeeklyIndicator_Click(object sender, EventArgs e)
        {
            NasdaqWeekly_Hendal.IndectorLayout = tableLayoutPanelNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_Combobox = indicatorComboBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqWeeklyIndicator_Click;
            NasdaqWeekly_Hendal.Indector_Name = "ddl_nasdaq_week_i_";
            NasdaqWeekly_Hendal.Indector_sender = sender;
            NasdaqWeekly_Hendal.Indector_TextBox = textBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqWeeklyMain;
            NasdaqWeekly_Hendal.Quotes = quote_nasdaq_weekly;
            NasdaqWeekly_Hendal.Selected = false;
            NasdaqWeekly_Hendal.SelectedIteam = indicatorComboBoxNasdaqWeeklyIndicator.SelectedItem.ToString();
            NasdaqWeekly_Hendal.OnIndectorChange();
            ////this 2 is the column count
            //int childIndex = tableLayoutPanelNasdaqWeeklyIndicator.ColumnCount + tableLayoutPanelNasdaqWeeklyIndicator.Controls.GetChildIndex((Control)sender);
            //if (ddl_nasdaq_month_i_ < childIndex || ddl_nasdaq_month_i_ == 0)
            //{
            //    ddl_nasdaq_month_i_ = childIndex;
            //    IndicatorComboBox indBox = new IndicatorComboBox();
            //    indBox.Size = indicatorComboBoxNasdaqWeeklyIndicator.Size;
            //    indBox.Dock = DockStyle.Fill;
            //    indBox.SelectedIndexChanged += new EventHandler(indicatorComboBoxNasdaqWeeklyIndicator_Click);
            //    indBox.Name = "ddl_nasdaq_month_i_" + childIndex;
            //    TextBox pText = new TextBox();
            //    pText.Dock = DockStyle.Fill;
            //    pText.Size = textBoxNasdaqWeeklyIndicator.Size;

            //    //
            //    //add row style here
            //    //
            //    tableLayoutPanelNasdaqWeeklyIndicator.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.Add(indBox);
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.Add(pText);

            //    //move to the right location
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.SetChildIndex(indBox, childIndex);
            //    tableLayoutPanelNasdaqWeeklyIndicator.Controls.SetChildIndex(pText, childIndex + 1);

            //    //add ZedGraph to match with the indicators from above    
            //    tableLayoutPanelNasdaqWeeklyMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            //    ZedGraphControl zChart = new ZedGraphControl();
            //    zChart.Dock = DockStyle.Fill;
            //    zChart.Size = new Size(tableLayoutPanelNasdaqWeeklyMain.GetColumnWidths()[0], 200);
            //    zChart.AutoSize = true;


            //    TableLayoutControlCollection x = tableLayoutPanelNasdaqWeeklyIndicator.Controls;
            //    IndicatorComboBox _x = x.Find("ddl_nasdaq_month_i_" + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;

            //    //MessageBox.Show(_x.SelectedItem.ToString());
            //    indicator_min(_x.SelectedItem.ToString(), zChart, quote_nasdaq_weekly);

            //    tableLayoutPanelNasdaqWeeklyMain.Controls.Add(zChart, 0, tableLayoutPanelNasdaqWeeklyMain.RowCount - 1);
            //}
        }
        #endregion

        #region Nasdaq Month
        OnChangeHandeller NasdaqMonth_Hendal = new OnChangeHandeller();
        private void comboBoxNasdaqMonthlyChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaqMonthlyChart();
        }
        private void NasdaqMonthlyChart()
        {
            if (comboBoxNasdaqMonthlyChart.SelectedItem != null)
            {
                MainChart mi = new MainChart();
                mi.FileName = "NASDAQ_monthly.csv";
                mi.FolderName = "Index";
                mi.Path = this.path;
                mi.Symbol = this.Symbol;
                mi.Layout = tableLayoutPanelNasdaqMonthlyPrice;
                mi.Overlay_key = "ddl_nasdaq_month_p_";
                string Selected = comboBoxNasdaqMonthlyChart.SelectedItem.ToString();
                quote_nasdaq_monthly.Clear();
                quote_nasdaq_monthly.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlNasdaqMonthlyTop, quote_nasdaq_monthly);
            }
        }
        //Nasdaq Monthly Price Tab
        private void priceComboBoxNasdaqMonthlyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {

            NasdaqMonth_Hendal.Price_Name = "ddl_nasdaq_month_p_";
            NasdaqMonth_Hendal.ComboBox = priceComboBoxNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.OnChangeClick = priceComboBoxNasdaqMonthlyPrice_Click;
            NasdaqMonth_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.Price_sender = sender;
            NasdaqMonth_Hendal.Price_textBox = textBoxNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaqMonthlyChart();
        }
        private void priceComboBoxNasdaqMonthlyPrice_Click(object sender, EventArgs e)
        {
            NasdaqMonth_Hendal.Price_Name = "ddl_nasdaq_month_p_";
            NasdaqMonth_Hendal.ComboBox = priceComboBoxNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.OnChangeClick = priceComboBoxNasdaqMonthlyPrice_Click;
            NasdaqMonth_Hendal.OverlayLayout_Price = tableLayoutPanelNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.Price_sender = sender;
            NasdaqMonth_Hendal.Price_textBox = textBoxNasdaqMonthlyPrice;
            NasdaqMonth_Hendal.OnChange();
            //MessageBox.Show(DowDaily_Hendal.count.ToString());
            NasdaqMonthlyChart();
        }
        //Nasdaq Monthly Indicator Tab
        private void indicatorComboBoxNasdaqMonthlyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            NasdaqMonth_Hendal.IndectorLayout = tableLayoutPanelNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_Combobox = indicatorComboBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqMonthlyIndicator_Click;
            NasdaqMonth_Hendal.Indector_Name = "ddl_nasdaq_month_i_";
            NasdaqMonth_Hendal.Indector_sender = sender;
            NasdaqMonth_Hendal.Indector_TextBox = textBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqMonthlyMain;
            NasdaqMonth_Hendal.Quotes = quote_nasdaq_monthly;
            NasdaqMonth_Hendal.Selected = true;
            NasdaqMonth_Hendal.SelectedIteam = indicatorComboBoxNasdaqMonthlyIndicator.SelectedItem.ToString();
            NasdaqMonth_Hendal.OnIndectorChange();


        }

        private void indicatorComboBoxNasdaqMonthlyIndicator_Click(object sender, EventArgs e)
        {
            NasdaqMonth_Hendal.IndectorLayout = tableLayoutPanelNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_Combobox = indicatorComboBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqMonthlyIndicator_Click;
            NasdaqMonth_Hendal.Indector_Name = "ddl_nasdaq_month_i_";
            NasdaqMonth_Hendal.Indector_sender = sender;
            NasdaqMonth_Hendal.Indector_TextBox = textBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqMonthlyMain;
            NasdaqMonth_Hendal.Quotes = quote_nasdaq_monthly;
            NasdaqMonth_Hendal.Selected = false;
            NasdaqMonth_Hendal.SelectedIteam = indicatorComboBoxNasdaqMonthlyIndicator.SelectedItem.ToString();
            NasdaqMonth_Hendal.OnIndectorChange();



        }
        #endregion
    }

    #region Class
    public class Overlay
    {
        public string Name { get; set; }
        public PointPairList pList { get; set; }
        public Color Color { get; set; }
        public SymbolType Type { get; set; }
    }

    public class StockPrice
    {
        public string date { get; set; }
        public string open { get; set; }
        public string low { get; set; }
        public string high { get; set; }
        public string close { get; set; }
        public string volume { get; set; }
    }
    public class Ascender : IComparer<PointPair>
    {
        public int Compare(PointPair x, PointPair y)
        {
            return x.X.CompareTo(y.X);
        }
    }

    public class QuoteComparer : IComparer<Quote>
    {
        public int Compare(Quote x, Quote y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
    public class StockComparer : IComparer<StockPrice>
    {
        public int Compare(StockPrice x, StockPrice y)
        {
            var _x = Convert.ToDateTime(x.date);
            var _y = Convert.ToDateTime(y.date);
            return _x.CompareTo(_y);
        }
    }
    public class PointPairComparer : IComparer<PointPair>
    {
        public int Compare(PointPair x, PointPair y)
        {
            return x.X.CompareTo(y.X);
        }
    }

    public class StockPtsComparer : IComparer<StockPt>
    {
        public int Compare(StockPt x, StockPt y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
    #endregion

}
