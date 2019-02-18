
namespace Ags.Data.Domain.Blogs
{
    /// <summary>
    /// Represents a blog post tag
    /// </summary>
    public partial class BlogPostTag
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tagged product count
        /// </summary>
        /// <value>The blog post count.</value>
        public int BlogPostCount { get; set; }
    }
}