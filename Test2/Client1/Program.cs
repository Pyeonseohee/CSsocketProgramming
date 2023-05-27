using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client1
{
    internal class Program
    {
        static void Main()
        {
            // EC2 인스턴스의 IP 주소와 포트 번호
            string ipAddress = "52.206.228.119";
            int port = 50000;
            Console.WriteLine("시작합니다!");
            try
            {
                // 서버에 연결
                TcpClient client = new TcpClient(ipAddress, port);
                Console.WriteLine("서버에 연결되었습니다.");

                // 서버와 데이터 교환
                NetworkStream stream = client.GetStream();

                // 데이터를 전송할 버퍼
                byte[] buffer = Encoding.ASCII.GetBytes("안녕하세요, EC2 서버!");

                // 데이터 전송
                stream.Write(buffer, 0, buffer.Length);
                Console.WriteLine("데이터를 서버로 전송했습니다.");

                //// 서버로부터 응답을 받음
                //buffer = new byte[1024];
                //int bytesRead = stream.Read(buffer, 0, buffer.Length);
                //string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //Console.WriteLine("서버 응답: " + response);

                // 연결 종료
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("오류: " + e.Message);
            }

            Console.ReadLine();
        }
        }
}