﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class SocketClient
{
    // Cached socket. Exists through lifetime of class
    Socket _socket = null;

    static ManualResetEvent _clientDone = new ManualResetEvent(false);
    const int TIMEOUT = 5000;

    const int MAX_BUFFER_SIZE = 524;


    public string Connect(string hostName, int portNumber)
    {
        string result = string.Empty;

        DnsEndPoint hostEntry = new DnsEndPoint(hostName, portNumber);

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        SocketAsyncEventArgs socketEventArgs = new SocketAsyncEventArgs();

        socketEventArgs.RemoteEndPoint = hostEntry;

        socketEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
        {
            result = e.SocketError.ToString();

            _clientDone.Set();
        });

        _clientDone.Reset();


        _socket.ConnectAsync(socketEventArgs);

        _clientDone.WaitOne(TIMEOUT);

        return result;
    }

    public string Send(string data)
    {

        string response = "Operation Timeout";

        if (_socket != null)
        {
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

            socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
            socketEventArg.UserToken = null;

            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                response = e.SocketError.ToString();

                _clientDone.Set();
            });


            byte[] payload = Encoding.UTF8.GetBytes(data);
            socketEventArg.SetBuffer(payload, 0, payload.Length);

            _clientDone.Reset();

            _socket.SendAsync(socketEventArg);

            _clientDone.WaitOne(TIMEOUT);
        }
        else
        {
            response = "Socket is not initialized";
        }
        return response;
    }

    /// <summary>
    /// Receive data from the server using the established socket connection
    /// </summary>
    /// <returns>The data received from the server</returns>
    public string Receive()
    {
        string response = "Operation Timeout";

        // We are receiving over an established socket connection
        if (_socket != null)
        {
            // Create SocketAsyncEventArgs context object
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;

            // Setup the buffer to receive the data
            socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

            // Inline event handler for the Completed event.
            // Note: This even handler was implemented inline in order to make 
            // this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                if (e.SocketError == SocketError.Success)
                {
                    // Retrieve the data from the buffer
                    response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                    response = response.Trim('\0');
                }
                else
                {
                    response = e.SocketError.ToString();
                }

                _clientDone.Set();
            });

            // Sets the state of the event to nonsignaled, causing threads to block
            _clientDone.Reset();

            // Make an asynchronous Receive request over the socket
            _socket.ReceiveAsync(socketEventArg);

            // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
            // If no response comes back within this time then proceed
            _clientDone.WaitOne(TIMEOUT);
        }
        else
        {
            response = "Socket is not initialized";
        }

        return response;
    }

    /// <summary>
    /// Closes the Socket connection and releases all associated resources
    /// </summary>
    public void Close()
    {
        if (_socket != null)
        {
            _socket.Close();
        }
    }



}