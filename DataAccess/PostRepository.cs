using Microsoft.EntityFrameworkCore;

using Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PostRepository : RepositoryBase<Post>
    {
        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await context.Set<Post>()
                .Include(p => p.User)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }
    }
}