using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[ExecuteInEditMode]
public class ReadyGameUI : MonoBehaviour
{
    public GUISkin skin;

    public GameObject EnterGameModeButton;
    public GameObject EnterTrainModeButton;

    //課程清單位置(之後要寫成彈性化相容於各種解析度)
    public Rect ClassListWindowRect = new Rect(25, 20, 250, 250);
    public Rect ClassInfoWindowRect = new Rect(300, 20, 250, 250);


    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public List<string> ClassNameList = new List<string>();   //課程名稱清單
    private int ChooseClassIndex;    //目前被選擇的課程序號
    public string CurrentChooseClassName;         //當前被選擇的課程名稱
    private string[] classListArrary = new string[0];   //儲存所有課程名稱的矩陣

    public Vector2 ClassInfoScrollViewPosition; //課程資訊滾輪位置
    public List<Object> ClassInfoObjectList = new List<Object>();   //課程資訊物件清單


    void Start()
    {
        this.ChooseClassIndex = -1;
        this.EnterGameModeButton.SetActive(false);
        this.EnterTrainModeButton.SetActive(false);

        //先行載入課程清單資訊
        StartCoroutine(this.CreateClassNameList());
    }

    /// <summary>
    /// 生成課程名稱清單
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateClassNameList()
    {
        while (!XmlManager.script)     //確認XmlManager script是否存在  
            yield return null;

        while (!XmlManager.script.XmlRoadFinish)    //確認XmlManager XML file是否已經載入完成
            yield return null;

        //將XML File的課程清單載入到課程名稱清單(ClassNameList)
        this.ClassNameList.Clear();
        foreach (XmlNode node in XmlManager.script.ClassListNode.ChildNodes)
            this.ClassNameList.Add(node.Name);

        //將所有的課程清單名稱存入矩陣中
        this.classListArrary = new string[this.ClassNameList.Count];
        this.ClassNameList.CopyTo(classListArrary, 0);

    }

    /// <summary>
    /// 生成課程資訊的物件清單
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateClassInfoObjectList()
    {
        while (!XmlManager.script)     //確認XmlManager script是否存在  
            yield return null;

        while (!XmlManager.script.XmlRoadFinish)    //確認XmlManager XML file是否已經載入完成
            yield return null;

        //將XML File的課程單字載入到課程資訊物件清單(ClassInfoObjectList)
        this.ClassInfoObjectList.Clear();
        foreach (XmlNode node in XmlManager.script.ClassListNode.SelectSingleNode(CurrentChooseClassName).ChildNodes)
        {
            Object obj = ABTextureManager.script.TextureCollection.Find((Object temp) => { return temp.name == node.Name; });
            this.ClassInfoObjectList.Add(obj);
        }
        ABTextureManager.script.ChooseClassWordCollection = new List<Object>(this.ClassInfoObjectList);
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        //課程清單視窗
        this.ClassListWindowRect = GUILayout.Window((int)WindowID.ClassListWindow, this.ClassListWindowRect, this.ClassListWindow, "課程清單");

        //課程資訊視窗
        this.ClassInfoWindowRect = GUILayout.Window((int)WindowID.ClassInfoWindow, this.ClassInfoWindowRect, this.ClassInfoWindow, this.CurrentChooseClassName);
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
                    List<Object> tempDir = new List<Object>(this.ClassInfoObjectList);  //不可直接對List做讀取，須建立一個暫存
                    foreach (var key in tempDir)
                    {
                        //------一個單字的架構------                        
                        GUIContent content = new GUIContent(key.name, key as Texture);  //圖+文字內容
                        GUILayout.Label(content, GUILayout.Height(70));
                        //------一個單字的架構------
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
            int oldIndex = this.ChooseClassIndex;
            this.ChooseClassIndex = GUILayout.SelectionGrid(this.ChooseClassIndex, this.classListArrary, 1);    //產生可選擇的清單

            //判斷被選擇的課程是否不一樣，會將課程資訊做更換
            if (oldIndex != this.ChooseClassIndex)
            {
                if (oldIndex == -1)
                {
                    this.EnterGameModeButton.SetActive(true);
                    this.EnterTrainModeButton.SetActive(true);
                }

                this.CurrentChooseClassName = this.classListArrary[this.ChooseClassIndex];  //儲存目前被選擇的課程名
                StartCoroutine(this.CreateClassInfoObjectList());
            }
        }
        GUILayout.EndScrollView();
    }


    public enum WindowID
    {
        ClassListWindow = 0, ClassInfoWindow = 2
    }
}
