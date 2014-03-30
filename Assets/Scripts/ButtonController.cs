using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public ButtonEvent buttonEvent;

    void OnMouseUp()
    {
        this.ButtonUp(this.buttonEvent);
    }

    void ButtonUp(ButtonEvent BtnEvent)
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
                ReadyGameUI script = GameObject.FindObjectOfType<ReadyGameUI>();
                GameDefinition.GameMode_DownSpeed = script.SetValue_DownSpeed;
                GameDefinition.GameMode_GameTime = script.SetValue_GameTime * 30;
                GameDefinition.GameMode_SuccessScore = script.SetValue_SuccessScore;
                Application.LoadLevel("Game");
                break;
            case ButtonEvent.EnterHome:
                Time.timeScale = 1;
                Application.LoadLevel("Home");
                break;
            case ButtonEvent.ExitGame:
                Application.Quit();
                break;
            case ButtonEvent.StopGame:
                if (GameObject.FindObjectOfType<GameModeManager>() != null)
                    GameModeManager.script.StopGame(this.gameObject);
                else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
                    TrainModeManager.script.StopGame(this.gameObject);
                break;
            case ButtonEvent.ResumeGame:
                if (GameObject.FindObjectOfType<GameModeManager>() != null)
                    GameModeManager.script.ResumeGame();
                else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
                    TrainModeManager.script.ResumeGame();
                break;
            default:
                break;
        }
    }

    public enum ButtonEvent
    {
        EnterReadyGame = 0, EnterEditMode = 1, ExitGame = 2, EnterGameMode = 3, EnterTrainMode = 4, EnterHome = 5,
        StopGame = 6, ResumeGame = 7
    }
}
