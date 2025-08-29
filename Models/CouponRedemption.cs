namespace LoyaltyApi.Models
{
    public class CouponRedemption
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int PointsRedeemed { get; set; }
        public decimal CouponValue { get; set; } // INR value
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
