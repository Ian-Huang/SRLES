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
        this.transform.position = this.StartPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.EndPoint, "time", this.MoveTime, "easetype", iTween.EaseType.spring));

        //iTween.ColorTo(this.gameObject, iTween.Hash("from", 1.0f, "to", 0, "time", 3));
        //iTween.ValueTo(this.gameObject, iTween.Hash("from", 1.0f, "to", 0, "time", 3, "easetype", iTween.EaseType.linear, "easetype", iTween.EaseType.linear, "onupdate", "TextColorTo"));
    }

    //void TextColorTo(float value)
    //{
    //    print(value);
    //}

    // Update is called once per frame
    void Update()
    {
        //print(this.gameObject.renderer.material.color);

    }
}
