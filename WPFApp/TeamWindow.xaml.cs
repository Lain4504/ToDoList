using System;
using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace WPFApp
{
    public partial class TeamWindow : Window
    {
        private readonly ITeamService _teamService;

        // Constructor không tham số
        public TeamWindow() : this(new TeamService(new TeamRepository(new ToDoListContext())))
        {
            InitializeComponent();
            LoadTeams();
        }
        // Constructor với tham số
        public TeamWindow(ITeamService teamService)
        {
            InitializeComponent();
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            LoadTeams();
        }
        // Các button thao tác với cửa sổ
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Giảm cửa sổ
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized; // Max
            else
                this.WindowState = WindowState.Normal; // Trở lại trạng thái bình thường
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Đóng cửa sổ
        }
        public void LoadTeams()
        {
            try
            {
                IEnumerable<BusinessObjects.Team> teams = _teamService.GetAllTeams().Where(t => t.DeletedAt == null);
                TeamListView.ItemsSource = teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void NewTeamButton_Click(object sender, RoutedEventArgs e)
        {
            NewTeamWindow newTeamWindow = new NewTeamWindow(_teamService);

            // Lắng nghe sự kiện TeamAdded để cập nhật giao diện team detail
            newTeamWindow.TeamAdded += (s, args) =>
            {
                var teamArgs = args as TeamAddedEventArgs;
                if (teamArgs != null)
                {
                    var newTeam = teamArgs.NewTeam;

                    // Cập nhật danh sách Teams
                    var teamsList = DataContext as ObservableCollection<BusinessObjects.Team>;
                    if (teamsList != null)
                    {
                        teamsList.Add(newTeam); // Thêm đội mới vào danh sách
                    }
                }
            };
            newTeamWindow.ShowDialog(); 
            LoadTeams();
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = TeamSearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                try
                {
                    IEnumerable<BusinessObjects.Team> teams = await _teamService.GetTeamByNameAsync(searchTitle);
                    TeamListView.ItemsSource = teams;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void UpdateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy team hiện tại từ dữ liệu được liên kết với ViewModel
            var selectedTeam = (sender as Button)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                UpdateTeam updateWindow = new UpdateTeam(_teamService, selectedTeam.TeamId);
                updateWindow.TeamUpdated += (s, args) =>
                {
                    LoadTeams();
                };
                updateWindow.ShowDialog();
            }
            else
            {
                NotificationWindow notification = new NotificationWindow("Please choose a team before pressing update.");
                notification.ShowDialog();
            }
        }

        // Sự kiện xóa 
        private void DeleteTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTeam = (sender as Button)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                // Hiển thị cửa sổ xác nhận xóa
                ConfirmationWindow confirmationWindow = new ConfirmationWindow();
                confirmationWindow.Owner = this; // Thiết lập chủ sở hữu của cửa sổ xác nhận
                confirmationWindow.ShowDialog(); // Hiển thị cửa sổ dưới dạng dialog

                // Kiểm tra nếu người dùng xác nhận xóa
                if (confirmationWindow.IsConfirmed)
                {
                    try
                    {
                        // Gọi service để xóa team
                        _teamService.DeleteTeam(selectedTeam.TeamId);

                        // Hiển thị thông báo thành công qua NotificationWindow
                        NotificationWindow notification = new NotificationWindow("Team deleted successfully!");
                        notification.ShowDialog();

                        // Xóa team khỏi danh sách hiện tại
                        var teamsList = DataContext as ObservableCollection<BusinessObjects.Team>;
                        if (teamsList != null)
                        {
                            teamsList.Remove(selectedTeam);
                            // Thông báo UI đã cập nhật dữ liệu
                            TeamListView.Items.Refresh();
                        }
                        LoadTeams();
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị thông báo lỗi qua NotificationWindow
                        NotificationWindow notification = new NotificationWindow($"Error deleting team: {ex.Message}");
                        notification.ShowDialog();
                    }
                    
                }
            }
            else
            {
                // Hiển thị thông báo khi không có team được chọn
                NotificationWindow notification = new NotificationWindow("No team selected for deletion.");
                notification.ShowDialog();
            }
        }
        private void TeamSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTitle = TeamSearchBox.Text.Trim();
            if (string.IsNullOrEmpty(searchTitle))
            {
                LoadTeams();
                SearchPlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
                SearchPlaceholderLabel.Visibility = Visibility.Collapsed;
        }

        private void TeamItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedTeam = (sender as Border)?.DataContext as BusinessObjects.Team;

            if (selectedTeam != null)
            {
                // Mở cửa sổ InsideTeam với thông tin của team đã chọn
                InsideTeam insideTeamWindow = new InsideTeam(new TeamService(new TeamRepository(new ToDoListContext())), selectedTeam.TeamId); // Thay thế _teamService nếu cần
                insideTeamWindow.ShowDialog(); // Hiển thị cửa sổ chi tiết team
            }
        }

    }
}