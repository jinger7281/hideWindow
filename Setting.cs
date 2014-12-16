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
                "<!--如果配置文件出错，删除即可-->\r\n" +
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

        /**
         * @brief 初始化配置文本
         * @param 空
         * @return 无返回值
         * */
        public void init()
        {
            FileStream fs = new FileStream(confPath, FileMode.Create, FileAccess.Write);
            fs.Write(Encoding.UTF8.GetBytes(initial), 0, Encoding.UTF8.GetByteCount(initial));
            fs.Close();
        }

        /**
         * @brief 更新XML文件中的内容
         * @param nodeName 节点名称 示例"/config/KeyWords"
         * @param nodeText 节点文本内容
         * @param attribText 节点属性文本
         * @param attribName 节点属性名称
         * @return 无返回值
         * */
        public void updateXML(String nodeName, String nodeText, String attribText = "", String attribName="")
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

        /**
         * @biref 读取XML文件中的指定节点的属性值或文本值
         * @param nodeName 节点的名称
         * @param attribName 节点的属性名称，默认为空
         * @return 如果attribName不为空返回节点的指定属性的值，如果为空返回节点的文本值
         * */
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

        /**
         * @brief 返回指定节点的子节点的内容
         * @param nodeName 父节点名称
         * @return 节点内容的文本列表
         * */
        public List<String> readXML(String nodeName)
        {
            xmlDoc.Load(confPath);
            List<String> childText = new List<string>();
            XmlNode list = xmlDoc.SelectSingleNode(nodeName);
            foreach (XmlNode i in list.ChildNodes)
            {
                childText.Add(i.InnerText);
            }
            return childText;
        }

        /**
         * @brief 为指定的节点添加子节点
         * @param parent 要添加的子节点的节点名称
         * @param nodeName 子节点的名称
         * @param nodeText 子节点的文本值
         * @param attribText 子节点的属性值
         * @param attribName 子节点的属性名称
         * @return 无返回值
         * */
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

        /**
         * @brief 判断某个节点或该节点的子节点是不是存在
         * @param nodeName 节点名称
         * @param nodeText 节点的文本内容
         * @param isParent 该节点是子节点还是父节点，默认是true表示该节点是父节点
         * @return 如果存在返回true,如果不存在返回false
         * */
        public bool isElementExist(String nodeName, String nodeText, bool isParent = true)
        {
            XmlNode currentNode = xmlDoc.SelectSingleNode(nodeName);
            if (currentNode == null) return false;  //如果当前（父）节点不存在那一定是不存在
            //判断当前节点是不是存在,如果是判断当前节点的话，isParent = false
            if (currentNode.InnerText.Equals(nodeText) && isParent == false)
            {
                return true;
            }

            //如果判断当前节点的子节点使用以下过程来判断
            if (currentNode.ChildNodes.Count > 0)
            {
                foreach(XmlNode i in currentNode.ChildNodes){
                    if (i.InnerText.Equals(nodeText))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
