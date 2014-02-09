using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CreateAssetBunldes : MonoBehaviour
{

    [MenuItem("Custom Editor/Create AssetBunldes")]
    static void ExecCreateAssetBunldes()
    {
       
        // AssetBundle 的資料夾名稱及副檔名
        string targetDir = "_AssetBunldes";
        string extensionName = ".assetBunldes";

        //取得在 Project 視窗中選擇的資源(包含資料夾的子目錄中的資源)
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //建立存放 AssetBundle 的資料夾
        if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

        foreach (Object obj in SelectedAsset)
        {

            Debug.Log("Create AssetBunldes name :" + obj);

        }


        string targetPath = targetDir + Path.DirectorySeparatorChar + "all" + extensionName;
  
        //这里注意第二个参数就行

        if (BuildPipeline.BuildAssetBundle(null, SelectedAsset, targetPath, BuildAssetBundleOptions.CollectDependencies))
        {

            AssetDatabase.Refresh();

        }
        else
        {



        }

    }
}
