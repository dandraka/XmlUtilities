using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Dandraka.XmlUtilities
{
    /// <summary>
    /// Implements dynamic XML parsing, the result being a dynamic object with properties 
    /// matching the xml nodes. 
    /// Note that if under a certain node there are multiple nodes
    /// with the same name, a list property will be created. The list property's name will
    /// be [common name]List, e.g. bookList.
    /// </summary>
    public static class XmlSlurper
    {
        /// <summary>
        /// Parses the given xml and returns a <c>System.Dynamic.ExpandoObject</c>.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static dynamic ParseText(string text)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);

            dynamic slurper = new ExpandoObject();

            var root = xmlDoc.DocumentElement;
            var rootNodes = root.ChildNodes;

            foreach (var a in root.Attributes.OfType<XmlAttribute>())
            {
                ((IDictionary<string, Object>)slurper).Add(getValidName(a.LocalName), a.Value);
            }
            foreach (var nodes in rootNodes.OfType<XmlNode>().GroupBy(x => x.LocalName))
            {
                addPropertyGroup(slurper, nodes);
            }

            return slurper;
        }        

        private static void addPropertyGroup(ExpandoObject slurper, IGrouping<string, XmlNode> nodes)
        {
            var nodeList = nodes.ToList();

            if (nodeList.Count == 1)
            {
                addProperty(slurper, nodeList[0]);
            }
            else
            {
                var groupName = getValidName(nodeList[0].LocalName) + "List";
                var groupList = new List<ExpandoObject>();
                ((IDictionary<string, Object>)slurper).Add(getValidName(groupName), groupList);
                foreach (var node in nodeList)
                {
                    addItemToList(groupList, node);
                }                
            }
        }

        private static void addItemToList(List<ExpandoObject> groupList, XmlNode node)
        {
            if ((node.HasChildNodes && node.ChildNodes.OfType<XmlNode>().Count(c => c.LocalName != "#text") > 0) ||
                node.Attributes.Count > 0)
            {
                dynamic slurperChild = new ExpandoObject();
                groupList.Add(slurperChild);

                foreach (XmlAttribute attr in node.Attributes)
                {
                    ((IDictionary<string, Object>)slurperChild).Add(getValidName(attr.LocalName), attr.Value);
                }
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.LocalName == "#text")
                    {
                        //groupList.Add(node.LocalName, node.Value);
                    }
                    else
                    {
                        addProperty(slurperChild, childNode);
                    }
                }
            }
            else
            {
                //groupList.Add(node.LocalName, node.Value ?? node.ChildNodes[0].Value);
            }
        }

        private static void addProperty(ExpandoObject slurper, XmlNode node)
        {
            if ((node.HasChildNodes && node.ChildNodes.OfType<XmlNode>().Count(c => c.LocalName != "#text") > 0) ||
                node.Attributes.Count > 0)
            {
                dynamic slurperChild = new ExpandoObject();
                ((IDictionary<string, Object>) slurper).Add(getValidName(node.LocalName), slurperChild);

                foreach (XmlAttribute attr in node.Attributes)
                {
                    ((IDictionary<string, Object>) slurperChild).Add(getValidName(attr.LocalName), attr.Value);
                }
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (childNode.LocalName == "#text")
                    {
                        ((IDictionary<string, Object>) slurper).Add("Value", node.Value);
                    }
                    else
                    {
                        addProperty(slurperChild, childNode);
                    }
                }
            }
            else
            {
                ((IDictionary<string, Object>) slurper).Add(getValidName(node.LocalName), node.Value ?? node.ChildNodes[0].Value);
            }
        }

        private static string getValidName(string nodeName)
        {
            Regex rgx = new Regex("[^0-9a-zA-Z]+");
            string res = rgx.Replace(nodeName, "");
            return res;
        }
    }
}
