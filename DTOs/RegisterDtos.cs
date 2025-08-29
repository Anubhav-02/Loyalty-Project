namespace LoyaltyApi.DTOs
{
    public class RegisterRequest
    {
        public string MobileNumber { get; set; } = string.Empty;
        public string? Name { get; set; }
    }

    public class RegisterResponse
    {
        public int MemberId { get; set; }
        public string MobileNumber { get; set; } = string.Empty;
        public string OtpForDemo { get; set; } = string.Empty; // return OTP in response for demo
        public string Message { get; set; } = "OTP generated. Use verify endpoint.";
    }
}
