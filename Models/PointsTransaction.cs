namespace LoyaltyApi.Models
{
    public class PointsTransaction
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public int PointsChange { get; set; } // positive for add, negative for redeem
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
