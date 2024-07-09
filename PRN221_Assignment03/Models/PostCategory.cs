using System;
using System.Collections.Generic;

namespace PRN221_Assignment03.Models;

public partial class PostCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
