using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Management;
using System.Security.Permissions;

namespace zCharts
{

    public class clsFileHandler
    {
        #region Constructors and properties
        /// <summary>
        /// Instantiate without a FileInfo - add it to the property later
        /// </summary>
        public clsFileHandler() { }
        /// <summary>
        /// Instantiate with the filename 
        /// </summary>
        /// <param name="sFilename">file name to be used to create the fileinfo object</param>
        public clsFileHandler(string sFilename)
        { FileInf = new FileInfo(sFilename); }

        /// <summary>
        /// FileInfo with the import/export file information
        /// </summary>
        public FileInfo FileInf { get; set; }
        private int mvHeaderRow = -1;
        /// <summary>
        /// The row containing the column titles -1 = no titles, 0 = first row
        /// </summary>
        public int HeaderRow
        {
            get { return mvHeaderRow; }
            set { mvHeaderRow = value; }
        }
        /// <summary>
        /// Zero based row containing the first data row
        /// </summary>
        public int DataRow1 { get; set; }
        /// <summary>
        /// field delimiter
        /// </summary>
        public string Delimiter { get; set; }
        /// <summary>
        /// Maximum rows ro read 0 = all rows
        /// </summary>
        public int MaxRows { get; set; }
        /// <summary>
        /// Name of the file without the extension
        /// </summary>
        public string NameOnly  //read only
        {
            get
            { return FileInf.Name.Substring(0, (FileInf.Name.Length - FileInf.Extension.Length)); }
        }
        public string UNCPath
        {
            get
            { return GetUNCPath(); }
        }
        private string GetUNCPath()
        {
            StringBuilder sPath = new StringBuilder();
            string sNetLtr;
            string sLtr = FileInf.FullName.Substring(0, 2);
            SelectQuery query = new SelectQuery(
                    "select name, ProviderName from win32_logicaldisk where drivetype=4");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                sNetLtr = Convert.ToString(mo["name"]);
                if (sNetLtr == sLtr)
                {
                    sPath.AppendFormat("{0}{1}", mo["ProviderName"], FileInf.DirectoryName.Substring(2));
                }
            }
            return sPath.ToString();
        }

        #endregion

        /// <summary>
        /// Read in the CSV file and move the data to a table
        /// </summary>
        /// <returns>Datatable with the CSV data loaded</returns>
        public DataTable CSVToTable()
        {
            try
            {
                // trap if the fileinfo has not been added to the object
                if (FileInf == null)
                { return null; }

                DataTable dtData = new DataTable();
                TextReader oTR = File.OpenText(FileInf.FullName);
                string sLine = null;
                string[] arData; //array of strings to load the data into for each line read in
                DataRow drData;
                int iRows = 0;

                //get the header row
                if (mvHeaderRow > 0)
                {
                    for (int i = 0; i < (mvHeaderRow + 1); i++)
                    {
                        sLine = CleanString(oTR.ReadLine());
                    }
                }
                else//get the first row to count the columns
                {
                    sLine = CleanString(oTR.ReadLine());
                }
                //create the columns in the table
                CreateColumns(dtData, sLine);

                //bail if the table failed
                if (dtData.Columns.Count == 0)
                { return null; }

                //reset the text reader
                oTR.Close();
                oTR = File.OpenText(FileInf.FullName);

                //get the first data line
                for (int i = 0; i < (DataRow1 + 1); i++)
                {
                    sLine = CleanString(oTR.ReadLine());
                }
                if (sLine != null)
                {
                    while (true)
                    {
                        //populate the string array with the line data
                        arData = sLine.Split(new string[] { Delimiter }, StringSplitOptions.None);
                        //load thedatarow
                        drData = dtData.NewRow();
                        for (int i = 0; i < dtData.Columns.Count; i++)
                        {
                            //test for additional fields - this can happen if there are stray commas
                            if (i < arData.Length)
                            {
                                drData[i] = arData[i];
                            }
                        }
                        //only get the top N rows if there is a max rows value > 0
                        iRows++;
                        if (MaxRows > 0 && iRows > MaxRows)
                        { break; }

                        //add the row to the table
                        dtData.Rows.Add(drData);

                        //read in the next line
                        sLine = CleanString(oTR.ReadLine());
                        if (sLine == null) { break; }
                    }
                }
                oTR.Close();
                oTR.Dispose();
                dtData.AcceptChanges();
                return dtData;
            }
            catch (Exception Exc)
            { throw Exc; }
        }

        /// <summary>
        /// Deal with single quiotes and backslash in the data
        /// </summary>
        /// <param name="sLine">String to check</param>
        /// <returns>Checked string</returns>
        private string CleanString(string sLine)
        {
            try
            {
                if (sLine == null) { return null; }
                sLine = sLine.Replace("'", "''");
                sLine = sLine.Replace("\"", "");
                return sLine;
            }
            catch (Exception Exc)
            { throw Exc; }
        }

        /// <summary>
        /// Create the datatable based on the title row of the number of elements in the 1st datarow array
        /// </summary>
        /// <param name="oTable">Table to add the columns to</param>
        /// <param name="sLine">title or first data row</param>
        private void CreateColumns(DataTable oTable, string sLine)
        {
            try
            {
                DataColumn oCol;
                string sTemp;
                int iCol = 0;
                string[] arData = sLine.Split(new string[] { Delimiter }, StringSplitOptions.None);
                for (int i = 0; i < arData.Length; i++)
                {
                    //get the header labels from the row
                    sTemp = string.Empty;
                    if (mvHeaderRow != -1)
                    {
                        sTemp = arData[i];
                    }

                    //deal with the empty string (may be missing from the row)
                    if ((sTemp.Trim()).Length == 0)
                    {
                        sTemp = string.Format("ColName_{0}", i.ToString());
                    }

                    //Deal with duplicate column names in the title row
                    iCol = oTable.Columns.Count + 100;
                    while (oTable.Columns.Contains(sTemp))
                    {
                        sTemp = string.Format("ColName_{0}", iCol.ToString());
                    }

                    oCol = new DataColumn(sTemp, System.Type.GetType("System.String"));
                    oTable.Columns.Add(oCol);
                }
            }
            catch (Exception Exc)
            { throw Exc; }
        }

        /// <summary>
        /// Export the table to the CSV filename
        /// </summary>
        /// <param name="dvData">Dataview to be exported</param>
        /// <param name="bExcludeTitles"></param>
        /// <returns>success</returns>
        public bool TableToCSV(DataView dvData, bool bExcludeTitles)
        {
            try
            {
                if (dvData == null) { return false; }
                StringBuilder sLine = new StringBuilder();

                //Delete the existing file
                if (FileInf.Exists)
                {
                    FileInf.Delete();
                }
                StreamWriter oSW = new StreamWriter(new FileStream(FileInf.FullName, FileMode.Create));

                if (!bExcludeTitles)
                {
                    foreach (DataColumn oCol in dvData.Table.Columns)
                    {
                        sLine.AppendFormat("{0}{1}", oCol.Caption, Delimiter);
                    }
                    //strip the trailing comma
                    sLine.Length = sLine.Length - 1;

                    //write the line
                    oSW.WriteLine(sLine.ToString());
                }

                for (int i = 0; i < dvData.Count; i++)
                {
                    sLine.Length = 0;
                    for (int j = 0; j < dvData.Table.Columns.Count; j++)
                    {
                        sLine.AppendFormat("{0}{1}", Convert.ToString(dvData[i][j]), Delimiter);
                    }
                    oSW.WriteLine(sLine.ToString());
                }
                oSW.Flush();
                oSW.Close();
                return true;
            }
            catch (Exception Exc)
            { throw Exc; }
        }
        public bool TableToCSV(DataView dvData, bool bExcludeTitles, string fileName)
        {
            try
            {
                if (dvData == null) { return false; }
                StringBuilder sLine = new StringBuilder();

                string strDir = System.IO.Directory.GetCurrentDirectory();

                FileIOPermission f2 = new FileIOPermission(FileIOPermissionAccess.Read, strDir);
                f2.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, strDir);
                try
                {
                    f2.Demand();
                }
                catch
                {
                    //MessageBox.Show("Permission Error!");
                    return false;
                }

                StreamWriter oSW = new StreamWriter(new FileStream(fileName, FileMode.Create));
                if (!bExcludeTitles)
                {
                    foreach (DataColumn oCol in dvData.Table.Columns)
                    {
                        sLine.AppendFormat("{0}{1}", oCol.Caption, Delimiter);
                    }
                    //strip the trailing comma
                    sLine.Length = sLine.Length - 1;

                    //write the line
                    oSW.WriteLine(sLine.ToString());
                }

                for (int i = 0; i < dvData.Count; i++)
                {
                    sLine.Length = 0;
                    for (int j = 0; j < dvData.Table.Columns.Count; j++)
                    {
                        sLine.AppendFormat("{0}{1}", Convert.ToString(dvData[i][j]), Delimiter);
                    }
                    oSW.WriteLine(sLine.ToString());
                }
                oSW.Flush();
                oSW.Close();
                oSW.Dispose(); //added

                return true;
            }
            //catch (Exception Exc)
            //{ throw Exc; }
            catch (Exception pe)
            {
                //MessageBox.Show(pe.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }
    }
}
