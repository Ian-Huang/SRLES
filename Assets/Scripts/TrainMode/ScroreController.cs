using UnityEngine;
using System.Collections;

public class ScroreController : MonoBehaviour
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    public float MoveTime;

    // Use this for initialization
    void Start()
    {
        this.Move();
    }

    public void Move()
    {
        this.transform.position = this.StartPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.EndPoint, "time", this.MoveTime, "easetype", iTween.EaseType.spring));
    }
}
