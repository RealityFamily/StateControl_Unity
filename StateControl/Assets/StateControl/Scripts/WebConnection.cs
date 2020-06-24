using System;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using System.Threading;

[ExecuteInEditMode]
public class WebConnection : MonoBehaviour
{
    public delegate void StateDelegate();

    public string DeviceName;
    public string GameName;

    [SerializeField]
    private List<string> Keys = new List<string>();
    private List<StateDelegate> Values = new List<StateDelegate>();
    public void Add(string key/*, StateDelegate value*/)
    {
        Keys.Add(key);
        // КОСТЫЛИЩЕ!!!!! ПЕРЕДЕЛАТЬ!!!!!
        //Values.Add(value);
    }
    public void Remove(string key)
    {
        //Values.RemoveAt(Keys.IndexOf(key));
        Keys.Remove(key);
    }
    public StateDelegate GetValue(string key)
    {
        return Values[Keys.IndexOf(key)];
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
        if (ContainsKey(key)) {
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

    public string BaseURL;

    private void Awake()
    {
        // АХТУНГ!!!!! ЦЕ КОСТЫЛИЩЕ!!!!! ПЕРЕДЕЛАТЬ!!!!
        foreach (var key in Keys)
        {
            Values.Add(delegate { });
        }
        //КОНЕЦ КОСТЫЛЯ
    }

    // Start is called before the first frame update
    void Start()
    { 
        
        if (!string.IsNullOrWhiteSpace(BaseURL)) {
            using (WebSocket ws = new WebSocket(BaseURL))
            {
                ws.OnMessage += Ws_OnMessage;
                ws.Connect();

                ws.Send(JsonUtility.ToJson(new
                {
                    GameName
                }));
            }
        }

        new Thread(() => {
            Thread.Sleep(50);
                GetValue("Reload").Invoke();
        }).Start();
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
