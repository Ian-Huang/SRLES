using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    public float MoveTime;
    public float DelayTime;

    public GameObject TimerTextObject;
    private int GameTime;

    // Use this for initialization
    void Start()
    {
        this.GameTime = GameDefinition.GameMode_GameTime;
        this.TimerTextObject.GetComponent<TextMesh>().text = this.GameTime.ToString();
        this.Move();
        InvokeRepeating("TimerFunction", 4, 1);
    }

    public void Move()
    {
        this.transform.position = this.StartPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.EndPoint, "time", this.MoveTime, "delay", this.DelayTime, "easetype", iTween.EaseType.spring));
    }

    void TimerFunction()
    {
        this.TimerTextObject.GetComponent<TextMesh>().text = this.GameTime.ToString();
        if (this.GameTime == 0)
        {
            //時間到
            print("時間到");
            GameModeManager.script.EndGame();
            CancelInvoke("TimerFunction");
        }
        this.GameTime--;
    }
}
