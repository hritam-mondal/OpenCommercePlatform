namespace OCP.API.Repositories;

public interface IPhoneRepository
{
    Task<bool> SendOtpAsync(string phoneNo, string otp, DateTime expireAt, int? applicationUserId,
        CancellationToken ct);

    Task<bool> VerifyOtpAsync(string phoneNo, string otp, CancellationToken ct);
}
