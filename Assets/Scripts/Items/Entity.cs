using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IPlaceable, IBreakable, IRepairable
{
    [Header("Debug Stuff")]
    public float itemPlaceHeight;
    public ItemStates currItemState = ItemStates.Fixed;
    public ItemActionState currItemActionState = ItemActionState.None;

    // References to the Meshes
    [SerializeField] private GameObject meshUnfired;
    [SerializeField] private GameObject meshFixed;
    [SerializeField] private GameObject meshBroken;

    #region IPlaceable
    public float GetPlaceHeight()
    {
        return itemPlaceHeight;
    }
    #endregion

    #region IBreakable
    public abstract void AttemptBreak(int itemTier);
    #endregion

    #region IRepairable
    public abstract void StartRepairing();

    public abstract void StopRepairing();

    public abstract void CompleteRepairing();

    #endregion

    #region Mesh Methods
    protected void UpdateItemMesh()
    {
        meshUnfired.SetActive(currItemState == ItemStates.Unfired);
        meshFixed.SetActive(currItemState == ItemStates.Fixed);
        meshBroken.SetActive(currItemState == ItemStates.Broken);
    }

    public void SetMeshColliders(bool isActive)
    {
        meshUnfired.GetComponent<Collider>().enabled = isActive;
        meshFixed.GetComponent<Collider>().enabled = isActive;
        meshBroken.GetComponent<Collider>().enabled = isActive;
    }
    #endregion

    // public abstract void AbstractMethod();
}
