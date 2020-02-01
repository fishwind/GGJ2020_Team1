using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clay : CraftMat
{
    private void Start()
    {
        currItemState = ItemStates.CraftMat;
        currItemActionState = ItemActionState.Pickup;
        currCraftMatType = CraftMatType.Clay;
    }
}
