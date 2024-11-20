namespace Services.TaskProcessing;

public class TaskSortingManager
{
    public IEnumerable<T> SortBySingleAttribute<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        return items.OrderBy(keySelector).ToList();
    }
}