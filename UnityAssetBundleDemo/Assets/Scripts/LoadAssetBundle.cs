using UnityEngine;
using System.Collections;
using AssetBundles;

public class LoadAssetBundle : MonoBehaviour {
//	static string assetBundleUrl = "http://172.16.129.32/UnityAssetBundle/UnityAnimationDemo/";
	static string assetBundleUrl = "http://192.168.0.100/UnityAssetBundle/UnityAnimationDemo/";
	string assetBundleName = "";
	string sceneName = "";
	string assetName = "";
//	private string[] activeVariants;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.Label (new Rect (10, 10, 300, 30), "Load AssetBundle");
		assetBundleUrl = GUI.TextField (new Rect (10, 50, 1200, 30), assetBundleUrl);
		if (GUI.Button(new Rect(10, 100, 300, 80), "Cube Animation From Scene")) {
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "cubescene";
			sceneName = "CubeScene";
			StartCoroutine (StartLoadAssetBundleScene ());
		}
		if (GUI.Button(new Rect(10, 200, 300, 80), "Sphere Animation From Scene")) {
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "animation/sphere";
			sceneName = "SphereScene";
			StartCoroutine (StartLoadAssetBundleScene ());
		}
		if (GUI.Button(new Rect(10, 300, 300, 80), "Sphere Animation From Prefab")) {
			AssetBundleManager.UnloadAssetBundle (assetBundleName);
			assetBundleName = "sphereprefab";
			assetName = "SpherePrefab.prefab";
			StartCoroutine (StartLoadAssetBundleAsset ());
		}
	}

	IEnumerator StartLoadAssetBundleScene() {
		yield return StartCoroutine (DownloadAssetBundle ());
//		AssetBundleManager.ActiveVariants = activeVariants;
		yield return StartCoroutine (InitializeLevelAsync (true));
	}

	IEnumerator StartLoadAssetBundleAsset() {
		yield return StartCoroutine (DownloadAssetBundle ());
//		AssetBundleManager.ActiveVariants = activeVariants;
		yield return StartCoroutine (InitializeAssetAsync ());
	}

	protected IEnumerator DownloadAssetBundle() {
		DontDestroyOnLoad (gameObject);
//		AssetBundleManager.SetSourceAssetBundleURL("file://" + Application.streamingAssetsPath + "/");
//		AssetBundleManager.SetSourceAssetBundleURL( Application.streamingAssetsPath + "/");
		AssetBundleManager.SetSourceAssetBundleURL (assetBundleUrl);
		var request = AssetBundleManager.Initialize ();
		if (request != null) {
			yield return StartCoroutine (request);
		}
	}

	protected IEnumerator InitializeLevelAsync(bool isAdditive) {
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync (assetBundleName, sceneName, isAdditive);
		if (request == null) {
			yield break;
		}
		yield return StartCoroutine (request);

		float elapsedTime = Time.realtimeSinceStartup - startTime;
    }

	protected IEnumerator InitializeAssetAsync() {
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync (assetBundleName, assetName, typeof(GameObject));
		if (request == null) {
			yield break;
		}
		yield return StartCoroutine (request);
		GameObject prefab = request.GetAsset<GameObject> ();
		if (prefab != null) {
			GameObject.Instantiate (prefab);
		}

		float elapsedTime = Time.realtimeSinceStartup - startTime;
	}

	public void sharedFunction(string argument) {
		Debug.Log ("[Han TEST IN UNITY] sharedFunction: " + argument);
	}
}
