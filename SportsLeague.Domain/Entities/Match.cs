using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Match : AuditBase
    {
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; } = null!;

        public int RefereeId { get; set; }
        public Referee Referee { get; set; } = null!;

        public DateTime MatchDate { get; set; }
        public MatchStatus Status { get; set; } = MatchStatus.Scheduled;
    }
}
