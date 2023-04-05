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
    public partial class renamePortflio : Form
    {
        public renamePortflio()
        {
            InitializeComponent();
        }
        public string name { get; set; }
        public bool root { get; set; }
        public renamePortflio(string name, bool root = true)
        {
            InitializeComponent();
            this.name = name;
            this.root = root;
            textBox1.Text = name;
        }
        loader lo = new loader();
        private void button1_Click(object sender, EventArgs e)
        {
            string tree = lo.view();
            if (tree.Length > 0)
            {
                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                int i = 0;
                foreach (treeview tr in _tree_view)
                {
                    if (name == tr.root)
                    {
                        _tree_view[i].root = textBox1.Text;
                    }

                    if (!root)
                    {
                        int j = 0;
                        foreach (string node in tr.node)
                        {
                            if (name == node)
                                _tree_view[i].node[j] = textBox1.Text;

                            j++;
                        }
                    }
                    i++;
                }

                i = 0;
                string save_data = @"[";
                foreach (treeview tr in _tree_view)
                {
                    save_data += "{'root':'" + tr.root + "','node':[";
                    //old node
                    int j = 0;
                    foreach (string nd in tr.node)
                    { save_data += "'" + nd + "'" + (j == tr.node.Length - 1 ? "" : ","); j++; }
                    //finish old node

                    save_data += "]}" + (i == tr.root.Length - 1 ? "" : ",");
                    i++;
                }
                save_data += "]";

                lo.save(save_data);

                this.Close();

            }
        }

        private void renamePortflio_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm fm = new MainForm();
            fm.Show();
        }
    }
}
