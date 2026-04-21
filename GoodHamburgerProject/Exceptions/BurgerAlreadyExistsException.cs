namespace GoodHamburgerProject.Exceptions
{
    public class BurgerAlreadyExistsException : BurgerException
    {
        public BurgerAlreadyExistsException()
            : base("A burger with this name already exists.") { }
    }
}