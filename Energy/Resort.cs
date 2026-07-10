using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Energy {
    internal class Resort {
        private const string cFileNameResort = "NetBeheer.xml";

        private List<ResortTariff> mResortTariffs = new List<ResortTariff>();
        private ResortTariff mSelectedResortTariff;

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

        internal Resort() {
            mResortTariffs = new List<ResortTariff>();
            mSelectedResortTariff = new ResortTariff();
            sLoadResortLines();
        }

        private void sLoadResortLines() {
            XmlDocument lDoc;
            XmlElement? lRoot;
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
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "Versie": {
                                        if (!int.TryParse(bNode.InnerText, out lVersion)) {
                                            lVersion = 0;
                                        }
                                        break;
                                    }
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
            }
        }

        private ResortTariff sProcessConnection(XmlNode pNode) {
            ResortTariff lResortLine;
            double lTemp;

            lResortLine = new ResortTariff();
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "Soort": {
                                lResortLine.xConnection = bNode.InnerText;
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

        internal void xSelectResortTariff(string pConnection) {
            foreach (ResortTariff bResortTariff in mResortTariffs) {
                if (bResortTariff.xConnection == pConnection) {
                    mSelectedResortTariff = bResortTariff;
                    break;
                }
            }
        }
    }
}
