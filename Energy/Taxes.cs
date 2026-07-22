using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Energy {
    internal class Taxes {
        private const string cFileName = "Belasting.xml";
        private const int cCurrentVersion = 1;
        bool mChanged;

        private double mTax;
        private double mReturn;

        internal double xTax {
            get {
                return mTax;
            }
            set {
                if (mTax != value) {
                    mTax = value;
                    mChanged = true;
                }
            }
        }

        internal double xReturn {
            get {
                return mReturn;
            }
            set {
                if (mReturn != value) {
                    mReturn = value;
                    mChanged = true;
                }
            }
        }

        internal Taxes() {
            mTax = 0.09161;
            mReturn = -1.42313;
            sLoadTaxes();
            mChanged = false;
        }

        private void sLoadTaxes() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            XmlNode? lElement;
            string lFileName;
            int lVersion;
            double lTemp;

            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileName);
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
                                case "PerKWh": {
                                        if (double.TryParse(bNode.InnerText, out lTemp)) {
                                            mTax = lTemp;
                                        }
                                        break;
                                    }
                                case "TerugPerDag": {
                                        if (double.TryParse(bNode.InnerText, out lTemp)) {
                                            mReturn = lTemp;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        internal void xSaveTaxes() {
            XmlDocument lDoc;
            XmlElement lRoot;
            XmlElement lEntry;
            XmlText lText;
            XmlAttribute lAttribute;

            if (mChanged) {
                lDoc = new XmlDocument();
                lRoot = lDoc.CreateElement("Belasting");
                lDoc.AppendChild(lRoot);
                lAttribute = lDoc.CreateAttribute("Versie");
                lAttribute.Value = cCurrentVersion.ToString();
                lRoot.Attributes.Append(lAttribute);

                lEntry = lDoc.CreateElement("PerKWh");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mTax.ToString());
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("TerugPerDag");
                lRoot.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mReturn.ToString());
                lEntry.AppendChild(lText);

                lDoc.Save(Path.Combine(Parameters.GetInstance.xDataDir, cFileName));
                mChanged = false;
            }
        }
    }
}
