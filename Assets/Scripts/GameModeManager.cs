using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager script;

    public GameObject TimerObject;

    public GameObject TotalScoreObject;
    [HideInInspector]
    public int CurrentTotalScore;

    public GameObject RecognizedScoreObject;
    public GameObject BonusScoreObject;
    public GameObject FailHintObject;

    public GameObject StopWindowObject;
    public GameObject StopButton;

    public GameObject EndingScoreObject;
    public GameObject EndingObject;

    public GameObject PicturePrefab;    //生成物件Prefab
    public float CreatePositionDistance;            //生成物件間隔

    [HideInInspector]
    public bool canRecognized;
    [HideInInspector]
    public ScreenRect Screenrect;
   // [HideInInspector]
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
        this.TimerObject.SetActive(true);

        this.StopWindowObject.SetActive(false);
        this.StopButton.SetActive(true);

        this.TotalScoreObject.SetActive(false);
        this.RecognizedScoreObject.SetActive(false);
        this.BonusScoreObject.SetActive(false);

        this.FailHintObject.GetComponent<TextMesh>().text = "發音不標準！\n加油！";
        this.FailHintObject.SetActive(false);

        this.EndingObject.SetActive(false);

        this.canRecognized = false;

        //-----擷取可視螢幕範圍-----
        Vector3 leftbottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 righttop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        this.Screenrect.top = righttop.y;
        this.Screenrect.bottom = leftbottom.y;
        this.Screenrect.left = leftbottom.x;
        this.Screenrect.right = righttop.x;

        this.SafeScreenrect.top = righttop.y + 6;
        this.SafeScreenrect.bottom = leftbottom.y;
        this.SafeScreenrect.left = leftbottom.x + 5 + 25;
        this.SafeScreenrect.right = righttop.x - 5;
        //-----擷取可視螢幕範圍-----
    }

    public void StartCreatePicture()
    {
        this.canRecognized = true;
        InvokeRepeating("CreatePicture", 0.1f, 0.1f);    //產生圖片設定，固定檢查間隔

        this.TotalScoreObject.SetActive(true);
        this.TotalScoreObject.GetComponent<TextMesh>().text = this.CurrentTotalScore.ToString();
    }

    public void StopGame()
    {
        this.canRecognized = false;
        this.StopWindowObject.SetActive(true);
        this.StopButton.SetActive(false);
        Time.timeScale = 0.00001f;
    }

    public void ResumeGame()
    {
        this.canRecognized = false;
        this.StopWindowObject.SetActive(false);
        this.StopButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void EndGame()
    {
        this.canRecognized = true;

        CancelInvoke("CreatePicture");
        foreach (PictureController script in GameObject.FindObjectsOfType<PictureController>())
            script.DestroySelf();

        this.StopButton.SetActive(false);
        this.TimerObject.SetActive(false);
        this.TotalScoreObject.SetActive(false);
        this.RecognizedScoreObject.SetActive(false);
        this.BonusScoreObject.SetActive(false);
        this.FailHintObject.SetActive(false);

        this.EndingScoreObject.GetComponent<TextMesh>().text = "Score：" + this.TotalScoreObject.GetComponent<TextMesh>().text;
        this.EndingObject.SetActive(true);
    }

    /// <summary>
    /// 產生圖片設定，固定檢查間隔
    /// </summary>
    void CreatePicture()
    {
        if (this.CurrentActivePictureList.Count == 0)
        {
            //設定產生位置(不能超出螢幕可視範圍)
            Vector3 createPos = new Vector3(Random.Range(this.SafeScreenrect.left, this.SafeScreenrect.right), this.SafeScreenrect.top, 0);
            Instantiate(this.PicturePrefab, createPos, this.PicturePrefab.transform.rotation);
        }
        else
        {
            if (Mathf.Abs(this.CurrentActivePictureList[this.CurrentActivePictureList.Count - 1].transform.position.y - this.SafeScreenrect.top) > this.CreatePositionDistance)
            {
                //設定產生位置(不能超出螢幕可視範圍)
                Vector3 createPos = new Vector3(Random.Range(this.SafeScreenrect.left, this.SafeScreenrect.right), this.SafeScreenrect.top, 0);
                Instantiate(this.PicturePrefab, createPos, this.PicturePrefab.transform.rotation);
            }
        }
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