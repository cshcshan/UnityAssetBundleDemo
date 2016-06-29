using UnityEngine;
using System.Collections;
using AssetBundles;

public class LoadAssetBundle : MonoBehaviour {
	static string[] SEPERATED_STRING = { "===SKYFAMILY===" };

	// Use this for initialization
	void Start () {
//		handleAssetBundleAsset ("http://172.16.129.32/UnityAssetBundle/AssetBundleTests/" + SEPERATED_STRING[0] + "mybundle" + SEPERATED_STRING[0] + "logo");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void sharedFunction(string argument) {
		Debug.Log ("[Han TEST IN UNITY] sharedFunction: " + argument);
	}

	public void handleAssetBundleLevel(string assetBundleInfo) {
		string[] parameters = assetBundleInfo.Split (SEPERATED_STRING, System.StringSplitOptions.RemoveEmptyEntries);
		string assetBundleUrl = "";
		string assetBundleName = "";
		string sceneName = "";
		for (int i = 0; i < parameters.Length; i++) {
			switch (i) {
			case 0:
				assetBundleUrl = parameters [i];
				break;
			case 1:
				assetBundleName = parameters [i];
				break;
			case 2:
				sceneName = parameters [i];
				break;
			default:
				break;
			}
		}
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleInfo: " + assetBundleInfo);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleUrl: " + assetBundleUrl);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleName: " + assetBundleName);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - sceneName: " + sceneName);
		StartCoroutine (StartLoadAssetBundleScene (assetBundleUrl, assetBundleName, sceneName, true));
	}

	public void handleAssetBundleAsset(string assetBundleInfo) {
		string[] parameters = assetBundleInfo.Split (SEPERATED_STRING, System.StringSplitOptions.RemoveEmptyEntries);
		string assetBundleUrl = "";
		string assetBundleName = "";
		string assetName = "";
		for (int i = 0; i < parameters.Length; i++) {
			switch (i) {
			case 0:
				assetBundleUrl = parameters [i];
				break;
			case 1:
				assetBundleName = parameters [i];
				break;
			case 2:
				assetName = parameters [i];
				break;
			default:
				break;
			}
		}
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleInfo: " + assetBundleInfo);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleUrl: " + assetBundleUrl);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetBundleName: " + assetBundleName);
		Debug.Log ("[Han TEST IN UNITY] HandleAssetBundleLevel - assetName: " + assetName);
		StartCoroutine (StartLoadAssetBundleAsset (assetBundleUrl, assetBundleName, assetName));
	}

	// Start Load AssetBundle
	protected IEnumerator StartLoadAssetBundleScene(string assetBundleUrl, string assetBundleName, string sceneName, bool isAdditive) {
		yield return StartCoroutine (InitializeAssetBundle (assetBundleUrl));
		yield return StartCoroutine (LoadScene (assetBundleName, sceneName, true));
	}

	protected IEnumerator StartLoadAssetBundleAsset(string assetBundleUrl, string assetBundleName, string assetName) {
		yield return StartCoroutine (InitializeAssetBundle (assetBundleUrl));
		yield return StartCoroutine (LoadAsset (assetBundleName, assetName));
	}

	// Initialize AssetBundle
	protected IEnumerator InitializeAssetBundle(string assetBundleUrl) {
		DontDestroyOnLoad (gameObject);
//		AssetBundleManager.SetSourceAssetBundleURL("file://" + Application.streamingAssetsPath + "/");
//		AssetBundleManager.SetSourceAssetBundleURL( Application.streamingAssetsPath + "/");
		AssetBundleManager.SetSourceAssetBundleURL (assetBundleUrl);
		var request = AssetBundleManager.Initialize ();
		if (request != null) {
			yield return StartCoroutine (request);
		}
	}

	// Initialize AssetBundle
	protected IEnumerator LoadScene(string assetBundleName, string sceneName, bool isAdditive) {
		float startTime = Time.realtimeSinceStartup;

		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync (assetBundleName, sceneName, isAdditive);
		if (request == null) {
			yield break;
		}
		yield return StartCoroutine (request);

		float elapsedTime = Time.realtimeSinceStartup - startTime;
		Debug.Log ("[Han TEST IN UNITY] Finished loading scene " + sceneName + " in " + elapsedTime + " seconds.");
    }

	protected IEnumerator LoadAsset(string assetBundleName, string assetName) {
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
		Debug.Log ("[Han TEST IN UNITY] Finished loading asset " + assetName + " in " + elapsedTime + " seconds.");
	}
}
