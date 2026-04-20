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
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace CRUD_App {
    public partial class AddUnit : Window {
        private ObservableCollection<Unit> _allUnits; 
        private string? _category;
        private string? _imagePath;
        public AddUnit(string category, ObservableCollection<Unit> allUnits) {
            InitializeComponent();
            string categoryText = category;
            _category = category;
            _allUnits = allUnits;
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
            if(unitName.Text == null || unitPoints.Text == null || unitName.Text == "" || unitPoints.Text == "") {
                MessageBox.Show("Unit must have a name and point value", "Name or Points not enetered", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = unitName.Text;
            int points = int.Parse(unitPoints.Text);

            Unit newUnit = new Unit {
                Name = name,
                Points = points,
                Category = _category,
                Image = _imagePath
            };
            _allUnits.Add(newUnit);
            Close();
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
