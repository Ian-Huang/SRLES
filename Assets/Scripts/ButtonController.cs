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
        switch (BtnEvent)
        {
            case ButtonEvent.EnterReadyGame:
                Application.LoadLevelAsync("ReadyGame");
                break;
            case ButtonEvent.EnterEditMode:
                Application.LoadLevelAsync("Edit");
                break;
            case ButtonEvent.EnterTrainMode:
                Application.LoadLevelAsync("TrainMode");
                break;
            case ButtonEvent.EnterGameMode:
                Application.LoadLevelAsync("Game");
                break;
            case ButtonEvent.EnterHome:
                Application.LoadLevelAsync("Home");
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
