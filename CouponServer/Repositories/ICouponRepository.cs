

using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace CouponServer.Repositories;

public interface ICouponRepository
{
    public Task<bool> HasUserReceivedCoupon(int userId);

    public Task<bool> TryIssueCoupon(int userId);
}