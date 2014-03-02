using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public ButtonEvent buttonEvent;

    void OnMouseDown()
    {
        this.ButtonDown(this.buttonEvent);
    }

    void ButtonDown(ButtonEvent BtnEvent)
    {
        //分別處理按鈕事件
        switch (BtnEvent)
        {
            case ButtonEvent.EnterReadyGame:
                Application.LoadLevel("ReadyGame");
                break;
            case ButtonEvent.EnterEditMode:
                Application.LoadLevel("Edit");
                break;
            case ButtonEvent.EnterTrainMode:
                Application.LoadLevel("TrainMode");
                break;
            case ButtonEvent.EnterGameMode:
                Application.LoadLevel("Game");
                break;
            case ButtonEvent.EnterHome:
                Application.LoadLevel("Home");
                break;
            case ButtonEvent.ExitGame:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public enum ButtonEvent
    {
        EnterReadyGame = 0, EnterEditMode = 1, ExitGame = 2, EnterGameMode = 3, EnterTrainMode = 4, EnterHome = 5
    }
}
