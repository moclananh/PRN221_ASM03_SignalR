using System;
using System.Collections.Generic;

namespace PRN221_Assignment03.Models;

public partial class AppUser
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
