﻿using UnityEngine;
using System.Collections;
using AssetBundles;

public class LoadAssetBundle : MonoBehaviour {
//	static string assetBundleUrl = "http://172.16.129.32/UnityAnimationDemo/";
	static string assetBundleUrl = "http://192.168.0.101/UnityAssetBundle/UnityAnimationDemo/";
	string assetBundleName = "";
	string sceneName = "";
	private string[] activeVariants;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Label (new Rect (10, 10, 300, 30), "Load AssetBundle");
		assetBundleUrl = GUI.TextField (new Rect (10, 50, 300, 30), assetBundleUrl);
		if (GUI.Button(new Rect(10, 100, 300, 80), "Cube Animation")) {
			Debug.Log ("Url: " + assetBundleUrl);
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "animation/cube";
			sceneName = "CubeScene";
			StartCoroutine (StartLoadAssetBundle ());
		}
		if (GUI.Button(new Rect(10, 200, 300, 80), "Sphere Animation")) {
			Debug.Log ("Url: " + assetBundleUrl);
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "animation/sphere";
			sceneName = "SphereScene";
			StartCoroutine (StartLoadAssetBundle ());
		}
	}

	IEnumerator StartLoadAssetBundle() {
		Debug.Log ("StartLoadAssetBundle: Before StartCoroutine (DownloadAssetBundle ())");
		yield return StartCoroutine (DownloadAssetBundle ());
		AssetBundleManager.ActiveVariants = activeVariants;
		Debug.Log ("StartLoadAssetBundle: Before StartCoroutine (InitializeLevelAsync (sceneName, true)");
		yield return StartCoroutine (InitializeLevelAsync (sceneName, true));
		Debug.Log ("StartLoadAssetBundle: End");
	}

	protected IEnumerator DownloadAssetBundle() {
		Debug.Log ("DownloadAssetBundle: Before DontDestroyOnLoad (gameObject)");
		DontDestroyOnLoad (gameObject);
		Debug.Log ("DownloadAssetBundle: Before AssetBundleManager.SetSourceAssetBundleURL (assetBundleUrl)");
		AssetBundleManager.SetSourceAssetBundleURL (assetBundleUrl);
		Debug.Log ("DownloadAssetBundle: Before AssetBundleManager.Initialize ()");
		var request = AssetBundleManager.Initialize ();
		if (request != null) {
			Debug.Log ("DownloadAssetBundle: Before StartCoroutine (request)");
			yield return StartCoroutine (request);
		}
		Debug.Log ("DownloadAssetBundle: End");
	}

	protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive) {
		Debug.Log ("InitializeLevelAsync: Start");
		float startTime = Time.realtimeSinceStartup;

		Debug.Log ("InitializeLevelAsync: Before AssetBundleManager.LoadLevelAsync (assetBundleName, levelName, isAdditive)");

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync (assetBundleName, levelName, isAdditive);
		if (request == null) {
			Debug.Log ("InitializeLevelAsync: request is null");
			yield break;
		}
		Debug.Log ("InitializeLevelAsync: Before StartCoroutine (request)");
		yield return StartCoroutine (request);

		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log ("Finished loading scene " + levelName + " in " + elapsedTime + " seconds.");
    }
}
