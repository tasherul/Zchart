using Newtonsoft.Json;
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
    public partial class importForm2 : Form
    {
        public importForm2()
        {
            InitializeComponent();
        }
        string[] data;
        string key { get; set; }
        public importForm2(string[] data, string key)
        {
            this.data = data;
            this.key = key;
            InitializeComponent();
        }
        List<string> symbol = new List<string>();
        private void button2_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Symbol");
            table.Columns.Add("Description");
            this.symbol.Clear();
            table.Rows.Clear();
            int symbol = 9;
            if (radioButton1.Checked)
                symbol = 44;
            else if (radioButton2.Checked)
                symbol = 32;
            else if (radioButton3.Checked)
                symbol = 9;
            else if (radioButton4.Checked)
                symbol = Convert.ToChar(textBox1.Text);
            string[] __data = this.data;
            if (Convert.ToInt32(numericUpDown1.Value) > 0)
            {
                Array.Clear(__data, 0, Convert.ToInt32(numericUpDown1.Value));
            }
            int i = 0;
            foreach (string s in __data)
            {
                if (s != null)
                {
                    string contant1 = "";
                    string contant2 = "";
                    bool first_contant = true;
                    foreach (char c in s)
                    {

                        if (c == symbol)
                            first_contant = false;

                        if (first_contant)
                            contant1 += c;
                        else
                            contant2 += c;
                    }
                    table.Rows.Add(contant1, contant2);
                    this.symbol.Add(contant1);

                }
                i++;
            }
            dataGridView1.DataSource = table;


        }
        loader lo = new loader();
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.symbol.Count > 0)
            {
                string tree = lo.view();
                if (tree.Length > 0)
                {
                    var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                    string save_data = "[";
                    int ii = 0;
                    foreach (treeview tr in _tree_view)
                    {
                        save_data += "{'root':'" + tr.root + "','node':[";
                        int j = 0;
                        if (tr.root == key)
                        {
                            int i = 0;
                            foreach (string s in this.symbol)
                            {
                                save_data += "'" + s.ToUpper() + "'" + (tr.node.Length > 0 ? "," : (i == symbol.Count - 1 ? "" : ","));
                                i++;
                            }
                        }
                        foreach (string nd in tr.node)
                        {
                            save_data += "'" + nd.ToUpper() + "'" + (j == tr.node.Length - 1 ? "" : ",");
                            j++;
                        }
                        save_data += "]}" + (_tree_view.Count - 1 == ii ? "" : ",");
                        ii++;
                    }
                    save_data += "]";
                    lo.save(save_data);
                }
                //Properties.Settings.Default["Symbol"] = store;
            }
            this.Close();
            MainForm fm = new MainForm();
            fm.Show();
        }

        private void importForm2_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm fm = new MainForm();
            fm.Show();
        }
    }
}
