using System.ComponentModel;

namespace TodoDesafio.Domain.Enums;

public enum Status
{
    [Description("Pending")]
    Pending = 0,

    [Description("In Progress")]
    InProgress = 1,

    [Description("Completed")]
    Completed = 2
}