namespace OCP.API.Services.Comm;

public interface IPhonesService
{
    Task<bool> SendOtpAsync(string phoneNo, int? applicationUserId, CancellationToken ct);
    Task<bool> VerifyOtpAsync(string phoneNo, string otp, CancellationToken ct);
}
