using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Energy {
    internal class Situation {
        private const string cFileName = "Situatie.xml";
        private const int cCurrentVersion = 1;

        private string mName;
        private bool mBusiness;
        private int mBattery;
        private string mConnectionLabel;
        private string mProviderLabel;
        private bool mChanged;

        internal string xName {
            get {
                return mName;
            }
        }

        internal bool xBusiness {
            get {
                return mBusiness;
            }
            set {
                if (mBusiness != value) {
                    mBusiness = value;
                    mChanged = true;
                }
            }
        }

        internal int xBattery {
            get {
                return mBattery;
            }
            set {
                if (mBattery != value) {
                    mBattery = value;
                    mChanged = true;
                }
            }
        }   

        internal string xConnectionLabel {
            get {
                return mConnectionLabel;
            }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    if (mConnectionLabel != value) {
                        mConnectionLabel = value;
                        mConnectionLabel = value;
                        mChanged = true;
                    }
                }
            }
        }

        internal string xProviderLabel {
            get {
                return mProviderLabel;
            }
            set {
                if (!string.IsNullOrEmpty(value)) {
                    if (mProviderLabel != value) {
                        mProviderLabel = value;
                        mChanged = true;
                    }
                }
            }
        }

        internal Situation(string pName) {
            mName = pName;
            mBusiness = false;
            mBattery = 0;
            mConnectionLabel = "";
            mProviderLabel = "";
            mChanged = false;
            sLoadSituation();
        }

        internal Situation() {
            mName = "";
            mBusiness = false;
            mBattery = 0;
            mConnectionLabel = "";
            mProviderLabel = "";
            mChanged = false;
        }

        private void sLoadSituation() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            XmlNode? lElement;
            string lFileName;
            int lVersion;
            int lTemp;

            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, mName, cFileName);
            if (File.Exists(lFileName)) {
                lDoc = new XmlDocument();
                lDoc.Load(lFileName);
                lRoot = lDoc.DocumentElement;
                if (lRoot != null) {
                    lElement = lRoot.GetAttributeNode("Versie");
                    if (lElement != null) {
                        if (!int.TryParse(lElement.InnerText, out lVersion)) {
                            lVersion = 0;
                        }
                    }
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "Naam": {
                                        mName = bNode.InnerText;
                                        break;
                                    }
                                case "Zakelijk": {
                                        mBusiness = bool.Parse(bNode.InnerText);
                                        break;
                                    }
                                case "Batterij": {
                                        if (int.TryParse(bNode.InnerText, out lTemp)) {
                                            mBattery = lTemp;
                                        }
                                        break;
                                    }
                                case "Aansluiting": {
                                        mConnectionLabel = bNode.InnerText;
                                        break;
                                    }
                                case "Contract": {
                                        mProviderLabel = bNode.InnerText;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        internal void xSaveSituation() {
            XmlDocument lDoc;
            XmlElement lRoot;
            XmlElement lEntry;
            XmlText lText;
            XmlAttribute lAttribute;

            if (mChanged && !string.IsNullOrEmpty(mName)) {
                lDoc = new XmlDocument();
                lRoot = lDoc.CreateElement("Situatie");
                lDoc.AppendChild(lRoot);
                lAttribute = lDoc.CreateAttribute("Versie");
                lAttribute.Value = cCurrentVersion.ToString();
                lRoot.Attributes.Append(lAttribute);

                lEntry = lDoc.CreateElement("Naam");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mName);
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("Zakelijk");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mBusiness.ToString());
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("Batterij");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mBattery.ToString());
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("Aansluiting");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mConnectionLabel);
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("Contract");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mProviderLabel);
                lEntry.AppendChild(lText);
                lDoc.Save(Path.Combine(Parameters.GetInstance.xDataDir, mName, cFileName));
                mChanged = false;
            }
        }
    }
}
