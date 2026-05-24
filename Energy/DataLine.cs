using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class DataLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");

        private DateTime mTimeStampUTC;
        private DateTime mTimeStampLocal;
        private double mMeterConsumed;
        private bool mConsumedEstimated;
        private double mMeterProduced;
        private bool mProducedEstimated;
        private double mPrice;
        private double mConsumed;
        private double mProduced;
        private double mNetConsumed;
        private double mNetProduced;
        private double mConsumedPrice;
        private double mProducedPrice;  
        private double mBattery;

        public string xTimeStampUTCStr {
            get {
                return mTimeStampUTC.ToString(mCulture);
            }
        }

        internal DateTime xTimeStampUTC {
            get {
                return mTimeStampUTC;
            }
        }

        public string xTimeStampLocalStr {
            get {
                return mTimeStampLocal.ToString(mCulture);
            }
        }

        internal DateTime xTimeStampLocal {
            get {
                return mTimeStampLocal;
            }
        }

        public string xMeterConsumedStr {
            get {
                return mMeterConsumed.ToString("###,##0.000", mCulture);
            }
        }

        internal double xMeterConsumed {
            get {
                return mMeterConsumed;
            }
            set {
                mMeterConsumed = value;
            }
        }

        public bool xConsumedEstimated {
            get {
                return mConsumedEstimated;
            }
            set {
                mConsumedEstimated = value;
            }
        }

        public string xMeterProducedStr {
            get {
                return mMeterProduced.ToString("###,##0.000", mCulture);
            }
        }

        internal double xMeterProduced {
            get {
                return mMeterProduced;
            }
            set {
                mMeterProduced = value;
            }
        }

        public bool xProducedEstimated {
            get {
                return mProducedEstimated;
            }
            set {
                mProducedEstimated = value;
            }
        }

        public string xPriceStr {
            get {
                return mPrice.ToString("###,##0.000", mCulture);
            }
        }

        internal double xPrice {
            get {
                return mPrice;
            }
            set {
                mPrice = value;
            }
        }

        public string xConsumedStr {
            get {
                return mConsumed.ToString("###,##0.000", mCulture);
            }
        }

        internal double xConsumed {
            get {
                return mConsumed;
            }
            set {
                mConsumed = value;
            }
        }

        public string xProducedStr {
            get {
                return mProduced.ToString("###,##0.000", mCulture);
            }
        }

        internal double xProduced {
            get {
                return mProduced;
            }
            set {
                mProduced = value;
            }
        }

        internal double xNetConsumed {
            get {
                return mNetConsumed;
            }
        }

        public string xNetConsumedStr {
            get {
                return mNetConsumed.ToString("###,##0.000", mCulture);
            }
        }

        internal double xNetProduced {
            get {
                return mNetProduced;
            }
        }

        public string xNetProducedStr {
            get {
                return mNetProduced.ToString("###,##0.000", mCulture);
            }
        }

        internal double xConsumedPrice {
            get {
                return mConsumedPrice;
            }
        }

        public string xConsumedPriceStr {
            get {
                return mConsumedPrice.ToString("##0.000", mCulture);
            }
        }

        internal double xProducedPrice {
            get {
                return mProducedPrice;
            }
        }

        public string xProducedPriceStr {
            get {
                return mProducedPrice.ToString("##0.000", mCulture);
            }
        }

        internal double xBattery {
            get {
                return mBattery;
            }
        }

        public string xBatteryStr {
            get {
                return mBattery.ToString("##0.000", mCulture);
            }
        }

        internal void xCalculate(Provider pProvider, double pTax, int pMaxBattery, double pLastBattery) {
            sCalculateBattery(pMaxBattery, pLastBattery);
            sCalculatePrices(pProvider, pTax);
        }

        private void sCalculateBattery(int pMaxBattery, double pLastBattery) {
            double lBattery;

            lBattery = pLastBattery + mProduced - mConsumed;
            if (lBattery < 0) {
                mNetConsumed = -lBattery;
                mNetProduced = 0;
                lBattery = 0;
            } else {
                if (lBattery > pMaxBattery) {
                    mNetConsumed = 0;
                    mNetProduced = lBattery - pMaxBattery;
                    lBattery = pMaxBattery;
                } else {
                    mNetConsumed = 0;
                    mNetProduced = 0;
                }
            }
            mBattery = lBattery;
        }

        private void sCalculatePrices(Provider pProvider, double pTax) {
            if (pProvider.xConsumedFixedPrice == 0) {
                mConsumedPrice = mNetConsumed * (mPrice + pProvider.xConsumedExtra + pTax);
            } else {
                mConsumedPrice = mNetConsumed * (pProvider.xConsumedFixedPrice + pProvider.xConsumedExtra + pTax);
            }
            if (pProvider.xProducedFixedPrice == 0) {
                mProducedPrice = mNetProduced * (mPrice + pProvider.xProducedExtra);
            } else {
                mProducedPrice = mNetProduced * (pProvider.xProducedFixedPrice + pProvider.xProducedExtra);
            }
        }

        internal DataLine(DateTime pTimeStampUTC, DateTime pTimeStampLocal, double pMeterConsumed, bool pConsumedEstimated, double pMeterProduced, bool pProducedEstimated, double pPrice, double pConsumed, double pProduced) {
            mTimeStampUTC = pTimeStampUTC;
            mTimeStampLocal = pTimeStampLocal;
            mMeterConsumed = pMeterConsumed;
            mConsumedEstimated = pConsumedEstimated;
            mMeterProduced = pMeterProduced;
            mProducedEstimated = pProducedEstimated;
            mPrice = pPrice;
            mConsumed = pConsumed;
            mProduced = pProduced;
            mNetConsumed = 0;
            mNetProduced = 0;
            mConsumedPrice = 0;
            mProducedPrice = 0;
            mBattery = 0;
        }

        internal DataLine(DateTime pTimeStampUTC) {
            mTimeStampUTC = pTimeStampUTC;
            mTimeStampLocal = pTimeStampUTC.ToLocalTime();
            mMeterConsumed = 0;
            mConsumedEstimated = true;
            mMeterProduced = 0;
            mProducedEstimated = true;
            mPrice = 0;
            mConsumed = 0;
            mProduced = 0;
            mNetConsumed = 0;
            mNetProduced = 0;
            mConsumedPrice = 0;
            mProducedPrice = 0;
            mBattery = 0;
        }
    }
}
