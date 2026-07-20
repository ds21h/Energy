using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class ResortTariff :IComparable<ResortTariff>, IComparable{
        private int mFases;
        private int lMax;
        private double mPriceDay;
        private double mPriceYear;

        public string xConnection {
            get {
                return mFases.ToString() + "*" + lMax.ToString() + "A";
            }
        }

        internal int xFases {
            get {
                return mFases;
            }
            set {
                mFases = value;
            }
        }

        internal int xMax {
            get {
                return lMax;
            }
            set {
                lMax = value;
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

        internal ResortTariff() {
            mFases = 0;
            lMax = 0;
            mPriceDay = 0;
            mPriceYear = 0;
        }

        internal ResortTariff(int pFases, int pMax, double pPriceDay, double pPriceYear) {
            mFases = pFases;
            lMax = pMax;
            mPriceDay = pPriceDay;
            mPriceYear = pPriceYear;
        }

        public int CompareTo(ResortTariff? pTariff) {
            int lResult;

            if (pTariff == null) {
                lResult = 1;
            } else {
                lResult = mFases.CompareTo(pTariff.mFases);
                if (lResult == 0) {
                    lResult = lMax.CompareTo(pTariff.lMax);
                }
            }

            return lResult;
        }

        int IComparable.CompareTo(object? pObject) {
            int lResult;

            if (pObject == null) {
                lResult = 1;
            } else if (pObject is ResortTariff pTariff) {
                lResult = CompareTo(pTariff);
            } else {
                lResult = 1;
            }
            return lResult;
        }
    }
}
