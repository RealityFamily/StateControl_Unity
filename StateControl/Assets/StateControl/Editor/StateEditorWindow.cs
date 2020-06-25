using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

public class StateEditorWindow : EditorWindow
{
    private WebConnection _webConnection = null;
    private bool statesOpend = false;
    private Vector2 _statesScroll = Vector2.zero;

    private string _errorMessage = null;

    private void OnGUI()
    {
        var states = GameObject.FindGameObjectWithTag("States");
        

        if (states != null)
        {
            _webConnection = states.GetComponent<WebConnection>();
            DrawStateEditor();
        } else
        {
            DrawStateListError();
        }
    }

    private void DrawStateListError()
    {
        GUILayout.Label("Add States from the top menu to change it.", EditorStyles.boldLabel);
    }

    private void DrawStateEditor()
    {
        //if (EditorApplication.isPlaying) return;

        // Draw App Name
        GUILayout.Label("Application name:", EditorStyles.boldLabel);
        _webConnection.GameName = EditorGUILayout.TextField(_webConnection.GameName);

        EditorGUILayout.Space();

        GUILayout.Label("Server URL (without protocol):");
        _webConnection.BaseURL = EditorGUILayout.TextField(_webConnection.BaseURL);

        EditorGUILayout.Space();

        statesOpend = EditorGUILayout.Foldout(statesOpend, "States:", true);
        EditorGUI.indentLevel++;
        if (statesOpend)
        {
            _statesScroll = EditorGUILayout.BeginScrollView(_statesScroll);

            for (int i = 0; i < _webConnection.GetLenght(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUI.indentLevel * 30);

                GUILayout.Label((i + 1).ToString() + ".", GUILayout.Width(15));

                string key = _webConnection.GetKey(i);
                var _newKey = EditorGUILayout.TextField(key);
                if (_newKey != _webConnection.GetKey(i))
                {
                    if (!_webConnection.ContainsKey(_newKey)) {
                        _webConnection.ChangeKey(key, _newKey);
                        _errorMessage = "";
                    } else
                    {
                        _errorMessage = "State with this name is already exist.";
                    }
                }

                if (Buttons.Delete())
                {
                    _webConnection.Remove(key);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUI.indentLevel * 30);
            if (Buttons.Add())
            {
                if (!_webConnection.ContainsKey("")) {
                    _webConnection.Add(""/*, delegate { }*/);
                    _errorMessage = "";
                } else
                {
                    _errorMessage = "State with empty name is already exist.";
                }
            }
            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(_errorMessage))
            {
                EditorGUILayout.HelpBox(_errorMessage, MessageType.Error);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}

#endif