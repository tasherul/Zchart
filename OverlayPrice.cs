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
    public class OverlayPrice
    {
        private List<Quote> Quotes {get;set;}

        public OverlayPrice(List<Quote> Quotes)
        {
            this.Quotes = Quotes;
        }
        public List<Overlay> drop_overlay(string overlay_name)
        {
            //return overlay_bollinger_bands(20, 2);
            //OverlayPrice price = new OverlayPrice(quote);
            //return price.overlay_bollinger_bands(20, 2);
            switch (overlay_name)
            {
                case "Bollinger Bands":
                   return overlay_bollinger_bands(20, 2);
                case "Arnaud Legoux Moving Average(ALMA)":
                    return Arnaud_Legoux_Moving_Average(10, 0.5, 6);
                case "Chandelier Exit":
                    return Chandelier_Exit(22, 3,ChandelierType.Long);
                case "Donchian Channels":
                    return Donchian_Channels(22);
                case "Double Exponential Moving Average(DEMA)":
                    return Double_Exponential_Moving_Average(22);
                case "Ehlers Fisher Transform":
                    return Ehlers_Fisher_Transform(20);
               case "Endpoint Moving Average(EPMA)":
                    return Endpoint_Moving_Average(20);
               case "Exponential Moving Average(EMA)":
                    return Exponential_Moving_Average(20);
               case "Fractal Chaos Bands(FCB)":
                    return Fractal_Chaos_Bands(14);
               //case "Heikin - Ashi":
              case "Hilbert Transform Instantaneous Trendline":
                    return Hilbert_Transform_Instantaneous_Trendline();
              case "Hull Moving Average(HMA)":
                    return Hull_Moving_Average(20);
              case "Kaufman's Adaptive Moving Average (KAMA)":
                    return Kaufman_Adaptive_Moving_Average(10, 2, 30);
              case "Keltner Channels":
                    return Keltner_Channels(20,(decimal)2.0, 10);
              case "Least Squares Moving Average(LSMA)":
                    return Endpoint_Moving_Average(20);
              case "MESA Adaptive Moving Average(MAMA)":
                    return MESA_Adaptive_Moving_Average(0.5, 0.05);
              case "Moving Average Envelopes":
                    return Moving_Average_Envelopes(20, 2.5, MaType.SMA);
              case "Parabolic SAR":
                    return Parabolic_SAR((decimal)0.02, (decimal)0.2);
                //  case "Pivot Points":
              case "Pivots":
                    return Pivos(2, 2, 20, EndType.HighLow);
              case "Price Channels":
                    return Donchian_Channels(20);
                //    case "Renko Chart":
              case "Rolling Pivot Points":
                    return Rolling_Pivot_Points(14, 0, PivotPointType.Woodie);
              case "Simple Moving Average(SMA)":
                    return Simple_Moving_Average(20);
              case "Smoothed Moving Average(SMMA)":
                    return Smoothed_Moving_Average(20);
              case "Standard Deviation Channels":
                    return Standard_Deviation_Channels(20, 2);
              case "STARC Band":
                    return STARC_Band(20,(decimal) 2.0, 10);
              case "Super Trend":
                    return Super_Trend(20, 2);
              case "Tillson T3 Moving Average":
                    return Tillson_T3_Moving_Average(5, 0.7);
              case "Triple Exponential Moving Average(TEMA)":
                    return Triple_Exponential_Moving_Average(20);
              case "Volatility Stop":
                    return Volatility_Stop(20, 2.5);
              case "Volume Simple Moving Average":
                    return Smoothed_Moving_Average(20);
              case "Volume Weighted Average Price(VWAP)":
                    return Volume_Weighted_Average_Price();
              case "Volume Weighted Moving Average(VWMA)":
                    return Volume_Weighted_Moving_Average(10);
              case "Weighted Moving Average(WMA)":
                    return Weighted_Moving_Average(10);
              case "Williams Fractal":
                    return Williams_Fractal(10);
              case "Zig Zag":
                    return Zig_Zag(EndType.Close, 3);
                default:
                    return new List<Overlay> { };
                 
            }

        }

        private List<Overlay> Zig_Zag(EndType close = EndType.Close, int parameter1=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ZigZagResult> results = quotes.GetZigZag(close,parameter1);
                foreach (ZigZagResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.RetraceLow != null)
                        y1 = (double)vr.RetraceLow;
                    if (vr.RetraceHigh != null)
                        y2 = (double)vr.RetraceHigh;
                    if (vr.ZigZag != null)
                        y3 = (double)vr.ZigZag;


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

                li.Add(new Overlay { Name = "RetraceLow", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "RetraceHigh", Color = Color.Gray, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "ZigZag", Color = Color.Blue, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Williams_Fractal(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<FractalResult> results = quotes.GetFractal(parameter1);
                foreach (FractalResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.FractalBear != null)
                        y1 = (double)vr.FractalBear;
                    if (vr.FractalBull != null)
                        y2 = (double)vr.FractalBull;
                    //if (vr.Sar != null)
                    //    y3 = (double)vr.Sar;


                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "FractalBear", Color = Color.Green, Type = SymbolType.Circle, pList = list1 });
                li.Add(new Overlay { Name = "FractalBull", Color = Color.Red, Type = SymbolType.Circle, pList = list2 });
                //li.Add(new Overlay { Name = "Sar", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Weighted_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<WmaResult> results = quotes.GetWma(parameter1);
                foreach (WmaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.Wma != null)
                        y1 = (double)vr.Wma;
                    //if (vr.UpperBand != null)
                    //    y2 = (double)vr.UpperBand;
                    //if (vr.Sar != null)
                    //    y3 = (double)vr.Sar;


                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Wma", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Sar", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Volume_Weighted_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<VwmaResult> results = quotes.GetVwma(parameter1);
                foreach (VwmaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.Vwma != null)
                        y1 = (double)vr.Vwma;
                    //if (vr.UpperBand != null)
                    //    y2 = (double)vr.UpperBand;
                    //if (vr.Sar != null)
                    //    y3 = (double)vr.Sar;


                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Vwma", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Sar", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Volume_Weighted_Average_Price()
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<VwapResult> results = quotes.GetVwap();
                foreach (VwapResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.Vwap != null)
                        y1 = (double)vr.Vwap;
                    //if (vr.UpperBand != null)
                    //    y2 = (double)vr.UpperBand;
                    //if (vr.Sar != null)
                    //    y3 = (double)vr.Sar;


                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Vwap", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Sar", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Volatility_Stop(int parameter1=0, double parameter2=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<VolatilityStopResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetVolatilityStop(parameter1, parameter2) : quotes.GetVolatilityStop();
                foreach (VolatilityStopResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.LowerBand != null)
                        y1 = (double)vr.LowerBand;
                    if (vr.UpperBand != null)
                        y2 = (double)vr.UpperBand;
                    if (vr.Sar != null)
                        y3 = (double)vr.Sar;

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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Sar", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Triple_Exponential_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<TemaResult> results = parameter1 != 0 ? quotes.GetTema(parameter1) : quotes.GetTema(parameter1);
                foreach (TemaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.Tema != null)
                        y1 = (double)vr.Tema;
                    //if (vr.UpperBand != null)
                    //    y2 = (double)vr.UpperBand;
                    //if (vr.SuperTrend != null)
                    //    y3 = (double)vr.SuperTrend;


                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Tema", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "SuperTrend", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Tillson_T3_Moving_Average(int parameter1=0, double parameter2=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<T3Result> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetT3(parameter1, parameter2) : quotes.GetT3();
                foreach (T3Result vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.T3 != null)
                        y1 = (double)vr.T3;
                    //if (vr.UpperBand != null)
                    //    y2 = (double)vr.UpperBand;
                    //if (vr.SuperTrend != null)
                    //    y3 = (double)vr.SuperTrend;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "T3", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "SuperTrend", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Super_Trend(int parameter1=0, int parameter2=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SuperTrendResult> results = parameter1 != 0 && parameter2!=0 ? quotes.GetSuperTrend(parameter1, parameter2) : quotes.GetSuperTrend();
                foreach (SuperTrendResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.LowerBand != null)
                        y1 = (double)vr.LowerBand;
                    if (vr.UpperBand != null)
                        y2 = (double)vr.UpperBand;
                    if (vr.SuperTrend != null)
                        y3 = (double)vr.SuperTrend;


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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "SuperTrend", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> STARC_Band(int parameter1=0, decimal parameter2=0, int parameter3=0)
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
                IEnumerable<StarcBandsResult> results = parameter1 != 0 ? quotes.GetStarcBands(parameter1, parameter2, parameter3) : quotes.GetStarcBands();
                foreach (StarcBandsResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.LowerBand != null)
                        y1 = (double)vr.LowerBand;
                    if (vr.UpperBand != null)
                        y2 = (double)vr.UpperBand;
                    if (vr.Centerline != null)
                        y3 = (double)vr.Centerline;


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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Centerline", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Standard_Deviation_Channels(int parameter1=0, int parameter2=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0,y2=0,y3=0,y4=0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();
                PointPairList list4 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<StdDevChannelsResult> results = parameter1 != 0 ? quotes.GetStdDevChannels(parameter1, parameter2) : quotes.GetStdDevChannels();
                foreach (StdDevChannelsResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;

                    if (vr.LowerChannel != null)
                        y1 = (double)vr.LowerChannel;
                    if (vr.UpperChannel != null)
                        y2 = (double)vr.UpperChannel;
                    if (vr.Centerline != null)
                        y3 = (double)vr.Centerline;


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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "LowerChannel", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "UpperChannel", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Centerline", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Smoothed_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0;
                PointPairList list1 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SmaResult> results = parameter1 != 0 ? quotes.GetSma(parameter1) : quotes.GetSma(parameter1);
                foreach (SmaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Sma != null)
                        y1 = (double)vr.Sma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Sma", Color = Color.Blue, Type = SymbolType.Circle, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Simple_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 0, y6 = 0;
                PointPairList list1 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<SmaResult> results = parameter1 != 0 ? quotes.GetSma(parameter1) : quotes.GetSma(parameter1);
                foreach (SmaResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Sma != null)
                        y1 = (double)vr.Sma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Sma", Color = Color.Blue, Type = SymbolType.Circle, pList = list1 });
            }
            return li;
        }

        private List<Overlay> Rolling_Pivot_Points(int parameter1, int parameter2, PivotPointType woodie = PivotPointType.Woodie)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0, y4 = 0, y5 = 0, y6 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();
                PointPairList list4 = new PointPairList();
                PointPairList list5 = new PointPairList();
                PointPairList list6 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<RollingPivotsResult> results = quotes.GetRollingPivots(parameter1, parameter2, woodie);
                foreach (RollingPivotsResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.R1 != null)
                        y1 = (double)vr.R1;

                    if (vr.R2 != null)
                        y2 = (double)vr.R2;

                    if (vr.PP != null)
                        y3 = (double)vr.PP;

                    if (vr.S1 != null)
                        y4 = (double)vr.S1;

                    if (vr.S2 != null)
                        y5 = (double)vr.S2;

                   

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    if (y3 != 0)
                        list3.Add(x, y3);
                    if (y4 != 0)
                        list4.Add(x, y4);
                    if (y5 != 0)
                        list5.Add(x, y5);

                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                list3.Sort(new PointPairComparer());
                list4.Sort(new PointPairComparer());
                list5.Sort(new PointPairComparer());
                
                li.Add(new Overlay { Name = "R1", Color = Color.Red, Type = SymbolType.Circle, pList = list1 });
                li.Add(new Overlay { Name = "R2", Color = Color.IndianRed, Type = SymbolType.Circle, pList = list2 });
                li.Add(new Overlay { Name = "PP", Color = Color.Gray, Type = SymbolType.Circle, pList = list3 });
                li.Add(new Overlay { Name = "S1", Color = Color.LightGreen, Type = SymbolType.Circle, pList = list4 });
                li.Add(new Overlay { Name = "S2", Color = Color.Green, Type = SymbolType.Circle, pList = list5 });
                //li.Add(new Overlay { Name = "LowTrend", Color = Color.Blue, Type = SymbolType.Circle, pList = list6 });
            }
            return li;
        }

        private List<Overlay> Pivos(int parameter1=0, int parameter2=0, int parameter3=0, EndType highLow = EndType.HighLow)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0,y4=0,y5=0,y6=0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();
                PointPairList list4 = new PointPairList();
                PointPairList list5 = new PointPairList();
                PointPairList list6 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<PivotsResult> results = parameter2 != 0 ? quotes.GetPivots(parameter1, parameter2, parameter3, highLow) : quotes.GetPivots();
                foreach (PivotsResult vr in results)
                {
                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.HighLine != null)
                        y1 = (double)vr.HighLine;

                    if (vr.HighPoint != null)
                        y2 = (double)vr.HighPoint;

                    if (vr.HighTrend != null)
                        y3 = (double)vr.HighTrend;

                    if (vr.LowLine != null)
                        y4 = (double)vr.LowLine;

                    if (vr.LowPoint != null)
                        y5 = (double)vr.LowPoint;

                    if (vr.LowTrend != null)
                        y6 = (double)vr.LowTrend;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    if (y3 != 0)
                        list3.Add(x, y3);
                    if (y4 != 0)
                        list4.Add(x, y4);
                    if (y5 != 0)
                        list5.Add(x, y5);
                    if (y6 != 0)
                        list6.Add(x, y6);
                }
                list1.Sort(new PointPairComparer());
                list4.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "HighLine", Color = Color.Violet, Type = SymbolType.Circle, pList = list1 });
                //li.Add(new Overlay { Name = "HighPoint", Color = Color.Violet, Type = SymbolType.Circle, pList = list2 });
                //li.Add(new Overlay { Name = "HighTrend", Color = Color.Violet, Type = SymbolType.Circle, pList = list3 });
                li.Add(new Overlay { Name = "LowLine", Color = Color.Blue, Type = SymbolType.Circle, pList = list4 });
                //li.Add(new Overlay { Name = "LowPoint", Color = Color.Blue, Type = SymbolType.Circle, pList = list5 });
                //li.Add(new Overlay { Name = "LowTrend", Color = Color.Blue, Type = SymbolType.Circle, pList = list6 });
            }
            return li;
        }

        private List<Overlay> Parabolic_SAR(decimal parameter1=0, decimal parameter2 =0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ParabolicSarResult> results = parameter2 != 0 ? quotes.GetParabolicSar(parameter1, parameter2) : quotes.GetParabolicSar(parameter1, parameter2);
                foreach (ParabolicSarResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Sar != null)
                        y1 = (double)vr.Sar;

                    //if (vr.UpperEnvelope != null)
                    //    y2 = (double)vr.UpperEnvelope;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Sar", Color = Color.Violet, Type = SymbolType.Circle, pList = list1 });
                //li.Add(new Overlay { Name = "UpperEnvelope", Color = Color.DarkGray, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.Orange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Moving_Average_Envelopes(int parameter1, double parameter2=0, MaType sMA=MaType.SMA)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<MaEnvelopeResult> results = parameter2 != 0 ? quotes.GetMaEnvelopes(parameter1, parameter2) : quotes.GetMaEnvelopes(parameter1);
                foreach (MaEnvelopeResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.LowerEnvelope != null)
                        y1 = (double)vr.LowerEnvelope;

                    if (vr.UpperEnvelope != null)
                        y2 = (double)vr.UpperEnvelope;

                    if (vr.Centerline != null)
                        y3 = (double)vr.Centerline;

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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "LowerEnvelope", Color = Color.DarkGray, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "UpperEnvelope", Color = Color.DarkGray, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Centerline", Color = Color.Orange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> MESA_Adaptive_Moving_Average(double parameter1, double parameter2)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<MamaResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetMama(parameter1, parameter2) : quotes.GetMama();
                foreach (MamaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Fama != null)
                        y1 = (double)vr.Fama;

                    if (vr.Mama != null)
                        y2 = (double)vr.Mama;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Fama", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Mama", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Keltner_Channels(int parameter1, decimal parameter2, int parameter3)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<KeltnerResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetKeltner(parameter1, parameter2, parameter3) : quotes.GetKeltner();
                foreach (KeltnerResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UpperBand != null)
                        y1 = (double)vr.UpperBand;

                    if (vr.LowerBand != null)
                        y2 = (double)vr.LowerBand;

                    if (vr.Centerline != null)
                        y3 = (double)vr.Centerline;

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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Kaufman_Adaptive_Moving_Average(int parameter1 = 0, int parameter2 = 0, int parameter3 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<KamaResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0? quotes.GetKama(parameter1, parameter2, parameter3) : quotes.GetKama();
                foreach (KamaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.ER != null)
                        y1 = (double)vr.ER;

                    if (vr.Kama != null)
                        y2 = (double)vr.Kama;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "ER", Color = Color.Black, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Kama", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Hull_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<HmaResult> results = quotes.GetHma(parameter1);
                foreach (HmaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Hma != null)
                        y1 = (double)vr.Hma;

                    //if (vr.LowerBand != null)
                    //    y2 = (double)vr.LowerBand;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Hma", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "LowerBand", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Hilbert_Transform_Instantaneous_Trendline()
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<HtlResult> results =quotes.GetHtTrendline();
                foreach (HtlResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.SmoothPrice != null)
                        y1 = (double)vr.SmoothPrice;

                    if (vr.Trendline != null)
                        y2 = (double)vr.Trendline;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "SmoothPrice", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Trendline", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Fractal_Chaos_Bands(int parameter1=0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<FcbResult> results = parameter1!=0? quotes.GetFcb(parameter1): quotes.GetFcb();
                foreach (FcbResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UpperBand != null)
                        y1 = (double)vr.UpperBand;

                    if (vr.LowerBand != null)
                        y2 = (double)vr.LowerBand;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Green, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Red, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Exponential_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<EmaResult> results = quotes.GetEma(parameter1);
                foreach (EmaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Ema != null)
                        y1 = (double)vr.Ema;

                    //if (vr.Trigger != null)
                    //    y2 = (double)vr.Trigger;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Ema", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Trigger", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Endpoint_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<EpmaResult> results = quotes.GetEpma(parameter1);
                foreach (EpmaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Epma != null)
                        y1 = (double)vr.Epma;

                    //if (vr.Trigger != null)
                    //    y2 = (double)vr.Trigger;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Epma", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "Trigger", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Ehlers_Fisher_Transform(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<FisherTransformResult> results = parameter1!=0?quotes.GetFisherTransform(parameter1): quotes.GetFisherTransform();
                foreach (FisherTransformResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Fisher != null)
                        y1 = (double)vr.Fisher;

                    if (vr.Trigger != null)
                        y2 = (double)vr.Trigger;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    if (y2 != 0)
                        list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Fisher", Color = Color.Red, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "Trigger", Color = Color.Blue, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Double_Exponential_Moving_Average(int parameter1)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                //PointPairList list2 = new PointPairList();
                //PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<DemaResult> results = quotes.GetDema(parameter1);
                foreach (DemaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Dema != null)
                        y1 = (double)vr.Dema;

                    //if (vr.LowerBand != null)
                    //    y2 = (double)vr.LowerBand;

                    //if (vr.Centerline != null)
                    //    y3 = (double)vr.Centerline;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Dema", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "LowerBand", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Donchian_Channels(int parameter1 = 0)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<DonchianResult> results = parameter1 != 0 ? quotes.GetDonchian(parameter1) : quotes.GetDonchian();
                foreach (DonchianResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UpperBand != null)
                        y1 = (double)vr.UpperBand;

                    if (vr.LowerBand != null)
                        y2 = (double)vr.LowerBand;

                    if (vr.Centerline != null)
                        y3 = (double)vr.Centerline;

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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Orange, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Centerline", Color = Color.DarkOrange, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Chandelier_Exit(int parameter1 = 0, double parameter2 = 0, ChandelierType type = ChandelierType.Long)
        {
            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<ChandelierResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetChandelier(parameter1, parameter2, type) : quotes.GetChandelier();
                foreach (ChandelierResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.ChandelierExit != null)
                        y1 = (double)vr.ChandelierExit;

                    //if (vr.LowerBand != null)
                    //    y2 = (double)vr.LowerBand;

                    //if (vr.Sma != null)
                    //    y3 = (double)vr.Sma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "ChandelierExit", Color = Color.Orange, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "LowerBand", Color = Color.Gray, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Sma", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        private List<Overlay> Arnaud_Legoux_Moving_Average(int parameter1 = 0, double parameter2 = 0, double parameter3 =0)
        {

            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<AlmaResult> results = parameter1 != 0 && parameter2 != 0 && parameter3 != 0 ? quotes.GetAlma(parameter1, parameter2, parameter3) : quotes.GetAlma();
                foreach (AlmaResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.Alma != null)
                        y1 = (double)vr.Alma;

                    //if (vr.LowerBand != null)
                    //    y2 = (double)vr.LowerBand;

                    //if (vr.Sma != null)
                    //    y3 = (double)vr.Sma;

                    if (y1 != 0)
                        list1.Add(x, y1);
                    //if (y2 != 0)
                    //    list2.Add(x, y2);
                    //if (y3 != 0)
                    //    list3.Add(x, y3);
                }
                list1.Sort(new PointPairComparer());
                //list2.Sort(new PointPairComparer());
                //list3.Sort(new PointPairComparer());
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "Alma", Color = Color.Blue, Type = SymbolType.None, pList = list1 });
                //li.Add(new Overlay { Name = "LowerBand", Color = Color.Gray, Type = SymbolType.None, pList = list2 });
                //li.Add(new Overlay { Name = "Sma", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

        public List<Overlay> overlay_bollinger_bands( int parameter1 = 0, int parameter2 = 0)
        {

            List<Overlay> li = new List<Overlay>();
            if (Quotes.Count > 0)
            {
                double x, y1 = 0, y2 = 0, y3 = 0;
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                PointPairList list3 = new PointPairList();

                IEnumerable<Quote> quotes = Quotes;
                IEnumerable<BollingerBandsResult> results = parameter1 != 0 && parameter2 != 0 ? quotes.GetBollingerBands(parameter1, parameter2) : quotes.GetBollingerBands();
                foreach (BollingerBandsResult vr in results)
                {

                    XDate xDate = new XDate(vr.Date);
                    x = xDate.XLDate;
                    if (vr.UpperBand != null)
                        y1 = (double)vr.UpperBand;

                    if (vr.LowerBand != null)
                        y2 = (double)vr.LowerBand;

                    if (vr.Sma != null)
                        y3 = (double)vr.Sma;

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
                //list4.Sort(new PointPairComparer());
                li.Add(new Overlay { Name = "UpperBand", Color = Color.Gray, Type = SymbolType.None, pList = list1 });
                li.Add(new Overlay { Name = "LowerBand", Color = Color.Gray, Type = SymbolType.None, pList = list2 });
                li.Add(new Overlay { Name = "Sma", Color = Color.Gray, Type = SymbolType.None, pList = list3 });
            }
            return li;
        }

    }
}
