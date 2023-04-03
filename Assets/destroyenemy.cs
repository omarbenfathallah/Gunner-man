using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyenemy : MonoBehaviour
{
    public int lifepointenemy = 100;
    public Transform Player;
    public Rigidbody Tdammage;
    private Rigidbody D;

    private bool dam = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (dam == true)
        {
            D = Instantiate(Tdammage, transform.position, Quaternion.identity);
           
        }
        
        transform.LookAt(Player);
        transform.Translate(Vector3.forward * Time.deltaTime);
        if (lifepointenemy <= 0)
        {
            userinterface.score += 1;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            lifepointenemy -= 25;
            
            Destroy(collision.gameObject);
            dam = true;
        }
    }

}
