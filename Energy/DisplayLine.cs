using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class DisplayLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");
        DataLine mDataLine;
        private double mNetConsumed;
        private double mNetProduced;
        private double mBattery;
        private double mConsumedPrice;
        private double mProducedPrice;

        public string xTimeStampUTCStr {
            get {
                return mDataLine.xTimeStampUTC.ToString(mCulture);
            }
        }

        public string xTimeStampLocalStr {
            get {
                return mDataLine.xTimeStampLocal.ToString(mCulture);
            }
        }

        internal DateTime xTimeStampLocal {
            get {
                return mDataLine.xTimeStampLocal;
            }
        }

        public string xMeterConsumedStr {
            get {
                return mDataLine.xMeterConsumed.ToString("###,##0.000", mCulture);
            }
        }

        public bool xConsumedEstimated {
            get {
                return mDataLine.xConsumedEstimated;
            }
        }

        public string xMeterProducedStr {
            get {
                return mDataLine.xMeterProduced.ToString("###,##0.000", mCulture);
            }
        }

        public bool xProducedEstimated {
            get {
                return mDataLine.xProducedEstimated;
            }
        }

        public string xPriceStr {
            get {
                return mDataLine.xPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedStr {
            get {
                return mDataLine.xConsumed.ToString("###,##0.000", mCulture);
            }
        }

        internal double xConsumed {
            get {
                return mDataLine.xConsumed;
            }
        }

        public string xProducedStr {
            get {
                return mDataLine.xProduced.ToString("###,##0.000", mCulture);
            }
        }

        internal double xProduced {
            get {
                return mDataLine.xProduced;
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

        public string xConsumedPriceStr {
            get {
                return mConsumedPrice.ToString("##0.000", mCulture);
            }
        }

        internal double xConsumedPrice {
            get {
                return mConsumedPrice;
            }
        }

        public string xProducedPriceStr {
            get {
                return mProducedPrice.ToString("##0.000", mCulture);
            }
        }

        internal double xProducedPrice {
            get {
                return mProducedPrice;
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

        internal DisplayLine(Provider pProvider, DataLine pDataLine, double pTax, int pMaxBattery, double pLastBattery) {
            mDataLine = pDataLine;
            sCalculateBattery(pMaxBattery, pLastBattery);
            sCalculatePrices(pProvider, pTax);
        }

        private void sCalculatePrices(Provider pProvider, double pTax) {
            if (pProvider.xConsumedFixedPrice == 0) {
                mConsumedPrice = mNetConsumed * (mDataLine.xPrice + pProvider.xConsumedExtra + pTax);
            } else {
                mConsumedPrice = mNetConsumed * (pProvider.xConsumedFixedPrice + pProvider.xConsumedExtra + pTax);
            }
            if (pProvider.xProducedFixedPrice == 0) {
                mProducedPrice = mNetProduced * (mDataLine.xPrice - pProvider.xProducedExtra);
            } else {
                mProducedPrice = mNetProduced * (pProvider.xProducedFixedPrice - pProvider.xProducedExtra);
            }
        }

        private void sCalculateBattery(int pMaxBattery, double pLastBattery) {
            double lBattery;

            lBattery = pLastBattery + mDataLine.xProduced;
            if (lBattery > pMaxBattery) {
                mNetProduced = lBattery - pMaxBattery;
                lBattery = pMaxBattery;
            } else {
                mNetProduced = 0;
            }
            lBattery -= mDataLine.xConsumed;
            if (lBattery < 0) {
                mNetConsumed = -lBattery;
                lBattery = 0;
            } else {
                mNetConsumed = 0;
            }
            mBattery = lBattery;
        }
    }
}
