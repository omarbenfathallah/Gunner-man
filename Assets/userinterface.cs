using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class userinterface : MonoBehaviour
{
    public static int score = 0;
    public Text Tscore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Tscore.text = ("Score: " + score);
    }
}
