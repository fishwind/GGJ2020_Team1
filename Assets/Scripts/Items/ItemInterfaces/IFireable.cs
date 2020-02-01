using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable 
{
    // Return True if Item is in Unfired State
    bool CheckIfFireable();

    // Used to Change item state after item finish firing
    void CompleteFiring();
}
