namespace LoyaltyApi.DTOs
{
    public class VerifyOtpRequest
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
    }

    public class VerifyOtpResponse
    {
        public int MemberId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = "Verification successful";
    }
}
