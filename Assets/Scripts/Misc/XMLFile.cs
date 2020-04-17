using System;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml.XPath;
public class XMLFile
{
 
    public string FileName { get; private set; }
    public XDocument Document { get; private set; }
    private XElement Root;
    private XMLFile(string fileName, XDocument document)
    {
        FileName = fileName;
        Document = document;
        Root = Document.Root;
    }


    private XElement CreateNode(string name, string[] attributes = null, string innerText = null)
    {
        List<XAttribute> xAtribs = new List<XAttribute>();
        if (attributes != null)
        {
            foreach (string s in attributes)
            {
                string[] atVal = NodeAttributeFromString(s);
                xAtribs.Add(new XAttribute(atVal[0], atVal[1]));
            }
        }
        XElement node = new XElement(name);
        foreach(XAttribute xatr in xAtribs)
        {
            node.Add(xatr);
        }
        if (innerText != null)
            node.Value = innerText;

        return node;
        
    }

    //Creates a new root node with given attributes
    public XElement AddNode(string name, string[] attributes=null, string innerText=null)
    {
        XElement node = CreateNode(name, attributes, innerText);
        Root.Add(node);
        return node;
    }

    public XElement AddChildNode(XElement parent, string name, string[] attributes=null, string innerText = null)
    {
        XElement childNode = CreateNode(name, attributes, innerText);
        parent.Add(childNode);
        return childNode;
    }

    public void Save()
    {
        Document.Save(FileName);
    }

    public static XMLFile CreateXML(string fileName, string rootName)
    {

        XDocument doc = new XDocument(new XElement(rootName));
        return new XMLFile(fileName, doc);
    }
    public static string[] NodeAttributeFromString(string total)
    {
        string[] atVal = total.Split('=');
        if (atVal.Length != 2)
        {
            throw new Exception("Given string is not in the correct form." +
                " Must take the form \"'attribute\'=\"'value\'\"");
        }
        return atVal;
    }
}
