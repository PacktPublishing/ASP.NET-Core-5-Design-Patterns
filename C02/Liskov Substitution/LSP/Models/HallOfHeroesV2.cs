namespace LSP.Models
{
    public class HallOfHeroesV2 : HallOfFame
    {
        public override void Add(Ninja ninja)
        {
            if (InternalMembers.Contains(ninja))
            {
                throw new DuplicateNinjaException();
            }
            InternalMembers.Add(ninja);
        }
    }
}
