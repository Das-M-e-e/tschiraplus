namespace Services.TaskServices;

public interface ITaskSortingManager
{
    public IEnumerable<T> SortBySingleAttribute<T, TKey>(IEnumerable<T> items, Func<T, TKey> keySelector) where TKey : IComparable<TKey>;
    public IEnumerable<T> FilterByPredicate<T>(IEnumerable<T> items, Func<T, bool> predicate);
}