using atpccl.Bezier;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        BezierFigure _bezier;
        public BezierFigure Bezier
        {
            get { return _bezier; }
            set
            {
                _bezier = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bezier"));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Bezier = new BezierFigure()
            {
                StartPoint = new Point(10, 10),
                EndPoint = new Point(100, 100),
                StartBezierPoint = new Point(20, 20),
                EndBezierPoint = new Point(200, 200)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
