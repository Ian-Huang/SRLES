using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class ServerProgram
    {
        public static void Main(string[] args)
        {
            bool closed = false;
            byte[] testByte = new byte[1];

            System.Net.IPAddress theIPAddress;
            //建立 IPAddress 物件(本機)
            theIPAddress = System.Net.IPAddress.Parse("127.0.0.1");

            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(theIPAddress, 36000);
            //啟動監聽
            myTcpListener.Start();
            Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
            Socket mySocket = myTcpListener.AcceptSocket();
            do
            {
                try
                {
                    //偵測是否有來自用戶端的連線要求，若是
                    //用戶端請求連線成功，就會秀出訊息。
                    //if (mySocket.Connected)
                    //{


                    try
                    {
                        //使用Peek測試連線是否仍存在
                        if (mySocket.Connected && mySocket.Poll(0, SelectMode.SelectRead))
                            closed = (mySocket.Receive(testByte, SocketFlags.Peek) == 0);
                    }
                    catch (SocketException se)
                    {
                        closed = true;
                    }
                    if (!closed)
                    {
                        int dataLength;
                        Console.WriteLine("連線成功 !!");
                        byte[] myBufferBytes = new byte[1000];
                        //取得用戶端寫入的資料
                        dataLength = mySocket.Receive(myBufferBytes);

                        Console.WriteLine("接收到的資料長度 {0} \n ", dataLength.ToString());
                        Console.WriteLine("取出用戶端寫入網路資料流的資料內容 :");
                        Console.WriteLine(Encoding.ASCII.GetString(myBufferBytes, 0, dataLength) + "\n");
                        
                        //將接收到的資料回傳給用戶端
                        int si = mySocket.Send(myBufferBytes, dataLength, 0);
                    }
                    else
                    {
                        mySocket.Close();
                        break;
                    }
                    Thread.Sleep(20);
                    //}
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    mySocket.Close();
                    break;
                }
            } while (true);
            Console.WriteLine("結束連線");
            Console.Read();        
        }
    }
}
