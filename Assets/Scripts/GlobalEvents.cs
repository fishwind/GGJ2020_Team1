using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GenericEvent();
public delegate void GenericIntEvent(int val);
public delegate void GenericBoolEvent(bool val);
public delegate void GenericFloatEvent(float val);

//this class has all the communication events. All features should use this to talk to each other and not directly access.

public static class GlobalEvents
{
    public static GenericEvent OnPlayerApproachHouse;
    public static void SendPlayerApproachHouse()
    {
        OnPlayerApproachHouse?.Invoke();
    }
    public static GenericEvent OnPlayerStartDestroyDoor;
    public static void SendPlayerStartDestroyDoor()
    {
        OnPlayerStartDestroyDoor?.Invoke();
    }
    public static GenericEvent OnPlayerDestroyedDoor;
    public static void SendPlayerDestroyedDoor()
    {
        OnPlayerDestroyedDoor?.Invoke();
    }
    public static GenericIntEvent OnPlayerStartDestroyAll;
    public static void SendPlayerStartDestroyAll(int whichtier)
    {
        OnPlayerStartDestroyAll?.Invoke(whichtier);
    }
    public static GenericIntEvent OnPlayerDestroyedAll;
    public static void SendPlayerDestroyedAll(int whichtier)
    {
        OnPlayerDestroyedAll?.Invoke(whichtier);
    }
    public static GenericEvent OnPlayerStartLeave;
    public static void SendPlayerStartLeave()
    {
        OnPlayerStartLeave?.Invoke();
    }
    public static GenericIntEvent OnPlayerLeaveComplete;
    public static void SendPlayerLeaveComplete(int whichtier)
    {
        OnPlayerLeaveComplete?.Invoke(whichtier);
    }

    public static GenericBoolEvent OnPlayerMoveAllowChange;
    public static void SendPlayerMoveAllowChange(bool canMove)
    {
        OnPlayerMoveAllowChange?.Invoke(canMove);
    }

    public static GenericFloatEvent OnRepairGameplayStart;
    public static void SendRepairGameplayStart(float duration)
    {
        OnRepairGameplayStart?.Invoke(duration);
    }

    public static GenericFloatEvent OnRepairReturnDuration;
    public static void SendRepairReturnDuration(float duration)
    {
        OnRepairReturnDuration?.Invoke(duration);
    }

    public static GenericEvent OnHeroQuestComplete;
    public static void SendHeroQuestComplete()
    {
        OnHeroQuestComplete?.Invoke();
    }

    public static GenericEvent OnHeroReturningToHouse;
    public static void SendHeroReturningToHouse()
    {
        OnHeroReturningToHouse?.Invoke();
    }

    public static GenericEvent OnCollectCoins;
    public static void SendCollectCoins()
    {
        OnCollectCoins?.Invoke();
    }
}
