using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPListener : MonoBehaviour
{
    private Thread udpListeningThread;
    private UdpClient udpClient;
    private bool isListening = true;
    public int listenPort = 50000;

    public float CMSignal = 0;

    void Start()
    {
        udpListeningThread = new Thread(new ThreadStart(ListenForMessages))
        {
            IsBackground = true
        };
        udpListeningThread.Start();
    }

    private void ListenForMessages()
    {
        udpClient = new UdpClient(listenPort);
        udpClient.EnableBroadcast = true;

        while (isListening)
        {
            print("Listening for messages");
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);

                // Convert bytes to text
                string receivedText = Encoding.ASCII.GetString(receivedBytes);

                if (receivedText.StartsWith("CMSignal")){
                    CMSignal = float.Parse(receivedText.Split(' ')[1]);
                    print("Received CMSignal: " + CMSignal);
                }else{
                    print("Received: " + receivedText + " from " + remoteEndPoint);
                }
                // Use Unity's main thread to log the message or update the UI
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    void OnDisable()
    {
        isListening = false;
        udpClient.Close();
        udpListeningThread.Join();
        Debug.Log("UDP Listener stopped.");
    }
}
