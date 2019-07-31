using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DoorLock
{
    public class PicklockTest
    {
        private readonly Picklock sut = new Picklock();

        public class OpenLock : PicklockTest
        {
            [Fact]
            public void Should_set_the_signature()
            {
                // Arrange
                var @lock = new BasicLock("key1");

                // Act
                sut.OpenLock(@lock, "expectedSignature");

                // Assert
                Assert.Equal("expectedSignature", sut.Signature);
            }

            [Fact]
            public void Should_return_false_when_the_signature_does_not_match_the_lock()
            {
                // Arrange
                var @lock = new BasicLock("key1");

                // Act
                var result = sut.OpenLock(@lock, "invalidKey");

                // Assert
                Assert.False(result, "The result should be false when the signature does not match the lock");
            }

            [Fact]
            public void Should_return_true_hen_the_signature_match_the_lock()
            {
                // Arrange
                var @lock = new BasicLock("key1");

                // Act
                var result = sut.OpenLock(@lock, "key1");

                // Assert
                Assert.True(result, "The result should be true when the signature match the lock");
            }

            [Fact]
            public void Should_unlock_the_lock_when_the_signature_match_the_lock()
            {
                // Arrange
                var @lock = new BasicLock("key1");
                @lock.Lock(new BasicKey("key1"));
                Assert.True(@lock.IsLocked, "The lock should be locked");

                // Act
                sut.OpenLock(@lock, "key1");

                // Assert
                Assert.False(@lock.IsLocked, "The lock should be unlocked");
            }
        }
    }
}
