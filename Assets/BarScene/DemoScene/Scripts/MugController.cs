using UnityEngine;
using System.Collections;

public class MugController : MonoBehaviour {
	public float MoveSpeed = 5f;

	public void Move(float hInput, float vInput) {
		transform.Translate (transform.forward * vInput);
		transform.Rotate (transform.up, 1);
	}
}
