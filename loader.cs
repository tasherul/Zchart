using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zCharts
{
    public  class loader
    {

        public bool save(string data)
        {
            try
            {
                string db = Properties.Settings.Default["Database"].ToString();
                if (!File.Exists(db))
                    File.Create(db);
                    
                
                using (StreamWriter writetext = new StreamWriter(db))
                {
                    writetext.WriteLine(data);
                }
                Properties.Settings.Default["tree_files"] = data;
                return true;
            }
            catch(Exception )
            {
                return false;
            }
        }
        public List<string> nodes()
        {
            var _t = JsonConvert.DeserializeObject<List<treeview>>(view());
            List<string> o = new List<string>();
            foreach (treeview t in _t)
            {
                foreach (string s in t.node)
                {
                    o.Add(s);

                }
            }   
            return o;
        }
        public bool save(string data,string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            try
            {
                using (FileStream fs = File.Create(path))
                {
                    // Add some text to file    
                    Byte[] title = new UTF8Encoding(true).GetBytes(data);
                    fs.Write(title, 0, title.Length);
                }

                //using (StreamWriter writetext = new StreamWriter(path))
                //{
                //    writetext.WriteLine(data);
                //}                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string view()
        {
            string line = "";
            string db = Properties.Settings.Default["Database"].ToString();
            if (!File.Exists(db))
                File.Create(db);

            using (StreamReader sr = new StreamReader(db))
            {
                line += sr.ReadLine();
            }
            return line;
        }
        public string view(string path)
        {
            string line = "";
            string s = "";
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    s += line;
                }
            }
            return s;
        }
    }
    public class treeview
    {
        public string root { get; set; }
        public string[] node { get; set; }
    }
}
