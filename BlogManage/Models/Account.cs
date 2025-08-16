using System;
using System.Collections.Generic;

namespace BlogManage.Models;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Status { get; set; }

    public DateOnly CreatedDate { get; set; }

    public DateOnly? UpdateDate { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
