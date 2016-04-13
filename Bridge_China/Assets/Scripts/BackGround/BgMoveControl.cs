using UnityEngine;
using System.Collections;

public class BgMoveControl : MonoBehaviour {

	public float speed;

	public float leftPos;
	public float rightPos;

	float moveFast = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (BridgePanelControl.isMoveFast) {
			moveFast = 15.0f;
		} 
		else {
			moveFast = 1.0f;
		}
		gameObject.transform.Translate (new Vector3 (Time.smoothDeltaTime * speed*0.02f*moveFast, 0, 0));
		if (gameObject.transform.localPosition.x <= leftPos) {
			gameObject.transform.localPosition = new Vector3(rightPos,0,0);
		}
	}
}
