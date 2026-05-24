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

        internal Provider(string pProvider) {
            mProvider = pProvider;
            mVariant = "";
            mMonthlyTariff = 8.5 * 100 / 121;
            mConsumedFixedPrice = 0.0;
            mConsumedExtra = 0.001488;
            mProducedFixedPrice = 0.0;
            mProducedExtra = 0.0;
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
