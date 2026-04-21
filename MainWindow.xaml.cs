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
        private ObservableCollection<Unit> _allUnitsList;
        private ListCollectionView _viewAllUnits;
        private IUnitRepository _unitRepository = new UnitRepository();

        private string? _selectedCategory;

        public MainWindow()
        {
            InitializeComponent();

            _allUnitsList= new ObservableCollection<Unit>();

            foreach (var categoryName in _categoryWeights.Keys) {
                if (categoryName == "Allied Units") {
                    continue;
                }
                _allUnitsList.Add(new Unit {
                    Category = categoryName,
                    Name = null,
                    Points = 0
                });
            }

            //_allUnitsList.Add(new Unit {
            //    Name = "Eightbound",
            //    Points = 135,
            //    Image = "/Images/Eightbound.jpg",
            //    Category = "Other Datasheets"
            //});

            //_allUnitsList.Add(new Unit {
            //    Name = "Angron",
            //    Points = 365,
            //    Image = "/Images/Angron.jpg",
            //    Category = "Characters"
            //});

            //_allUnitsList.Add(new Unit {
            //    Name = "Chaos Rhino",
            //    Points = 85,
            //    Image = "/Images/Rhino.jpg",
            //    Category = "Dedicated Transport"
            //});

            //_allUnitsList.Add(new Unit {
            //    Name = "Khorne Berzerkers",
            //    Points = 180,
            //    Image = "/Images/Berzerkers.jpg",
            //    Category = "Battleline"
            //});

            
            _viewAllUnits = (ListCollectionView)CollectionViewSource.GetDefaultView(_allUnitsList);

            _viewAllUnits.GroupDescriptions.Clear();
            _viewAllUnits.SortDescriptions.Clear();

            _viewAllUnits.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            var categorySorter = new CategoryComparer(_categoryWeights);

            _viewAllUnits.CustomSort = categorySorter;

            //_view.Filter = (obj) => {
            //    var unit = obj as Unit;
            //    if (string.IsNullOrEmpty(_selectedCategory)) return true;
            //    return unit?.Category == _selectedCategory;
            //};

            dbList.ItemsSource = _viewAllUnits;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e) {
            BackButton.Visibility = Visibility.Collapsed;
        }

        //private void AddUnit_Click(object sender, RoutedEventArgs e) {
        //    var button = sender as Button;

        //    var selectedUnit = button.DataContext as Unit;

        //    if (selectedUnit != null) {
        //        _myArmyUnits.Add(selectedUnit);
        //        _armyView.Refresh();
        //    }
        //}
        //private void OpenSelection_Click(object sender, RoutedEventArgs e) {
        //    var button = sender as Button;
        //    if (button?.Tag == null) return;

        //    _selectedCategory = button.Tag as string;

        //    _viewAllUnits.Refresh();
        //    BackButton.Visibility = Visibility.Visible;
        //}

        private void RemoveUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var unitToRemove = button?.DataContext as Unit;

            if (unitToRemove != null) {
                _allUnitsList.Remove(unitToRemove);
            }
        }

        private void EditUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var unitToEdit = button?.DataContext as Unit;

            if (unitToEdit != null) {
                EditUnit editUnit = new EditUnit(unitToEdit);
                editUnit.ShowDialog();
                _viewAllUnits.Refresh();
            }
        }

        private void NewUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            if (button?.Tag == null) return;

            _selectedCategory = button.Tag as string;

            AddUnit addUnit = new AddUnit(_selectedCategory, _allUnitsList);
            addUnit.ShowDialog();
            _viewAllUnits.Refresh();
        }
    }
}