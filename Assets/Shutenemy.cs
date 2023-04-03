using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shutenemy : MonoBehaviour
{


    public Rigidbody rb;
    public float speed = 1000f;
    private Rigidbody b;
    public GameObject canon;
    private float timer = 0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        if (timer <= 0f)
        {

            b = Instantiate(rb, canon.transform.position, Quaternion.identity);
            b.GetComponent<Rigidbody>().AddForce(canon.transform.forward * speed);
            timer = 5f;
        }


    }


    

}
