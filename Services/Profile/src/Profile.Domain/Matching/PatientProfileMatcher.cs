namespace Clinic.Profile.Domain.Matching;

public static class PatientProfileMatcher
{
    public const int FirstNameWeight = 5;
    public const int LastNameWeight = 5;
    public const int MiddleNameWeight = 5;
    public const int DateOfBirthWeight = 3;
    public const int MatchThreshold = 13;

    public static int CalculateScore(
        string firstNameA,
        string lastNameA,
        string? middleNameA,
        DateOnly dateOfBirthA,
        string firstNameB,
        string lastNameB,
        string? middleNameB,
        DateOnly dateOfBirthB)
    {
        var score = 0;

        if (NamesEqual(firstNameA, firstNameB))
            score += FirstNameWeight;

        if (NamesEqual(lastNameA, lastNameB))
            score += LastNameWeight;
        
        if (NamesEqual(middleNameA, middleNameB))
            score += MiddleNameWeight;

        if (dateOfBirthA == dateOfBirthB)
            score += DateOfBirthWeight;

        return score;
    }

    public static bool IsMatch(int score) => score >= MatchThreshold;

    private static bool NamesEqual(string? a, string? b)
    {
        var left = (a ?? string.Empty).Trim();
        var right = (b ?? string.Empty).Trim();
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}