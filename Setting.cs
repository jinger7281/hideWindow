using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace hideForm
{
    class Setting
    {
        private String initial;
        public static String confPath = "./config.xml";
        private XmlDocument xmlDoc;

        public Setting()
        {
            initial = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<config>\r\n" +
                "\t<!--\r\n" +
                "\t\tHideKey ShowKey是隐藏和显示程序的键 fk是功能键代码\r\n" +
                "\t\tfk代码列表：\r\n" +
                "\t\t0 不使用功能键\r\n" +
                "\t\t1 使用ALT键\r\n" +
                "\t\t2 使用CTRL键\r\n" +
                "\t\t3 使用SHIFT键\r\n" +
                "\t\t8 使用Win键\r\n" +
                "\t-->\r\n" +
                "\t<HideKey fk=\"0\">F6</HideKey>\r\n" +
                "\t<ShowKey fk=\"0\">F8</ShowKey>\r\n" +
                "\t<!--隐藏模式-->\r\n" +
                "\t<Mode>KeyWord</Mode>\r\n" +
                "\t<!--关键词列表-->\r\n" +
                "\t<KeyWords>\r\n" +
                "\t\t<KeyWord>魔兽世界</KeyWord>\r\n" +
                "\t\t<KeyWord>梦幻西游</KeyWord>\r\n" +
                "\t\t<KeyWord>完美国际</KeyWord>\r\n" +
                "\t\t<KeyWord>完美世界</KeyWord>\r\n" +
                "\t\t<KeyWord>LOL</KeyWord>\r\n" +
                "\t\t<KeyWord>天龙八部</KeyWord>\r\n" +
                "\t\t<KeyWord>地下城与勇士</KeyWord>\r\n" +
                "\t</KeyWords>\r\n" +
                "</config>\r\n";
            xmlDoc = new XmlDocument();
        }

        public void init()
        {
            FileStream fs = new FileStream(confPath, FileMode.Create, FileAccess.Write);
            fs.Write(Encoding.UTF8.GetBytes(initial), 0, Encoding.UTF8.GetByteCount(initial));
            fs.Close();
        }

        public void updateXML(String nodeName, String nodeText, String attribText, String attribName="")
        {
            xmlDoc.Load(confPath);
            XmlNode list = xmlDoc.SelectSingleNode(nodeName);
            XmlElement ele = (XmlElement)list;
            ele.InnerText = nodeText;
            if (attribName.Length > 0)
            {
                ele.SetAttribute(attribName, attribText);
            }
            xmlDoc.Save(confPath);
        }

        public String readXML(String nodeName, String attribName = "")
        {
            xmlDoc.Load(confPath);
            XmlNode list = xmlDoc.SelectSingleNode(nodeName);
            XmlElement ele = (XmlElement)list;
            if (attribName.Length <= 0)
            {
                return ele.InnerText;
            }
            else
            {
                return ele.GetAttribute(attribName);
            }
        }

        public void addElement(String parent, String nodeName, String nodeText, String attribText = "", String attribName = "")
        {
            xmlDoc.Load(confPath);
            XmlNode list = xmlDoc.SelectSingleNode(parent);
            XmlElement ele = xmlDoc.CreateElement(nodeName);
            ele.InnerText = nodeText;
            if (attribName.Length > 0)
            {
                ele.SetAttribute(attribName, attribText);
            }
            list.PrependChild(ele);
            xmlDoc.Save(confPath);
        }

        public bool isElementExist(String nodeName, String nodeText, bool isParent = true)
        {
            //TODO 本方法没有正确判断XML是否存在
            if (xmlDoc.SelectSingleNode(nodeName) != null && xmlDoc.SelectSingleNode(nodeName).InnerText.Equals(nodeText))
            {
                return true;
            }
            return false;
        }
    }
}
