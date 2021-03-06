﻿using UnityEngine;
using System.Collections;

public class PictureController : MonoBehaviour
{
    public float DownSpeed;     //掉落速度

    // Use this for initialization
    IEnumerator Start()
    {
        while (!ABTextureManager.script.ABRoadFinish)     //確認GameManager 圖庫載入完畢後，才開始動作
            yield return null;

        this.DownSpeed = GameDefinition.GameMode_DownSpeed;

        int num;

        //確保不連續出現相同物件
        if (GameModeManager.script.CurrentActivePictureList.Count > 0)
        {
            do
            {
                num = Random.Range(0, ABTextureManager.script.ChooseClassWordCollection.Count);
            } while (GameModeManager.script.CurrentActivePictureList[GameModeManager.script.CurrentActivePictureList.Count - 1].name == ABTextureManager.script.ChooseClassWordCollection[num].name && ABTextureManager.script.ChooseClassWordCollection.Count > 1);
        }
        else
            num = Random.Range(0, ABTextureManager.script.ChooseClassWordCollection.Count);

        this.gameObject.name = ABTextureManager.script.ChooseClassWordCollection[num].name;      //將物件名稱命名為辨識物名
        this.renderer.material.mainTexture = ABTextureManager.script.ChooseClassWordCollection[num] as Texture;
        GameModeManager.script.CurrentActivePictureList.Add(this.gameObject);   //將物件儲存到容器
    }

    // Update is called once per frame
    void Update()
    {
        if (ABTextureManager.script.ABRoadFinish)
        {
            this.transform.Translate(new Vector3(0, 0, this.DownSpeed) * Time.deltaTime);

            if (this.transform.position.y < GameModeManager.script.SafeScreenrect.bottom)   //當低於螢幕底邊時，消除此物件
            {
                this.DestroySelf();
            }
        }
    }
    /// <summary>
    /// 刪除自己，並移除設定
    /// </summary>
    public void DestroySelf()
    {
        this.renderer.material.mainTexture = null;
        GameModeManager.script.CurrentActivePictureList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
