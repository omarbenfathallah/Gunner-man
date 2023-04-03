using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spoinboss : MonoBehaviour
{
    private Rigidbody bB;
    public Rigidbody rbB;
    public float speed = 10f;
    public Transform player;
    private static bool targetScore = false;
    private int scoretargetint = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (userinterface.score ==  scoretargetint && destroyme.currentHealth > 0 )
        {
            targetScore = true;
            bB = Instantiate(rbB, transform.position, Quaternion.identity);
            targetScore = false;
            scoretargetint += 2;
        }

    }
    
}
