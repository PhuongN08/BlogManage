using System;
using System.Collections.Generic;

namespace BlogManage.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Summary { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? Thumbnail { get; set; }

    public int AuthorId { get; set; }

    public int StatusId { get; set; }

    public int CategoryId { get; set; }

    public DateOnly CreatedDate { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public virtual Profile Author { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Status Status { get; set; } = null!;
}
