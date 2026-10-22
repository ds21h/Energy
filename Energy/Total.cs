using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class Total {
        private protected int mYear;
        private protected int mMonth;
        private protected double mConsumed;
        private protected double mConsumedNet;
        private protected double mConsumedPrice;
        private protected double mConsumedPriceNet;
        private protected double mConsumedResortPrice;
        private protected double mProduced;
        private protected double mProducedNet;
        private protected double mProducedPrice;
        private protected double mTax;

        internal int xYear {
            get {
                return mYear;
            }
        }

        internal int xMonth {
            get {
                return mMonth;
            }
        }

        internal double xConsumed {
            get {
                return mConsumed;
            }
        }

        internal double xConsumedNet {
            get {
                return mConsumedNet;
            }
        }

        internal double xProduced {
            get {
                return mProduced;
            }
        }

        internal double xProducedNet {
            get {
                return mProducedNet;
            }
        }

        internal double xConsumedPriceNet {
            get {
                return mConsumedPriceNet;
            }
        }

        internal double xConsumedPrice {
            get {
                return mConsumedPrice;
            }
        }

        internal double xConsumedResortPrice {
            get {
                return mConsumedResortPrice;
            }
        }

        internal double xProducedPrice {
            get {
                return mProducedPrice;
            }
        }

        internal double xTax {
            get {
                return mTax;
            }
        }

        internal Total(int pYear, int pMonth) {
            mYear = pYear;
            mMonth = pMonth;
            mConsumed = 0;
            mConsumedNet = 0;
            mConsumedPrice = 0;
            mConsumedPriceNet = 0;
            mConsumedResortPrice = 0;
            mProduced = 0;
            mProducedNet = 0;
            mProducedPrice = 0;
            mTax = 0;
        }

        internal void xAddLine(DisplayLine pLine) {
            mConsumed += pLine.xConsumed;
            mConsumedNet += pLine.xNetConsumed;
            mConsumedPrice += pLine.xConsumedPrice;
            mConsumedPriceNet += pLine.xConsumedPriceNet;
            mConsumedResortPrice += pLine.xConsumedResortPrice;
            mProduced += pLine.xProduced;
            mProducedNet += pLine.xNetProduced;
            mProducedPrice += pLine.xProducedPrice;
            mTax += pLine.xTax;
        }
    }
}
