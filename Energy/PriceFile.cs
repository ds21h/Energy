using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;

namespace Energy {
    class PriceFile {
        private struct strInterval {
            internal DateTime xStart;
            internal DateTime xEnd;
            internal bool xValid;
        }
        private CultureInfo mPriceCulture = new CultureInfo("en-US");

        private DateTime mStart;
        private DateTime mEnd;
        private string mFileName;
        private PriceLine[] mLines = new PriceLine[0];

        internal PriceLine[] xLines {
            get {
                return mLines;
            }
        }

        internal PriceFile(string pFileName, DateTime pStart, DateTime pEnd) {
            mFileName = pFileName;
            mStart = DateTime.SpecifyKind(pStart.ToUniversalTime(), DateTimeKind.Utc);
            mEnd = DateTime.SpecifyKind(pEnd.ToUniversalTime(), DateTimeKind.Utc);
            sInitLines();
            sProcessFile(pFileName);
            sCorrectPriceLines();
        }

        private void sInitLines() {
            int lIndex;
            DateTime lTimestamp;

            mLines = new PriceLine[(int)(mEnd - mStart).TotalMinutes / 15];
            lTimestamp = mStart;
            for (lIndex = 0; lIndex < mLines.Length; lIndex++) {
                mLines[lIndex] = new PriceLine(lTimestamp);
                lTimestamp = lTimestamp.AddMinutes(15);
            }
        }

        private void sProcessFile(string pFileName) {
            XmlDocument lDoc;
            XmlElement? lRoot;
            strInterval lInterval;

            if (File.Exists(pFileName)) {
                lDoc = new XmlDocument();
                lDoc.Load(pFileName);
                lRoot = lDoc.DocumentElement;
                if (lRoot != null) {
                    foreach (XmlNode bNode in lRoot.ChildNodes) {
                        if (bNode.NodeType != XmlNodeType.Comment) {
                            switch (bNode.Name) {
                                case "period.timeInterval": {
                                        lInterval = sProcessInterval(bNode);
                                        break;
                                    }
                                case "TimeSeries": {
                                        sProcessTimeSeries(bNode);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        private strInterval sProcessInterval(XmlNode pNode) {
            strInterval lInterval;
            DateTimeOffset lResultOffset;
            int lCount;

            lInterval = new strInterval();
            lCount = 0;
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "start": {
                                if (DateTimeOffset.TryParse(bNode.InnerText, out lResultOffset)) {
                                    lInterval.xStart = lResultOffset.ToUniversalTime().DateTime;
                                    lInterval.xStart = DateTime.SpecifyKind(lInterval.xStart, DateTimeKind.Utc);
                                    lCount++;
                                }
                                break;
                            }
                        case "end": {
                                if (DateTimeOffset.TryParse(bNode.InnerText, out lResultOffset)) {
                                    lInterval.xEnd = lResultOffset.ToUniversalTime().DateTime;
                                    lInterval.xEnd = DateTime.SpecifyKind(lInterval.xEnd, DateTimeKind.Utc);
                                    lCount++;
                                }
                                break;
                            }
                    }
                }
            }
            lInterval.xValid = (lCount == 2);
            return lInterval;
        }

        private void sProcessTimeSeries(XmlNode pNode) {
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "Period": {
                                sProcessPeriod(bNode);
                                break;
                            }
                    }
                }
            }
        }

        private void sProcessPeriod(XmlNode pNode) {
            strInterval lInterval;
            int lPosition;
            int lIndex;
            double lPrice;
            bool lValid = false;
            string lResolution;

            lInterval = new strInterval();
            foreach (XmlNode bNode in pNode.ChildNodes) {
                if (bNode.NodeType != XmlNodeType.Comment) {
                    switch (bNode.Name) {
                        case "timeInterval": {
                                lInterval = sProcessInterval(bNode);
                                if (lInterval.xStart <= mEnd) {
                                    if (lInterval.xEnd >= mStart) {
                                        lValid = true;
                                    }
                                }
                                break;
                            }
                        case "Point": {
                                if (lValid) {
                                    lPosition = -1;
                                    lPrice = 0;
                                    foreach (XmlNode bbNode in bNode.ChildNodes) {
                                        if (bNode.NodeType != XmlNodeType.Comment) {
                                            switch (bbNode.Name) {
                                                case "position": {
                                                        if (!int.TryParse(bbNode.InnerText, out lPosition)) {
                                                            lPosition = -1;
                                                        }
                                                        break;
                                                    }
                                                case "price.amount": {
                                                        if (!double.TryParse(bbNode.InnerText, mPriceCulture, out lPrice)) {
                                                            lPrice = 0;
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                    }
                                    lIndex = sCalculatePosition(lInterval.xStart, lPosition);
                                    if ((lIndex >= 0) && (lIndex < mLines.Length)) {
                                        if (!mLines[lIndex].xProvided) {
                                            mLines[lIndex].xValue = lPrice/1000;
                                            mLines[lIndex].xProvided = true;    
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }

        private int sCalculatePosition(DateTime pTimeStamp, int pPosition) {
            int lResult;
            DateTime lCurrent;

            lCurrent = pTimeStamp.AddMinutes((pPosition - 1) * 15);
            lResult = (int)((lCurrent - mStart).TotalMinutes / 15);
            return lResult;
        }

        private void sCorrectPriceLines() {
            int lIndex;

            for (lIndex = 0; lIndex < mLines.Length; lIndex++) {
                if (!mLines[lIndex].xProvided) {
                    if (lIndex > 0) {
                        mLines[lIndex].xValue = mLines[lIndex - 1].xValue;
                    } else {
                        mLines[lIndex].xValue = 0;
                    }
                }
            }
        }
    }
}
