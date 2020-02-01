using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsController : MonoBehaviour
{
    private void Update()
    {
        if (MainMenuController.Instance.IsOptionsDisplayed && Input.GetButton("Cancel"))
        {
            MainMenuController.Instance.Options();
        }
    }
}
