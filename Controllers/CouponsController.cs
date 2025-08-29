using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LoyaltyApi.Data;
using LoyaltyApi.DTOs;
using LoyaltyApi.Models;

namespace LoyaltyApi.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    [Authorize]
    public class CouponsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CouponsController(AppDbContext db) { _db = db; }

        private int? GetUserId()
        {
            var idStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        [HttpPost("redeem")]
        public async Task<ActionResult<CouponResponse>> Redeem(RedeemCouponRequest req)
        {
            var userId = GetUserId();
            if (userId == null || userId.Value != req.MemberId)
                return Forbid();

            if (req.TierPoints != 500 && req.TierPoints != 1000)
                return BadRequest("TierPoints must be 500 or 1000");

            var member = await _db.Members.FindAsync(req.MemberId);
            if (member == null) return NotFound("Member not found");
            if (member.PointsBalance < req.TierPoints)
                return BadRequest("Insufficient points");

            decimal value = req.TierPoints == 500 ? 50m : 100m;

            member.PointsBalance -= req.TierPoints;
            member.UpdatedAt = DateTime.UtcNow;

            var code = $"COUPON{(int)value}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
            var redemption = new CouponRedemption
            {
                MemberId = member.Id,
                CouponCode = code,
                PointsRedeemed = req.TierPoints,
                CouponValue = value
            };
            _db.CouponRedemptions.Add(redemption);
            _db.PointsTransactions.Add(new PointsTransaction
            {
                MemberId = member.Id,
                PointsChange = -req.TierPoints,
                Description = $"Redeemed for ₹{value} coupon"
            });

            await _db.SaveChangesAsync();

            return Ok(new CouponResponse
            {
                MemberId = member.Id,
                CouponCode = code,
                PointsDeducted = req.TierPoints,
                CouponValue = value,
                NewBalance = member.PointsBalance,
                Message = "Coupon redeemed"
            });
        }
    }
}
