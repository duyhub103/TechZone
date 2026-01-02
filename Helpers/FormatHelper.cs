using System.Globalization;

public static class FormatHelper
{
    public static string ToVnd(this decimal amount)
    {
        var culture = CultureInfo.GetCultureInfo("vi-VN");
        return amount.ToString("C0", culture);
    }
}
