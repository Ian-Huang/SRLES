using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Diagnostics;
using System;
using System.Text;

/// <summary>
/// 連接語音辨識伺服器功能腳本
/// </summary>
public class SocketClient : MonoBehaviour
{
    private bool isReadingData = false; //確認是否開始進行分析字串
    private string readString;          //接收Server傳送訊息的完整字串

    public int bufferSize;
    public string TestString;
    public Rect rect;

    public string IP = "127.0.0.1";
    public int Port = 36000;

    //語音程式的Process
    private Process process;

    byte[] myBufferBytes = new byte[1024];

    //宣告網路資料流變數
    private NetworkStream myNetworkStream;
    //宣告 Tcp 用戶端物件
    private TcpClient myTcpClient;

    void Awake()
    {
        this.process = new Process();
        this.process.StartInfo.FileName = Application.dataPath + @"/SocketServer.exe";
        this.process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;  //輸出時記得改Hide
        this.process.Start();
    }

    // Use this for initialization
    void Start()
    {
        this.ServerConnect();       //對語音辨識伺服器進行連線
        this.ReadServerResponse();  //接收Server的回應
    }

    void ServerConnect()
    {
        //建立 TcpClient 物件
        this.myTcpClient = new TcpClient();
        try
        {
            //測試連線至遠端主機
            this.myTcpClient.Connect(this.IP, this.Port);
            print("連線成功 !!\n");
            this.myNetworkStream = myTcpClient.GetStream();

            StartCoroutine(this.SetRecognitionInfo());
        }
        catch (Exception e)
        {
            print(e.Message);
            print("主機" + this.IP + " 通訊埠 " + this.Port.ToString() + " 無法連接  !!");
            return;
        }
    }

    /// <summary>
    /// 設定即將辨識的單字與辨識度分數設定
    /// </summary>
    IEnumerator SetRecognitionInfo()
    {
        yield return new WaitForSeconds(3);     //等待3秒，在設定單字，以免導致過快設定出錯
        string sendStr;
        Byte[] myBytes;

        ////-----發送訊息，辨識度分數設定-----
        //sendStr = "SettingRecongnitionScore:";    //功能設定字
        //sendStr += GameDefinition.GameMode_SuccessScore.ToString();

        //myBytes = Encoding.UTF8.GetBytes(sendStr);       //String to Byte
        //this.myNetworkStream.Write(myBytes, 0, myBytes.Length); //發送至Server
        ////-----發送訊息，辨識度分數設定-----

        //-----發送訊息，讓語音伺服器開始設定辨識單字-----
        sendStr = "SettingWord:";    //功能設定字
        foreach (var temp in ABTextureManager.script.ChooseClassWordCollection)
            sendStr += (temp.name + ",");   //加入待辨識的字串

        myBytes = Encoding.UTF8.GetBytes(sendStr);       //String to Byte
        this.myNetworkStream.Write(myBytes, 0, myBytes.Length); //發送至Server
        //-----發送訊息，讓語音伺服器開始設定辨識單字-----
    }

    public void SpeakWord(string word)
    {
        string sendStr;
        Byte[] myBytes;

        //-----發送訊息，念出單字-----
        sendStr = "SpeakWord:";    //功能設定字
        sendStr += word;

        myBytes = Encoding.UTF8.GetBytes(sendStr);       //String to Byte
        this.myNetworkStream.Write(myBytes, 0, myBytes.Length); //發送至Server
        //-----發送訊息，念出單字-----

    }

    // Update is called once per frame
    void Update()
    {
        //透過接收Server端發送訊息，進行字串分析
        if (this.isReadingData)
        {
            print(this.readString);
            //字串判定
            string[] reStringSplit = this.readString.Split(':'); //動作字串分析 [0] ，以":"做區隔
            switch (reStringSplit[0])
            {
                case "RecognizedWord":     //刪除圖片(接收Server發送的刪除單字)
                    if (GameObject.FindObjectOfType<GameModeManager>() != null)
                    {
                        string[] reWordSplit = reStringSplit[1].Split(',');

                        if (Int32.Parse(reWordSplit[1]) >= GameDefinition.GameMode_SuccessScore)    //辨識分數大於設定值才能刪除物件
                        {
                            //將Server傳送的辨識字進行搜尋，如在場景上則進行刪除動作
                            GameObject tempObj = GameModeManager.script.CurrentActivePictureList.Find((obj) => { return obj.name == reWordSplit[0]; });
                            if (tempObj != null)
                                tempObj.GetComponent<PictureController>().DestroySelf();
                        }
                        else
                            print("分數過低");
                    }
                    else if (GameObject.FindObjectOfType<TrainModeManager>() != null)
                    {
                        string[] reWordSplit = reStringSplit[1].Split(',');
                        CardMove cardMove = TrainModeManager.script.CurrentTargetCardObject.GetComponent<CardMove>();
                        if (cardMove.isCanRecognized)   //確認可以開始辨識
                        {
                            if (cardMove.CardText == reWordSplit[0])    //目前卡片單字與辨識字相同
                            {
                                TrainModeManager.script.ShowScore(reWordSplit[1]);                                
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            this.isReadingData = false; //每次進行完後，關閉分析
        }
    }

    void ReadServerResponse()
    {
        this.myNetworkStream.BeginRead(this.myBufferBytes, 0, 1024, new AsyncCallback(this.EndReadCallback), null);
    }

    private void EndReadCallback(IAsyncResult ar)
    {
        //解析Server的回應
        int bytesRead = this.myNetworkStream.EndRead(ar);
        this.readString = Encoding.UTF8.GetString(this.myBufferBytes, 0, bytesRead);    //Btye to String
        this.isReadingData = true;  //開啟Update()，進行字串處理

        this.ReadServerResponse();  //再次等待Server的回應
    }


    //void OnGUI()
    //{
    //    this.TestString = GUI.TextField(new Rect(50, 50, 200, 50), this.TestString);
    //    if (GUI.Button(this.rect, "發送訊息"))
    //    {
    //        Byte[] myBytes = Encoding.UTF8.GetBytes(this.TestString);
    //        this.myNetworkStream.Write(myBytes, 0, myBytes.Length);
    //    }
    //    if (GUI.Button(new Rect(50, 175, 200, 50), "設定單字"))
    //    {
    //        //-----發送訊息，讓語音伺服器開始設定辨識單字-----
    //        string sendStr = "SettingWord:";    //功能設定字
    //        foreach (var temp in ABTextureManager.script.ChooseClassWordCollection)
    //            sendStr += (temp.name + ",");   //加入待辨識的字串

    //        Byte[] myBytes = Encoding.UTF8.GetBytes(sendStr);       //String to Byte
    //        this.myNetworkStream.Write(myBytes, 0, myBytes.Length); //發送至Server
    //        //-----發送訊息，讓語音伺服器開始設定辨識單字-----
    //    }
    //}

    ///// <summary>
    ///// 結束程式時，關閉語音辨識伺服器
    ///// </summary>
    //void OnApplicationQuit()
    //{
    //    //this.myTcpClient.Close();       //關閉連接Server的接口
    //    this.process.CloseMainWindow(); //關閉
    //}

    /// <summary>
    /// 場景結束時，關閉語音辨識伺服器
    /// </summary>
    void OnDisable()
    {
        this.process.CloseMainWindow(); //關閉
    }
}
