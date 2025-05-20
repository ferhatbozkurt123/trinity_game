using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseDialog : MonoBehaviour
{
    public Transform pauseMenu;
    public CanvasGroup background;
    public static bool isPaused;

    void Start()
    {
        pauseMenu.gameObject.SetActive(false);
        background.alpha = 0;
        pauseMenu.localPosition = new Vector2(0, -Screen.height);
        Time.timeScale = 1f; // Ensure the game starts unpaused
        isPaused = false; // Ensure the game starts in an unpaused state
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (global.isLocalGamePaused.Value && isPaused ==false) || (!global.isLocalGamePaused.Value && isPaused == true))
        {
            Debug.Log("[Pause aialog] ESC tusuna basildi");
            if (isPaused)
            {
                CloseDialog();
            }
            else
            {
                ShowPauseMenu();
            }
        }
    }

    private void ShowPauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.gameObject.SetActive(true);
        isPaused = true;
        global.isLocalGamePaused.Value = true;
        background.alpha = 0;
        background.LeanAlpha(1, 0.5f);
        background.gameObject.SetActive(true);

        pauseMenu.localPosition = new Vector2(0, -Screen.height);
        pauseMenu.LeanMoveLocalY(0, 0.1f).setEaseOutExpo().setOnComplete(OnComplete).delay = 0.1f;
    }

    public void CloseDialog()
    {
        background.gameObject.SetActive(false);
        isPaused = false;
        global.isLocalGamePaused.Value = false;
        background.LeanAlpha(0, 0.5f);
        pauseMenu.LeanMoveLocalY(-Screen.height, 0.5f).setEaseInExpo();
        Time.timeScale = 1f;
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Oyun kapandi");
    }

    public void ContinueButton()
    {
        CloseDialog();
    }

    public void RestartButton()
    {
        CloseDialog();   // Pause menüsünü kapat
        // Oyunu yeniden baþlat
        global.PlayMod.Value = global.Mods.StartNewGame;

    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnComplete()
    {
        Time.timeScale = 0f;
    }
}
