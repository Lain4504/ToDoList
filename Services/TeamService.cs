using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public Team GetTeamById(int teamId)
        {
            return _teamRepository.GetTeamById(teamId);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepository.GetAll();
        }

        public void CreateTeam(Team team, int adminUserId)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team));
            }
            var adminUser = _teamRepository.GetUserById(adminUserId);
            if (adminUser == null)
            {
                throw new Exception("Admin user not found");
            }
            team.AdminUserId = adminUser.UserId;
            team.Status = TeamStatus.ACTIVE;
            _teamRepository.CreateTeam(team);
        }
        public void UpdateTeam(Team team)
        {
            if (team == null)
            {
                throw new ArgumentNullException(nameof(team));
            }
            _teamRepository.UpdateTeam(team);
        }

        public void DeleteTeam(int teamId)
        {
            _teamRepository.DeleteTeam(teamId);
        }
        public async Task<IEnumerable<User>> GetMembersByTeam(int teamId)
        {
            var team = _teamRepository.GetTeamById(teamId);
            if (team == null)
                throw new Exception("Team not found");
            return await _teamRepository.GetMembersByTeamIdAsync(teamId);
        }
        public void AddMemberInTeam(int teamId, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var team = _teamRepository.GetTeamById(teamId);
            if (team == null)
                throw new Exception("Team not found");
            if (team.Members.Contains(user))
                throw new Exception("Team was contain user");
            team.Members.Add(user);
            _teamRepository.UpdateTeam(team);
        }
        public void RemoveMemberFromTeam(int teamId, User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var team = _teamRepository.GetTeamById(teamId);
            if (team == null)
                throw new Exception("Team not found");
            if (!team.Members.Contains(user))
                throw new Exception("Team didn't contain user");
            team.Members.Remove(user);
            _teamRepository.UpdateTeam(team);
        }
        public async Task UpdateTeamStatusAsync(int teamId, TeamStatus newStatus)
        {
            await _teamRepository.UpdateTeamStatusAsync(teamId, newStatus);
        }

        public async Task<bool> IsAdminUserAsync(int userId)
        {
            return await _teamRepository.IsAdminUser(userId);
        }
        public async Task<IEnumerable<Team>> GetTeamByNameAsync(string name)
        {
            return await _teamRepository.GetTeamByNameAsync(name);
        }
    }
}

