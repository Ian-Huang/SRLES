using UnityEngine;
using System.Collections;

public class TrainModeManager : MonoBehaviour
{
    public GameObject ArrowObject;      //箭頭物件
    public int CurrentCardIndex;        //當前圖庫索引號
    public GameObject CreateCardObject; //當前卡片物件

    [HideInInspector]
    public GameObject CurrentTargetObject;

    public static TrainModeManager script;

    public void NextCard()
    {
        if (this.CurrentCardIndex + 1 >= ABTextureManager.script.ChooseClassWordCollection.Count)
            this.CurrentCardIndex = 0;
        else
            this.CurrentCardIndex++;

        this.CurrentTargetObject.GetComponent<CardMove>().RightLeave();

        this.CurrentTargetObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetObject.GetComponent<CardMove>().LeftEnter();
    }

    public void PreviousCard()
    {
        if (this.CurrentCardIndex - 1 < 0)
            this.CurrentCardIndex = ABTextureManager.script.ChooseClassWordCollection.Count - 1;
        else
            this.CurrentCardIndex--;

        this.CurrentTargetObject.GetComponent<CardMove>().LeftLeave();

        this.CurrentTargetObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetObject.GetComponent<CardMove>().RightEnter();
    }

    /// <summary>
    /// 倒數結束後，開始產生第一張字卡
    /// </summary>
    public void StartTrainMode()
    {
        this.CurrentTargetObject = Instantiate(this.CreateCardObject) as GameObject;
        this.CurrentTargetObject.GetComponent<CardMove>().LeftEnter();

        this.ArrowObject.SetActive(true);
    }

    void Awake()
    {
        script = this;
    }

    // Use this for initialization
    void Start()
    {
        this.ArrowObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
