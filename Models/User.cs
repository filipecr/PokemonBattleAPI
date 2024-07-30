using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheProjectTascamon.Models;

public partial class User 
{
    public  int Id { get; set; }
    public  string Email { get; set; }
    public  string PasswordHash { get; set; }
    public  string UserName { get; set; }
    public virtual ICollection<Battle> Battles { get; set; } = new List<Battle>();
}
