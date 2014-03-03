using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
    public Vector3 StartPoint;
    public Vector3 RunPoint;
    public Vector3 EndPoint;

    public float RunTime;
    public iTween.EaseType easeType;

    // Use this for initialization
    void Start()
    {
        this.transform.position = this.StartPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.RunPoint, "time", this.RunTime, "easetype", this.easeType));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
