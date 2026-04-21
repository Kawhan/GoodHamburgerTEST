namespace GoodHamburgerProject.Exceptions
{
    public class InvalidOrderItemTypeException : OrderException
    {
        public InvalidOrderItemTypeException()
           : base("Only one burger is allowed.") { }
    }
}
