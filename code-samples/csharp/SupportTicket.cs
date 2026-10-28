// SupportTicket.cs — PATTERN SAMPLE ONLY, fictional data.
// Pattern from Hexaware resume work: ticket lifecycle, SLA, audit history.
using System;
using System.Collections.Generic;

public enum TicketPriority { Low, Medium, High, Critical }
public enum TicketStatus { Open, Assigned, InProgress, Resolved, Closed }

public record TicketAuditEntry(DateTimeOffset At, string Action, string ActorRole);

public class TicketService
{
    // SLA deadline is calculated from priority at creation — a pure function,
    // easy to unit-test, the same style as the eligibility rules in the demo.
    public DateTimeOffset SlaDeadline(TicketPriority priority, DateTimeOffset createdAt) => priority switch
    {
        TicketPriority.Critical => createdAt.AddHours(4),
        TicketPriority.High => createdAt.AddHours(24),
        TicketPriority.Medium => createdAt.AddDays(3),
        _ => createdAt.AddDays(5)
    };

    public bool IsEscalated(TicketStatus status, DateTimeOffset slaDeadline, DateTimeOffset now)
        => status is TicketStatus.Open or TicketStatus.Assigned or TicketStatus.InProgress
           && now > slaDeadline;
}
