using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyme : MonoBehaviour
{
    public int maxHealth = 100;
    public static int  currentHealth ;
    public  HealthBar healthBar;
    public Text Thelth;
    public GameObject DeathPanel;
    public GameObject Coinsx3;
    public GameObject Revive;
    public GameObject BackToMenu;

    public GameObject GameOver;
    public Text TGameOver;

   
  


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        DeathPanel.SetActive(false);
    }

// Update is called once per frame
void Update()
    {
        Thelth.text = ("health: " + currentHealth);
        if (currentHealth <= 0)
        {
            DeathPanel.SetActive(true);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
            if (collision.gameObject.tag == "enemy")
            {

                currentHealth -= 10;
                healthBar.SetHealth(currentHealth);
                
            }
       
    }
    
}

