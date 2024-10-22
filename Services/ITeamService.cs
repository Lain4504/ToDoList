using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ITeamService
    {
        Team GetTeamById(int teamId);
        IEnumerable<Team> GetAllTeams();
        void CreateTeam(Team team, int adminUserId);
        void UpdateTeam(Team team);
        void DeleteTeam(int teamId);
        void AddMemberInTeam(int teamId, User user);
        void RemoveMemberFromTeam(int teamId, User user);
        Task UpdateTeamStatusAsync(int teamId, TeamStatus newStatus);
        Task<bool> IsAdminUserAsync(int userId);
        Task<IEnumerable<Team>> GetTeamByNameAsync(string name);
    }
}
