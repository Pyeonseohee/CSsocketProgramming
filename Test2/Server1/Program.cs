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
                    byte[] buffer = new byte[24000];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Console.WriteLine("connect");


                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            Console.WriteLine("memory streaming");
                            int byteRead;
                            while ((byteRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                memoryStream.Write(buffer, 0, byteRead);
                            }

                            // 이미지 저장
                            File.WriteAllBytes(imagePath, memoryStream.ToArray());
                        }
                        Console.WriteLine("이미지 저장 완료");


                    // 수신한 데이터 처리
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine(data);
                        dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                        if (jsonObject.ROUTE == "Login") // Login이면
                        {
                            LoginHandler(jsonObject);
                        }
                        else if (jsonObject.ROUTE == "Register") // Register이면
                        {
                            RegisterHandler(jsonObject);
                        }
                        else
                        {
                            Console.WriteLine("else");
                        }
                        // 클라이언트에게 응답 전송
                        byte[] response = Encoding.UTF8.GetBytes("서버 응답: " + data);
                        stream.Write(response, 0, response.Length);
                    }

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