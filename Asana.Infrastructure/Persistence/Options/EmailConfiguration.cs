namespace Asana.Infrastructure.Persistence.Options
{
    public class EmailOptions
    {

        public const string Email = "Email";


        public string From { get; set; }

        public string Name { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
