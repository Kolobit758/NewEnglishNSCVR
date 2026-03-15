using UnityEngine;
using System;

public static class GameEvent
{
    public static Action OnNPCSpawn;
    public static Action OnNPCComming;
    public static Action OnNPCOrdered;
    public static Action OnPlayerConfirmedOrder;
    public static Action OnCookingFinished;
    public static Action OnPlayerServed;
    public static Action OnNPCPaid;
    public static Action OnNPCLeave;
    public static Action OnNPCLeaveFinished;
    public static Action OnGameEnding;

    // ===== Conversation =====
    public static Action OnConversationStart;
    public static Action OnConversationEnd;
    public static Action OnNPCchangeConver;
}
