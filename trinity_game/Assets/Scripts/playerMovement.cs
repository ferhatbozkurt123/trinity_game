using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class playerMovement : NetworkBehaviour // MonoBehaviour
{
    /// <summary>
    /// Trinity 1.0
    /// Oyuncu Hareket fonsiyonlarýný içerir
    /// </summary>
    //public playerMovement parent;
    public GameObject prefab;           // Bomba prefabý
    public GameObject PlayerCamera;     // Oyuncu kamerasý
    public CharacterController characterController;
    public float speed = 5.0f;          // Oyuncunun hýzý
    public float jumpForce = 5.0f;      // Oyuncunun zýplama gücü
    private float horizontalInput;      // yatay girdi
    private float forwardInput;         // Dikey girdi
    private Rigidbody playerRb;         // Oyuncu fizik nesnesi
    private int health=5;               // Oyuncunun Saðlýk durumu
    private int score = 0;              // Oyuncunun Skoru
    public int bombCount=1;             // Oyuncunun Bomba býrakma sayýsý
    public int bombLength = 4;          // Oyuncunun Bomba etki uzunluðu
    private int bombTimeToExplode = 3;  // Bomba patlama süresi
    private bool CameraMod = true;     // 1 Tepeden 2 Oyuncu gözünden

    // CharacterController için
    //public float speed = 12f;
    //public float gravity = -9.81f * 2;
    //public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;


    //Vector3 velocity;

    bool isGrounded;

    [SerializeField] private List<GameObject> spawnedBombList = new List<GameObject>();

    // Rigidbody nesnesi bulunur
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        if (global.MultiPlayermi > 1)
        {
            if (!IsOwner) return;  // Multiplayerda kendi karakteri deðilse çýk 
                                   // Clienttaki host karakterine karýþma
                                   // Hosttaki client karakterine karýþma
        }
        global.parent = this;   /// Burada Multiplayer için clientda host da bu deðeri deðiþtirir
                                // Get set fonksiyonu yap    ÖNEMLÝ
        KameraGuncelle();
    }

    public override void OnNetworkSpawn()
    {

        //base.OnNetworkSpawn();
        if (IsOwner)
        {
            //LocalInstance = this;
            //global.Player1 = this;
        }
        Vector3 position;   // = new Vector3((0 + 0.5f) * global.genislikKatsayisi, 0.5f, (0 + 0.5f) * global.genislikKatsayisi);
        if (!IsServer)   // Client ise
        {
            position = new Vector3((0 + 0.5f) * global.genislikKatsayisi, 1.5f, (0 + 0.5f) * global.genislikKatsayisi); 
        }
        else 
        {
            position = new Vector3((global.en - 1.5f) * global.genislikKatsayisi, 1.5f, (global.boy - 1.5f) * global.genislikKatsayisi);
        }
        Debug.Log("Spawned Player: " + this.OwnerClientId.ToString());
    }

    // Her frame de bu iþlemleri yap
    void Update()
    {
        
        // Multiplayersa
        if (global.MultiPlayermi > 1)
        {
            //if (!IsServer) return;
            if (!IsOwner) return;  // Multiplayerda kendi karakteri deðilse çýk 
                                    // Clienttaki host karakterine karýþma
                                    // Hosttaki client karakterine karýþma
        }
        
        // Bekleme modundaysa iþlem yapma
        if (global.PlayMod.Value == global.Mods.Waiting ||
            global.PlayMod.Value == global.Mods.TimeOver ||
            global.PlayMod.Value == global.Mods.GameOver || 
            global.PlayMod.Value == global.Mods.Ending ||
            global.PlayMod.Value == global.Mods.StartNewGame ||
            global.PlayMod.Value == global.Mods.WaitingForPlayer) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        /*
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        */

        // Oyuncu girdilerini oku
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        
        if (CameraMod == true)  // Top kamera aktifse
        {
            //characterController.gameObject.SetActive(false);
            // Oyuncuyu ileri hareket ettir
            //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
            //transform.Translate(Vector3.right* Time.deltaTime * speed * horizontalInput);
            transform.position += Vector3.right * Time.deltaTime * speed * horizontalInput;
            transform.position += Vector3.forward * Time.deltaTime * speed * forwardInput;
        }
        else   // Oyuncu modunda yapýlacak girdi iþelmleri
        {
            transform.localPosition += Vector3.right * Time.deltaTime * speed * horizontalInput;
            transform.localPosition += Vector3.forward * Time.deltaTime * speed * forwardInput;
            //characterController.gameObject.SetActive(true);

            //Vector3 move = transform.right * horizontalInput + transform.forward * forwardInput;

            //characterController.Move(move * speed * Time.deltaTime);
            //right is the red Axis, foward is the blue axis

        }

        /* Characktercontroller için
        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
        */
        ////----- Animasyon koddlarý



        ////-----
        if (Input.GetKeyDown(KeyCode.V) && IsServer)
        {
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        }

            // Oyuncuyu zýplat 
            if (Input.GetKeyDown(KeyCode.Space))
        {
            global.audioManager.PlaySFX(global.audioManager.auPlayerJump);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        }
        // Oyuncunun konumuna Bomba býrak
        if (Input.GetKeyDown(KeyCode.Tab))
        {            
            
            // map[transform.localPosition.x, 0, transform.localPosition.z] = global.Nesne.Bomba;
            
            // Birakabilecek bomba varsa Bomba býrak
            if (bombCount > 0)
            {
                // Multiplayersa
                if (global.PlayerCount > 1)
                {
                    bombServerRpc();
                }
                else
                {
                    Vector3 position = new Vector3((((int)transform.localPosition.x) + 0.5f) * global.genislikKatsayisi, 0.5f, (((int)transform.localPosition.z) + 0.5f) * global.genislikKatsayisi);
                    // Debug.Log("Bomba Olusturuluyor :");
                    GameObject bomba = Instantiate(prefab, position, Quaternion.identity);
                    bomba.GetComponent<BombSystem>().timeToExplode = bombTimeToExplode;
                    bomba.GetComponent<BombSystem>().bombLength = bombLength;
                    bomba.GetComponent<BombSystem>().explodeTime = 3;

                    //parent = this;
                    // Bombayý gönder Network Bileþeni ile

                    bombCount--;            // Bomba sayacýný azalt
                }
                //Destroy(bomba);
                StartCoroutine(startBombCountRestore());    // Bomba patlayýnca sayacý artýr
            }

        }

        // Kamera deðiþtirme iþlemi
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraMod = !CameraMod;
            KameraGuncelle();
        }

    }

    public void KameraGuncelle()
    {
        if (CameraMod == true)  // Top kamera aktif ise
        {
            PlayerCamera.SetActive(false);
            global.MainCamera.SetActive(true);
        }
        if (CameraMod == false)  // Oyuncu kamerasý aktif ise
        {
            global.MainCamera.SetActive(false);
            PlayerCamera.SetActive(true);
        }
    }

    [ServerRpc]
    private void bombServerRpc()
    {
        Vector3 position = new Vector3((((int)transform.localPosition.x) + 0.5f) * global.genislikKatsayisi, 0.5f, (((int)transform.localPosition.z) + 0.5f) * global.genislikKatsayisi);
        Debug.Log("player: Bomba Olusturuluyor :" + global.playerName + "   PlayMod: " + global.PlayMod + "  isServer:" + IsServer + "  isOvner:" + IsOwner + "  isClient:" + IsClient);
        GameObject bomba = Instantiate(prefab, position, Quaternion.identity);
        bomba.GetComponent<BombSystem>().timeToExplode = bombTimeToExplode;
        bomba.GetComponent<BombSystem>().bombLength = bombLength;
        bomba.GetComponent<BombSystem>().explodeTime = 3;

        //parent = this;   // Kodu baþka bir tarafa al. Parent ayrý yerde olsun.
        // Bombayý gönder Network Bileþeni ile
        if (global.PlayerCount > 1)
        {
            spawnedBombList.Add(bomba);
            // Parent e aktar.
            bomba.GetComponent<BombSystem>().parent = this;
            bomba.GetComponent<NetworkObject>().Spawn();
        }
        bombCount--;            // Bomba sayacýný azalt
    }

    [ServerRpc(RequireOwnership =false)]
    public void DestroyServerRpc()
    {
        GameObject toDestroy = spawnedBombList[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedBombList.Remove(toDestroy);
        Destroy(toDestroy);
    }



    // Bomba patladýðýnda bomba sayacýný artýrýr.
    IEnumerator startBombCountRestore()
    {
        {
            yield return new WaitForSeconds(bombTimeToExplode);
            bombCount++;
            StopCoroutine(startBombCountRestore());     // Coroutine i durdur
        }


    }

    // Oyuncu öldüðünde gerçekleþtirilecek iþlemler
    public void die()
    {
        global.audioManager.PlaySFX(global.audioManager.auPlayerExplode);
        //Debug.Log("Oyuncu ölüyor");
        global.PlayMod.Value = global.Mods.Ending;            // Oyun bitiyor durumuna geç
        gameObject.SetActive(false);                // Oyuncuyu kaybet
    }

    // Oyuncu bir nesneye temas ettiyse
    private void OnCollisionEnter(Collision collision)
    {
        // Bomba Sayýsýný artýran PowerUp a çarpmýþsa: Tag
        if (collision.gameObject.CompareTag("puBombCount"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPowerUpEat);
            bombCount++;
            // bomba arttýðýnda bomba icon u büyüsün küçülsün
            //global.bombIcon.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 1).setEaseInOutQuart();
            Destroy(collision.gameObject);
        }

        // Bomba etki alanýný artýran Bomba alan artýrýcýya çarpmýþsa: Tag
        if (collision.gameObject.CompareTag("puBombLength"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPowerUpEat);
            bombLength++;
            Destroy(collision.gameObject);
        }

        // Bomba Hýzýný artýran Speed a çarpmýþsa: Tag
        if (collision.gameObject.CompareTag("puSpeed"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPowerUpEat);
            speed +=2;
            Destroy(collision.gameObject);
        }

        // Bomba Hayalet fonksiyonu Powerupa çarpmýþsa: Tag
        if (collision.gameObject.CompareTag("puGhost"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPowerUpEat);
            Destroy(collision.gameObject);
        }

        // Baþka bir Player a çarpmýþsa: Tag
        if (collision.gameObject.CompareTag("Player"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPlayerHi);
            Debug.Log("Kimsin kardeþ...");
        }
        
    }


    ///------- Animasyon fonksiyonlarý
 



    /// 
    /// ------------

}
