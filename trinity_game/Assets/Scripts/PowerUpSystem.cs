using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PowerUpSystem : NetworkBehaviour
{
    public playerMovement parent;
    [SerializeField] private List<GameObject> spawnedPowerUpList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
