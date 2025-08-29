using System.ComponentModel.DataAnnotations;

namespace LoyaltyApi.Models
{
    public class Member
    {
        public int Id { get; set; }
        [Required, MaxLength(15)]
        public string MobileNumber { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Name { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public int PointsBalance { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
