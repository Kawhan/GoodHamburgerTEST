namespace GoodHamburgerProject.Exceptions
{
    public class AccompanimentNotFound : AccompanimentException
    {
        public AccompanimentNotFound()
            : base("No Accompaniment was found with that ID.") { }
    }
}
