using System;
using System.Collections.Generic;

namespace BlogManage.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? CreatedDate { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
