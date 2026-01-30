namespace PRN232.NMS.Services.Models.ResponseModels.TagResponses
{
    public class GetAllTagResponse
    {
        public int TagId { get; set; }

        public string TagName { get; set; } = null!;

        public string? Note { get; set; }
    }
}
