using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Energy {
    internal class Data {
        private static Data? mData;

        private const string cFileNameData = "Standen";
        private const string cFileNameProviders = "Providers.xml";
        private const int cCurrentVersionBase = 1;
        private const int cCurrentVersionProviders = 1;
        private CultureInfo mFileCulture = new CultureInfo("nl-NL");
        private List<DataLine> mLines = new List<DataLine>(50000);
        private DateTime mStart;
        private DateTime mEnd;
        private bool mDataModified;
        private bool mProvidersModified;
        private List<Provider> mProviders = new List<Provider>();
        private Provider mSelectedProvider;
        private double mTax = 0.09161;
        private double mResort = 1.0824;
        private int mBattery = 0;

        internal List<DataLine> xLines {
            get {
                return mLines;
            }
        }

        internal List<Provider> xProviders {
            get {
                return mProviders;
            }
        }

        internal Provider xSelectedProvider {
            get {
                return mSelectedProvider;
            }
            set {
                mSelectedProvider = value;
            }
        }

        internal DataLine? xLastEntry {
            get {
                DataLine? lLine;
                if (mLines.Count > 0) {
                    lLine = mLines[mLines.Count - 1];
                } else {
                    lLine = null;
                }
                return lLine;
            }
        }

        internal double xResort {
            get {
                return mResort;
            }
        }   

        internal int xBattery {
            get {
                return mBattery;
            }
            set {
                mBattery = value;
            }
        }

        internal static Data getInstance {
            get {
                if (mData == null) {
                    mData = new Data();
                }
                return mData;
            }
        }

        internal bool xProvidersChanged {
            set {
                mProvidersModified = value;
            }
        }

        private Data() {
            sReadData();
            if (mLines.Count > 0) {
                mStart = mLines[0].xTimeStampUTC;
                mEnd = mLines[mLines.Count - 1].xTimeStampUTC;
            } else {
                mStart = DateTime.MinValue;
                mEnd = DateTime.MinValue;
            }
            mDataModified = false;
            mProvidersModified = false;
            sReadProviders();
//            sCalculateProviders();
            if (mProviders.Count > 0) {
                mSelectedProvider = mProviders[0];
            } else {
                mSelectedProvider = new Provider();
            }
            xCalculate();
        }

        private void sReadData() {
            string lFileName;
            string? lLine;
            string[] lParts;
            StreamReader lStreamIn;
            int lVersion;
            DateTime lTimeStampUTC;
            DateTime lTimeStampLocal;
            double lMeterConsumed;
            bool lConsumedEstimated;
            double lMeterProduced;
            bool lProducedEstimated;
            double lPrice;
            double lConsumed;
            double lProduced;
            DataLine lDataLine;

            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileNameData + ".csv");
            if (File.Exists(lFileName)) {
                try {
                    lStreamIn = new StreamReader(lFileName);
                    do {
                        lLine = lStreamIn.ReadLine();
                        if (lLine == null) {
                            break;
                        }
                        lParts = lLine.Split(';');
                        if (lParts.Length >= 10) {
                            if (int.TryParse(lParts[0], out lVersion)) {
                                if (lVersion == 1) {
                                    if (DateTime.TryParse(lParts[1], mFileCulture, out lTimeStampUTC)) {
                                        lTimeStampUTC = DateTime.SpecifyKind(lTimeStampUTC, DateTimeKind.Utc);
                                        if (DateTime.TryParse(lParts[2], mFileCulture, out lTimeStampLocal)) {
                                            lTimeStampLocal = DateTime.SpecifyKind(lTimeStampLocal, DateTimeKind.Local);
                                            if (double.TryParse(lParts[3], NumberStyles.Any, mFileCulture, out lMeterConsumed)) {
                                                if (bool.TryParse(lParts[4], out lConsumedEstimated)) {
                                                    if (double.TryParse(lParts[5], NumberStyles.Any, mFileCulture, out lMeterProduced)) {
                                                        if (bool.TryParse(lParts[6], out lProducedEstimated)) {
                                                            if (double.TryParse(lParts[7], NumberStyles.Any, mFileCulture, out lPrice)) {
                                                                if (double.TryParse(lParts[8], NumberStyles.Any, mFileCulture, out lConsumed)) {
                                                                    if (double.TryParse(lParts[9], NumberStyles.Any, mFileCulture, out lProduced)) {
                                                                        lDataLine = new DataLine(lTimeStampUTC, lTimeStampLocal, lMeterConsumed, lConsumedEstimated, lMeterProduced, lProducedEstimated, lPrice, lConsumed, lProduced);
                                                                        mLines.Add(lDataLine);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    } while (true);
                    lStreamIn.Close();
                } catch (Exception ex) {
                    mLines.Clear();
                }
            }
        }

        internal void xSaveData() {
            string lFileName;

            if (mDataModified) {
                lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileNameData + ".csv");
                xWriteData(lFileName);
            }
        }

        internal void xWriteData(string pFileName) {
            StringBuilder lBuilder;
            StreamWriter lStreamOut;

            lStreamOut = new StreamWriter(pFileName, false);
            lStreamOut.WriteLine("Version;TimeStampUTC;TimeStampLocal;MeterConsumed;ConsumedEstimated;MeterProduced;ProducedEstimated;Price;Consumed;Produced");
            foreach (DataLine bLine in mLines) {
                lBuilder = new StringBuilder();
                lBuilder.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}",
                    cCurrentVersionBase,
                    bLine.xTimeStampUTC.ToString(mFileCulture),
                    bLine.xTimeStampLocal.ToString(mFileCulture),
                    bLine.xMeterConsumed.ToString(mFileCulture),
                    bLine.xConsumedEstimated.ToString(),
                    bLine.xMeterProduced.ToString(mFileCulture),
                    bLine.xProducedEstimated.ToString(),
                    bLine.xPrice.ToString(mFileCulture),
                    bLine.xConsumed.ToString(mFileCulture),
                    bLine.xProduced.ToString(mFileCulture)));
                lStreamOut.Write(lBuilder.ToString());
            }
            lStreamOut.Close();
        }

        private void sReadProviders() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            string lFileName;
            int lVersion;
            Provider lProvider;

            mProviders.Clear();
            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileNameProviders);
            if (File.Exists(lFileName)) {
                lDoc = new XmlDocument();
                lDoc.Load(lFileName);
                lRoot = lDoc.DocumentElement;
                if (lRoot != null) {
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "Version": {
                                        if (!int.TryParse(bNode.InnerText, out lVersion)){
                                            lVersion = 0;
                                        }
                                        break;
                                    }
                                case "Provider": {
                                        lProvider = sProcessProvider(bNode);
                                        if (lProvider != null) {
                                            mProviders.Add(lProvider);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        private Provider sProcessProvider(XmlNode pNode) {
            Provider lProvider;
            double lTemp;

            lProvider = new Provider();
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "Provider": {
                                lProvider.xProvider = bNode.InnerText;
                                break;
                            }
                        case "Variant": {
                                lProvider.xVariant = bNode.InnerText;
                                break;
                            }
                        case "MonthlyTariff": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xMonthlyTariff = lTemp;
                                }
                                break;
                            }
                        case "ConsumedFixedPrice": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xConsumedFixedPrice = lTemp;
                                }
                                break;
                            }
                        case "ConsumedExtra": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xConsumedExtra = lTemp;
                                }
                                break;
                            }
                        case "ProducedFixedPrice": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xProducedFixedPrice = lTemp;
                                }
                                break;
                            }
                        case "ProducedExtra": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xProducedExtra = lTemp;
                                }
                                break;
                            }
                    }
                }
            }
            return lProvider;
        }

        internal void xSaveProviders() {
            XmlDocument lDoc;
            XmlElement lRoot;
            XmlElement lProviderElement;
            XmlElement lEntry;
            XmlText lText;
            XmlAttribute lAttribute;

            if (mProvidersModified) {
                lDoc = new XmlDocument();
                lRoot = lDoc.CreateElement("Providers");
                lDoc.AppendChild(lRoot);
                lAttribute = lDoc.CreateAttribute("Version");
                lAttribute.Value = cCurrentVersionProviders.ToString();
                lRoot.Attributes.Append(lAttribute);
                foreach (Provider bProvider in mProviders) {
                    lProviderElement = lDoc.CreateElement("Provider");
                    lRoot.AppendChild(lProviderElement);

                    lEntry = lDoc.CreateElement("Provider");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xProvider);
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("Variant");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xVariant);
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("MonthlyTariff");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xMonthlyTariff.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ConsumedFixedPrice");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xConsumedFixedPrice.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ConsumedExtra");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xConsumedExtra.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ProducedFixedPrice");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xProducedFixedPrice.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ProducedExtra");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xProducedExtra.ToString());
                    lEntry.AppendChild(lText);
                }
                lDoc.Save(Path.Combine(Parameters.GetInstance.xDataDir, cFileNameProviders));
                mProvidersModified = false;
            }
        }

        private void sCalculateProviders() {
            foreach (Provider bProvider in mProviders) {
                bProvider.xImportLines(mLines, mTax);
            }
        }

        internal bool xProviderPresent(string pProvider, string pVariant) {
            bool lResult;

            lResult = false;
            foreach (Provider bProvider in mProviders) {
                if (bProvider.xProvider == pProvider && bProvider.xVariant == pVariant) {
                    lResult = true;
                    break;
                }
            }
            return lResult;
        }

        internal void xAddProvider(Provider pProvider) {
            mProviders.Add(pProvider);
            mProviders.Sort();
            mProvidersModified = true;
        }

        internal void xDeleteProvider(Provider pProvider) {
            mProviders.Remove(pProvider);
            mProvidersModified = true;
        }

        internal void xImportMeterFile(MeterFile pMeterConsumed, MeterFile pMeterProduced, PriceFile pPrices) {
            DateTime lCurrent;
            DataLine lLine;
            MeterLine[] lLinesConsumed;
            MeterLine[] lLinesProduced;
            PriceLine[] lPriceLines;
            int lIndex;
            int lIndexTotal;

            if (pMeterConsumed.xLastEntry > mEnd) {
                if (mStart > DateTime.MinValue) {
                    lCurrent = mEnd;
                } else {
                    mStart = pMeterConsumed.xFirstEntry;
                    lCurrent = mStart.AddMinutes(-15);
                }
                mEnd = pMeterConsumed.xLastEntry;
                do {
                    lCurrent = lCurrent.AddMinutes(15);
                    if (lCurrent > mEnd) {
                        break;
                    }
                    lLine = new DataLine(lCurrent);
                    mLines.Add(lLine);
                } while (true);
            }
            lLinesConsumed = pMeterConsumed.xLines;
            lLinesProduced = pMeterProduced.xLines;
            lPriceLines = pPrices.xLines;
            lIndexTotal = (int)((pMeterConsumed.xFirstEntry - mStart).TotalMinutes / 15);
            for (lIndex = 0; lIndex < lLinesConsumed.Length; lIndex++) {
                if (mLines[lIndexTotal].xConsumedEstimated) {
                    mLines[lIndexTotal].xMeterConsumed = lLinesConsumed[lIndex].xMeterValue;
                    mLines[lIndexTotal].xConsumedEstimated = lLinesConsumed[lIndex].xEstimated;
                    mLines[lIndexTotal].xConsumed = lLinesConsumed[lIndex].xPeriodValue;
                }
                if (mLines[lIndexTotal].xProducedEstimated) {
                    mLines[lIndexTotal].xMeterProduced = lLinesProduced[lIndex].xMeterValue;
                    mLines[lIndexTotal].xProducedEstimated = lLinesProduced[lIndex].xEstimated;
                    mLines[lIndexTotal].xProduced = lLinesProduced[lIndex].xPeriodValue;
                }
                mLines[lIndexTotal].xPrice = lPriceLines[lIndex].xValue;
                lIndexTotal++;
            }
            mDataModified = true;
            xCalculate();
        }

        internal void xCalculate() {
            double lLastBattery;

            lLastBattery = 0;
            foreach (DataLine bLine in mLines) {
                bLine.xCalculate(mSelectedProvider, mTax, mBattery, lLastBattery);
                lLastBattery = bLine.xBattery;
            }
            sCalculateProviders();
        }
    }
}
