namespace SrpLab;

public sealed class LoanDesk
{
    public decimal RequestedAmount { get; }
    public int CreditScore { get; }
    public int EmploymentMonths { get; }
    public bool HasCollateral { get; }

    public LoanDesk(decimal requestedAmount, int creditScore, int employmentMonths, bool hasCollateral)
    {
        RequestedAmount = requestedAmount;
        CreditScore = creditScore;
        EmploymentMonths = employmentMonths;
        HasCollateral = hasCollateral;
    }

    public decimal RiskScore()
    {
        decimal score = 100m;
        score -= Math.Max(0, 700 - CreditScore) * 0.15m;
        if (EmploymentMonths < 6) score -= 20m;
        if (RequestedAmount > 50_000m && !HasCollateral) score -= 25m;
        if (RequestedAmount > 150_000m) score -= 10m;
        return Math.Clamp(score, 0m, 100m);
    }

    public bool IsEligible() => RiskScore() >= 55m && CreditScore >= 580;
}

public sealed class LoanComplianceChecker
{
    public IReadOnlyList<string> RequiredDocuments(LoanDesk desk)
    {
        var docs = new List<string> { "National ID", "Proof of income (3 months)" };
        if (desk.RequestedAmount > 40_000m) docs.Add("Bank statements (6 months)");
        if (desk.HasCollateral) docs.Add("Collateral ownership deed");
        if (desk.EmploymentMonths < 12) docs.Add("Employer letter");
        if (!desk.IsEligible()) docs.Add("Manual underwriter referral form");
        return docs;
    }
}

public sealed class LoanDecisionLetterFormatter
{
    private readonly LoanComplianceChecker _complianceChecker = new();

    public string DecisionLetter(LoanDesk desk, string applicantName)
    {
        if (desk.IsEligible())
        {
            return $"Dear {applicantName},\nYour request for {desk.RequestedAmount:C} is pre-approved (risk {desk.RiskScore():0}).\n" +
                   $"Please upload: {string.Join("; ", _complianceChecker.RequiredDocuments(desk))}.\n";
        }

        return $"Dear {applicantName},\nWe are unable to approve {desk.RequestedAmount:C} at this time.\n" +
               $"Reference risk={desk.RiskScore():0}. You may reapply after improving documentation.\n";
    }
}

public sealed class UnderwriterCsvExporter
{
    public string UnderwriterCsvRow(LoanDesk desk, string applicationId)
    {
        return $"{applicationId},{desk.CreditScore},{desk.EmploymentMonths},{(desk.HasCollateral ? 1 : 0)},{desk.RiskScore():0.00},{(desk.IsEligible() ? "Y" : "N")}";
    }
}