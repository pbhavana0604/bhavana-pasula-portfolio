// HealthcareDashboard.cs — PATTERN SAMPLE ONLY, fictional data, no PHI.
// Pattern from UCLA Health resume work: capacity dashboard + FHIR R4 summary
// + SignalR-style live updates + AI summary that MUST be human-reviewed.
// This file is illustrative; the runnable, tested code is in src/IowaBenefitsPortal/.
using System;
using System.Collections.Generic;

public record CapacityUnit(string UnitName, int TotalBeds, int OccupiedBeds)
{
    public int AvailableBeds => TotalBeds - OccupiedBeds;
    public double OccupancyRate => TotalBeds == 0 ? 0 : (double)OccupiedBeds / TotalBeds;
}

// FHIR R4-style summary shape. Real identifiers are never put in samples.
public record FhirPatientSummary(string FictionalId, string DisplayName, IReadOnlyList<string> Conditions);

public class CapacityService
{
    // Pure, testable rule — the same separation used in EligibilityService.
    public string OccupancyBand(CapacityUnit unit) => unit.OccupancyRate switch
    {
        >= 0.95 => "Critical",
        >= 0.85 => "High",
        _ => "Normal"
    };
}

public record AiSummaryRequest(string FictionalPatientId, string ClinicalNotesFictional);
public record AiSummaryResult(string DraftSummary, bool RequiresHumanReview = true);
// Rule enforced in code, not just in docs: an AI draft is never shown as fact
// until a clinician reviews it. See docs/ARCHITECTURE.md for the same
// "groundedness + human review" note used for RAG summaries.
