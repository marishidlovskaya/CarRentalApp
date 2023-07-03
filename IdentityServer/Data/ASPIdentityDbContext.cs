using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data
{
    public class ASPIdentityDbContext: IdentityDbContext
    {
        public ASPIdentityDbContext(DbContextOptions<ASPIdentityDbContext> options) : base(options)
        {

        }
    }
}
