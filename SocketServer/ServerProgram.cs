using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Speech.Recognition;
using System.Threading;
using System.Text;

namespace SocketServer
{
    /// <summary>
    /// Description：
    ///     語音伺服器物件
    /// </summary>
    class ServerProgram
    {
        private SpeechRecognitionEngine speechEngine;   //語音辨識引擎
        private RecognizerInfo recognizerInfo = null;   //語音辨識引擎資訊

        private bool checkClientClosed = false;         //確認目前與Client連線狀態
        private Socket mySocket;                        //Server Socket物件

        public static void Main(string[] args)
        {
            ServerProgram master = new ServerProgram();

            //將語音辨識初始化設定獨立執行緒
            Thread SRInitThread = new Thread(new ThreadStart(master.SpeechRecongnitionInit));
            SRInitThread.Start();

            //建立監聽物件
            TcpListener myTcpListener = new TcpListener(System.Net.IPAddress.Parse("127.0.0.1"), 36000);
            myTcpListener.Start();  //啟動監聽
            Console.WriteLine("通訊埠 36000 等待用戶端連線...... !!");
            master.mySocket = myTcpListener.AcceptSocket();     //這邊會開始等待client連線(程式會在此等待回應)
            Console.WriteLine("連線成功 !!");

            //開始進入接收訊息迴圈，通過接收的訊息進行Server動作的判定
            do
            {
                try
                {
                    //使用Peek測試連線是否仍存在
                    if (master.mySocket.Connected && master.mySocket.Poll(0, SelectMode.SelectRead))
                        master.checkClientClosed = (master.mySocket.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                catch (Exception e)
                {
                    master.checkClientClosed = true;    //Client關閉連線
                    Console.WriteLine("連線測試Exception：" + e.Message);
                }

                if (!master.checkClientClosed)
                {
                    //取得用戶端寫入的資料
                    int dataLength;
                    byte[] myBufferBytes = new byte[1024];
                    string receiveString;

                    dataLength = master.mySocket.Receive(myBufferBytes);    //接收Client發送的訊息(程式會在此等待回應)
                    receiveString = Encoding.UTF8.GetString(myBufferBytes, 0, dataLength);  //Byte to String
                    master.SendMessageToClient("回復Client端發送的訊息-" + receiveString);

                    //字串判定
                    string[] receiveStringSplit = receiveString.Split(':'); //動作字串分析 [0] ，以":"做區隔
                    switch (receiveStringSplit[0])
                    {
                        case "SettingWord":     //設定辨識單字
                            List<string> words = receiveStringSplit[1].Split(',').ToList();
                            words.RemoveAt(words.Count - 1);    //移掉最後面多出的空字串
                            //foreach (string str in words)
                            //{
                            //    Console.WriteLine(str);
                            //}
                            master.SendMessageToClient("語音辨識伺服器，開始設定單字");
                            master.SpeechRecongnitionSettingWords(words.ToArray());
                            break;
                        case "Abc":
                            Console.WriteLine(receiveString);
                            master.SendMessageToClient("請刪除全部物件");
                            break;
                        default:
                            break;
                    }
                }

                Thread.Sleep(10);
            } while (!master.checkClientClosed);

            Console.WriteLine("結束連線");
        }

        /// <summary>
        /// 初始化語音辨識相關功能
        /// </summary>
        private void SpeechRecongnitionInit()
        {
            //Initialize speech recognition    
            Console.WriteLine("語音相關物件初始化開始");

            try
            {
                recognizerInfo = (from a in SpeechRecognitionEngine.InstalledRecognizers()
                                  where a.Culture.Name == "en-US"
                                  select a).FirstOrDefault();
            }
            catch (Exception)
            {
                this.SendMessageToClient("語音辨識引擎設定錯誤");
            }

            if (recognizerInfo != null)
            {
                this.speechEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
                Choices recognizerString = new Choices();
                recognizerString.Add("Init test");

                GrammarBuilder grammarBuilder = new GrammarBuilder();

                //Specify the culture to match the recognizer in case we are running in a different culture.                                 
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(recognizerString);

                // Create the actual Grammar instance, and then load it into the speech recognizer.
                Grammar grammar = new Grammar(grammarBuilder);

                //載入辨識字串
                this.speechEngine.LoadGrammarAsync(grammar);
                this.speechEngine.SpeechRecognized += SreSpeechRecognized;

                try
                {
                    this.speechEngine.SetInputToDefaultAudioDevice();
                    this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception)
                {
                    this.SendMessageToClient("麥克風裝置設定錯誤");
                }
            }
            Console.WriteLine("語音相關物件初始化完成");
        }

        /// <summary>
        /// 語音辨識單字設定
        /// </summary>
        private void SpeechRecongnitionSettingWords(string[] words)
        {
            //Initialize speech recognition    
            Console.WriteLine("語音辨識單字設定開始");

            if (recognizerInfo != null)
            {
                this.speechEngine.UnloadAllGrammars();

                Choices recognizerString = new Choices();
                recognizerString.Add(words);

                GrammarBuilder grammarBuilder = new GrammarBuilder();

                //Specify the culture to match the recognizer in case we are running in a different culture.                                 
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(recognizerString);

                // Create the actual Grammar instance, and then load it into the speech recognizer.
                Grammar grammar = new Grammar(grammarBuilder);

                //載入辨識字串                
                this.speechEngine.LoadGrammarAsync(grammar);
            }
            Console.WriteLine("語音辨識單字設定完成");
        }

        /// <summary>
        /// 當辨識到設定字串將呼叫此函式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string message = "辨識單字：" + e.Result.Text + "準確度：" + e.Result.Confidence.ToString("0.00");
            Console.WriteLine(message);

            string sendStr = "DeletePicture:";    //功能設定字(使Client刪除辨識單字圖片)
            sendStr += e.Result.Text;

            this.SendMessageToClient(sendStr);
        }

        /// <summary>
        /// 傳送訊息到Client端
        /// </summary>
        /// <param name="message">訊息內容</param>
        void SendMessageToClient(string message)
        {
            Byte[] myBytes = Encoding.UTF8.GetBytes(message);       //String to Byte
            try
            {
                Console.WriteLine("對Client傳送： " + message);
                this.mySocket.Send(myBytes, myBytes.Length, 0);     //送出到Client
            }
            catch (Exception e)
            {
                Console.WriteLine("傳送訊息Exception：" + e.Message);
            }
        }

        /// <summary>
        /// 解構元(釋放資源)
        /// </summary>
        ~ServerProgram()
        {
            try
            {
                //Server Socket 關閉
                this.mySocket.Close();

                //語音辨識引擎 關閉
                if (this.speechEngine != null)
                {
                    this.speechEngine.RecognizeAsyncCancel();
                    this.speechEngine.RecognizeAsyncStop();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("解構元Exception：" + e.Message);
            }
        }
    }
}
