using UnityEngine;
using System.Collections;

public class ABtest : MonoBehaviour
{
    public GameObject testObject;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadAB());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadAB()
    {
        print(@"file:///D:\Github\SRLES\all.assetBunldes");
        WWW www = new WWW(@"file:///D:\Github\SRLES\all.assetBunldes");

        yield return www;
        print("OK!!");
        print(www.size);
        AssetBundle asset = www.assetBundle;
        print(www.assetBundle.LoadAll().Length);

        Texture tt = asset.Load("Apple") as Texture;

        this.testObject.renderer.material.mainTexture = tt;



    }
}
