using OCP.API.Data;
using OCP.API.Models;

namespace OCP.API.Repositories;

public class PhoneRepository(ApplicationDbContext context) : IPhoneRepository
{
    public async Task<bool> SendOtpAsync(string phoneNo,
        string otp,
        DateTime expireAt,
        int? applicationUserId,
        CancellationToken ct)
    {
        var phone = new Phone()
        {
            PhoneNo = phoneNo,
            Otp = otp,
            ExpireDate = expireAt,
            IsVerified = false,
            ApplicationUserId = applicationUserId
        };

        await context.Phones.AddAsync(phone, ct);
        var affected = await context.SaveChangesAsync(ct);

        return affected > 0;
    }

    public async Task<bool> VerifyOtpAsync(string phoneNo, string otp, CancellationToken ct)
    {
        var phone = await context.Phones
            .FirstOrDefaultAsync(p =>
                    p.PhoneNo == phoneNo &&
                    p.Otp == otp &&
                    (p.ExpireDate == null || p.ExpireDate >= DateTimeOffset.UtcNow),
                ct);

        if (phone == null)
            return false;

        phone.IsVerified = true;
        var affected = await context.SaveChangesAsync(ct);

        return affected > 0;
    }
}
