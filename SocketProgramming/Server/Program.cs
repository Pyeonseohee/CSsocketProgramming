using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("server console");
            byte[] msg = new byte[8];

            //서버쪽 대기 시작하는코드
            Server server = new Server();
            server.Start();
            server.Close();
        }

        public class Server
        {
            Socket serverSocket;
            List<Socket> connectedClients = new List<Socket>();
            int serverPort = 20000;
            public void Start()
            {
                try
                {
                    Console.WriteLine("start....");
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
                    serverSocket.Bind(serverEP); // 서버소켓에 ip, port 할당
                    Console.WriteLine("success bind!!!!!!");
                    serverSocket.Listen(10); // 클라이언트의 연결 요청을 대기 상태로 만듦. 백로그 큐 = 클라이언트들의 연결 요처 대기실
                    
                    Socket clientSocket = serverSocket.Accept();
                    Console.WriteLine("Connection success!");

                    byte[] buffer = new byte[1024];

                    while (!Console.KeyAvailable)
                    {
                        // client에게 받음
                        int n = clientSocket.Receive(buffer);
                        string data = Encoding.UTF8.GetString(buffer, 0, n);

                        Console.WriteLine(data);
                        clientSocket.Send(buffer, 0, n, SocketFlags.None); // echo
                        clientSocket.Close();
                    }
                    //serverSocket.BeginAccept(AcceptCallback, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }

            public void Close()
            {
                if (serverSocket != null)
                {
                    serverSocket.Close();
                    serverSocket.Dispose();
                }

                foreach (Socket socket in connectedClients)
                {
                    socket.Close();
                    socket.Dispose();
                }
                connectedClients.Clear();

                // 새로운 소켓 생성
                //serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }

            public class AsyncObject
            {
                public byte[] Buffer;
                public Socket WorkingSocket;
                public readonly int BufferSize;
                public AsyncObject(int bufferSize)
                {
                    BufferSize = bufferSize;
                    Buffer = new byte[(long)BufferSize];
                }

                public void ClearBuffer()
                {
                    Array.Clear(Buffer, 0, BufferSize);
                }
            }

            void AcceptCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = serverSocket.EndAccept(ar);
                    AsyncObject obj = new AsyncObject(1920 * 1080 * 3);
                    obj.WorkingSocket = client;
                    connectedClients.Add(client);
                    client.BeginReceive(obj.Buffer, 0, 1920 * 1080 * 3, 0, DataReceived, obj);

                    serverSocket.BeginAccept(AcceptCallback, null);
                }
                catch (Exception e)
                { }
            }

            void DataReceived(IAsyncResult ar)
            {
                AsyncObject obj = (AsyncObject)ar.AsyncState;

                int received = obj.WorkingSocket.EndReceive(ar);

                byte[] buffer = new byte[received];

                Array.Copy(obj.Buffer, 0, buffer, 0, received);
            }

            public void Send(byte[] msg)
            {
                serverSocket.Send(msg);
            }

        }

    }
}