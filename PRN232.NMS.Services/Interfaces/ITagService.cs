using PRN232.NMS.Repo.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.NMS.Services.Interfaces
{
    public interface ITagService
    {
        Task<Tag> GetByIdAsync(int id);
    }
}
