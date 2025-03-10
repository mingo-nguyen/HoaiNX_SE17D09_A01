using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
    }
}
