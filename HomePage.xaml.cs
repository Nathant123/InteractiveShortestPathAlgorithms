using System;
using System.Collections.Generic;
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

namespace InteractiveShortestPathAlgorithms
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void btnAStar_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = (MainWindow)Window.GetWindow(this); // casted to access eventhandler

            if (currentWindow != null)
               currentWindow.NavigationEventHandler("AStar");

        }

        private void btnDijkstra_Click(object sender, RoutedEventArgs e)
        {
            var currentWindow = (MainWindow)Window.GetWindow(this); // casted to access eventhandler

            if (currentWindow != null)// if it exists
                currentWindow.NavigationEventHandler("Dijkstra");
        }
    }
}
