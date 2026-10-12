namespace SrpLab;

public sealed class CheckoutBasket
{
    private readonly List<(string Sku, decimal Price, int Qty)> _lines = new();
    private readonly CouponParser _couponParser = new();
    private string? _couponRaw;
    private bool _giftWrap;

    public IReadOnlyList<(string Sku, decimal Price, int Qty)> Lines => _lines;

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
        _lines.Add((sku, price, qty));
    }

    public void ApplyCouponText(string? couponText) => _couponRaw = couponText;
    public void EnableGiftWrap() => _giftWrap = true;

    public decimal SubTotal() => _lines.Sum(l => l.Price * l.Qty);

    public decimal DiscountAmount() => _couponParser.CalculateDiscount(_couponRaw, SubTotal());

    public decimal GrandTotal()
    {
        var total = SubTotal() - DiscountAmount();
        if (_giftWrap) total += 4.99m;
        return Math.Max(0m, total);
    }
}

public sealed class CouponParser
{
    public decimal CalculateDiscount(string? couponRaw, decimal subTotal)
    {
        if (string.IsNullOrWhiteSpace(couponRaw)) return 0m;
        var t = couponRaw.Trim().ToUpperInvariant();
        if (t.StartsWith("SAVE") && int.TryParse(t[4..], out var pct) && pct is > 0 and <= 50)
            return Math.Round(subTotal * pct / 100m, 2);
        if (t.Contains("FREESHIP")) return 0m;
        if (t == "WELCOME10") return Math.Min(10m, subTotal);
        return 0m;
    }
}

public sealed class GiftCardFormatter
{
    public string FormatCard(CheckoutBasket basket, string fromName)
    {
        var items = string.Join(", ", basket.Lines.Select(l => l.Sku));
        return $"Dear friend,\nA gift from {fromName} awaits ({items}).\nTotal surprise value: {basket.GrandTotal():C}\n";
    }
}

public sealed class PaymentAuthorizerStub
{
    public string AuthorizePayment(CheckoutBasket basket, string cardLast4)
    {
        var payload = $"{basket.GrandTotal():0.00}|{cardLast4}|{basket.Lines.Count}";
        var hash = payload.GetHashCode();
        return $"AUTH-{Math.Abs(hash):X8}";
    }
}