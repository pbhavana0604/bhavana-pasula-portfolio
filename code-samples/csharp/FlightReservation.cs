// FlightReservation.cs — PATTERN SAMPLE ONLY, fictional data, no real passengers.
// Pattern from Southwest Airlines resume work: fare search, hold, change/cancel.
using System;

public enum ReservationStatus { Held, Confirmed, Changed, Cancelled }

public record FareOption(string FlightFictional, decimal Price, string Cabin);

public class ReservationService
{
    // A hold expires; expiry is part of the domain, not a UI timer.
    public bool IsHoldExpired(DateTimeOffset holdExpiresAt, DateTimeOffset now)
        => now > holdExpiresAt;

    public ReservationStatus Cancel(ReservationStatus current) => current switch
    {
        ReservationStatus.Held or ReservationStatus.Confirmed => ReservationStatus.Cancelled,
        _ => current // Already changed/cancelled: cancelling again is a no-op, not an error that corrupts state.
    };
}
