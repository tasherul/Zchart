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
    //ComboItem class
    public class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public string parameter { get; set; }
        public override string ToString()
        {
            return Text;
        }
    }

    public partial class IndicatorComboBox : System.Windows.Forms.ComboBox
    {
        private LicenseUsageMode m_ctorLMUsageMode = LicenseManager.UsageMode;

        public IndicatorComboBox()
        {
            InitializeComponent();

            if (m_ctorLMUsageMode == LicenseUsageMode.Runtime)
            {      
                ComboboxItem item = new ComboboxItem();
                item.Text = "-None-";
                item.Value = 0;

                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Aroon";
                item.Value = 1;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Accumulation / Distribution Line(ADL)";
                item.Value = 2;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Average Directional Index(ADX)";
                item.Value = 3;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Average True Range(ATR)";
                item.Value = 4;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Awesome Oscillator(AO)";
                item.Value = 5;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Balance of Power(BOP)";
                item.Value = 6;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Bolinger Band Width";
                item.Value = 7;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Bollinger %B";
                item.Value = 8;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Bull and Bear Power";
                item.Value = 9;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Chaikin Money Flow(CMF)";
                item.Value = 10;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Chaikin Oscillator";
                item.Value = 11;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Choppiness Index";
                item.Value = 12;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Commodity Channel Index(CCI)";
                item.Value = 13;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "ConnorsRSI";
                item.Value = 14;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Detrended Price Oscillator(DPO)";
                item.Value = 15;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Elder - ray Index";
                item.Value = 16;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Force Index";
                item.Value = 17;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Gator Oscillator";
                item.Value = 18;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Historical Volatility(HV)";
                item.Value = 19;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Hurst Exponent";
                item.Value = 20;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Ichimoku Cloud";
                item.Value = 21;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "KDJ Index";
                item.Value = 22;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Klinger Volume Oscillator";
                item.Value = 23;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Momentum Oscillator";
                item.Value = 24;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Money Flow Index(MFI)";
                item.Value = 25;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Moving Average Convergence / Divergence(MACD)";
                item.Value = 26;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "On - Balance Volume(OBV)";
                item.Value = 27;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Percentage Volume Oscillator(PVO)";
                item.Value = 28;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Price Momentum Oscillator(PMO)";
                item.Value = 29;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Price Relative Strength(PRS)";
                item.Value = 30;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Rate of Change(ROC)";
                item.Value = 31;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Relative Strength Index(RSI)";
                item.Value = 32;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Rescaled Range Analysis";
                item.Value = 33;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "ROC with Bands";
                item.Value = 34;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Schaff Trend Cycle";
                item.Value = 35;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Stochastic Momentum Index(SMI)";
                item.Value = 36;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Stochastic Oscillator";
                item.Value = 37;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Stochastic RSI";
                item.Value = 38;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Super Trend";
                item.Value = 39;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Triple EMA Oscillator(TRIX)";
                item.Value = 40;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "True Strength Index(TSI)";
                item.Value = 41;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Ulcer Index(UI)";
                item.Value = 42;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Ultimate Oscillator";
                item.Value = 43;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Volume Simple Moving Average";
                item.Value = 44;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Vortex Indicator(VI)";
                item.Value = 45;
                item.parameter = "14";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Williams %R";
                item.Value = 46;
                item.parameter = "";this.Items.Add(item);

                item = new ComboboxItem();
                item.Text = "Williams Alligator";
                item.Value = 47;
                item.parameter = "";this.Items.Add(item);

                this.SelectedIndex = 0;
           }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
