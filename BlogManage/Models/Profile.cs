using System;
using System.Collections.Generic;

namespace BlogManage.Models;

public partial class Profile
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public bool Sex { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Avatar { get; set; }

    public DateOnly CreatedDate { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public int RoleId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Role Role { get; set; } = null!;
}
