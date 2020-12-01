using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;


public class NetworkConnection : MonoBehaviour {
    Thread mThread;
    public string host = "192.168.100.156"; //enter ip of raspberry pi which is in the same network
    public int port = 2121;
    TcpClient client;
    NetworkStream s;
    Vector3 pos, oldPos = Vector3.zero;
    public float speed = 1;
    public float divBy = 1;


    public float aButton;
    public float bButton;
    public float xButton;
    public float yButton;

    public float joystick1X;
    public float joystick1Y;

    public float joystick2X;
    public float joystick2Y;

    void Start() {
        /*try { 
            client = new TcpClient(host, port);
        } catch { }
        if (IsConnected(client)) {
            s = client.GetStream();
        }
        InvokeRepeating("pinger", 0f, 5f);*/
        ThreadStart ts = new ThreadStart(Client);
        mThread = new Thread(ts);
        mThread.Start(); 
    }

    void Client() {
        //while (!IsConnected(client)) { }
        try {
            client = new TcpClient(host, port);
            s = client.GetStream();
            byte[] byteBuffer = Encoding.UTF8.GetBytes("Connected to client");
            s.Write(byteBuffer, 0, byteBuffer.Length);
            while (true) {
                byte[] buffer = new byte[client.ReceiveBufferSize];

                int bytesRead = s.Read(buffer, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log(dataReceived);
                //Debug.Log(dataReceived);
                //pos = StringToVector3(dataReceived);
                // Debug.Log(pos);
                ParseInput(dataReceived);

            }
            s.Close();
        } 
        finally {
            client.Close();
        }
    }

    void Update() {
        //transform.position = Vector3.Lerp(pos, transform.position, Time.deltaTime * speed);

        transform.position = new Vector3(transform.position.x + joystick1X, transform.position.y + joystick1Y, transform.position.z);

    }

    public Vector3 StringToVector3(string sVector) {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")")) {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }


    public void ParseInput(string sInput)
    {
        // Remove the parentheses
        //if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        //{
        //    sVector = sVector.Substring(1, sVector.Length - 2);
        //}

        // split the items
        string[] sArray = sInput.Split(',');

        // store as a Vector3

        aButton = float.Parse(sArray[0]);
        bButton = float.Parse(sArray[1]);
        xButton = float.Parse(sArray[2]);
        yButton = float.Parse(sArray[3]);

        joystick1X = float.Parse(sArray[4]);
        joystick1Y = float.Parse(sArray[5]);

        joystick2X = float.Parse(sArray[6]);
        joystick2Y = float.Parse(sArray[7]);


    }


    void pinger() {
        if (!IsConnected(client)) { 
            client = new TcpClient(host, port);
            s = client.GetStream();
        }
        byte[] ping = Encoding.UTF8.GetBytes("ping");
        s.Write(ping, 0, ping.Length);
        Debug.Log("ping");
    }

    public bool IsConnected(TcpClient _tcpClient) {
        try {
            if (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected) {
                /* pear to the documentation on Poll:
                    * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                    * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                    * -or- true if data is available for reading; 
                    * -or- true if the connection has been closed, reset, or terminated; 
                    * otherwise, returns false
                    */

        // Detect if client disconnected
        if (_tcpClient.Client.Poll(0, SelectMode.SelectRead)) {
                    byte[] buff = new byte[1];
                    if (_tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0) {
                        // Client disconnected
                        return false;
                    } else {
                        return true;
                    }
                }

                return true;
            } else {
                return false;
            }
        } catch {
            return false;
        }
    }
}