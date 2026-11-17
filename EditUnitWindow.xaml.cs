using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CRUD_App
{
    public partial class EditUnit : Window
    {
        private Unit _unitToEdit;
        private byte[] _image;
        private string _imagePath;
        IUnitRepository _unitRepository = new UnitRepository();
        public EditUnit(Unit unitToEdit)
        {
            InitializeComponent();
            _unitToEdit = unitToEdit;
            unitName.Text = unitToEdit.Name;
            unitPoints.Text = unitToEdit.Points.ToString();
            _image = _unitToEdit.Image;
            unitImage.Source = Utility.ByteArrayToBitmapImage(_image);
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
            if (string.IsNullOrWhiteSpace(unitName.Text) || string.IsNullOrWhiteSpace(unitPoints.Text)) {
                MessageBox.Show("Unit must have a name and point value", "Name or Points not entered", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(unitPoints.Text, out int points)) {
                MessageBox.Show("Points must be a valid number", "Invalid points", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string? oldName = _unitToEdit.Name;
            int oldPoints = _unitToEdit.Points;
            byte[]? oldImage = _unitToEdit.Image;

            _unitToEdit.Name = unitName.Text;
            _unitToEdit.Points = points;
            if (_imagePath != null) {
                _unitToEdit.Image = File.ReadAllBytes(_imagePath);
            }

            if (_unitRepository.Edit(_unitToEdit)) {
                Close();
            }
            else {
                _unitToEdit.Name = oldName;
                _unitToEdit.Points = oldPoints;
                _unitToEdit.Image = oldImage;
                MessageBox.Show("Save Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
