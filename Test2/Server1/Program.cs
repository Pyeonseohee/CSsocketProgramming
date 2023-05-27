using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server1
{
    internal class Program
    {
        static void Main()
        {
            // 서버의 IP 주소와 포트 번호
            string ipAddress = "52.206.228.119";
            int port = 50000;

            try
            {
                // 서버 소켓 생성
                TcpListener server = new TcpListener(IPAddress.Parse(ipAddress), port);

                // 클라이언트 연결 대기
                server.Start();
                Console.WriteLine("서버가 시작되었습니다. 클라이언트 연결 대기 중...");

                // 클라이언트 연결 수락
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("클라이언트가 연결되었습니다.");

                // 클라이언트와 데이터 교환
                NetworkStream stream = client.GetStream();

                //// 클라이언트로부터 데이터 수신
                //byte[] buffer = new byte[1024];
                //int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //Console.WriteLine("클라이언트로부터 데이터를 수신했습니다: " + dataReceived);

                //// 클라이언트에 응답 전송
                //string response = "서버에서 클라이언트로 응답합니다.";
                //byte[] responseData = Encoding.ASCII.GetBytes(response);
                //stream.Write(responseData, 0, responseData.Length);
                //Console.WriteLine("클라이언트에 응답을 전송했습니다.");

                // 연결 종료
                client.Close();
                server.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine("오류: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}