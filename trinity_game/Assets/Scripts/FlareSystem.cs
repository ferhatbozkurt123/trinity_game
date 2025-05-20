using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class FlareSystem : NetworkBehaviour
{
    /// <summary>
    /// Alev sistemini kontrol eden fonksiyonlar
    /// Flare prefab iþlemlerini içerir
    /// Flare belli bir yönde ve oyuncunun bomba etki alaný uzunluðuna göre alev süreçlerini yönetir.
    /// 
    /// </summary>
    public playerMovement parent;

    public int flareLength;             // Alev uzunluðu. Oyuncu özelliklerine göre
    public float speed;                 // Hýzý
    public float timeToFlare;           // Alev oluþturma süresi
    public float explodeTime;           // Patlama süresi
    public Vector3 direction;           // Alevin yönü
    public GameObject bombFlareprefab;  // Alev prefabý
    public Time startTime;              // baþlama zamaný

    [SerializeField] private List<GameObject> spawnedFlareList = new List<GameObject>();


    // Patlama Coroutine çalýþtýrýlýr
    void Start()
    {
        StartCoroutine(startExplode());
    }


    IEnumerator startExplode()
    {
        Debug.Log("FlareSystem 1 - " + global.Player1.gameObject.name + " - " + global.MultiPlayermi.ToString() + " - " + IsOwner);

        // Multiplayersa
        if (global.MultiPlayermi < 3)
        {
            if (IsOwner || global.MultiPlayermi==1) // Að da ise
            {
                // patlama rutini
                {
                    // Alev oluþturma süresine kadar bekle
                    yield return new WaitForSeconds(timeToFlare);

                    if (flareLength > 0)        // Bombanýn Alevý sonuna gelmediyse
                    {
                        if (global.MultiPlayermi > 1)
                        {
                            flareCreateServerRpc();
                        }
                        else
                        {
                            // Vector3 position = new Vector3((transform.localPosition.x) * global.genislikKatsayisi, 0.5f, (transform.localPosition.z+1f) * global.genislikKatsayisi);
                            Vector3 position = transform.position + direction;
                            // Debug.Log("Bir sonraki Flare Olusturuluyor :");
                            GameObject flare = Instantiate(bombFlareprefab, position, Quaternion.identity);
                            flare.transform.Rotate(direction, Space.World);
                            flare.GetComponent<FlareSystem>().direction = direction;
                            flare.GetComponent<FlareSystem>().timeToFlare = timeToFlare;
                            flare.GetComponent<FlareSystem>().flareLength = flareLength - 1;  // Alev uzunluk mantýðý
                            flare.GetComponent<FlareSystem>().explodeTime = explodeTime;
                        }
                    }
                    Destroy(this.gameObject, explodeTime);  // Mevcut Alev Süresi bitince yok edilsin
                }
            }
            StopCoroutine(startExplode());          // Patlama Coroutini silinsin.
        }
    }

    // Alev nesnesi bir nesneye çarptýysa
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Flare çarptý");
        StopCoroutine(startExplode());                  // Patlama Coroutini durdurulur
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Alev Duvara çarpmýþsa 
            global.audioManager.PlaySFX(global.audioManager.auWallRockHit);
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
        }
        if (collision.gameObject.CompareTag("Rock"))
        {
            // Alev Kayaya çarpmýþsa 
            global.audioManager.PlaySFX(global.audioManager.auWallRockHit);
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // Alev Oyuncuya çarpmýþsa 
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
            // Çarptýðýn oyuncuyu öldür
            collision.gameObject.GetComponent<playerMovement>().die();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Alev Düþmana çarpmýþsa 
            global.audioManager.PlaySFX(global.audioManager.auEnemyExplode);
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(collision.gameObject);  // Çarptýðýn nesneyi yoket
            Destroy(this.gameObject);       // Bu alevi yoket

        }
        if (collision.gameObject.CompareTag("Box"))
        {
            // Alev Kutuya çarpmýþsa 
            global.audioManager.PlaySFX(global.audioManager.auBoxExplode);
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
                                            // Çarptýðýn bombayý patlat
            collision.gameObject.GetComponent<boxSystem>().Explode();
        }
        if (collision.gameObject.CompareTag("PowerUp"))
        {
            // Alev PowerUpa çarpmýþsa 
            global.audioManager.PlaySFX(global.audioManager.auPlayerJump);
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
        }
        if (collision.gameObject.CompareTag("Bomb"))
        {
            // Alev Bombaya çarpmýþsa 
            flareLength = 0;                // Daha fazla alev oluþturma
            Destroy(this.gameObject);       // Bu alevi yoket
            collision.gameObject.GetComponent<BombSystem>().BombExplode();

        }
        if (collision.gameObject.CompareTag("BombFlare"))
        {
            // Alev Aleve çarpmýþsa 
            //Destroy(this.gameObject);       // Bu alevi yoket
        }


    }

    [ServerRpc]
    private void flareCreateServerRpc()
    {
        // Vector3 position = new Vector3((transform.localPosition.x) * global.genislikKatsayisi, 0.5f, (transform.localPosition.z+1f) * global.genislikKatsayisi);
        Vector3 position = transform.position + direction;
        // Debug.Log("Bir sonraki Flare Olusturuluyor :");
        GameObject flare = Instantiate(bombFlareprefab, position, Quaternion.identity);
        flare.transform.Rotate(direction, Space.World);
        flare.GetComponent<FlareSystem>().direction = direction;
        flare.GetComponent<FlareSystem>().timeToFlare = timeToFlare;
        flare.GetComponent<FlareSystem>().flareLength = flareLength - 1;  // Alev uzunluk mantýðý
        flare.GetComponent<FlareSystem>().explodeTime = explodeTime;

        if (global.MultiPlayermi > 1)
        {
            spawnedFlareList.Add(flare);
            // Parent e aktar.
            flare.GetComponent<FlareSystem>().parent = global.parent;
            flare.GetComponent<NetworkObject>().Spawn();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedFlareList[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedFlareList.Remove(toDestroy);
        Destroy(toDestroy);
    }
    
}
