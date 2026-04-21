namespace GoodHamburgerProject.Exceptions
{
    public abstract class OrderException : Exception
    {
        protected OrderException(string message) : base(message) { }
    }
}
