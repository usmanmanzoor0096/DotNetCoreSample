namespace AuthService.Models.Models
{
    public class LogonStat
    {
        public string IP { get; set; }
        public int UserId { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string HostName { get; set; }
        public string SessionId { get; set; }
        public bool LoggedInAsUser { get; set; }
        public int? OriginalUserId { get; set; }
    }
}
