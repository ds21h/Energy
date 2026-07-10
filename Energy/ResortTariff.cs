using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class ResortTariff {
        private string mConnection;
        private double mPriceDay;
        private double mPriceYear;

        internal string xConnection {
            get {
                return mConnection;
            }
            set {
                mConnection = value;
            }
        }

        internal double xPriceDay {
            get {
                return mPriceDay;
            }
            set {
                mPriceDay = value;
            }
        }

        internal double xPriceYear {
            get {
                return mPriceYear;
            }
            set {
                mPriceYear = value;
            }
        }

        public ResortTariff() {
            mConnection = "";
            mPriceDay = 0;
            mPriceYear = 0;
        }
    }
}
