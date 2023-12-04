using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIToDoList.Model;

public partial class UserT
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TaskT> TaskTs { get; } = new List<TaskT>();
}
