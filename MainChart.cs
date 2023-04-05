using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using static zCharts.MainForm;

namespace zCharts
{
    public class MainChart
    {
        #region Default Section
        public void Default_Candlestick_Chart(ZedGraphControl zgc)
        {
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "Candlestick Chart";
            myPane.XAxis.Title.Text = "Date";
            myPane.YAxis.Title.Text = "Price";

            StockPointList spl = new StockPointList();
            Random rand = new Random();

            // First day is jan 1st
            XDate xDate = new XDate(2006, 8, 1);
            double open = 120.0;

            for (int i = 0; i < 0; i++)
            {
                double x = xDate.XLDate;
                double close = open + rand.NextDouble() * 10.0 - 5.0;
                double hi = Math.Max(open, close) + rand.NextDouble() * 5.0;
                double low = Math.Min(open, close) - rand.NextDouble() * 5.0;

                StockPt pt = new StockPt(x, hi, low, open, close, 100000);
                spl.Add(pt);

                open = close;
                // Advance one day
                xDate.AddDays(1.0);
                // but skip the weekends
                // if (XDate.XLDateToDayOfWeek(xDate.XLDate) == 6)
                // xDate.AddDays(3.0);
            }

            JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick("", spl);
            myCurve.Stick.IsAutoSize = true;
            myCurve.Stick.Color = Color.Blue;

            // Use DateAsOrdinal to skip weekend gaps
            myPane.XAxis.Type = AxisType.DateAsOrdinal;

            // pretty it up a little
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);

            //always put this line here for refresh axis scale
            zgc.AxisChange();
            zgc.Refresh();
        }
        public void Default_OHLC_Chart(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "OHLC BAR";
            myPane.XAxis.Title.Text = "Date";
            myPane.YAxis.Title.Text = "Price";

            StockPointList spl = new StockPointList();
            Random rand = new Random();

            // First day is jan 1st
            XDate xDate = new XDate(2021, 1, 1);
            double open = 250.0;

            for (int i = 0; i < 0; i++)
            {
                double x = xDate.XLDate;
                double close = open + rand.NextDouble() * 10.0 - 5.0;
                double hi = Math.Max(open, close) + rand.NextDouble() * 5.0;
                double low = Math.Min(open, close) - rand.NextDouble() * 5.0;

                StockPt pt = new StockPt(x, hi, low, open, close, 100000);
                spl.Add(pt);

                open = close;
                // Advance one day
                xDate.AddDays(1.0);
                // but skip the weekends
                if (XDate.XLDateToDayOfWeek(xDate.XLDate) == 6)
                    xDate.AddDays(2.0);
            }

            OHLCBarItem myCurve = myPane.AddOHLCBar("", spl, Color.Black);
            myCurve.Bar.IsAutoSize = true;
            myCurve.Bar.Color = Color.Blue;

            // Use DateAsOrdinal to skip weekend gaps
            myPane.XAxis.Type = AxisType.DateAsOrdinal;

            // pretty it up a little
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);

            zgc.AxisChange();
            zgc.Refresh();
        }
        public void Default_Line_Chart(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;

            // Set the Titles
            myPane.Title.Text = "Line Chart";
            myPane.XAxis.Title.Text = "Price";
            myPane.YAxis.Title.Text = "Date";

            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            PointPairList list1 = new PointPairList();

            for (int i = 0; i < 0; i++)
            {
                x = (double)i + 5;
                y1 = 1.5 + Math.Sin((double)i * 0.2);
                list1.Add(x, y1);

            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("", list1, Color.Red, SymbolType.Circle);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
            zgc.Refresh();
        }
        #endregion

        #region Declieration Variable
        private static clsFileHandler oFH = new clsFileHandler();
        public string Symbol { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string FolderName { get; set; }
        string Delimiter = ",";
        int DataRow1 = 1;
        int HeaderRow = 0;
        int MaxRows = 0;
        private List<Quote> quotes = new List<Quote>();
        public TableLayoutPanel Layout { get; set; }
        public string Overlay_key {get;set;}
        public string Indector_key { get; set; }
        public string FirstOverlay { get; set; }
        #endregion


        public MainChart()
        {
            string dx = Properties.Settings.Default["Data_location"].ToString();
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.Path = dx.Length > 0 ? dx : path;
        }


        public  List<Quote> Quotes (string FullPath)
        {
            string __path = FullPath;
            

            quotes.AddRange(private_Quotes(__path));
            return quotes;
        }
        public  List<Quote> Quotes()
        {
            string __path = this.Path + "\\\\zLoader\\\\"+ FolderName + "\\\\" + FileName;            
            

            quotes.AddRange(private_Quotes(__path));
            return quotes;
        }
        private List<Quote> private_Quotes(string Path)
        {
            List<Quote> li = new List<Quote>();
            if (File.Exists(Path))
            {
                oFH = new clsFileHandler(Path);
                if (oFH.FileInf.Exists)
                {
                    DataTable dtData = new DataTable();
                    oFH.Delimiter = this.Delimiter;
                    oFH.DataRow1 = this.DataRow1;
                    oFH.HeaderRow = this.HeaderRow;
                    oFH.MaxRows = this.MaxRows;
                    dtData = oFH.CSVToTable();
                    if (dtData.Rows.Count > 1)
                    {
                        foreach (DataRow dr in dtData.Rows)
                        {
                            li.Add(new Quote()
                            {
                                Date = Convert.ToDateTime(dr[0].ToString()),
                                Open = Convert.ToDecimal(dr[1].ToString()),
                                High = Convert.ToDecimal(dr[2].ToString()),
                                Low = Convert.ToDecimal(dr[3].ToString()),
                                Close = Convert.ToDecimal(dr[4].ToString()),
                                Volume = Convert.ToDecimal(dr[5].ToString())
                            });
                        }
                        li.Sort(new QuoteComparer());
                        return li;
                    }
                }
            }
            return new List<Quote>() { };
        }

        public void Chart(string List, ZedGraphControl zgc, List<Quote> stockPrice)
        {
            switch (List)
            {
                case "CandleStick":
                     CandleChart(zgc, stockPrice,true);
                    break;
                case "OHLC Bars":
                    OHLC_Graph(zgc, stockPrice,true);
                    break;
                case "Solid Lines":
                    SolidLinkChart(zgc, stockPrice,true);
                    break;
                default:
                    break;
            }
        }

        private void CandleChart(ZedGraphControl zgc,List<Quote> stockPrice, bool overlay = false)
        {
            zgc.GraphPane.CurveList.Clear();
            zgc.GraphPane.GraphObjList.Clear();

            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "Candlestick Chart";
            myPane.XAxis.Title.Text = "Date";
            myPane.YAxis.Title.Text = "Price";

            StockPointList spl = new StockPointList();
            Random rand = new Random();

            foreach (Quote st in stockPrice)
            {
                XDate xDate = new XDate(st.Date);
                double x = xDate.XLDate;
                double open = Convert.ToDouble(st.Open);
                double close = Convert.ToDouble(st.Close);
                double hi = Convert.ToDouble(st.High);
                double low = Convert.ToDouble(st.Low);
                double volumn = Convert.ToDouble(st.Volume);         
                StockPt pt = new StockPt(x, hi, low, open, close, volumn);
                spl.Add(pt);
                xDate.AddDays(1.0);
            }

            JapaneseCandleStickItem myCurve = myPane.AddJapaneseCandleStick(this.Symbol, spl);
            myCurve.Stick.IsAutoSize = true;
            myCurve.Stick.Color = Color.Blue;

            if (overlay)
                foreach (Overlay o in Overlaylist())
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

            // Use DateAsOrdinal to skip weekend gaps
            myPane.XAxis.Type = AxisType.Date;

            // pretty it up a little
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);

            //always put this line here for refresh axis scale
            zgc.IsZoomOnMouseCenter = true;
            // zgc.IsEnableHZoom = true;
            // zgc.IsEnableVZoom = false;
            zgc.AxisChange();
            zgc.Refresh();
        }
        private void SolidLinkChart(ZedGraphControl zgc, List<Quote> stockPrice, bool overlay = false)
        {
            zgc.GraphPane.CurveList.Clear();
            zgc.GraphPane.GraphObjList.Clear();
            GraphPane myPane = zgc.GraphPane;
            myPane.Title.Text = "Line Chart";
            myPane.XAxis.Title.Text = "Price";
            myPane.YAxis.Title.Text = "Date";

            PointPairList list1 = new PointPairList();
            StockPointList spl = new StockPointList();
            foreach (Quote st in stockPrice)
            {
                XDate xDate = new XDate(st.Date);
                double x = xDate.XLDate;
                double open = Convert.ToDouble(st.Open);
                double close = Convert.ToDouble(st.Close);
                double hi = Convert.ToDouble(st.High);
                double low = Convert.ToDouble(st.Low);
                double volumn = Convert.ToDouble(st.Volume);
                StockPt pt = new StockPt(x, hi, low, open, close, volumn);
                spl.Add(pt);
                xDate.AddDays(1.0);
                list1.Add(x, close);
            }            
            LineItem myCurve = myPane.AddCurve(this.Symbol, list1, Color.Black, SymbolType.None);

            if (overlay)
                foreach (Overlay o in Overlaylist())
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);
            
            zgc.GraphPane.XAxis.Type = AxisType.Date;
            zgc.IsZoomOnMouseCenter = true;
            zgc.AxisChange();
            zgc.Refresh();
        }
        private void OHLC_Graph(ZedGraphControl zgc, List<Quote> stockPrice, bool overlay = false)
        {
            zgc.GraphPane.CurveList.Clear();
            zgc.GraphPane.GraphObjList.Clear();
            GraphPane myPane = zgc.GraphPane;

            myPane.Title.Text = "OHLC BAR";
            myPane.XAxis.Title.Text = "Date";
            myPane.YAxis.Title.Text = "Price";

            StockPointList spl = new StockPointList();
            Random rand = new Random();
            foreach (Quote st in stockPrice)
            {
                XDate xDate = new XDate(Convert.ToDateTime(st.Date));
                double x = xDate.XLDate;
                double open = Convert.ToDouble(st.Open);
                double close = Convert.ToDouble(st.Close);
                double hi = Convert.ToDouble(st.High);
                double low = Convert.ToDouble(st.Low);
                double volumn = Convert.ToDouble(st.Volume);                
                StockPt pt = new StockPt(x, hi, low, open, close, volumn);
                spl.Add(pt);
                xDate.AddDays(1.0);
            }

            OHLCBarItem myCurve = myPane.AddOHLCBar(this.Symbol, spl, Color.Black);
            myCurve.Bar.IsAutoSize = true;
            myCurve.Bar.Color = Color.Blue;

            if (overlay)
                foreach (Overlay o in Overlaylist())
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type);

            myPane.XAxis.Type = AxisType.Date;
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            myPane.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 255), 45.0f);
            zgc.IsZoomOnMouseCenter = true;
            zgc.AxisChange();
            zgc.Refresh();


        }
        
        private List<Overlay> Overlaylist(string priceName = "-None-")
        {
            OverlayPrice price = new OverlayPrice(this.quotes);
            List<Overlay> main_overlay = new List<Overlay>();

            if (priceName != "-None-")
                main_overlay.AddRange(price.drop_overlay(priceName));

            if (FirstOverlay != null)
                main_overlay.AddRange(price.drop_overlay(FirstOverlay));

            foreach (Control p in this.Layout.Controls)
            {
                if (p.Name.Contains(this.Overlay_key))
                {
                    TableLayoutControlCollection x = this.Layout.Controls;
                    PriceComboBox _x = x.Find(p.Name, false).FirstOrDefault() as PriceComboBox;
                    if (_x.SelectedItem.ToString() != "-None-")
                        main_overlay.AddRange(price.drop_overlay(_x.SelectedItem.ToString()));

                }
            }
           
            return main_overlay;
        }
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
