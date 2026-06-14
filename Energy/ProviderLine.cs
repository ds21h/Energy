using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class ProviderLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");
        Provider mProvider;
        DataLine mDataLine;
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

        public string xNetConsumedStr {
            get {
                return mDataLine.xNetConsumed.ToString("###,##0.000", mCulture);
            }
        }

        public Double xNetConsumed {
            get {
                return mDataLine.xNetConsumed;
            }
        }

        public string xNetProducedStr {
            get {
                return mDataLine.xNetProduced.ToString("###,##0.000", mCulture);
            }
        }

        public double xNetProduced {
            get {
                return mDataLine.xNetProduced;
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
                return mDataLine.xBattery;
            }
        }

        public string xBatteryStr {
            get {
                return mDataLine.xBattery.ToString("##0.000", mCulture);
            }
        }

        internal ProviderLine(Provider pProvider, DataLine pDataLine, double pTax) {
            mProvider = pProvider;
            mDataLine = pDataLine;
            if (pProvider.xConsumedFixedPrice == 0) {
                mConsumedPrice = mDataLine.xNetConsumed * (mDataLine.xPrice + mProvider.xConsumedExtra + pTax);
            } else {
                mConsumedPrice = mDataLine.xNetConsumed * (mProvider.xConsumedFixedPrice + mProvider.xConsumedExtra + pTax);
            }
            if (pProvider.xProducedFixedPrice == 0) {
                mProducedPrice = mDataLine.xNetProduced * (mDataLine.xPrice - mProvider.xProducedExtra);
            } else {
                mProducedPrice = mDataLine.xNetProduced * (mProvider.xProducedFixedPrice - mProvider.xProducedExtra);
            }
        }
    }
}
