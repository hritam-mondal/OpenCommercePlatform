namespace OCP.API.DTOs;

public sealed class VerifyOtpDto
{
    public string PhoneNo { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
}
