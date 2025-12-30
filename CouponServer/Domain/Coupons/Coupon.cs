namespace CouponServer.Domain.Coupons;

public class Coupon
{
    public int UserId { get; set; }
    public int CouponId { get; private set; }
    
    private Coupon() { }

    public static Coupon Create(int userId)
    {
        return new Coupon
        {
            UserId = userId
        };
    }
}