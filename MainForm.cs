using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;
using Skender.Stock.Indicators;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using System.Text.RegularExpressions;

namespace zCharts
{
    public partial class MainForm : Form
    {
        #region indicator chart height
        public static int indicatorHeight = 0;
        #endregion

        #region Global Veriable and Right Panel setups
        loader lo = new loader();
        liteDB db = new liteDB(Properties.Settings.Default["liteDB"].ToString());
        string[] table = new string[] { "daily" };

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

        //CandleStick Scan -- Sequence =16
        int PW16 = 0;
        bool Hided16 = false;

        //Business Logic
        string tree = "";
        string path { get; set; }

        string Symbol { get; set; }
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

        #endregion

        #region Main Functions and GUI - All right Panel setups
        private string __f_overlay(PriceComboBox price)
        {
            if (price.SelectedItem != null)
            {
                return price.SelectedItem.ToString();
            }
            return null;
        }
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
            foreach(string t in table)
                dataGridViewRSIDaily.DataSource = db.view(t);

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

            //CandleStick Scan settings
            PW16 = SPanel15.Width;
            Hided16 = false;
            buttonLeft16.Visible = false;
            buttonTop16.Visible = true;

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Get the height for indicator chart for each indicator
            Graphics g = this.CreateGraphics();
            try
            {
                //set 3 inches height
                indicatorHeight = Convert.ToInt32( 3.0* g.DpiY);
            }
            finally
            {
                g.Dispose();
            }

            zedGraphDailyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlWeeklyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlMonthlyTop.GraphPane.XAxis.Type= AxisType.Date;
            zedGraphControlMinuteTop.GraphPane.XAxis.Type = AxisType.Date;

            zedGraphControlDowDailyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlDowWeeklyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlDowMonthlyTop.GraphPane.XAxis.Type = AxisType.Date;

            zedGraphControlNasdaqDailyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlNasdaqWeeklyTop.GraphPane.XAxis.Type = AxisType.Date;
            zedGraphControlNasdaqMonthlyTop.GraphPane.XAxis.Type = AxisType.Date;

            //Set default values for RSI Scan
            numericUpDownRSIDaily.Value = 3;
            numericUpDownRSIWeekly.Value = 3;

            //clear up RSI Scan datagridviews
            //for daily and weekly scan
            dataGridViewRSIDaily.DataSource = null;
            dataGridViewRSIWeekly.DataSource = null;

            dataGridViewRSIDaily.ColumnCount = 11;
            dataGridViewRSIDaily.Columns[0].Name = "ScanDate";
            dataGridViewRSIDaily.Columns[1].Name = "SymbolName";
            dataGridViewRSIDaily.Columns[2].Name = "ScanName";
            dataGridViewRSIDaily.Columns[3].Name = "ScanValue";
            dataGridViewRSIDaily.Columns[4].Name = "Signal";
            dataGridViewRSIDaily.Columns[5].Name = "date";
            dataGridViewRSIDaily.Columns[6].Name = "open";
            dataGridViewRSIDaily.Columns[7].Name = "high";
            dataGridViewRSIDaily.Columns[8].Name = "low";
            dataGridViewRSIDaily.Columns[9].Name = "close";
            dataGridViewRSIDaily.Columns[10].Name = "volume";

            dataGridViewRSIWeekly.ColumnCount = 11;
            dataGridViewRSIWeekly.Columns[0].Name = "ScanDate";
            dataGridViewRSIWeekly.Columns[1].Name = "SymbolName";
            dataGridViewRSIWeekly.Columns[2].Name = "ScanName";
            dataGridViewRSIWeekly.Columns[3].Name = "ScanValue";
            dataGridViewRSIWeekly.Columns[4].Name = "Signal";
            dataGridViewRSIWeekly.Columns[5].Name = "date";
            dataGridViewRSIWeekly.Columns[6].Name = "open";
            dataGridViewRSIWeekly.Columns[7].Name = "high";
            dataGridViewRSIWeekly.Columns[8].Name = "low";
            dataGridViewRSIWeekly.Columns[9].Name = "close";
            dataGridViewRSIWeekly.Columns[10].Name = "volume";

            dataGridViewBollinger1Weekly.ColumnCount = 11;
            dataGridViewBollinger1Weekly.Columns[0].Name = "ScanDate";
            dataGridViewBollinger1Weekly.Columns[1].Name = "SymbolName";
            dataGridViewBollinger1Weekly.Columns[2].Name = "ScanName";
            dataGridViewBollinger1Weekly.Columns[3].Name = "ScanValue";
            dataGridViewBollinger1Weekly.Columns[4].Name = "Signal";
            dataGridViewBollinger1Weekly.Columns[5].Name = "date";
            dataGridViewBollinger1Weekly.Columns[6].Name = "open";
            dataGridViewBollinger1Weekly.Columns[7].Name = "high";
            dataGridViewBollinger1Weekly.Columns[8].Name = "low";
            dataGridViewBollinger1Weekly.Columns[9].Name = "close";
            dataGridViewBollinger1Weekly.Columns[10].Name = "volume";

            dataGridViewBollinger1Daily.ColumnCount = 11;
            dataGridViewBollinger1Daily.Columns[0].Name = "ScanDate";
            dataGridViewBollinger1Daily.Columns[1].Name = "SymbolName";
            dataGridViewBollinger1Daily.Columns[2].Name = "ScanName";
            dataGridViewBollinger1Daily.Columns[3].Name = "ScanValue";
            dataGridViewBollinger1Daily.Columns[4].Name = "Signal";
            dataGridViewBollinger1Daily.Columns[5].Name = "date";
            dataGridViewBollinger1Daily.Columns[6].Name = "open";
            dataGridViewBollinger1Daily.Columns[7].Name = "high";
            dataGridViewBollinger1Daily.Columns[8].Name = "low";
            dataGridViewBollinger1Daily.Columns[9].Name = "close";
            dataGridViewBollinger1Daily.Columns[10].Name = "volume";

            dataGridViewBollinger2Daily.ColumnCount = 11;
            dataGridViewBollinger2Daily.Columns[0].Name = "ScanDate";
            dataGridViewBollinger2Daily.Columns[1].Name = "SymbolName";
            dataGridViewBollinger2Daily.Columns[2].Name = "ScanName";
            dataGridViewBollinger2Daily.Columns[3].Name = "ScanValue";
            dataGridViewBollinger2Daily.Columns[4].Name = "Signal";
            dataGridViewBollinger2Daily.Columns[5].Name = "date";
            dataGridViewBollinger2Daily.Columns[6].Name = "open";
            dataGridViewBollinger2Daily.Columns[7].Name = "high";
            dataGridViewBollinger2Daily.Columns[8].Name = "low";
            dataGridViewBollinger2Daily.Columns[9].Name = "close";
            dataGridViewBollinger2Daily.Columns[10].Name = "volume";

            dataGridViewBollinger2Weekly.ColumnCount = 11;
            dataGridViewBollinger2Weekly.Columns[0].Name = "ScanDate";
            dataGridViewBollinger2Weekly.Columns[1].Name = "SymbolName";
            dataGridViewBollinger2Weekly.Columns[2].Name = "ScanName";
            dataGridViewBollinger2Weekly.Columns[3].Name = "ScanValue";
            dataGridViewBollinger2Weekly.Columns[4].Name = "Signal";
            dataGridViewBollinger2Weekly.Columns[5].Name = "date";
            dataGridViewBollinger2Weekly.Columns[6].Name = "open";
            dataGridViewBollinger2Weekly.Columns[7].Name = "high";
            dataGridViewBollinger2Weekly.Columns[8].Name = "low";
            dataGridViewBollinger2Weekly.Columns[9].Name = "close";
            dataGridViewBollinger2Weekly.Columns[10].Name = "volume";

            dataGrid(dataGridViewMacdDaily);
            dataGrid(dataGridViewMacdWeekly);
            dataGrid(dataGridViewCandleDaily);
            dataGrid(dataGridViewCandleWeekly);
        }

        private void dataGrid(DataGridView dt)
        {
            dt.ColumnCount = 11;
            dt.Columns[0].Name = "ScanDate";
            dt.Columns[1].Name = "SymbolName";
            dt.Columns[2].Name = "ScanName";
            dt.Columns[3].Name = "ScanValue";
            dt.Columns[4].Name = "Signal";
            dt.Columns[5].Name = "date";
            dt.Columns[6].Name = "open";
            dt.Columns[7].Name = "high";
            dt.Columns[8].Name = "low";
            dt.Columns[9].Name = "close";
            dt.Columns[10].Name = "volume";
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

        //CandleStick Scan settings
        private void timerCandleStick_Tick(object sender, EventArgs e)
        {
            if (Hided16)
            {
                SPanel16.Width = SPanel16.Width + 20;
                if (SPanel16.Width >= PW11)
                {
                    timerCandleStick.Stop();
                    Hided16 = false;
                    this.Refresh();
                }
            }
            else
            {
                SPanel16.Width = SPanel16.Width - 20;
                if (SPanel16.Width <= 0)
                {
                    timerCandleStick.Stop();
                    Hided16 = true;
                    this.Refresh();
                }
            }
        }

        private void buttonLeft16_Click(object sender, EventArgs e)
        {
            if (Hided16)
            {
                buttonLeft16.Visible = false;
                buttonTop16.Visible = true;
                buttonTop16.Text = "CandleStick Settings";
            }

            timerCandleStick.Start();
        }

        private void buttonTop16_Click(object sender, EventArgs e)
        {
            if (!Hided16)
            {
                buttonLeft16.Text = "\nC\na\nn\nd\nl\ne\n \nS\ne\nt\nt\ni\nn\ng\ns";
                buttonTop16.Visible = false;
                buttonLeft16.Visible = true;
            }

            timerCandleStick.Start();
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

        #region Main chart sequence controls
        public static int Daily_Chart_Children_Index = 0;
        public static int Weekly_Chart_Children_Index = 0;
        public static int Monthly_Chart_Children_Index = 0;
        public static int Minute_Chart_Children_Index = 0;

        public static int Dow_Daily_Chart_Children_Index = 0;
        public static int Dow_Weekly_Chart_Children_Index = 0;
        public static int Dow_Monthly_Chart_Children_Index = 0;

        public static int Nasdaq_Daily_Chart_Children_Index = 0;
        public static int Nasdaq_Weekly_Chart_Children_Index = 0;
        public static int Nasdaq_Monthly_Chart_Children_Index = 0;
        #endregion

        #region Daily Tab
        OnChangeHandeller daily_Hendal = new OnChangeHandeller();
        private void priceComboBoxDailyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            daily_Hendal.Price_Name = "ddl_d_p_";
            daily_Hendal.ComboBox = priceComboBoxDailyPrice;
            daily_Hendal.OnChangeClick = priceComboBoxDailyPrice_Click;
            daily_Hendal.OverlayLayout_Price = tableLayoutPanelDailyPrice;
            daily_Hendal.Price_sender = sender;
            daily_Hendal.Price_textBox = textBoxDailyPrice;
            daily_Hendal.OnChange();
            Daily_chart();
  
        }
        private void priceComboBoxDailyPrice_Click(object sender, EventArgs e)
        {
            daily_Hendal.Price_Name = "ddl_d_p_";
            daily_Hendal.ComboBox = priceComboBoxDailyPrice;
            daily_Hendal.OnChangeClick = priceComboBoxDailyPrice_Click;
            daily_Hendal.OverlayLayout_Price = tableLayoutPanelDailyPrice;
            daily_Hendal.Price_sender = sender;
            daily_Hendal.Price_textBox = textBoxDailyPrice;
            daily_Hendal.OnChange();
            Daily_chart();
        }

        //daily stock Tab
        private void indicatorComboBoxDailyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            daily_Hendal.IndectorLayout = tableLayoutPanelDailyIndicator;
            daily_Hendal.Indector_Combobox = indicatorComboBoxDailyIndicator;
            daily_Hendal.Indector_OnChangeClick = indicatorComboBoxDailyIndicator_Click;
            daily_Hendal.Indector_Name = "ddl_d_p_";
            daily_Hendal.Indector_sender = sender;
            daily_Hendal.Indector_TextBox = textBoxDailyIndicator;
            daily_Hendal.Indector_MainLayout = tableLayoutPanelDailyMain;
            daily_Hendal.Quotes = quote_daily;
            daily_Hendal.Selected = true;
            daily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            daily_Hendal.OnIndectorChange();
            
        } 
        private void indicatorComboBoxDailyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            daily_Hendal.IndectorLayout = tableLayoutPanelDailyIndicator;
            daily_Hendal.Indector_Combobox = indicatorComboBoxDailyIndicator;
            daily_Hendal.Indector_OnChangeClick = indicatorComboBoxDailyIndicator_Click;
            daily_Hendal.Indector_Name = "ddl_d_p_";
            daily_Hendal.Indector_sender = sender;
            daily_Hendal.Indector_TextBox = textBoxDailyIndicator;
            daily_Hendal.Indector_MainLayout = tableLayoutPanelDailyMain;
            daily_Hendal.Quotes = quote_daily;
            daily_Hendal.Selected = false;
            daily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            daily_Hendal.OnIndectorChange();
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
                    mi.Overlay_key = "ddl_d_p_";
                    mi.FirstOverlay = __f_overlay(priceComboBoxDailyPrice);
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
        #endregion

        #region weekly Tab
        OnChangeHandeller weekly_Hendal = new OnChangeHandeller();
        private void priceComboBoxWeeklyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            weekly_Hendal.Price_Name = "ddl_w_p_";
            weekly_Hendal.ComboBox = priceComboBoxWeeklyPrice;
            weekly_Hendal.OnChangeClick = priceComboWeeklyPrice_Click;
            weekly_Hendal.OverlayLayout_Price = tableLayoutPanelWeeklyPrice;
            weekly_Hendal.Price_sender = sender;
            weekly_Hendal.Price_textBox = textBoxWeeklyPrice;
            weekly_Hendal.OnChange();
            WeeklyChart();
        }
        private void priceComboWeeklyPrice_Click(object sender, EventArgs e)
        {
            weekly_Hendal.Price_Name = "ddl_w_p_";
            weekly_Hendal.ComboBox = priceComboBoxWeeklyPrice;
            weekly_Hendal.OnChangeClick = priceComboWeeklyPrice_Click;
            weekly_Hendal.OverlayLayout_Price = tableLayoutPanelWeeklyPrice;
            weekly_Hendal.Price_sender = sender;
            weekly_Hendal.Price_textBox = textBoxWeeklyPrice;
            weekly_Hendal.OnChange();
            WeeklyChart();
        }

        //Weekly stock Tab
        private void indicatorComboBoxWeeklyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            weekly_Hendal.IndectorLayout = tableLayoutPanelWeeklyIndicator;
            weekly_Hendal.Indector_Combobox = indicatorComboBoxWeeklyIndicator;
            weekly_Hendal.Indector_OnChangeClick = indicatorComboBoxWeeklyIndicator_Click;
            weekly_Hendal.Indector_Name = "ddl_w_i_";
            weekly_Hendal.Indector_sender = sender;
            weekly_Hendal.Indector_TextBox = textBoxWeeklyIndicator;
            weekly_Hendal.Indector_MainLayout = tableLayoutPanelWeeklyMain;
            weekly_Hendal.Quotes = quote_weekly;
            weekly_Hendal.Selected = true;
            weekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            weekly_Hendal.OnIndectorChange();
        }
        private void indicatorComboBoxWeeklyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            weekly_Hendal.IndectorLayout = tableLayoutPanelWeeklyIndicator;
            weekly_Hendal.Indector_Combobox = indicatorComboBoxWeeklyIndicator;
            weekly_Hendal.Indector_OnChangeClick = indicatorComboBoxWeeklyIndicator_Click;
            weekly_Hendal.Indector_Name = "ddl_w_i_";
            weekly_Hendal.Indector_sender = sender;
            weekly_Hendal.Indector_TextBox = textBoxWeeklyIndicator;
            weekly_Hendal.Indector_MainLayout = tableLayoutPanelWeeklyMain;
            weekly_Hendal.Quotes = quote_weekly;
            weekly_Hendal.Selected = true;
            weekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            weekly_Hendal.OnIndectorChange();
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
                    mi.FirstOverlay = __f_overlay(priceComboBoxWeeklyPrice);
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
        OnChangeHandeller Monthly_Hendal = new OnChangeHandeller();
        private void priceComboBoxMonthlyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            Monthly_Hendal.Price_Name = "ddl_m_p_";
            Monthly_Hendal.ComboBox = priceComboBoxMonthlyPrice;
            Monthly_Hendal.OnChangeClick = priceComboBoxMonthlyPrice_Click;
            Monthly_Hendal.OverlayLayout_Price = tableLayoutPanelMonthlyPrice;
            Monthly_Hendal.Price_sender = sender;
            Monthly_Hendal.Price_textBox = textBoxMonthlyPrice;
            Monthly_Hendal.OnChange();
            MonthlyChart();
            
        }
        private void priceComboBoxMonthlyPrice_Click(object sender, EventArgs e)
        {
            Monthly_Hendal.Price_Name = "ddl_m_p_";
            Monthly_Hendal.ComboBox = priceComboBoxMonthlyPrice;
            Monthly_Hendal.OnChangeClick = priceComboBoxMonthlyPrice_Click;
            Monthly_Hendal.OverlayLayout_Price = tableLayoutPanelMonthlyPrice;
            Monthly_Hendal.Price_sender = sender;
            Monthly_Hendal.Price_textBox = textBoxMonthlyPrice;
            Monthly_Hendal.OnChange();
            MonthlyChart();
        }

        //Monthly stock Tab
        private void indicatorComboBoxMonthlyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            Monthly_Hendal.IndectorLayout = tableLayoutPanelMonthlyIndicator;
            Monthly_Hendal.Indector_Combobox = indicatorComboBoxMonthlyIndicator;
            Monthly_Hendal.Indector_OnChangeClick = indicatorComboBoxMonthlyIndicator_Click;
            Monthly_Hendal.Indector_Name = "ddl_m_i_";
            Monthly_Hendal.Indector_sender = sender;
            Monthly_Hendal.Indector_TextBox = textBoxMonthlyIndicator;
            Monthly_Hendal.Indector_MainLayout = tableLayoutPanelMonthlyMain;
            Monthly_Hendal.Quotes = quote_monthly;
            Monthly_Hendal.Selected = true;
            Monthly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            Monthly_Hendal.OnIndectorChange();
        }
        private void indicatorComboBoxMonthlyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            Monthly_Hendal.IndectorLayout = tableLayoutPanelMonthlyIndicator;
            Monthly_Hendal.Indector_Combobox = indicatorComboBoxMonthlyIndicator;
            Monthly_Hendal.Indector_OnChangeClick = indicatorComboBoxMonthlyIndicator_Click;
            Monthly_Hendal.Indector_Name = "ddl_m_i_";
            Monthly_Hendal.Indector_sender = sender;
            Monthly_Hendal.Indector_TextBox = textBoxMonthlyIndicator;
            Monthly_Hendal.Indector_MainLayout = tableLayoutPanelMonthlyMain;
            Monthly_Hendal.Quotes = quote_monthly;
            Monthly_Hendal.Selected = true;
            Monthly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            Monthly_Hendal.OnIndectorChange();          
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
                    mi.FirstOverlay = __f_overlay(priceComboBoxMonthlyPrice);
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
        OnChangeHandeller Minute_Hendal = new OnChangeHandeller();
        private void priceComboBoxMinutePrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            Minute_Hendal.Price_Name = "ddl_min_p_";
            Minute_Hendal.ComboBox = priceComboBoxMinutePrice;
            Minute_Hendal.OnChangeClick = priceComboBoxMinutePrice_Click;
            Minute_Hendal.OverlayLayout_Price = tableLayoutPanelMinutePrice;
            Minute_Hendal.Price_sender = sender;
            Minute_Hendal.Price_textBox = textBoxMinutePrice;
            Minute_Hendal.OnChange();
            MinuteChart();

            
        }
        private void priceComboBoxMinutePrice_Click(object sender, EventArgs e)
        {
            //this 2 is the column count
            Minute_Hendal.Price_Name = "ddl_min_p_";
            Minute_Hendal.ComboBox = priceComboBoxMinutePrice;
            Minute_Hendal.OnChangeClick = priceComboBoxMinutePrice_Click;
            Minute_Hendal.OverlayLayout_Price = tableLayoutPanelMinutePrice;
            Minute_Hendal.Price_sender = sender;
            Minute_Hendal.Price_textBox = textBoxMinutePrice;
            Minute_Hendal.OnChange();
            MinuteChart();
            //show comments for developer only
            //MessageBox.Show("Add this price indicator into Left side -> Top Chart!");
        }
        //Minutes stock indicator Tab
        private void indicatorComboBoxMinuteIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            Minute_Hendal.IndectorLayout = tableLayoutPanelMinuteIndicator;
            Minute_Hendal.Indector_Combobox = indicatorComboBoxMinuteIndicator;
            Minute_Hendal.Indector_OnChangeClick = indicatorComboBoxMinuteIndicator_Click;
            Minute_Hendal.Indector_Name = "ddl_min_i_";
            Minute_Hendal.Indector_sender = sender;
            Minute_Hendal.Indector_TextBox = textBoxMinuteIndicator;
            Minute_Hendal.Indector_MainLayout = tableLayoutPanelMinuteMain;
            Minute_Hendal.Quotes = quote_min;
            Minute_Hendal.Selected = true;
            Minute_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            Minute_Hendal.OnIndectorChange();
        }   

        private void indicatorComboBoxMinuteIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            Minute_Hendal.IndectorLayout = tableLayoutPanelMinuteIndicator;
            Minute_Hendal.Indector_Combobox = indicatorComboBoxMinuteIndicator;
            Minute_Hendal.Indector_OnChangeClick = indicatorComboBoxMinuteIndicator_Click;
            Minute_Hendal.Indector_Name = "ddl_min_i_";
            Minute_Hendal.Indector_sender = sender;
            Minute_Hendal.Indector_TextBox = textBoxMinuteIndicator;
            Minute_Hendal.Indector_MainLayout = tableLayoutPanelMinuteMain;
            Minute_Hendal.Quotes = quote_min;
            Minute_Hendal.Selected = true;
            Minute_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            Minute_Hendal.OnIndectorChange();
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
                    mi.FirstOverlay = __f_overlay(priceComboBoxMinutePrice);
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
                mi.FirstOverlay = __f_overlay(priceComboBoxDowDailyPrice);
                string Selected = comboBoxDowDailyChart.SelectedItem.ToString();
                quote_dow_daily.Clear();
                quote_dow_daily.AddRange(mi.Quotes());
                mi.Chart(Selected, zedGraphControlDowDailyTop, quote_dow_daily);
            }
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
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowDaily_Hendal.IndectorLayout = tableLayoutPanelDowDailyIndicator;
            DowDaily_Hendal.Indector_Combobox = indicatorComboBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxDowDailyIndicator_Click;
            DowDaily_Hendal.Indector_Name = "ddl_dow_day_i_";
            DowDaily_Hendal.Indector_sender = sender;
            DowDaily_Hendal.Indector_TextBox = textBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_MainLayout = tableLayoutPanelDowDailyMain;
            DowDaily_Hendal.Quotes = quote_dow_daily;
            DowDaily_Hendal.Selected = true;
            DowDaily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowDaily_Hendal.OnIndectorChange();
        }

        private void indicatorComboBoxDowDailyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowDaily_Hendal.IndectorLayout = tableLayoutPanelDowDailyIndicator;
            DowDaily_Hendal.Indector_Combobox = indicatorComboBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxDowDailyIndicator_Click;
            DowDaily_Hendal.Indector_Name = "ddl_dow_day_i_";
            DowDaily_Hendal.Indector_sender = sender;
            DowDaily_Hendal.Indector_TextBox = textBoxDowDailyIndicator;
            DowDaily_Hendal.Indector_MainLayout = tableLayoutPanelDowDailyMain;
            DowDaily_Hendal.Quotes = quote_dow_daily;
            DowDaily_Hendal.Selected = true;//double check?
            DowDaily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowDaily_Hendal.OnIndectorChange();
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
                mi.FirstOverlay = __f_overlay(priceComboBoxDowWeeklyPrice);
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
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowWeekly_Hendal.IndectorLayout = tableLayoutPanelDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_Combobox = indicatorComboBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowWeeklyIndicator_Click;
            DowWeekly_Hendal.Indector_Name = "ddl_dow_week_i_" ;
            DowWeekly_Hendal.Indector_sender = sender;
            DowWeekly_Hendal.Indector_TextBox = textBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_MainLayout = tableLayoutPanelDowWeeklyMain;
            DowWeekly_Hendal.Quotes = quote_dow_weekly;
            DowWeekly_Hendal.Selected = true;
            DowWeekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowWeekly_Hendal.OnIndectorChange();

        }
        private void indicatorComboBoxDowWeeklyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowWeekly_Hendal.IndectorLayout = tableLayoutPanelDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_Combobox = indicatorComboBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowWeeklyIndicator_Click;
            DowWeekly_Hendal.Indector_Name = "ddl_dow_week_i_";
            DowWeekly_Hendal.Indector_sender = sender;
            DowWeekly_Hendal.Indector_TextBox = textBoxDowWeeklyIndicator;
            DowWeekly_Hendal.Indector_MainLayout = tableLayoutPanelDowWeeklyMain;
            DowWeekly_Hendal.Quotes = quote_dow_weekly;
            DowWeekly_Hendal.Selected = true; //overlay onto top price chart
            DowWeekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowWeekly_Hendal.OnIndectorChange();
        }
        #endregion

        #region DowMonthly
        OnChangeHandeller DowMonthly_Hendal = new OnChangeHandeller();
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
                mi.FirstOverlay = __f_overlay(priceComboBoxDowMonthlyPrice);
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

        private void priceComboBoxDowMonthlyPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            DowMonthly_Hendal.Price_Name = "ddl_dow_month_p_";
            DowMonthly_Hendal.ComboBox = priceComboBoxDowMonthlyPrice;
            DowMonthly_Hendal.OnChangeClick = priceComboBoxDowMonthlyPrice_Click;
            DowMonthly_Hendal.OverlayLayout_Price = tableLayoutPanelDowMonthlyPrice;
            DowMonthly_Hendal.Price_sender = sender;
            DowMonthly_Hendal.Price_textBox = textBoxDowMonthlyPrice;
            DowMonthly_Hendal.OnChange();
            ////this 2 is the column count
            //int childIndex = tableLayoutPanelDowMonthlyPrice.ColumnCount + tableLayoutPanelDowMonthlyPrice.Controls.GetChildIndex((Control)sender);
            //if (ddl_dow_month_p_ < childIndex || ddl_dow_month_p_ == 0)
            //{
            //    ddl_dow_month_p_ = childIndex;
            //    PriceComboBox pCombo = new PriceComboBox();
            //    pCombo.Size = priceComboBoxDowMonthlyPrice.Size;
            //    pCombo.SelectedIndexChanged += new EventHandler(priceComboBoxDowMonthlyPrice_Click);
            //    pCombo.Dock = DockStyle.Fill;
            //    pCombo.Name = "ddl_dow_month_p_" + childIndex;
            //    TextBox pText = new TextBox();
            //    pText.Size = textBoxDowMonthlyPrice.Size;
            //    pText.Dock = DockStyle.Fill;

            //    //
            //    //add row style here
            //    //
            //    tableLayoutPanelDowMonthlyPrice.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
            //    tableLayoutPanelDowMonthlyPrice.Controls.Add(pCombo);
            //    tableLayoutPanelDowMonthlyPrice.Controls.Add(pText);

            //    //move to the right location
            //    tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pCombo, childIndex);
            //    tableLayoutPanelDowMonthlyPrice.Controls.SetChildIndex(pText, childIndex + 1);

            //    //DowMonthlyChart();
            //    //show comments for developer only
            //    // MessageBox.Show("Add this price indicator into Left side->Top Price Chart!");
            //}
            DowMonthlyChat();
        }

        private void priceComboBoxDowMonthlyPrice_Click(object sender, EventArgs e)
        {
            DowMonthly_Hendal.Price_Name = "ddl_dow_month_p_";
            DowMonthly_Hendal.ComboBox = priceComboBoxDowMonthlyPrice;
            DowMonthly_Hendal.OnChangeClick = priceComboBoxDowMonthlyPrice_Click;
            DowMonthly_Hendal.OverlayLayout_Price = tableLayoutPanelDowMonthlyPrice;
            DowMonthly_Hendal.Price_sender = sender;
            DowMonthly_Hendal.Price_textBox = textBoxDowMonthlyPrice;
            DowMonthly_Hendal.OnChange();
            DowMonthlyChat();
            //show comments for developer only
            //MessageBox.Show("Add this price indicator into Left side -> Top Chart!");
        }

        //Dow Monthly Indicator Tab
        private void indicatorComboBoxDowMonthlyIndicator_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowMonthly_Hendal.IndectorLayout = tableLayoutPanelDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_Combobox = indicatorComboBoxDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowMonthlyIndicator_Click;
            DowMonthly_Hendal.Indector_Name = "ddl_dow_month_i_";
            DowMonthly_Hendal.Indector_sender = sender;
            DowMonthly_Hendal.Indector_TextBox = textBoxDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_MainLayout = tableLayoutPanelDowMonthlyMain;
            DowMonthly_Hendal.Quotes = quote_dow_monthly;
            DowMonthly_Hendal.Selected = true;
            DowMonthly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowMonthly_Hendal.OnIndectorChange();          
        }

        private void indicatorComboBoxDowMonthlyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            DowMonthly_Hendal.IndectorLayout = tableLayoutPanelDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_Combobox = indicatorComboBoxDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_OnChangeClick = indicatorComboBoxDowMonthlyIndicator_Click;
            DowMonthly_Hendal.Indector_Name = "ddl_dow_month_i_";
            DowMonthly_Hendal.Indector_sender = sender;
            DowMonthly_Hendal.Indector_TextBox = textBoxDowMonthlyIndicator;
            DowMonthly_Hendal.Indector_MainLayout = tableLayoutPanelDowMonthlyMain;
            DowMonthly_Hendal.Quotes = quote_dow_monthly;
            DowMonthly_Hendal.Selected = true;
            DowMonthly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            DowMonthly_Hendal.OnIndectorChange();
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
                mi.FirstOverlay = __f_overlay(priceComboBoxNasdaqDailyPrice);
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
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaDaily_Hendal.IndectorLayout = tableLayoutPanelNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_Combobox = indicatorComboBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqDailyIndicator_Click;
            NasdaDaily_Hendal.Indector_Name = "ddl_nasdaq_day_i_";
            NasdaDaily_Hendal.Indector_sender = sender;
            NasdaDaily_Hendal.Indector_TextBox = textBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqDailyMain;
            NasdaDaily_Hendal.Quotes = quote_nasdaq_daily;
            NasdaDaily_Hendal.Selected = true;
            NasdaDaily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            NasdaDaily_Hendal.OnIndectorChange();
        }

        private void indicatorComboBoxNasdaqDailyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaDaily_Hendal.IndectorLayout = tableLayoutPanelNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_Combobox = indicatorComboBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqDailyIndicator_Click;
            NasdaDaily_Hendal.Indector_Name = "ddl_nasdaq_day_i_";
            NasdaDaily_Hendal.Indector_sender = sender;
            NasdaDaily_Hendal.Indector_TextBox = textBoxNasdaqDailyIndicator;
            NasdaDaily_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqDailyMain;
            NasdaDaily_Hendal.Quotes = quote_nasdaq_daily;
            NasdaDaily_Hendal.Selected = true;
            NasdaDaily_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
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
                mi.FirstOverlay = __f_overlay(priceComboBoxNasdaqWeeklyPrice);
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
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaqWeekly_Hendal.IndectorLayout = tableLayoutPanelNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_Combobox = indicatorComboBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqWeeklyIndicator_Click;
            NasdaqWeekly_Hendal.Indector_Name = "ddl_nasdaq_week_i_";
            NasdaqWeekly_Hendal.Indector_sender = sender;
            NasdaqWeekly_Hendal.Indector_TextBox = textBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqWeeklyMain;
            NasdaqWeekly_Hendal.Quotes = quote_nasdaq_weekly;
            NasdaqWeekly_Hendal.Selected = true;
            NasdaqWeekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            NasdaqWeekly_Hendal.OnIndectorChange();
        
        }
        private void indicatorComboBoxNasdaqWeeklyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaqWeekly_Hendal.IndectorLayout = tableLayoutPanelNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_Combobox = indicatorComboBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqWeeklyIndicator_Click;
            NasdaqWeekly_Hendal.Indector_Name = "ddl_nasdaq_week_i_";
            NasdaqWeekly_Hendal.Indector_sender = sender;
            NasdaqWeekly_Hendal.Indector_TextBox = textBoxNasdaqWeeklyIndicator;
            NasdaqWeekly_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqWeeklyMain;
            NasdaqWeekly_Hendal.Quotes = quote_nasdaq_weekly;
            NasdaqWeekly_Hendal.Selected = true;
            NasdaqWeekly_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            NasdaqWeekly_Hendal.OnIndectorChange();
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
                mi.FirstOverlay = __f_overlay(priceComboBoxNasdaqMonthlyPrice);
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
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaqMonth_Hendal.IndectorLayout = tableLayoutPanelNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_Combobox = indicatorComboBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqMonthlyIndicator_Click;
            NasdaqMonth_Hendal.Indector_Name = "ddl_nasdaq_month_i_";
            NasdaqMonth_Hendal.Indector_sender = sender;
            NasdaqMonth_Hendal.Indector_TextBox = textBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqMonthlyMain;
            NasdaqMonth_Hendal.Quotes = quote_nasdaq_monthly;
            NasdaqMonth_Hendal.Selected = true;
            NasdaqMonth_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            NasdaqMonth_Hendal.OnIndectorChange();
        }

        private void indicatorComboBoxNasdaqMonthlyIndicator_Click(object sender, EventArgs e)
        {
            IndicatorComboBox currentIndicatorComboBox = (IndicatorComboBox)sender;
            NasdaqMonth_Hendal.IndectorLayout = tableLayoutPanelNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_Combobox = indicatorComboBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_OnChangeClick = indicatorComboBoxNasdaqMonthlyIndicator_Click;
            NasdaqMonth_Hendal.Indector_Name = "ddl_nasdaq_month_i_";
            NasdaqMonth_Hendal.Indector_sender = sender;
            NasdaqMonth_Hendal.Indector_TextBox = textBoxNasdaqMonthlyIndicator;
            NasdaqMonth_Hendal.Indector_MainLayout = tableLayoutPanelNasdaqMonthlyMain;
            NasdaqMonth_Hendal.Quotes = quote_nasdaq_monthly;
            NasdaqMonth_Hendal.Selected = true;
            NasdaqMonth_Hendal.SelectedIteam = currentIndicatorComboBox.SelectedItem.ToString();
            NasdaqMonth_Hendal.OnIndectorChange();
        }
        #endregion

        private void buttonRSIScanDaily_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();            
            int i = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownRSIDaily.Value);
            List<Quote> q = new List<Quote>();
            string table = "daily";
            foreach (string s in lo.nodes())
            {
                i++;
                mi.FolderName = "Daily";
                mi.FileName = s + "_daily.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "RSI scan daily:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;
                
                IEnumerable<RsiResult> _RsiResults = q.GetRsi();
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                int n = RsiResults.Count();
                int m = CciResults.Count();
                int o = StochResults.Count();

                for (int j = 0; j <= lookbackwardPeriod-1; j++)
                {
                    if (n > 0 && n - 1 - j < 0) continue;

                    if (  n > 0 && RsiResults[n -1 - j].Rsi < 31)
                    {
                        if (CciResults[m -1 - j].Cci < -95 && (StochResults[o -1- j].K < 20 || StochResults[o -1 - j].D < 20))
                        {
                            int q_index = q.Count - 1 - j;

                            db.insert(new ScanResult() {
                                SymbolName=s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open  = Convert.ToDouble(q[q_index].Open),
                                date  = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low  = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = "OverSold",
                                ScanName ="RSI Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = { 
                                DateTime.Now.ToString(),
                                s,
                                "RSI Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            
                            dataGridViewRSIDaily.Rows.Add(newRow);

                        }
                    }
                    else if (n > 0 && RsiResults[n -1- j].Rsi > 65 )
                    {
                        if (CciResults[m -1 - j].Cci > 95 && (StochResults[o -1- j].K >= 80 || StochResults[o -1- j].D >= 80))
                        {
                            int q_index = q.Count - 1 - j;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = "OverBought",
                                ScanName = "RSI Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "RSI Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewRSIDaily.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            //dataGridViewRSIDaily.DataSource = db.view(table);

            //MessageBox.Show("RSI Daily Scan is completed!");
        }
 
        private void buttonRSIWeekly_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int i = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownRSIDaily.Value);
            List<Quote> q = new List<Quote>();
            string table = "weekly";
            foreach (string s in lo.nodes())
            {
                i++;
                mi.FolderName = "Weekly";
                mi.FileName = s + "_weekly.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "RSI Scan Weekly:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi();
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                int n = RsiResults.Count();
                int m = CciResults.Count();
                int o = StochResults.Count();

                for (int j = 0; j <= lookbackwardPeriod-1; j++)
                {
                    if (n > 0 && n - 1 - j < 0) continue;

                    if (n > 0 && RsiResults[n - 1 - j].Rsi < 31)
                    {
                        if (CciResults[m - 1 - j].Cci < -95 && (StochResults[o - 1 - j].K < 20 || StochResults[o - 1 - j].D < 20))
                        {
                            int q_index = q.Count - 1 - j;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = "RSI Scan Weekly",
                                ScanName = "OverSold",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            //dataGridViewRSIDaily.DataSource = db.view("daily");
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "RSI Scan Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewRSIWeekly.Rows.Add(newRow);
                            
                        }
                    }
                    else if (n > 0 && RsiResults[n - 1 - j].Rsi > 65)
                    {
                        if (CciResults[m - 1 - j].Cci > 95 && (StochResults[o - 1 - j].K >= 80 || StochResults[o - 1 - j].D >= 80))
                        {
                            int q_index = q.Count - 1 - j;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[j].Close),
                                open = Convert.ToDouble(q[j].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[j].High),
                                low = Convert.ToDouble(q[j].Low),
                                volume = Convert.ToDouble(q[j].Volume),
                                Signal = "RSI Scan Weekly",
                                ScanName = "OverBought",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "RSI Scan Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewRSIWeekly.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //dataGridViewRSIWeekly.DataSource = db.view(table);
            //MessageBox.Show("RSI Weekly Scan is completed!");
            
        }

        private void contextMenuStripRSIDaily_Opening(object sender, CancelEventArgs e)
        {

        }

        private void exportToAcsvFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }
        private string OpenSavefileDialog(string file = "")
        {
            string Filename = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Excel|*.xls";
            saveFileDialog.Filter = file!=""? "pdf File|*.pdf" : "csv File|*.csv";
            saveFileDialog.Title = "Save Report";
            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                Filename = saveFileDialog.FileName;

            }

            return Filename;
        }
        public static void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        private void exportToAPDFFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }
        private void exportToAcsvFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }
        public void ExportToPdf(DataTable dt, string strFilePath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(strFilePath, FileMode.Create));
            document.Open();
            iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            PdfPRow row = null;
            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            int iCol = 0;
            string colname = "";
            PdfPCell cell = new PdfPCell(new Phrase("Products"));

            cell.Colspan = dt.Columns.Count;

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new Phrase(c.ColumnName, font5));
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int h = 0; h < dt.Columns.Count; h++)
                    {
                        table.AddCell(new Phrase(r[h].ToString(), font5));
                    }
                }
            }
            document.Add(table);
            document.Close();
        }
        private void exportToAPDFFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }

        private void contextMenuStripRSIWeekly_Opening(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Export to PDF File! 4532423423");
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
           // MessageBox.Show("Export to PDF File! 123123455");
        }

        private void buttonBollinger1Daily_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownBollinger2Daily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "bollinger_daily";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Daily";
                mi.FileName = s + "_daily.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Bollinger scan daily:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                //IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                //StochResult[] StochResults = _StochResults.ToArray();
                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if (q[n - 1 - i].Low <= BBresults[n - 1 - i].LowerBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi < 35 && CciResults[n - 1 - i].Cci < -95)
                        {
                            string signal = "";
                            if (Cmfresults[n - 1 - i].Cmf >= 0) signal = "Low+";
                            if (Cmfresults[n - 1 - i].Cmf < 0) signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewBollinger1Daily.Rows.Add(newRow);

                        }
                    }
                    else if (q[n - 1 - i].High >= BBresults[n - 1 - i].UpperBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi >= 60 && CciResults[n - 1 - i].Cci > 95)
                        {
                            string signal = "";
                            if (Cmfresults[n - 1 - i].Cmf > 0) signal = "High";
                            if (Cmfresults[n - 1 - i].Cmf <= 0) signal = "High+";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewBollinger1Daily.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        //this is the Bollinger Scan 1 -weekly button click event
        //sorry I did not update the button name timely
        private void button1_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownBollinger2Daily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "bollinger_Weekly";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Weekly";
                mi.FileName = s + "_Weekly.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Bollinger scan Weekly:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                //IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                //StochResult[] StochResults = _StochResults.ToArray();
                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if (q[n - 1 - i].Low <= BBresults[n - 1 - i].LowerBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi < 35 && CciResults[n - 1 - i].Cci < -95)
                        {
                            string signal = "";
                            if (Cmfresults[n - 1 - i].Cmf >= 0) signal = "Low+";
                            if (Cmfresults[n - 1 - i].Cmf < 0) signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewBollinger1Weekly.Rows.Add(newRow);

                        }
                    }
                    else if (q[n - 1 - i].High >= BBresults[n - 1 - i].UpperBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi >= 60 && CciResults[n - 1 - i].Cci > 95)
                        {
                            string signal = "";
                            if (Cmfresults[n - 1 - i].Cmf > 0) signal = "High";
                            if (Cmfresults[n - 1 - i].Cmf <= 0) signal = "High+";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewBollinger1Weekly.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            //dataGridViewRSIDaily.DataSource = db.view(table);

            //MessageBox.Show("RSI Daily Scan is completed!");
        }

        private void contextMenuStripBollinger1Daily_Opening(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Export to PDF File! 2342342");
        }

        private void contextMenuStripBollinger1Weekly_Opening(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("Export to PDF File! 234234");
        }

 

        private void buttonBollinger2Daily_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownBollinger2Daily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "bollinger_daily2";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Daily";
                mi.FileName = s + "_daily.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Bollinger scan daily 2:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if (q[n - 1 - i].Low <= BBresults[n - 1 - i].LowerBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi < 35 && CciResults[n - 1 - i].Cci < -100 && (StochResults[n-1-i].K <20 || StochResults[n-1-i].D <20))
                        {
                            string signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Daily 2",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewBollinger2Daily.Rows.Add(newRow);

                        }
                    }
                    else if (q[n - 1 - i].High >= BBresults[n - 1 - i].UpperBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi >= 60 && CciResults[n - 1 - i].Cci > 100 && (StochResults[n-1-i].K >= 80 || StochResults[n-1-i].D >= 80))
                        {
                            string signal = "High";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Daily 2",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Daily 2",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewBollinger2Daily.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private void buttonBollinger2Weekly_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownBollinger2Daily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "bollinger_weekly2";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Weekly";
                mi.FileName = s + "_weekly.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Bollinger scan weekly 2:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if (q[n - 1 - i].Low <= BBresults[n - 1 - i].LowerBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi < 35 && CciResults[n - 1 - i].Cci < -100 && (StochResults[n - 1 - i].K < 20 || StochResults[n - 1 - i].D < 20))
                        {
                            string signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Weekly 2",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewBollinger2Weekly.Rows.Add(newRow);

                        }
                    }
                    else if (q[n - 1 - i].High >= BBresults[n - 1 - i].UpperBand)
                    {
                        if (RsiResults[n - 1 - i].Rsi >= 60 && CciResults[n - 1 - i].Cci > 100 && (StochResults[n - 1 - i].K >= 80 || StochResults[n - 1 - i].D >= 80))
                        {
                            string signal = "High";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Bollinger Weekly 2",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Bollinger Weekly 2",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewBollinger2Weekly.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private void exportToCsvFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_daily2"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToPDFFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_daily2"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }

        private void contextMenuStripBollinger2Weekly_Opening(object sender, CancelEventArgs e)
        {
            //no need
            //MessageBox.Show("Export to PDF File 2342!");
        }

        private void exportToCsvFileToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_Weekly2"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToPDFFileToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_Weekly2"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }
        private void exportToPDFFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_Weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }
        private void exportToCsvFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_Weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToCsvFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToPDFFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("bollinger_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }

        //MACD Scan
        private void buttonMacdDaily_Click(object sender, EventArgs e)
        {

            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownMacdDaily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "macd_daily";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Daily";
                mi.FileName = s + "_daily.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "MACD Scan Daily:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<MacdResult> _MacdResults = q.GetMacd();
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                MacdResult[] MacdResults = _MacdResults.ToArray();

                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {

             
                    if ((RsiResults[n - 1 - i].Rsi < 35) && (CciResults[n - 1 - i].Cci < -95))
                    {
                        if (MacdResults[n - 1 - i].Histogram >= 0 && MacdResults[n - 2 - i].Histogram < 0)
                        {
                            string signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "MACD Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "MACD Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewMacdDaily.Rows.Add(newRow);

                        }
                    }
                    else if ((RsiResults[n - 1 - i].Rsi >= 60) && (CciResults[n - 1 - i].Cci > 95))
                    {
                        if (MacdResults[n - 1 - i].Histogram <= 0 && MacdResults[n - 2 - i].Histogram > 0)
                        {
                            string signal = "High";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "MACD Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "MACD Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewMacdDaily.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }

        private void buttonMacdWeekly_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownMacdDaily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "macd_weekly";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Weekly";
                mi.FileName = s + "_weekly.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "MACD Scan Weekly:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<MacdResult> _MacdResults = q.GetMacd();
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                MacdResult[] MacdResults = _MacdResults.ToArray();

                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {


                    if ((RsiResults[n - 1 - i].Rsi < 35) && (CciResults[n - 1 - i].Cci < -95))
                    {
                        if (MacdResults[n - 1 - i].Histogram >= 0 && MacdResults[n - 2 - i].Histogram < 0)
                        {
                            string signal = "Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "MACD Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "MACD Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewMacdWeekly.Rows.Add(newRow);

                        }
                    }
                    else if ((RsiResults[n - 1 - i].Rsi >= 60) && (CciResults[n - 1 - i].Cci > 95))
                    {
                        if (MacdResults[n - 1 - i].Histogram <= 0 && MacdResults[n - 2 - i].Histogram > 0)
                        {
                            string signal = "High";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "MACD Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "MACD Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewMacdWeekly.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }

        //CandleStick Scan
        private void buttonCandleDaily_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownCandleDaily.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "candle_daily";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Daily";
                mi.FileName = s + "_daily.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Candle Scan Daily:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<MacdResult> _MacdResults = q.GetMacd();
                IEnumerable<CandleResult> _CandleResult = q.GetDoji(0.5);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<StochResult> _StochResult = q.GetStoch();

                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                MacdResult[] MacdResults = _MacdResults.ToArray();
                CandleResult[] CandleResult = _CandleResult.ToArray();
                StochResult[] StochResult = _StochResult.ToArray();

                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if ((RsiResults[n - 1 - i].Rsi < 35) && (CciResults[n - 1 - i].Cci < -100) && (StochResult[n - 1].D < 20 && StochResult[n - 1].K < 20))
                    {
                        if (CandleResult[n - 1 - i].Signal == Signal.Neutral)
                        {
                            string signal = "Doji-Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Doji Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Doji Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewCandleWeekly.Rows.Add(newRow);

                        }
                    }
                    else if ((RsiResults[n - 1 - i].Rsi >= 60) && (CciResults[n - 1 - i].Cci > 100) && (StochResult[n - 1].K >80 && StochResult[n - 1].D > 80) )
                    {
                        if (CandleResult[n - 1 - i].Signal == Signal.Neutral)
                        {
                            string signal = "Doji-high";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Doji Daily",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Doji Daily",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewCandleWeekly.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

        }

        private void buttonCandleWeekly_Click(object sender, EventArgs e)
        {
            MainChart mi = new MainChart();
            int j = 0;
            int lookbackwardPeriod = Convert.ToInt32(numericUpDownCandleWeekly.Value);
            int standardDeviations = 2;
            List<Quote> q = new List<Quote>();
            string table = "candle_weekly";
            foreach (string s in lo.nodes())
            {
                j++;
                mi.FolderName = "Weekly";
                mi.FileName = s + "_weekly.csv";
                q.Clear();
                q = mi.Quotes();

                //display these info on status bar
                toolStripStatusLabel1.Text = "Candle Scan Weekly:";
                toolStripStatusLabel2.Text = "Symbol:" + s;
                toolStripStatusLabel3.Text = "Data Record count:" + q.Count.ToString();

                //if no data record for this symbol then continue
                if (q.Count == 0) continue;

                IEnumerable<RsiResult> _RsiResults = q.GetRsi(lookbackwardPeriod);
                IEnumerable<MacdResult> _MacdResults = q.GetMacd();
                IEnumerable<CandleResult> _CandleResult = q.GetDoji(0.5);
                IEnumerable<BollingerBandsResult> _BBresults = q.GetBollingerBands(lookbackwardPeriod, standardDeviations);
                IEnumerable<StochResult> _StochResult = q.GetStoch();

                IEnumerable<CmfResult> _Cmfresults = q.GetCmf(lookbackwardPeriod);
                IEnumerable<CciResult> _CciResults = q.GetCci();
                IEnumerable<StochResult> _StochResults = q.GetStoch();
                RsiResult[] RsiResults = _RsiResults.ToArray();
                CciResult[] CciResults = _CciResults.ToArray();
                BollingerBandsResult[] BBresults = _BBresults.ToArray();
                CmfResult[] Cmfresults = _Cmfresults.ToArray();
                StochResult[] StochResults = _StochResults.ToArray();
                MacdResult[] MacdResults = _MacdResults.ToArray();
                CandleResult[] CandleResult = _CandleResult.ToArray();
                StochResult[] StochResult = _StochResult.ToArray();

                int n = q.Count();
                //int m = CciResults.Count();
                //int o = StochResults.Count();

                for (int i = 0; i <= lookbackwardPeriod - 1; i++)
                {
                    if ((RsiResults[n - 1 - i].Rsi < 35) && (CciResults[n - 1 - i].Cci < -100) && (StochResult[n - 1].D < 20 && StochResult[n - 1].K < 20))
                    {
                        if (CandleResult[n - 1 - i].Signal == Signal.Neutral)
                        {
                            string signal = "Doji-Low";
                            int q_index = q.Count - 1 - i;
                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[q_index].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Doji Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);

                            //add new row logic below:
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Doji Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverSold",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};

                            dataGridViewCandleWeekly.Rows.Add(newRow);

                        }
                    }
                    else if ((RsiResults[n - 1 - i].Rsi >= 60) && (CciResults[n - 1 - i].Cci > 100) && (StochResult[n - 1].K > 80 && StochResult[n - 1].D > 80))
                    {
                        if (CandleResult[n - 1 - i].Signal == Signal.Neutral)
                        {
                            string signal = "Doji-high";
                            int q_index = q.Count - 1 - i;

                            db.insert(new ScanResult()
                            {
                                SymbolName = s,
                                close = Convert.ToDouble(q[q_index].Close),
                                open = Convert.ToDouble(q[q_index].Open),
                                date = q[j].Date,
                                ScanDate = DateTime.Now,
                                high = Convert.ToDouble(q[q_index].High),
                                low = Convert.ToDouble(q[q_index].Low),
                                volume = Convert.ToDouble(q[q_index].Volume),
                                Signal = signal,
                                ScanName = "Doji Weekly",
                                ScanValue = (double)RsiResults[n - 1 - j].Rsi,
                            }, table);
                            string[] newRow = {
                                DateTime.Now.ToString(),
                                s,
                                "Doji Weekly",
                                RsiResults[n - 1 - j].Rsi.ToString(),
                                "OverBought",
                                q[q_index].Date.ToString(),
                                q[q_index].Open.ToString(),
                                q[q_index].High.ToString(),
                                q[q_index].Low.ToString(),
                                q[q_index].Close.ToString(),
                                q[q_index].Volume.ToString()};
                            dataGridViewCandleWeekly.Rows.Add(newRow);

                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        //zScan Scan
        private void buttonZscanDaily_Click(object sender, EventArgs e)
        {

        }

        private void buttonZscanWeekly_Click(object sender, EventArgs e)
        {

        }

        private void exportCSVFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("macd_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());

        }

        private void exportToPDFFileToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("macd_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }

        private void contextMenuStripMacdWeekly_Opening(object sender, CancelEventArgs e)
        {

        }

        private void exportCSVFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("candle_weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());


        }

        private void exportToPDFFileToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("macd_weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }

        private void exportToCSVFileToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //daily
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("candle_weekly"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToPDFFileToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //daily 
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("candle_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));

        }

        private void exportCSVFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("candle_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ToCSV(table, OpenSavefileDialog());
        }

        private void exportToPDFFileToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("ScanDate", typeof(DateTime));
            table.Columns.Add("SymbolName", typeof(string));
            table.Columns.Add("ScanName", typeof(string));
            table.Columns.Add("ScanValue", typeof(string));
            table.Columns.Add("Signal", typeof(string));
            table.Columns.Add("date", typeof(string));
            table.Columns.Add("open", typeof(string));
            table.Columns.Add("high", typeof(string));
            table.Columns.Add("low", typeof(string));
            table.Columns.Add("close", typeof(string));
            table.Columns.Add("volume", typeof(string));

            foreach (ScanResult s in db.view("candle_daily"))
                table.Rows.Add(s.Id, s.ScanDate, s.SymbolName, s.ScanName, s.ScanValue, s.Signal, s.date, s.open, s.high, s.low, s.close, s.volume);

            ExportToPdf(table, OpenSavefileDialog("pdf"));
        }
    }

   

}
