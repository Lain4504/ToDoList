using System;
using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace WPFApp
{
    public partial class TeamWindow : Window
    {
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly IToDoService _toDoService;
        private readonly int _loggedInUserID;
        private User _currentUser;

        // Constructor with parameters
        public TeamWindow(ITeamService teamService, IUserService userService, IToDoService toDoService, int loggedInUserID)
        {
            InitializeComponent();
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            _loggedInUserID = loggedInUserID;

            // Initialize current user
            _currentUser = _userService.GetCurrentUser(); // Đảm bảo phương thức này trả về một người dùng hợp lệ
            if (_currentUser == null)
            {
                MessageBox.Show("Current user is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LoadTeams(_loggedInUserID);
        }


        // Load teams for the logged-in user
        private void LoadTeams(int userID)
        {
            try
            {
                var teams = _teamService.GetAllTeams()
                                        .Where(t => t.DeletedAt == null && t.AdminUserId == userID)
                                        .ToList();
                TeamListView.ItemsSource = teams;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error loading teams: {ex.Message}");
            }
        }

        // Minimize window
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Maximize/restore window
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Normal) ? WindowState.Maximized : WindowState.Normal;
        }

        // Close window
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // New team button click
        private void NewTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var newTeamWindow = new NewTeamWindow(_teamService, _loggedInUserID);
            newTeamWindow.TeamAdded += (s, args) => LoadTeams(_loggedInUserID);
            newTeamWindow.ShowDialog();
        }

        // Search teams
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = TeamSearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                try
                {
                    var teams = await _teamService.GetTeamByNameAsync(searchTitle);
                    TeamListView.ItemsSource = teams;
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Error searching teams: {ex.Message}");
                }
            }
        }

        // Update team button click
        private void UpdateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = (sender as Button)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                var updateWindow = new UpdateTeam(_teamService, selectedTeam.TeamId);
                updateWindow.TeamUpdated += (s, args) => LoadTeams(_loggedInUserID);
                updateWindow.ShowDialog();
            }
            else
            {
                ShowNotification("Please choose a team before pressing update.");
            }
        }

        // Delete team button click
        private void DeleteTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = (sender as Button)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                var confirmationWindow = new ConfirmationWindow { Owner = this };
                confirmationWindow.ShowDialog();

                if (confirmationWindow.IsConfirmed)
                {
                    TryDeleteTeam(selectedTeam.TeamId);
                }
            }
            else
            {
                ShowNotification("No team selected for deletion.");
            }
        }

        // Try to delete team with error handling
        private void TryDeleteTeam(int teamId)
        {
            try
            {
                _teamService.DeleteTeam(teamId);
                ShowNotification("Team deleted successfully!");
                LoadTeams(_loggedInUserID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error deleting team: {ex.Message}");
            }
        }

        // Search box text changed
        private void TeamSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TeamSearchBox.Text.Trim()))
            {
                LoadTeams(_loggedInUserID);
            }
        }

        // Double click on team item
        private void TeamItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedTeam = (sender as Border)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                var insideTeamWindow = new InsideTeam(new TeamService(new TeamRepository(new ToDoListContext())), selectedTeam.TeamId, this);
                insideTeamWindow.Show();
                this.Hide();
            }
        }



        // Helper methods for notifications
        private void ShowNotification(string message)
        {
            var notificationWindow = new NotificationWindow(message);
            notificationWindow.ShowDialog();
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        // Show teams button click
        private void ShowTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userService == null)
                MessageBox.Show("UserService is not initialized.");
            if (_toDoService == null)
                MessageBox.Show("ToDoService is not initialized.");
            if (_currentUser == null)
                MessageBox.Show("CurrentUser is not initialized.");
            if (_userService == null || _toDoService == null || _currentUser == null)
            {
                MessageBox.Show("One or more required services are not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Tạo và hiển thị cửa sổ mới cho các đội của người dùng
                ShowTeamBelongForUser showTeamWindow = new ShowTeamBelongForUser(_userService, _toDoService, _loggedInUserID, _currentUser);
                showTeamWindow.Show();
                this.Close(); // Đóng cửa sổ hiện tại nếu cần
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing teams: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
