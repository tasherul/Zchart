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
        public bool selected { get; set; }
        public string SelectedValue { get; set; }
        private int index_table=0;
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
            int zedChartIndex =  childIndex / 2 ;

            if (Indctor_ddl < childIndex || Indctor_ddl == 0)
            {
                index_table++;
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
                ZedGraphControl zChart = __zChart(zedChartIndex);


                if (Selected == true)
                { 

                    this.indicator(SelectedIteam, zChart); 
                }
                else
                {
                    TableLayoutControlCollection x = IndectorLayout.Controls;
                    IndicatorComboBox _x = x.Find(Indector_Name + (childIndex == 2 ? childIndex : (childIndex - 2)), false).FirstOrDefault() as IndicatorComboBox;
                    this.indicator(_x.SelectedItem.ToString(), zChart);
                }
                __add_chart(zChart, zedChartIndex);

                //re-enforce the height of zgc
                //Size zSize = new Size(zgc.Size.Width, MainForm.indicatorHeight);
                

            }//if statement
            else
            {
                int x = ((childIndex == 3 ? 2 : childIndex) - 2);
                string __ = __find(Indector_Name + x);
                int y = index(x);
                if (__ == "-None-")
                {
                    //Indector_MainLayout.Controls.RemoveAt(y);
                    Indector_MainLayout.RowStyles[y].Height = 0;
                }
                else if(MessageBox.Show("Do You want to add "+__+" Chart?","New Chart",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    ZedGraphControl zChart = __zChart(zedChartIndex + 2);
                    this.indicator(__, zChart);
                    __add_chart(zChart, zedChartIndex + 2);
                }              
            }
        }

        private int index(int data)
        {
            if(data ==0)
             return 1;
            else
            {
                if (data == 2)
                    return 2;
                else
                {
                    if (data > 2)
                    {
                        return (data / 2) + 1;
                    }
                    else
                        return 2;
                }
            }
        }
        //set the row style with AutoSize
        private void __add_chart(ZedGraphControl zgc , int childrenIndex,int setindex=0)
        {
            int width = zgc.Size.Width;
            int height = MainForm.indicatorHeight;
            zgc.Size = new Size(width, height);
            Indector_MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MainForm.indicatorHeight));
            Indector_MainLayout.Controls.Add(zgc);
            Indector_MainLayout.Controls.SetChildIndex(zgc, childrenIndex);

            //if (setindex == 0)
            //{
            //    //Indector_MainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize, 400F));
            //    Indector_MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MainForm.indicatorHeight));
            //    Indector_MainLayout.Controls.Add(zgc);
            //    Indector_MainLayout.Controls.SetChildIndex(zgc, childrenIndex);
            //}
            //else
            //{
            //    //Indector_MainLayout.Controls.(setindex);
            //    Indector_MainLayout.RowStyles[setindex] = (new RowStyle(SizeType.Absolute, MainForm.indicatorHeight));
            //    Indector_MainLayout.SetRow(zgc, setindex);
            //    Indector_MainLayout.SetColumn(zgc, 0);
            //    Indector_MainLayout.ResumeLayout();
            //}



        }

        private ZedGraphControl __zChart(int __)
        {
            ZedGraphControl zChart = new ZedGraphControl();
            zChart.Dock = DockStyle.Fill;

            //set indicatorHeight to 3 inches
            zChart.Size = new Size(Indector_MainLayout.GetColumnWidths()[0], MainForm.indicatorHeight);
            zChart.AutoSize = false;
            zChart.Name = Indector_Name + "zed" + __.ToString();
            zChart.GraphPane.XAxis.Type = AxisType.Date;

            return zChart;
        }

        private string __find(string __)
        {
            TableLayoutControlCollection x = IndectorLayout.Controls;
            bool __f = false;

            foreach(Control a in x)            
                if (a.Name.Contains(__))                
                    if (a.Name == __)
                        __f = true;

            if (__f)
            {
                IndicatorComboBox _ = x.Find(__, false).FirstOrDefault() as IndicatorComboBox;
                return _.SelectedItem.ToString();
            }
            return SelectedIteam;
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

                //myPane.AddJapaneseCandleStick
                foreach (Overlay o in ov)
                    myPane.AddCurve(o.Name, o.pList, o.Color, o.Type );

                zgc.AxisChange();
            }
        }


    }
}
