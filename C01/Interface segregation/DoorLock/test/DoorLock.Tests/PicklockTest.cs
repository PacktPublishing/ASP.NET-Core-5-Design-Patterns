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
        public class CreateMatchingKeyFor : PicklockTest
        {
            [Fact]
            public void Should_return_the_matching_key_when_provided()
            {
                // Arrange
                var sut = new Picklock(new[] { "key1" });
                var @lock = new BasicLock("key1");

                // Act
                var key = sut.CreateMatchingKeyFor(@lock);

                // Assert
                Assert.NotNull(key);
                Assert.Equal("key1", key.Signature);
            }

            [Fact]
            public void Should_throw_an_ImpossibleToPickTheLockException_when_no_matching_key_can_be_generated()
            {
                // Arrange
                var sut = new Picklock(new[] { "key2" });
                var @lock = new BasicLock("key1");

                // Act & Assert
                Assert.Throws<ImpossibleToPickTheLockException>(() => sut.CreateMatchingKeyFor(@lock));
            }
        }
    }
}
