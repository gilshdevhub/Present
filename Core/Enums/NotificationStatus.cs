using System.Runtime.Serialization;

namespace Core.Enums;

public enum NotificationStatus
{
    [EnumMember(Value = "Waiting To Handle")]
    WaitingToHandle = 1,
    [EnumMember(Value = "Canceled")]
    Canceled = 2,
    [EnumMember(Value = "Waiting To Send")]
    WaitingToSend = 3,
    [EnumMember(Value = "Sent")]
    Sent = 4,
    CanceledByMachine = 5
}
