namespace LSP.Models
{
    public class HallOfHeroesViolatingLSP : HallOfFame
    {
        public override void Add(Ninja ninja)
        {
            if (InternalMembers.Contains(ninja))
            {
                return;
            }
            InternalMembers.Add(ninja);
        }
    }
}
