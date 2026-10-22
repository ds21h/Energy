using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class VarTariff {
        private struct TariffLine {
            internal int mStartHour;
            internal double mPrice;
            internal TariffLine(int startHour, double price) {
                mStartHour = startHour;
                mPrice = price;
            }
        }

        internal int xCount {
            get {
                return mTariffLines.Count;
            }
        }

        internal int xStartHour(int index) {
            return mTariffLines[index].mStartHour;
        }

        internal double xPrice(int index) {
            return mTariffLines[index].mPrice;
        }

        internal double xTariff(DateTime pTimeStamp) {
            double lResult = 0.0;
            int lIndex;

            for (lIndex = 0; lIndex < mTariffLines.Count; lIndex++) {
                if (pTimeStamp.Hour < mTariffLines[lIndex].mStartHour) {
                    break;
                } else {
                    lResult = mTariffLines[lIndex].mPrice;
                }
            }
            return lResult;
        }

        private List<TariffLine> mTariffLines;

        internal VarTariff() {
            mTariffLines = new List<TariffLine>();
            xFinishTariff();
        }

        internal void xClearTariff() {
            mTariffLines.Clear();
        }

        internal void xAddTariffLine(int startHour, double price) {
            mTariffLines.Add(new TariffLine(startHour, price));
        }

        internal void xFinishTariff() {
            mTariffLines.Sort((line1, line2) => line1.mStartHour.CompareTo(line2.mStartHour));
            if (mTariffLines.Count == 0) {
                mTariffLines.Add(new TariffLine(0, 0.0));
            } else {
                if (mTariffLines[0].mStartHour != 0) {
                    mTariffLines.Insert(0, new TariffLine(0, 0.0));
                }
            }
        }
    }
}
