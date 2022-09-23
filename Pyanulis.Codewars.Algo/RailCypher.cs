namespace Pyanulis.Codewars.Algo;

internal class RailCypher
{
    public static string Cypher(string s, int n)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        if (n == 1)
        {
            return s;
        }

        List<string> layers = GetLayers(s, n);
        return string.Join("", layers);
    }

    public static string Decypher(string s, int n)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }

        if (n == 1)
        {
            return s;
        }

        // Since lengths of the original and cypher texts are the same, cypher algorithm can be used to find out length of every layer.
        List<string> layers = GetLayers(s, n);

        // Now we know length of substrings to get real cypher layers.
        int[] indices = new int[layers.Count];
        int start = 0;
        for (int i = 0; i != layers.Count; i++)
        {
            indices[i] = 0;
            int length = layers[i].Length;
            layers[i] = s.Substring(start, length);
            start += length;
        }

        string decypher = string.Empty;
        bool forward = true;
        int layer = 1;
        for (int i = 0; i < s.Length; i++)
        {
            decypher += layers[layer - 1][indices[layer - 1]++];
            UpdateLayer(n, ref layer, ref forward);
        }

        return decypher;
    }

    private static List<string> GetLayers(string s, int n)
    {
        if (string.IsNullOrEmpty(s))
        {
            return new List<string>();
        }

        if (n == 1)
        {
            return new List<string>() { s };
        }

        string cypher = string.Empty;

        bool forward = true;
        int layer = 1;
        List<string> layers = new List<string>();
        for (int i = 0; i < n; i++)
        {
            layers.Add(string.Empty);
        }
        for (int i = 0; i < s.Length; i++)
        {
            layers[layer - 1] += s[i];
            UpdateLayer(n, ref layer, ref forward);
        }

        return layers;
    }

    private static void UpdateLayer(int n, ref int layer, ref bool forward)
    {
        layer = forward ? ++layer : --layer;
        forward = (forward && layer < n) || (!forward && layer == 1);
    }
}
