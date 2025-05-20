using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiPlayerMenuUı : MonoBehaviour
{
    /// <summary>
    /// Trinity 1.0
    /// Lobi ana menü işlemleri
    /// </summary>
    /// 

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject joinMenu;
    // Butonlar
    [SerializeField] private Button btnHost;
    [SerializeField] private Button btnClient;
    [SerializeField] private Button btnMainMenu;
    [SerializeField] private Button btnJoin;
    [SerializeField] private MeshRenderer meshRenderer;
    public Text playerName;

    public List<Color> colors = new List<Color>();


    void Start()
    {

        btnHost.onClick.AddListener(OnHostClicked);
        btnClient.onClick.AddListener(OnClientClicked);
        //btnJoin.onClick.AddListener(OnJoinClicked);
        if (!PlayerPrefs.HasKey("playername"))// Kullanıcı ismi var mı
        {
            PlayerPrefs.SetString("playername", "Player"); // Yoksa kaydet
            PlayerPrefs.Save();
        }
        playerName.text = PlayerPrefs.GetString("playername");// kullanıcı adını oku
        Debug.Log(playerName.text);
    }
    void OnDisable()
    {
        //btnHost.onClick.RemoveListener(OnHostClicked);
        //btnClient.onClick.RemoveListener(OnClientClicked);
        //btnJoin.onClick.RemoveListener(OnJoinClicked);

    }
    private void OnMainMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClientClicked()
    {
        global.playerName = playerName.text;
        PlayerPrefs.SetString("playername", playerName.text); // kaydet
        PlayerPrefs.Save();
        global.PlayerCount = 3;   // Client Multiplayer
        global.MultiPlayermi = 3;
        global.IlkAcilis = 1;
        global.LoadingSceneName = "SampleScene";
        SceneManager.LoadScene("Loading");
        //SceneManager.LoadScene("SampleScene");
    }

    public void OnHostClicked()
    {
        global.playerName = playerName.text;
        PlayerPrefs.SetString("playername", playerName.text); // kaydet
        PlayerPrefs.Save();
        //meshRenderer.material.color = colors[0];
        global.PlayerCount = 2;
        global.MultiPlayermi = 2;
        global.IlkAcilis = 1;
        global.LoadingSceneName = "SampleScene";        
        SceneManager.LoadScene("Loading");
        //SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        //playerName.text = networkPlayerName.Value.ToString();
        //meshRenderer.material.color = colors[(int)OwnerClientId];
    }
}
