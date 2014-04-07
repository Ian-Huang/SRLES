using UnityEngine;
using System.Collections;

public class GameDefinition
{
    public static float GameMode_DownSpeed;
    public static int GameMode_GameTime = 60;
    public static int GameMode_SuccessScore;

    public const int Slider_DownSpeedMin = 3;
    public const int Slider_DownSpeedMax = 12;

    public const int Slider_SuccessScoreMin = 40;
    public const int Slider_SuccessScoreMax = 80;

    public const float Normal_ScreenWidth = 1280.0f;
    public const float Normal_ScreenHeight = 720.0f;
}
