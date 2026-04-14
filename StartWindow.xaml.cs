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

namespace CRUD_App {
    public partial class StartWindow : Window {
        private MainWindow _existingMainWindow;
        public List<string> Factions { get; set; } = new List<string> {
            "World Eaters",
            "Space Marines",
            "Death Guard",
        };
        public List<string> Detachments { get; set; } = new List<string> {
            "Berzerker Warband",
            "Cult of Blood",
            "Goretrack Onlaught",
            "Khorne Deamonkin",
            "Possessed Slaughterband",
            "Vessels of Wrath",
        };

        public StartWindow() {
            InitializeComponent();
            DataContext = this;
        }

        private void CreateArmy(object sender, RoutedEventArgs e) {
            string armyName = armyNameTbox.Text.Trim();
            var selectedFaction = Faction.SelectedItem as string;
            var selectedDetachment = Detachment.SelectedItem as string;
            int selectedBattlesize = 0;
            if (Battlesize1.IsChecked == true) selectedBattlesize = 1000;
            else if (Battlesize2.IsChecked == true) selectedBattlesize = 2000;
            else if (Battlesize3.IsChecked == true) selectedBattlesize = 3000;


            if (armyNameTbox.Text == null || armyName == "") {
                MessageBox.Show("The Army Name cannot be empty!", 
                    "Missing Data", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning);
                return;
            }
            
            if(string.IsNullOrEmpty(selectedFaction)) {
                MessageBox.Show("You must select a Faction!",
                    "Missing Data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(selectedDetachment)) {
                MessageBox.Show("You must select a Detachment!",
                    "Missing Data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if(_existingMainWindow == null) {
                _existingMainWindow = new MainWindow(armyName, selectedBattlesize, this);
            }

            _existingMainWindow.YourArmy.Text = armyName;

            _existingMainWindow.Show();
            Hide();
        }

        private void Faction_SelectionChanged(object sender, RoutedEventArgs e) {
            if (Faction.SelectedItem == null) return;

            string selectedFaction = Faction.SelectedItem as string;

            if(selectedFaction == "World Eaters") {
                Detachment.ItemsSource = Detachments;
            }
            else {
                Detachment.ItemsSource = null;
            }
        }
    }
}
