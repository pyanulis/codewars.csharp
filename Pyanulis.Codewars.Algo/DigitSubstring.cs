using MemChunk = System.ReadOnlyMemory<char>;

namespace Pyanulis.Codewars.Algo;

public partial class DigitSubstring
{
    public static long FindByChunksE(string substring)
    {
        if (!long.TryParse(substring, out long number))
        {
            return -1;
        }

        if (substring.All(c => c == '0'))
        {
            return GetLengthBefore((long)Math.Pow(10, substring.Length)) + 1;
        }

        if (substring.All(c => c == '9'))
        {
            return GetLengthBefore(9L * (long)Math.Pow(10, substring.Length - 1) - 1) + 1;
        }

        (long min, int offset) min = (-1, 0);
        for (int d = 1; d <= substring.Length; d++)
        {
            (long min, int offset) num = ProcessString(substring, d);
            min = Update(min, num);
            if (d == substring.Length && long.TryParse(substring, out long l) && l.Digits() == d)
            {
                min = Update(min, (l, 0));
            }
        }

        if (min.min != -1)
        {
            return GetLengthBefore(min.min) + min.offset;
        }
        return -1;
    }

    public static long FindByInc(string substring)
    {
        if (!long.TryParse(substring, out long number))
        {
            return -1;
        }

        if (substring.All(c => c == '0'))
        {
            return GetLengthBefore((long)Math.Pow(10, substring.Length)) + 1;
        }

        if (substring.All(c => c == '9'))
        {
            return GetLengthBefore(9L * (long)Math.Pow(10, substring.Length - 1) - 1) + 1;
        }

        (long min, int offset) min = (-1, 0);
        for (int d = 1; d <= substring.Length; d++)
        {
            (long min, int offset) num = ProcessStringInc(substring, d);
            min = Update(min, num);
            if (d == substring.Length && long.TryParse(substring, out long l) && l.Digits() == d)
            {
                min = Update(min, (l, 0));
            }
        }

        if (min.min != -1)
        {
            return GetLengthBefore(min.min) + min.offset;
        }
        return -1;
    }

    public static (long min, int offset) Update((long min, int offset) min, (long min, int offset) num)
    {
        (long min, int offset) res = min;
        if ((min.min == -1 && num.min > 0) ||
            ((min.min > num.min ||
            (min.min == num.min && min.offset > num.offset))
            && num.min > 0))
        {
            res = num;
        }

        return res;
    }

    public static (long min, int offset) ProcessStringInc(string substring, int d)
    {
        (long min, int offset) min = (-1, 0);
        int start = d == substring.Length ? 1 : 0;
        for (int i = start; i < d; i++)
        //for (int i = d - 1; i >= start; i--)
        {
            (long min, int offset) num = ProcessStringInc(substring, d, i);
            min = Update(min, num);
        }

        return min;
    }

    public static (long min, int offset) ProcessStringInc(string substring, int d, int offset)
    {
        int curDim = d;
        MemChunk prevNum = offset == 0 ? substring.AsMemory(0, curDim) : substring.AsMemory(0, offset);
        int start = prevNum.Span.Length;
        MemChunk firstNum = prevNum;
        MemChunk secondNum = MemChunk.Empty;

        if (firstNum.ToNum().Digits() < d && offset == 0)
        {
            return (-1, 0);
        }
        while (start < substring.Length)
        {
            if (prevNum.Span.Length == curDim)
            {
                long next = prevNum.ToNum() + 1;
                int ln = start + next.Digits() > substring.Length ? substring.Length - start : next.Digits();
                MemChunk nextNum = substring.AsMemory(start, ln);
                if (nextNum.ToNum() == next)
                {
                    curDim = nextNum.Span.Length;
                    start += curDim;
                    if (secondNum.IsEmpty)
                    {
                        secondNum = nextNum;
                    }
                    prevNum = nextNum;
                    continue;
                }
                else if (!CanBeSeq(prevNum, nextNum, curDim))
                {
                    return (-1, 0);
                }
                if (secondNum.IsEmpty)
                {
                    secondNum = nextNum;
                }
                break;
            }
            bool overlap = start + curDim > substring.Length;
            int l = overlap ? substring.Length - start : curDim;
            MemChunk num = substring.AsMemory(start, l);
            if (!CanBeSeq(prevNum, num, curDim))
            {
                overlap = start + ++curDim > substring.Length;
                l = overlap ? substring.Length - start : curDim;
                num = substring.AsMemory(start, l);
                if (!CanBeSeq(prevNum, num, curDim, true))
                {
                    return (-1, 0);
                }
            }
            start += curDim;
            prevNum = num;
            if (secondNum.IsEmpty)
            {
                secondNum = num;
            }
        }

        if (firstNum.Span.Length == d)
        {
            return (firstNum.ToNum(), 0);
        }

        if (secondNum.Span.Length == d || secondNum.Span.Length == d + 1)
        {
            long n = secondNum.ToNum() - 1;
            int shift = n == firstNum.ToNum() ? 0 : n.Digits() - firstNum.Span.Length;
            return (n, shift);
        }

        long merged = MergeM(firstNum, secondNum, d, firstNum.Span.Length + secondNum.Span.Length > substring.Length);
        if (merged.Digits() == d)
        {
            return (merged, d - firstNum.Span.Length);
        }

        return (-1, 0);
    }

    public static (long min, int offset) ProcessString(string substring, int d)
    {
        (long min, int offset) min = (-1, 0);

        if (d == substring.Length)
        {
            for (int i = 0; i < substring.Length; i++)
            {
                if (substring.Substring(i).StartsWith('0'))
                {
                    continue;
                }
                string s = string.Concat(substring.Substring(i), substring.Substring(0, i));
                min = Update(min, (long.Parse(s), s.Length - i));
            }
            return min;
        }

        int start = d == substring.Length ? 1 : 0;
        for (int i = start; i < d; i++)
        //for (int i = d - 1; i >= start; i--)
        {
            (long min, int offset) num = ProcessString(substring, d, i);
            min = Update(min, num);
        }

        return min;
    }

    public static (long min, int offset) ProcessString(string substring, int d, int offset)
    {
        int curDim = d;
        MemChunk prevNum = offset == 0 ? substring.AsMemory(0, curDim) : substring.AsMemory(0, offset);
        int start = prevNum.Span.Length;
        MemChunk firstNum = prevNum;
        MemChunk secondNum = MemChunk.Empty;
        while (start < substring.Length)
        {
            bool overlap = start + curDim > substring.Length;
            int l = overlap ? substring.Length - start : curDim;
            MemChunk num = substring.AsMemory(start, l);
            if (!CanBeSeq(prevNum, num, curDim))
            {
                overlap = start + ++curDim > substring.Length;
                l = overlap ? substring.Length - start : curDim;
                num = substring.AsMemory(start, l);
                if (!CanBeSeq(prevNum, num, curDim, true))
                {
                    return (-1, 0);
                }
            }
            start += curDim;
            prevNum = num;
            if (secondNum.IsEmpty)
            {
                secondNum = num;
            }
        }

        if (firstNum.Span.Length == d)
        {
            return (firstNum.ToNum(), 0);
        }

        if (secondNum.Span.Length == d || secondNum.Span.Length == d + 1)
        {
            long n = secondNum.ToNum() - 1;
            int shift = n == firstNum.ToNum() ? 0 : n.Digits() - firstNum.Span.Length;
            return (n, shift);
        }

        long merged = MergeM(firstNum, secondNum, d, firstNum.Span.Length + secondNum.Span.Length > substring.Length);
        if (merged.Digits() == d)
        {
            return (merged, d - firstNum.Span.Length);
        }

        return (-1, 0);
    }

    public static long MergeM(MemChunk leftMc, MemChunk rightMc, int d, bool overlap)
    {
        int leftDim = leftMc.Span.Length;
        int rightDim = rightMc.Span.Length;

        long left = leftMc.ToNum();
        long right = rightMc.ToNum();
        //int leftDim = left.Digits();
        //int rightDim = right.Digits();

        // => leftDim < d && rightDim < d)
        long multi = (long)Math.Pow(10, d - rightDim);
        long mod = (long)Math.Pow(10, leftDim);
        if (leftDim == d - rightDim)
        {
            //if (left % 10 == 9)
            if (IsAllNines(leftMc))
            {
                long guess = (right - 1) * multi + left;
                return guess;
                //long guessNext = (guess + 1) / 10;
                //if (guessNext == right % 10 || guessNext == right / multi)
                //{
                //    return guess;
                //}
            }
            return right * multi + left;
        }
        else if (leftDim + rightDim == d + 1)
        {
            if (leftMc.ToArray()[0] == rightMc.ToArray()[rightMc.Span.Length - 1])
            {
                return (right * multi) + (left % (long)Math.Pow(10, leftDim - 1));
            }
            if (left % 10 == 9)
            {
                return right * multi - 1L;
            }
            return (right * multi) + (left % (long)Math.Pow(10, leftDim - 1));
        }
        return right * multi + left;
    }

    public static bool CanBeSeq(MemChunk mcLeft, MemChunk mcRight, int d, bool dimChange = false)
    {
        bool res = false;
        long left = mcLeft.ToNum();
        long right = mcRight.ToNum();
        int dLeft = mcLeft.Span.Length;
        int dRight = mcRight.Span.Length;
        //int dLeft = left.Digits();
        //int dRight = right.Digits();

        if (right == 0 || right.Digits() < dRight) // leading zero in the right number - can't be a real number
        {
            return false;
        }

        // When rank is changing one of number should be full:
        // 9910 -> 99-100, but 910 can't be 99-100, it'll be 9-10.
        // Also left number should be all 9's
        if (dimChange && ((dLeft < d && dRight < d) || !IsAllNines(mcLeft)))
        {
            return false;
        }

        if (IsMax(left, d))
        {
            int diff = (d + 1) - dRight;
            return left + 1 == right * (long)Math.Pow(10, diff);
        }

        if (dLeft == d && left.Digits() < dLeft)
        {
            return false;
        }

        if (dLeft == dRight && dLeft == d)
        {
            return right - left == 1;
        }

        if (dLeft < dRight && dRight == d)
        {
            long diff = right - left;
            return diff % (long)Math.Pow(10, dLeft) == 1;
        }

        if (dLeft > dRight && dLeft == d)
        {
            return ((left + 1) / (long)(Math.Pow(10, dLeft - dRight))) == right;
        }

        if (dLeft < d && dRight < d)
        //if (dLeft + dRight == d + 1)
        {
            if (dLeft + dRight == d + 1
                && mcLeft.ToArray()[0] == mcRight.ToArray()[mcRight.Span.Length - 1]
                && left % 10 != 9)
            {
                return true;
            }

            long multi = (long)Math.Pow(10, d - dRight);
            long mod = (long)Math.Pow(10, dLeft);
            if (dLeft == d - dRight)
            {
                return true;
                //return (left != 0 && ((right * multi) - 1) % mod == left) || (left == 0 && (right * multi) % mod == left);
            }
            //else if (dLeft + dRight == d + 1)
            //{
            //    long nn = (left % 10 == 9) ? left + 1 : left;
            //    return right % 10 == nn / ((long)Math.Pow(10, nn.Digits() - 1));
            //}
            else
            {
                return false;
            }
        }

        return res;

        bool IsMax(long n, int d)
        {
            return n == (long)Math.Pow(10, d) - 1;
        }
    }

    public static long GetLengthBefore(long num)
    {
        int digits = num.Digits();
        long length = GetAllLengthN(digits - 1);
        //length += (digits * ((num % (int)Math.Pow(10, digits - 1)) + 1));
        long nines = (long)Math.Pow(10, digits - 1) - 1;
        length += (digits * (num - nines - 1));
        return length;
    }

    public static long GetAllLengthN(long n)
    {
        long length = 0;
        for (long i = n; i > 0; i--)
        {
            length += (9L * (long)Math.Pow(10, i - 1L)) * i;
        }
        return length;
    }

    public static long GetNumAt(long index)
    {
        long nums = 9L;
        long d = 1;

        while (index > nums)
        {
            index -= nums;
            d++;
            nums = (9L * (long)Math.Pow(10, d - 1L)) * d;
        }
        long number = (long)Math.Pow(10, d - 1) + (index / d);

        return number;
    }

    public static bool IsAllNines(MemChunk mc)
    {
        return mc.ToArray().All(c => c == '9');
    }
}

public static class Extensions
{
    public static IEnumerable<MemChunk> Split(this string s, int length, int offset = 0)
    {
        MemChunk memory = s.AsMemory();
        if (offset > 0)
        {
            yield return memory.Slice(0, offset);
        }
        for (int i = offset; i < s.Length; i += length)
        {
            yield return memory.Slice(i, Math.Min(length, s.Length - i));
        }
    }

    public static long ToNum(this ReadOnlyMemory<char> s)
    {
        return long.Parse(s.Span);
    }

    //public static long ToNum(this IEnumerable<MemChunk> s)
    //{
    //    IEnumerable<MemChunk> reverse = s.Reverse();
    //    long num = 0;
    //    int i = 0;
    //    foreach (MemChunk item in reverse)
    //    {
    //        num += long.Parse(item.Span) * (long)Math.Pow(10, i);
    //    }
    //    return num;
    //}

    public static int Digits(this long n) =>
    n == 0L ? 1 : (n > 0L ? 1 : 2) + (int)Math.Log10(Math.Abs((double)n));

    public static bool CanBePrevTo(this long n, long next)
    {
        bool res = false;
        int nDim = n.Digits();
        int nextDim = next.Digits();
        if (nDim == nextDim)
        {
            return next - n == 1;
        }

        if (nDim < nextDim)
        {
            if (n == 0)
            {
                return true;
            }
            long diff = next - n;
            return diff % 10 == 1;
        }

        if (nDim > nextDim)
        {
            return (n / (long)(Math.Pow(10, nDim - nextDim))) == next;
        }

        return res;
    }

    public static bool CanBePrevTo(this long n, long next, int d)
    {
        bool res = false;
        int nDim = n.Digits();
        int nextDim = next.Digits();
        if (nDim == nextDim && nDim == d)
        {
            return next - n == 1;
        }

        if (nDim < nextDim && nextDim == d)
        {
            long diff = next - n;
            return diff % 10 == 1;
        }

        if (nDim > nextDim && nDim == d)
        {
            return ((n + 1) / (long)(Math.Pow(10, nDim - nextDim))) == next;
        }

        if (nDim < d && nextDim < d)
        {
            long multi = (long)Math.Pow(10, d - nextDim);
            long mod = (long)Math.Pow(10, nDim);
            if (nDim == d - nextDim)
            {
                return (n != 0 && ((next * multi) - 1) % mod == n) || (n == 0 && (next * multi) % mod == n);
            }
            else if (nDim + nextDim == d + 1)
            {
                return next % 10 == n / ((long)Math.Pow(10, nDim - 1));
            }
            else
            {
                return false;
            }
        }

        return res;
    }

    public static bool CanBePrevTo(this long n, long next, int nDim, int nextDim, int d)
    {
        bool res = false;
        if (nDim == nextDim && nDim == d)
        {
            return next - n == 1;
        }

        if (nDim < nextDim && nextDim == d)
        {
            long diff = next - n;
            return diff % 10 == 1;
        }

        if (nDim > nextDim && nDim == d)
        {
            return ((n + 1) / (long)(Math.Pow(10, nDim - nextDim))) == next;
        }

        if (nDim < d && nextDim < d)
        {
            long multi = (long)Math.Pow(10, d - nextDim);
            long mod = (long)Math.Pow(10, nDim);
            if (nDim == d - nextDim)
            {
                return (n != 0 && ((next * multi) - 1) % mod == n) || (n == 0 && (next * multi) % mod == n);
            }
            else if (nDim + nextDim == d + 1)
            {
                long nn = (n % 10 == 9) ? n + 1 : n;
                return next % 10 == nn / ((long)Math.Pow(10, nn.Digits() - 1));
            }
            else
            {
                return false;
            }
        }

        return res;
    }
}
