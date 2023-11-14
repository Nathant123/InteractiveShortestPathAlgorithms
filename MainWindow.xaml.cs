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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event EventHandler<string> PageNavigationRequested;
        protected readonly List<Page> Pages = new List<Page>();
        public MainWindow()
        {
            InitializeComponent();
            // initalize pages
            var homePage = new HomePage();
            var AStar = new AStar();
            var DijkstraPage = new Dijkstra();

            Pages.Add(homePage);
            Pages.Add(AStar);
            Pages.Add(DijkstraPage);

            mainFrame.Content = homePage;
        
        }
        public void Navigate(Page page) =>  mainFrame.Content = page;
        internal void NavigationEventHandler(string pageName)
        {
            PageNavigationRequested?.Invoke(this, pageName);
            OnPageNavigationRequested(this, pageName);
        }
        private void OnPageNavigationRequested(object sender, string pageName)
        {
            switch (pageName)
            {
                case "HomePage":
                    Navigate(Pages[0]);
                    break;
                case "AStar":
                    Navigate(Pages[1]);
                    break;

                case "Dijkstra":
                    Navigate(Pages[2]);
                    break;
            }
        }
    }
}
