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
    [Route("api/points")]
    [Authorize]
    public class PointsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PointsController(AppDbContext db) { _db = db; }

        private int? GetUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(idStr, out var id)) return id;
            return null;
        }

        [HttpPost("add")]
        public async Task<ActionResult<PointsResponse>> Add(AddPointsRequest req)
        {
            var userId = GetUserId();
            if (userId == null || userId.Value != req.MemberId)
                return Forbid();

            var member = await _db.Members.FindAsync(req.MemberId);
            if (member == null) return NotFound("Member not found");
            if (!member.IsVerified) return BadRequest("Member not verified");

            // Rule: every ₹100 = 10 points
            var pointsToAdd = (int)Math.Floor(req.PurchaseAmount / 100m) * 10;
            if (pointsToAdd <= 0)
            {
                return BadRequest("Purchase amount too low to earn points.");
            }

            member.PointsBalance += pointsToAdd;
            member.UpdatedAt = DateTime.UtcNow;
            _db.PointsTransactions.Add(new PointsTransaction
            {
                MemberId = member.Id,
                PointsChange = pointsToAdd,
                Description = string.IsNullOrWhiteSpace(req.Description) ? $"Earned for purchase ₹{req.PurchaseAmount}" : req.Description
            });
            await _db.SaveChangesAsync();

            return Ok(new PointsResponse
            {
                MemberId = member.Id,
                PointsBalance = member.PointsBalance,
                LastChange = pointsToAdd,
                Message = "Points added"
            });
        }

        [HttpGet("{memberId:int}")]
        public async Task<ActionResult<PointsResponse>> GetPoints(int memberId)
        {
            var userId = GetUserId();
            if (userId == null || userId.Value != memberId)
                return Forbid();

            var member = await _db.Members.FindAsync(memberId);
            if (member == null) return NotFound("Member not found");

            return Ok(new PointsResponse
            {
                MemberId = member.Id,
                PointsBalance = member.PointsBalance,
                LastChange = 0,
                Message = "Balance fetched"
            });
        }
    }
}
