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
            MonthTotal lMonthTotalYear;
            DayTotal lDayTotal = new DayTotal(0, 0, 0);
            DayTotal lDayTotalMonth;
            DisplayLine lDisplayLine;
            double lLastBattery;
            int lMaxBattery;

            lLastBattery = 0;
            lMaxBattery = Data.getInstance.xSituation.xBattery;
            mMonthTotals.Clear();
            if (pDataLines.Count > 0) {
                lYear = pDataLines[0].xTimeStampLocal.Year;
                lMonth = pDataLines[0].xTimeStampLocal.Month;
                lMonthTotalYear = new MonthTotal(lYear);    
                lDayTotalMonth = new DayTotal(lYear, lMonth);
                mDisplayLines = new List<DisplayLine>(pDataLines.Count);
                foreach (DataLine lDataLine in pDataLines) {
                    lYear = lDataLine.xTimeStampLocal.Year;
                    lMonth = lDataLine.xTimeStampLocal.Month;
                    lDay = lDataLine.xTimeStampLocal.Day;
                    if (!lMonthTotal.xIsCurrent(lYear, lMonth)) {
                        if (!lMonthTotalYear.xIsCurrent(lYear)) {
                            mMonthTotals.Add(lMonthTotal);
                            lMonthTotal = new MonthTotal(lYear);
                        }
                        lMonthTotal = new MonthTotal(lYear, lMonth);
                        mMonthTotals.Add(lMonthTotal);
                    }
                    if (!lDayTotal.xIsCurrent(lYear, lMonth, lDay)) {
                        if (!lDayTotalMonth.xIsCurrent(lYear, lMonth)) {
                            mDayTotals.Add(lDayTotalMonth);
                            lDayTotalMonth = new DayTotal(lYear, lMonth);
                        }
                        lDayTotal = new DayTotal(lYear, lMonth, lDay);
                        mDayTotals.Add(lDayTotal);
                    }
                    lDisplayLine = new DisplayLine(pProvider, lDataLine, pTax, lMaxBattery, lLastBattery);
                    mDisplayLines.Add(lDisplayLine);
                    lMonthTotal.xAddLine(lDisplayLine);
                    lMonthTotalYear.xAddLine(lDisplayLine);
                    lDayTotal.xAddLine(lDisplayLine);
                    lDayTotalMonth.xAddLine(lDisplayLine);
                }
                mDayTotals.Add(lDayTotalMonth);
                mMonthTotals.Add(lMonthTotalYear);
            }
        }
    }
}
