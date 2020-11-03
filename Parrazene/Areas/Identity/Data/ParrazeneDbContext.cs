using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parrazene.Areas;
using Parrazene.Areas.Identity.Data;

namespace Parrazene.Data
{
    public class ParrazeneDbContext : IdentityDbContext<User> 
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationUser> ConversationUsers { get; set; }
        public DbSet<ConversationMessage> ConversationMessages { get; set; }

        public ParrazeneDbContext(DbContextOptions<ParrazeneDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //++++++++++++++++++++++++++ a one to many and a one to one relationship formation between Conversation and Conversation Message
            // Data anotations and navigation properties also in place to support this 

            // You may use this instead of the inverse property
            // // The One to Many relationship between
            // // User.Id (Principal End) and Photo.OwnerId
            // modelBuilder.Entity<Photo>()
            //     .HasOne(p => p.Owner)
            //     .WithMany(u => u.Photos)
            //     .HasForeignKey(p => p.OwnerId);

            // Establishes 1:0..1 relationship between
            // Photo.Id (Principal End) and User.DefaultPhoto (Dependent end)
            //Ref- https://entityframeworkcore.com/knowledge-base/47728331/how-to-define-multiple-relationships-between-two-entities-
           
            builder.Entity<Conversation>()
                .HasOne(c => c.LatestMessage)
                .WithOne() //we leave this empty because it doesn't correspond to a navigation property in the Photos table
                .HasForeignKey<Conversation>(c => c.LatestMessageId);

            //++++++++++++++++++++++++++ a one to many and a one to one relationship formation between Conversation and Conversation Message

        }


    }
}
