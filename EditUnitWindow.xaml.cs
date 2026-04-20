using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CRUD_App
{
    /// <summary>
    /// Interaction logic for EditUnit.xaml
    /// </summary>
    public partial class EditUnit : Window
    {
        private Unit _unitToEdit;
        private string? _imagePath;
        public EditUnit(Unit unitToEdit)
        {
            InitializeComponent();
            _unitToEdit = unitToEdit;
            unitName.Text = unitToEdit.Name;
            unitPoints.Text = unitToEdit.Points.ToString();
            _imagePath = _unitToEdit.Image;
            if (unitToEdit.Image != null) {
                unitImage.Source = new BitmapImage(new Uri(unitToEdit.Image.ToString(), UriKind.Relative ));
            }
        }

        private void CloseEdit_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EditUnit_Click(object sender, RoutedEventArgs e) {
            if (unitImage.Source == null) {
                MessageBox.Show("Upload an image first!", "Image not uploaded", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (unitName.Text == null || unitPoints.Text == null || unitName.Text == "" || unitPoints.Text == "") {
                MessageBox.Show("Unit must have a name and point value", "Name or Points not enetered", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _unitToEdit.Name = unitName.Text;
            _unitToEdit.Points = int.Parse(unitPoints.Text);
            _unitToEdit.Image = _imagePath;
            Close();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg) | *.jpg; *.jpeg";

            if (openFileDialog.ShowDialog() == true) {
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
