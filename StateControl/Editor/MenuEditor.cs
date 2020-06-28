using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class MenuEditor : MonoBehaviour
{
    private const string GROUP_NAME = "StateEditor";

    private const string MENU_ITEM_STATE_EDITOR = GROUP_NAME + "/Open State Editor";
    private const string MENU_ITEM_NEW_STATE_LIST = GROUP_NAME + "/Add States";
    private const string MENU_ITEM_BUILD_AND_SEND = GROUP_NAME + "/Pack and send";

    [MenuItem(MENU_ITEM_STATE_EDITOR, false, 0)]
    static void OpenStateUI()
    {
        EditorWindow.GetWindow<StateEditorWindow>("State Editor");
    }

    [MenuItem(MENU_ITEM_NEW_STATE_LIST, false, 20)]
    static void AddStateObj()
    {
        SerializedObject tagsManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagsManager.FindProperty("tags");

        bool found = false;
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals("States")) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
            n.stringValue = "States";
            tagsManager.ApplyModifiedProperties();
        }

        var temp = GameObject.FindGameObjectWithTag("States");

        if (temp == null)
        {
            GameObject stateObj = new GameObject();
            stateObj.name = "States";
            stateObj.tag = "States";
            stateObj.AddComponent<WebConnection>();
        } else
        {
            EditorUtility.DisplayDialog("Warning", "State List is already on scene.\nOpen State Editor.", "Ok");
        }
    }

    [MenuItem(MENU_ITEM_BUILD_AND_SEND, false, 40)]
    static void Build_And_Send()
    {
        EditorWindow.GetWindow<PackAndSendWindow>("Pack and Send");
    }
}

#endif