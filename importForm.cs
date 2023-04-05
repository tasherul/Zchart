using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zCharts
{
    public partial class importForm : Form
    {
        string key { get; set; }
        public importForm(string key)
        {
            this.key = key;
            InitializeComponent();

        }

        string[] lines;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string files = openFileDialog1.FileName;
                lblfilename.Text = files;
                if (File.Exists(files))
                {
                    // Read a text file line by line.  
                    lines = File.ReadAllLines(files);
                    string data = "";
                    foreach (string line in lines)
                        data += line;

                    //MessageBox.Show(data);
                }
            }
        }
        bool next_closing = false;
        private void button2_Click(object sender, EventArgs e)
        {
            next_closing = true;
            importForm2 fr = new importForm2(lines, this.key);
            fr.Show();
            this.Close();

        }
 

        private void importForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (!next_closing)
            {

                MainForm fr = new MainForm();
                fr.Show();
                this.Hide();
            }
        }
    }
}
