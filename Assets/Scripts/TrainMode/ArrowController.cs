using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour
{
    public ArrowType Arrowtype;

    public Sprite Sprite_normal;
    public Sprite Sprite_active;

    void OnMouseDown()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.Sprite_active;
    }

    void OnMouseUp()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.Sprite_normal;
    }

    void OnMouseUpAsButton()
    {
        //按鈕觸發
        if (this.Arrowtype == ArrowType.LeftArrow)
            TrainModeManager.script.PreviousCard();
        else
            TrainModeManager.script.NextCard();
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum ArrowType
    {
        RightArrow = 1, LeftArrow = 2
    }
}
