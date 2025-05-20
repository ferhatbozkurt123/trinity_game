using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button btnServer;
    [SerializeField] private Button btnHost;
    [SerializeField] private Button btnClient;
    [SerializeField] private Text txtPlayerCount;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        btnServer.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        btnHost.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            // global.Player1 = FindObjectOfType<NetworkManager>().client.connection.playerControllers[0].gameObject;
            global.Player1 = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
           // PlacePlayerOnMap(oyuncuPrefab);
        });
        btnClient.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //global.Player1 = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
            // PlacePlayerOnMap(oyuncuPrefab);
        });
    }

    private void Update()
    {
        txtPlayerCount.text= "Ply: " + playersCount.Value.ToString();
        global.CurrentPlayerCount = playersCount.Value;
        if (!IsServer) return;
        if (NetworkManager.Singleton != null)
        {
            if (NetworkManager.Singleton.ConnectedClients != null)
                playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
            //// Burada hata alýyoruz. Host ve client baðlantýsý olmadýðýnda Bunu kontrol et
        }
    }
}
