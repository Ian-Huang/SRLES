using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EditUI : MonoBehaviour
{
    //課程清單位置(之後要寫成彈性化相容於各種解析度)
    public Rect ClassListWindowRect = new Rect(20, 20, 250, 250);
    public Rect HintWindowRect = new Rect(20, 20, 120, 50);

    public Vector2 ClassListScrollViewPosition; //課程清單滾輪位置
    public Dictionary<string, bool> ClassTogglesMap = new Dictionary<string, bool>();   //課程清單對照表
    public int CreateClassCount = 1;    //課程清單產生次數(避免重複命名使用)

    void OnGUI()
    {
        //課程清單視窗
        this.ClassListWindowRect = GUILayout.Window(0, this.ClassListWindowRect, this.ClassListWindow, "課程清單");

        //提醒視窗
        //this.HintWindowRect = GUILayout.Window(1, this.HintWindowRect, this.HintWindow, "提醒");
    }

    /// <summary>
    /// 課程清單視窗
    /// </summary>
    /// <param name="windowID"></param>
    void ClassListWindow(int windowID)
    {
        this.ClassListScrollViewPosition = GUILayout.BeginScrollView(ClassListScrollViewPosition);
        {
            Dictionary<string, bool> tempDir = new Dictionary<string, bool>(this.ClassTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存

            foreach (var key in tempDir.Keys)
            {
                //------一個課程的架構------
                GUILayout.BeginHorizontal();
                {
                    this.ClassTogglesMap[key] = GUILayout.Toggle(this.ClassTogglesMap[key], key);
                    GUILayout.FlexibleSpace();
                    GUILayout.Button("編輯");
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
                while (this.ClassTogglesMap.ContainsKey("未命名課程" + this.CreateClassCount))
                    this.CreateClassCount++;

                //將新增課程至對照表
                this.ClassTogglesMap.Add("未命名課程" + this.CreateClassCount, false);
                //將清單滾輪移動至最下方
                this.ClassListScrollViewPosition.y = int.MaxValue;
            }
            if (GUILayout.Button("刪除"))
            {
                Dictionary<string, bool> tempDir = new Dictionary<string, bool>(this.ClassTogglesMap);  //不可直接對Dictionary做讀取，須建立一個暫存

                foreach (var key in tempDir.Keys)
                {
                    if (this.ClassTogglesMap[key])
                        this.ClassTogglesMap.Remove(key);
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
        switch (windowID)
        {
            //課程清單刪除提示
            case 1:
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
}
