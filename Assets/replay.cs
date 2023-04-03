using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class replay : MonoBehaviour
{
    public string scene;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
        userinterface.score = 0;

    }


}
