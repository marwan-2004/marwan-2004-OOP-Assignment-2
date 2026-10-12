namespace SrpLab;

public sealed class WarehousePickList
{
    private readonly List<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> _lines = new();

    public IReadOnlyList<(string Sku, string Aisle, int Bin, int QtyNeeded, int QtyOnHand)> Lines => _lines;

    public void AddNeed(string sku, string aisle, int bin, int qtyNeeded, int qtyOnHand)
    {
        _lines.Add((sku, aisle, bin, qtyNeeded, qtyOnHand));
    }

    public IReadOnlyList<(string Sku, int Allocated)> Allocate()
    {
        var result = new List<(string, int)>();
        foreach (var line in _lines)
        {
            var alloc = Math.Min(line.QtyNeeded, line.QtyOnHand);
            result.Add((line.Sku, alloc));
        }
        return result;
    }
}

public sealed class WarehousePathOptimizer
{
    public IReadOnlyList<(string Aisle, int Bin, string Sku, int Qty)> WalkingOrder(WarehousePickList pickList)
    {
        return pickList.Lines
            .OrderBy(l => l.Aisle)
            .ThenBy(l => l.Bin)
            .Select(l => (l.Aisle, l.Bin, l.Sku, Math.Min(l.QtyNeeded, l.QtyOnHand)))
            .Where(x => x.Item4 > 0)
            .ToList();
    }
}

public sealed class HandheldPickerScriptFormatter
{
    private readonly WarehousePathOptimizer _pathOptimizer = new();

    public string PickerScript(WarehousePickList pickList)
    {
        var steps = _pathOptimizer.WalkingOrder(pickList)
            .Select((s, i) => $"{i + 1}. Go aisle {s.Aisle} bin {s.Bin}: pick {s.Qty} × {s.Sku}");
        var allocations = pickList.Allocate();
        var shortfalls = allocations.Where(a =>
        {
            var need = pickList.Lines.First(l => l.Sku == a.Sku).QtyNeeded;
            return a.Allocated < need;
        });
        var warn = shortfalls.Any()
            ? "SHORTAGES: " + string.Join(", ", shortfalls.Select(s => s.Sku))
            : "SHORTAGES: none";
        return string.Join('\n', steps) + "\n" + warn;
    }
}

public sealed class WmsXmlBatchExporter
{
    public string WmsXmlBatch(WarehousePickList pickList, string batchId)
    {
        var parts = pickList.Allocate().Select(a => $"<line sku=\"{a.Sku}\" qty=\"{a.Allocated}\" />");
        return $"<batch id=\"{batchId}\">{string.Join("", parts)}</batch>";
    }
}