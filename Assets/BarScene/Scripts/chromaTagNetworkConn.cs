using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.IO;

public class chromaTagNetworkConn : MonoBehaviour {
	public string connectionIP = "127.0.0.1";
	public int connectionPort = 9499;

	private Vector3 position;
	private Quaternion rotation;

	private Vector3 initialPosition;
	private Quaternion initialRotation;

	internal Boolean socketReady = false;
	private NetworkStream stream;
	private StreamReader r;
	private TcpClient socket;
	bool isRunning = true;


	void Start() {
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		position = new Vector3 (0, 0, 0);
		rotation = new Quaternion ();

		// start background thread
		Thread trackingThread = new Thread(new ThreadStart(tracking));
		trackingThread.Start ();
	}

	void Update() {
		transform.position = initialPosition + position;
		transform.rotation = initialRotation * rotation;
	}

	public void setupConn(String host, Int32 port) {
		try {
			Debug.Log ("Attempting to connect to " + host + ":" + port);
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			r = new StreamReader(stream);
			socketReady = true;
		}
		catch (Exception e) {
			Debug.Log("Socket error: " + e);
		}
	}

	void tracking() {
		while (isRunning) {
			if (!socketReady) {
				setupConn (connectionIP, connectionPort);
				continue;
			}
			string line = r.ReadLine();
			if (line == null) {
				isRunning = false;
				break;
			}
			var values = line.Trim (new char[]{'[',']'}).Split(',');
			double[] doubles = Array.ConvertAll(values, new Converter<string, double>(Double.Parse));
			float[] d = Array.ConvertAll<double, float>(doubles, x => (float)x);
			for (int i = 0; i < 12; i++) {
				if (Double.IsNaN(doubles[i])) {
					Debug.Log (values[i]);
				}
			}
			Matrix4x4 m = new Matrix4x4();
			m.SetRow(0, new Vector4(d[0],d[1],d[2],d[3]));
			m.SetRow(1, new Vector4(d[4],d[5],d[6],d[7]));
			m.SetRow(2, new Vector4(d[8],d[9],d[10],d[11]));
			m.SetRow(3, new Vector4(0,0,0,0));
			position = m.GetColumn(3);
			rotation = Quaternion.LookRotation(
				m.GetColumn(2),
				m.GetColumn(1)
			);
			//Debug.Log(rotation[0]);
		}
	}

}
