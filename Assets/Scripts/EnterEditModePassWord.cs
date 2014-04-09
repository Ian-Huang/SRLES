using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EnterEditModePassWord : MonoBehaviour
{
    public Rect TextBoxRect;
    public Rect EnterButtonRect;

    private string enter_password = "";

    public GameObject ErrorTextObject;
    public GUISkin skin;
    private Vector2 ratio;

    // Use this for initialization
    void Start()
    {
        this.ErrorTextObject.SetActive(false);
    }

    void Update()
    {
        this.ratio = new Vector2(Screen.width / GameDefinition.Normal_ScreenWidth, Screen.height / GameDefinition.Normal_ScreenHeight);
    }

    void OnGUI()
    {
        GUI.skin = this.skin;

        this.enter_password = GUI.PasswordField(
            new Rect(this.TextBoxRect.x * this.ratio.x, this.TextBoxRect.y * this.ratio.y, this.TextBoxRect.width * this.ratio.x, this.TextBoxRect.height * this.ratio.y),
            this.enter_password, '*');

        if (this.ErrorTextObject.activeSelf)
        {
            if (this.enter_password.Length > 0)
                this.ErrorTextObject.SetActive(false);
        }

        if (GUI.Button(new Rect(this.EnterButtonRect.x * this.ratio.x, this.EnterButtonRect.y * this.ratio.y, this.EnterButtonRect.width * this.ratio.x, this.EnterButtonRect.height * this.ratio.y), "進入"))
        {
            if (enter_password == GameDefinition.EnterEditmodePassword)
            {
                Application.LoadLevel("EditMode");
            }
            else
            {
                this.enter_password = "";
                this.ErrorTextObject.SetActive(true);
            }
        }
    }
}