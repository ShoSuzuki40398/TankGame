using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    /// <summary>
    /// �ŏ��l�����v�f��Ԃ��܂�
    /// </summary>
    public static TSource FindMin<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
    {
        return self.First(c => selector(c).Equals(self.Min(selector)));
    }

    /// <summary>
    /// �ő�l�����v�f��Ԃ��܂�
    /// </summary>
    public static TSource FindMax<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
    {
        return self.First(c => selector(c).Equals(self.Max(selector)));
    }

    /// <summary>
    /// �z����̗v�f���w��L�[�Ń\�[�g���܂�
    /// </summary>
    public static void Sort<TSource, TResult>(this TSource[] array,Func<TSource, TResult> selector) where TResult : IComparable
    {
        Array.Sort(array, (x, y) =>
        {
            var result = selector(x).CompareTo(selector(y));
            return result;
        });
    }

}
