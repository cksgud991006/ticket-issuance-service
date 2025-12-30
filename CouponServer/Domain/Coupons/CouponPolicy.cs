using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CouponServer.Domain.Coupons;

public class CouponPolicy
{
    public int Id { get; set; }
    public int TotalQuantity { get; set;}
    public int IssuedCoupons { get; set;}

    public static CouponPolicy CreateDefault()
        => new CouponPolicy 
        {
            TotalQuantity = 1000,
            IssuedCoupons = 0
        };
}
