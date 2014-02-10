using UnityEngine;
using System.Collections;

public class PictureController : MonoBehaviour
{
    public string PictureName;  //圖片名稱(語音辨識用)
    public float DownSpeed;     //掉落速度

    // Use this for initialization
    void Start()
    {
        int num = Random.Range(0, GameManager.script.TextureCollection.Count);
        this.PictureName = GameManager.script.TextureCollection[num].name;
        this.renderer.material.mainTexture = GameManager.script.TextureCollection[num] as Texture;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector3(0, 0, this.DownSpeed) * Time.deltaTime);

        if (this.transform.position.y < GameManager.script.SafeScreenrect.bottom)   //當低於螢幕底邊時，消除此物件
        {
            this.renderer.material.mainTexture = null;
            Destroy(this.gameObject);
        }
    }
}
