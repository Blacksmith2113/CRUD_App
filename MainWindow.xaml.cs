using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRUD_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Unit> myUnits = new List<Unit>();

            myUnits.Add(new Unit {
                Name = "Angron",
                Points = 365,
                Image = "/Angron.jpg"
            });

            myUnits.Add(new Unit {
                Name = "Khorne Berzerkers",
                Points = 180,
                Image = "/Berzerkers.jpg"
            });

            UnitList.ItemsSource = myUnits;

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}