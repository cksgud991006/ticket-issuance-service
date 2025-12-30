using System.ComponentModel.DataAnnotations;

namespace CouponServer.Dto;

public record CouponIssueRequest(
    [Required] int UserId,
    [Required] string IdempotencyKey
);