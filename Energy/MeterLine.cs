using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class MeterLine {
        private DateTime mTimeStamp;
        private double mMeterValue;
        private double mPeriodValue;
        private bool mEstimated;

        internal DateTime xTimeStamp {
            get {
                return mTimeStamp; 
            }
        }

        internal double xMeterValue {
            get {
                return mMeterValue; 
            }
            set {
                mMeterValue = value;
            }
        }

        internal double xPeriodValue {
            get {
                return mPeriodValue; 
            }
            set {
                mPeriodValue = value;
            }
        }

        internal bool xEstimated {
            get {
                return mEstimated; 
            }
            set {
                mEstimated = value;
            }
        }

        internal MeterLine(DateTime pTimeStamp) {
            mTimeStamp = pTimeStamp;
            mMeterValue = 0;
            mPeriodValue = 0;
            mEstimated = true; 
        }

        internal MeterLine(DateTime pTimeStamp, double pValue) {
            mTimeStamp = pTimeStamp;
            mMeterValue = pValue;
            mPeriodValue = 0;
            mEstimated = true;
        }
    }
}
