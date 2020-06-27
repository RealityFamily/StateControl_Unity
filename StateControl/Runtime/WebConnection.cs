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
        Values.Clear();
        foreach (var key in Keys)
        {
            Values.Add(delegate { });
        }
    }

    // Start is called before the first frame update
    void Start()
    { 
        if (!string.IsNullOrWhiteSpace(BaseURL)) {
            ws = new WebSocket("ws://" + BaseURL + "/GameControl");
            ws.OnMessage += Ws_OnMessage;
            ws.Connect();

            ws.Send(JsonConvert.SerializeObject(new
            {
                Game = GameName
            }));
        }
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        var json = JObject.Parse(e.Data.ToString());
        if (json.ContainsKey("DeviceSession") && json.ContainsKey("GameName") && json.ContainsKey("State"))
        {
            if (DeviceSession == json["DeviceSession"].ToString() && GameName == json["GameName"].ToString())
            {
                InvokeDelegate(json["State"].ToString());
            }
        } else if (json.ContainsKey("DeviceSession"))
        {
            DeviceSession = json["DeviceSession"].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    private void OnOpenHandler(object sender, EventArgs e)
    {
        Debug.Log("WebSocket connected!");
    }





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
