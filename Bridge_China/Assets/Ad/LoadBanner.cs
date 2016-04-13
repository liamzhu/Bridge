using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

public class LoadBanner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DateTime dt1 = DateTime.Parse("2016-4-24");
		//if(System.DateTime.Now > new System.DateTime()
		if(DateTime.Compare(DateTime.Now, dt1) > 0){
			//SendMessageUpwards("ShowAD");
			StartCoroutine (WaitToLoadAd(0.1f));
		}
		//StartCoroutine (WaitToLoadAd(0.1f));
	}

	IEnumerator WaitToLoadAd(float time){
		yield return new WaitForSeconds (time);
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RequestBanner()
	{
		#if UNITY_EDITOR
		string adUnitId = "unused";
		#elif UNITY_ANDROID
		string adUnitId = "ca-app-pub-5388514788898470/9891282343";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		
		//		AdRequest request = new AdRequest.Builder()
		//			.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
		//			.AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")  // My test iPod Touch 5.
		//			.Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
}
