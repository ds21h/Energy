using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class Provider : IComparable<Provider> {
        internal enum TariffPeriod {
            Day,
            Month
        }
        private string mProvider;
        private string mVariant;
        private double mTariff;
        private TariffPeriod mPeriod;
        private double mConsumedFixedPriceHigh;
        private double mConsumedFixedPriceLow;
        private double mConsumedExtra;
        private double mProducedFixedPriceHigh;
        private double mProducedFixedPriceLow;
        private double mProducedExtra;

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

        internal double xTariff {
            get {
                return mTariff;
            }
            set {
                mTariff = value;
            }
        }

        internal TariffPeriod xPeriod {
            get {
                return mPeriod;
            }
            set {
                mPeriod = value;
            }
        }   

        internal double xConsumedFixedPriceHigh {
            get {
                return mConsumedFixedPriceHigh;
            }
            set {
                mConsumedFixedPriceHigh = value;
            }
        }

        internal double xConsumedFixedPriceLow {
            get {
                return mConsumedFixedPriceLow;
            }
            set {
                mConsumedFixedPriceLow = value;
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

        internal double xProducedFixedPriceHigh {
            get {
                return mProducedFixedPriceHigh;
            }
            set {
                mProducedFixedPriceHigh = value;
            }
        }

        internal double xProducedFixedPriceLow {
            get {
                return mProducedFixedPriceLow;
            }
            set {
                mProducedFixedPriceLow = value;
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

        internal Provider() {
            mProvider = "";
            mVariant = "";
            mTariff = 0;
            mConsumedFixedPriceHigh = 0.0;
            mConsumedFixedPriceLow = 0.0;
            mConsumedExtra = 0;
            mProducedFixedPriceHigh = 0.0;
            mProducedFixedPriceLow = 0.0;
            mProducedExtra = 0.0;
        }

        internal Provider (string pProvider, string pVariant, double pMonthlyTariff, TariffPeriod pPeriod, double pConsumedFixedPriceHigh, double pConsumedFixedPriceLow, double pConsumedExtra, double pProducedFixedPriceHigh, double pProducedFixedPriceLow, double pProducedExtra) {
            mProvider = pProvider;
            mVariant = pVariant;
            mTariff = pMonthlyTariff;
            mPeriod = pPeriod;
            mConsumedFixedPriceHigh = pConsumedFixedPriceHigh;
            mConsumedFixedPriceLow = pConsumedFixedPriceLow;
            mConsumedExtra = pConsumedExtra;
            mProducedFixedPriceHigh = pProducedFixedPriceHigh;
            mProducedFixedPriceLow = pProducedFixedPriceLow;
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
    }
}
