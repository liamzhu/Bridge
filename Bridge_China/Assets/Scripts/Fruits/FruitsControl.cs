using UnityEngine;
using System.Collections;

public class FruitsControl : MonoBehaviour {
	public BridgePanelControl bridgeControl;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Destroy(gameObject);
		bridgeControl.ChangeFruitNum ();
		GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_coin);
	}
	
//	void OnCollisionEnter(Collision other) {
//		Debug.Log (other.gameObject.name);
//	}
}
