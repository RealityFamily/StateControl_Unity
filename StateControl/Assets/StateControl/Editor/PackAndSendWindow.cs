using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System;

#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class PackAndSendWindow : EditorWindow
{
    private WebConnection _webConnection = null;
    private bool statesOpend;
    private Vector2 _statesScroll;

    private bool serverStatus = false;
    private List<string> serverStates = new List<string>();
    private bool serverStatesOpend;

    private void Awake()
    {
        var states = GameObject.FindGameObjectWithTag("States");
        if (states != null)
        {
            _webConnection = states.GetComponent<WebConnection>();

            reloadServerInfo();
        }        
    }

    private async void reloadServerInfo()
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync("http://" + _webConnection.BaseURL + "/api/add_states/check/" + _webConnection.GameName);

        if (response.IsSuccessStatusCode)
        {
            var temp = JsonConvert.DeserializeObject<List<string>>(response.Content.ReadAsStringAsync().Result);
            if (temp.Count > 0)
            {
                serverStatus = true;
                serverStates = temp;
            }
            else
            {
                serverStatus = false;
            }
        }
    }

    private void OnGUI()
    {
        if (_webConnection != null)
        {
            DrawPackAndSend();
        }
        else
        {
            DrawStateListError();
        }
    }

    private void DrawStateListError()
    {
        GUILayout.Label("Add States from the top menu to change it.", EditorStyles.boldLabel);
    }

    private async void DrawPackAndSend()
    {
        _statesScroll = EditorGUILayout.BeginScrollView(_statesScroll, GUILayout.ExpandHeight(false));

        GUILayout.Label("Version on the server:", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * 30);
        GUILayout.Label("Already loaded on the server:  " + (serverStatus ? "Yes" : "No"), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        if (serverStatus) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 15);
            serverStatesOpend = EditorGUILayout.Foldout(serverStatesOpend, "States:", true);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            if (serverStatesOpend)
            {
                int index = 1;
                foreach (string State in serverStates)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUI.indentLevel * 30);
                    GUILayout.Label(index.ToString() + ". " + State);
                    EditorGUILayout.EndHorizontal();
                    index++;
                }
            }
        }

        GUILayout.Space(40);

        GUILayout.Label("Pack and Send to the server:", EditorStyles.boldLabel);
        EditorGUI.indentLevel = 1;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * 30);
        GUILayout.Label("Application name:  " + _webConnection.GameName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * 30);
        GUILayout.Label("Send to:  http://" + _webConnection.BaseURL + "/");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(EditorGUI.indentLevel * 15);
        statesOpend = EditorGUILayout.Foldout(statesOpend, "States:", true);
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel++;
        if (statesOpend)
        {
            int index = 1;
            for (int i = 0; i < _webConnection.GetLenght(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 30);
                GUILayout.Label(index.ToString() + ". " + _webConnection.GetKey(i));
                EditorGUILayout.EndHorizontal();
                index++;
            }
        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(20);

        if (Buttons.PackAndSend())
        {
            List<string> states = new List<string>();
            for (int i = 0; i < _webConnection.GetLenght(); i++)
            {
                states.Add(_webConnection.GetKey(i));
            }

            var json = JsonConvert.SerializeObject(new
            {
                gameName = _webConnection.GameName,
                statesList = states
            });

            Debug.Log(json);

            HttpClient client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://" + _webConnection.BaseURL + "/api/add_states/add");
            request.Content = new StringContent(json);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.SendAsync(request);

            reloadServerInfo();
        }
    }
}
#endif