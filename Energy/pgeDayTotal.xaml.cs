using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Energy {
    /// <summary>
    /// Interaction logic for pgeDayTotal.xaml
    /// </summary>
    public partial class pgeDayTotal: Page {
        private ObservableCollection<DayTotalLine> mTotalLines;

        public pgeDayTotal() {
            InitializeComponent();
            mTotalLines = new ObservableCollection<DayTotalLine>();
            lstTotals.ItemsSource = mTotalLines;
            xRefresh();
        }

        internal void xRefresh() {
            mTotalLines.Clear();
            foreach (DayTotal lDayTotal in Data.getInstance.xDisplayData.xDayTotals) {
                mTotalLines.Add(new DayTotalLine(lDayTotal));
            }
        }
    }
}
