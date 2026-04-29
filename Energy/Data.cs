using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace Energy {
    internal class Data {
        private static Data? mData;

        private const string cFileName = "Standen";
        private const int cMinVersion = 1;
        private const int cCurrentVersion = 1;
        private CultureInfo mFileCulture = new CultureInfo("nl-NL");
        private List<DataLine> mLines = new List<DataLine>(50000);
        private DateTime mStart;
        private DateTime mEnd;
        private bool mModified;

        internal List<DataLine> xLines {
            get {
                return mLines;
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

        internal static Data getInstance {
            get {
                if (mData == null) {
                    mData = new Data();
                }
                return mData;
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
            mModified = false;
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

            lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileName + ".csv");
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

            if (mModified) {
                lFileName = Path.Combine(Parameters.GetInstance.xDataDir, cFileName + ".csv");
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
                    cCurrentVersion,
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
            mModified = true;
        }
    }
}
