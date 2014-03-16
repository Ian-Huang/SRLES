using UnityEngine;
using System.Collections;

public class BackgroundMove : MonoBehaviour
{
    public float MoveTime;

    // Use this for initialization
    void Start()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("x", 2.1f, "y", 2.1f, "time", this.MoveTime));
        iTween.MoveTo(this.gameObject, iTween.Hash("x", 2.7f, "y", -0.67f, "time", this.MoveTime));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
