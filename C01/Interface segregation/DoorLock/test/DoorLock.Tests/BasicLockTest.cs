using Moq;
using System;
using Xunit;

namespace DoorLock.Tests
{
    // I based my keys on http://ascii.co.uk/art/keys
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

        private readonly Mock<IKey> _workingKeyMock;
        private readonly Mock<IKey> _invalidKeyMock;


        public BasicLockTest()
        {
            sut = new BasicLock(_workingKeySignature);
            _invalidKeyMock = new Mock<IKey>();
            _invalidKeyMock.Setup(x => x.Signature).Returns(_invalidKeySignature);
            _workingKeyMock = new Mock<IKey>();
            _workingKeyMock.Setup(x => x.Signature).Returns(_workingKeySignature);
        }


        public class DoesMatch : BasicLockTest
        {
            [Fact]
            public void Should_return_true_when_the_key_matches_the_lock()
            {
                // Act
                var result = sut.DoesMatch(_workingKeyMock.Object);

                // Assert
                Assert.True(result, "The key should match the lock.");
            }

            [Fact]
            public void Should_return_false_when_the_key_does_not_match_the_lock()
            {
                // Act
                var result = sut.DoesMatch(_invalidKeyMock.Object);

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
                sut.Lock(_workingKeyMock.Object);

                // Assert
                Assert.True(sut.IsLocked, "The lock should be locked");
            }

            [Fact]
            public void Should_throw_a_KeyDoesNotMatchException_when_the_key_does_not_match_the_lock()
            {
                Assert.Throws<KeyDoesNotMatchException>(
                    () => sut.Lock(_invalidKeyMock.Object));
            }
        }

        public class Unlock : BasicLockTest
        {
            public Unlock()
            {
                sut.Lock(_workingKeyMock.Object);
                Assert.True(sut.IsLocked, "The lock should be locked");
            }

            [Fact]
            public void Should_unlock_the_lock_when_the_key_matches_the_lock()
            {
                // Act
                sut.Unlock(_workingKeyMock.Object);

                // Assert
                Assert.False(sut.IsLocked, "The lock should be unlocked");
            }

            [Fact]
            public void Should_throw_a_KeyDoesNotMatchException_when_the_key_does_not_match_the_lock()
            {
                Assert.Throws<KeyDoesNotMatchException>(
                    () => sut.Unlock(_invalidKeyMock.Object));
            }
        }
    }
}
