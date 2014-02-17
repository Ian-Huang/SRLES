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
    /// 加入新課程節點
    /// </summary>
    /// <param name="className">課程節點名</param>
    public void CreateNewClassToList(string className)
    {
        XmlElement classNode = this.doc.CreateElement(className);
        this.ClassListNode.AppendChild(classNode);

        this.doc.Save(this.DocumentName);   //存檔
    }

    public void ModifyClassName(string oldClassName, string newClassName)
    {
        //未完成
        XmlNode classNode = this.ClassListNode.SelectSingleNode(oldClassName);
        //print(classNode.Value);
    }

    /// <summary>
    /// 加入新單字節點到指定課程節點
    /// </summary>
    /// <param name="className">課程節點名</param>
    /// <param name="wordName">單字名</param>
    public void CreateNewWordToClass(string className, string wordName)
    {
        XmlNode classNode = this.ClassListNode.SelectSingleNode(className);

        if (classNode.SelectSingleNode(wordName) == null)   //不重複儲存同名節點
        {
            XmlElement wordNode = this.doc.CreateElement(wordName);
            classNode.AppendChild(wordNode);

            this.doc.Save(this.DocumentName);   //存檔
        }
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
