using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;

// http://answers.unity3d.com/questions/15422/unity-project-and-3rd-party-apps.html#answer-15477
public class s_TCP {
	internal Boolean socketReady = false;
	
	TcpClient socket;
	NetworkStream stream;
	StreamWriter w;
	StreamReader r;

	public void setupSocket(String host, Int32 port) {
		try {
			Debug.Log ("Attempting to connect to " + host + ":" + port);
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			stream.ReadTimeout = 1;
			w = new StreamWriter(stream);
			r = new StreamReader(stream);
			socketReady = true;
		}
		catch (Exception e) {
			Debug.Log("Socket error: " + e);
		}
	}
	public void writeSocket(string theLine) {
		if (!socketReady)
			return;
		String foo = theLine + "\r\n";
		w.Write(foo);
		w.Flush();
	}
	public String readSocket() {
		if (!socketReady)
			return "";
		try {
			return r.ReadLine();
		} catch (Exception e) {
			return "";
		}
	}
	public void closeSocket() {
		if (!socketReady)
			return;
		w.Close();
		r.Close();
		socket.Close();
		socketReady = false;
	}
} // end class s_TCP
