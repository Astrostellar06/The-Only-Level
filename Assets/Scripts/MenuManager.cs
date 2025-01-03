using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject firstMenu;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraGo;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject settings;
    [SerializeField] private OverlayManager overlay;
    private bool inMenu = true;

    public void Play()
    {
        menu.SetActive(false);
        player.SetActive(true);
        cameraGo.SetActive(true);
        gameUI.SetActive(true);
        firstMenu.SetActive(false);
        inMenu = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        overlay.NewLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quitting");
    }

    public void OpenSettings()
    {
        firstMenu.SetActive(false);
        settings.SetActive(true);
        inMenu = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !inMenu)
        {
            menu.SetActive(true);
            firstMenu.SetActive(true);
            player.SetActive(false);
            cameraGo.SetActive(false);
            gameUI.SetActive(false);
            settings.SetActive(false);
            inMenu = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (Manager.instance.level.value == 29)
                Manager.instance.currentExit.Open();
        }
    }
}
