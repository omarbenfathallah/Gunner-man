using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyBoss : MonoBehaviour
{
    public int lifepointboss = 500;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.Translate(Vector3.forward * Time.deltaTime);
        if (lifepointboss <= 0)
        {
            userinterface.score += 4;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            lifepointboss -= 25;
            Destroy(collision.gameObject);
        }
    }

}

