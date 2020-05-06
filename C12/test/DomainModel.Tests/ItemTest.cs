using System;
using Xunit;

namespace DomainModel
{
    public class ItemTest
    {
        public class AddStock : ItemTest
        {
            [Fact]
            public void Should_add_the_specified_amount_to_QuantityInStock()
            {
                // Arrange
                var sut = new Item();

                // Act
                sut.AddStock(2);

                // Assert
                Assert.Equal(2, sut.QuantityInStock);
            }
        }

        public class RemoveStock : ItemTest
        {
            [Fact]
            public void Should_remove_the_specified_amount_to_QuantityInStock()
            {
                // Arrange
                var sut = new Item { QuantityInStock = 5 };

                // Act
                sut.RemoveStock(2);

                // Assert
                Assert.Equal(3, sut.QuantityInStock);
            }

            [Fact]
            public void Should_throw_a_NotEnoughStockException_when_the_specified_amount_of_items_to_remove_is_greater_than_QuantityInStock()
            {
                // Arrange
                var sut = new Item { QuantityInStock = 2 };

                // Act & Assert
                var stockException = Assert.Throws<NotEnoughStockException>(
                    () => sut.RemoveStock(3)
                );
                Assert.Equal(2, stockException.QuantityInStock);
                Assert.Equal(3, stockException.AmountToRemove);
            }
        }
    }
}
