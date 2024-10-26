using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace WPFApp
{
    public partial class NewTeamWindow : Window
    {
        private readonly ITeamService _teamService;
        public delegate void TeamAddedEventHandler(object sender, EventArgs e);
        public event TeamAddedEventHandler TeamAdded;
        // Parameterized constructor
        public NewTeamWindow(ITeamService teamService)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window_MouseLeftButtonDown);
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TeamNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(StatusTextBox.Text) ||
                    string.IsNullOrWhiteSpace(AdminIdTextBox.Text))
                {
                    NotificationWindow notificationWindow = new NotificationWindow("Please fill in all fields.");
                    notificationWindow.Show();
                    return;
                }

                var newTeam = new Team
                {
                    Name = TeamNameTextBox.Text,
                    Status = (TeamStatus)Enum.Parse(typeof(TeamStatus), StatusTextBox.Text),
                    AdminUserId = int.Parse(AdminIdTextBox.Text),
                    DeletedAt = null,
                };

                _teamService.CreateTeam(newTeam, newTeam.AdminUserId);

                // Gọi sự kiện TeamAdded, truyền team mới tạo vào
                TeamAdded?.Invoke(this, new TeamAddedEventArgs(newTeam));

                NotificationWindow notification = new NotificationWindow("Team added successfully");
                notification.Show();
                Close();
            }
            catch (Exception ex)
            {
                NotificationWindow notification = new NotificationWindow($"Error: {ex.Message}");
                notification.Show();
            }
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
