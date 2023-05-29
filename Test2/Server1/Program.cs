using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.Collections.Specialized;
using System.Text.Json.Nodes;

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
                Console.WriteLine("server start... waiting Client");

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
                Console.WriteLine("Connection to client!");


                void HandleClient(object clientObj)
                {
                    TcpClient client = (TcpClient)clientObj;

                    // 클라이언트와의 데이터 통신 처리
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // 수신한 데이터 처리
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

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

        public static void SQLConnect(dynamic jsonData)
        {
            // RDS 서버에 접속
            string StringToConnection = "Server=nowmsm-db.cirkkpu5fv9s.us-east-1.rds.amazonaws.com;Database=nowMSM;Uid=admin;Pwd=00000000;";
            using (MySqlConnection conn = new MySqlConnection(StringToConnection))
            {
                Console.Write("success connection!");
                try
                {
                    conn.Open();
                    string InsertQuery = $"insert into user(id, name, pw) values('{jsonData.ID}', '{jsonData.NAME}', '{jsonData.PWD}')";
                    Console.Write("SQL insert start!");

                    // command connection
                    MySqlCommand cmd = new MySqlCommand(InsertQuery, conn);

                    // 만약에 내가처리한 Mysql에 정상적으로 들어갔다면 메세지를 보여주라는 뜻
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.Write("Insert success!");
                    }
                    else
                    {
                        Console.Write("Insert error!");
                    }
                }catch(Exception e)
                {
                    Console.Write(e.ToString());
                }
            }
        }
        // Login packet
        public static void LoginHandler(dynamic jsonData)
        {
            Console.WriteLine("Login Handler!");
            Console.WriteLine(jsonData);
        }

        // Register packet
        public static void RegisterHandler(dynamic jsonData)
        {
            Console.WriteLine("Register Handler!");
            Console.WriteLine(jsonData);
            SQLConnect(jsonData);
        }


    }
}