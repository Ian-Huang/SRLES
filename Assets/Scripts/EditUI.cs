using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

[ExecuteInEditMode]
public class EditUI : MonoBehaviour
{
    public GUISkin skin;

    //課程清單位置(之後要寫成彈性化相容於各種解析度)
    public Rect WordDataWindowRect = new Rect(575, 20, 250, 250);
    public Rect ClassInfoWindowRect = new Rect(300, 20, 250, 250);
    public Rect ClassListWindowRect = new Rect(25, 20, 250, 250);
    public Rect HintWindowRect = new Rect(20, 20, 120, 50);

    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public Dictionary<string, bool> ClassListTogglesMap = new Dictionary<string, bool>();   //課程清單對照表
    public int CreateClassCount = 1;    //課程清單產生次數(避免重複命名使用)

    public Vector2 WordDataScrollViewPosition;  //單字庫滾輪位置
    public Dictionary<Object, bool> WordDataTogglesMap = new Dictionary<Object, bool>();   //單字庫對照表
    public string WordSearchString = "";        //單字庫搜尋列字串

    public Vector2 ClassInfoScrollViewPosition; //課程資訊滾輪位置
    public Dictionary<Object, bool> ClassInfoTogglesMap = new Dictionary<Object, bool>();   //課程資訊對照表
    public string ModifyClassNameString = "";         //課程名稱修改用字串
    public string CurrentEditClassName;

    void Start()
    {
        StartCoroutine(this.CreateClassListToggleMap());

        StartCoroutine(this.CreateWordDataToggleMap());
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
        foreach (XmlNode node in XmlManager.script.ClassListNode.SelectSingleNode(CurrentEditClassName).ChildNodes)
        {
            Object obj = ABTextureManager.script.TextureCollection.Find((Object temp) => { return temp.name == node.Name; });
            this.ClassInfoTogglesMap.Add(obj, false);
        }
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
        foreach (XmlNode node in XmlManager.script.ClassListNode.ChildNodes)
            this.ClassListTogglesMap.Add(node.Name, false);
    }

    /// <summary>
    /// 生成單字庫映照表
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateWordDataToggleMap()
    {
        yield return ABTextureManager.script;   //確認ABTextureManager script是否存在  
        yield return ABTextureManager.script.ABRoadFinish;    //確認ABTextureManager AB資源是否已經載入完成

        //將AB所有的圖檔載入到單字庫映照表(WordDataToggleMap)
        foreach (var temp in ABTextureManager.script.TextureCollection)
            this.WordDataTogglesMap.Add(temp, false);
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        //課程清單視窗
        this.ClassListWindowRect = GUILayout.Window((int)WindowID.ClassListWindow, this.ClassListWindowRect, this.ClassListWindow, "課程清單");

        //單字庫視窗
        this.WordDataWindowRect = GUILayout.Window((int)WindowID.WordDataWindow, this.WordDataWindowRect, this.WordDataWindow, "單字庫");

        //課程資訊視窗
        this.ClassInfoWindowRect = GUILayout.Window((int)WindowID.ClassInfoWindow, this.ClassInfoWindowRect, this.ClassInfoWindow, this.CurrentEditClassName);

        //提醒視窗
        //this.HintWindowRect = GUILayout.Window(1, this.HintWindowRect, this.HintWindow, "提醒");
    }

    /// <summary>
    /// 課程資訊視窗
    /// </summary>
    /// <param name="windowID"></param>
    void ClassInfoWindow(int windowID)
    {
        //-------將目前編輯的課程重新命名-------
        GUILayout.BeginHorizontal();
        {
            this.ModifyClassNameString = GUILayout.TextField(this.ModifyClassNameString, 10);     //課程名稱修改用字串          
            if (GUILayout.Button("修改課程名", GUILayout.ExpandWidth(false)))
            {
                XmlManager.script.ModifyClassName(this.CurrentEditClassName, this.ModifyClassNameString);
                this.CurrentEditClassName = this.ModifyClassNameString;
            }
        }
        GUILayout.EndHorizontal();
        //-------將目前編輯的課程重新命名-------

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
                        this.ClassInfoTogglesMap[key] = GUILayout.Toggle(this.ClassInfoTogglesMap[key], content, GUILayout.Height(50));   //因圖片太大，須設定大小                           
                        //------單字庫一個單字的架構------
                    }
                }
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("儲存"))
            {
                Dictionary<Object, bool> tempDir = new Dictionary<Object, bool>(this.ClassInfoTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
                //將目前所選擇單字儲存至XML File
                foreach (var key in tempDir.Keys)
                {
                    //新增單字資訊至XML File
                    XmlManager.script.CreateNewWordToClass(this.CurrentEditClassName, key.name);
                }

            }
            if (GUILayout.Button("刪除"))
            {
                Dictionary<Object, bool> tempDir = new Dictionary<Object, bool>(this.ClassInfoTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
                //確認對照表，將被勾選的物件刪除
                foreach (var key in tempDir.Keys)
                {
                    if (this.ClassInfoTogglesMap[key])
                    {
                        //將ClassInfoTogglesMap中對應的課程刪除
                        this.ClassInfoTogglesMap.Remove(key);
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 單字庫視窗
    /// </summary>
    /// <param name="windowID"></param>
    void WordDataWindow(int windowID)
    {
        //------單字搜尋------
        GUILayout.BeginHorizontal();
        {
            this.WordSearchString = GUILayout.TextField(this.WordSearchString, 15);     //單字搜尋輸入列            
            GUILayout.Box("搜尋", GUILayout.ExpandWidth(false));
        }
        GUILayout.EndHorizontal();
        //------單字搜尋------

        this.WordDataScrollViewPosition = GUILayout.BeginScrollView(this.WordDataScrollViewPosition);
        {
            if (ABTextureManager.script)    //確認ABTextureManager script是否存在        
                if (ABTextureManager.script.ABRoadFinish)    //確認ABTextureManager AB資源是否已經載入完成
                {
                    Dictionary<Object, bool> tempDir = new Dictionary<Object, bool>(this.WordDataTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
                    foreach (var key in tempDir.Keys)
                    {
                        if (key.name.StartsWith(this.WordSearchString, System.StringComparison.OrdinalIgnoreCase))  //將搜尋列輸入的字串與單字庫各單字開頭進行比較，以方便進行搜尋
                        {
                            //------單字庫一個單字的架構------                        
                            GUIContent content = new GUIContent(key.name, key as Texture);  //圖+文字內容
                            this.WordDataTogglesMap[key] = GUILayout.Toggle(this.WordDataTogglesMap[key], content, GUILayout.Height(50));   //因圖片太大，須設定大小                           
                            //------單字庫一個單字的架構------
                        }
                    }
                }
        }
        GUILayout.EndScrollView();

        //將在單字庫所選的單字新增至目前編輯的課程
        if (GUILayout.Button("新增至課程"))
        {
            Dictionary<Object, bool> tempDir = new Dictionary<Object, bool>(this.WordDataTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存

            //確認對照表，將被勾選的物件新增至課程
            foreach (var key in tempDir.Keys)
            {
                if (this.WordDataTogglesMap[key])
                {
                    if (!this.ClassInfoTogglesMap.ContainsKey(key))     //確認目前的課程是否已經有選擇的單字
                        this.ClassInfoTogglesMap.Add(key, false);
                    //將單字庫被選擇的狀態取消
                    this.WordDataTogglesMap[key] = false;
                }
            }
        }
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

            foreach (var key in tempDir.Keys)
            {
                //------一個課程的架構------
                GUILayout.BeginHorizontal();
                {
                    this.ClassListTogglesMap[key] = GUILayout.Toggle(this.ClassListTogglesMap[key], key);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("編輯"))
                    {
                        this.CurrentEditClassName = this.ModifyClassNameString = key;
                        StartCoroutine(this.CreateClassInfoTogglesMap());
                    }
                }
                GUILayout.EndHorizontal();
                //------一個課程的架構------
            }
        }
        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        {
            //-------課程清單"新增"、"刪除"兩個Button設置-------
            if (GUILayout.Button("新增"))
            {
                //確認是否有重複命名問題
                while (this.ClassListTogglesMap.ContainsKey("未命名課程" + this.CreateClassCount))
                    this.CreateClassCount++;

                //新增資訊至XML File
                XmlManager.script.CreateNewClassToList("未命名課程" + this.CreateClassCount);

                //將新增課程至對照表
                this.ClassListTogglesMap.Add("未命名課程" + this.CreateClassCount, false);
                //將清單滾輪移動至最下方
                this.ClassListScrollViewPosition.y = int.MaxValue;
            }
            if (GUILayout.Button("刪除"))
            {
                Dictionary<string, bool> tempDir = new Dictionary<string, bool>(this.ClassListTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存
                //確認對照表，將被勾選的物件刪除
                foreach (var key in tempDir.Keys)
                {
                    if (this.ClassListTogglesMap[key])
                    {
                        //將XML File中對應的課程刪除
                        XmlManager.script.DeleteClassFromList(key);

                        //將ClassListTogglesMap中對應的課程刪除
                        this.ClassListTogglesMap.Remove(key);
                    }
                }
            }
            //-------課程清單"新增"、"刪除"兩個Button設置-------
        }
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// 提醒視窗使用(將會依照傳入ID來決定是哪種提示視窗)
    /// </summary>
    /// <param name="windowID">識別視窗種類ID</param>
    void HintWindow(int windowID)
    {
        switch ((WindowID)windowID)
        {
            //課程清單刪除提示
            case WindowID.ClassListDeleteHint:
                GUILayout.Label("即將刪除選擇清單");
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("確定"))
                    {
                        //未完成
                    }
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("取消"))
                    {
                        //未完成
                    }
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();
                break;
            default:
                break;
        }
    }

    public enum WindowID
    {
        ClassListWindow = 0, WordDataWindow = 1, ClassInfoWindow = 2,
        ClassListDeleteHint = 100
    }
}
