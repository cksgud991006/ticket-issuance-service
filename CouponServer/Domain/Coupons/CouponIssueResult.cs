using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using CouponServer.Dto;

namespace CouponServer.Domain.Coupons;

public enum CouponIssueFailureReason
{
    AlreadyIssued,
    SoldOut,
    InvalidId
}

public class CouponIssueResult
{
    public bool IsSuccess { get; }
    public CouponIssueFailureReason? FailureReason { get; }
    public Coupon? Coupon { get; }
    private CouponIssueResult(bool success, CouponIssueFailureReason? failureReason, Coupon? coupon)
    {
        IsSuccess = success;
        FailureReason = failureReason;
        Coupon = coupon;
    }

    public static CouponIssueResult Success(Coupon coupon) 
        => new CouponIssueResult(true, null, coupon);
    public static CouponIssueResult AlreadyIssued()
        => new CouponIssueResult(false, CouponIssueFailureReason.AlreadyIssued, null);
    public static CouponIssueResult SoldOut() 
        => new CouponIssueResult(false, CouponIssueFailureReason.SoldOut, null);
    public static CouponIssueResult InvalidId()
        => new CouponIssueResult(false, CouponIssueFailureReason.InvalidId, null);
    
}