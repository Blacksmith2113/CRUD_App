using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CRUD_App {
    public partial class AddUnit : Window {
        private string? _category;
        private string? _imagePath;
        IUnitRepository _unitRepository = new UnitRepository();
        ObservableCollection<Unit> _allUnits;
        public AddUnit(string category, ObservableCollection<Unit> allUnits) {
            InitializeComponent();
            _allUnits = allUnits;           
            string categoryText = category;
            _category = category;
            if(_category == "Characters" || _category == "Other Datasheets") {
                categoryText = _category.TrimEnd('s');
            }
            namingMessage.Text = "Name this " + categoryText;
        }

        private void CloseAddUnit_Cick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SaveUnit_Click(object sender, RoutedEventArgs e) {
            if(unitImage.Source == null) {
                MessageBox.Show("Upload an image first!", "Image not uploaded", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(unitName.Text == "" || unitPoints.Text == "") {
                MessageBox.Show("Unit must have a name and point value", "Name or Points not entered", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!int.TryParse(unitPoints.Text, out int points)) {
                MessageBox.Show("Points must be a valid number", "Invalid points", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Unit unit = new Unit();
            unit.Name = unitName.Text;
            unit.Points = points;
            unit.Category = _category;
            if (_imagePath != null) {
                unit.Image = File.ReadAllBytes(_imagePath);
            }
            if (_unitRepository.Create(unit)) {
                _allUnits.Add(unit);
                Close();
            }
            else
                MessageBox.Show("Save Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg) | *.jpg; *.jpeg";

            if(openFileDialog.ShowDialog() == true) {
                _imagePath = openFileDialog.FileName;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(_imagePath);
                image.EndInit();
                unitImage.Source = image;
            }
        }
    }
}
