using UnityEngine;

public class Spin : MonoBehaviour {
	public float spinRate;

	public void Update() {
		transform.Rotate (transform.up, spinRate * Time.deltaTime);
	}
}