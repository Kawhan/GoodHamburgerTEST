namespace GoodHamburgerProject.Exceptions
{
    public class AccompanimentAlreadyExistsException : AccompanimentException
    {
        public AccompanimentAlreadyExistsException()
           : base("A Accompaniment with this name already exists.") { }
    }
}
