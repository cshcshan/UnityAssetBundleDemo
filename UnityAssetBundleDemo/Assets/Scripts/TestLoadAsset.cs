using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssetBundles;

public class TestLoadAsset : MonoBehaviour {

	public Text myText;
	static string assetBundleUrl = "http://172.16.129.32/UnityAssetBundle/AssetBundleTests/";

	private string assetBundleName = "mybundle";
	static string LAST_HASH = "LAST_HASH";


	void OnGUI() {
		GUI.Label (new Rect (10, 10, 300, 30), "Load AssetBundle");
		assetBundleUrl = GUI.TextField (new Rect (10, 50, 1200, 30), assetBundleUrl);
		if (GUI.Button(new Rect(10, 100, 300, 80), "Load Asset Bundles")) {
			Debug.Log ("[Han TEST IN UNITY]");

			StartCoroutine(xxx());
		}
	}

	IEnumerator xxx() {
		yield return StartCoroutine(Initialize());
		yield return StartCoroutine(LoadMyAssets());
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


		request = AssetBundleManager.LoadAssetAsync(assetBundleName, "data.bytes", typeof(TextAsset) );
		if (request == null)
			yield break;
		yield return StartCoroutine(request);

		// Get the asset.
		TextAsset txt = request.GetAsset<TextAsset> ();

//		if (txt != null)
//			myText.text = txt.text;

//		PlayerPrefs.SetString (LAST_HASH, AssetBundleManager.AssetBundleManifestObject.GetAssetBundleHash (assetBundleName).ToString());
	}


	IEnumerator LoadAssetsFromBundle (AssetBundle bundle) {

		AssetBundleRequest request = bundle.LoadAssetAsync ("logo", typeof(GameObject));

		yield return request;

		GameObject obj = request.asset as GameObject;
		GameObject clone = GameObject.Instantiate(obj);

		request = bundle.LoadAssetAsync ("data", typeof(TextAsset));
		yield return request;

		TextAsset txt = request.asset as TextAsset;
		if (txt != null)
			myText.text = txt.text;
		

		bundle.Unload(false);
	}
}
