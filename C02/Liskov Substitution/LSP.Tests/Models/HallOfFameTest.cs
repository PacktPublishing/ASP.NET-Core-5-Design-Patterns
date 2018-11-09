// #define HALL_OF_HEROES
// #define HALL_OF_HEROES_2
#define HALL_OF_HEROES_2_FIXED
using Xunit;

namespace LSP.Models
{
    public class HallOfFameTest
    {
        private readonly HallOfFame sut;

        public HallOfFameTest()
        {
#if HALL_OF_HEROES
            sut = new HallOfHeroes();
#elif HALL_OF_HEROES_2
            sut = new HallOfHeroesV2();
#elif HALL_OF_HEROES_2_FIXED
            sut = new HallOfHeroesV2Fixed();
#else
            sut = new HallOfFame();
#endif
        }

        public class Add : HallOfFameTest
        {
            [Fact]
            public void Should_not_add_existing_ninja()
            {
                // Arrange
                var expectedNinja = new Ninja { Kills = 200 };

                // Act
                sut.Add(expectedNinja);
                sut.Add(expectedNinja);

                // Assert
                Assert.Collection(sut.Members, 
                    ninja => Assert.Same(expectedNinja, ninja)
                );
            }

            public static TheoryData<Ninja> NinjaWithAtLeast100Kills => new TheoryData<Ninja>
            {
                new Ninja { Kills = 100 },
                new Ninja { Kills = 101 },
                new Ninja { Kills = 200 },
            };

            [Theory]
            [MemberData(nameof(NinjaWithAtLeast100Kills))]
            public void Should_add_the_specified_ninja(Ninja expectedNinja)
            {
                // Act
                sut.Add(expectedNinja);

                // Assert
                Assert.Collection(sut.Members,
                    ninja => Assert.Same(expectedNinja, ninja)
                );
            }
        }

        public class Members : HallOfFameTest
        {
            [Fact]
            public void Should_return_ninja_ordered_by_kills_desc()
            {
                // Arrange
                sut.Add(new Ninja { Kills = 100 });
                sut.Add(new Ninja { Kills = 150 });
                sut.Add(new Ninja { Kills = 200 });

                // Act
                var result = sut.Members;

                // Assert
                Assert.Collection(result,
                    ninja => Assert.Equal(200, ninja.Kills),
                    ninja => Assert.Equal(150, ninja.Kills),
                    ninja => Assert.Equal(100, ninja.Kills)
                );
            }
        }
    }
}
