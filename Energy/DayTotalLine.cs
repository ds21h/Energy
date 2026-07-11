using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Energy {
    internal class DayTotalLine {
        private CultureInfo mCulture = new CultureInfo("nl-NL");

        DayTotal mDayTotal;

        public string xYear {
            get {
                return mDayTotal.xYear.ToString();
            }
        }

        public string xMonth {
            get {
                return mDayTotal.xMonth.ToString();
            }
        }

        public string xDay {
            get {
                string lResult;

                if (mDayTotal.xIsTotal) {
                    lResult = "Totaal";
                } else {
                    lResult = mDayTotal.xDay.ToString();
                }
                return lResult;
            }
        }

        public string xConsumed {
            get {
                return mDayTotal.xConsumed.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedNet {
            get {
                return mDayTotal.xConsumedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xProduced {
            get {
                return mDayTotal.xProduced.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedNet {
            get {
                return mDayTotal.xProducedNet.ToString("###,##0.000", mCulture);
            }
        }

        public string xConsumedPrice {
            get {
                return mDayTotal.xConsumedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xProducedPrice {
            get {
                return mDayTotal.xProducedPrice.ToString("###,##0.000", mCulture);
            }
        }

        public string xTotalPriceExVAT {
            get {
                return (mDayTotal.xConsumedPrice - mDayTotal.xProducedPrice).ToString("###,##0.000", mCulture);
            }
        }

        private double sTotalPriceInVAT {
            get {
                return (mDayTotal.xConsumedPrice * 1.21) - mDayTotal.xProducedPrice;
                ;
            }
        }

        public string xTotalPriceInVAT {
            get {
                return sTotalPriceInVAT.ToString("###,##0.000", mCulture);
            }
        }

        public string xPriceKWhCons {
            get {
                string lResult;

                if (mDayTotal.xConsumedNet == 0) {
                    lResult = "0,000";
                } else {
                    lResult = ((mDayTotal.xConsumedPrice * 1.21) / mDayTotal.xConsumedNet).ToString("###,##0.000", mCulture);
                }   
                return lResult;
            }
        }

        public string xPriceKWhProd {
            get {
                string lResult;

                if (mDayTotal.xProducedNet == 0) {
                    lResult = "0,000";
                } else {
                    lResult = (mDayTotal.xProducedPrice / mDayTotal.xProducedNet).ToString("###,##0.000", mCulture);
                }
                return lResult;
            }
        }

        public bool xIsTotal {
            get {
                return mDayTotal.xIsTotal;
            }
        }

        internal DayTotalLine(DayTotal pDayTotal) {
            mDayTotal = pDayTotal;
        }
    }
}
