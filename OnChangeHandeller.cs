using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace zCharts
{
    public class OnChangeHandeller
    {
        public TableLayoutPanel OverlayLayout_Price { get; set; }
        public PriceComboBox ComboBox { get; set; }
        public EventHandler OnChangeClick { get; set; }
        public string Price_Name { get; set; }
        public object Price_sender { get; set; }
        public TextBox Price_textBox { get; set; }
        int Price_ddl = 0;
        public int count { get; set; }
        public void OnChange()
        {
            int childIndex = OverlayLayout_Price.ColumnCount + OverlayLayout_Price.Controls.GetChildIndex((Control)Price_sender);
            if (Price_ddl < childIndex || Price_ddl == 0)
            {
                Price_ddl = childIndex;
                PriceComboBox pCombo = new PriceComboBox();
                pCombo.Size = ComboBox.Size;
                pCombo.SelectedIndexChanged += new EventHandler(OnChangeClick);
                pCombo.Dock = DockStyle.Fill;
                pCombo.Name = Price_Name + childIndex;
                TextBox pText = new TextBox();
                pText.Size = Price_textBox.Size;
                pText.Dock = DockStyle.Fill;

                OverlayLayout_Price.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                OverlayLayout_Price.Controls.Add(pCombo);
                OverlayLayout_Price.Controls.Add(pText);

                OverlayLayout_Price.Controls.SetChildIndex(pCombo, childIndex);
                OverlayLayout_Price.Controls.SetChildIndex(pText, childIndex + 1);

                count = Price_ddl;
            }
        }

        public TableLayoutPanel IndectorLayout { get; set; }
        public object Indector_sender { get; set; }
        public IndicatorComboBox Indector_Combobox { get; set; }
        public string Indector_Name { get; set; }
        public TextBox Indector_TextBox { get; set; }
        public EventHandler Indector_OnChangeClick { get; set; }
        public TableLayoutPanel Indector_MainLayout { get; set; }
        
        public List<Quote> Quotes { get; set; }

        public string SelectedIteam { get; set; }
        public bool Selected { get; set; }

        int Indctor_ddl = 0;

        public void OnIndectorChange()
        {
            //this 2 is the column count
            int childIndex = IndectorLayout.ColumnCount + IndectorLayout.Controls.GetChildIndex((Control)Indector_sender);
            if (Indctor_ddl < childIndex || Indctor_ddl == 0)
            {
                Indctor_ddl = childIndex;
                IndicatorComboBox indBox = new IndicatorComboBox();
                indBox.Size = Indector_Combobox.Size;
                indBox.Dock = DockStyle.Fill;
                indBox.SelectedIndexChanged += new EventHandler(Indector_OnChangeClick);
                indBox.Name = Indector_Name + (childIndex == 3 ? 2 : childIndex);
                TextBox pText = new TextBox();
                pText.Dock = DockStyle.Fill;
                pText.Size = Indector_TextBox.Size;

                //
                //add row style here
                //
                IndectorLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize, 30F));
                IndectorLayout.Controls.Add(indBox);
                IndectorLayout.Controls.Add(pText);

                //move to the right location
                IndectorLayout.Controls.SetChildIndex(indBox, childIndex);
                IndectorLayout.Controls.SetChildIndex(pText, childIndex + 1);

                //add ZedGraph to match with the indicators from above    
                Indector_MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                ZedGraphControl zChart = new ZedGraphControl();
                zChart.Dock = DockStyle.Fill;
                zChart.Size = new Size(Indector_MainLayout.GetColumnWidths()[0], 200);
                zChart.AutoSize = true;

                if (Selected == true)                
                    this.indicator(SelectedIteam, zChart);
                else
                {
                    TableLayoutControlCollection x = IndectorLayout.Controls;
                    IndicatorComboBox _x = x.Find(Indector_Name + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                    this.indicator(_x.SelectedItem.ToString(), zChart);
                }
                Indector_MainLayout.Controls.Add(zChart, 0, Indector_MainLayout.RowCount - 1);
            }
        }

        private void indicator(string indectorName, ZedGraphControl zgc)
        {
            IndectorPrice ip = new IndectorPrice(Quotes);
            List<Overlay> ov = ip.indicator_call(indectorName);

            if (Quotes.Count > 0)
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


    }
}
