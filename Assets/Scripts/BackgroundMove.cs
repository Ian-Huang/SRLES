using UnityEngine;
using System.Collections;

public class BackgroundMove : MonoBehaviour
{
    public float MoveTime;
    public float Delayime;

    public Vector2 movePosition;
    public Vector2 moveScale;

    // Use this for initialization
    void Start()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("x", this.movePosition.x, "y", this.movePosition.y, "delay", this.Delayime, "time", this.MoveTime));
        iTween.ScaleTo(this.gameObject, iTween.Hash("x", this.moveScale.x, "y", this.moveScale.y, "delay", this.Delayime, "time", this.MoveTime));
    }
}