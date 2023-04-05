using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zCharts
{
    public partial class PriceComboBox : System.Windows.Forms.ComboBox
    {
        private LicenseUsageMode m_ctorLMUsageMode = LicenseManager.UsageMode;

        public PriceComboBox()
        {
            InitializeComponent();

            if (m_ctorLMUsageMode == LicenseUsageMode.Runtime)
            {
                ComboboxItem item = new ComboboxItem();
                item.Text = "-None-";
                item.Value = 0;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Bollinger Bands";
                item.Value = 1;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Arnaud Legoux Moving Average(ALMA)";
                item.Value = 2;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Chandelier Exit";
                item.Value = 3;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Donchian Channels";
                item.Value = 4;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Double Exponential Moving Average(DEMA)";
                item.Value = 5;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Ehlers Fisher Transform";
                item.Value = 6;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Endpoint Moving Average(EPMA)";
                item.Value = 7;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Exponential Moving Average(EMA)";
                item.Value = 8;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Fractal Chaos Bands(FCB)";
                item.Value =9;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Heikin - Ashi";
                item.Value = 10;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Hilbert Transform Instantaneous Trendline";
                item.Value = 11;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Hull Moving Average(HMA)";
                item.Value = 12;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Kaufman's Adaptive Moving Average (KAMA)";
                item.Value = 13;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Keltner Channels";
                item.Value = 14;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Least Squares Moving Average(LSMA)";
                item.Value = 15;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "MESA Adaptive Moving Average(MAMA)";
                item.Value = 16;
                this.Items.Add(item);


                item = new ComboboxItem();
                item.Text = "Moving Average Envelopes";
                item.Value = 17;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Parabolic SAR";
                item.Value = 18;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Pivot Points";
                item.Value = 19;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Pivots";
                item.Value = 20;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Price Channels";
                item.Value = 21;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Renko Chart";
                item.Value = 22;
                this.Items.Add(item);


                item = new ComboboxItem();
                item.Text = "Rolling Pivot Points";
                item.Value = 23;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Simple Moving Average(SMA)";
                item.Value = 24;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Smoothed Moving Average(SMMA)";
                item.Value = 25;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Standard Deviation Channels";
                item.Value = 26;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "STARC Band";
                item.Value = 27;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Super Trend";
                item.Value = 28;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Tillson T3 Moving Average";
                item.Value = 29;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Triple Exponential Moving Average(TEMA)";
                item.Value = 30;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Volatility Stop";
                item.Value = 31;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Volume Simple Moving Average";
                item.Value = 32;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Volume Weighted Average Price(VWAP)";
                item.Value = 33;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Volume Weighted Moving Average(VWMA)";
                item.Value = 34;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Weighted Moving Average(WMA)";
                item.Value = 35;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Williams Fractal";
                item.Value = 36;
                this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Zig Zag";
                item.Value = 37;
                this.Items.Add(item);

                this.SelectedIndex = 0;
            }
            

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
