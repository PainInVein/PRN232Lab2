namespace PRN232.NMS.API.Models.RequestModels.TagRequests
{
    public class CreateTagRequest
    {
        public string TagName { get; set; } = null!;

        public string? Note { get; set; }
    }
}
