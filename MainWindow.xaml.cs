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
                    Name = null, // To jest kluczowe – po tym rozpoznamy "ducha"
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

        private void ShowCharacters_Click(object sender, RoutedEventArgs e) {
            _selectedCategory = "Characters";

            _view.Refresh();


            BackButton.Visibility = Visibility.Visible;
            //CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }

        private void ShowBattleline_Click(object sender, RoutedEventArgs e) {
            _selectedCategory = "Battleline";

            _view.Refresh();

            BackButton.Visibility = Visibility.Visible;
            //CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }
        private void ShowTransport_Click(object sender, RoutedEventArgs e) {
            _selectedCategory = "Dedicated Transport";

            _view.Refresh();

            BackButton.Visibility = Visibility.Visible;
            //CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }
        private void ShowOther_Click(object sender, RoutedEventArgs e) {
            _selectedCategory = "Other Datasheets";

            _view.Refresh();

            BackButton.Visibility = Visibility.Visible;
            //CategoryView.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Visible;
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e) {
            BackButton.Visibility = Visibility.Collapsed;
            UnitSelectionView.Visibility = Visibility.Collapsed;
            YourArmyList.Visibility = Visibility.Visible; // Przywracamy listę armii
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

            // 1. Pobieramy kategorię z taga (np. "Characters")
            _selectedCategory = button.Tag.ToString();

            // 2. Odświeżamy widok wszystkich jednostek (zadziała filtr)
            _view.Refresh();

            // 3. Zarządzamy widocznością (to co robiły stare metody)
            BackButton.Visibility = Visibility.Visible;
            UnitSelectionView.Visibility = Visibility.Visible;

            // Opcjonalnie: jeśli Twoja lista armii ma być schowana pod wyborem:
            YourArmyList.Visibility = Visibility.Collapsed;
        }
    }
}