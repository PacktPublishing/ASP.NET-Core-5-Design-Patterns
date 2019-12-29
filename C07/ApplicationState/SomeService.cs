using System;

namespace ApplicationState
{
    public class SomeService
    {
        private readonly IMyApplicationWideService _myApplicationWideService;

        public SomeService(IMyApplicationWideService myApplicationWideService)
        {
            _myApplicationWideService = myApplicationWideService ?? throw new ArgumentNullException(nameof(myApplicationWideService));
        }

        public string DoSomething()
        {
            if (_myApplicationWideService.Has<string>("some-key"))
            {
                return _myApplicationWideService.Get<string>("some-key");
            }
            return null;
        }
    }
}
