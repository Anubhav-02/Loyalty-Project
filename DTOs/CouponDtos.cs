namespace LoyaltyApi.DTOs
{
    public class RedeemCouponRequest
    {
        public int MemberId { get; set; }
        public int TierPoints { get; set; } // 500 or 1000
    }

    public class CouponResponse
    {
        public int MemberId { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public int PointsDeducted { get; set; }
        public decimal CouponValue { get; set; }
        public int NewBalance { get; set; }
        public string Message { get; set; } = "Coupon created";
    }
}
