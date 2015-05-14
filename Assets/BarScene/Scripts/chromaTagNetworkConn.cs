using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.IO;

public class chromaTagNetworkConn : MonoBehaviour {
	public string connectionIP = "127.0.0.1";
	public int connectionPort = 9499;
	public float speed = 0.1f;
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
		rotation = Quaternion.identity;

		// start background thread
		Thread trackingThread = new Thread(new ThreadStart(tracking));
		trackingThread.Start ();
	}

	void Update() {
		transform.position = Vector3.MoveTowards (
			transform.position,
			initialPosition + position / 100,
			speed*Time.deltaTime
			);

	if (!Double.IsNaN (rotation.x * rotation.y * rotation.z * rotation.w)) {
			transform.rotation = Quaternion.RotateTowards(
				transform.rotation,
				initialRotation * rotation,
				50f*Time.deltaTime
				);
		}
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

//	public Quaternion MatToQuaternion( Matrix4x4 a ) {
//		Quaternion q = new Quaternion();
//		float trace = a[0][0] + a[1][1] + a[2][2]; // I removed + 1.0f; see discussion with Ethan
//		if( trace > 0 ) {// I changed M_EPSILON to 0
//			float s = 0.5f / Math.Sqrt(trace+ 1.0f);
//			q.w = 0.25f / s;
//			q.x = ( a[2][1] - a[1][2] ) * s;
//			q.y = ( a[0][2] - a[2][0] ) * s;
//			q.z = ( a[1][0] - a[0][1] ) * s;
//		} else {
//			if ( a[0][0] > a[1][1] && a[0][0] > a[2][2] ) {
//				q.w = (a[2][1] - a[1][2] ) / s;
//				q.x = 0.25f * s;
//				q.y = (a[0][1] + a[1][0] ) / s;
//				q.z = (a[0][2] + a[2][0] ) / s;
//			} else if (a[1][1] > a[2][2]) {
//				float s = 2.0f * sqrtf( 1.0f + a[1][1] - a[0][0] - a[2][2]);
//				q.w = (a[0][2] - a[2][0] ) / s;
//				q.x = (a[0][1] + a[1][0] ) / s;
//				q.y = 0.25f * s;
//				q.z = (a[1][2] + a[2][1] ) / s;
//			} else {
//				float s = 2.0f * sqrtf( 1.0f + a[2][2] - a[0][0] - a[1][1] );
//				q.w = (a[1][0] - a[0][1] ) / s;
//				q.x = (a[0][2] + a[2][0] ) / s;
//				q.y = (a[1][2] + a[2][1] ) / s;
//				q.z = 0.25f * s;
//			}
//		}
//		return q;
//	}
	
	void tracking() {
		while (isRunning) {
			if (!socketReady) {
				Thread.Sleep (300);
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
			if (!Double.IsNaN (d[0]*d[1]*d[4]*d[5]*d[8]*d[9])) {
				rotation = Quaternion.LookRotation(
					m.GetColumn(2),
					m.GetColumn(1)
					);
			}
		}
	}

}
