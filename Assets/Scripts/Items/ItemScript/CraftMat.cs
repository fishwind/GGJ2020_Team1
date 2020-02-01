using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMat : Entity
{
    public CraftMatType currCraftMatType;

    private void Start()
    {
        currItemState = ItemStates.CraftMat;
        currItemActionState = ItemActionState.Pickup;
    }
}
