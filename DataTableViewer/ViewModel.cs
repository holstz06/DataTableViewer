using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;

namespace DataTableViewer
{
    public class ViewModel : INotifyPropertyChanged
    { 
        public ViewModel()
        {

        }

        private string DirectoryString 
            = @"C:/Users/tyhol/Documents/Visual Studio 2015/Projects/DataTableViewer/DataTableViewer/bin/Debug/DataFiles";

        private DataTable _DgData = new DataTable("Data");
        public DataTable DgData
        {
            get { return _DgData; }
            set { _DgData = value; NotifyPropertyChanged("DgData"); }
        }

        /// <summary>
        /// A collection of paths names (XML) for the datagrid to display
        /// </summary>
        private ObservableCollection<string> _FileNames = new ObservableCollection<string>();
        public ObservableCollection<string> FileNames
        {
            get
            {
                DirectoryInfo directory = new DirectoryInfo(DirectoryString);
                FileInfo[] files = directory.GetFiles("*.xml");

                foreach (FileInfo file in files)
                {
                    _FileNames.Add(file.Name);
                }

                return _FileNames;
            }
            set
            {
                _FileNames = value; NotifyPropertyChanged("FileNames");
            }
        }

        /// <summary>
        /// The file that has been selected from the collection
        /// </summary>
        private string _SelectedFile;
        public string SelectedFile
        {
            get { return _SelectedFile; }
            set { _SelectedFile = value; NotifyPropertyChanged("SelectedFile"); }
        }

        /// <summary>
        /// Imports new data fom an xml file
        /// </summary>
        private void ChangeData()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(DirectoryString + "//" + SelectedFile, XmlReadMode.InferSchema);
            DgData.Clear();
            DgData = dataset.Tables[0];
        }

        private void WriteData()
        {
            DataSet dataset = new DataSet();
            dataset.Tables.Add(DgData);
            dataset.WriteXml(DirectoryString + "//" + SelectedFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == "SelectedFile")
                ChangeData();
        }
    }
}
