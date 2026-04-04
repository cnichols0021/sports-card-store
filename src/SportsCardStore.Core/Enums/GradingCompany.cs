namespace SportsCardStore.Core.Enums;

/// <summary>
/// Represents the grading company that assessed the card condition
/// </summary>
public enum GradingCompany
{
    Raw = 0,    // Ungraded card
    PSA = 1,    // Professional Sports Authenticator
    BGS = 2,    // Beckett Grading Services
    SGC = 3     // Sportscard Guaranty Corporation
}