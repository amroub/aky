namespace aky.Foundation.Utility.Logging.Model
{
    using System.Security.Claims;

    public class ApplicationLog
    {
        public string Application { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
