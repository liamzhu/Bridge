using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class LoadAd : MonoBehaviour {
	public InterstitialAd interstitial;
	
	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		RequestInterstitial ();
	}

	// Update is called on		ce per frame
	void Update () {

	}
	
	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-5388514788898470/2368015548";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void ShowAD(){
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }  
	}

//	void OnGUI()  
//	{  
//		//开始按钮  
//		if (GUI.Button (new Rect (0, 100, 200, 100), "myAd")) {  
////			//RequestInterstitial();
//if (interstitial.IsLoaded()) {
//	interstitial.Show();
//}  
//
//		}
//	}
}
