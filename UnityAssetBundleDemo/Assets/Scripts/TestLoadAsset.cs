using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssetBundles;

public class TestLoadAsset : MonoBehaviour {

	public Text myText;
//	static string host = "http://172.16.129.32/UnityAssetBundle/";
//	static string host = "http://172.20.10.4/UnityAssetBundle/";
	static string host = "http://192.168.0.100/UnityAssetBundle/";
	string assetBundleUrl = host + "AssetBundleTests/";

	private string assetBundleName = "mybundle";
	static string LAST_HASH = "LAST_HASH";


	void OnGUI() {
		assetBundleUrl = GUI.TextField (new Rect (350, 50, 1200, 30), assetBundleUrl);
		if (GUI.Button(new Rect(350, 100, 300, 80), "Load Assets")) {
			StartCoroutine(xxx());
		}
		if (GUI.Button(new Rect(350, 200, 300, 80), "Load Animation Scene")) {
			StartCoroutine(yyy());
		}
		if (GUI.Button(new Rect(350, 300, 300, 80), "Load Animation Assets")) {
			// animation/sphereprefab
			StartCoroutine(zzz());
		}
	}

	IEnumerator xxx() {
		assetBundleUrl = host + "AssetBundleTests/";
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine(LoadMyAssets());
	}

	IEnumerator yyy() {
		assetBundleUrl = host + "UnityAnimationDemo/";
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine (LoadMyScene ());
	}

	IEnumerator zzz() {
		assetBundleUrl = host + "UnityAnimationDemo/";
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine(LoadMyAnimationAsset());
	}


	// Use this for initialization
	IEnumerator Start ()
	{
		//in case you need to wipe out the cache
//		Caching.CleanCache ();

		/*
		var lastHash = PlayerPrefs.GetString (LAST_HASH);

		if (lastHash != string.Empty && Caching.IsVersionCached (assetBundleName, Hash128.Parse (lastHash))) {

			WWW www = WWW.LoadFromCacheOrDownload (assetBundleName, Hash128.Parse (lastHash), 0);

			// Wait for download to complete
			yield return www;

			// Load and retrieve the AssetBundle
			AssetBundle bundle = www.assetBundle;

			StartCoroutine(LoadAssetsFromBundle (bundle));

			www.Dispose();


		} else {

			yield return StartCoroutine(Initialize() );

			yield return StartCoroutine(LoadMyAssets () );

		}
		*/

		yield break;
	}

	IEnumerator Initialize()
	{
		DontDestroyOnLoad(gameObject);

		#if UNITY_EDITOR 
		AssetBundleManager.SetSourceAssetBundleURL("file://" + Application.streamingAssetsPath + "/");
		#else
		AssetBundleManager.SetSourceAssetBundleURL( Application.streamingAssetsPath + "/");
		#endif

		//Or Load from a server!
//		AssetBundleManager.SetSourceAssetBundleURL("http://dl.dropboxusercontent.com/u/YOUR_DROP_BOX/");
		AssetBundleManager.SetSourceAssetBundleURL(assetBundleUrl);

		// Initialize AssetBundleManifest which loads the AssetBundleManifest object.
		var request = AssetBundleManager.Initialize();
		if (request != null)
			yield return StartCoroutine(request);
	}

	IEnumerator LoadMyAssets ()
	{
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, "logo", typeof(GameObject) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();

		if (prefab != null)
			GameObject.Instantiate(prefab);

//		request = AssetBundleManager.LoadAssetAsync(assetBundleName, "data.bytes", typeof(TextAsset) );
//		if (request == null)
//			yield break;
//		yield return StartCoroutine(request);

		// Get the asset.
		TextAsset txt = request.GetAsset<TextAsset> ();

//		if (txt != null)
//			myText.text = txt.text;

//		PlayerPrefs.SetString (LAST_HASH, AssetBundleManager.AssetBundleManifestObject.GetAssetBundleHash (assetBundleName).ToString());
	}


//	IEnumerator LoadAssetsFromBundle (AssetBundle bundle) {
//
//		AssetBundleRequest request = bundle.LoadAssetAsync ("logo", typeof(GameObject));
//
//		yield return request;
//
//		GameObject obj = request.asset as GameObject;
//		GameObject clone = GameObject.Instantiate(obj);
//
//		request = bundle.LoadAssetAsync ("data", typeof(TextAsset));
//		yield return request;
//
//		TextAsset txt = request.asset as TextAsset;
//		if (txt != null)
//			myText.text = txt.text;
//		
//
//		bundle.Unload(false);
//	}

	IEnumerator LoadMyScene() {
		// Load level from assetBundle.
		AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync ("cubescene", "CubeScene", true);
		if (request == null) {
			yield break;
		}
		yield return StartCoroutine (request);
	}

	IEnumerator LoadMyAnimationAsset ()
	{
		AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync("sphereprefab", "SpherePrefab.prefab", typeof(GameObject) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		GameObject prefab = request.GetAsset<GameObject> ();

		if (prefab != null)
			GameObject.Instantiate(prefab);
		
		TextAsset txt = request.GetAsset<TextAsset> ();
	}
}
