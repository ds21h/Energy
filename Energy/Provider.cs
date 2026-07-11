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
        private double mConsumedFixedPrice;
        private double mConsumedExtra;
        private double mProducedFixedPrice;
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

        internal Provider() {
            mProvider = "";
            mVariant = "";
            mTariff = 0;
            mConsumedFixedPrice = 0.0;
            mConsumedExtra = 0;
            mProducedFixedPrice = 0.0;
            mProducedExtra = 0.0;
        }

        internal Provider (string pProvider, string pVariant, double pMonthlyTariff, TariffPeriod pPeriod, double pConsumedFixedPrice, double pConsumedExtra, double pProducedFixedPrice, double pProducedExtra) {
            mProvider = pProvider;
            mVariant = pVariant;
            mTariff = pMonthlyTariff;
            mPeriod = pPeriod;
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
    }
}
