using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows;
using PropertyChanged;
using System;

namespace DataTableViewer
{
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new ViewModel();
            DataContext = viewModel;
            //viewModel = (ViewModel)DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable datatable = ((DataView)XmlDataGrid.ItemsSource).ToTable();
                viewModel.DgData = datatable;
                viewModel.WriteData();
                MessageBox.Show("Saved successfully.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }

    [ImplementPropertyChanged]
    public class ViewModel : INotifyPropertyChanged
    {
        string DirectoryString = @"C:\Users\tyhol\Source\Repos\TCC_MVVM\TCC_MVVM\bin\Release";

        public DataTable DgData { get; set; } = new DataTable();
        public string SelectedFile { get; set; }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     A collection of paths names (XML) for the datagrid to display
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        ObservableCollection<string> _FileNames = new ObservableCollection<string>();
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

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Imports new data fom an xml file
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        void ChangeData()
        {
            DataSet dataset = new DataSet();
            dataset.ReadXml(DirectoryString + "//" + SelectedFile, XmlReadMode.InferSchema);
            DgData.Clear();
            DgData = dataset.Tables[0];
        }

        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Copies the data from the dataset to the xml file
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        public void WriteData()
        {
            DataSet dataset = new DataSet();
            dataset.Tables.Add(DgData);
            dataset.WriteXml(DirectoryString + "//" + SelectedFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == "SelectedFile")
                ChangeData();
        }
    }    
}
