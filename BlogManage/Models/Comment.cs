using System;
using System.Collections.Generic;

namespace BlogManage.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int BlogId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public bool Status { get; set; }

    public DateOnly CreatedDate { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual Profile User { get; set; } = null!;
}
