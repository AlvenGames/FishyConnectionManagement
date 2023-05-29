using System;
using FishNet.Broadcast;

namespace FishNet.ConnectionManagement
{
    public interface IApprovalMethod<TPayload> where TPayload : struct, IBroadcast
    {
        TPayload ConnectionPayload { get; set; }
        event Action<ApprovalResult<TPayload>> OnConnectionPayloadApproval;
    }
}