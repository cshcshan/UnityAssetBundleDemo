# Unity to AssetBundle Demo

* ### Create AssetBundle

1. Switch Unity project's platform.
2. Importing AssetBundleManager package.
3. Setting AssetBundle and AssetBundle Variant's name of asset.
4. Building AssetBundles by selecting 'Assets/AssetBundles/Build AssetBundles'.

* ### Server

1. After building AssetBundles, copy '{Project}/AssetBundles' folder to server and rename it. There are many platform name's folders in the {Project}/AssetBundles, if you build many platform for AssetBundles.

* ### Load AssetBundle

1. The AssetBundle's url is 'http://{ip or host name}/{AssetBundles or what folder name you named}/'. For example is 'http://172.16.129.32/UnityAnimationDemo/'.
