using Repositories;
using System.Windows;
using DataAccessLayer;
using System.Collections.Generic;
using BusinessObjects;

namespace WPFApp
{
    public partial class ShowTeamBelongForUser : Window
    {
        private readonly UserRepository _userRepository;
        private Team _selectedTeam;

        public ShowTeamBelongForUser(int userId)
        {
            InitializeComponent();
            var context = new ToDoListContext();
            _userRepository = new UserRepository(context);
            LoadUserTeams(userId);
        }

        private void LoadUserTeams(int userId)
        {
            var teams = _userRepository.GetTeamsForUser(userId);
            TeamsListBox.ItemsSource = teams;
        }

        private void TeamsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TeamsListBox.SelectedItem is Team selectedTeam)
            {
                _selectedTeam = selectedTeam;
                ViewTasksButton.IsEnabled = _selectedTeam != null;
            }
            else
            {
                _selectedTeam = null;
                ViewTasksButton.IsEnabled = false;
            }
        }

        private void LoadTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(UserIdTextBox.Text, out int userId))
            {
                LoadUserTeams(userId);
            }
            else
            {
                MessageBox.Show("Please enter a valid User ID.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
