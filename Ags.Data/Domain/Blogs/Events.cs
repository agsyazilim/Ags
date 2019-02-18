

namespace Ags.Data.Domain.Blogs
{
    /// <summary>
    /// Blog post comment approved event
    /// </summary>
    public class BlogCommentApprovedEvent
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        public BlogCommentApprovedEvent(BlogComment blogComment)
        {
            this.BlogComment = blogComment;
        }

        /// <summary>
        /// Blog post comment
        /// </summary>
        /// <value>The blog comment.</value>
        public BlogComment BlogComment { get; }
    }
}