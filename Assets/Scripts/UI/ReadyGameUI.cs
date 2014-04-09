using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

//[ExecuteInEditMode]
public class ReadyGameUI : MonoBehaviour
{
    public GUISkin skin;

    public GameObject EnterGameModeButton;
    public GameObject EnterTrainModeButton;
    public GameObject EditPasswordWindowObject;

    //課程清單位置(之後要寫成彈性化相容於各種解析度)
    public Rect ClassListWindowRect = new Rect(25, 20, 250, 250);
    public Rect ClassInfoWindowRect = new Rect(300, 20, 250, 250);
    public Rect GameSettingWindowRect = new Rect(575, 20, 250, 250);


    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public List<string> ClassNameList = new List<string>();   //課程名稱清單
    private int ChooseClassIndex;    //目前被選擇的課程序號
    public string CurrentChooseClassName;         //當前被選擇的課程名稱
    private string[] classListArrary = new string[0];   //儲存所有課程名稱的矩陣

    public Vector2 ClassInfoScrollViewPosition; //課程資訊滾輪位置
    public List<Object> ClassInfoObjectList = new List<Object>();   //課程資訊物件清單

    public float SetValue_DownSpeed;
    public int SetValue_SuccessScore;

    private bool isClassinfoOpen = false;
    private string[] DifficultLevelArrary = new string[] { "簡單", "普通", "困難", "自訂" };   //儲存遊戲難易度名稱陣列
    private int ChooseDifficultLevelIndex = 1;    //目前被選擇的遊戲難易度序號
    private Vector2 ratio;

    void Start()
    {
        //場景一開始未選擇課程前，隱藏開始遊戲、練習模式按鈕(防呆)
        this.ChooseClassIndex = -1;
        this.EnterGameModeButton.SetActive(false);
        this.EnterTrainModeButton.SetActive(false);
        this.EditPasswordWindowObject.SetActive(false);
        //場景一開始未選擇課程前，隱藏開始遊戲、練習模式按鈕(防呆)

        this.SetValue_DownSpeed = (GameDefinition.Slider_DownSpeedMax + GameDefinition.Slider_DownSpeedMin) / 2.0f;
        this.SetValue_SuccessScore = (GameDefinition.Slider_SuccessScoreMax + GameDefinition.Slider_SuccessScoreMin) / 2;

        //先行載入課程清單資訊
        StartCoroutine(this.CreateClassNameList());
    }

    void Update()
    {
        this.ratio = new Vector2(Screen.width / GameDefinition.Normal_ScreenWidth, Screen.height / GameDefinition.Normal_ScreenHeight);
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
        {
            //排除沒有單字的課程
            if (node.ChildNodes.Count > 0)
                this.ClassNameList.Add(node.Name);
        }
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

        if (!EditPasswordWindowObject.activeSelf)
        {
            //課程清單視窗
            GUILayout.Window((int)WindowID.ClassListWindow,
                new Rect(this.ClassListWindowRect.x * this.ratio.x, this.ClassListWindowRect.y * this.ratio.y, this.ClassListWindowRect.width * this.ratio.x, this.ClassListWindowRect.height * this.ratio.y),
                this.ClassListWindow, "課程清單");

            if (this.isClassinfoOpen)
            {
                //課程資訊視窗
                GUILayout.Window((int)WindowID.ClassInfoWindow,
                     new Rect(this.ClassInfoWindowRect.x * this.ratio.x, this.ClassInfoWindowRect.y * this.ratio.y, this.ClassInfoWindowRect.width * this.ratio.x, this.ClassInfoWindowRect.height * this.ratio.y),
                     this.ClassInfoWindow, this.CurrentChooseClassName);

                //遊戲設定視窗
                GUILayout.Window((int)WindowID.GameSettingWindow,
                    new Rect(this.GameSettingWindowRect.x * this.ratio.x, this.GameSettingWindowRect.y * this.ratio.y, this.GameSettingWindowRect.width * this.ratio.x, this.GameSettingWindowRect.height * this.ratio.y),
                    this.GameSettingWindow, "遊戲設定");
            }
        }
    }

    /// <summary>
    /// 遊戲設定視窗
    /// </summary>
    /// <param name="windowID"></param>
    void GameSettingWindow(int windowID)
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Label("物品掉落速度", this.skin.GetStyle("SettingLable"));

            float oldSpeed = this.SetValue_DownSpeed;
            this.SetValue_DownSpeed = GUILayout.HorizontalSlider(this.SetValue_DownSpeed, GameDefinition.Slider_DownSpeedMin, GameDefinition.Slider_DownSpeedMax);
            if (oldSpeed != this.SetValue_DownSpeed)
                this.ChooseDifficultLevelIndex = 3; //困難度跳為"自訂"

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("慢", this.skin.GetStyle("SettingLable"));
                GUILayout.FlexibleSpace();
                GUILayout.Label("快", this.skin.GetStyle("SettingLable"));
            } GUILayout.EndHorizontal();
        } GUILayout.EndVertical();

        GUILayout.Box("", GUILayout.Height(2.5f * this.ratio.x));
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("發音準確度：", this.skin.GetStyle("SettingLable"));

                GUILayout.Label(this.SetValue_SuccessScore.ToString(), this.skin.GetStyle("SettingLable-Pick"));
                GUILayout.FlexibleSpace();
            } GUILayout.EndHorizontal();

            int oldScore = this.SetValue_SuccessScore;
            this.SetValue_SuccessScore = Mathf.FloorToInt(GUILayout.HorizontalSlider(this.SetValue_SuccessScore, GameDefinition.Slider_SuccessScoreMin, GameDefinition.Slider_SuccessScoreMax));
            if (oldScore != this.SetValue_SuccessScore)
                this.ChooseDifficultLevelIndex = 3; //困難度跳為"自訂"

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(GameDefinition.Slider_SuccessScoreMin.ToString(), this.skin.GetStyle("SettingLable"));
                GUILayout.FlexibleSpace();
                GUILayout.Label(GameDefinition.Slider_SuccessScoreMax.ToString(), this.skin.GetStyle("SettingLable"));
            } GUILayout.EndHorizontal();
        } GUILayout.EndVertical();

        GUILayout.Box("", GUILayout.Height(2.5f * this.ratio.x));
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        {
            GUILayout.Label("難易度", this.skin.GetStyle("SettingLable"));

            int oldIndex = this.ChooseDifficultLevelIndex;
            this.ChooseDifficultLevelIndex = GUILayout.SelectionGrid(this.ChooseDifficultLevelIndex, this.DifficultLevelArrary, 4);    //產生可選擇的清單

            //判斷被選擇的難易度是否不一樣，會將難易度做更換
            if (oldIndex != this.ChooseDifficultLevelIndex)
            {
                switch (this.ChooseDifficultLevelIndex)
                {
                    case 0:
                        this.SetValue_DownSpeed = GameDefinition.Slider_DownSpeedMin;
                        this.SetValue_SuccessScore = GameDefinition.Slider_SuccessScoreMin;
                        break;
                    case 1:
                        this.SetValue_DownSpeed = (GameDefinition.Slider_DownSpeedMax + GameDefinition.Slider_DownSpeedMin) / 2.0f;
                        this.SetValue_SuccessScore = (GameDefinition.Slider_SuccessScoreMax + GameDefinition.Slider_SuccessScoreMin) / 2;
                        break;
                    case 2:
                        this.SetValue_DownSpeed = GameDefinition.Slider_DownSpeedMax;
                        this.SetValue_SuccessScore = GameDefinition.Slider_SuccessScoreMax;
                        break;
                    default:
                        break;
                }
            }
        } GUILayout.EndVertical();
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
                        GUILayout.Label(content, GUILayout.Height(60 * this.ratio.x), GUILayout.Width((ClassInfoWindowRect.width - 50) * this.ratio.x));
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
                    this.isClassinfoOpen = true;
                }

                this.CurrentChooseClassName = this.classListArrary[this.ChooseClassIndex];  //儲存目前被選擇的課程名
                StartCoroutine(this.CreateClassInfoObjectList());
            }
        }
        GUILayout.EndScrollView();

    }


    public enum WindowID
    {
        ClassListWindow = 0, ClassInfoWindow = 2, GameSettingWindow = 3
    }
}
