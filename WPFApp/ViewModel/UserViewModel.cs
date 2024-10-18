using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WPFApp.ViewModel
{
    public class UserViewModel
    {
        public ObservableCollection<Team> Teams { get; set; } = new ObservableCollection<Team>();

        public async Task LoadUserTeams(int userId)
        {
            using var client = new HttpClient();
            try
            {
                var response = await client.GetAsync($"https://yourapiurl/api/user/{userId}/teams");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var teams = JsonConvert.DeserializeObject<List<Team>>(jsonString);
                    Teams = new ObservableCollection<Team>(teams);
                }
                else
                {
                    
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public class Team
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
    }
}
