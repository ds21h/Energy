using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace Energy {
    internal class MeterFile {
        private CultureInfo mFileCulture = new CultureInfo("en-US");
        private DateTime mStart;
        private DateTime mEnd;
        private string mFileName;
        private MeterLine[] mLines;
        private bool mFileOK;

        internal bool xFileOK {
            get {
                return mFileOK;
            }
        }

        internal MeterLine[] xLines {
            get { 
                return new ArraySegment<MeterLine>(mLines, 0, mLines.Length - 1).ToArray();
            }
        }

        internal DateTime xFirstEntry {
            get {
                DateTime lResult;

                lResult = DateTime.MinValue;
                if (mLines.Length > 0) {
                    lResult = mLines[0].xTimeStamp;
                }
                return lResult;
            }
        }

        internal DateTime xLastEntry {
            get {
                DateTime lResult;
                lResult = DateTime.MinValue;
                if (mLines.Length > 0) {
                    lResult = mLines[mLines.Length - 2].xTimeStamp;
                }
                return lResult;
            }
        }

        internal MeterFile(string pFileName, DateTime pStart, DateTime pEnd) {
            mFileName = pFileName;
            mLines = new MeterLine[0];
            if (pStart < pEnd) {
                mFileOK = true;
                mStart = DateTime.SpecifyKind(pStart.ToUniversalTime(), DateTimeKind.Utc);
                mEnd = DateTime.SpecifyKind(pEnd.ToUniversalTime(), DateTimeKind.Utc).AddMinutes(15);
                mFileOK = sProcess();
            } else {
                mFileOK = false;
            }
        }

        private bool sProcess() {
            bool lResult;

            lResult = false;
            sInit();
            if (sReadFile()) {
                if (sCorrectFile()) {
                    sCalculateConsumption();
                    lResult = true;
                }
            }
            return lResult;
        }

        private void sInit() {
            int lIndex;
            DateTime lTimestamp;

            mLines = new MeterLine[(int)(mEnd - mStart).TotalMinutes/15];
            lTimestamp = mStart;
            for (lIndex = 0; lIndex < mLines.Length; lIndex++) {
                mLines[lIndex] = new MeterLine(lTimestamp);
                lTimestamp = lTimestamp.AddMinutes(15);
            }
        }

        private bool sReadFile() {
            StreamReader lStreamIn;
            string? lLine;
            string[] lParts;
            DateTime lTimeStamp;
            int lIndex;
            double lValue;
            bool lResult;

            lResult = false;
            try {
                lStreamIn = new StreamReader(mFileName);
                do {
                    lLine = lStreamIn.ReadLine();
                    if (lLine == null) {
                        break;
                    }
                    lParts = lLine.Split(',');
                    if (lParts.Length >= 2) {
                        lTimeStamp = sParseDate(lParts[0]);
                        if (lTimeStamp != DateTime.MinValue) {
                            lValue = sParseValue(lParts[1]);
                            if (lValue >= 0) {
                                lIndex = (int)((lTimeStamp - mStart).TotalMinutes / 15);
                                if (lIndex >= 0 && lIndex < mLines.Length) {
                                    mLines[lIndex].xMeterValue = lValue;
                                    mLines[lIndex].xEstimated = false;
                                    lResult = true;
                                }
                            }
                        }
                    }
                    if (!lResult) {
                        break;
                    }
                } while (true);
                lStreamIn.Close();
            } catch (Exception) {
                lResult = false;
            }
            return lResult;
        }

        private bool sCorrectFile() {
            int lIndex;
            int lStart;
            int lStartSearch;
            int lEnd;
            int lEndSearch;
            int lCorrection;
            int lStatus;
            MeterLine lLine;
            bool lResult;

            lStart = -1;
            lResult = false;
            if (mLines.Length > 0) {
                if (!mLines[0].xEstimated && !mLines[mLines.Length - 1].xEstimated) {
                    lResult = true;
                    for (lIndex = 0; lIndex < mLines.Length - 1; lIndex++) {
                        lLine = mLines[lIndex];
                        if (lLine.xEstimated) {
                            if (lStart < 0) {
                                lStart = lIndex - 1;
                                lResult = false;
                            }
                        } else {
                            if (lStart >= 0) {
                                lEnd = lIndex;
                                lStartSearch = lStart;
                                lEndSearch = lEnd;
                                lCorrection = 96;
                                do {
                                    lStatus = 0;
                                    lStartSearch = lStart - lCorrection;
                                    lEndSearch = lEnd - lCorrection;
                                    if (lStartSearch < 0) {
                                        lStatus++;
                                    } else {
                                        if (sTestRange(lStartSearch, lEndSearch)) {
                                            sCorrectLines(lStart, lEnd, lStartSearch);
                                            lResult = true;
                                            break;
                                        }
                                    }
                                    lStartSearch = lStart + lCorrection;
                                    lEndSearch = lEnd + lCorrection;
                                    if (lEndSearch >= mLines.Length) {
                                        lStatus++;
                                    }
                                    if (sTestRange(lStartSearch, lEndSearch)) {
                                        sCorrectLines(lStart, lEnd, lStartSearch);
                                        lResult = true;
                                        break;
                                    }
                                    lCorrection += 96;
                                } while (lStatus < 2);
                                lStart = -1;
                            }
                        }
                    }
                }
            }
            return lResult;
        }

        private bool sTestRange(int pStart, int pEnd) {
            int lIndex;
            bool lResult;

            lResult = true;
            if (pStart >= 0 && pEnd < mLines.Length - 1) {
                for (lIndex = pStart; lIndex < pEnd + 1; lIndex++) {
                    if (mLines[lIndex].xEstimated) {
                        lResult = false;
                        break;
                    }
                }
            }
            return lResult;
        }

        private void sCorrectLines(int lStart, int lEnd, int lStartSource) {
            int lIndex;
            int lSourceIndex;
            double lTotal;
            double lTotalSource;
            double lCurrentValue;
            double lFactor;
            double lChange;

            lTotal = mLines[lEnd].xMeterValue - mLines[lStart].xMeterValue;
            lTotalSource = mLines[lStartSource + (lEnd - lStart)].xMeterValue - mLines[lStartSource].xMeterValue;
            lCurrentValue = mLines[lStart].xMeterValue;
            lSourceIndex = lStartSource + 1;
            for (lIndex = lStart + 1; lIndex < lEnd; lIndex++) {
                if (lTotalSource == 0) {
                    lChange = lTotal/(lEnd - lStart);
                } else {
                    lFactor = (mLines[lSourceIndex + 1].xMeterValue - mLines[lSourceIndex].xMeterValue) / lTotalSource;
                    lChange = lTotal * lFactor;
                }
                lCurrentValue += lChange;
                mLines[lIndex].xMeterValue = lCurrentValue;
                lSourceIndex++;
            }
        }

        private void sCalculateConsumption() {
            int lIndex;

            for (lIndex = 0; lIndex < mLines.Length - 1; lIndex++) {
                mLines[lIndex].xPeriodValue = mLines[lIndex + 1].xMeterValue - mLines[lIndex].xMeterValue;
            }
        }

        private DateTime sParseDate(string pDate) {
            DateTimeOffset lResultOffset;
            DateTime lResult;

            if (DateTimeOffset.TryParse(pDate, mFileCulture, out lResultOffset)) {
                lResult = lResultOffset.ToUniversalTime().DateTime;
                lResult = DateTime.SpecifyKind(lResult, DateTimeKind.Utc);
            } else {
                lResult = DateTime.MinValue;
            }
            return lResult;
        }

        private double sParseValue(string pValue) {
            double lResult;

            if (!Double.TryParse(pValue, mFileCulture, out lResult)) {
                lResult = -1;
            }
            return lResult;
        }

/*        internal void sExportTable(string pFileName) {
            StringBuilder lBuilder;
            CultureInfo lCulture = new CultureInfo("nl-NL");

            lBuilder = new StringBuilder();
            lBuilder.AppendLine("Timestamp;Value;Consumed;Estimated");
            foreach (MeterLine bLine in mLines) {
                lBuilder.AppendLine(string.Format("{0};{1};{2};{3}",
                    bLine.xTimeStamp.ToString(),
                    bLine.xMeterValue.ToString(),
                    bLine.xPeriodValue.ToString(),
                    bLine.xEstimated));
            }
            System.IO.File.WriteAllText(pFileName, lBuilder.ToString());
        } */
    }
}
