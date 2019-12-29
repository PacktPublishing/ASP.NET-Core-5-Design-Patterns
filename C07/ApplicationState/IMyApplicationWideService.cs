namespace ApplicationState
{
    public interface IMyApplicationWideService
    {
        TItem Get<TItem>(string key);
        bool Has<TItem>(string key);
        TItem Set<TItem>(string key, TItem value);
    }
}
