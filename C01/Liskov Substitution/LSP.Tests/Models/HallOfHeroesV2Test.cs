// #define HALL_OF_HEROES
// #define HALL_OF_HEROES_2
// #define HALL_OF_HEROES_2_FIXED

namespace LSP.Models
{
    public class HallOfHeroesV2Test : BaseHallOfHeroesTest
    {
        protected override HallOfHeroes sut { get; } = new HallOfHeroesV2();
    }
}
