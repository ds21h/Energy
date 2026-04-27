using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    class PriceLine {
        private DateTime mTimeStamp;
        private double mValue;
        private bool mProvided;

        internal DateTime xTimeStamp {
            get {
                return mTimeStamp;
            }
        }

        internal double xValue {
            get {
                return mValue;
            }
            set {
                mValue = value;
            }
        }

        internal bool xProvided {
            get {
                return mProvided;
            }
            set {
                mProvided = value;
            }
        }

        internal PriceLine(DateTime pTimeStamp) {
            mTimeStamp = pTimeStamp;
            mValue = 0;
            mProvided = false;
        }
    }
}
