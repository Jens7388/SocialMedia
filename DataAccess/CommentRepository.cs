using Microsoft.EntityFrameworkCore;
using Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommentRepository : RepositoryBase<Comment>
    {
        public override async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await context.Set<Comment>()
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
                .ToListAsync();
        }
        public override async Task<Comment> GetByIdAsync(int? id)
        {
            return await context.Set<Comment>()
                .Include(c => c.User)
                .OrderByDescending(c => c.Created)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
