using CouponServer.Services;
using CouponServer.Domain.Coupons;
using CouponServer.Dto;

namespace CouponServer.Endpoints;

public static class CouponEndpoint
{
    public static void MapCouponEndPoints(this WebApplication app)
    {
        // GET 
        app.MapGet("/coupons/users/{userId}", GetCouponByUserId);
        app.MapGet("/coupons/{couponId}", GetCouponByCouponId);
        
        // POST
        app.MapPost("/coupons/issue", IssueCoupon);
    }

    private static async Task<IResult> GetCouponByUserId(
        int userId)
    {
        return Results.Ok();
    } 

    private static async Task<IResult> GetCouponByCouponId(
        int couponId)
    {
        return Results.Ok();
    } 

    private static async Task<IResult> IssueCoupon(
        CouponIssueRequest request,
        ICouponService service)
    {
        var result = await service.IssueAsync(request.UserId);
        return result switch
        {
            { IsSuccess: true } =>
                Results.Ok(),

            { FailureReason: CouponIssueFailureReason.AlreadyIssued } =>
                Results.Conflict("Coupon already issued"),

            { FailureReason: CouponIssueFailureReason.SoldOut } =>
                Results.Conflict("Coupon sold out"),

            _ =>
                Results.StatusCode(500)
        };
    }
}