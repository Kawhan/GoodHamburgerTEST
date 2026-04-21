namespace GoodHamburgerProject.Exceptions
{
    public class UpdateOrderException : OrderException
    {
        public UpdateOrderException()
           : base("Error updating order.") { }
    }
}
