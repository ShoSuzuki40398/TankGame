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
}
