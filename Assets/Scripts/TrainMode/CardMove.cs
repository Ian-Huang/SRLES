using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
    [HideInInspector]
    public string CardText;     //卡片名稱

    public bool isCanRecognized;

    public GameObject TextObejct;
    public GameObject TextureObject;

    public Vector3 LeftPoint;
    public Vector3 MiddlePoint;
    public Vector3 RightPoint;
    public float RunTime;
    public iTween.EaseType easeType;

    // Use this for initialization
    void Start()
    {
        this.CardText = ABTextureManager.script.ChooseClassWordCollection[TrainModeManager.script.CurrentCardIndex].name;
        this.TextObejct.GetComponent<TextMesh>().text = this.CardText;
        this.TextureObject.renderer.material.mainTexture = ABTextureManager.script.ChooseClassWordCollection[TrainModeManager.script.CurrentCardIndex] as Texture;
        this.isCanRecognized = false;
    }

    public void RightEnter()
    {
        this.transform.position = this.RightPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.MiddlePoint, "time", this.RunTime, "easetype", this.easeType, "oncomplete", "EnterComplete"));
    }
    public void LeftEnter()
    {
        this.transform.position = this.LeftPoint;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.MiddlePoint, "time", this.RunTime, "easetype", this.easeType, "oncomplete", "EnterComplete"));
    }
    void EnterComplete()
    {        
        GameObject.FindObjectOfType<SocketClient>().SpeakWord(this.CardText);
        this.isCanRecognized = true;
    }


    public void RightLeave()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.RightPoint, "time", this.RunTime, "easetype", this.easeType, "oncomplete", "LeaveComplete"));
    }
    public void LeftLeave()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", this.LeftPoint, "time", this.RunTime, "easetype", this.easeType, "oncomplete", "LeaveComplete"));
    }

    void LeaveComplete()
    {
        Destroy(this.gameObject);
    }
}
