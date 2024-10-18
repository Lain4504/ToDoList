using BusinessObjects;

namespace Repositories
{
    public interface ITeamRepository
    {
        Team GetTeamById(int teamId);
        IEnumerable<Team> GetAll();
        void AddTeam(Team team);
        void UpdateTeam(Team team);
        void DeleteTeam(int teamId);
        IEnumerable<User> GetMembers(int teamId);
        void AddMember(Team team, User user);
        void RemoveMember(Team team, User user);
    }
}
