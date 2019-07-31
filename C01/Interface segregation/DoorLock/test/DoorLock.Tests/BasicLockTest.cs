using System;
using Xunit;

namespace DoorLock.Tests
{
    //
    // Note that I based my ascii keys on work found at http://ascii.co.uk/art/keys
    //
    public class BasicLockTest
    {
        private const string _workingKeySignature = @"
   .———.
  /    |\________________
 ()    | ________   _   _)
  \    |/        |_| | |
   `———'             |_|
";
        private const string _invalidKeySignature = @"
 .--.
/.-. '----------.
\'-' .--'--''-'-'
 '--'
";
        private readonly BasicLock sut;

        private readonly IKey _workingKey;
        private readonly IKey _invalidKey;

        public BasicLockTest()
        {
            sut = new BasicLock(_workingKeySignature);
            _invalidKey = new BasicKey(_invalidKeySignature);
            _workingKey = new BasicKey(_workingKeySignature);
        }

        public class DoesMatch : BasicLockTest
        {
            [Fact]
            public void Should_return_true_when_the_key_matches_the_lock()
            {
                // Act
                var result = sut.DoesMatch(_workingKey);

                // Assert
                Assert.True(result, "The key should match the lock.");
            }

            [Fact]
            public void Should_return_false_when_the_key_does_not_match_the_lock()
            {
                // Act
                var result = sut.DoesMatch(_invalidKey);

                // Assert
                Assert.False(result, "The key should not match the lock.");
            }
        }

        public class Lock : BasicLockTest
        {
            public Lock()
            {
                Assert.False(sut.IsLocked, "The lock should be unlocked");
            }

            [Fact]
            public void Should_lock_the_lock_when_the_key_matches_the_lock()
            {
                // Act
                sut.Lock(_workingKey);

                // Assert
                Assert.True(sut.IsLocked, "The lock should be locked");
            }

            [Fact]
            public void Should_throw_a_KeyDoesNotMatchException_when_the_key_does_not_match_the_lock()
            {
                Assert.Throws<KeyDoesNotMatchException>(() => sut.Lock(_invalidKey));
            }
        }

        public class Unlock : BasicLockTest
        {
            public Unlock()
            {
                sut.Lock(_workingKey);
                Assert.True(sut.IsLocked, "The lock should be locked");
            }

            [Fact]
            public void Should_unlock_the_lock_when_the_key_matches_the_lock()
            {
                // Act
                sut.Unlock(_workingKey);

                // Assert
                Assert.False(sut.IsLocked, "The lock should be unlocked");
            }

            [Fact]
            public void Should_throw_a_KeyDoesNotMatchException_when_the_key_does_not_match_the_lock()
            {
                Assert.Throws<KeyDoesNotMatchException>(() => sut.Unlock(_invalidKey));
            }
        }
    }
}
