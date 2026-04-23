namespace GoodHamburgerProject.Exceptions
{
    public class AccompanimentNotFoundException : AccompanimentException
    {
        public AccompanimentNotFoundException()
            : base("No Accompaniment was found with that ID.") { }
    }
}
