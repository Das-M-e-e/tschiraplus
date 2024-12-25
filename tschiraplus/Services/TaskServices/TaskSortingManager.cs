namespace Services.TaskServices;

public class TaskSortingManager : ITaskSortingManager
{
    public IEnumerable<T> SortBySingleAttribute<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        return items.OrderBy(keySelector).ToList();
    }

    public IEnumerable<T> FilterByPredicate<T>(
        IEnumerable<T> items,
        Func<T, bool> predicate)
    {
        return items.Where(predicate).ToList();
    }
}