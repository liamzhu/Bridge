using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgePanelControl : MonoBehaviour {
    static string BESTSCOREKEY = "BridgeBestScore";
	static string GOTAPPLENUM = "GotAppleNum";

	public static bool isMoveFast = false;

    public GameObject templateBottomItem;
    public GameObject objParent;
    public PressControl pressControl;
    public CharacterControl objHuman;

    public UILabel curScoreLabel;           //当前成绩
    public UILabel bestScoreLabel;          //最好成绩
	public UILabel appleNumLabel;			//获得的苹果数,用来解锁小动物
	public UILabel tipToJump;

    int curScore = 0;
    int bestScore = 0;
	int appleNum = 0;

    public GameObject objGameOverPanel;
    public GameObject objScorePanel;

    int[] LenArray = {90,60,60,60,70,80,100};
    int[] DisArray = {150,200,250,280,300,350,400,420};

    int desDis = 0;
    int desWidth = 0;
    List<BottomItem> objList = new List<BottomItem>();

    float leftPosX = -270.0f;
    float humanInitPosX = -230.0f;

    //-320 180
	// Use this for initialization
	void Start () {
		InitGame ();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

	void InitGame(){
		for (int i = 0; i < objList.Count; i++) {
			Destroy(objList[i].gameObject);
		}
		objList.Clear ();

		curScore = 0;
		objHuman.gameObject.transform.localPosition = new Vector3(-270.0f,0,0);

		CreateItem(100, leftPosX);
		pressControl.bridgeObj = objList[0].birdgeSprite;
		
		//设固定值
		desDis = 200;
		desWidth = 70;
		CreateItem(desWidth, leftPosX+desDis);

		objGameOverPanel.SetActive(false);
		objScorePanel.SetActive(true);
		
		//设置当前最好成绩
		curScoreLabel.text = "0";
		bestScore = PlayerPrefs.GetInt(BESTSCOREKEY, 0);
		bestScoreLabel.text = bestScore.ToString();

		appleNum = PlayerPrefs.GetInt (GOTAPPLENUM,0);
		appleNumLabel.text = appleNum.ToString ();

		tipToJump.gameObject.SetActive (false);

		GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_bg);
		pressControl.InitData();
	}

	public void HumanJump(){
		objHuman.CharacterJump ();
	}

    public void HumanRun(int bridgeLen) {
		//摔死
        if (bridgeLen < desDis - (objList[0].width/2 + objList[1].width/2)||bridgeLen - 1 > desDis - objList[0].width/2 + desWidth/2){
			objHuman.CharacterRun(bridgeLen+objList[0].width/2,true,false);
            //设置最好成绩
            if (curScore > bestScore) {
                PlayerPrefs.SetInt(BESTSCOREKEY, curScore);
            }

			objList[1].enermyControl.gameObject.SetActive(false);
        }
        else
        {   //走到中间 
			objHuman.CharacterRun(desDis,false,false);

//			if(objList[1].enermyControl.gameObject.activeSelf){ //向左移动dis
//				objList[1].enermyControl.CharacterRun(desDis,false,true);
//				tipToJump.gameObject.SetActive(true);
//			}
        }  
    }
	
    public void WaitToDie(float waitTime) {
        //开始旋转
        InvokeRepeating("BridgeRotate", 0.1f, 0.01f);
    }

    void BridgeRotate() {
        pressControl.bridgeObj.gameObject.transform.Rotate(new Vector3(0, 0, -1));
        objHuman.transform.localPosition += new Vector3(0,-10,0);

        GameAudio.sharedInstance.Stop(GameAudio.sharedInstance.Music_bg);
        //GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_help);
        //Debug.Log(pressControl.bridgeObj.gameObject.transform.localEulerAngles.z);
        if (pressControl.bridgeObj.gameObject.transform.localEulerAngles.z <= 230.0f){
            CancelInvoke("BridgeRotate");
			GameOver();
        }
    }

	public void GameOver(){
		//弹出结束框
		objGameOverPanel.SetActive(true);
		objScorePanel.SetActive(false);
		GameAudio.sharedInstance.Play(GameAudio.sharedInstance.Music_gameOver);
		tipToJump.gameObject.SetActive(false);
	}

    public void WaitToMoveRightFinish(float waitTime){
		tipToJump.gameObject.SetActive(false);
		objHuman.Rotate ();

        desWidth = LenArray[Mathf.FloorToInt(UnityEngine.Random.Range(0, LenArray.Length))];
        desDis = DisArray[Mathf.FloorToInt(UnityEngine.Random.Range(0, DisArray.Length))];

        CreateItem(desWidth, objList[1].transform.localPosition.x+ desDis);//此时List长度为3
        Destroy(objList[0].gameObject);
        objList.RemoveAt(0);

        //人和list里的两个物体，向左移动
        float moveLeftLen = objHuman.transform.localPosition.x - humanInitPosX;
		TweenPosition.Begin(objHuman.gameObject, moveLeftLen*0.002f, new Vector3(objHuman.transform.localPosition.x - moveLeftLen, objHuman.transform.localPosition.y, 0));
		TweenPosition.Begin(objList[0].gameObject, moveLeftLen*0.002f, 
		                    new Vector3(objList[0].gameObject.transform.localPosition.x - moveLeftLen, objList[0].gameObject.transform.localPosition.y, 0));
		TweenPosition.Begin(objList[1].gameObject, moveLeftLen*0.002f, 
		                    new Vector3(objList[1].gameObject.transform.localPosition.x - moveLeftLen, objList[1].gameObject.transform.localPosition.y, 0));
		StartCoroutine(WaitToMoveLeftFinish(moveLeftLen*0.002f));
		isMoveFast = true;


		curScore++;
		curScoreLabel.text = curScore.ToString();
		
		//设置最好成绩
		if (curScore > bestScore){
			bestScore = curScore;
			bestScoreLabel.text = curScore.ToString();
			PlayerPrefs.SetInt(BESTSCOREKEY, curScore);
		}
    }

    IEnumerator WaitToMoveLeftFinish(float waitTime){
        yield return new WaitForSeconds(waitTime);
        pressControl.InitData();
        pressControl.bridgeObj = objList[0].birdgeSprite;
		isMoveFast = false;
    }

    GameObject CreateItem(int width,float posX) {
        GameObject tempObj = Instantiate(templateBottomItem) as GameObject;
        tempObj.SetActive(true);
        tempObj.transform.parent = objParent.transform;
        tempObj.transform.localScale = Vector3.one;
        //tempObj.transform.localPosition = Vector3.zero;
        //tempObj.GetComponent<UISprite>().width = width;
        tempObj.transform.localPosition = new Vector3(posX,-420.0f,0.0f);

        BottomItem bottomItem = tempObj.GetComponent<BottomItem>();
        bottomItem.SetBridge(width);
        objList.Add(bottomItem);

//		//是否带苹果
//		if (curScore >= 1) {
//			if(Random.Range(1,11) >= 5){
//				//生成水果
//				//bottomItem.fruitControl.gameObject.SetActive(true);
//				if(desDis >= 200){
//					bottomItem.enermyControl.gameObject.SetActive(true);
//				}
//			}
//		}
        return tempObj;
    }

    public void ClickOnTryAgain() {
		InitGame ();
    }

	public void ChangeFruitNum(){
		appleNum++;
		appleNumLabel.text = appleNum.ToString();
		PlayerPrefs.SetInt(GOTAPPLENUM, appleNum);
	}

	public void ClickOnCamera(){
		Debug.Log ("CLICK ON CAMERA************************");
	}
}
