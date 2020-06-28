using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Buttons
{
    // return delete button
    public static bool Delete()
    {
        return GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash"), GUILayout.Width(30));
    }

    //return add button
    public static bool Add()
    {
        return GUILayout.Button("+");
    }

    // return pack and send button
    public static bool PackAndSend()
    {
        return GUILayout.Button("Pack and Send");
    }
}
