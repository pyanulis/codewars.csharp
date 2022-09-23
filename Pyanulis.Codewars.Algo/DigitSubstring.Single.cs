using MemChunk = System.ReadOnlyMemory<char>;
using MemChunkList = System.Collections.Generic.IEnumerable<System.ReadOnlyMemory<char>>;

namespace Pyanulis.Codewars.Algo;
public partial class DigitSubstring
{
    public static long FindPosition(string substring)
    {
        if (!long.TryParse(substring, out long num))
        {
            return -1;
        }

        long pos = 0;
        int start = 1;
        string increment = string.Empty;
        for (long i = start; i < long.MaxValue; i++)
        {
            string newNum = i.ToString();
            increment += newNum;
            if (increment.Length > (substring.Length + 4))
            {
                int offset = increment.Length - (substring.Length + 4);
                increment = increment.Substring(offset);
                pos += offset;
            }
            if (increment.Contains(substring))
            {
                Console.WriteLine($"Iterations: {i}");
                return pos + increment.IndexOf(substring);
            }
            if (i == long.MaxValue - 1)
            {
                Console.WriteLine("Exceed");
            }
        }

        return -1;
    }

    public static long FindByChunks(string substring)
    {
        if (!long.TryParse(substring, out long number))
        {
            return -1;
        }

        (long min, int offset) num = (-1, 0);
        for (int d = 1; d <= substring.Length; d++)
        {
            List<MemChunkList> lists = GetAllChunks(substring, d);
            foreach (MemChunkList list in lists)
            {
                (long min, int offset) numD = GetMinWithOffset(list, d);
                if (numD.min != -1 && numD.min.Digits() == d && ((numD.min < num.min || num.min == -1)))// || (numD.min == num.min && numD.offset < num.offset)) 
                {
                    num = numD;
                }
            }
            if (d == substring.Length && long.TryParse(substring, out long l) && l.Digits() == d && (l <= num.min || num.min == -1))
            {
                num = (l, 0);
            }
        }

        if (num.min != -1)
        {
            return GetLengthBefore(num.min) + num.offset;
        }
        return -1;
    }

    public static List<MemChunkList> GetAllChunks(string substring, int d)
    {
        List<MemChunkList> list = new List<MemChunkList>();

        int start = d == substring.Length ? 1 : 0;
        //for (int i = start; i < d; i++)
        for (int i = d - 1; i >= start; i--)
        {
            list.Add(GetChunks(substring, d, i));
        }

        return list;
    }

    public static MemChunkList GetChunks(string substring, int d, int offset)
    {
        if (offset >= d)
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        return substring.Split(d, offset);
    }

    public static long MergeE(long left, long right, int d)
    {
        int leftDim = left.Digits();
        int rightDim = right.Digits();
        if (leftDim == d)
        {
            return left;
        }

        if (rightDim == d)
        {
            return right - 1;
        }

        // => leftDim < d && rightDim < d)
        long multi = (long)Math.Pow(10, d - rightDim);
        long mod = (long)Math.Pow(10, leftDim);
        if (leftDim == d - rightDim)
        {
            if (left % 10 == 9)
            {
                return (right - 1) * multi + left;
            }
            return right * multi + left;
        }
        else if (leftDim + rightDim == d + 1)
        {
            if (left % 10 == 9)
            {
                return right * multi - 1L;
            }
            return (right * multi) + (left % (long)Math.Pow(10, leftDim - 1));
        }
        return right * multi + left;
    }

    public static (long, int) GetMinWithOffset(MemChunkList chunks, int d)
    {
        if (!IsInOrder(chunks, d))
        {
            return (-1, 0);
        }

        List<long> nums = new List<long>(chunks.Select(mc => mc.ToNum()));
        long num0 = nums[0];
        long num1 = nums[1];
        int d0 = num0.Digits();
        int d1 = num1.Digits();
        if (d0 >= d1 && d0 == d)
        {
            return (num0, 0);
        }
        else if (d0 < d1 && d1 == d)
        {
            return (num1 - 1, d1 - d0 - (chunks.ElementAt(0).Span.Length - d0));
        }

        long merged = MergeE(num0, num1, d);
        if (merged.Digits() == d)
        {
            return (merged, d - d0 - (chunks.ElementAt(0).Span.Length - d0));
        }

        return (-1, 0);
    }

    public static bool IsInOrder(MemChunkList chunks, int d)
    {
        MemChunk prevNum = chunks.ElementAt(0);
        for (int i = 1; i < chunks.Count(); i++)
        {
            MemChunk num = chunks.ElementAt(i);
            if (!CanBeSeq(prevNum, num, d))
            {
                return false;
            }
            prevNum = num;
        }

        return true;
    }

    public static long FindPositionE(string substring)
    {
        if (!int.TryParse(substring, out int num))
        {
            return -1;
        }

        (long min, int offset) result = GetMin(substring);
        long index = GetLengthBefore(result.min);
        return index + result.offset;
    }

    private static int GetLengthN(int n)
    {
        // exclude 0
        return (9 * (int)Math.Pow(10, n - 1)) * n;
    }

    public static (long, int) GetMin(string s)
    {
        int leadingZeroes = s.TakeWhile(x => x == '0').Count();
        if (leadingZeroes >= s.Length - 1)
        {
            int m = leadingZeroes == s.Length ? 1 : int.Parse(s.Substring(leadingZeroes, 1));
            return ((long)Math.Pow(10, leadingZeroes) * m, 1);
        }
        if (s.Length == 1)
        {
            return (long.Parse(s), 0);
        }

        int offset = 0;
        for (int d = 1; d <= s.Length; d++)
        {
            bool isSeq = true;
            int shift = 0;
            long first = 0;
            if (d > (s.Length / 2))
            {
                long matchMin = long.MaxValue;
                int iOffset = 0;
                for (int i = 1; i <= d - 1; i++)
                {
                    long match = TryFindMatch(s.Substring(0, i), s.Substring(i), d);
                    if (match != -1 && match != matchMin)
                    {
                        matchMin = Math.Min(match, matchMin);
                        iOffset = i;
                    }
                }
                if (matchMin < long.MaxValue)
                {
                    return (matchMin, d - iOffset);
                }
            }
            while (shift < d)
            {
                int ind = shift;
                first = long.Parse(s.Substring(ind, d));
                long num;
                if (first.ToString().Length < d)
                {
                    shift++;
                    continue;
                }
                if (shift > 0)
                {
                    string part = s.Substring(0, shift);
                    if ((first - 1).ToString().EndsWith(part))
                    {
                        first = first - 1;
                        offset = first.ToString().Length - shift;
                    }
                    else
                    {
                        isSeq = false;
                        break;
                    }
                }
                int innerDim = d;
                while (ind < s.Length && ind + innerDim <= s.Length)
                {
                    num = long.Parse(s.Substring(ind, innerDim));
                    string guess = (num + 1).ToString();
                    if (s.IndexOf(guess) == ind + innerDim ||
                        (s.Length - (ind + guess.Length) < innerDim && guess.StartsWith(s.Substring(ind + innerDim)) && s.Substring(ind + innerDim) != ""))
                    {
                        ind += innerDim;
                        innerDim = guess.Length;
                    }
                    else if (ind + innerDim >= s.Length)
                    {
                        isSeq = true;
                        break;
                    }
                    else
                    {
                        isSeq = false;
                        break;
                    }
                }
                if (isSeq)
                {
                    break;
                }
                shift++;
            }
            if (!isSeq)
            {
                continue;
            }
            else
            {
                return (first, offset);
            }
        }

        return (long.Parse(s), 0);
    }

    public static long TryFindMatch(string left, string right, int rank)
    {
        long num = -1;
        if (left.Length + right.Length == rank)
        {
            if (left.All(c => c == '9'))
            {
                right = (long.Parse(right) - 1).ToString();
            }
            num = long.Parse(right + left);
        }
        else if (left.Length + right.Length > rank && right[right.Length - 1] == left[0])
        {
            num = long.Parse(right + left.Substring(1));
        }

        if (num.ToString().Length != rank)
        {
            return -1;
        }
        return num;
    }

    public static long TryMatch(string left, string right, int rank)
    {
        int leftPart = rank - left.Length;

        int minLeft = (int)Math.Pow(10, leftPart - 1);
        int maxLeft = (int)Math.Pow(10, leftPart) - 1;

        for (int nl = minLeft; nl <= maxLeft; nl++)
        {
            long leftGuess = long.Parse(nl.ToString() + left) + 1;
            if (leftGuess.ToString().StartsWith(right))
            {
                return leftGuess - 1;
            }
        }

        return -1;
    }

    public static long GetMin(MemChunkList chunks, int d)
    {
        if (!IsInOrder(chunks, d))
        {
            return -1;
        }

        List<long> nums = new List<long>(chunks.Select(mc => mc.ToNum()));
        if (nums[0].Digits() == nums[1].Digits())
        {
            return nums[0];
        }

        return nums[1] - 1;
    }

    public static long Merge(long left, long right, int d)
    {
        long leftPower = (long)Math.Pow(10, left.Digits());
        long rightPower = (long)Math.Pow(10, d - right.Digits());
        return ((right * rightPower / leftPower) * leftPower) + left;
    }

    public static bool _IsInOrder(MemChunkList chunks, int d)
    {
        List<long> nums = new List<long>(chunks.Select(mc => mc.ToNum()));

        long prevNum = nums[0];
        for (int i = 1; i < nums.Count; i++)
        {
            long num = nums[i];
            if (num.Digits() < chunks.ElementAt(i).Span.Length) // leading zero in the right number - can't be a real number
            {
                return false;
            }
            if (prevNum.CanBePrevTo(num, chunks.ElementAt(i - 1).Span.Length, chunks.ElementAt(i).Span.Length, d))
            //if (prevNum.CanBePrevTo(num, d))
            {
                prevNum = num;
                continue;
            }
            return false;
        }

        return true;
    }
}
