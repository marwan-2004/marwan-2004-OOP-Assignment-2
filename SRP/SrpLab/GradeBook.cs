namespace SrpLab;

public sealed class GradeBook
{
    private readonly Dictionary<string, List<decimal>> _scores = new(StringComparer.OrdinalIgnoreCase);

    public IEnumerable<string> StudentIds => _scores.Keys;

    public void Record(string studentId, decimal score)
    {
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
        if (!_scores.TryGetValue(studentId, out var list))
        {
            list = new List<decimal>();
            _scores[studentId] = list;
        }
        list.Add(score);
    }

    public decimal Average(string studentId)
    {
        if (!_scores.TryGetValue(studentId, out var list) || list.Count == 0) return 0m;
        return Math.Round(list.Average(), 2);
    }
}

public sealed class AcademicPolicyEvaluator
{
    public string Letter(decimal average)
    {
        if (average >= 90) return "A";
        if (average >= 80) return "B";
        if (average >= 70) return "C";
        if (average >= 60) return "D";
        return "F";
    }

    public bool MeetsHonorRoll(decimal average)
    {
        var letter = Letter(average);
        return average >= 85 && letter is "A" or "B";
    }
}

public sealed class TranscriptFormatter
{
    private readonly AcademicPolicyEvaluator _policy = new();

    public string FormatPlain(GradeBook gradeBook, string studentId, string fullName)
    {
        var avg = gradeBook.Average(studentId);
        var letter = _policy.Letter(avg);
        var honor = _policy.MeetsHonorRoll(avg);
        return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\nAverage: {avg}\nLetter: {letter}\nHonor: {honor}\n";
    }
}

public sealed class GradeBookCsvExporter
{
    private readonly AcademicPolicyEvaluator _policy = new();

    public string ExportCsv(GradeBook gradeBook)
    {
        var rows = new List<string> { "studentId,average,letter,honor" };
        foreach (var id in gradeBook.StudentIds.OrderBy(x => x))
        {
            var avg = gradeBook.Average(id);
            var letter = _policy.Letter(avg);
            var honor = _policy.MeetsHonorRoll(avg);
            rows.Add($"{id},{avg},{letter},{(honor ? 1 : 0)}");
        }
        return string.Join('\n', rows);
    }
}