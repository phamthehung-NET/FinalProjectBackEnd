using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectBackEnd.Data;

public class ApplicationDbContext : IdentityDbContext<CustomUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Classroom> Classrooms { get; set; }

    public DbSet<ClassTeacherSubject> ClassTeacherSubjects { get; set;}

    public DbSet<Comment> Comments { get; set; }

    public DbSet<GroupChat> GroupChats { get; set; }

    public DbSet<Marks> Marks { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Post> Posts { get; set; }

    public DbSet<ReplyComment> ReplyComments { get; set; }

    public DbSet<StudentClass> StudentClasses { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public DbSet<TeacherSubject> TeacherSubjects { get; set; }

    public DbSet<UserGroupChat> UserGroupChats { get; set; }

    public DbSet<UserInfo> UserInfos { get; set; }

    public DbSet<UserLikeComment> UserLikeComments { get; set; }

    public DbSet<UserLikePost> UserLikePosts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        this.SeedUsers(builder);
        this.SeedRoles(builder);
        this.SeedUserRoles(builder);
        this.SeedUserInfos(builder);
        builder.Entity<CustomUser>().HasOne(u => u.UserInfo).WithOne(i => i.CustomUser).HasForeignKey<UserInfo>(e => e.UserId);
    }

    private void SeedUsers(ModelBuilder builder)
    {
        var passwordHasher = new PasswordHasher<CustomUser>();
        CustomUser user = new CustomUser()
        {
            Id = "1",
            UserName = "Principal",
            Email = "principal@gmail.com",
            NormalizedUserName = "principal",
            PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
            LockoutEnabled = true,
            EmailConfirmed = true,
        };
        CustomUser user2 = new CustomUser()
        {
            Id = "2",
            UserName = "Vice-Principal",
            Email = "vice-principal@gmail.com",
            NormalizedUserName = "vice-principal",
            PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
            LockoutEnabled = true,
            EmailConfirmed = true,
        };


        builder.Entity<CustomUser>().HasData(user);
        builder.Entity<CustomUser>().HasData(user2);
    }

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole() { Id = "1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "admin" },
            new IdentityRole() { Id = "2", Name = "Teacher", ConcurrencyStamp = "2", NormalizedName = "teacher" },
            new IdentityRole() { Id = "3", Name = "Student", ConcurrencyStamp = "3", NormalizedName = "student" }
            );
    }
    private void SeedUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "1", UserId = "1" });
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "1", UserId = "2" });
    }

    private void SeedUserInfos(ModelBuilder builder)
    {
        builder.Entity<UserInfo>().HasData(
            new UserInfo() { Id = 1, UserId = "1", },
            new UserInfo() { Id = 2, UserId = "2", }
            );
    }
}
