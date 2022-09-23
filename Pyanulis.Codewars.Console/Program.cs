// See https://aka.ms/new-console-template for more information
using Pyanulis.Codewars.Algo;
using System.Diagnostics;
using System.Numerics;

//string text = "wearediscoveredfleeatonce";
//int layers = 3;
//string cypher = RailCypher.Cypher(text, layers);
//Console.WriteLine(cypher);
//Console.ReadLine();
//Console.WriteLine(RailCypher.Decypher(cypher, layers));
//Console.ReadLine();

Stopwatch sw = new Stopwatch();

while (true)
{
    string substring = Console.ReadLine();
    bool strong = substring.EndsWith("s");
    substring = substring.Trim('s').Trim();

    if (substring.EndsWith("p"))
    {
        substring = substring.Trim('p').Trim();
        long index = DigitSubstring.GetLengthBefore(long.Parse(substring));
        Console.WriteLine(index);
        continue;
    }

    if (substring.Contains("f"))
    {
        substring = substring.Trim('f').Trim();
        long index = DigitSubstring.FindByChunks(substring);
        Console.WriteLine(index);
        continue;
    }

    if (substring.Contains("a"))
    {
        substring = substring.Trim('a').Trim();
        long index = DigitSubstring.FindByChunksE(substring);
        Console.WriteLine(index);
        continue;
    }

    if (substring.Contains("n"))
    {
        substring = substring.Trim('n').Trim();
        long number = DigitSubstring.GetNumAt(long.Parse(substring));
        Console.WriteLine(number);
        continue;
    }

    if (substring.Contains("cm"))
    {
        try
        {
            string[] vs = substring.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            substring = substring.Replace("cm", "").Trim();

            for (int d = 1; d < substring.Length; d++)
            {
                List<IEnumerable<ReadOnlyMemory<char>>> lists = DigitSubstring.GetAllChunks(vs[0].Replace("cm", "").Trim(), d);
                foreach (var list in lists)
                {
                    List<long> nums = new List<long>(list.Select(c => c.ToNum()));
                    foreach (var number in nums)
                    {
                        Console.Write($"{number} ");
                    }
                    Console.WriteLine();
                    Console.WriteLine(DigitSubstring.GetMinWithOffset(list, d));
                    Console.WriteLine();
                }
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        continue;
    }

    if (substring.Contains("c"))
    {
        try
        {
            string[] vs = substring.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            substring = substring.Trim('c').Trim();
            List<IEnumerable<ReadOnlyMemory<char>>> lists = DigitSubstring.GetAllChunks(vs[0].Trim('c').Trim(), int.Parse(vs[1]));
            foreach (var list in lists)
            {
                List<long> nums = new List<long>(list.Select(c => c.ToNum()));
                foreach (var number in nums)
                {
                    Console.Write($"{number} ");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        continue;
    }

    if (strong)
    {
        Console.WriteLine("Strong check:");
        sw.Restart();
        Console.WriteLine(DigitSubstring.FindPosition(substring).ToString() + " " + sw.Elapsed);
    }
    else
    {
        sw.Restart();
        (long min, int offset) result = DigitSubstring.GetMin(substring);
        Console.WriteLine(result.ToString() + " " + sw.Elapsed);

        sw.Restart();
        Console.WriteLine(DigitSubstring.FindPositionE(substring).ToString() + " " + sw.Elapsed);
    }

    Console.WriteLine();
}
