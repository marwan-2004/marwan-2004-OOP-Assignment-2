namespace SrpLab;

public sealed class KitchenTicket
{
    private readonly List<(string Item, List<string> Ingredients, int PrepMinutes)> _items = new();

    public IReadOnlyList<(string Item, List<string> Ingredients, int PrepMinutes)> Items => _items;

    public void AddItem(string item, IEnumerable<string> ingredients, int prepMinutes)
    {
        _items.Add((item, ingredients.Select(i => i.Trim().ToLowerInvariant()).ToList(), prepMinutes));
    }
}

public sealed class AllergenDetector
{
    public IReadOnlyList<string> DetectAllergens(KitchenTicket ticket)
    {
        var hits = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (_, ingredients, _) in ticket.Items)
        {
            foreach (var ing in ingredients)
            {
                if (ing.Contains("milk") || ing.Contains("cheese") || ing.Contains("butter")) hits.Add("dairy");
                if (ing.Contains("wheat") || ing.Contains("flour") || ing.Contains("bread")) hits.Add("gluten");
                if (ing.Contains("peanut") || ing.Contains("almond") || ing.Contains("cashew")) hits.Add("nuts");
                if (ing.Contains("shrimp") || ing.Contains("prawn") || ing.Contains("crab")) hits.Add("shellfish");
            }
        }
        return hits.OrderBy(x => x).ToList();
    }
}

public sealed class KitchenEtaCalculator
{
    private readonly AllergenDetector _allergenDetector = new();

    public int EstimatedReadyMinutes(KitchenTicket ticket, int openStations)
    {
        if (openStations <= 0) openStations = 1;
        var sequential = ticket.Items.Sum(i => i.PrepMinutes);
        var parallel = (int)Math.Ceiling(sequential / (double)openStations);
        if (_allergenDetector.DetectAllergens(ticket).Count > 0) parallel += 3;
        var longest = ticket.Items.Count == 0 ? 0 : ticket.Items.Max(i => i.PrepMinutes);
        return Math.Max(parallel, longest);
    }
}

public sealed class ThermalTicketRenderer
{
    private readonly AllergenDetector _allergenDetector = new();
    private readonly KitchenEtaCalculator _etaCalculator = new();

    public string RenderThermalTicket(KitchenTicket ticket, int orderNumber)
    {
        var width = 32;
        var line = new string('=', width);
        var body = string.Join('\n', ticket.Items.Select(i => $"* {i.Item.ToUpperInvariant()} ({i.PrepMinutes}m)"));
        var allergens = _allergenDetector.DetectAllergens(ticket);
        var allergyLine = allergens.Count == 0 ? "ALLERGENS: none" : "ALLERGENS: " + string.Join(",", allergens);
        return $"{line}\nORDER #{orderNumber}\nETA {_etaCalculator.EstimatedReadyMinutes(ticket, 2)} MIN\n{body}\n{allergyLine}\n{line}\n";
    }
}

public sealed class ExpoLaneRouter
{
    private readonly AllergenDetector _allergenDetector = new();
    private readonly KitchenEtaCalculator _etaCalculator = new();

    public string ExpoLaneHint(KitchenTicket ticket)
    {
        return _allergenDetector.DetectAllergens(ticket).Count > 0
            ? "LANE-ALLERGY"
            : _etaCalculator.EstimatedReadyMinutes(ticket, 2) > 20
                ? "LANE-SLOW"
                : "LANE-FAST";
    }
}