using System;

namespace ApplicationState
{
    /// <remarks>
    /// This class is never used in the program.
    /// This is an example of how to inject IMyApplicationState into a class.
    /// </remarks>
    public class SomeConsumer
    {
        private readonly IApplicationState _myApplicationWideService;

        public SomeConsumer(IApplicationState myApplicationWideService)
        {
            _myApplicationWideService = myApplicationWideService ?? throw new ArgumentNullException(nameof(myApplicationWideService));
        }

        public string GetSomeKey()
        {
            if (_myApplicationWideService.Has<string>("some-key"))
            {
                return _myApplicationWideService.Get<string>("some-key");
            }
            return null;
        }
    }
}
