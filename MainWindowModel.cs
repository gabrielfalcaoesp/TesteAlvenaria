using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TesteAlvenaria.Core;
using TesteAlvenaria.Libs;
using TesteAlvenaria.Teste;

namespace TesteAlvenaria
{
    internal class MainWindowModel : AbsViewModel
    {
        #region Variables and properties

        private readonly Dispatcher _dispatcher;
        private string _filePath = string.Empty;
        private bool _isIndeterminate = false;
        private int _progress = 0;
        private int _max = 100;
        private string? _selectedWall = null;

        public string FilePath { get => _filePath; set { _filePath = value; OnPropertyChanged(); } }
        public bool IsIndeterminate { get => _isIndeterminate; set { _isIndeterminate = value; OnPropertyChanged(); } }
        public int MaxValue { get => _max; set { _max = value; OnPropertyChanged(); } }
        public int Progress { get => _progress; set { _progress = value; OnPropertyChanged(); } }
        public ObservableCollection<string> WallList { get; set; } = new ObservableCollection<string>();
        public string? SelectedWall { get => _selectedWall; set { _selectedWall = value; OnPropertyChanged(); } }
        public List<IWallData> Walls { get; } = new List<IWallData>();
        public int ShowRow { get; set; } = 0;


        public ICommand ChooseButton { get; }
        public ICommand ProcessButton { get; }
        public ICommand ShowFirstRowButton { get; }
        public ICommand ShowSecondRowButton { get; }

        #endregion

        #region Constructors

        public MainWindowModel()
        {
            _dispatcher = Application.Current.Dispatcher;
            ChooseButton = new GenericCommand(obj => ChooseFile(), obj => !IsIndeterminate);
            ProcessButton = new GenericCommand(obj => ProcessFile(), obj => FilePath != string.Empty && !IsIndeterminate);
            ShowFirstRowButton = new GenericCommand(
    obj =>
    {
        ShowRow = 0;
        SelectedWall = null;
        MainWindow mainWindow = new MainWindow();
        mainWindow.DrawFirstRow();
        
    },
    obj => !IsIndeterminate
);
            ShowSecondRowButton = new GenericCommand(obj => { ShowRow = 20; SelectedWall = null; }, obj => !IsIndeterminate);

            RectangleGeometry myRectangleGeometry = new RectangleGeometry();
            myRectangleGeometry.Rect = new Rect(50, 50, 25, 25);


            Canvas cnv_preview = new Canvas();
            cnv_preview.Name = "cnv_preview";
            cnv_preview.Margin = new System.Windows.Thickness(3);


            Path myPath = new Path();
            myPath.Fill = Brushes.LemonChiffon;
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myRectangleGeometry;

            cnv_preview.Children.Add(myPath);


        }



        #endregion

        #region Functions


        private void ChooseFile()
        {
            FilePath = "";
            var dlg = new OpenFileDialog();
            dlg.Filter = "Arquivos de texto (*.txt)|*.txt";
            dlg.Multiselect = false;
            if (dlg.ShowDialog() != true)
                return;
            FilePath = dlg.FileName;
            Progress = 0;
        }

        private async void ProcessFile()
        {
            IsIndeterminate = true;
            WallList.Clear();
            var task = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);

                return MainTest.RunTest(FilePath);
            });
            var result = await task;
            _dispatcher.Invoke(() =>
            {
                IsIndeterminate = false;
                MaxValue = 100;
                Progress = 100;
                WallList.Clear();
                Walls.Clear();
                Walls.AddRange(result);
                Walls.ForEach(w => WallList.Add(w.Name));
                SelectedWall = null;
                CommandManager.InvalidateRequerySuggested();
            });
        }

        #endregion
    }

}
