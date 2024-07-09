using System;
using System.Collections.Generic;

namespace PRN221_Assignment03.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int? AuthorId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string PublishStatus { get; set; }

    public int? CategoryId { get; set; }

    public virtual AppUser? Author { get; set; }

    public virtual PostCategory? Category { get; set; }
}
