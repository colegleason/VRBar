using UnityEngine;
using System.Collections;

public class chromaTagNetworkConn : MonoBehaviour {
	public string connectionIP = "127.0.0.1";
	public int connectionPort = 9499;
	public s_TCP conn;
	public MugController mug;
	private float MoveDir = 0.01f;
	void attemptConnection()  {
		conn.setupSocket (connectionIP, connectionPort);
	}

	void Start () {
		conn = new s_TCP ();
	}

	// Update is called once per frame
	void Update () {
		if (!conn.socketReady) {
			attemptConnection ();
		} else {
			string s = conn.readSocket ();
			if (s != "") {
				Debug.Log (s);
			}
		}
		transform.Translate (transform.forward * 1);
		transform.Rotate (transform.up, 1);
	}
}
