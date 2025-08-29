using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoyaltyApi.Data;
using LoyaltyApi.DTOs;
using LoyaltyApi.Models;
using LoyaltyApi.Utils;

namespace LoyaltyApi.Controllers
{
    [ApiController]
    [Route("api/member")]
    public class MemberController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public MemberController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.MobileNumber))
            {
                return BadRequest("MobileNumber is required.");
            }
            var existing = await _db.Members.FirstOrDefaultAsync(m => m.MobileNumber == req.MobileNumber);
            if (existing == null)
            {
                existing = new Member
                {
                    MobileNumber = req.MobileNumber,
                    Name = req.Name,
                    IsVerified = false
                };
                _db.Members.Add(existing);
            }

            // generate dummy OTP
            var otp = new Random().Next(100000, 999999).ToString();
            existing.OtpCode = otp;
            existing.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5);
            existing.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new RegisterResponse
            {
                MemberId = existing.Id,
                MobileNumber = existing.MobileNumber,
                OtpForDemo = otp
            });
        }

        [HttpPost("verify")]
        public async Task<ActionResult<VerifyOtpResponse>> Verify(VerifyOtpRequest req)
        {
            var member = await _db.Members.FirstOrDefaultAsync(m => m.MobileNumber == req.MobileNumber);
            if (member == null) return NotFound("Member not found");
            if (member.OtpCode != req.OtpCode || member.OtpExpiresAt < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired OTP");
            }
            member.IsVerified = true;
            member.OtpCode = null;
            member.OtpExpiresAt = null;
            member.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            var token = JwtTokenHelper.GenerateToken(
                member.Id,
                _config["Jwt:Issuer"] ?? "LoyaltyApi",
                _config["Jwt:Audience"] ?? "LoyaltyApiUsers",
                _config["Jwt:Key"] ?? "super_secret_demo_key_change_me");

            return Ok(new VerifyOtpResponse { MemberId = member.Id, Token = token });
        }
    }
}
