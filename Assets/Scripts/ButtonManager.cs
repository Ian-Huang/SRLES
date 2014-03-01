using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager script;

    void Awake()
    {
        script = this;
    }

    public void ButtonDown(ButtonEvent BtnEvent)
    {
        switch (BtnEvent)
        {
            case ButtonEvent.EnterStartGame:
                Application.LoadLevelAsync("ReadyGame");
                break;
            case ButtonEvent.EnterEditMode:
                Application.LoadLevelAsync("Edit");
                break;
            case ButtonEvent.ExitGame:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum ButtonEvent
    {
        EnterStartGame = 0, EnterEditMode = 1, ExitGame = 2
    }
}
