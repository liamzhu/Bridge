using UnityEngine;
using System.Collections;

public class PressControl : MonoBehaviour {
    public BridgePanelControl bridgeControl;
    public UISprite bridgeObj;

    bool isAddLength = true;
    bool isLengthChangeOver = false;
    bool isRotateChangeOver = false;

	bool isPressDown = false;
	int pressTimeCount = 0;
	bool isLongPress = false;
	// Use this for initialization
	void Start () {

	}

    public void InitData() {
        isAddLength = true;
        isLengthChangeOver = false;
        isRotateChangeOver = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(Time.frameCount % 5 == 0){
			if (isPressDown) {
				pressTimeCount++;
				//Debug.Log("*************** == "+pressTimeCount.ToString());
				if(pressTimeCount >= 5){
					BeginChange();
					isPressDown = false;
					pressTimeCount = 0;
					isLongPress = true;
				}
			}
		}
	}

	void BeginChange(){
		GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_ganZi);
		bridgeObj.gameObject.SetActive(true);
		InvokeRepeating("BridgeChange", 0.01f, 0.01f);	
	}

	void OnPress(bool isDown){
		if (bridgeControl.objGameOverPanel.activeSelf) {
			return;
		}

		if (isDown) {	//按下
            if (isLengthChangeOver) {
                return;
            }
			isPressDown = true;
            //bridgeObj.gameObject.SetActive(true);
            //InvokeRepeating("BridgeChange", 0.5f, 0.01f);	
		} 
		else {		
			isPressDown = false;
			pressTimeCount = 0;

			if(bridgeControl.objHuman.isMoveStart){
				//bridgeControl.HumanJump();
			}
		
			if(!isLongPress){
				return;
			}
			isLongPress = false;

			GameAudio.sharedInstance.Stop(GameAudio.sharedInstance.Music_ganZi);
			//弹起
            CancelInvoke("BridgeChange");
            isLengthChangeOver = true;

            if (isRotateChangeOver){
                return;
            }
            
            //开始旋转
            InvokeRepeating("BridgeRotate", 0.1f, 0.01f);
		}
	}

    void BridgeChange() {
        if (isAddLength){
            bridgeObj.height += 3;
        }
        else {
            bridgeObj.height -= 3;
        }

        if (bridgeObj.height > 500) {
            isAddLength = false;
        }

        if (bridgeObj.height <= 5) {
            isAddLength = true;
        }
    }

    void BridgeRotate() {
        bridgeObj.gameObject.transform.Rotate(new Vector3(0, 0, -2));
        if (bridgeObj.gameObject.transform.localEulerAngles.z <= 270.0f){
            isRotateChangeOver = true;
            CancelInvoke("BridgeRotate");
            bridgeControl.HumanRun(bridgeObj.height);
        }
    }	
}
