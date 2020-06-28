using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebConnection : MonoBehaviour
{
    public delegate void StateDelegate();
    private static readonly Queue<StateDelegate> _executionQueue = new Queue<StateDelegate>();

    public string DeviceSession;
    public string GameName;
    public string BaseURL;
    private WebSocket ws;

    private void Awake()
    {
        // create empty delegates to list of States names
        Values.Clear();
        foreach (var key in Keys)
        {
            Values.Add(delegate { });
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        // Connect to server via WebSocket
        if (!string.IsNullOrWhiteSpace(BaseURL)) {
            ws = new WebSocket("ws://" + BaseURL + "/GameControl");
            ws.OnMessage += Ws_OnMessage;
            ws.Connect();

            // and send started app name
            ws.Send(JsonConvert.SerializeObject(new
            {
                Game = GameName
            }));
        }
    }

    private void OnDestroy()
    {
        // Close connection when game stops
        ws.Close();
    }

    // Get and check messages from WebSocket
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        var json = JObject.Parse(e.Data.ToString());
        // Check that message has at least one of needed types 
        if (json.ContainsKey("DeviceSession") && json.ContainsKey("GameName") && json.ContainsKey("State"))
        {
            // if it is message with state, start invoke this delegate
            if (DeviceSession == json["DeviceSession"].ToString() && GameName == json["GameName"].ToString())
            {
                InvokeDelegate(json["State"].ToString());
            }
        } else if (json.ContainsKey("DeviceSession"))
        {
            // if it is deviceSession info, save it
            DeviceSession = json["DeviceSession"].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // invoke delegates in their queue in main thread
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }




    // Custom Map/Dictionary structure for saving States name and their delegates
    [SerializeField]
    private List<string> Keys = new List<string>();
    private List<StateDelegate> Values = new List<StateDelegate>();
    public void Add(string key)
    {
        Keys.Add(key);
    }
    public void Remove(string key)
    {
        Keys.Remove(key);
    }
    public StateDelegate GetValue(string key)
    {
        return Keys.IndexOf(key) > -1 ? Values[Keys.IndexOf(key)] : null;
    }
    public string GetKey(int index)
    {
        return Keys[index];
    }
    public void ChangeKey(string oldKey, string newKey)
    {
        Keys[Keys.IndexOf(oldKey)] = newKey;
    }
    public int GetLenght()
    {
        return Keys.Count;
    }
    public bool ContainsKey(string key)
    {
        return Keys.Find(item => item == key) != null;
    }
    public void ConnectToDelegate(string key, StateDelegate value)
    {
        if (ContainsKey(key))
        {
            Values[Keys.IndexOf(key)] += value;
        }
    }
    public void DisconnectToDelegate(string key, StateDelegate value)
    {
        if (ContainsKey(key))
        {
            Values[Keys.IndexOf(key)] -= value;
        }
    }
    private void InvokeDelegate(string State)
    {
        if (ContainsKey(State))
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(Values[Keys.IndexOf(State)]);
            }
        }
    }
}
