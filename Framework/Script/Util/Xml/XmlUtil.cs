using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class XmlUtil
{
    private XmlDocument _doc = null;
    public string InnerXml { get { return _doc == null ? string.Empty : _doc.InnerXml; } }
    public bool IsLoad { get { return _doc != null; } }
    public string FileName;

    #region Save
    public void Save(string fileName)
    {
        FileName = Path.GetFileName(fileName);
        _doc.Save(fileName);
    }

    public XmlUtil_Node CreateRootNode(string nodeName)
    {
        _doc = new XmlDocument();
        _doc.AppendChild(_doc.CreateXmlDeclaration("1.0", "UTF-8", "yes"));

        var xmlElement = _doc.CreateElement(nodeName);
        _doc.AppendChild(xmlElement);
        var xmlNode = new XmlUtil_Node(xmlElement, FileName);

        return xmlNode;
    }
    #endregion 

    #region Load

    public async UniTask<bool> LoadAsset(string filePath)
    {
        FileName = filePath;
        _doc = new XmlDocument();
        string name = Path.GetFileNameWithoutExtension(filePath);
        var asset = await AddressableManager.Load<TextAsset>(name);
        var memoryStream = new MemoryStream(asset.bytes);
        var reader = XmlReader.Create(memoryStream);
        
        try
        {
            _doc.Load(reader);
        }
        catch (Exception e)
        {
            DebugManager.LogError(e.Message);
            return false;
        }
        return true;
    }

    public XmlUtil_Node GetRootChild(string nodeName)
    {
        if (_doc == null)
            return null;

        XmlNode xmlNode = _doc.SelectSingleNode(nodeName);
        XmlUtil_Node node = new XmlUtil_Node(xmlNode as XmlElement, FileName);
        return node;
    }
    
    #endregion
}

public class XmlUtil_Node
{
    private XmlElement _xmlElement = null;
    public bool IsEmpty { get { return _xmlElement.IsEmpty; } }

    public string InnerXml
    {
        get { return _xmlElement == null ? string.Empty : _xmlElement.InnerXml; }
        set { if (_xmlElement != null) _xmlElement.InnerXml = value; }
    }
    public string Name { get { return _xmlElement == null ? string.Empty : _xmlElement.Name; } }
    private XmlDocument ownerDocument { get { return _xmlElement?.OwnerDocument; } }
    public string FileName = string.Empty;
    public XmlAttributeCollection Attributes { get { return _xmlElement.Attributes; } }

    public XmlUtil_Node(XmlElement xmlElement, string fileName)
    {
        _xmlElement = xmlElement;
        FileName = fileName;
    }

    public bool IsName(string name)
    {
        if (_xmlElement == null || string.Equals(_xmlElement.Name, name) == false)
        {
            return false;
        }

        return true;
    }

    public void ErrorPrint(string message)
    {
        var navi = _xmlElement.CreateNavigator();
        Debug.LogErrorFormat("{0} \n {1} {2}", FileName, message, navi.OuterXml);
    }

    #region Save
    public XmlUtil_Node AddNode(string nodeName)
    {
        if (ownerDocument == null)
            return null;
        
        var xmlElement = ownerDocument.CreateElement(nodeName);
        _xmlElement.AppendChild(xmlElement);
        var xmlNode = new XmlUtil_Node(xmlElement, FileName);

        return xmlNode;
    }

    public void AddAttribute(string attrName, string value)
    {
        if (ownerDocument == null)
            return;
        
        var xmlAttr = ownerDocument.CreateAttribute(attrName);
        xmlAttr.Value = value;
        _xmlElement.SetAttributeNode(xmlAttr);
    }

    public XmlUtil_Node AddNodeAttribute(string nodeName, string attrName, string value)
    {
        var node = AddNode(nodeName);
        node.AddAttribute(attrName, value);
        return node;
    }
    #endregion

    #region Remove
    public bool RemoveNode(XmlUtil_Node removeNode)
    {
        _xmlElement.RemoveChild(removeNode._xmlElement as XmlNode);
        return true;
    }

    public bool RemoveAttribute(string attrName)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        
        _xmlElement.RemoveAttributeNode(xmlAttr);
        return true;
    }
    #endregion
    
    #region Load

    public XmlUtil_Node GetChild(string nodeName)
    {
        var xmlElement = _xmlElement.SelectSingleNode(nodeName) as XmlElement;
        if (xmlElement == null)
            return null;
        
        var node = new XmlUtil_Node(xmlElement, FileName);
        return node;
    }

    public void GetChildren(ref List<XmlUtil_Node> children)
    {
        var enumerator = _xmlElement.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var element = enumerator.Current as XmlElement;
            if (element == null)
                continue;

            var node = new XmlUtil_Node(element, FileName);
            children.Add(node);
        }
    }

    public List<XmlUtil_Node> GetChildren(string nodeName)
    {
        var nodeList = _xmlElement.SelectNodes(nodeName);
        if (nodeList == null || nodeList.Count == 0)
            return null;

        var xmlNodeList = new List<XmlUtil_Node>();
        foreach (var eChild in nodeList)
        {
            var element = eChild as XmlElement;
            if(element == null)
                continue;
            
            var node = new XmlUtil_Node(element, FileName);
            xmlNodeList.Add(node);
        }

        return xmlNodeList;
    }

    //GetAttr-------------------------------------------------------------------------------------------
    public bool GetAttr(string attrName, ref string stringValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        stringValue = xmlAttr.Value;
        return true;
    }

    public bool GetAttr(string attrName, ref byte byteValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        byteValue = System.Convert.ToByte(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref int integerValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        integerValue = System.Convert.ToInt32(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref uint integerValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        integerValue = System.Convert.ToUInt32(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref long longValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        longValue = System.Convert.ToInt64(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref ulong longValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        longValue = System.Convert.ToUInt64(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref float floatValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        floatValue = System.Convert.ToSingle(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref short shortValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        shortValue = System.Convert.ToInt16(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref ushort shortValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        shortValue = System.Convert.ToUInt16(xmlAttr.Value);
        return true;
    }
    
    public bool GetAttr(string attrName, ref bool boolValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        boolValue = false;
        if (xmlAttr.Value.ToUpper().Equals("TRUE"))
            boolValue = true;
        
        return true;
    }
    
    public bool GetAttr(string attrName, ref Vector3 vector3Value)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        string[] temp = xmlAttr.Value.Trim(new char[] { '(', ')' }).Replace(" ", "").Split(',');
        vector3Value.x = float.Parse(temp[0]);
        vector3Value.y = float.Parse(temp[1]);
        vector3Value.z = float.Parse(temp[2]);
        
        return true;
    }
    
    public bool GetAttr(string attrName, ref Vector4 vector4Value)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        string[] temp = xmlAttr.Value.Trim(new char[] { '(', ')' }).Replace(" ", "").Split(',');
        vector4Value.x = float.Parse(temp[0]);
        vector4Value.y = float.Parse(temp[1]);
        vector4Value.z = float.Parse(temp[2]);
        vector4Value.w = float.Parse(temp[3]);
        
        return true;
    }
    
    public bool GetAttr(string attrName, ref Color colorValue)
    {
        var xmlAttr = _xmlElement.Attributes.GetNamedItem(attrName) as XmlAttribute;
        if (xmlAttr == null)
            return false;
        string[] temp = xmlAttr.Value.Trim(new char[]{'(',')',' ','R','G','B','A'}).Split(',');
        colorValue.r = float.Parse(temp[0]);
        colorValue.g = float.Parse(temp[1]);
        colorValue.b = float.Parse(temp[2]);
        colorValue.a = float.Parse(temp[3]);
        
        return true;
    }
    //GetAttr-------------------------------------------------------------------------------------------
    
    //GetChildAttr--------------------------------------------------------------------------------------
    public bool GetChildAttr(string nodeName, string attrName, ref string stringValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref stringValue);
    }

    public bool GetChildAttr(string nodeName, string attrName, ref byte byteValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref byteValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref int integerValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref integerValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref uint integerValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref integerValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref long longValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref longValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref ulong longValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref longValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref float floatValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref floatValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref short shortValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref shortValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref ushort shortValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref shortValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref bool boolValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref boolValue);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref Vector3 vector3Value)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref vector3Value);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref Vector4 vector4Value)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref vector4Value);
    }
    
    public bool GetChildAttr(string nodeName, string attrName, ref Color colorValue)
    {
        var xmlNode = GetChild(nodeName);
        if (nodeName == null)
            return false;
        
        return xmlNode.GetAttr(attrName, ref colorValue);
    }
    //GetChildAttr--------------------------------------------------------------------------------------
    #endregion
}