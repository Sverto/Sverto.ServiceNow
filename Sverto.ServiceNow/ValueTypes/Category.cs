namespace Sverto.ServiceNow.ValueTypes
{
    public static class Category
    {
        public static string Incident { get; } = "incident";
        public static string SecurityThread { get; } = "security";
        public static string Event { get; } = "event";
        public static string Complaint { get; } = "complaint";
        public static string RequestForInformation { get; } = "request_info";
    }
}
