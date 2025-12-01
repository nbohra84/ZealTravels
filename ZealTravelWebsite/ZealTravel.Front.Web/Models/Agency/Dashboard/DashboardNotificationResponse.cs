using DocumentFormat.OpenXml.Wordprocessing;

namespace ZealTravel.Front.Web.Models.Agency.Dashboard
{
    public class DashboardNotificationResponse
    {
        public string Description { get; set; }
        public string? Subject { get; set; }
        public string? Link { get; set; }
    }
}
