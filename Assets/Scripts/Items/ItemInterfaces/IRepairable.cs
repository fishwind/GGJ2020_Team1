using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepairable
{
    // Used by the Player to Start Repairing Broken Items
    void StartRepairing();

    // Used by the Player when they stop halfway while repairing Broken Items
    void StopRepairing();

    // Used by the Player when they stop halfway while repairing Broken Items
    void CompleteRepairing();

    float GetRepairTime();
}
