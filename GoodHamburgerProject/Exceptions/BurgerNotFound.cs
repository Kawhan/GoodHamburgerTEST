namespace GoodHamburgerProject.Exceptions
{
    public class BurgerNotFound : BurgerException
    {
        public BurgerNotFound()
            : base("No hamburger was found with that ID.") { }
    }
}
