using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;
using System.Text.Json.Nodes;
using Server1;
using System.IO;

namespace Server1
{
    internal class Program
    {
        static void Main()
        {
            // 서버의 IP 주소와 포트 번호
            string ipAddress = "127.0.0.1";
            int port = 50000;
            // 이미지 저장 경로
            string imagePath = "image.jpg";
            string saveFilePath = "log/image.jpg";

            try
            {
                // 서버 소켓 생성
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // 클라이언트 연결 대기
                server.Start();
                Console.WriteLine("server start... waiting Client");

                while (true)
                {
                    // 클라이언트 연결 수락
                    TcpClient AcceptedClient = server.AcceptTcpClient();
                    Console.WriteLine("Connection to client!");
                    // 새로운 클라이언트가 연결되면 쓰레드 생성 및 시작
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(AcceptedClient);
                }

                // 클라이언트 연결 수락
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connection to client!");


                void HandleClient(object clientObj)
                {
                    TcpClient client = (TcpClient)clientObj;
                    
                    // 클라이언트와의 데이터 통신 처리
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1036];
                    int bytesRead = 0;
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                        Console.WriteLine("reading bytes");
                        Console.WriteLine("byteRead: "+ bytesRead);
                        string receivedString = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine("receiveString: " + receivedString);
                        // 프레임 구분자를 기준으로 데이터 분할
                        string[] frames = receivedString.Split(new string[] { "||" }, StringSplitOptions.None);

                        foreach (string frame in frames)
                        {
                            Console.WriteLine("check frame!!!!");
                            Console.WriteLine("frame: " + frame);
                            // 프레임 헤더와 푸터를 사용하여 유효한 프레임인지 확인
                            if (frame.StartsWith("start") && frame.EndsWith("end"))
                            {
                                // 프레임 헤더와 푸터를 제거한 실제 데이터 추출
                                string frameData = frame.Substring(5, frame.Length - 8);
                                Console.WriteLine("frameData: " + frameData);
                                // 데이터 저장 등 필요한 처리 수행
                                // 여기서는 예시로 파일에 저장하는 코드를 제공합니다.
                                Console.WriteLine("making file stream");
                                using (FileStream fileStream = new FileStream("frame.jpg", FileMode.Append))
                                {
                                    byte[] frameBytes = Encoding.UTF8.GetBytes(frameData);
                                    fileStream.Write(frameBytes, 0, frameBytes.Length);
                                }
                                Console.WriteLine("file success!!!!!!!!!!!!!!!!!!!!!!");
                            }
                        }
                    }

                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    Console.WriteLine("memory streaming");
                    //    int byteRead;
                    //    int cnt = 0;
                    //if ((byteRead = stream.Read(buffer, 0, buffer.Length) )> 0)
                    //{

                    //    Console.WriteLine(buffer.Length);
                    //    Console.WriteLine(byteRead);
                    //    cnt++;
                    //    memoryStream.Write(buffer, 0, byteRead);
                    //    Console.WriteLine(memoryStream.ToArray());
                    //}

                    //    // 이미지 저장
                    //    File.WriteAllBytes("image"+cnt.ToString()+".png", memoryStream.ToArray());
                    //}
                    //Console.WriteLine("image store success!");

                    //while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    //{

                    //    // 수신한 데이터 처리
                    //    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    //    Console.WriteLine(data);
                    //    dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                    //    if (jsonObject.ROUTE == "Login") // Login이면
                    //    {
                    //        LoginHandler(jsonObject);
                    //    }
                    //    else if (jsonObject.ROUTE == "Register") // Register이면
                    //    {
                    //        RegisterHandler(jsonObject);
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("else");
                    //    }
                    //}
                    //    // 클라이언트에게 응답 전송
                    //    byte[] response = Encoding.UTF8.GetBytes("서버 응답: " + data);
                    //    stream.Write(response, 0, response.Length);
                    //}

                    // 클라이언트와의 연결 종료
                    Console.WriteLine("client " + client.Client.RemoteEndPoint + "end");
                    client.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("오류: " + e.Message);
            }
            Console.ReadLine();
        }

        // Login packet
        public static void LoginHandler(dynamic jsonData)
        {
            Console.WriteLine("Login Handler!");
            Console.WriteLine(jsonData);
            SQLClass.LoginPostSQL(jsonData);
        }

        // Register packet
        public static void RegisterHandler(dynamic jsonData)
        {
            Console.WriteLine("Register Handler!");
            Console.WriteLine(jsonData);
            SQLClass.RegisterPostSQL(jsonData);
        }


    }
}