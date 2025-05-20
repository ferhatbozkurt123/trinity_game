using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static global;

public class GameOverScreen : MonoBehaviour
{

    /// <summary>
    /// Oyun esnasýnda GameOver ekranýnýn fonksiyonlarýný içerir
    /// </summary>

    public GameObject gameOverBackScreen;

    void Start()
    {
        
    }

    
    void Update()
    {
        // Oyun modu GameOver mod olmuþsa GameOver iþlemlerini baþlat
        // Debug.Log("Oyun sonu ekraný baþlatýlýyor");
        if (global.PlayMod.Value == global.Mods.GameOver)
        {
            GameOver();
        }

    }

    public void GameOver()
    {
        // Game over penceresi açýldý Oyun modunu deðiþtir.
        global.PlayMod.Value = global.Mods.GameOverOpenned;
        gameOverBackScreen.SetActive(true);  // GameOver penceresini görünür yap
    }
    public void MainMenuButton()
    {
        // Ana menüye dön
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartButton()
    {
        gameOverBackScreen.SetActive(false);
        // Oyunu yeniden baþlat
        global.PlayMod.Value = global.Mods.StartNewGame;
        //global.BeginGame();
    }
}
