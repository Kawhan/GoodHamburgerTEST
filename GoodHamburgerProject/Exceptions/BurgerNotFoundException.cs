namespace GoodHamburgerProject.Exceptions
{
    public class BurgerNotFoundException : BurgerException
    {
        public BurgerNotFoundException()
            : base("No hamburger was found with that ID.") { }
    }
}
