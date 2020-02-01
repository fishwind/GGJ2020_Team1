using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IPlaceable, IBreakable, IRepairable
{
    public float itemPlaceHeight;

    [Header("DO NOT EDIT IN INSPECTOR")]
    public ItemStates currItemState = ItemStates.Fixed;
    public ItemActionState currItemActionState = ItemActionState.None;

    #region IPlaceable
    public virtual float GetPlaceHeight()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IBreakable
    public virtual void AttemptBreak()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IRepairable
    public virtual void StartRepairing()
    {
        throw new System.NotImplementedException();
    }

    public virtual void StopRepairing()
    {
        throw new System.NotImplementedException();
    }

    public virtual void CompleteRepairing()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    // public abstract void AbstractMethod();
}
