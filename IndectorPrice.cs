using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using static zCharts.MainForm;

namespace zCharts
{
    public class IndectorPrice
    {
        private List<Quote> Quotes { get; set; }

        public IndectorPrice(List<Quote> Quotes)
        {
            this.Quotes = Quotes;
        }

        public List<Overlay> indicator_call (string indicatorComboBoxDailyIndicator)
        {
            switch (indicatorComboBoxDailyIndicator)
            {
                case "Aroon":
                   return Aroon(25);
                case "Accumulation / Distribution Line(ADL)":
                    return Accumulation_Distribution( 25);
                case "Average Directional Index(ADX)":
                    return Average_Directional_Index( 14);
                    
                case "Average True Range(ATR)":
                    return Average_True_Range( 14);
                    
                case "Awesome Oscillator(AO)":
                    return Awesome_Oscillator( 14);
                    
                case "Balance of Power(BOP)":
                    return Balance_of_Power( 14);
                    
                case "Bolinger Band Width":
                    return Bolinger_Band_Width( 20, 2);
                    
                case "Bollinger %B":
                    return Bollinger( 20, 2);
                    
                case "Bull and Bear Power":
                    return Bull_and_Bear_Power( 14);
                    
                case "Chaikin Money Flow(CMF)":
                    return Chaikin_Money_Flow();
                    
                case "Chaikin Oscillator":
                    return Chaikin_Oscillator();
                    
                case "Choppiness Index":
                    return Choppiness_Index( 14);
                    
                case "Commodity Channel Index(CCI)":
                    return Commodity_Channel_Index( 14);
                    
                case "ConnorsRSI":
                    return ConnorsRSI( 3, 2, 100);
                    
                case "Detrended Price Oscillator(DPO)":
                    return Detrended_Price_Oscillator( 14);
                    
              //  case "Elder - ray Index":
                    // no chart avaiable
              //      
              //  case "Force Index":
                    // no chart avaiable
               //     
               // case "Gator Oscillator":
                    // no chart avaiable
                    
                case "Historical Volatility(HV)":
                    return Historical_Volatility( 10);
                    
                case "Hurst Exponent":
                    return Hurst_Exponent();
                    
               // case "Ichimoku Cloud":
                    // no chart avaiable
               //     
                case "KDJ Index":
                    return KDJ_Index( 14, 3, 3);
                    
                case "Klinger Volume Oscillator":
                    return Klinger_Volume_Oscillator( 34, 55, 13);
                    
                case "Momentum Oscillator":
                    return Momentum_Oscillator( 20);
                    
                case "Money Flow Index(MFI)":
                    return Money_Flow_Index();
                    
                case "Moving Average Convergence / Divergence(MACD)":
                    return Moving_Average_Convergence( 12, 26, 9);
                    
                case "On - Balance Volume(OBV)":
                    return On_Balance_Volume();
                    
                case "Percentage Volume Oscillator(PVO)":
                    return Percentage_Volume_Oscillator( 12, 26, 9);
                    
                case "Price Momentum Oscillator(PMO)":
                    return Price_Momentum_Oscillator( 12, 26, 9);
                    
                case "Price Momentum Oscillator(PRS)":
                    return Price_Momentum_Oscillator( 35, 20, 10);
                    
                case "Rate of Change(ROC)":
                    return Momentum_Oscillator( 20);
                    
                case "Relative Strength Index(RSI)":
                    return Relative_Strength_Index( 14);
                    
                case "Rescaled Range Analysis":
                    return Rescaled_Range_Analysis();
                    
                case "ROC with Bands":
                    return ROC_with_Bands( 14, 2, 5);
                    
                case "Schaff Trend Cycle":
                    return Schaff_Trend_Cycle( 10, 23, 50);
                    
                case "Stochastic Momentum Index(SMI)":
                    return Stochastic_Momentum_Index( 14, 20, 5, 3);
                    
                case "Stochastic Oscillator":
                    return Stochastic_Oscillator( 10, 23, 50);
                    
                case "Stochastic RSI":
                    return Stochastic_RSI( 14, 20, 5, 3);
                    
                case "Super Trend":
                    return Super_Trend( 14, 3);
                    
                case "Triple EMA Oscillator(TRIX)":
                    return Triple_EMA_Oscillator( 14);
                    
                case "True Strength Index(TSI)":
                    return True_Strength_Index( 14, 20, 5);
                    
                case "Ulcer Index(UI)":
                    return Ulcer_Index( 14);
                    
                case "Ultimate Oscillator":
                    return Ultimate_Oscillator( 7, 14, 28);
                    
                case "Volume Simple Moving Average":
                    return Volume_Simple_Moving_Average( 14);
                    
                case "Vortex Indicator(VI)":
                    return VortexIndicator( 14);
                    
                case "Williams %R":
                    return Williams( 14);
                    
                case "Williams Alligator":
                    return Williams_Alligator();
                    
                default:
                    return new List<Overlay> { };
            }
        }

        private List<Overlay> Williams_Alligator(int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {               
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AlligatorResult> results = parameter != 0 ? quotes.GetAlligator(parameter) : quotes.GetAlligator();
                foreach (AlligatorResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Jaw != null)
                        y1 = (double)vr.Jaw;

                    if (vr.Lips != null)
                        y2 = (double)vr.Lips;

                    if (vr.Teeth != null)
                        y3 = (double)vr.Teeth;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    if (y3 != 0)
                        list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                list3.Sort(new PointPairComparer());

                li.Add(new Overlay { Name = "Jaw", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Lips", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Teeth", Color = Color.Green, Type = SymbolType.None, pList = list3 });

            }
            return li;
        }

        private List<Overlay> ROC_with_Bands(  int parameter1, int parameter2, int parameter3)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {

                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();
                PointPairList list4 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<RocWbResult> results = quotes.GetRocWb(parameter1, parameter2, parameter3);
                foreach (RocWbResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Roc != null)
                        y1 = (double)vr.Roc;
                    if (vr.RocEma != null)
                        y2 = (double)vr.RocEma;
                    if (vr.LowerBand != null)
                        y3 = (double)vr.LowerBand;
                    if (vr.UpperBand != null)
                        y4 = (double)vr.UpperBand;

                    //if (vr.Trix != null)
                    //     y2 = (double)vr.Trix;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    if (y3 != 0)
                        list3.Add(x, y3);
                    if (y4 != 0)
                        list4.Add(x, y4);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                list3.Sort(new PointPairComparer());
                list4.Sort(new PointPairComparer());

                
                li.Add(new Overlay { Name = "Roc", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "RocEma", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Orange, Type = SymbolType.None, pList = list3 });
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Orange, Type = SymbolType.None, pList = list4 });
               
            }
            return li;
        }

        private List<Overlay> Bull_and_Bear_Power(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ElderRayResult> results = parameter != 0 ? quotes.GetElderRay(parameter) : quotes.GetElderRay();
                //double[] y = new double[1];
                foreach (ElderRayResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.BearPower != null)
                        y1 = (double)vr.BearPower;

                    if (vr.BullPower != null)
                        y2 = (double)vr.BullPower;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());

                li.Add(new Overlay { Name = "BearPower", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "BullPower", Color = Color.Black, Type = SymbolType.None, pList = list2 });
              
            }
            return li;
        }

        private List<Overlay> Williams(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
               
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<WilliamsResult> results = parameter != 0 ? quotes.GetWilliamsR(parameter) : quotes.GetWilliamsR();
                foreach (WilliamsResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.WilliamsR != null)
                        y1 = (double)vr.WilliamsR;

                    //if (vr.Trix != null)
                    //     y2 = (double)vr.Trix;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //   if (y2 != 0)
                    //    list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "WilliamsR", Color = Color.Black, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Volume_Simple_Moving_Average(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SmaResult> results = parameter != 0 ? quotes.GetSma(parameter) : quotes.GetSma(20);
                foreach (SmaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Sma != null)
                        y1 = (double)vr.Sma;

                    //if (vr.Trix != null)
                    //     y2 = (double)vr.Trix;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //   if (y2 != 0)
                    //    list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Sma", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Ultimate_Oscillator(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<UltimateResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetUltimate(parameter1, parameter2, parameter3) : quotes.GetUltimate();
                foreach (UltimateResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Ultimate != null)
                        y1 = (double)vr.Ultimate;

                    // if (vr.Tsi != null)
                    //     y2 = (double)vr.Tsi;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    //  if (y2 != 0)
                    //     list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Ultimate", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                
            }
            return li;
        }

        private List<Overlay> Ulcer_Index(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
               
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<UlcerIndexResult> results = parameter != 0 ? quotes.GetUlcerIndex(parameter) : quotes.GetUlcerIndex(14);
                foreach (UlcerIndexResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UI != null)
                        y1 = (double)vr.UI;

                    //if (vr.Trix != null)
                    //     y2 = (double)vr.Trix;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //   if (y2 != 0)
                    //    list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UI", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> True_Strength_Index(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0, string CurveName1 = "A", string CurveName2 = "B", string title = null, string xtitle = null, string ytitle = null)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<TsiResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetTsi(parameter1, parameter2, parameter3) : quotes.GetTsi();
                foreach (TsiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Tsi != null)
                        y2 = (double)vr.Tsi;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Tsi", Color = Color.Red, Type = SymbolType.None, pList = list2 });
            }
            return li;
        }

        private List<Overlay> Triple_EMA_Oscillator(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
               
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<TrixResult> results = parameter != 0 ? quotes.GetTrix(parameter) : quotes.GetTrix(14);
                foreach (TrixResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Trix != null)
                        y2 = (double)vr.Trix;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Trix", Color = Color.Red, Type = SymbolType.None, pList = list2 });
               
                
            }
            return li;
        }

        private List<Overlay> Super_Trend(  int parameter1 = 0, int parameter2 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
               
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SuperTrendResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetSuperTrend(parameter1, parameter2) : quotes.GetSuperTrend();
                foreach (SuperTrendResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UpperBand != null)
                        y1 = (double)vr.UpperBand;

                    if (vr.LowerBand != null)
                        y2 = (double)vr.LowerBand;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                
            }
            return li;
        }

        private List<Overlay> Stochastic_RSI(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0,int parameter4 =0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
              
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StochRsiResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 && parameter4 != 0 ? quotes.GetStochRsi(parameter1, parameter2, parameter3, parameter4) : quotes.GetStochRsi(14, 14, 1, 1);
                foreach (StochRsiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.StochRsi != null)
                        y2 = (double)vr.StochRsi;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "StochRsi", Color = Color.Red, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Stochastic_Oscillator(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StochResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetStoch(parameter1, parameter2, parameter3) : quotes.GetStoch();
                foreach (StochResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.K != null)
                        y1 = (double)vr.K;

                    if (vr.D != null)
                        y2 = (double)vr.D;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "K", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "D", Color = Color.Red, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Stochastic_Momentum_Index(int parameter1 = 0, int parameter2 = 0, int parameter3 = 0,int parameter4=0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SmiResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 && parameter4 != 0 ? quotes.GetSmi(parameter1, parameter2, parameter3, parameter4) : quotes.GetSmi(14, 20, 5, 3);
                foreach (SmiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Smi != null)
                        y2 = (double)vr.Smi;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Smi", Color = Color.Red, Type = SymbolType.None, pList = list2 });  
                
            }
            return li;
        }

        private List<Overlay> Schaff_Trend_Cycle(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {               
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StcResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetStc(parameter1, parameter2, parameter3) : quotes.GetStc();
                foreach (StcResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Stc != null)
                        y1 = (double)vr.Stc;

                    //if (vr.Pmo != null)
                    //     y2 = (double)vr.Pmo;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    //   if (y2 != 0)
                    //   list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Stc", Color = Color.Green, Type = SymbolType.None, pList = list1 });                
            }
            return li;
        }

        private List<Overlay> Rescaled_Range_Analysis(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<HurstResult> results = parameter != 0 ? quotes.GetHurst(parameter) : quotes.GetHurst();
                foreach (HurstResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.HurstExponent != null)
                        y1 = (double)vr.HurstExponent;

                    //  if (vr.ObvSma != null)
                    //     y2 = (double)vr.ObvSma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //  list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                // list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "HurstExponent", Color = Color.Orange, Type = SymbolType.None, pList = list1 });

                //LineItem myCurve2 = myPane.AddCurve(CurveName2,list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Relative_Strength_Index(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<RsiResult> results = parameter != 0 ? quotes.GetRsi(parameter) : quotes.GetRsi();
                foreach (RsiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Rsi != null)
                        y1 = (double)vr.Rsi;

                    //  if (vr.ObvSma != null)
                    //     y2 = (double)vr.ObvSma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //  list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Rsi", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                //LineItem myCurve2 = myPane.AddCurve(CurveName2,list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Price_Momentum_Oscillator(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<PmoResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetPmo(parameter1, parameter2, parameter3) : quotes.GetPmo();
                foreach (PmoResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Pmo != null)
                        y2 = (double)vr.Pmo;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Pmo", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                
            }
            return li;
        }

        private List<Overlay> Percentage_Volume_Oscillator(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<PvoResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetPvo(parameter1, parameter2, parameter3) : quotes.GetPvo();
                foreach (PvoResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Pvo != null)
                        y2 = (double)vr.Pvo;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Signal", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Pvo", Color = Color.Red, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> On_Balance_Volume(int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ObvResult> results = parameter != 0 ? quotes.GetObv(parameter) : quotes.GetObv();
                foreach (ObvResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    //if (vr.Obv != null)
                    y1 = (double)vr.Obv;

                    if (vr.ObvSma != null)
                        y2 = (double)vr.ObvSma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Obv", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "ObvSma", Color = Color.Blue, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Moving_Average_Convergence(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<MacdResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetMacd(parameter1, parameter2, parameter3) : quotes.GetMacd();
                foreach (MacdResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Signal != null)
                        y1 = (double)vr.Signal;

                    if (vr.Macd != null)
                        y2 = (double)vr.Macd;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());

                li.Add(new Overlay { Name = "Signal", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Macd", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                
            }
            return li;
        }

        private List<Overlay> Money_Flow_Index(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<MfiResult> results = parameter != 0 ? quotes.GetMfi(parameter) : quotes.GetMfi();
                foreach (MfiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Mfi != null)
                        y1 = (double)vr.Mfi;

                    //if (vr.RocSma != null)
                    //    y2 = (double)vr.RocSma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //  if (y2 != 0)
                    //   list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Mfi", Color = Color.Red, Type = SymbolType.None, pList = list1 });


                // LineItem myCurve2 = myPane.AddCurve(CurveName2,
                //   list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Momentum_Oscillator(  int parameter = 0)
        {
            
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<RocResult> results = parameter != 0 ? quotes.GetRoc(parameter) : quotes.GetRoc(20);
                foreach (RocResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Roc != null)
                        y1 = (double)vr.Roc;

                    if (vr.RocSma != null)
                        y2 = (double)vr.RocSma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Roc", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "RocSma", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
            }
            return li;
        }

        private List<Overlay> Klinger_Volume_Oscillator(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<KvoResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetKvo(parameter1, parameter2, parameter3) : quotes.GetKvo();
                foreach (KvoResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Oscillator != null)
                        y1 = (double)vr.Oscillator;

                    if (vr.Signal != null)
                        y2 = (double)vr.Signal;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Oscillator", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Signal", Color = Color.Red, Type = SymbolType.None, pList = list2 });
            }
            return li;
        }

        private List<Overlay> KDJ_Index(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StochResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetStoch(parameter1, parameter2, parameter3) : quotes.GetStoch();
                foreach (StochResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.K != null)
                        y1 = (double)vr.K;

                    if (vr.D != null)
                        y2 = (double)vr.D;

                    if (y1 != 0)
                        list1.Add(x, y1);

                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "K", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "D", Color = Color.Blue, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Hurst_Exponent(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<HurstResult> results = parameter != 0 ? quotes.GetHurst(parameter) : quotes.GetHurst();
                foreach (HurstResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.HurstExponent != null)
                        y1 = (double)vr.HurstExponent;

                    //if (vr.Mean != null)
                    //    y2 = (double)vr.Mean;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "HurstExponent", Color = Color.OrangeRed, Type = SymbolType.None, pList = list1 });
            }

            return li;
        }

        private List<Overlay> Historical_Volatility(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
              
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StdDevResult> results = parameter != 0 ? quotes.GetStdDev(parameter) : quotes.GetStdDev(10);
                foreach (StdDevResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.StdDev != null)
                        y1 = (double)vr.StdDev;

                    if (vr.Mean != null)
                        y2 = (double)vr.Mean;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "StdDev", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Mean", Color = Color.Blue, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Detrended_Price_Oscillator(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<DpoResult> results = parameter != 0 ? quotes.GetDpo(parameter) : quotes.GetDpo(14);
                foreach (DpoResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Dpo != null)
                        y1 = (double)vr.Dpo;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);

                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Dpo", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> ConnorsRSI(  int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ConnorsRsiResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetConnorsRsi(parameter1, parameter2, parameter3) : quotes.GetConnorsRsi();
                foreach (ConnorsRsiResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.ConnorsRsi != null)
                        y1 = (double)vr.ConnorsRsi;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "ConnorsRsi", Color = Color.Orange, Type = SymbolType.None, pList = list1 });

                //LineItem myCurve2 = myPane.AddCurve(CurveName2,
                //      list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Commodity_Channel_Index(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<CciResult> results = parameter != 0 ? quotes.GetCci(parameter) : quotes.GetCci();
                foreach (CciResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Cci != null)
                        y1 = (double)vr.Cci;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Cci", Color = Color.Orange, Type = SymbolType.None, pList = list1 });

                //LineItem myCurve2 = myPane.AddCurve(CurveName2,
                //      list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Choppiness_Index(int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ChopResult> results = parameter != 0 ? quotes.GetChop(parameter) : quotes.GetChop();
                foreach (ChopResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Chop != null)
                        y1 = (double)vr.Chop;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Chop", Color = Color.Orange, Type = SymbolType.None, pList = list1 });

                //LineItem myCurve2 = myPane.AddCurve(CurveName2,
                //      list2, Color.Blue, SymbolType.None);
                
            }
            return li;
        }

        private List<Overlay> Chaikin_Oscillator(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ChaikinOscResult> results = parameter != 0 ? quotes.GetChaikinOsc(parameter) : quotes.GetChaikinOsc();
                foreach (ChaikinOscResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Oscillator != null)
                        y1 = (double)vr.Oscillator;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Oscillator", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                
            }
            return li;
        }

        private List<Overlay> Chaikin_Money_Flow(int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<CmfResult> results = parameter != 0 ? quotes.GetCmf(parameter) : quotes.GetCmf();
                foreach (CmfResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Cmf != null)
                        y1 = (double)vr.Cmf;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Cmf", Color = Color.Blue, Type = SymbolType.None, pList = list1 });                
            }
            return li;
        }

        private List<Overlay> Bolinger_Band_Width(  int parameter1 = 0, int parameter2 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<BollingerBandsResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetBollingerBands(parameter1, parameter2) : quotes.GetBollingerBands();
                foreach (BollingerBandsResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.PercentB != null)
                        y1 = (double)vr.PercentB;

                    if (vr.Width != null)
                        y2 = (double)vr.Width;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                //LineItem myCurve = myPane.AddCurve(CurveName1,
                //      list1, Color.Blue, SymbolType.None);
                li.Add(new Overlay { Name = "PercentB", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Width", Color = Color.Red, Type = SymbolType.None, pList = list2 });
            }
            return li;
        }
        private List<Overlay> Bollinger(  int parameter1 = 0, int parameter2 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<BollingerBandsResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetBollingerBands(parameter1, parameter2) : quotes.GetBollingerBands();
                foreach (BollingerBandsResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.PercentB != null)
                        y1 = (double)vr.PercentB;

                    if (vr.Width != null)
                        y2 = (double)vr.Width;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "PercentB", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                
            }
            return li;
        }

        private List<Overlay> Balance_of_Power(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();



                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<BopResult> results = parameter != 0 ? quotes.GetBop(parameter) : quotes.GetBop();
                foreach (BopResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Bop != null)
                        y1 = (double)vr.Bop;

                    //if (vr.Normalized != null)
                    //    y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    // list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Bop", Color = Color.Red, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Awesome_Oscillator(  int parameter1 = 0, int parameter2 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AwesomeResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetAwesome(parameter1, parameter2) : quotes.GetAwesome();
                foreach (AwesomeResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Oscillator != null)
                        y1 = (double)vr.Oscillator;

                    if (vr.Normalized != null)
                        y2 = (double)vr.Normalized;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Oscillator", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Normalized", Color = Color.Blue, Type = SymbolType.None, pList = list2 });                
            }
            return li;
        }

        private List<Overlay> Average_True_Range(  int parameter = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AtrResult> results = parameter != 0 ? quotes.GetAtr(parameter) : quotes.GetAtr();
                foreach (AtrResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Atr != null)
                        y1 = (double)vr.Atr;

                    //if (vr.AdlSma != null)
                    //y2 = (double)vr.MoneyFlowVolume;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    //list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Atr", Color = Color.Red, Type = SymbolType.None, pList = list1 });

            }
            return li;
        }

        private List<Overlay> Average_Directional_Index(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                // Make up some data arrays based on the Sine function
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();



                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AdxResult> results = quotes.GetAdx(parameter);
                foreach (AdxResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Adx != null)
                        y1 = (double)vr.Adx;

                    //if (vr.AdlSma != null)
                    //y2 = (double)vr.MoneyFlowVolume;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    //list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());

                li.Add(new Overlay { Name = "Adx", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                //   LineItem myCurve2 = myPane.AddCurve(CurveName2,
                //          list2, Color.Blue, SymbolType.None);

            }
            return li;
        }

        private List<Overlay> Accumulation_Distribution(  int parameter = 0)
        {
                        List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                // Make up some data arrays based on the Sine function
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AdlResult> results = quotes.GetAdl();
                foreach (AdlResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    //if (vr.Adl != null)
                    y1 = (double)vr.Adl;

                    //if (vr.AdlSma != null)
                    y2 = (double)vr.MoneyFlowVolume;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y1 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Adl", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "MoneyFlowVolume", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                
            }
            return li;
        }

        private List<Overlay> Aroon(  int parameter)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {

                // Make up some data arrays based on the Sine function
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();



                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AroonResult> results = quotes.GetAroon(parameter);
                foreach (AroonResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.AroonUp != null)
                        y1 = (double)vr.AroonUp.Value;

                    if (vr.AroonDown != null)
                        y2 = (double)vr.AroonDown.Value;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "AroonUp", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "AroonDown", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                
            }
            return li;
        }

        private List<Overlay> VortexIndicator(  int parameter)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                // Make up some data arrays based on the Sine function
                double x, y1 = 0, y2 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();



                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<VortexResult> results = quotes.GetVortex(parameter);
                foreach (VortexResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Nvi != null)
                        y1 = (double)vr.Nvi.Value;

                    if (vr.Pvi != null)
                        y2 = (double)vr.Pvi.Value;
                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Nvi", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Pvi", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
            }
            return li;
        }

    }
}
