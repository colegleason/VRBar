using UnityEngine;
using System;
using System.Collections;

public class chromaTagNetworkConn : MonoBehaviour {
	public string connectionIP = "172.17.98.206";
	public int connectionPort = 9499;
	public s_TCP conn;
	void attemptConnection()  {
		conn.setupSocket (connectionIP, connectionPort);
	}

	void Start () {
		conn = new s_TCP ();
		attemptConnection ();
	}
	static byte[] GetBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}
	// Update is called once per frame
	void Update () {
		if (!conn.socketReady) {
			attemptConnection ();
		} else {
			string s = conn.readSocket ();
			if (s != "") {
			Debug.Log(s);
			}
		}
	}
}
