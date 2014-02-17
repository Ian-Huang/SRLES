using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class XmlManager : MonoBehaviour
{
    public string DocumentName = "SystemData.xml";
    [HideInInspector]
    public bool XmlRoadFinish = false;          //讀取狀態flag
    public XmlNode ClassListNode;               //課程清單節點

    private XmlDocument doc = new XmlDocument();

    public static XmlManager script;

    void Awake()
    {
        script = this;
    }

    // Use this for initialization
    void Start()
    {
        this.XmlInit();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void XmlInit()
    {
        this.XmlRoadFinish = false;

        //假如未建立檔案
        if (!File.Exists(this.DocumentName))
        {
            //建立根節點(ClassList)課程清單
            XmlElement classList = this.doc.CreateElement("ClassList");
            this.doc.AppendChild(classList);
        }
        else
            this.doc.Load(this.DocumentName);

        //yield return ABTextureManager.script;   //確認ABTextureManager script是否存在  
        //yield return ABTextureManager.script.ABRoadFinish;    //確認ABTextureManager AB資源是否已經載入完成

        this.ClassListNode = this.doc.SelectSingleNode("ClassList");   //課程清單主節點        

        this.XmlRoadFinish = true;
    }
    
    /// <summary>
    /// 課程名稱修改
    /// </summary>
    /// <param name="oldClassName">舊節點名稱</param>
    /// <param name="newClassName">新節點名稱</param>
    public void ModifyClassName(string oldClassName, string newClassName)
    {
        //找出要修改的舊節點
        XmlNode oldNode = this.ClassListNode.SelectSingleNode(oldClassName);

        //產生要取代的新節點
        XmlElement newNode = this.doc.CreateElement(newClassName);
        //將舊節點內部xml text複製到新節點
        newNode.InnerXml = oldNode.InnerXml;

        //新舊取代
        this.ClassListNode.ReplaceChild(newNode, oldNode);

        this.doc.Save(this.DocumentName);   //存檔
    }

    /// <summary>
    /// 儲存新單字到指定課程
    /// </summary>
    /// <param name="className">課程節點名</param>
    /// <param name="words">單字組</param>
    public void CreateNewWordToClass(string className, string[] words)
    {
        //找出要修改的課程節點
        XmlNode classNode = this.ClassListNode.SelectSingleNode(className);

        //修改後的課程節點(更換課程單字)
        XmlNode newClassNode = this.doc.CreateElement(className);
        foreach (string word in words)
        {
            XmlNode node = this.doc.CreateElement(word);
            newClassNode.AppendChild(node);
        }
        //取代舊的課程節點
        this.ClassListNode.ReplaceChild(newClassNode, classNode);
        this.doc.Save(this.DocumentName);   //存檔
    }


    /// <summary>
    /// 加入新課程節點
    /// </summary>
    /// <param name="className">課程節點名</param>
    public void CreateNewClassToList(string className)
    {
        XmlElement classNode = this.doc.CreateElement(className);
        this.ClassListNode.AppendChild(classNode);

        this.doc.Save(this.DocumentName);   //存檔
    }

    /// <summary>
    /// 刪除課程節點
    /// </summary>
    /// <param name="className">課程節點名</param>
    public void DeleteClassFromList(string className)
    {
        XmlNode classNode = this.ClassListNode.SelectSingleNode(className);   //課程清單主節點   
        this.ClassListNode.RemoveChild(classNode);

        this.doc.Save(this.DocumentName);   //存檔
    }

    public void SaveXml()
    {
        this.doc.Save(this.DocumentName);
    }

    /* 
         * ---插入課程單字---
                XmlElement ClassNode = doc.CreateElement("Class1");
                        foreach (var temp in ABTextureManager.script.TextureCollection)
                {
                    XmlElement word = doc.CreateElement("Word");
                    word.InnerText = temp.name;
                    ClassNode.AppendChild(word);
                }
                main.AppendChild(ClassNode);
        */

    /*
     * ---刪除---
    XmlNode sNode = main.SelectSingleNode("Class1");   //課程清單主節點   
    main.RemoveChild(sNode);
    */
}
