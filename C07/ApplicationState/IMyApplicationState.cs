namespace ApplicationState
{
    public interface IMyApplicationState
    {
        TItem Get<TItem>(string key);
        bool Has<TItem>(string key);
        void Set<TItem>(string key, TItem value);
    }
}
