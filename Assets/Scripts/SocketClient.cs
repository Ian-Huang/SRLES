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
        }
        catch (Exception e)
        {
            print(e.Message);
            print("主機" + this.IP + " 通訊埠 " + this.Port.ToString() + " 無法連接  !!");
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    void ReadServerResponse()
    {
        this.myNetworkStream.BeginRead(this.myBufferBytes, 0, 1024, new AsyncCallback(this.EndReadCallback), null);
    }

    private void EndReadCallback(IAsyncResult ar)
    {
        //解析Server的回應
        int bytesRead = this.myNetworkStream.EndRead(ar);
        string reString = Encoding.UTF8.GetString(this.myBufferBytes, 0, bytesRead);    //Btye to String

        print(reString);
        switch (reString)
        {
            default:
                break;
        }
        this.ReadServerResponse();  //再次等待Server的回應
    }


    void OnGUI()
    {
        this.TestString = GUI.TextField(new Rect(50, 50, 200, 50), this.TestString);
        if (GUI.Button(this.rect, "發送訊息"))
        {
            Byte[] myBytes = Encoding.UTF8.GetBytes(this.TestString);
            this.myNetworkStream.Write(myBytes, 0, myBytes.Length);
        }
        if (GUI.Button(new Rect(50, 175, 200, 50), "設定單字"))
        {
            //-----發送訊息，讓語音伺服器開始設定辨識單字-----
            string sendStr = "SettingWord:";    //功能設定字
            foreach (var temp in GameManager.script.TextureCollection)
                sendStr += (temp.name + ",");   //加入待辨識的字串

            Byte[] myBytes = Encoding.UTF8.GetBytes(sendStr);       //String to Byte
            this.myNetworkStream.Write(myBytes, 0, myBytes.Length); //發送至Server
            //-----發送訊息，讓語音伺服器開始設定辨識單字-----
        }
    }

    /// <summary>
    /// 結束程式時，關閉語音辨識伺服器
    /// </summary>
    void OnApplicationQuit()
    {
        //this.myTcpClient.Close();       //關閉連接Server的接口
        this.process.CloseMainWindow(); //關閉
    }
}
