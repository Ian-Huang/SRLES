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
        this.process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
        this.process.Start();
    }

    // Use this for initialization
    void Start()
    {
        this.ServerConnect();
        this.ReadDataInit();
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

    void ReadDataInit()
    {
        this.myNetworkStream.BeginRead(this.myBufferBytes, 0, 1024, new AsyncCallback(this.EndReadCallback), null);
    }

    private void EndReadCallback(IAsyncResult ar)
    {
        int bytesRead = this.myNetworkStream.EndRead(ar);
        string reString = Encoding.ASCII.GetString(this.myBufferBytes, 0, bytesRead);
        print(reString);
        this.ReadDataInit();
    }


    void OnGUI()
    {
        this.TestString = GUI.TextField(new Rect(50, 50, 200, 50), this.TestString);
        if (GUI.Button(this.rect, "發送訊息"))
        {
            Byte[] myBytes = Encoding.ASCII.GetBytes(this.TestString);
            this.myNetworkStream.Write(myBytes, 0, myBytes.Length);
        }
    }

    /// <summary>
    /// 結束程式時，關閉語音辨識伺服器
    /// </summary>
    void OnApplicationQuit()
    {
        this.process.CloseMainWindow(); //關閉

    }
}
