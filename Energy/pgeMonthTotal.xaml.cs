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
    /// Interaction logic for pgeTotal.xaml
    /// </summary>
    public partial class pgeMonthTotal: Page {
        private ObservableCollection<MonthTotalLine> mTotalLines;

        public pgeMonthTotal() {
            InitializeComponent();
            mTotalLines = new ObservableCollection<MonthTotalLine>();
            lstTotals.ItemsSource = mTotalLines;
            xRefresh();
        }

        internal void xRefresh() {
            mTotalLines.Clear();
            foreach (MonthTotal lMonthTotal in Data.getInstance.xDisplayData.xMonthTotals) {
                mTotalLines.Add(new MonthTotalLine(lMonthTotal));
            }
        }   
    }
}
