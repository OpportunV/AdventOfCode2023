namespace Extensions;

public static class EnumerableExtensions
{
    public static TSource? MinByOrDefault<TSource, TKey>(this IEnumerable<TSource> source,
                                                         Func<TSource, TKey> keySelector)
    {
        var list = source.ToList();
        return list.Any()
            ? list.MinBy(keySelector)
            : default;
    }

    public static IEnumerable<TSource> Print<TSource>(this IEnumerable<TSource> source)
    {
        var list = source.ToList();
        if (list.Any())
        {
            foreach (var value in list)
            {
                Console.WriteLine(value);
            }
        }

        return list;
    }
}