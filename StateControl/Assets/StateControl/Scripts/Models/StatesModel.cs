using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class States
{
    private string deviceName;
    private string gameName;
    private List<string> statesList;

    public string DeviceName { get { return deviceName; } set { deviceName = value; } }
    public string GameName { get { return gameName; } set { gameName = value; } }
    public List<string> StatesList { get { return statesList; } set { statesList = value; } }
}
