using System.ComponentModel;
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
        private readonly Dictionary<string, int> _categoryWeights = new Dictionary<string, int> {
                {"Character", 1 },
                {"Battleline", 2 },
                {"Dedicated Transport", 3 },
                {"Other Datasheets", 4 },
                {"Allied Units", 5 }
            };

        public MainWindow()
        {
            InitializeComponent();

            List<Unit> myUnits = new List<Unit>();

            myUnits.Add(new Unit {
                Name = "Angron",
                Points = 365,
                Image = "/Angron.jpg",
                Category = "Characters"
            });

            myUnits.Add(new Unit {
                Name = "Khorne Berzerkers",
                Points = 180,
                Image = "/Berzerkers.jpg",
                Category = "Battleline"
            });

            myUnits.Add(new Unit {
                Name = "Chaos Rhino",
                Points = 85,
                Image = "/Rhino.jpg",
                Category = "Dedicated Transport"
            });

            myUnits.Add(new Unit {
                Name = "Eightbound",
                Points = 135,
                Image = "/Eightbound.jpg",
                Category = "Other Datasheets"
            });

            UnitList.ItemsSource = myUnits;

            ICollectionView view = CollectionViewSource.GetDefaultView(myUnits);
            


        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}