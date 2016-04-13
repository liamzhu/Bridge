using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 工具类
/// </summary>
public static class Utility 
{
    static Color setColorAndAlpha(Color origin, float alpha) {
        origin = new Color(origin.r, origin.g, origin.b, alpha);
        return origin;
    }

    //set alpha of colo
    //if current shader of the material doesn't support alpha channerl, change its shader, and save origin to dict for later use
    public static void setTransparent(GameObject obj, float alpha, Shader newShader, ref Dictionary<Renderer, Shader> shaderDict) {
        if (!obj) { return; }
		
        if (shaderDict == null) shaderDict = new Dictionary<Renderer, Shader>();

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer) {
            if (renderer.material.HasProperty("_Color")) {
                //need process

                //save old shader
                if (shaderDict.ContainsKey(renderer)) shaderDict.Remove(renderer);
                shaderDict.Add(renderer, renderer.material.shader);

                //replace old shader with newer
                renderer.material.shader = newShader;

                //and set alpha of color
                Color origin = renderer.material.color;
                renderer.material.color = setColorAndAlpha(origin, alpha);
            }
        }

        //we may also need to process children of the obj
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) { return; }
        foreach (Renderer r in renderers) {
            //todo:redundancy code

            if (r.material.HasProperty("_Color")) {
                //need process

                //save old shader
                if (shaderDict.ContainsKey(r)) shaderDict.Remove(r);
                shaderDict.Add(r, r.material.shader);

                //replace old shader with newer
                r.material.shader = newShader;

                //and set alpha of color
                Color origin = r.material.color;
                r.material.color = setColorAndAlpha(origin, alpha);
            }
        }
    }

    //revert alpha of the given object
    public static void revertMaterial(GameObject obj, float alpha, Dictionary<Renderer, Shader> shaderDict) {
        if (!obj) { return; }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer) {
            if (shaderDict.ContainsKey(renderer)) {
                renderer.material.shader = shaderDict[renderer];
                if (renderer.material.HasProperty("_Color")) {
                    Color origin = renderer.material.color;
                    renderer.material.color = setColorAndAlpha(origin, alpha);
                }
            }
        }

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) { return; }
        foreach (Renderer r in renderers) {
            //todo:redundancy code
            if (shaderDict.ContainsKey(r)) {
                r.material.shader = shaderDict[r];
                if (r.material.HasProperty("_Color")) {
                    Color origin = r.material.color;
                    r.material.color = setColorAndAlpha(origin, alpha);
                }
            }
        }
    }

    public static void setObjectColor(GameObject obj, Color color) {
        if (!obj) { return; }

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer) {
            if (renderer.material.HasProperty("_Color")) {
                renderer.material.color = setColorAndAlpha(color, 1);
            }
        }

        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0) { return; }
        foreach (Renderer r in renderers) {
            if (r.material.HasProperty("_Color")) {
                r.material.color = setColorAndAlpha(color, 1);
            }
        }
    }
	
	public static Color getObjectColor(GameObject obj)
	{
		Color tempColor = new Color(0,0,0,0);

		if (!obj) { return tempColor; }

        Renderer renderer = obj.GetComponentInChildren<Renderer>();
        if (renderer) 
		{
            if (renderer.material.HasProperty("_Color")) 
			{
                tempColor.a = renderer.material.color.a;
				tempColor.r = renderer.material.color.r;
				tempColor.g = renderer.material.color.g;
				tempColor.b = renderer.material.color.b;
            }
        }

		return tempColor;
    }
	
    public static Vector3 worldScale(Transform transform) {
        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null) {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }

        return worldScale;
    }

    //assign object to given layer, includes children
    public static void setLayerRecursively(GameObject obj, int layer) {
        foreach (Transform trans in obj.GetComponentsInChildren<Transform>(true)) {
            trans.gameObject.layer = layer;
        }
    }

    //center, r are in world space
    public static List<GameObject> objectsInRange(Vector3 center, float r, LayerMask layer) {
        if (r <= 0) return null;

        List<GameObject> objs = new List<GameObject>();
        Collider[] cols=Physics.OverlapSphere(center, r);
        foreach (Collider col in cols){
            if (col.gameObject.layer != layer) 
                continue;

            GameObject obj = col.gameObject;
            objs.Add(obj);

            //show
            setObjectColor(obj, Color.red);
        }
        return objs;
    }

    public static bool isObjectInRange(GameObject srcObj, GameObject distObj, float dist) {
        return Vector3.SqrMagnitude(srcObj.transform.position - distObj.transform.position) <= Mathf.Pow(dist, 2);
    }

    public static float localToWorldLength(GameObject obj, float length) { 
        //in order to get the length in world space, we need to times obj's lossyScale
        return length * Vector3.Magnitude(obj.transform.lossyScale);
    }

    public static Vector3 objPosInWorld(GameObject obj) {
        if (!obj) return Vector3.zero;

        //get position of obj, plus the collider postion, so we get the collider position in world space
        CharacterController ctrl = obj.GetComponent<CharacterController>();
        Vector3 offset = Vector3.zero;
        if (ctrl != null) {
            offset = Vector3.Scale(ctrl.center, obj.transform.lossyScale);
        }

        Vector3 pos = obj.transform.position + offset;
        return pos;
    }
	
	//get max local scale of the obj,includes all children
	public static Vector2 maxLocScale(GameObject obj){
		Vector2 scale=obj.transform.localScale;
		
		foreach(Transform trans in obj.transform){
			if(scale.x<trans.localScale.x)scale.x=trans.localScale.x;
			if(scale.y<trans.localScale.y)scale.y=trans.localScale.y;
		}
		
		return scale;
	}
	
	public static void shuffle<T>(ref List<T> src){
		if(src==null||src.Count<=0)return;
		
		int count=src.Count;
		for(int i=0;i<count;++i){
			T tmp=src[i];
			int randomIdx=UnityEngine.Random.Range(0,count-1);
			src[i]=src[randomIdx];
			src[randomIdx]=tmp;
		}
	}
	
	public static void shuffle<T>(ref T[] src){
		if(src==null)return;
		
		int count=src.Length;
		for(int i=0;i<count;++i){
			T tmp=src[i];
			int randomIdx=UnityEngine.Random.Range(0,count);
			src[i]=src[randomIdx];
			src[randomIdx]=tmp;
		}
	}
	
	public static void shuffle<T>(ref List<T[]> src){
		if(src==null)return;
		
		int count=src.Count;
		for(int i=0;i<count;++i){
			T[] tmp=src[i];
			
			shuffle(ref tmp);
			
			int randomIdx=UnityEngine.Random.Range(0,count);
			src[i]=src[randomIdx];
			src[randomIdx]=tmp;
		}
	}

    public static IEnumerator swapObject(GameObject objA, GameObject objB, float speed, float minDist, Action onStart, Action onFinished)
    {
        if (objA.Equals(objB)) yield return null;

        if (onStart != null) onStart();

        Vector3 posA = objA.transform.position;
        Vector3 posB = objB.transform.position;

        bool m = true;

        while (m)
        {
            //float dis = Vector3.Distance(firstObject.transform.position, secondPos);
            float s = speed * Time.deltaTime;
            //s /= dis;

            //just change x axis of position
            Vector3 aToB = new Vector3(posB.x, objA.transform.position.y, objA.transform.position.z);
            objA.transform.position = Vector3.MoveTowards(objA.transform.position, aToB, s);
            //objA.transform.position = Vector3.Lerp(objA.transform.position, targetPos, s);

            Vector3 bToA = new Vector3(posA.x, objB.transform.position.y, objB.transform.position.z);
            objB.transform.position = Vector3.MoveTowards(objB.transform.position, bToA, s);
            //objB.transform.position = Vector3.Lerp(objB.transform.position, targetPos, s);

            if (Vector3.SqrMagnitude(objA.transform.position - aToB) <= Mathf.Pow(minDist, 2) &&
            Vector3.SqrMagnitude(objB.transform.position - bToA) <= Mathf.Pow(minDist, 2))
            {
                m = false;

                //call back
                if (onFinished != null) onFinished();
            }
            yield return null;
        }
    }

    //spawn num points between start and end
    public static float[] randomValuesInRange(float start, float end, int num) {
        float[] vals = new float[num];

        float step = (end - start) / num;
        for(int i = 0; i < num; ++i) {
            float s = start + i * step;
            float e = s + step;
            float val = UnityEngine.Random.Range(s, e);
            vals[i] = val;
        }

        return vals;
    }

    //divide to several parts, add up to a total of sum
    //better shuffled
    public static int[] randomValuesToSum(int sum, int num) {
        int[] vals = new int[num];

        int tmpSum = 0;
        for(int i = 0; i < num; ++i) {
            int val = 0;
            if(i == num - 1) {
                val = sum - tmpSum;
                vals[i] = val;
                break;
            }

            //don't wanna 0,so we start from 1
            val = Mathf.FloorToInt(UnityEngine.Random.Range(1, sum - tmpSum));
            vals[i] = val;
            tmpSum += val;
        }

        shuffle(ref vals);

        return vals;
    }
}
