using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class Provider : IComparable<Provider> {
        private string mProvider;
        private string mVariant;
        private double mMonthlyTariff;
        private double mConsumedFixedPrice;
        private double mConsumedExtra;
        private double mProducedFixedPrice;
        private double mProducedExtra;
        private List<ProviderLine> mProviderLines = new List<ProviderLine>();
        private List<MonthTotal> mMonthTotals = new List<MonthTotal>();

        public String xLabel {
            get {
                return mVariant == "" ? mProvider : $"{mProvider} ({mVariant})";
            }
        }

        public string xProvider {
            get {
                return mProvider;
            }
            internal set {
                mProvider = value;  
            }
        }

        public string xVariant {
            get {
                return mVariant;
            }
            internal set {
                mVariant = value;
            }
        }

        internal double xMonthlyTariff {
            get {
                return mMonthlyTariff;
            }
            set {
                mMonthlyTariff = value;
            }
        }

        internal double xConsumedFixedPrice {
            get {
                return mConsumedFixedPrice;
            }
            set {
                mConsumedFixedPrice = value;
            }
        }

        internal double xConsumedExtra {
            get {
                return mConsumedExtra;
            }
            set {
                mConsumedExtra = value;
            }
        }

        internal double xProducedFixedPrice {
            get {
                return mProducedFixedPrice;
            }
            set {
                mProducedFixedPrice = value;
            }
        }

        internal double xProducedExtra {
            get {
                return mProducedExtra;
            }
            set {
                mProducedExtra = value;
            }
        }

        internal List<ProviderLine> xProviderLines {
            get {
                return mProviderLines;
            }
        }

        internal List<MonthTotal> xMonthTotals {
            get {
                return mMonthTotals;
            }
        }

        internal Provider() {
            mProvider = "";
            mVariant = "";
            mMonthlyTariff = 0;
            mConsumedFixedPrice = 0.0;
            mConsumedExtra = 0;
            mProducedFixedPrice = 0.0;
            mProducedExtra = 0.0;
        }

        internal Provider (string pProvider, string pVariant, double pMonthlyTariff, double pConsumedFixedPrice, double pConsumedExtra, double pProducedFixedPrice, double pProducedExtra) {
            mProvider = pProvider;
            mVariant = pVariant;
            mMonthlyTariff = pMonthlyTariff;
            mConsumedFixedPrice = pConsumedFixedPrice;
            mConsumedExtra = pConsumedExtra;
            mProducedFixedPrice = pProducedFixedPrice;
            mProducedExtra = pProducedExtra;
        }

        public int CompareTo(Provider? pProvider2) {
            int lResult;

            if (pProvider2 == null) {
                lResult = 1;
            } else {
                lResult = string.Compare(mProvider, pProvider2.mProvider, StringComparison.OrdinalIgnoreCase);
                if (lResult == 0) {
                    lResult = string.Compare(mVariant, pProvider2.mVariant, StringComparison.OrdinalIgnoreCase);
                }
            }
            return lResult;
        }

        internal void xImportLines(List<DataLine> pDataLines, double pTax) {
            int lYear;
            int lMonth;
            MonthTotal lMonthTotal = new MonthTotal(0, 0);
            ProviderLine lProviderLine;

            mMonthTotals.Clear();
            mProviderLines = new List<ProviderLine>(pDataLines.Count);
            foreach (DataLine lDataLine in pDataLines) {
                lYear = lDataLine.xTimeStampLocal.Year;
                lMonth = lDataLine.xTimeStampLocal.Month;
                if (!lMonthTotal.xIsCurrent(lYear, lMonth)) {
                    lMonthTotal = new MonthTotal(lYear, lMonth);
                    mMonthTotals.Add(lMonthTotal);
                }
                lProviderLine = new ProviderLine(this, lDataLine, pTax);
                mProviderLines.Add(lProviderLine);
                lMonthTotal.xAddLine(lProviderLine);
            }
        }   
    }
}
