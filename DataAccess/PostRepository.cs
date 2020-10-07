using Microsoft.EntityFrameworkCore;

using Models.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PostRepository : RepositoryBase<Post>
    {
        public override async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await context.Set<Post>()
                .Include("Comments")
                .Include("User")
                .ToListAsync();
        }
    }
}