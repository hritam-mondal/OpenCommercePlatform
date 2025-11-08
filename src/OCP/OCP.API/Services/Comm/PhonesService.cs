using OCP.API.Repositories;

namespace OCP.API.Services.Comm;

public class PhonesService(IPhoneRepository repo) : IPhonesService
{
    public async Task<bool> SendOtpAsync(string phoneNo, int? applicationUserId, CancellationToken ct)
    {
        var otp = Random.Shared.Next(100000, 999999).ToString();
        var expireAt = DateTime.UtcNow.AddMinutes(10);
        return await repo.SendOtpAsync(phoneNo, otp, expireAt, applicationUserId, ct);
    }

    public Task<bool> VerifyOtpAsync(string phoneNo, string otp, CancellationToken ct)
    {
        return repo.VerifyOtpAsync(phoneNo, otp, ct);
    }
}
