namespace Services.TaskServices;

public class TaskSortingManager : ITaskSortingManager
{
    /// <summary>
    /// Generic sorting method.
    /// Sorts a List of objects by a single attribute
    /// that needs to be a comparable (e.g. string, int, etc.).
    /// </summary>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <returns>The sorted List</returns>
    public IEnumerable<T> SortBySingleAttribute<T, TKey>(
        IEnumerable<T> items,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        return items.OrderBy(keySelector).ToList();
    }
    
    /// <summary>
    /// Generic filtering method.
    /// Filters a List of objects by one predicate.
    /// </summary>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T> FilterByPredicate<T>(
        IEnumerable<T> items,
        Func<T, bool> predicate)
    {
        return items.Where(predicate).ToList();
    }
}