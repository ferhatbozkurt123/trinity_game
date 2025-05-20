using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : NetworkBehaviour
{
    /// <summary>
    /// Trinity 1.0 
    /// Canavar sistemi AI altyap�s� v.1
    /// </summary>
    
    public playerMovement parent;


    //public Transform player;
    NavMeshAgent agent;

    // Oyuncu pozisyonlar�
    Vector3 OyuncuPosizyonu;
    Vector3 GezintiPosizyonu;

    // Zemin ve Oyuncular� tespit et
    public LayerMask Zemin, Oyuncumu;

    public float health;

    // Gezinti De�i�kenleri
    public Vector3 yuruyusNoktasi;
    bool yuruyusNoktasiBelirlendi;
    public float yuruyusNoktasiMesafesi;

    // Sald�rma de�i�kenleri
    public float saldirilarArasiSure;
    bool saldirildi;
    public GameObject projectile;

    // Sonlu Durumlu Mekanizmalar
    public float gorusMesafesi, atakMesafesi;
    public bool OyuncuGorusMesafesinde, OyuncuSaldiriMesafesinde;


    private Animator animator;



    // A��l��ta NavMeshAgen vb bul

    void Start()
    {
        if (global.MultiPlayermi > 1) if (!IsOwner) { return; }
        //Debug.Log("<" + global.Multiplayer.ToString() + "> Enemy start");
        agent = GetComponent<NavMeshAgent>();
        //Debug.Log("<" + global.Multiplayer.ToString() + "> Enemy start yapt�");
        OyuncuPosizyonu = transform.position;  // Bulundu�un konumu haf�zaya al
        GezintiPosizyonu = GezintiPozisyonuBelirle(); // Rastgele bir hedef belirle
        //agent.destination = GezintiPosizyonu;
        animator = GetComponent<Animator>();  // Animator component initialization

        // Existing initialization...
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // NavMeshAgent settings
        agent.speed = 2.0f;  // Adjust this value to change the speed
        agent.acceleration = 4.0f;  // Adjust this value to change the acceleration
        agent.angularSpeed = 120.0f;  // Adjust this value to change the turning speed

       
    }

    // Update is called once per frame
    void Update()
    {
        if (global.MultiPlayermi > 1)
            if (!IsOwner) { return; }
        //Debug.Log("<" + global.Multiplayer.ToString() + "> Enemy update");
        if (global.MultiPlayermi > 1) if (!IsServer) { return; } // Server de�ilse client ise d�n
        //Debug.Log("<" + global.Multiplayer.ToString() + "> Enemy update yapt�");
        // Bekleme modundaysa i�lem yapma
        if (global.PlayMod.Value == global.Mods.Waiting ||
            global.PlayMod.Value == global.Mods.TimeOver ||
            global.PlayMod.Value == global.Mods.GameOver ||
            global.PlayMod.Value == global.Mods.Ending ||
            global.PlayMod.Value == global.Mods.StartNewGame ||
            global.PlayMod.Value == global.Mods.WaitingForPlayer) return;

        //agent.destination = player.position; // Silinecek
        //Check for sight and attack range
        OyuncuGorusMesafesinde = Physics.CheckSphere(transform.position, gorusMesafesi, Oyuncumu);
        OyuncuSaldiriMesafesinde = Physics.CheckSphere(transform.position, atakMesafesi, Oyuncumu);

        if (!OyuncuGorusMesafesinde && !OyuncuSaldiriMesafesinde) Geziyor();
        if (OyuncuGorusMesafesinde && !OyuncuSaldiriMesafesinde) OyuncuyaGit();
        if (OyuncuSaldiriMesafesinde && OyuncuGorusMesafesinde) OyuncuyaSaldir();


        // Update animations based on state
        animator.SetBool("isWalking", !OyuncuGorusMesafesinde && !OyuncuSaldiriMesafesinde);
        //animator.SetBool("isAttacking", OyuncuSaldiriMesafesinde && OyuncuGorusMesafesinde);

       
    }

    private Vector3 RastgeleYonBelirle()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(1f, 1f)).normalized;

    }
    private Vector3 GezintiPozisyonuBelirle()
    {
        return OyuncuPosizyonu + RastgeleYonBelirle() * Random.Range(10f, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ba�ka bir Player a �arpm��sa: Tag
        if (collision.gameObject.CompareTag("Player"))
        {
            global.audioManager.PlaySFX(global.audioManager.auPlayerHi);
            collision.gameObject.GetComponent<playerMovement>().die();
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            GezintiPosizyonu = GezintiPozisyonuBelirle();
        }
        if (collision.gameObject.CompareTag("Rock"))
        {
            GezintiPosizyonu = GezintiPozisyonuBelirle();
        }
    }

    public void Geziyor()
    {
        if (!yuruyusNoktasiBelirlendi) GezintiPozisyonuBul();

        if (yuruyusNoktasiBelirlendi)
            agent.SetDestination(yuruyusNoktasi);

        Vector3 distanceToWalkPoint = transform.position - yuruyusNoktasi;

        //yuruyusNoktasi reached
        if (distanceToWalkPoint.magnitude < 1f)
            yuruyusNoktasiBelirlendi = false;

        animator.SetBool("isWalking", true);
    }
    private void GezintiPozisyonuBul()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-yuruyusNoktasiMesafesi, yuruyusNoktasiMesafesi);
        float randomX = Random.Range(-yuruyusNoktasiMesafesi, yuruyusNoktasiMesafesi);

        yuruyusNoktasi = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(yuruyusNoktasi, -transform.up, 2f, Zemin))
            yuruyusNoktasiBelirlendi = true;
    }

    private void OyuncuyaGit()
    {
        agent.SetDestination(global.Player1.transform.position);
        animator.SetBool("isWalking", true);
    }

    private void OyuncuyaSaldir()
    {
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);
        agent.SetDestination(global.Player1.transform.position);
        transform.LookAt(global.Player1.transform);
        /*
        if (!saldirildi)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            saldirildi = true;
            Invoke(nameof(ResetAttack), saldirilarArasiSure);
        }*/
    }
    private void ResetAttack()
    {
        saldirildi = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //animator.SetTrigger("takeDamage");

        if (health <= 0)
        {
            //animator.SetTrigger("die");
            Invoke(nameof(DestroyEnemy), 0.5f);

        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atakMesafesi);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gorusMesafesi);
    }

}
