namespace ApplicationState
{
    public interface IMyApplicationState
    {
        TItem Get<TItem>(string key);
        bool Has<TItem>(string key);
        TItem Set<TItem>(string key, TItem value);
    }
}
