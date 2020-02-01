using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameMenuController : MonoBehaviour
{
    public struct Menuscreen
    {
        public const int
            RECIPE = 0,
            MAP = 1,
            OPTION = 2;
        public const int
            Count = 3;
    }

    public Transform menu;
    private float offscreenPosY = -2500f;

    public List<Transform> menuImages;
    private bool isMenuDisplayed = false;
    private int currMenuscreen = 0;
    private int prevMenuscreen = 0;

    public float inputCD = 0.5f;
    private float currInput;

    private void ShowMenu(bool isShow)
    {
        menu.DOLocalMoveY(isShow ? 0 : offscreenPosY, 0.3f);
        UpdateMenuscreen();
    }

    private void UpdateMenuscreen()
    {
        if (!isMenuDisplayed) return;

        menuImages[prevMenuscreen].gameObject.SetActive(false);
        menuImages[currMenuscreen].gameObject.SetActive(true);

        currInput = inputCD;
    }

    private void Update()
    {
        if (Input.GetButtonUp("Cancel"))
        {
            isMenuDisplayed = !isMenuDisplayed;
            GlobalEvents.SendMenuOpened(isMenuOpen: isMenuDisplayed);
            ShowMenu(isMenuDisplayed);
        }

        if (currInput > float.Epsilon)
            currInput -= Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") > float.Epsilon && currInput < float.Epsilon)
        {
            prevMenuscreen = currMenuscreen;
            currMenuscreen++;
            if (currMenuscreen > Menuscreen.Count - 1) currMenuscreen = 0;
            UpdateMenuscreen();
        }

        if (Input.GetAxisRaw("Horizontal") < -float.Epsilon && currInput < float.Epsilon)
        {
            prevMenuscreen = currMenuscreen;
            currMenuscreen--;
            if (currMenuscreen < 0) currMenuscreen = Menuscreen.Count - 1;
            UpdateMenuscreen();
        }

    }

}
