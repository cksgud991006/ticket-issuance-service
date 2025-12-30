using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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

    private CouponIssueResult(bool success, CouponIssueFailureReason? failureReason)
    {
        IsSuccess = success;
        FailureReason = failureReason;
    }

    public static CouponIssueResult Success() 
        => new CouponIssueResult(true, null);
    public static CouponIssueResult AlreadyIssued()
        => new CouponIssueResult(false, CouponIssueFailureReason.AlreadyIssued);
    public static CouponIssueResult SoldOut() 
        => new CouponIssueResult(false, CouponIssueFailureReason.SoldOut);
    public static CouponIssueResult InvalidId()
        => new CouponIssueResult(false, CouponIssueFailureReason.InvalidId);
    
}