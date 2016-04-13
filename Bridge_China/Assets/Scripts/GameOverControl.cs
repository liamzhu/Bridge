using UnityEngine;
using System.Collections;
using System;
using cn.sharesdk.unity3d;

public class GameOverControl : MonoBehaviour {
    public UILabel curScoreLabel;
    public UILabel bestScoreLabel;

    public BridgePanelControl bridgeControl;

    static int loseNum = 0;

	private ShareSDK ssdk;
	string shareStr = "";

    void OnEnable() {
        curScoreLabel.text = bridgeControl.curScoreLabel.text;
        bestScoreLabel.text = bridgeControl.bestScoreLabel.text;

        loseNum++;
        if (loseNum == 2) {
			DateTime dt1 = DateTime.Parse("2016-4-14");
			//if(System.DateTime.Now > new System.DateTime()
			if(DateTime.Compare(DateTime.Now, dt1) > 0){
				SendMessageUpwards("ShowAD");
			}
			loseNum = 0;
        }

		TweenScale tweenScale = TweenScale.Begin<TweenScale>(gameObject, 0.2f);
		tweenScale.from = new Vector3 (0.2f,0.2f,1.0f);
		tweenScale.to = Vector3.one;
    }

	// Use this for initialization
	void Start () {
		ssdk = gameObject.GetComponent<ShareSDK>();
		ssdk.authHandler = AuthResultHandler;
		ssdk.shareHandler = ShareResultHandler;
		ssdk.showUserHandler = GetUserInfoResultHandler;
		ssdk.getFriendsHandler = GetFriendsResultHandler;
		ssdk.followFriendHandler = FollowFriendResultHandler;

		shareStr = "我当前的最好成绩是 "+bridgeControl.bestScoreLabel.text +"。 不服来战！";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	Texture2D screenShot;
	public void CaptureScreenShot(){
		float offsetX = 0;
		float offsetY = 0;
		int width = Screen.width;
		int height = Screen.height;
		
		Rect rect = new Rect (offsetX, offsetY, width, height);
		Camera camera = Camera.main;
		//Texture2D tex = Util.CaptureCamera(Camera.main,curRec);
		
		// 创建一个RenderTexture对象
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
		camera.targetTexture = rt;
		camera.Render();
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
		//ps: camera2.targetTexture = rt;
		//ps: camera2.Render();
		//ps: -------------------------------------------------------------------
		
		// 激活这个rt, 并从中中读取像素。
		RenderTexture.active = rt;
		screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);
		screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
		screenShot.Apply();
		
		// 重置相关参数，以使用camera继续在屏幕上显示
		camera.targetTexture = null;
		//ps: camera2.targetTexture = null;
		RenderTexture.active = null; // JC: added to avoid errors
		GameObject.Destroy(rt);
		
//		image.mainTexture = screenShot;//对屏幕缓存进行显示（缩略图）
//		
//		//置屏幕时的宽高
//		int ManualWidth = 750;		
//		int ManualHeight = 1334;
//		int lastWidth = 0;
//		int lastHeight = 0;
//		
//		if (System.Convert.ToSingle (Screen.height) / Screen.width > System.Convert.ToSingle (ManualHeight) / ManualWidth) {
//			lastHeight = Mathf.RoundToInt (System.Convert.ToSingle (ManualWidth) / Screen.width * Screen.height);
//			lastWidth = Mathf.RoundToInt (System.Convert.ToSingle (ManualWidth) / Screen.width * Screen.width);
//		}
//		else {
//			lastHeight = ManualHeight;
//			//lastWidth = Mathf.RoundToInt(ManualWidth * (System.Convert.ToSingle (Screen.height) / Screen.width));
//			lastWidth = Mathf.RoundToInt (System.Convert.ToSingle (ManualHeight) / Screen.height * Screen.width);
//		}
//		//image.MakePixelPerfect ();
//		image.width = Mathf.RoundToInt (lastWidth * 0.9f);
//		image.height = Mathf.RoundToInt (lastHeight * 0.9f);
//		image.gameObject.SetActive (true);
	}
	
	//新浪微博
	public void ShareWeiBo(){
		if (screenShot == null) {
			CaptureScreenShot ();
		}

		//拷贝图片
		string imagePath = Application.persistentDataPath + "/TheDreamLandShot.png";
		// 最后将这些纹理数据，成一个png图片文件
		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(imagePath, bytes);
		
		Hashtable content = new Hashtable();
		content["content"] = shareStr;		//可以修改
		content["image"] = imagePath;
		content["title"] = "天天过桥";//标题
		content["description"] = "test description";
		content["url"] = "http://android.myapp.com/myapp/detail.htm?apkName=com.OneGame.Bridge"; //ios
		content["type"] = ContentType.Image;
		content["siteUrl"] = "http://sharesdk.cn";
		content["site"] = "";
		content["musicUrl"] = "";
		
		ssdk.ShowShareView (PlatformType.SinaWeibo, content);
		//GameAudioYinXiao.sharedInstance.Play (GameAudioYinXiao.sharedInstance.Music_300);
	}
	
	//微信
	public void ShareWeChat(){
		if (screenShot == null) {
			CaptureScreenShot ();
		}
		
		//拷贝图片
		string imagePath = Application.persistentDataPath + "/TheDreamLandShot.png";
		// 最后将这些纹理数据，成一个png图片文件
		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(imagePath, bytes);
		
		Hashtable content = new Hashtable();
		content["content"] = shareStr;		//可以修改
		content["image"] = imagePath;
		content["title"] = "天天过桥";//标题
		content["description"] = shareStr;
		content["url"] = "http://android.myapp.com/myapp/detail.htm?apkName=com.OneGame.Bridge"; //ios
		content["type"] = ContentType.Webpage;
		content["siteUrl"] = "http://sharesdk.cn";
		content["site"] = "";
		content["musicUrl"] = "";
		
		ssdk.ShowShareView (PlatformType.WeChat, content);
	}
	
	//微信朋友圈
	public void ShareWeChatMoments(){
		if (screenShot == null) {
			CaptureScreenShot ();
		}

		//拷贝图片
		string imagePath = Application.persistentDataPath + "/TheDreamLandShot.png";
		// 最后将这些纹理数据，成一个png图片文件
		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(imagePath, bytes);
		
		Hashtable content = new Hashtable();
		content["content"] = shareStr;		//可以修改
		content["image"] = imagePath;
		content["title"] = "天天过桥";//标题
		content["description"] = shareStr;
		content["url"] = "http://android.myapp.com/myapp/detail.htm?apkName=com.OneGame.Bridge"; //安卓
		content["type"] = ContentType.Webpage;
		content["siteUrl"] = "http://sharesdk.cn";
		content["site"] = "";
		content["musicUrl"] = "";
		
		ssdk.ShowShareView (PlatformType.WeChatMoments, content);
	}
	
//	//faceBook
//	public void ShareFaceBook(){
//		//拷贝图片
//		string imagePath = Application.persistentDataPath + "/TheDreamLandShot.png";
//		// 最后将这些纹理数据，成一个png图片文件
//		byte[] bytes = screenShot.EncodeToPNG();
//		System.IO.File.WriteAllBytes(imagePath, bytes);
//		
//		Hashtable content = new Hashtable();
//		content["content"] = "https://itunes.apple.com/us/app/the-dreamland/id1078869865?mt=8";
//		content["image"] = imagePath;
//		content["title"] = "The dreamland-lost stars";//标题
//		content["description"] = "descriptiondescriptiondescriptiondescriptiondescription";
//		content ["url"] = "https://itunes.apple.com/us/app/the-dreamland/id1078869865?mt=8"; //ios
//		content["type"] = ContentType.Image;
//		content["siteUrl"] = null;
//		content["site"] = null;
//		content["musicUrl"] = null;
//		
//		ssdk.ShowShareView (PlatformType.Twitter, content);
//		GameAudioYinXiao.sharedInstance.Play (GameAudioYinXiao.sharedInstance.Music_300);
//	}
	
	void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			//NGUIDebug.Log ("authorize success !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			//NGUIDebug.Log ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			//NGUIDebug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			//NGUIDebug.Log ("=============void AuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)");
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			//NGUIDebug.Log ("cancel !");
		}
	}
	
	void GetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			//NGUIDebug.Log ("get user info result :");
			//NGUIDebug.Log (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			//NGUIDebug.Log ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			//NGUIDebug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			//NGUIDebug.Log ("=============void GetUserInfoResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)");
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			//NGUIDebug.Log ("cancel !");
		}
	}
	
	void ShareResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			//NGUIDebug.Log ("share successfully - share result :");
			//NGUIDebug.Log (MiniJSON.jsonEncode(result));
			//NGUIDebug.Log ("=============分享成功！");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			//NGUIDebug.Log ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			//NGUIDebug.Log ("=============分享失败1111！");
			//NGUIDebug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			if(result["error_code"].ToString()=="208"){
				//NGUIDebug.Log ("=============没有安装微信的客户端啊！");
				if(!noInstallLabel.gameObject.activeSelf){
					noInstallLabel.gameObject.SetActive (true);
					
					TweenAlpha tweenAlpha = TweenAlpha.Begin<TweenAlpha>(noInstallLabel.gameObject,2.0f);
					tweenAlpha.from = 1.0f;
					tweenAlpha.to = 0.3f;
					
					StartCoroutine(WaitToHideTipLabel(1.0f));
				}
			}
			//NGUIDebug.Log ("=============分享失败222！");
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			//NGUIDebug.Log ("cancel !");
			//NGUIDebug.Log ("=============分享取消！");
		}
	}
	
	IEnumerator WaitToHideTipLabel(float time){
		yield return new WaitForSeconds (time);
		//noInstallLabel.gameObject.SetActive (false);
	}
	
	void GetFriendsResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{			
			//NGUIDebug.Log ("get friend list result :");
			//NGUIDebug.Log (MiniJSON.jsonEncode(result));
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			//NGUIDebug.Log ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			//NGUIDebug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			//NGUIDebug.Log ("cancel !");
		}
	}
	
	void FollowFriendResultHandler (int reqID, ResponseState state, PlatformType type, Hashtable result)
	{
		if (state == ResponseState.Success)
		{
			//NGUIDebug.Log ("Follow friend successfully !");
		}
		else if (state == ResponseState.Fail)
		{
			#if UNITY_ANDROID
			//NGUIDebug.Log ("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
			#elif UNITY_IPHONE
			//NGUIDebug.Log ("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
			#endif
		}
		else if (state == ResponseState.Cancel) 
		{
			//NGUIDebug.Log ("cancel !");
		}
	}
}
