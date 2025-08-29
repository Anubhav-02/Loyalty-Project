namespace LoyaltyApi.DTOs
{
    public class AddPointsRequest
    {
        public int MemberId { get; set; }
        public decimal PurchaseAmount { get; set; } // in INR
        public string? Description { get; set; }
    }

    public class PointsResponse
    {
        public int MemberId { get; set; }
        public int PointsBalance { get; set; }
        public int LastChange { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
