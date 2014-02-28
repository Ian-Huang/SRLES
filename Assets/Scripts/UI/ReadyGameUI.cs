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
    public Rect ClassInfoWindowRect = new Rect(300, 20, 250, 250);


    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public Dictionary<string, bool> ClassListTogglesMap = new Dictionary<string, bool>();   //課程清單對照表
    private int ChooseClassIndex = -1;    //目前被選擇的課程序號

    public Vector2 ClassInfoScrollViewPosition; //課程資訊滾輪位置
    public Dictionary<Object, bool> ClassInfoTogglesMap = new Dictionary<Object, bool>();   //課程資訊對照表
    public string ModifyClassNameString;        //課程名稱修改用字串
    public string CurrentEditClassName;         //當前選擇編輯的課程名稱

    private string[] classListArrary = new string[0];   //儲存課程清單矩陣

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

        this.classListArrary = new string[this.ClassListTogglesMap.Keys.Count];
        this.ClassListTogglesMap.Keys.CopyTo(classListArrary, 0);
    }

    /// <summary>
    /// 生成課程資訊單字映照表
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateClassInfoTogglesMap()
    {
        yield return XmlManager.script;   //確認XmlManager script是否存在  
        yield return XmlManager.script.XmlRoadFinish;    //確認XmlManager XML file是否已經載入完成

        //將XML File的課程單字載入到課程資訊單字映照表(ClassInfoTogglesMap)
        this.ClassInfoTogglesMap.Clear();
        foreach (XmlNode node in XmlManager.script.ClassListNode.SelectSingleNode(CurrentEditClassName).ChildNodes)
        {
            Object obj = ABTextureManager.script.TextureCollection.Find((Object temp) => { return temp.name == node.Name; });
            this.ClassInfoTogglesMap.Add(obj, false);
        }
    }

    void Update()
    {
        if (this.ChooseClassIndex != -1)
        {
            this.CurrentEditClassName = this.classListArrary[this.ChooseClassIndex];
            StartCoroutine(this.CreateClassInfoTogglesMap());
        }
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        //課程清單視窗
        this.ClassListWindowRect = GUILayout.Window((int)WindowID.ClassListWindow, this.ClassListWindowRect, this.ClassListWindow, "課程清單");

        //課程資訊視窗
        this.ClassInfoWindowRect = GUILayout.Window((int)WindowID.ClassInfoWindow, this.ClassInfoWindowRect, this.ClassInfoWindow, this.CurrentEditClassName);
    }

    /// <summary>
    /// 課程資訊視窗
    /// </summary>
    /// <param name="windowID"></param>
    void ClassInfoWindow(int windowID)
    {

        this.ClassInfoScrollViewPosition = GUILayout.BeginScrollView(this.ClassInfoScrollViewPosition);
        {
            if (ABTextureManager.script)    //確認ABTextureManager script是否存在        
                if (ABTextureManager.script.ABRoadFinish)    //確認ABTextureManager AB資源是否已經載入完成
                {
                    Dictionary<Object, bool> tempDir = new Dictionary<Object, bool>(this.ClassInfoTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
                    foreach (var key in tempDir.Keys)
                    {
                        //------單字庫一個單字的架構------                        
                        GUIContent content = new GUIContent(key.name, key as Texture);  //圖+文字內容
                        this.ClassInfoTogglesMap[key] = GUILayout.Toggle(this.ClassInfoTogglesMap[key], content, GUILayout.Height(70));   //因圖片太大，須設定大小                           
                        //------單字庫一個單字的架構------
                    }
                }
        }
        GUILayout.EndScrollView();


    }

    /// <summary>
    /// 課程清單視窗
    /// </summary>
    /// <param name="windowID"></param>
    void ClassListWindow(int windowID)
    {
        this.ClassListScrollViewPosition = GUILayout.BeginScrollView(this.ClassListScrollViewPosition);
        {

            this.ChooseClassIndex = GUILayout.SelectionGrid(this.ChooseClassIndex, this.classListArrary, 1);


        }
        GUILayout.EndScrollView();
    }


    public enum WindowID
    {
        ClassListWindow = 0, ClassInfoWindow = 2
    }
}
