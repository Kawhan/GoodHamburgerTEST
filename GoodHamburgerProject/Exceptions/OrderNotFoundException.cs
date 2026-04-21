namespace GoodHamburgerProject.Exceptions
{
    public class OrderNotFoundException : OrderException
    {
        public OrderNotFoundException()
            : base("Order not found.") { }
    }
}
