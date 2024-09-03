using System.Linq.Expressions;

namespace Core.Extensions;

public static class IQueriableExtension
{
    public static IQueryable<T> FilterData<T>(this IQueryable<T> query, List<KeyValuePair<bool, Expression<Func<T, bool>>>> filterMapping)
    {
        filterMapping.ForEach(action =>
        {
            if (action.Key)
            {
                query = query.Where(action.Value);
            }
        });

        return query;
    }
}
