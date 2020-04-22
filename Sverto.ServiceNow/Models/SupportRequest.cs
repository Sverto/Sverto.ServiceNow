using Newtonsoft.Json;
using Sverto.ServiceNow.API;
using Sverto.ServiceNow.ValueTypes;

namespace Sverto.ServiceNow.Models
{
    public class SupportRequest : Record
    {
        public new static string TABLE_NAME { get; } = "u_support_request";

        [JsonProperty("number")] // SRxxxxxxx
        public string Number { get; set; }

        [JsonProperty("sys_created_by")] // KDM_SMALS
        public string CreatedBy { get; set; }

        [JsonProperty("opened_at")] // yyyy-MM-dd HH:mm:ss
        public string OpenedAt { get; set; }

        [JsonProperty("u_outage_start")] // yyyy-MM-dd HH:mm:ss
        public string OutageStart { get; set; }

        [JsonProperty("contact_type")] // mail
        public ContactType? ContactType { get; set; }

        [JsonProperty("category")] // event
        public string Category { get; set; } = "incident";

        [JsonProperty("state")]
        public State? State { get; set; }

        // Priority
        [JsonProperty("impact")]
        public Impact? Impact { get; set; }

        [JsonProperty("urgency")]
        public Urgency? Urgency { get; set; }

        [JsonProperty("priority")] // 40 (=1, 2, 4, 8, 16 or 40)
        public int? Priority { get; set; }

        [JsonProperty("company")]
        public ResourceLink Company { get; set; }

        [JsonProperty("company.name")]
        public string CompanyName { get; set; }

        [JsonProperty("u_invoicing_company")]
        public ResourceLink InvoicingCompany { get; set; }

        [JsonProperty("u_invoicing_company.name")]
        public string InvoicingCompanyName { get; set; }

        [JsonProperty("service_offering")]
        public ResourceLink Service { get; set; }

        [JsonProperty("service_offering.name")]
        public string ServiceName { get; set; }

        [JsonProperty("u_sub_service")]
        public ResourceLink Subservice { get; set; }

        [JsonProperty("u_sub_service.name")]
        public string SubserviceName { get; set; }

        [JsonProperty("caller_id")]
        public ResourceLink Caller { get; set; }

        [JsonProperty("caller_id.first_name")]
        public string CallerFirstName { get; set; }

        [JsonProperty("caller_id.last_name")]
        public string CallerLastName { get; set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("watch_list")] // (comma seprated list of ids or emails)
        public string WatchList { get; set; }

        [JsonProperty("active")]
        public bool? IsActive { get; set; }

        [JsonProperty("u_ticket_language")] // en
        public Language? Language { get; set; }

        [JsonProperty("assignment_group")]
        public ResourceLink AssignmentGroup { get; set; }

        [JsonProperty("assignment_group.name")]
        public string AssignmentGroupName { get; set; }

        [JsonProperty("assigned_to")]
        public ResourceLink AssignedTo { get; set; }

        [JsonProperty("u_rootcause")]
        public RootCause? RootCause { get; set; }

        [JsonProperty("u_internal_solution")]
        public string InternalSolution { get; set; }

        [JsonProperty("close_notes")]
        public string CloseNotes { get; set; }

        [JsonProperty("u_initial_first_line")]
        public ResourceLink OwnerFirstLine { get; set; }

        [JsonProperty("u_initial_first_line.name")]
        public string OwnerFirstLineName { get; set; }

        [JsonProperty("u_third_party_communication")]
        public string ThirdPartyCommunication { get; set; }

    }
}
