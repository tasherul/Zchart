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
    public partial class newPortfolioForm : Form
    {
        loader lo = new loader();
        List<string> profolio = new List<string>();
        private void __default()
        {
            treeView1.Nodes.Clear();
            string tree = lo.view();
            if (tree.Length > 0)
            {
                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                int i = 0;
                foreach (treeview tr in _tree_view)
                {
                    treeView1.Nodes.Add(tr.root);
                    //realtime_treeView.Nodes.Add(tr.root);
                    foreach (string node in tr.node)
                    {
                        treeView1.Nodes[i].Nodes.Add(node);
                        //realtime_treeView.Nodes[i].Nodes.Add(node);
                    }
                    i++;
                }
            }
        }
        public newPortfolioForm()
        {
            InitializeComponent();
            __default();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string save_data = @"[";
                string tree = lo.view();
                if (tree.Length > 0)
                {
                    var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                    foreach (treeview tr in _tree_view)
                    {
                        save_data += "{'root':'" + tr.root + "','node':[";
                        int j = 0;
                        foreach (string nd in tr.node)
                        {
                            save_data += "'" + nd + "'" + (j == tr.node.Length - 1 ? "" : ",");
                            j++;
                        }
                        save_data += "]},";
                    }
                }
                int i = 0;
                // foreach(string s in profolio)
                //{
                save_data += "{'root':'" + textBox1.Text + "','node':[]}";
                //i++;
                //}
                save_data += "]";
                lo.save(save_data);
                __default();
                textBox1.Text = "";
            }
        }

        private void newPortfolioForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm fm = new MainForm();
            fm.Show();
        }
    }
}
