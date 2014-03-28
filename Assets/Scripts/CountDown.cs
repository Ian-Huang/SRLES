using UnityEngine;
using System.Collections;

/// <summary>
/// 遊戲開始前3秒倒數
/// </summary>
public class CountDown : MonoBehaviour
{
    private SmoothMoves.BoneAnimation boneAnimation;

    // Use this for initialization
    void Start()
    {
        this.boneAnimation = this.GetComponent<SmoothMoves.BoneAnimation>();
        this.boneAnimation.Play("ReadyGo");
        this.boneAnimation.RegisterUserTriggerDelegate(this.CountDownEnding);
    }

    void CountDownEnding(SmoothMoves.UserTriggerEvent triggerDelegate)
    {
        if (GameObject.FindObjectOfType<GameModeManager>() != null)
            GameModeManager.script.StartCreatePicture();    //呼叫GameManager開始產生物件

        else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
            TrainModeManager.script.StartTrainMode();

        Destroy(this.gameObject);
    }
}