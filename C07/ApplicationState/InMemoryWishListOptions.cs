using ApplicationState.Internal;
using Microsoft.Extensions.Options;

namespace ApplicationState
{
    public class InMemoryWishListOptions : IConfigureOptions<InMemoryWishListOptions>, IValidateOptions<InMemoryWishListOptions>
    {
        public ISystemClock SystemClock { get; set; }
        public int ExpirationInSeconds { get; set; }

        void IConfigureOptions<InMemoryWishListOptions>.Configure(InMemoryWishListOptions options)
        {
            if (SystemClock == default)
            {
                SystemClock = new SystemClock();
            }
            if (ExpirationInSeconds == default)
            {
                ExpirationInSeconds = 30;
            }
        }

        ValidateOptionsResult IValidateOptions<InMemoryWishListOptions>.Validate(string name, InMemoryWishListOptions options)
        {
            if (SystemClock == default)
            {
                return ValidateOptionsResult.Fail("SystemClock cannot be null.");
            }
            if (options.ExpirationInSeconds <= 0)
            {
                return ValidateOptionsResult.Fail("ExpirationInSeconds should be greater than 0.");
            }
            return ValidateOptionsResult.Success;
        }
    }
}
