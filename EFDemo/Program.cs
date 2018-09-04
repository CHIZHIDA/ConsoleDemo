using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDemo
{
    [Table("Blog")]
    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }

    [Table("User")]
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string DisplayName { get; set; }
    }

    [Table("Post")]
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class EFCodeFirstDbContext:DbContext
    {
        public EFCodeFirstDbContext():base("name=EFDbConn")
        { }

        public DbSet<Blog> Blog { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EFCodeFirstDbContext())
            {
                // Create and save a new Blog
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };
                db.Blog.Add(blog);
                db.SaveChanges();
                // Display all Blog from the database
                var query = from b in db.Blog
                            orderby b.Name
                            select b;

                Console.WriteLine("All blog in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.Write("Enter a name for a new User: ");
                var username = Console.ReadLine();

                var user = new User { Username = username };
                db.User.Add(user);
                db.SaveChanges();
                // Display all Blogs from the database
                var list = from b in db.User
                            orderby b.Username
                            select b;

                Console.WriteLine("All User in the database:");
                foreach (var item in list)
                {
                    Console.WriteLine(item.Username);
                }




                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
