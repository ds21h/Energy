using System;
using System.Collections.Generic;
using System.Text;

namespace Energy {
    internal class DisplayData {
        private List<DisplayLine> mDisplayLines = new List<DisplayLine>();
        private List<MonthTotal> mMonthTotals = new List<MonthTotal>();
        private List<DayTotal> mDayTotals = new List<DayTotal>();

        internal List<DisplayLine> xDisplayLines {
            get {
                return mDisplayLines;
            }
        }

        internal List<MonthTotal> xMonthTotals {
            get {
                return mMonthTotals;
            }
        }

        internal List<DayTotal> xDayTotals {
            get {
                return mDayTotals;
            }
        }

        internal DisplayData() {
            Provider lProvider;
            List<DataLine> lLines;
            double lTax;

            lProvider = Data.getInstance.xProviders.xSelectedProvider;
            lLines = Data.getInstance.xExtData.xLines;
            lTax = Data.getInstance.xTaxes.xTax;
            sProcessLines(lProvider, lLines, lTax);
        }

        private void sProcessLines(Provider pProvider,List<DataLine> pDataLines, double pTax) {
            int lYear;
            int lMonth;
            int lDay;
            MonthTotal lMonthTotal = new MonthTotal(0, 0);
            DayTotal lDayTotal = new DayTotal(0, 0, 0);
            DisplayLine lDisplayLine;
            double lLastBattery;
            int lMaxBattery;

            lLastBattery = 0;
            lMaxBattery = Data.getInstance.xSituation.xBattery;
            mMonthTotals.Clear();
            mDisplayLines = new List<DisplayLine>(pDataLines.Count);
            foreach (DataLine lDataLine in pDataLines) {
                lYear = lDataLine.xTimeStampLocal.Year;
                lMonth = lDataLine.xTimeStampLocal.Month;
                lDay = lDataLine.xTimeStampLocal.Day;
                if (!lMonthTotal.xIsCurrent(lYear, lMonth)) {
                    lMonthTotal = new MonthTotal(lYear, lMonth);
                    mMonthTotals.Add(lMonthTotal);
                }
                if (!lDayTotal.xIsCurrent(lYear, lMonth, lDay)) {
                    lDayTotal = new DayTotal(lYear, lMonth, lDay);
                    mDayTotals.Add(lDayTotal);
                }
                lDisplayLine = new DisplayLine(pProvider, lDataLine, pTax, lMaxBattery, lLastBattery);
                mDisplayLines.Add(lDisplayLine);
                lMonthTotal.xAddLine(lDisplayLine);
                lDayTotal.xAddLine(lDisplayLine);
            }
        }
    }
}
