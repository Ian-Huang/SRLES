using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;

namespace SocketServer
{
    class ServerProgram
    {
        private SpeechRecognitionEngine speechEngine;  //語音辨識引擎
        private bool checkClientClosed = false;
        private byte[] testByte = new byte[1];

        private Socket mySocket;

        ~ServerProgram()
        {
            this.mySocket.Close();
            if (this.speechEngine != null)
            {
                this.speechEngine.RecognizeAsyncCancel();
                this.speechEngine.RecognizeAsyncStop();
            }
        }

        public static void Main(string[] args)
        {
            ServerProgram master = new ServerProgram();

            Thread SRInitThread = new Thread(new ThreadStart(master.CreateSpeechRecongnition));
            SRInitThread.Start();

            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 36000);
            //啟動監聽
            myTcpListener.Start();
            Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");

            master.mySocket = myTcpListener.AcceptSocket();
            do
            {
                try
                {
                    try
                    {
                        //使用Peek測試連線是否仍存在
                        if (master.mySocket.Connected && master.mySocket.Poll(0, SelectMode.SelectRead))
                            master.checkClientClosed = (master.mySocket.Receive(master.testByte, SocketFlags.Peek) == 0);
                    }
                    catch (Exception)
                    {
                        master.checkClientClosed = true;
                    }

                    if (!master.checkClientClosed)
                    {
                        int dataLength;
                        Console.WriteLine("連線成功 !!");
                        byte[] myBufferBytes = new byte[1000];
                        //取得用戶端寫入的資料
                        dataLength = master.mySocket.Receive(myBufferBytes);

                        Console.WriteLine("接收到的資料長度 {0} \n ", dataLength.ToString());
                        Console.WriteLine("取出用戶端寫入網路資料流的資料內容 :");
                        Console.WriteLine(Encoding.UTF8.GetString(myBufferBytes, 0, dataLength) + "\n");

                        //將接收到的資料回傳給用戶端
                        master.mySocket.Send(myBufferBytes, dataLength, 0);
                    }
                    else
                    {
                        break;
                    }
                    Thread.Sleep(20);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            } while (true);

            Console.WriteLine("結束連線");
        }

        private void CreateSpeechRecongnition()
        {
            //Initialize speech recognition    
            Console.WriteLine("語音相關物件初始化開始");
            var recognizerInfo = (from a in SpeechRecognitionEngine.InstalledRecognizers()
                                  where a.Culture.Name == "en-US"
                                  select a).FirstOrDefault();

            if (recognizerInfo != null)
            {
                this.speechEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
                Choices recognizerString = new Choices();
                recognizerString.Add("test");
                recognizerString.Add("apple");

                GrammarBuilder grammarBuilder = new GrammarBuilder();

                //Specify the culture to match the recognizer in case we are running in a different culture.                                 
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(recognizerString);

                // Create the actual Grammar instance, and then load it into the speech recognizer.
                var grammar = new Grammar(grammarBuilder);

                //載入辨識字串
                this.speechEngine.LoadGrammarAsync(grammar);
                this.speechEngine.SpeechRecognized += SreSpeechRecognized;
                this.speechEngine.SetInputToDefaultAudioDevice();
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            Console.WriteLine("語音相關物件初始化結束");
        }

        void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string message = "辨識單字：" + e.Result.Text + "準確度：" + e.Result.Confidence.ToString("0.00");

            Console.WriteLine(message);

            Byte[] myBytes = Encoding.UTF8.GetBytes(message);
            this.mySocket.Send(myBytes, myBytes.Length, 0);

            //if (e.Result.Confidence < this.confidenceValue)//肯定度低於0.75，判為錯誤語句
            //{
            //    //this.textBlock1.Text = e.Result.Text.ToString();
            //    return;
            //}

            //foreach (var name in imageNameList)
            //{
            //    if (name.Equals(e.Result.Text))
            //    {
            //        if (this.DeleteImage(name))
            //        {
            //            this.totalScore++;
            //            ScoreText.Text = this.totalScore.ToString();
            //            this.PlaySound(this.successSoundPlayer);
            //        }
            //    }
            //}
        }
    }
}
