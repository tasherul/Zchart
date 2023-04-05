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
    public partial class addSymbolForm : Form
    {
        public addSymbolForm()
        {
            InitializeComponent();
            __default();
        }
        loader lo = new loader();
        private void __default()
        {
            string tree = lo.view();
            if (tree.Length > 0)
            {

                var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(tree);
                int i = 0;
                foreach (treeview tr in _tree_view)
                {
                    treeView1.Nodes.Add(tr.root);
                    foreach (string node in tr.node)
                    {
                        treeView1.Nodes[i].Nodes.Add(node);
                    }
                    i++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var _tree_view = JsonConvert.DeserializeObject<List<treeview>>(lo.view());
            int i = 0;
            string save_data = @"[";
            foreach (treeview tr in _tree_view)
            {
                save_data += "{'root':'" + tr.root + "','node':[";
                //old node
                int j = 0;
                foreach (string nd in tr.node)
                { save_data += "'" + nd.ToUpper() + "'" + (j == tr.node.Length - 1 ? "" : ","); j++; }
                //finish old node

                //add new node
                if (tr.root == treeView1.SelectedNode.Text)
                { save_data += (j > 0 ? "," : "") + "'" + textBox1.Text.ToUpper() + "'"; }
                //finish new node

                save_data += "]}" + (i == tr.root.Length - 1 ? "" : ",");
                i++;
            }
            save_data += "]";
            TreeNode node = new TreeNode(textBox1.Text);
            treeView1.SelectedNode.Nodes.Add(node);
            lo.save(save_data);
            textBox1.Text = "";
        }

        private void addSymbolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm fm = new MainForm();
            fm.Show();
        }
    }
}
