using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public ButtonManager.ButtonEvent ButtonEvent;

    void OnMouseDown()
    {
        ButtonManager.script.ButtonDown(this.ButtonEvent);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
