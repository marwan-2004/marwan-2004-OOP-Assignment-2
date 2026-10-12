namespace PatternsLab.Problems.Builder;

public sealed class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public string AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    public CourseRegistration(
        string studentEmail,
        string courseCode,
        string accessMode,
        string? groupCode,
        string? discountCode,
        bool sendWhatsApp,
        bool sendEmailWelcome,
        string? mentorNote,
        DateOnly? preferredStart)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email required");
        if (string.IsNullOrWhiteSpace(courseCode)) throw new ArgumentException("course required");

        if (accessMode == "LiveGroup" && string.IsNullOrWhiteSpace(groupCode))
            throw new InvalidOperationException("LiveGroup requires GroupCode");
        if (accessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(groupCode))
            throw new InvalidOperationException("VideosOnly cannot have GroupCode");

        StudentEmail = studentEmail;
        CourseCode = courseCode;
        AccessMode = accessMode;
        GroupCode = groupCode;
        DiscountCode = discountCode;
        SendWhatsApp = sendWhatsApp;
        SendEmailWelcome = sendEmailWelcome;
        MentorNote = mentorNote;
        PreferredStart = preferredStart;
    }

    public override string ToString()
        => $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";
}

public class CourseRegistrationBuilder
{
    private readonly string _studentEmail;
    private readonly string _courseCode;
    private readonly string _accessMode;
    private string? _groupCode;
    private string? _discountCode;
    private bool _sendWhatsApp;
    private bool _sendEmailWelcome;
    private string? _mentorNote;
    private DateOnly? _preferredStart;

    // Required parameters enforced at builder initialization (R2)
    public CourseRegistrationBuilder(string studentEmail, string courseCode, string accessMode)
    {
        _studentEmail = studentEmail;
        _courseCode = courseCode;
        _accessMode = accessMode;
    }

    public static CourseRegistrationBuilder Create(string studentEmail, string courseCode, string accessMode)
    {
        return new CourseRegistrationBuilder(studentEmail, courseCode, accessMode);
    }

    // Optional steps configured via fluent methods (R1)
    public CourseRegistrationBuilder WithGroupCode(string? groupCode)
    {
        _groupCode = groupCode;
        return this;
    }

    public CourseRegistrationBuilder WithDiscountCode(string? discountCode)
    {
        _discountCode = discountCode;
        return this;
    }

    public CourseRegistrationBuilder WithWhatsApp(bool send = true)
    {
        _sendWhatsApp = send;
        return this;
    }

    public CourseRegistrationBuilder WithEmailWelcome(bool send = true)
    {
        _sendEmailWelcome = send;
        return this;
    }

    public CourseRegistrationBuilder WithMentorNote(string? note)
    {
        _mentorNote = note;
        return this;
    }

    public CourseRegistrationBuilder WithPreferredStart(DateOnly? start)
    {
        _preferredStart = start;
        return this;
    }

    // Build() validates business rules before returning object (R3)
    public CourseRegistration Build()
    {
        if (string.IsNullOrWhiteSpace(_studentEmail))
            throw new InvalidOperationException("Student email is required.");
        if (string.IsNullOrWhiteSpace(_courseCode))
            throw new InvalidOperationException("Course code is required.");

        if (_accessMode == "LiveGroup" && string.IsNullOrWhiteSpace(_groupCode))
            throw new InvalidOperationException("LiveGroup requires GroupCode.");

        if (_accessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(_groupCode))
            throw new InvalidOperationException("VideosOnly cannot have GroupCode.");

        return new CourseRegistration(
            _studentEmail,
            _courseCode,
            _accessMode,
            _groupCode,
            _discountCode,
            _sendWhatsApp,
            _sendEmailWelcome,
            _mentorNote,
            _preferredStart);
    }
}

public static class RegistrationCallSites
{
    
    public static CourseRegistration CreateLiveStudentUgly()
    {
        return CourseRegistrationBuilder
            .Create("sara@mail.com", "SEF-101", "LiveGroup")
            .WithGroupCode("G1")
            .WithDiscountCode("EARLY10")
            .WithWhatsApp()
            .WithEmailWelcome()
            .WithMentorNote("Needs evening slot")
            .WithPreferredStart(new DateOnly(2026, 10, 1))
            .Build();
    }

    public static CourseRegistration CreateVideosOnlyUgly()
    {
        return CourseRegistrationBuilder
            .Create("ali@mail.com", "SEF-101", "VideosOnly")
            .WithEmailWelcome()
            .Build();
    }
}