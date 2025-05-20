using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class boxSystem : NetworkBehaviour
{
    public GameObject puBombCount;
    public GameObject puBombLength;
    public GameObject puSpeed;
    public GameObject puGhost;

    public playerMovement parent;

    [SerializeField] private List<GameObject> spawnedPowerupList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explode()
    {
        if (global.MultiPlayermi > 1)
            if (!IsOwner)
            {
                return;
            }
            Destroy(this.gameObject);
        int powerupVar = (int)Random.Range(0.0f, 10.0f);
        if (powerupVar > 7)
        {
            // Multiplayersa
            if (global.MultiPlayermi > 1)
            {
                powerupServerRpc();
            }
            else
            {

                Vector3 position = new Vector3((((int)transform.localPosition.x) + 0.5f) * global.genislikKatsayisi, 0.5f, (((int)transform.localPosition.z) + 0.5f) * global.genislikKatsayisi);
                Debug.Log("PowerUp Olusturuluyor :");
                int powerup = (int)Random.Range(0.0f, 4.0f);
                GameObject puItem;
                switch (powerup)
                {
                    case 0:
                        puItem = Instantiate(puBombCount, position, Quaternion.identity);
                        break;
                    case 1:
                        puItem = Instantiate(puBombLength, position, Quaternion.identity);
                        break;
                    case 2:
                        puItem = Instantiate(puSpeed, position, Quaternion.identity);
                        break;
                    case 3:
                        puItem = Instantiate(puGhost, position, Quaternion.identity);
                        break;
                }
            }
        }
        

    }

    [ServerRpc]
    private void powerupServerRpc()
    {
        Vector3 position = new Vector3((((int)transform.localPosition.x) + 0.5f) * global.genislikKatsayisi, 0.5f, (((int)transform.localPosition.z) + 0.5f) * global.genislikKatsayisi);
        Debug.Log("PowerUp Olusturuluyor :");
        int powerup = (int)Random.Range(0.0f, 4.0f);
        GameObject puItem = null;
        switch (powerup)
        {
            case 0:
                puItem = Instantiate(puBombCount, position, Quaternion.identity);
                break;
            case 1:
                puItem = Instantiate(puBombLength, position, Quaternion.identity);
                break;
            case 2:
                puItem = Instantiate(puSpeed, position, Quaternion.identity);
                break;
            case 3:
                puItem = Instantiate(puGhost, position, Quaternion.identity);
                break;
        }
        if (global.PlayerCount > 1)
        {
            spawnedPowerupList.Add(puItem);
            // Parent e aktar.
            puItem.GetComponent<PowerUpSystem>().parent = global.parent;
            puItem.GetComponent<NetworkObject>().Spawn();
        }
    }
    /*
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedPowerupList[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedPowerupList.Remove(toDestroy);
        Destroy(toDestroy);
    }
    */
}
