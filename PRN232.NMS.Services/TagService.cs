using PRN232.NMS.Repo.EntityModels;
using PRN232.NMS.Services.Interfaces;
using Repositories;

namespace PRN232.NMS.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService() => _unitOfWork ??= new UnitOfWork();

        public async Task<Tag> GetByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.TagRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n");
            }
            return null;

        }
    }
}
