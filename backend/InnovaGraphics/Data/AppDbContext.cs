using InnovaGraphics.Data.Configuration;
using InnovaGraphics.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InnovaGraphics.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Theory> Theory { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<Teacher> Teacher { get; set; }

        public DbSet<ShopItem> ShopItems { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Background> Background { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Hint> Hint { get; set; }
        public DbSet<MusicTheme> MusicTheme { get; set; }
        public DbSet<PlanetAccess> PlanetAccess { get; set; }

        public DbSet<Question> Question { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<PlanetInfo> PlanetInfo { get; set; }
        public DbSet<Planet> Planet { get; set; }

        public DbSet<Method> Method { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<CompetitionInfo> CompetitionInfo { get; set; }
        public DbSet<ChatMessage> ChatMessage { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Battle> Battle { get; set; }
        public DbSet<TokenManager> TokenManager { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<UserTest> UserTest { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new TestConfiguration());
            modelBuilder.ApplyConfiguration(new ShopItemConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new PlanetConfiguration());
            modelBuilder.ApplyConfiguration(new PlanetAccessConfiguration());
            modelBuilder.ApplyConfiguration(new MusicThemeConfiguration());
            modelBuilder.ApplyConfiguration(new HintConfiguration());
            modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new BattleConfiguration());
            modelBuilder.ApplyConfiguration(new BackgroundConfiguration());
            modelBuilder.ApplyConfiguration(new AvatarConfiguration());
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            modelBuilder.ApplyConfiguration(new UserTestConfiguration());


            modelBuilder.Entity<ShopItem>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }

    }



}
