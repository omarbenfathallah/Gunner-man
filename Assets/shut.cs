using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shut : MonoBehaviour
{


    public Rigidbody rb;
    public float speed = 2000f;
    private Rigidbody b;
    public GameObject canon;
  


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
   public void shute()
    {


        if ( destroyme.currentHealth > 0)
        {
            b = Instantiate(rb, canon.transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(canon.transform.forward * speed);
        }
       

    }
    
}