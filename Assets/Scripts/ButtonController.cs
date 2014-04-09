using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public ButtonEvent buttonEvent;

    public Sprite Sprite_normal;
    public Sprite Sprite_active;

    void OnMouseDown()
    {
        if (this.Sprite_active != null)
            this.GetComponent<SpriteRenderer>().sprite = this.Sprite_active;
    }

    void OnMouseUp()
    {
        if (this.Sprite_normal != null)
            this.GetComponent<SpriteRenderer>().sprite = this.Sprite_normal;
    }

    void OnMouseUpAsButton()
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
                //Application.LoadLevel("EditMode");
                if (GameObject.FindObjectOfType<ReadyGameUI>() != null)
                    GameObject.FindObjectOfType<ReadyGameUI>().EditPasswordWindowObject.SetActive(true);

                else if (GameObject.FindObjectOfType<HomeUI>() != null)
                    GameObject.FindObjectOfType<HomeUI>().EditPasswordWindowObject.SetActive(true);
                break;
            case ButtonEvent.EnterTrainMode:
                Application.LoadLevel("TrainMode");
                break;
            case ButtonEvent.EnterGameMode:
                if (GameObject.FindObjectOfType<ReadyGameUI>() != null)
                {
                    ReadyGameUI script = GameObject.FindObjectOfType<ReadyGameUI>();
                    GameDefinition.GameMode_DownSpeed = script.SetValue_DownSpeed;
                    GameDefinition.GameMode_SuccessScore = script.SetValue_SuccessScore;
                }
                Application.LoadLevel("GameMode");
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
                    GameModeManager.script.StopGame();
                else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
                    TrainModeManager.script.StopGame();
                break;
            case ButtonEvent.ResumeGame:
                if (GameObject.FindObjectOfType<GameModeManager>() != null)
                    GameModeManager.script.ResumeGame();
                else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
                    TrainModeManager.script.ResumeGame();
                break;
            case ButtonEvent.TrainModeSpeakButton:
                GameObject.FindObjectOfType<SocketClient>().SpeakWord(ABTextureManager.script.ChooseClassWordCollection[TrainModeManager.script.CurrentCardIndex].name);
                break;
            case ButtonEvent.TrainModeLeftArrow:
                TrainModeManager.script.PreviousCard();
                break;
            case ButtonEvent.TrainModeRightArrow:
                TrainModeManager.script.NextCard();
                break;
            case ButtonEvent.AddWordtoClassButton:
                GameObject.FindObjectOfType<EditUI>().AddWordtoClass();
                break;
            case ButtonEvent.DeleteWordfromClassButton:
                GameObject.FindObjectOfType<EditUI>().DeleteWordfromClass();
                break;
            case ButtonEvent.EditPasswordCancel:
                if (GameObject.FindObjectOfType<ReadyGameUI>() != null)
                    GameObject.FindObjectOfType<ReadyGameUI>().EditPasswordWindowObject.SetActive(false);

                else if (GameObject.FindObjectOfType<HomeUI>() != null)
                    GameObject.FindObjectOfType<HomeUI>().EditPasswordWindowObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public enum ButtonEvent
    {
        EnterReadyGame = 0, EnterEditMode = 1, ExitGame = 2, EnterGameMode = 3, EnterTrainMode = 4, EnterHome = 5,
        StopGame = 6, ResumeGame = 7,
        TrainModeSpeakButton = 8, TrainModeRightArrow = 9, TrainModeLeftArrow = 10,
        AddWordtoClassButton = 11, DeleteWordfromClassButton = 12,
        EditPasswordCancel = 13
    }
}
