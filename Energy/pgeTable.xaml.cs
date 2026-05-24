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
    /// Interaction logic for pgeTable.xaml
    /// </summary>
    public partial class pgeTable: Page {
        private ObservableCollection<DataLine> mDataLines;
        public pgeTable() {
            InitializeComponent();
            mDataLines = new ObservableCollection<DataLine>(Data.getInstance.xLines);
            lstIntervals.ItemsSource = mDataLines;
        }

        internal void xRefresh() {
            mDataLines.Clear();
            foreach (DataLine lLine in Data.getInstance.xLines) {
                mDataLines.Add(lLine);
            }
        }
    }
}
