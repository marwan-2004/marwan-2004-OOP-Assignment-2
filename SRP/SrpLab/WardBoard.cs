namespace SrpLab;

public sealed class WardBoard
{
    private readonly Dictionary<int, string> _bedPatient = new();
    private readonly Dictionary<int, int> _vitalsScore = new();
    private readonly ClinicalAcuityEvaluator _acuityEvaluator = new();
    private readonly PagerLogger _pagerLogger = new();

    public IReadOnlyDictionary<int, string> BedPatients => _bedPatient;
    public IReadOnlyDictionary<int, int> VitalsScores => _vitalsScore;

    public void AssignBed(int bed, string patientId, int heartRate, int spo2)
    {
        if (bed <= 0) throw new ArgumentOutOfRangeException(nameof(bed));
        if (string.IsNullOrWhiteSpace(patientId)) throw new ArgumentException("patient required");

        _bedPatient[bed] = patientId.Trim().ToUpperInvariant();
        var acuity = _acuityEvaluator.ScoreAcuity(heartRate, spo2);
        _vitalsScore[bed] = acuity;

        if (acuity >= 8)
            _pagerLogger.LogCodeYellow(bed);
    }

    public int ScoreAcuity(int heartRate, int spo2) => _acuityEvaluator.ScoreAcuity(heartRate, spo2);

    public IReadOnlyList<string> DrainPagerLog() => _pagerLogger.DrainPagerLog();
}

public sealed class ClinicalAcuityEvaluator
{
    public int ScoreAcuity(int heartRate, int spo2)
    {
        var score = 0;
        if (heartRate > 120 || heartRate < 45) score += 4;
        else if (heartRate > 100) score += 2;
        if (spo2 < 90) score += 5;
        else if (spo2 < 94) score += 2;
        return Math.Min(score, 10);
    }
}

public sealed class PagerLogger
{
    private readonly List<string> _pagerLog = new();

    public void LogCodeYellow(int bed)
    {
        _pagerLog.Add($"CODE-YELLOW bed={bed} at {DateTime.UtcNow:HH:mm}");
    }

    public IReadOnlyList<string> DrainPagerLog()
    {
        var copy = _pagerLog.ToList();
        _pagerLog.Clear();
        return copy;
    }
}

public sealed class NurseHandoffNoteFormatter
{
    public string BuildHandoffNote(WardBoard board, int bed)
    {
        if (!board.BedPatients.TryGetValue(bed, out var patient))
            return $"Bed {bed}: empty";

        var acuity = board.VitalsScores[bed];
        var tone = acuity >= 8 ? "ESCALATE" : acuity >= 4 ? "WATCH" : "STABLE";
        return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {patient} · acuity={acuity} · {tone}";
    }
}

public sealed class WardCensusExporter
{
    public string ExportCensusCsv(WardBoard board)
    {
        var lines = new List<string> { "bed,patient,acuity" };
        foreach (var bed in board.BedPatients.Keys.OrderBy(x => x))
            lines.Add($"{bed},{board.BedPatients[bed]},{board.VitalsScores[bed]}");
        return string.Join('\n', lines);
    }
}