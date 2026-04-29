using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Energy {
    internal class MeterFile {
        private CultureInfo mFileCulture = new CultureInfo("en-US");
        private DateTime mStart;
        private DateTime mEnd;
        private string mFileName;
        private MeterLine[] mLines;

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
            mStart = pStart;
            mEnd = pEnd.AddMinutes(15);
            sProcess();
        }

        private void sProcess() {
            sInit();
//            sExportTable(@"D:\Test\Energy\ExportFile01.csv");
            sReadFile();
//            sExportTable(@"D:\Test\Energy\ExportFile02.csv");
            sCorrectFile();
//            sExportTable(@"D:\Test\Energy\ExportFile03.csv");
            sCalculateConsumption();
//            sExportTable(@"D:\Test\Energy\ExportFile04.csv");
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

        private void sReadFile() {
            StreamReader lStreamIn;
            string? lLine;
            string[] lParts;
            DateTime lTimeStamp;
            int lIndex;
            double lValue;

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
                            }
                        }
                    }
                }
            } while (true);
            lStreamIn.Close();
        }

        private void sCorrectFile() {
            int lIndex;
            int lStart;
            int lStartSearch;
            int lEnd;
            int lEndSearch;
            int lCorrection;
            int lStatus;

            lStart = -1;
            for (lIndex = 0; lIndex < mLines.Length - 1; lIndex++) {
                if (mLines[lIndex].xEstimated) {
                    if (lStart < 0) {
                        lStart = lIndex - 1;
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
                                break;
                            }
                            lCorrection += 96;
                        } while (lStatus < 2);
                        lStart = -1;
                    }
                }
            }
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

            lTotal = mLines[lEnd].xMeterValue - mLines[lStart].xMeterValue;
            lTotalSource = mLines[lStartSource + (lEnd - lStart)].xMeterValue - mLines[lStartSource].xMeterValue;
            lCurrentValue = mLines[lStart].xMeterValue;
            lSourceIndex = lStartSource + 1;
            for (lIndex = lStart + 1; lIndex < lEnd; lIndex++) {
                lFactor = (mLines[lSourceIndex + 1].xMeterValue - mLines[lSourceIndex].xMeterValue) / lTotalSource;
                lCurrentValue += lTotal * lFactor;
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
