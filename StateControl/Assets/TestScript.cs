using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Text Status;

    private WebConnection _states;

    // Start is called before the first frame update
    void Start()
    {
        _states = GameObject.FindGameObjectWithTag("States").GetComponent<WebConnection>();

        _states.ConnectToDelegate("Blue", () => {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        });

        _states.ConnectToDelegate("Red", () => {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        });

        _states.ConnectToDelegate("Green", () => {
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        });
    }

    // Update is called once per frame
    void Update()
    {
        Status.text = "DeviceSession: " + _states.DeviceSession;
    }
}
