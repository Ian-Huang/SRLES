using UnityEngine;
using System.Collections;

public class TrainModeManager : MonoBehaviour
{
    public GameObject ArrowObject;      //箭頭物件
    public int CurrentCardIndex;        //當前圖庫索引號
    public GameObject CreateCardObject; //預設產生卡片物件
    public GameObject CreateScoreObject;//預設產生分數物件

    public GameObject StopWindowObject;
    public GameObject StopButton;

    [HideInInspector]
    public GameObject CurrentTargetCardObject;  //當前卡片物件
    [HideInInspector]
    public GameObject CurrentTargetScoreObject; //當前分數物件
    [HideInInspector]
    public bool canRecognized;

    public static TrainModeManager script;

    public void NextCard()
    {
        if (this.CurrentTargetScoreObject != null)
            Destroy(this.CurrentTargetScoreObject);

        if (this.CurrentCardIndex + 1 >= ABTextureManager.script.ChooseClassWordCollection.Count)
            this.CurrentCardIndex = 0;
        else
            this.CurrentCardIndex++;

        this.CurrentTargetCardObject.GetComponent<CardMove>().RightLeave();

        this.CurrentTargetCardObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetCardObject.GetComponent<CardMove>().LeftEnter();
    }

    public void PreviousCard()
    {
        if (this.CurrentTargetScoreObject != null)
            Destroy(this.CurrentTargetScoreObject);

        if (this.CurrentCardIndex - 1 < 0)
            this.CurrentCardIndex = ABTextureManager.script.ChooseClassWordCollection.Count - 1;
        else
            this.CurrentCardIndex--;

        this.CurrentTargetCardObject.GetComponent<CardMove>().LeftLeave();

        this.CurrentTargetCardObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetCardObject.GetComponent<CardMove>().RightEnter();
    }

    public void ShowScore(string score)
    {
        if (this.CurrentTargetScoreObject != null)
            Destroy(this.CurrentTargetScoreObject);

        this.CurrentTargetScoreObject = Instantiate(this.CreateScoreObject) as GameObject;
        this.CurrentTargetScoreObject.GetComponent<TextMesh>().text = score;
    }

    /// <summary>
    /// 倒數結束後，開始產生第一張字卡
    /// </summary>
    public void StartTrainMode()
    {
        this.canRecognized = true;
        this.CurrentTargetCardObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetCardObject.GetComponent<CardMove>().LeftEnter();

        this.ArrowObject.SetActive(true);
    }

    public void StopGame()
    {
        this.canRecognized = false;
        this.StopWindowObject.SetActive(true);
        this.StopButton.SetActive(false);
    }

    public void ResumeGame()
    {
        this.canRecognized = true;
        this.StopWindowObject.SetActive(false);
        this.StopButton.SetActive(true);
    }

    void Awake()
    {
        script = this;
    }

    // Use this for initialization
    void Start()
    {
        this.canRecognized = false;
        this.StopWindowObject.SetActive(false);
        this.StopButton.SetActive(true);
        this.ArrowObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
