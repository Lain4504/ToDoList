﻿using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace WPFApp
{
    public partial class InsideTeam : Window
    {
        public int TeamId { get; private set; }
        private readonly ITeamService _teamService;
        private TeamWindow _teamWindow;
        // Constructor với tham số cho service và teamId
        public InsideTeam(ITeamService teamService, int teamId, TeamWindow teamWindow)
        {
            InitializeComponent();
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            TeamId = teamId; // Lưu teamId
            LoadTeamUsers(TeamId); // Tải danh sách tác vụ cho teamId đã được truyền vào
            SetTeamName(TeamId);
            _teamWindow = teamWindow;
        }
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
            _teamWindow.Show();
        }
        public void SetTeamName(int teamID)
        {
            var team = _teamService.GetTeamById(teamID);
            TeamNameLabel.Content = "Team " + team.Name; // Cập nhật nội dung của Label
        }
        public void LoadTeamUsers(int teamID)
        {
            try
            {
                IEnumerable<User> members = _teamService.GetUsersInTeam(teamID);
                MemberListView.ItemsSource = members;
                MemberListView.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy UserId của thành viên được chọn từ CommandParameter
            int userIdToDelete = (int)((Button)sender).CommandParameter;

            // Hiển thị cửa sổ xác nhận xóa
            ConfirmationWindow confirmationWindow = new ConfirmationWindow();
            confirmationWindow.Owner = this;
            confirmationWindow.ShowDialog();

            // Kiểm tra nếu người dùng xác nhận xóa
            if (confirmationWindow.IsConfirmed)
            {
                try
                {
                    // Gọi service để xóa thành viên
                    _teamService.RemoveMemberFromTeam(TeamId, userIdToDelete);

                    // Hiển thị thông báo thành công
                    NotificationWindow notification = new NotificationWindow("Member deleted successfully!");
                    notification.ShowDialog();
                    MemberListView.Items.Refresh();
                    // Cập nhật danh sách thành viên sau khi xóa
                    LoadTeamUsers(TeamId);
                }
                catch (Exception ex)
                {
                    // Hiển thị thông báo lỗi
                    NotificationWindow notification = new NotificationWindow($"Error deleting member: {ex.Message}");
                    notification.ShowDialog();
                }
            }
            else
            {
                // Hiển thị thông báo khi không có task được chọn
                NotificationWindow notification = new NotificationWindow("No member selected for deletion.");
                notification.ShowDialog();
            }
           
        }
        private void NewMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // Khởi tạo cửa sổ thêm thành viên mới với các dịch vụ cần thiết
            Invitemember newMember = new Invitemember(new TeamService(new TeamRepository(new ToDoListContext())), TeamId);

            // Đăng ký sự kiện MemberAdded để cập nhật danh sách sau khi thành viên mới được thêm
            newMember.MemberAdded += (s, args) =>
            {
                var memberArgs = args as MemberAddedEventArgs;
                if (memberArgs != null)
                {
                    var newMember = memberArgs.NewMember;
                    MemberListView.Items.Refresh();
                    LoadTeamUsers(TeamId);
                }
            };

            newMember.ShowDialog();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTitle = MemberSearchBox.Text.Trim();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                try
                {
                    IEnumerable<User> member = await _teamService.GetUserByNameAsync(TeamId,searchTitle);
                    MemberListView.ItemsSource = member;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error searching tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadTeamUsers(TeamId);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ TaskWindow và truyền teamId
            TaskWindow taskWindow = new TaskWindow(new ToDoService(new ToDoRepository(new ToDoListContext())), TeamId);
            taskWindow.Show();
        }
        private void MemberSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTitle = MemberSearchBox.Text.Trim();
            if (string.IsNullOrEmpty(searchTitle))
            {
                LoadTeamUsers(TeamId);
                SearchPlaceholderLabel.Visibility = Visibility.Visible;
            }
            else
                SearchPlaceholderLabel.Visibility = Visibility.Collapsed;
        }

    }
}
