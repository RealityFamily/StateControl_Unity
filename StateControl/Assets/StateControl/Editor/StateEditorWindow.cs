using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;

public class StateEditorWindow : EditorWindow
{
    private GameObject _states = null;

    private void OnGUI()
    {
        _states = GameObject.FindGameObjectWithTag("States");

        if (_states != null)
        {
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
        throw new NotImplementedException();
    }
}

#endif