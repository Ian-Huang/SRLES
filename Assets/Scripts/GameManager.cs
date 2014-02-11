using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager script;

    public GameObject PicturePrefab;    //生成物件Prefab
    public float CreateTime;            //生成物件間隔

    public bool isLoading = true;       //讀取狀態flag

    public ScreenRect Screenrect;
    public ScreenRect SafeScreenrect;
    public List<Object> TextureCollection;  //自AB讀取到的圖片清單
    public List<GameObject> CurrentActivePictureList;

    void Awake()
    {
        script = this;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadAssetBundle());

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

        InvokeRepeating("CreatePicture", 0.1f, this.CreateTime);    //產生圖片設定，固定間隔
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.isLoading)
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

    /// <summary>
    /// 載入存有圖片的AssetBundle
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadAssetBundle()
    {
        this.isLoading = true;  //檔案讀取flag (正在讀取...)

        WWW www = new WWW(@"file:///" + Application.dataPath + @"/all.assetBunldes");

        yield return www;       //等待下載完成

        this.TextureCollection = new List<Object>(www.assetBundle.LoadAll());   //將AB中的圖片載入到List清單
        www.assetBundle.Unload(false);

        this.isLoading = false; //檔案讀取flag (讀取完畢...)
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