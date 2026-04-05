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
                {"Characters", 1 },
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
                Name = "Eightbound",
                Points = 135,
                Image = "/Eightbound.jpg",
                Category = "Other Datasheets"
            });

            myUnits.Add(new Unit {
                Name = "Angron",
                Points = 365,
                Image = "/Angron.jpg",
                Category = "Characters"
            });

            myUnits.Add(new Unit {
                Name = "Chaos Rhino",
                Points = 85,
                Image = "/Rhino.jpg",
                Category = "Dedicated Transport"
            });

            myUnits.Add(new Unit {
                Name = "Khorne Berzerkers",
                Points = 180,
                Image = "/Berzerkers.jpg",
                Category = "Battleline"
            });


            UnitList.ItemsSource = myUnits;

            
            var view = (ListCollectionView)CollectionViewSource.GetDefaultView(myUnits);

            view.GroupDescriptions.Clear();
            view.SortDescriptions.Clear();

            view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            var categorySorter = new CategoryComparer(_categoryWeights);

            view.CustomSort = categorySorter;

            view.Refresh();

            UnitList.ItemsSource = view;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void ShowCharacters_Click(object sender, RoutedEventArgs e) {
            CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }

        private void ShowBattleline_Click(object sender, RoutedEventArgs e) {
            CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e) {
            UnitSelectionView.Visibility = Visibility.Collapsed;
            CategoryView.Visibility = Visibility.Visible;
        }
    }
}