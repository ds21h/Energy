using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class ResortTariff :IComparable<ResortTariff>, IComparable{
        private int mFases;
        private int lMax;
        private double mPriceDay;
        private double mPriceYear;
        VarTariff mSummerTariff;
        VarTariff mWinterTariff;

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

        internal int xSummerCount {
            get {
                return mSummerTariff.xCount;
            }
        }

        internal int xWinterCount {
            get {
                return mWinterTariff.xCount;
            }
        }

        internal double xSummerTariff(DateTime pTimeStamp) {
            return mSummerTariff.xTariff(pTimeStamp);
        }

        internal double xWinterTariff(DateTime pTimeStamp) {
            return mWinterTariff.xTariff(pTimeStamp);
        }

        internal int xSummerStartHour(int pIndex) {
            return mSummerTariff.xStartHour(pIndex);
        }
        
        internal int xWinterStartHour(int pIndex) {
            return mWinterTariff.xStartHour(pIndex);
        }

        internal double xSummerPrice(int pIndex) {
            return mSummerTariff.xPrice(pIndex);
        }

        internal double xWinterPrice(int pIndex) {
            return mWinterTariff.xPrice(pIndex);
        }

        internal ResortTariff() {
            mFases = 0;
            lMax = 0;
            mPriceDay = 0;
            mPriceYear = 0;
            mSummerTariff = new VarTariff();
            mWinterTariff = new VarTariff();
        }

        internal ResortTariff(int pFases, int pMax, double pPriceDay, double pPriceYear) {
            mFases = pFases;
            lMax = pMax;
            mPriceDay = pPriceDay;
            mPriceYear = pPriceYear;
            mSummerTariff = new VarTariff();
            mWinterTariff = new VarTariff();
        }

        internal void xAddVarTariffs(VarTariff pSummerTariff, VarTariff pWinterTariff) {
            mSummerTariff = pSummerTariff;
            mWinterTariff = pWinterTariff;
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
