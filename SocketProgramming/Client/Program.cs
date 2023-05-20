
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("client console");
            //클라이언트쪽 연결시도하는 코드
            Client client = new Client();
            client.Connect();
        }

        public class Client
        {
            Socket clientSocket;
            int serverPort = 20000;
            public void Connect()
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverAddr = IPAddress.Parse("127.0.0.1");
                IPEndPoint serverEP = new IPEndPoint(serverAddr, serverPort);

                clientSocket.Connect(serverEP);

                byte[] data = Encoding.UTF8.GetBytes("I'm Client!");

                clientSocket.Send(data);
                Console.WriteLine("send data success!");
                //clientSocket.BeginConnect(clientEP, new AsyncCallback(ConnectCallback), clientSocket);
            }
            public void Close()
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                    clientSocket.Dispose();
                }
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
            void ConnectCallback(IAsyncResult ar)
            {
                    try
                    {
                        Socket client = (Socket)ar.AsyncState;
                        client.EndConnect(ar);
                        AsyncObject obj = new AsyncObject(4096);
                        obj.WorkingSocket = clientSocket;
                        clientSocket.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
                    }
                    catch (Exception e)
                    {
                    }
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
                clientSocket.Send(msg);
            }
        }
    }
}