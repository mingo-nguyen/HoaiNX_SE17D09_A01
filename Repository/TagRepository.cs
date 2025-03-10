using Domain;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementContext _context;

        public TagRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }
    }
}
