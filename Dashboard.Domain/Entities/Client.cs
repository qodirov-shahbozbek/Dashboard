namespace Dashboard.Domain.Entities
{
    public class Client
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
    }
}
