using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[ExecuteInEditMode]
public class ReadyGameUI : MonoBehaviour
{
    public GUISkin skin;


    void Start()
    {

    }

    void OnGUI()
    {
        GUI.skin = this.skin;
    }


    public enum WindowID
    {

    }
}
