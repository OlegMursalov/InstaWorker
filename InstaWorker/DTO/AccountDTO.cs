namespace InstaWorker.DTO
{
    public class AccountDTO
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
        public bool IsAuthorized { get; set; }
    }
}