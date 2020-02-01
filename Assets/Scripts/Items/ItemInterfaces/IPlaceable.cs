using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    // Used by the Player to Determine the Height of the 3D Model to place it
    float GetPlaceHeight();
}
