using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WebConnection _states = GameObject.FindGameObjectWithTag("States").GetComponent<WebConnection>();

        _states.ConnectToDelegate("Start", () => { Debug.Log("Start"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
