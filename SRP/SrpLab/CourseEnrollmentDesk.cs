namespace SrpLab;

public sealed class CourseEnrollmentDesk
{
    private readonly HashSet<string> _seated = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<string> _waitlist = new();
    public int Capacity { get; }
    public decimal Tuition { get; }
    public string CourseCode { get; }

    public CourseEnrollmentDesk(string courseCode, int capacity, decimal tuition)
    {
        CourseCode = courseCode;
        Capacity = capacity;
        Tuition = tuition;
    }

    public bool IsSeated(string studentEmail) => _seated.Contains(studentEmail);

    public string Register(string studentEmail)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email");
        var email = studentEmail.Trim();

        if (_seated.Contains(email) || _waitlist.Contains(email))
            return "ALREADY_REGISTERED";

        if (_seated.Count < Capacity)
        {
            _seated.Add(email);
            return "SEATED";
        }

        _waitlist.Add(email);
        return $"WAITLIST:{_waitlist.Count}";
    }

    public int WaitlistPosition(string studentEmail)
    {
        var idx = _waitlist.FindIndex(x => x.Equals(studentEmail, StringComparison.OrdinalIgnoreCase));
        return idx < 0 ? -1 : idx + 1;
    }

    public void PromoteFromWaitlist(int seats)
    {
        while (seats > 0 && _waitlist.Count > 0 && _seated.Count < Capacity)
        {
            var next = _waitlist[0];
            _waitlist.RemoveAt(0);
            _seated.Add(next);
            seats--;
        }
    }
}

public sealed class EnrollmentWelcomePacketFormatter
{
    public string GenerateMarkdown(CourseEnrollmentDesk desk, string studentEmail, string studentName)
    {
        var status = desk.IsSeated(studentEmail) ? "confirmed seat" : $"waitlist #{desk.WaitlistPosition(studentEmail)}";
        return $"# Welcome to {desk.CourseCode}\nHi {studentName},\nYour status: **{status}**.\n" +
               $"Bring a laptop. Discord onboarding link: https://example.invalid/{desk.CourseCode.ToLowerInvariant()}\n";
    }
}

public sealed class EnrollmentInvoiceFormatter
{
    public string GenerateInvoiceLine(CourseEnrollmentDesk desk, string studentEmail)
    {
        if (!desk.IsSeated(studentEmail)) return $"{desk.CourseCode},WAITLIST,0.00";
        var vat = Math.Round(desk.Tuition * 0.14m, 2);
        return $"{desk.CourseCode},TUITION,{desk.Tuition:0.00},VAT,{vat:0.00},TOTAL,{(desk.Tuition + vat):0.00}";
    }
}