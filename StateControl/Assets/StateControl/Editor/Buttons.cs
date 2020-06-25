using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Buttons
{
    public static bool Delete()
    {
        return GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash"), GUILayout.Width(30));
    }

    public static bool Add()
    {
        return GUILayout.Button("+");
    }

    public static bool PackAndSend()
    {
        return GUILayout.Button("Pack and Send");
    }
}
