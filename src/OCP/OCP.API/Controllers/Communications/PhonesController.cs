using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OCP.API.DTOs;
using OCP.API.Services.Comm;

namespace OCP.API.Controllers.Communications;

[Route("api/v1/comm/[controller]")]
[ApiController]
[Authorize]
public class PhonesController(IPhonesService service) : ControllerBase
{
    [HttpPost("send-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpDto dto, CancellationToken ct)
    {
        var ok = await service.SendOtpAsync(dto.PhoneNo, null, ct);
        return ok ? Ok(new { message = "OTP sent" }) : Problem("Failed to send OTP");
    }

    [HttpPost("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> Verify([FromBody] VerifyOtpDto dto, CancellationToken ct)
    {
        var ok = await service.VerifyOtpAsync(dto.PhoneNo, dto.Otp, ct);
        return ok ? Ok(new { message = "Verified" }) : Unauthorized(new { error = "Invalid or expired OTP" });
    }
}
