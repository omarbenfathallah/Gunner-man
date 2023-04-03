using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class spoin : MonoBehaviour
{
    private float autospointimer = 0f;
    public Rigidbody rb;
    
    public float speed = 10f;
    private Rigidbody b;
    
    public Transform Player;


    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
        autospointimer += Time.deltaTime;
        
            while (autospointimer >= 5f && destroyme.currentHealth > 0)

            {
                spoinenemy();
          
                autospointimer = 0f;
            }
        
        
      //  b.GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }


    private void spoinenemy()
        {
            b = Instantiate(rb, transform.position, Quaternion.identity);
   

    }
    

}