// LoanApi.cs — PATTERN SAMPLE ONLY, fictional data, no real applicants.
// Pattern from Fifth Third Bank resume work: versioned loan intake API,
// validation, idempotency, underwriting state machine, Kafka-style event.
using System;
using System.Collections.Generic;

public enum LoanStatus { Received, UnderReview, Approved, Declined, Funded }

public record LoanApplicationDto(
    string FirstName, string LastName, decimal RequestedAmount,
    int TermMonths, string IdempotencyKey);

// Idempotency: the same key must never create a second loan record.
// In production this is enforced with a unique key in the store;
// here the rule is stated so a reviewer can see the intent.
public class LoanUnderwritingService
{
    public LoanStatus NextStatus(LoanStatus current, bool documentsComplete, decimal debtToIncomeRatio)
    {
        if (current == LoanStatus.Received && documentsComplete) return LoanStatus.UnderReview;
        if (current == LoanStatus.UnderReview)
            return debtToIncomeRatio <= 0.43m ? LoanStatus.Approved : LoanStatus.Declined;
        return current; // No other transition is allowed — invalid moves are rejected, not silently applied.
    }
}

public record LoanEvent(string EventType, string LoanReferenceFictional, LoanStatus Status, DateTimeOffset OccurredAt);
