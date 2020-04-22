namespace Sverto.ServiceNow.ValueTypes
{
    public enum State
    {
        New = 0,
        Assigned = 2,
        WorkInProgress = 3,
        WaitingForChange = 4,
        WaitingForSupplier = 6,
        WaitingForUser = 8,
        SolutionRejected = 9,
        ClosedMail = 11,
        Closed = 12,
        Cancelled = 13
    }
}
