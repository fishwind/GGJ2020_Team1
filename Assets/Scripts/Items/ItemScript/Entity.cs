using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IPlaceable
{
    [Header("Debug Stuff")]
    public float itemPlaceHeight;
    public ItemStates currItemState = ItemStates.Fixed;
    public ItemActionState currItemActionState = ItemActionState.None;

    // References to the Meshes
    [SerializeField] private GameObject meshUnfired = null;
    [SerializeField] private GameObject meshFixed = null;
    [SerializeField] private GameObject meshBroken = null;

    #region IPlaceable
    public float GetPlaceHeight()
    {
        return itemPlaceHeight;
    }
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
        if (meshUnfired != null)
            meshUnfired.GetComponent<Collider>().enabled = isActive;
        if (meshFixed != null)
            meshFixed.GetComponent<Collider>().enabled = isActive;
        if (meshBroken != null)
            meshBroken.GetComponent<Collider>().enabled = isActive;
    }
    #endregion

    #region Old Code
    /*
    #region IBreakable
    public abstract void AttemptBreak(int itemTier);
    #endregion

    #region IRepairable
    public abstract void StartRepairing();

    public abstract void StopRepairing();

    public abstract void CompleteRepairing();

    #endregion
    */
    #endregion

    // public abstract void AbstractMethod();
}
