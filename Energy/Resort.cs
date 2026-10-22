using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Energy {
    internal class Resort {
        private const string cFileNameResort = "NetBeheer.xml";
        private const int cMinVersion = 1;
        private const int cCurrentVersion = 2;

        private int mStartZomer;
        private int mEndZomer;
        int mYear;
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

        internal double xResortTariff(DateTime pTimeStamp) {
            double lResult = 0.0;

            if (pTimeStamp.Month < mStartZomer || pTimeStamp.Month > mEndZomer) {
                lResult = mSelectedResortTariff.xWinterTariff(pTimeStamp);
            } else {
                lResult = mSelectedResortTariff.xSummerTariff(pTimeStamp);
            }
            return lResult;
        }
        internal Resort() {
            mResortTariffs = new List<ResortTariff>();
            mSelectedResortTariff = new ResortTariff();
            mStartZomer = 0;
            mEndZomer = 12;
            mYear = DateTime.Now.Year;
            sLoadResortLines();
            mChanged = false;
        }

        private void sLoadResortLines() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            XmlNode? lElement;
            string lFileName;
            int lVersion = 0;
            ResortTariff lResortLine;
            int lTemp;

            mResortTariffs.Clear();
            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileNameResort);
            if (File.Exists(lFileName)) {
                lDoc = new XmlDocument();
                lDoc.Load(lFileName);
                lRoot = lDoc.DocumentElement;
                if (lRoot != null) {
                    lElement = lRoot.GetAttributeNode("Versie");
                    if (lElement != null) {
                        if (int.TryParse(lElement.InnerText, out lTemp)) {
                            lVersion = lTemp;
                        }
                    }
                    lElement = lRoot.GetAttributeNode("Jaar");
                    if (lElement != null) {
                        if (int.TryParse(lElement.InnerText, out lTemp)) {
                            mYear = lTemp;
                        }
                    }
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "Zomer": {
                                        foreach (XmlNode bbNode in bNode.ChildNodes) {
                                            if (bbNode.NodeType != XmlNodeType.Comment) {
                                                switch (bbNode.Name) {
                                                    case "StartMaand": {
                                                            if (int.TryParse(bbNode.InnerText, out lTemp)) {
                                                                mStartZomer = lTemp;
                                                            }
                                                            break;
                                                        }
                                                    case "EindMaand": {
                                                            if (int.TryParse(bbNode.InnerText, out lTemp)) {
                                                                mEndZomer = lTemp;
                                                            }
                                                            break;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                    break;
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
                                    if (lTempInt == 24) {
                                        lTempInt = 0;
                                    }
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
                        case "Variabel": {
                                sProcessVariable(bNode, lResortLine);
                                break;
                            }
                    }
                }
            }
            return lResortLine;
        }

        private void sProcessVariable(XmlNode pNode, ResortTariff pResortLine) {
            double lTemp;
            int lTempInt;
            int lStartHour;
            double lPrice;
            VarTariff lSummerTariff;
            VarTariff lWinterTariff;

            lSummerTariff = new VarTariff();
            lWinterTariff = new VarTariff();
            lSummerTariff.xClearTariff();
            lWinterTariff.xClearTariff();
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "Zomer": {
                                foreach (XmlNode bxNode in bNode.ChildNodes) {
                                    if (bxNode.NodeType != XmlNodeType.Comment) {
                                        if (bxNode.Name == "Periode") {
                                            lStartHour = 0;
                                            lPrice = 0.0;
                                            foreach (XmlNode byNode in bxNode.ChildNodes) {
                                                if (byNode.NodeType != XmlNodeType.Comment) {
                                                    switch (byNode.Name) {
                                                        case "StartUur": {
                                                                if (int.TryParse(byNode.InnerText, out lTempInt)) {
                                                                    lStartHour = lTempInt;
                                                                }
                                                                break;
                                                            }
                                                        case "Prijs": {
                                                                if (double.TryParse(byNode.InnerText, out lTemp)) {
                                                                    lPrice = lTemp;
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                            lSummerTariff.xAddTariffLine(lStartHour, lPrice);
                                        }
                                    }
                                }
                            }
                            break;
                        case "Winter": {
                                foreach (XmlNode bxNode in bNode.ChildNodes) {
                                    if (bxNode.NodeType != XmlNodeType.Comment) {
                                        if (bxNode.Name == "Periode") {
                                            lStartHour = 0;
                                            lPrice = 0.0;
                                            foreach (XmlNode byNode in bxNode.ChildNodes) {
                                                if (byNode.NodeType != XmlNodeType.Comment) {
                                                    switch (byNode.Name) {
                                                        case "StartUur": {
                                                                if (int.TryParse(byNode.InnerText, out lTempInt)) {
                                                                    lStartHour = lTempInt;
                                                                }
                                                                break;
                                                            }
                                                        case "Prijs": {
                                                                if (double.TryParse(byNode.InnerText, out lTemp)) {
                                                                    lPrice = lTemp;
                                                                }
                                                                break;
                                                            }
                                                    }
                                                }
                                            }
                                            lWinterTariff.xAddTariffLine(lStartHour, lPrice);
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            lSummerTariff.xFinishTariff();
            lWinterTariff.xFinishTariff();
            pResortLine.xAddVarTariffs(lSummerTariff, lWinterTariff);
        }

        internal void xSaveResortLines() {
            XmlDocument lDoc;
            XmlElement lRoot;
            XmlElement lAansluitingElement;
            XmlElement lTariefElement;
            XmlElement lEntry;
            XmlElement lVariabelElement;
            XmlElement lSeizoenElement;
            XmlElement lPeriodeElement;
            XmlText lText;
            XmlAttribute lAttribute;
            int lIndex;

            if (mChanged) {
                lDoc = new XmlDocument();
                lRoot = lDoc.CreateElement("Liander");
                lDoc.AppendChild(lRoot);
                lAttribute = lDoc.CreateAttribute("Versie");
                lAttribute.Value = cCurrentVersion.ToString();
                lRoot.Attributes.Append(lAttribute);
                lAttribute = lDoc.CreateAttribute("Jaar");
                lAttribute.Value = mYear.ToString();
                lRoot.Attributes.Append(lAttribute);

                lSeizoenElement = lDoc.CreateElement("Zomer");
                lRoot.AppendChild(lSeizoenElement);

                lEntry = lDoc.CreateElement("StartMaand");
                lSeizoenElement.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mStartZomer.ToString());
                lEntry.AppendChild(lText);

                lEntry = lDoc.CreateElement("EindMaand");
                lSeizoenElement.AppendChild(lEntry);
                lText = lDoc.CreateTextNode(mEndZomer.ToString());
                lEntry.AppendChild(lText);

                foreach (ResortTariff bTariff in mResortTariffs) {
                    if (bTariff.xMax == 24) {
                        lIndex = 0;
                    }
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

                    lVariabelElement = lDoc.CreateElement("Variabel");
                    lAansluitingElement.AppendChild(lVariabelElement);

                    lSeizoenElement = lDoc.CreateElement("Zomer");
                    lVariabelElement.AppendChild(lSeizoenElement);
                    for (lIndex = 0; lIndex < bTariff.xSummerCount; lIndex++) {
                        lPeriodeElement = lDoc.CreateElement("Periode");
                        lSeizoenElement.AppendChild(lPeriodeElement);
                        lEntry = lDoc.CreateElement("StartUur");
                        lPeriodeElement.AppendChild(lEntry);
                        lText = lDoc.CreateTextNode(bTariff.xSummerStartHour(lIndex).ToString());
                        lEntry.AppendChild(lText);
                        lEntry = lDoc.CreateElement("Prijs");
                        lPeriodeElement.AppendChild(lEntry);
                        lText = lDoc.CreateTextNode(bTariff.xSummerPrice(lIndex).ToString());
                        lEntry.AppendChild(lText);
                    }

                    lSeizoenElement = lDoc.CreateElement("Winter");
                    lVariabelElement.AppendChild(lSeizoenElement);
                    for (lIndex = 0; lIndex < bTariff.xWinterCount; lIndex++) {
                        lPeriodeElement = lDoc.CreateElement("Periode");
                        lSeizoenElement.AppendChild(lPeriodeElement);
                        lEntry = lDoc.CreateElement("StartUur");
                        lPeriodeElement.AppendChild(lEntry);
                        lText = lDoc.CreateTextNode(bTariff.xWinterStartHour(lIndex).ToString());
                        lEntry.AppendChild(lText);
                        lEntry = lDoc.CreateElement("Prijs");
                        lPeriodeElement.AppendChild(lEntry);
                        lText = lDoc.CreateTextNode(bTariff.xWinterPrice(lIndex).ToString());
                        lEntry.AppendChild(lText);
                    }
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
