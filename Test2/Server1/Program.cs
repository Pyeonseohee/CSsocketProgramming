using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Server1
{
    internal class Program
    {
        static void Main()
        {
            // 서버의 IP 주소와 포트 번호
            string ipAddress = "127.0.0.1";
            int port = 50000;

            try
            { 
                // 서버 소켓 생성
                TcpListener server = new TcpListener(IPAddress.Any, port);

                // 클라이언트 연결 대기
                server.Start();
                Console.WriteLine("서버가 시작되었습니다. 클라이언트 연결 대기 중...");

                while (true)
                {
                    // 클라이언트 연결 수락
                    TcpClient AcceptedClient = server.AcceptTcpClient();

                    // 새로운 클라이언트가 연결되면 쓰레드 생성 및 시작
                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(AcceptedClient);
                }

                // 클라이언트 연결 수락
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("클라이언트가 연결되었습니다.");


                void HandleClient(object clientObj)
                {
                    TcpClient client = (TcpClient)clientObj;
                    Console.WriteLine("클라이언트 연결됨: " + client.Client.RemoteEndPoint);

                    // 클라이언트와의 데이터 통신 처리
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // 수신한 데이터 처리
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        var jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
                        Console.WriteLine(jsonObject);
                        if (jsonObject.Route == "Login")
                        {
                            LoginHandler();
                        }
                        else if (jsonObject.Route == "Register")
                        {
                            RegisterHandler();
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
                    Console.WriteLine("클라이언트 연결 종료: " + client.Client.RemoteEndPoint);
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
        public static void LoginHandler()
        {
            Console.WriteLine("Login Handler!");
        }

        // Register packet
        public static void RegisterHandler()
        {
            Console.WriteLine("Register Handler!");
        }


    }
}