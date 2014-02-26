using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[ExecuteInEditMode]
public class ReadyGameUI : MonoBehaviour
{
    public GUISkin skin;

    //課程清單位置(之後要寫成彈性化相容於各種解析度)
    public Rect ClassListWindowRect = new Rect(25, 20, 250, 250);

    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public Dictionary<string, bool> ClassListTogglesMap = new Dictionary<string, bool>();   //課程清單對照表
    public int ChooseClassIndex;    //目前被選擇的課程序號



    void Start()
    {
        //先行載入課程清單資訊
        StartCoroutine(this.CreateClassListToggleMap());
    }

    /// <summary>
    /// 生成課程清單映照表
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateClassListToggleMap()
    {
        yield return XmlManager.script;   //確認XmlManager script是否存在  
        yield return XmlManager.script.XmlRoadFinish;    //確認XmlManager XML file是否已經載入完成

        //將XML File的課程清單載入到課程清單映照表(ClassListToggleMap)
        this.ClassListTogglesMap.Clear();
        foreach (XmlNode node in XmlManager.script.ClassListNode.ChildNodes)
            this.ClassListTogglesMap.Add(node.Name, false);
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        //課程清單視窗
        this.ClassListWindowRect = GUILayout.Window((int)WindowID.ClassListWindow, this.ClassListWindowRect, this.ClassListWindow, "課程清單");

    }

    /// <summary>
    /// 課程清單視窗
    /// </summary>
    /// <param name="windowID"></param>
    void ClassListWindow(int windowID)
    {
        this.ClassListScrollViewPosition = GUILayout.BeginScrollView(this.ClassListScrollViewPosition);
        {
            Dictionary<string, bool> tempDir = new Dictionary<string, bool>(this.ClassListTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
            string[] tempStr = new string[tempDir.Keys.Count];
            tempDir.Keys.CopyTo(tempStr, 0);

            this.ChooseClassIndex = GUILayout.SelectionGrid(this.ChooseClassIndex, tempStr, 1);
        }
        GUILayout.EndScrollView();
    }


    public enum WindowID
    {
        ClassListWindow = 0
    }
}
