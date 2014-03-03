using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ABTextureManager : MonoBehaviour
{
    public static ABTextureManager script;

    public bool ABRoadFinish = false;          //讀取狀態flag
    public List<Object> TextureCollection;  //自AB讀取到的圖片清單
    public List<Object> ChooseClassWordCollection;  //自ReadyGame場景選擇進行遊戲的單字集合

    void Awake()
    {
        if (ABTextureManager.script != null)
            Destroy(this.gameObject);
        else
            script = this;
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        this.LoadAssetBundle();
    }

    public void LoadAssetBundle()
    {
        StartCoroutine(RunLoadAssetBundle());
    }

    /// <summary>
    /// 載入AssetBundle圖庫函式
    /// </summary>
    /// <returns></returns>
    IEnumerator RunLoadAssetBundle()
    {
        this.ABRoadFinish = false;  //檔案讀取flag (正在讀取...)

        WWW www = new WWW(@"file:///" + Application.dataPath + @"/all.assetBunldes");

        while (www == null)     //等待下載完成
            yield return null;

        this.TextureCollection = new List<Object>(www.assetBundle.LoadAll());   //將AB中的圖片載入到List清單
        www.assetBundle.Unload(false);

        this.ABRoadFinish = true; //檔案讀取flag (讀取完畢...)
    }
}
