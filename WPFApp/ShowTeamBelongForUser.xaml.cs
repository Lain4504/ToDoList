using System.Windows;
using WPFApp.ViewModel;

namespace WPFApp
{
    public partial class ShowTeamBelongForUser : Window
    {
        private UserViewModel userViewModel;

        public ShowTeamBelongForUser()
        {
            InitializeComponent();
            userViewModel = new UserViewModel();
            DataContext = userViewModel; // Set the DataContext to the instance
        }

        private async void LoadTeamsButton_Click(object sender, RoutedEventArgs e)
        {
            int userId = 1; // Replace with the actual user ID or get it from user input
            await userViewModel.LoadUserTeams(userId);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Logic to go back or close the window
        }
    }
}
