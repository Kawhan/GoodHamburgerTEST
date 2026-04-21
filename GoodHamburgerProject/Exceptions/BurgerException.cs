namespace GoodHamburgerProject.Exceptions
{
    public abstract class BurgerException : Exception
    {
        protected BurgerException(string message) : base(message) { }
    }
}
