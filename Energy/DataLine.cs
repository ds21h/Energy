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
        }
    }
}
