﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager script;

    public GameObject StopWindowObject;
    private GameObject stopButton;

    public GameObject PicturePrefab;    //生成物件Prefab
    public float CreateTime;            //生成物件間隔

    [HideInInspector]
    public ScreenRect Screenrect;
    [HideInInspector]
    public ScreenRect SafeScreenrect;
    [HideInInspector]
    public List<GameObject> CurrentActivePictureList;   //將當前在場景上的圖片物件儲存進容器中

    void Awake()
    {
        script = this;
    }

    // Use this for initialization
    void Start()
    {
        this.StopWindowObject.SetActive(false);

        //-----擷取可視螢幕範圍-----
        Vector3 leftbottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 righttop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        this.Screenrect.top = righttop.y;
        this.Screenrect.bottom = leftbottom.y;
        this.Screenrect.left = leftbottom.x;
        this.Screenrect.right = righttop.x;

        this.SafeScreenrect.top = righttop.y + 6;
        this.SafeScreenrect.bottom = leftbottom.y;
        this.SafeScreenrect.left = leftbottom.x + 5;
        this.SafeScreenrect.right = righttop.x - 5;
        //-----擷取可視螢幕範圍-----
    }

    public void StartCreatePicture()
    {
        InvokeRepeating("CreatePicture", 0.1f, this.CreateTime);    //產生圖片設定，固定間隔
    }

    public void StopGame(GameObject SendButton)
    {
        this.StopWindowObject.SetActive(true);
        Time.timeScale = 0.00001f;
        this.stopButton = SendButton;
        SendButton.SetActive(false);
    }

    public void ResumeGame()
    {
        this.StopWindowObject.SetActive(false);
        this.stopButton.SetActive(true);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ABTextureManager.script.ABRoadFinish)
        {

        }
    }

    /// <summary>
    /// 產生圖片設定，固定間隔
    /// </summary>
    void CreatePicture()
    {
        //設定產生位置(不能超出螢幕可視範圍)
        Vector3 createPos = new Vector3(Random.Range(this.SafeScreenrect.left, this.SafeScreenrect.right), this.SafeScreenrect.top, 0);

        Instantiate(this.PicturePrefab, createPos, this.PicturePrefab.transform.rotation);
    }

    [System.Serializable]
    public class ScreenRect
    {
        public float top;       //上
        public float bottom;    //下
        public float left;      //左
        public float right;     //右
    }
}