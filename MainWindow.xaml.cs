using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CRUD_App
{

    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, int> _categoryWeights = new Dictionary<string, int> {
                {"Characters", 1 },
                {"Battleline", 2 },
                {"Dedicated Transport", 3 },
                {"Other Datasheets", 4 },
                {"Allied Units", 5 }
            };
        private List<Unit> _allUnits;
        private ListCollectionView _view;
        private string _selectedCategory;
        private ObservableCollection<Unit> _myArmyUnits;
        private ListCollectionView _armyView;

        public MainWindow()
        {
            InitializeComponent();

            _allUnits= new List<Unit>();

            _allUnits.Add(new Unit {
                Name = "Eightbound",
                Points = 135,
                Image = "/Eightbound.jpg",
                Category = "Other Datasheets"
            });

            _allUnits.Add(new Unit {
                Name = "Angron",
                Points = 365,
                Image = "/Angron.jpg",
                Category = "Characters"
            });

            _allUnits.Add(new Unit {
                Name = "Chaos Rhino",
                Points = 85,
                Image = "/Rhino.jpg",
                Category = "Dedicated Transport"
            });

            _allUnits.Add(new Unit {
                Name = "Khorne Berzerkers",
                Points = 180,
                Image = "/Berzerkers.jpg",
                Category = "Battleline"
            });

            
            _view = (ListCollectionView)CollectionViewSource.GetDefaultView(_allUnits);

            _view.GroupDescriptions.Clear();
            _view.SortDescriptions.Clear();

            _view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            var categorySorter = new CategoryComparer(_categoryWeights);

            _view.CustomSort = categorySorter;

            _view.Filter = (obj) => {
                var unit = obj as Unit;
                if (string.IsNullOrEmpty(_selectedCategory)) return true;
                return unit?.Category == _selectedCategory;
            };

            _view.Refresh();

            UnitList.ItemsSource = _view;

            _myArmyUnits = new ObservableCollection<Unit>();
            foreach (var categoryName in _categoryWeights.Keys) {
                _myArmyUnits.Add(new Unit {
                    Category = categoryName,
                    Name = null,
                    Points = 0
                });
            }
            _armyView = (ListCollectionView)CollectionViewSource.GetDefaultView(_myArmyUnits);
            YourArmyList.ItemsSource = _myArmyUnits;

            _armyView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e) {
            BackButton.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Collapsed;
            YourArmyList.Visibility = Visibility.Visible;
        }

        private void AddUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;

            var selectedUnit = button.DataContext as Unit;

            if (selectedUnit != null) {
                _myArmyUnits.Add(selectedUnit);
                _armyView.Refresh();
            }
        }
        private void OpenSelection_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            if (button?.Tag == null) return;

            _selectedCategory = button.Tag.ToString();

            _view.Refresh();
            BackButton.Visibility = Visibility.Visible;
            UnitSelectionView.Visibility = Visibility.Visible;
            YourArmyList.Visibility = Visibility.Collapsed;
        }

        private void RemoveUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var unitToRemove = button?.DataContext as Unit;

            if (unitToRemove != null) {
                _myArmyUnits.Remove(unitToRemove);
            }
        }
    }
}