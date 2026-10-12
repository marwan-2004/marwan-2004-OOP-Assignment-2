using System.Diagnostics;
using PatternsLab.Problems.Builder;
using PatternsLab.Problems.Prototype;
using PatternsLab.Problems.Singleton;

Console.WriteLine("=== SINGLETON: AFTER ===\n");
var db = new DatabaseService();
var ui = new UiService();
Console.WriteLine();

db.Config.Theme = "Dark";
Console.WriteLine("Admin changed theme to Dark.");
ui.Render(); // Now prints Dark!

Console.WriteLine($"\nSame config object? {ReferenceEquals(db.Config, ui.Config)}"); // True
Console.WriteLine($"Times config was loaded from disk: {AppConfig.LoadCount}"); // 1

Console.WriteLine("\n=== PROTOTYPE: AFTER ===\n");

var prototypeOrc = new Orc(); // Initial slow load once
var sw = Stopwatch.StartNew();
var army = new List<Enemy>();

for (int i = 1; i <= 5; i++)
{
    var orc = prototypeOrc.Clone(); // Fast clone
    orc.Name = $"Orc-{i}";
    army.Add(orc);
}
Console.WriteLine($"Created 5 orcs in {sw.ElapsedMilliseconds} ms (Fast!)\n");

Enemy original = prototypeOrc;
Enemy copy = original.Clone();

Console.WriteLine($"Original model id: {original.ModelId}");
Console.WriteLine($"Copy model id:     {copy.ModelId}"); // Matches original

copy.Weapon.Damage = 999;
Console.WriteLine($"\nWe changed the COPY's weapon damage to 999.");
Console.WriteLine($"Original's weapon damage is now: {original.Weapon.Damage}"); // Stays 25 (Deep copy)

copy.Abilities.Add("Fire Breath");
Console.WriteLine($"Original abilities: {string.Join(", ", original.Abilities)}"); // Unaffected

Console.WriteLine("\n=== BUILDER: AFTER ===\n");

// Built using fluent builder
var liveReg = RegistrationCallSites.CreateLiveStudentUgly();
var videoReg = RegistrationCallSites.CreateVideosOnlyUgly();

Console.WriteLine(liveReg);
Console.WriteLine(videoReg);

// Proving validation inside Build()
Console.WriteLine("\n--- Testing Builder Validation Rules ---");
try
{
    var invalidLive = CourseRegistrationBuilder
        .Create("test@mail.com", "SEF-101", "LiveGroup")
        .Build(); // Missing GroupCode!
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Exception]: {ex.Message}");
}

try
{
    var invalidVideo = CourseRegistrationBuilder
        .Create("test@mail.com", "SEF-101", "VideosOnly")
        .WithGroupCode("G1") // Invalid for VideosOnly!
        .Build();
}
catch (Exception ex)
{
    Console.WriteLine($"[Expected Exception]: {ex.Message}");
}