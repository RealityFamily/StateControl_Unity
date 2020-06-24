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

        _states.ConnectToDelegate("Reload", () => { Debug.Log("Reload"); });

        _states.ConnectToDelegate("1", () => { Debug.Log("1"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
