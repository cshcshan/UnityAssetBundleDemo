using UnityEngine;
using System.Collections;
using AssetBundles;

public class LoadAssetBundle : MonoBehaviour {
	static string assetBundleUrl = "http://172.16.129.32/UnityAnimationDemo/";
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
		if (GUI.Button(new Rect(10, 10, 200, 80), "Cube Animation")) {
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "animation/cube";
			sceneName = "CubeScene";
			StartCoroutine (StartLoadAssetBundle ());
		}
		if (GUI.Button(new Rect(10, 100, 200, 80), "Sphere Animation")) {
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "animation/sphere";
			sceneName = "SphereScene";
			StartCoroutine (StartLoadAssetBundle ());
		}
	}

	IEnumerator StartLoadAssetBundle() {
		if (assetBundleName != "") {
		}
		yield return StartCoroutine (DownloadAssetBundle ());
		AssetBundleManager.ActiveVariants = activeVariants;
		yield return StartCoroutine (InitializeLevelAsync (sceneName, true));
	}

	protected IEnumerator DownloadAssetBundle() {
		DontDestroyOnLoad (gameObject);
		AssetBundleManager.SetSourceAssetBundleURL (assetBundleUrl);
		var request = AssetBundleManager.Initialize ();
		if (request != null) {
			yield return StartCoroutine (request);
		}
	}

	protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive) {
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync (assetBundleName, levelName, isAdditive);
		if (request == null) {
			yield break;
		}
		yield return StartCoroutine (request);

		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log ("Finished loading scene " + levelName + " in " + elapsedTime + " seconds.");
    }
}
