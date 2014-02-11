using UnityEngine;
using System.Collections;

public class PictureController : MonoBehaviour
{
    public float DownSpeed;     //掉落速度

    // Use this for initialization
    IEnumerator Start()
    {
        yield return GameManager.script.isLoading;      //確認GameManager 圖庫載入完畢後，才開始動作
        int num = Random.Range(0, GameManager.script.TextureCollection.Count);
        this.gameObject.name = GameManager.script.TextureCollection[num].name;      //將物件名稱命名為辨識物名
        this.renderer.material.mainTexture = GameManager.script.TextureCollection[num] as Texture;
        GameManager.script.CurrentActivePictureList.Add(this.gameObject);   //將物件儲存到容器
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.script.isLoading)
        {
            this.transform.Translate(new Vector3(0, 0, this.DownSpeed) * Time.deltaTime);

            if (this.transform.position.y < GameManager.script.SafeScreenrect.bottom)   //當低於螢幕底邊時，消除此物件
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
        GameManager.script.CurrentActivePictureList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
