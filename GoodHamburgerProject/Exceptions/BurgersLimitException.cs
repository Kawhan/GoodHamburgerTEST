namespace GoodHamburgerProject.Exceptions
{
    public class BurgersLimitException : OrderException
    {
        public BurgersLimitException()
            : base("Only one burger is allowed.") { }
    }
}
