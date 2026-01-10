using System.ComponentModel.DataAnnotations;

namespace CouponServer.Dto;

public record CouponIssueResponse(
    [Required] int UserId,
    [Required] int CouponId
);