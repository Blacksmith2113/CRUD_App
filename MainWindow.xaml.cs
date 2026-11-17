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
        private ObservableCollection<Unit> _allUnitsList = new ObservableCollection<Unit>();
        private ListCollectionView _viewAllUnits;
        private IUnitRepository _unitRepository = new UnitRepository();

        private string? _selectedCategory;

        public MainWindow()
        {
            InitializeComponent();

            _allUnitsList = _unitRepository.ReadAll();


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
            
            _viewAllUnits = (ListCollectionView)CollectionViewSource.GetDefaultView(_allUnitsList);

            _viewAllUnits.GroupDescriptions.Clear();
            _viewAllUnits.SortDescriptions.Clear();

            _viewAllUnits.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            var categorySorter = new CategoryComparer(_categoryWeights);

            _viewAllUnits.CustomSort = categorySorter;

            dbList.ItemsSource = _viewAllUnits;
        }

        private void Close_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BackToCategories_Click(object sender, RoutedEventArgs e) {
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void RemoveUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var unitToRemove = button?.DataContext as Unit;

            if (MessageBox.Show("Are you sure you want to delete this unit?", "Attention", MessageBoxButton.OKCancel) == MessageBoxResult.OK) {
                _allUnitsList.Remove(unitToRemove);
                _unitRepository.Delete(unitToRemove);
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

        private void AddUnit_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            if (button?.Tag == null) return;

            _selectedCategory = button.Tag as string;

            AddUnit addUnit = new AddUnit(_selectedCategory, _allUnitsList);
            addUnit.ShowDialog();
            _viewAllUnits.Refresh();
        }
    }
}