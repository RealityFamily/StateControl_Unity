using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;

public class WebConnection : MonoBehaviour
{
    public string BaseURL;

    private States states = null;

    // Start is called before the first frame update
    void Start()
    {
        states = gameObject.GetComponent<States>();

        if (!string.IsNullOrWhiteSpace(BaseURL)) {
            using (WebSocket ws = new WebSocket(BaseURL))
            {
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();

                ws.Send(JsonUtility.ToJson(states));
            }
        }
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnOpenHandler(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected!");
    }
}
