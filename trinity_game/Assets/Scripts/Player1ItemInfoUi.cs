using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player1ItemInfoUi : MonoBehaviour
{
    /// <summary>
    /// Trinty 1.0
    /// Oyuncu bilgilerini Kullanýcý panelinde gösteren fonksiyonlar
    /// </summary>

    public Text txtBombCount;           // Ekrandaki Bomba sayýsý 
    public Text txtBombLength;          // Ekrandaki Bomba uzunluðu 
    public Text txtSpeed;               // Ekrandaki Oyuncu hýzý 
    public Text txtGhost;               // Ekrandaki Hayalet fonksiyonu 
    public Text txtPlayerName;               // Ekrandaki Hayalet fonksiyonu 

    public GameObject playerStateUi;    // Oyuncu durum paneli

    // Oyun Baþladýðýnda
    void Start()
    {
        
    }

    // Her frame de
    void Update()
    {
        // Bekleme modundaysa iþlem yapma
        if (global.PlayMod.Value == global.Mods.Waiting ||
            global.PlayMod.Value == global.Mods.TimeOver ||
            global.PlayMod.Value == global.Mods.GameOver ||
            global.PlayMod.Value == global.Mods.Ending ||
            global.PlayMod.Value == global.Mods.StartNewGame ||
            global.PlayMod.Value == global.Mods.WaitingForPlayer) return;
        // Player1 Yoksa çýk.
        if (global.Player1 == null) return;
        // Bomba sayýsý uzunluðu vb ekranda güncelle
        txtBombCount.text = "x" + global.Player1.GetComponent<playerMovement>().bombCount.ToString();
        txtBombLength.text = "x" + global.Player1.GetComponent<playerMovement>().bombLength.ToString();
        txtSpeed.text = "x" + global.Player1.GetComponent<playerMovement>().speed.ToString();
        txtPlayerName.text = global.playerName;
    }
}
