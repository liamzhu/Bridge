using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

	float runSpeed = 2.0f;
	float[] jumpArray = {0,10,18,25,30,34,38,42,46,50,54,58,62,66,70,74,78,80,82,84,86,88,90,92,94,96,98,100,98,90,80,70,60,50,40,30,20,10,0};
	int jumpIndex = 0;
	int maxRunTimes = 0;
	int curRunTimes = 0;

	bool xMoveFinish = true;		//x方向移动是否结束了
	bool yMoveFinish = true;
	public bool isMoveStart = false;

	public Animator curAnimator;

	public BridgePanelControl bridgeControl;
	// Use this for initialization
	void Start () {
		InitData ();
	}

	void InitData(){
		jumpIndex = 0;
		maxRunTimes = 0;
		curRunTimes = 0;
		
		xMoveFinish = true;		//x方向移动是否结束了
		yMoveFinish = true;
		isMoveStart = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (xMoveFinish && yMoveFinish && isMoveStart) {
			isMoveStart = false;
			if(isDie){
				bridgeControl.WaitToDie(0.0f);
			}
			else{
				bridgeControl.WaitToMoveRightFinish(0.0f);
			}
		}
	}

	public void Rotate(){
		TweenRotation tweenRotate = TweenRotation.Begin<TweenRotation> (gameObject, 0.5f);
		tweenRotate.from = Vector3.zero;
		tweenRotate.to = new Vector3 (0,360,0);
		tweenRotate.delay = 0.5f;
		tweenRotate.eventReceiver = gameObject;
		tweenRotate.callWhenFinished = "RotateFinish";
	}

	void RotateFinish(){
		gameObject.transform.localEulerAngles = Vector3.zero;
	}

	bool isDie = false;
	bool isEnermy = false;
	public void CharacterRun(float dis,bool isDead,bool isEnermy){
		isDie = isDead;
		this.isEnermy = isEnermy;
		maxRunTimes = (int)(dis / runSpeed);
		InvokeRepeating("ChangePosX", 0.2f, 0.01f);
		curAnimator.SetBool ("isIdle", false);
		curAnimator.SetBool ("isWalk", true);
	}

	public void CharacterJump(){
		if (xMoveFinish && isDie) {
			return;
		}
		InvokeRepeating("ChangePosY", 0.1f, 0.01f);
	}

	void ChangePosX(){
		isMoveStart = true;
		xMoveFinish = false;
		curRunTimes++;
		if (isEnermy) {
			gameObject.transform.localPosition = 
				new Vector3 (gameObject.transform.localPosition.x + runSpeed/4, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
		}
		else {
			gameObject.transform.localPosition = 
				new Vector3 (gameObject.transform.localPosition.x + runSpeed, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
		}

		if (curRunTimes > maxRunTimes) {
			curRunTimes = 0;
			CancelInvoke("ChangePosX");
			xMoveFinish = true;

			curAnimator.SetBool ("isIdle",true);
			curAnimator.SetBool ("isWalk",false);

			if(isEnermy){
				gameObject.SetActive(false);
			}
		}
	}

	void ChangePosY(){
		yMoveFinish = false;
		jumpIndex++;
		gameObject.transform.localPosition = 
			new Vector3 (gameObject.transform.localPosition.x,jumpArray[jumpIndex],gameObject.transform.localPosition.z);

//		if (jumpIndex == 1) {
//			TweenRotation tweenRotate = TweenRotation.Begin<TweenRotation>(gameObject, 0.2f);
//			tweenRotate.from = Vector3.zero;
//			tweenRotate.to = new Vector3(0,0,-360.0f);
//		}

		if (jumpIndex >= jumpArray.Length - 1) {
			jumpIndex = 0;
			CancelInvoke("ChangePosY");
			yMoveFinish = true;
		}
	}

	public void GameFail(){
		CancelInvoke("ChangePosX");
		CancelInvoke("ChangePosY");
		curAnimator.SetBool ("isIdle",true);
		curAnimator.SetBool ("isWalk",false);
		InitData ();
	}

	void OnTriggerEnter(Collider other) {
		//GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_coin);
		Debug.Log ("############### === "+other.name);
		if (isEnermy) {
			bridgeControl.GameOver();
			gameObject.SetActive(false);
			//other.gameObject.SetActive(false);
			other.gameObject.GetComponent<CharacterControl>().GameFail();
		}
	}
}
