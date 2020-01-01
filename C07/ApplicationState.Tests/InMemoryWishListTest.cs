using ApplicationState.Internal;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationState
{
    public class InMemoryWishListTest
    {
        private readonly Mock<ISystemClock> _systemClockMock;
        private readonly InMemoryWishListOptions _options;
        private readonly InMemoryWishList sut;
        private readonly DateTimeOffset _utcNow;
        private readonly DateTimeOffset _expectedExpiryTime;

        public InMemoryWishListTest()
        {
            const int expirationInSeconds = 30;
            _utcNow = DateTimeOffset.UtcNow;
            _expectedExpiryTime = _utcNow.AddSeconds(expirationInSeconds);
            _systemClockMock = new Mock<ISystemClock>();
            _systemClockMock.Setup(x => x.UtcNow).Returns(_utcNow);
            _options = new InMemoryWishListOptions
            {
                SystemClock = _systemClockMock.Object,
                ExpirationInSeconds = expirationInSeconds
            };
            var optionsMock = new Mock<IOptions<InMemoryWishListOptions>>();
            optionsMock.Setup(x => x.Value).Returns(_options);
            sut = new InMemoryWishList(optionsMock.Object);
        }

        public class AddOrRefreshAsync : InMemoryWishListTest
        {
            [Fact]
            public async Task Should_create_new_item()
            {
                // Arrange
                const string expectedItemName = "NewItem";
                const int expectedCount = 1;

                // Act
                var result = await sut.AddOrRefreshAsync(expectedItemName);

                // Assert
                Assert.Equal(expectedItemName, result.Name);
                Assert.Equal(expectedCount, result.Count);
                Assert.Equal(_expectedExpiryTime, result.Expiration);

                var all = await sut.AllAsync();
                Assert.Collection(all,
                    x =>
                    {
                        Assert.Equal(expectedItemName, x.Name);
                        Assert.Equal(expectedCount, x.Count);
                        Assert.Equal(_expectedExpiryTime, x.Expiration);
                    }
                );
            }

            [Fact]
            public async Task Should_increment_Count_of_an_existing_item()
            {
                // Arrange
                const string itemName = "NewItem";
                const int expectedCount = 2;
                await sut.AddOrRefreshAsync(itemName);

                // Act
                var result = await sut.AddOrRefreshAsync(itemName);

                // Assert
                Assert.Equal(expectedCount, result.Count);
                var all = await sut.AllAsync();
                Assert.Collection(all, x => Assert.Equal(expectedCount, x.Count));
            }

            [Fact]
            public async Task Should_set_the_new_Expiration_date_of_an_existing_item()
            {
                // Arrange
                const string itemName = "NewItem";
                var initialItem = await sut.AddOrRefreshAsync(itemName);
                Assert.Equal(_expectedExpiryTime, initialItem.Expiration);

                var expectedUtcNow = DateTimeOffset.UtcNow;
                _systemClockMock.Setup(x => x.UtcNow).Returns(expectedUtcNow);
                var expectedExpiryTime = expectedUtcNow.AddSeconds(_options.ExpirationInSeconds);

                // Act
                var result = await sut.AddOrRefreshAsync(itemName);

                // Assert
                Assert.Equal(expectedExpiryTime, result.Expiration);
                var all = await sut.AllAsync();
                Assert.Collection(all, x => Assert.Equal(expectedExpiryTime, x.Expiration));
            }

            [Fact]
            public async Task Should_set_the_Count_of_expired_items_to_1()
            {
                // Arrange
                const string itemName = "NewItem";
                const int expectedCount = 1;
                var initialDate = DateTimeOffset.UtcNow.AddMinutes(-1);
                var expiredDate = initialDate.AddSeconds(_options.ExpirationInSeconds);
                _systemClockMock.Setup(x => x.UtcNow).Returns(initialDate);
                var initialItem = await sut.AddOrRefreshAsync(itemName);
                Assert.Equal(expiredDate, initialItem.Expiration);
                _systemClockMock.Setup(x => x.UtcNow).Returns(_utcNow);

                // Act
                var result = await sut.AddOrRefreshAsync(itemName);

                // Assert
                Assert.Equal(expectedCount, result.Count);
                var all = await sut.AllAsync();
                Assert.Collection(all, x => Assert.Equal(expectedCount, x.Count));
            }

            [Fact]
            public async Task Should_remove_expired_items()
            {
                // Arrange
                await sut.AddOrRefreshAsync("Item1");
                await sut.AddOrRefreshAsync("Item2");
                await sut.AddOrRefreshAsync("Item3");

                var initialDate = DateTimeOffset.UtcNow.AddMinutes(-1);
                _systemClockMock.Setup(x => x.UtcNow).Returns(initialDate);
                await sut.AddOrRefreshAsync("Item4");
                
                var utcNow = DateTimeOffset.UtcNow;
                _systemClockMock.Setup(x => x.UtcNow).Returns(utcNow);

                // Act
                await sut.AddOrRefreshAsync("Item5");

                // Assert
                var result = await sut.AllAsync();
                Assert.Collection(result,
                    x => Assert.Equal("Item1", x.Name),
                    x => Assert.Equal("Item2", x.Name),
                    x => Assert.Equal("Item3", x.Name),
                    x => Assert.Equal("Item5", x.Name)
                );
            }

        }

        public class AllAsync : InMemoryWishListTest
        {
            [Fact]
            public async Task Should_return_items_ordered_Count_Ascending()
            {
                // Arrange
                await sut.AddOrRefreshAsync("Item1");
                await sut.AddOrRefreshAsync("Item1");
                await sut.AddOrRefreshAsync("Item1");
                await sut.AddOrRefreshAsync("Item2");
                await sut.AddOrRefreshAsync("Item3");
                await sut.AddOrRefreshAsync("Item3");

                // Act
                var result = await sut.AllAsync();

                // Assert
                Assert.Collection(result,
                    x => Assert.Equal("Item1", x.Name),
                    x => Assert.Equal("Item3", x.Name),
                    x => Assert.Equal("Item2", x.Name)
                );
            }
        }
    }
}
