namespace GoodHamburgerProject.Exceptions
{
    public class DuplicateAccompanimentsException : OrderException
    {
        public DuplicateAccompanimentsException()
            : base("Duplicate Accompaniments are not allowed.") { }
    }
}
