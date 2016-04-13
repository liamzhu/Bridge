using UnityEngine;
using System.Collections;

public class AdjustToScreen : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		AdaptiveUI ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.frameCount % 2 == 0) {
			AdaptiveUI ();		
		}
	}

    /// <summary>
    /// 屏幕自适应 Adaptives the U.
    /// </summary>
    public static void AdaptiveUI()
    {
        //置屏幕时的宽高
        int ManualWidth = 640;
        int ManualHeight = 1136;
        UIRoot uiRoot = GameObject.FindObjectOfType<UIRoot>();
        if (uiRoot != null)
        {
            if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
                uiRoot.manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
            else
                uiRoot.manualHeight = ManualHeight;
        }
    }
}
