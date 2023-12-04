using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace APIToDoList.Model;

public partial class TaskT
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateOnly? Deadline { get; set; }

    public int? Priority { get; set; }

    public bool? Done { get; set; }

    [JsonIgnore]
    public virtual UserT User { get; set; } = null!;
}
