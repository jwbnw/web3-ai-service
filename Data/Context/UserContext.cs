using Microsoft.EntityFrameworkCore;
using Web3Ai.Service.Data.Entities;

namespace Web3Ai.Service.Data.Context;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set;} = null!;
}