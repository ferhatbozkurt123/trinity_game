using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Pause : MonoBehaviour
{
    public GameObject PauseMenuCanvas;
    public static bool isPaused; //The bool variable to hold information about game activation.


    // Start is called before the first frame update
    void Start()
    {
        PauseMenuCanvas.SetActive(false); //oyun baslar baslamaz kanvas gitsin
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //Ne zaman ki ESC tusuna basilir mouse ayarlarina gidilip ordan mouse tekrar aktif ediliyor.
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Oyun kapandi");
    }

    public void PlayButton()
    {
        ResumeGame();
    }


    public void PauseGame() //The function will be invoked whenever the ESC key is clicked.
    {
        PauseMenuCanvas.SetActive(true); //ekrani ac kapat
        isPaused = true;
        Time.timeScale = 0f;
        //SceneManager.LoadScene(2); //Gecisler arasi yasanan problemlerden oturu gecici olarak kaldirildi.


    }

    public void ResumeGame()
    {
        PauseMenuCanvas.SetActive(false); //canvayi yok et
        isPaused = false; 
        Time.timeScale = 1f;
        

    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }

}
