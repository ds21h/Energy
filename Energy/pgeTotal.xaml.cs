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
    public partial class pgeTotal: Page {
        private ObservableCollection<TotalLine> mTotalLines;

        public pgeTotal() {
            InitializeComponent();
            mTotalLines = new ObservableCollection<TotalLine>();
            lstTotals.ItemsSource = mTotalLines;
            xRefresh();
        }

        internal void xRefresh() {
            mTotalLines.Clear();
            foreach (MonthTotal lMonthTotal in Data.getInstance.xSelectedProvider.xMonthTotals) {
                mTotalLines.Add(new TotalLine(lMonthTotal));
            }
        }   
    }
}
