using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Ags.Data.Common;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.Blogs;
using Ags.Data.Domain.Catalog;
using Ags.Data.Domain.Common;
using Ags.Data.Domain.Configuration;
using Ags.Data.Domain.Customers;
using Ags.Data.Domain.Directory;
using Ags.Data.Domain.Logging;
using Ags.Data.Domain.Media;
using Ags.Data.Domain.Message;
using Ags.Data.Domain.News;
using Ags.Data.Domain.Polls;
using Ags.Data.Domain.Seo;
using Ags.Data.Domain.Stores;
using Ags.Data.Domain.Tasks;
using Ags.Data.Domain.Topics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ags.Data.Domain
{
    /// <summary>
    /// ApplicationDbContext
    /// </summary>
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDbContext
    {
        /// <summary>
        /// ApplicationDbContext
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
        public DbSet<UserAudit> UserAuditEvents { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTemplate> CategoryTemplates { get; set; }
        public DbSet<CategoryNews> CategoryNewses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<GenericAttribute> GenericAttributes { get; set; }
        public DbSet<Modules> Moduleses { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<SearchTerm> SearchTerms { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<PhotoGallery> PhotoGalleries { get; set; }
        public DbSet<PhotoGalleryMapping> PhotoGalleryMappings { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureBinary> PictureBinaries { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SliderPictureMapping> SliderPictureMappings { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<VideoGallery> VideoGalleries { get; set; }
        public DbSet<EnewsPaper> EnewsPapers { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<NewsPaperCategory> NewsPaperCategories { get; set; }
        public DbSet<NewsPictureMapping> NewsPictureMappings { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollAnswer> PollAnswers { get; set; }
        public DbSet<PollVotingRecord> PollVotingRecords { get; set; }
        public DbSet<UrlRecord> UrlRecords { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ScheduleTask> ScheduleTasks { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicTemplate> TopicTemplates { get; set; }
        public DbSet<EmailAccount> EmailAccounts { get; set; }
        public DbSet<VisitorCounter> VisitorCounter { get; set; }
        public DbSet<NewsCounter> NewsCounter { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
            modelBuilder.Query<PictureHashItem>();
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }
        #region Methods

        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of entity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>A set for the given entity type</returns>
        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }


        /// <summary>
        /// Creates a LINQ query for the query type based on a raw SQL query
        /// </summary>
        /// <typeparam name="TQuery">Query type</typeparam>
        /// <param name="sql">The raw SQL query</param>
        /// <returns>An IQueryable representing the raw SQL query</returns>
        public virtual IQueryable<TQuery> QueryFromSql<TQuery>(string sql) where TQuery : class
        {
            return this.Query<TQuery>().FromSql(sql);
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            return this.Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }


        /// <summary>
        /// Executes the given SQL against the database
        /// </summary>
        /// <param name="sql">The SQL to execute</param>
        /// <param name="doNotEnsureTransaction">true - the transaction creation is not ensured; false - the transaction creation is ensured.</param>
        /// <param name="timeout">The timeout to use for command. Note that the command timeout is distinct from the connection timeout, which is commonly set on the database connection string</param>
        /// <param name="parameters">Parameters to use with the SQL</param>
        /// <returns>The number of rows affected</returns>
        public virtual int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            int? previousTimeout = this.Database.GetCommandTimeout();
            this.Database.SetCommandTimeout(timeout);

            int result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = this.Database.BeginTransaction())
                {
                    result = this.Database.ExecuteSqlCommand(sql, parameters);
                    transaction.Commit();
                }
            }
            else
            {
                result = this.Database.ExecuteSqlCommand(sql, parameters);
            }

            //return previous timeout back
            this.Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        /// <summary>
        /// Detach an entity from the context
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        public virtual void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> entityEntry = this.Entry(entity);
            if (entityEntry == null)
            {
                return;
            }

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        #endregion
    }
}
