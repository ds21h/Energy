using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Energy {
    internal class Resort {
        private const string cFileNameResort = "NetBeheer.xml";
        private const int cCurrentVersion = 1;

        private List<ResortTariff> mResortTariffs = new List<ResortTariff>();
        private ResortTariff mSelectedResortTariff;
        private bool mChanged;

        internal List<ResortTariff> xResortTariffs {
            get {
                return mResortTariffs;
            }
        }

        internal ResortTariff xSelectedResortTariff {
            get {
                return mSelectedResortTariff;
            }
            set {
                mSelectedResortTariff = value;
            }
        }

        internal bool xResortChanged {
            set {
                mChanged = value;
            }
        }

        internal Resort() {
            mResortTariffs = new List<ResortTariff>();
            mSelectedResortTariff = new ResortTariff();
            sLoadResortLines();
            mChanged = false;
        }

        private void sLoadResortLines() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            XmlNode? lElement;
            string lFileName;
            int lVersion;
            ResortTariff lResortLine;

            mResortTariffs.Clear();
            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileNameResort);
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
                                case "Aansluiting": {
                                        lResortLine = sProcessConnection(bNode);
                                        if (lResortLine != null) {
                                            mResortTariffs.Add(lResortLine);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                mResortTariffs.Sort();
            }
        }

        private ResortTariff sProcessConnection(XmlNode pNode) {
            ResortTariff lResortLine;
            double lTemp;
            int lTempInt;

            lResortLine = new ResortTariff();
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "Fasen": {
                                if (int.TryParse(bNode.InnerText, out lTempInt)) {
                                    lResortLine.xFases = lTempInt;
                                }
                                break;
                            }
                        case "Max": {
                                if (int.TryParse(bNode.InnerText, out lTempInt)) {
                                    lResortLine.xMax = lTempInt;
                                }
                                break;
                            }
                        case "Tarief": {
                                foreach (XmlNode bxNode in bNode.ChildNodes) {
                                    if (bxNode.NodeType != XmlNodeType.Comment) {
                                        switch (bxNode.Name) {
                                            case "Dag": {
                                                    if (double.TryParse(bxNode.InnerText, out lTemp)) {
                                                        lResortLine.xPriceDay = lTemp;
                                                    }
                                                    break;
                                                }
                                            case "Jaar": {
                                                    if (double.TryParse(bxNode.InnerText, out lTemp)) {
                                                        lResortLine.xPriceYear = lTemp;
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
            return lResortLine;
        }

        internal void xSaveResortLines() {
            XmlDocument lDoc;
            XmlElement lRoot;
            XmlElement lAansluitingElement;
            XmlElement lTariefElement;
            XmlElement lEntry;
            XmlText lText;
            XmlAttribute lAttribute;

            if (mChanged) {
                lDoc = new XmlDocument();
                lRoot = lDoc.CreateElement("Liander");
                lDoc.AppendChild(lRoot);
                lAttribute = lDoc.CreateAttribute("Versie");
                lAttribute.Value = cCurrentVersion.ToString();
                lRoot.Attributes.Append(lAttribute);
                foreach (ResortTariff bTariff in mResortTariffs) {
                    lAansluitingElement = lDoc.CreateElement("Aansluiting");
                    lRoot.AppendChild(lAansluitingElement);

                    lEntry = lDoc.CreateElement("Fasen");
                    lAansluitingElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bTariff.xFases.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("Max");
                    lAansluitingElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bTariff.xMax.ToString());
                    lEntry.AppendChild(lText);

                    lTariefElement = lDoc.CreateElement("Tarief");
                    lAansluitingElement.AppendChild(lTariefElement);

                    lEntry = lDoc.CreateElement("Dag");
                    lTariefElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bTariff.xPriceDay.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("Jaar");
                    lTariefElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bTariff.xPriceYear.ToString());
                    lEntry.AppendChild(lText);
                }
                lDoc.Save(Path.Combine(Parameters.GetInstance.xDataDir, cFileNameResort));
                mChanged = false;
            }
        }

        internal void xSelectResortTariff(string pConnection) {
            foreach (ResortTariff bResortTariff in mResortTariffs) {
                if (bResortTariff.xConnection == pConnection) {
                    mSelectedResortTariff = bResortTariff;
                    break;
                }
            }
        }

        internal void xAddResortTariff(ResortTariff pResortTariff) {
            mResortTariffs.Add(pResortTariff);
            mResortTariffs.Sort();
            mChanged = true;
        }

        internal void xRemoveResortTariff(ResortTariff pResortTariff) {
            mResortTariffs.Remove(pResortTariff);
            mChanged = true;
        }
    }
}
