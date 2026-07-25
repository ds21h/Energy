using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Energy {
    internal class Providers {
        private const string cFileNameProviders = "Providers.xml";
        private const int cCurrentVersion = 1;

        private List<Provider> mProviders = new List<Provider>();
        private Provider mSelectedProvider;
        private bool mProvidersModified;

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

        internal bool xProvidersChanged {
            set {
                mProvidersModified = value;
            }
        }

        internal Providers() {
            mProviders = new List<Provider>();
            mSelectedProvider = new Provider();
            mProvidersModified = false;
            sLoadProviders();
        }

        private void sLoadProviders() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            XmlNode? lElement;
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
                    lElement = lRoot.GetAttributeNode("Version");
                    if (lElement != null) {
                        if (!int.TryParse(lElement.InnerText, out lVersion)) {
                            lVersion = 0;
                        }
                    }
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
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
                        case "Tariff": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xTariff = lTemp;
                                }
                                break;
                            }
                        case "Period": {
                                if (bNode.InnerText == "Month") {
                                    lProvider.xPeriod = Provider.TariffPeriod.Month;
                                } else {
                                    lProvider.xPeriod = Provider.TariffPeriod.Day;
                                }
                                break;
                            }
                        case "ConsumedFixedPriceHigh": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xConsumedFixedPriceHigh = lTemp;
                                }
                                break;
                            }
                        case "ConsumedFixedPriceLow": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xConsumedFixedPriceLow = lTemp;
                                }
                                break;
                            }
                        case "ConsumedExtra": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xConsumedExtra = lTemp;
                                }
                                break;
                            }
                        case "ProducedFixedPriceHigh": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xProducedFixedPriceHigh = lTemp;
                                }
                                break;
                            }
                        case "ProducedFixedPriceLow": {
                                if (double.TryParse(bNode.InnerText, out lTemp)) {
                                    lProvider.xProducedFixedPriceLow = lTemp;
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
                lAttribute.Value = cCurrentVersion.ToString();
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

                    lEntry = lDoc.CreateElement("Tariff");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xTariff.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("Period");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xPeriod == Provider.TariffPeriod.Month ? "Month" : "Day");
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ConsumedFixedPriceHigh");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xConsumedFixedPriceHigh.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ConsumedFixedPriceLow");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xConsumedFixedPriceLow.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ConsumedExtra");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xConsumedExtra.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ProducedFixedPriceHigh");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xProducedFixedPriceHigh.ToString());
                    lEntry.AppendChild(lText);

                    lEntry = lDoc.CreateElement("ProducedFixedPriceLow");
                    lProviderElement.AppendChild(lEntry);
                    lText = lDoc.CreateTextNode(bProvider.xProducedFixedPriceLow.ToString());
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

        internal void xSelectProvider(string pLabel) {
            foreach (Provider bProvider in mProviders) {
                if (bProvider.xLabel == pLabel) {
                    mSelectedProvider = bProvider;
                    break;
                }
            }
        }
    }
}

