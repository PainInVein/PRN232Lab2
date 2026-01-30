namespace PRN232.NMS.Services.Models.ResponseModels.SystemAccountResponses
{
    public class UserResponse
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; } = null!;
        public string AccountEmail { get; set; } = null!;
        public string AccountRole { get; set; } = null!;
    }
}
