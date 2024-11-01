using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WPFApp
{
    public partial class ShowTeamBelongForUser : Window
    {
        private readonly IUserService _userService;
        private readonly IToDoService _toDoService;
        private readonly int _userId;
        private readonly User _currentUser;

        public ShowTeamBelongForUser(IUserService userService, IToDoService toDoService, int userId, User currentUser)
        {
            InitializeComponent();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _toDoService = toDoService ?? throw new ArgumentNullException(nameof(toDoService));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            LoadTeams();
        }

        private void LoadTeams()
        {
            if (_currentUser != null)
            {
                var teams = _userService.GetTeamsByUserId(_currentUser.UserId) ?? Enumerable.Empty<Team>();
                TeamsListBox.ItemsSource = teams.ToList();
            }
            else
            {
                MessageBox.Show("No user is logged in.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private int GetSelectedUserId()
        {
    
            return _currentUser?.UserId ?? throw new InvalidOperationException("No user is selected.");
        }

        private void TeamsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ViewTasksButton.IsEnabled = TeamsListBox.SelectedItem != null;
        }

        private void ViewTasksButton_Click(object sender, RoutedEventArgs e)
        {
            if (TeamsListBox.SelectedItem is Team selectedTeam)
            {
                var todos = _toDoService.GetToDosByTeamId(selectedTeam.TeamId);
                if (todos != null && todos.Any())
                {
                    string todoList = string.Join("\n", todos.Select(t => t.Title));
                    MessageBox.Show($"ToDos for Team {selectedTeam.Name}:\n{todoList}", "ToDo List", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"No ToDos found for Team {selectedTeam.Name}.", "ToDo List", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a team first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private User GetCurrentUser()
        {
            return _currentUser;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
