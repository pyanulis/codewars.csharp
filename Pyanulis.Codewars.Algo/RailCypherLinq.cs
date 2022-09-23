namespace Pyanulis.Codewars.Algo;

internal class RailCypherLinq
{
    public static string Encode(string s, int n)
    {
        var mod = (n - 1) * 2;
        return string.Concat(s.Select((c, i) => new { c, i }).OrderBy(a => Math.Min(a.i % mod, mod - a.i % mod)).Select(a => a.c));
    }

    public static string Decode(string s, int n)
    {
        var mod = (n - 1) * 2;
        var pattern = Enumerable.Range(0, s.Length).OrderBy(i => Math.Min(i % mod, mod - i % mod));
        return string.Concat(s.Zip(pattern, (c, i) => new { c, i }).OrderBy(a => a.i).Select(a => a.c));
    }
}