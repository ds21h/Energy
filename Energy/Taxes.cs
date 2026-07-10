using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Energy {
    internal class Taxes {
        private const string cFileName = "Belasting.xml";
        private const int cCurrentVersion = 1;

        private double mTax;
        private double mReturn;

        internal double xTax {
            get {
                return mTax;
            }
        }

        internal double xReturn {
            get {
                return mReturn;
            }
        }

        internal Taxes() {
            mTax = 0.09161;
            mReturn = -1.42313;
            sLoadTaxes();
        }

        private void sLoadTaxes() {
            XmlDocument lDoc;
            XmlElement? lRoot;
            string lFileName;
            int lVersion;
            double lTemp;

            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileName);
            if (File.Exists(lFileName)) {
                lDoc = new XmlDocument();
                lDoc.Load(lFileName);
                lRoot = lDoc.DocumentElement;
                if (lRoot != null) {
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "Version": {
                                        if (!int.TryParse(bNode.InnerText, out lVersion)) {
                                            lVersion = 0;
                                        }
                                        break;
                                    }
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
    }
}
