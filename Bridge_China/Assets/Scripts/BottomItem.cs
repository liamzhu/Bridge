using UnityEngine;
using System.Collections;

public class BottomItem : MonoBehaviour {

    public int width;
    public UISprite birdgeSprite;
	public GameObject offsetObj;//偏移物体

	public FruitsControl fruitControl;
	public CharacterControl enermyControl;

	// Use this for initialization
	void Start () {
		//fruitControl.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetBridge(int width){
		//设置杠子位置
        birdgeSprite.gameObject.transform.localPosition = new Vector3(width/2.0f, 150, 0);
        birdgeSprite.height = 0;
        this.width = width;

		offsetObj.transform.localScale = new Vector3 (width/100.0f,1.0f,1.0f);
    }
}
