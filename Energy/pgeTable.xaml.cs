using System;
using System.Collections.Generic;
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
        public pgeTable() {
            InitializeComponent();
            xRefresh();
        }

        internal void xRefresh() {
            lstIntervals.ItemsSource = Data.getInstance.xLines;
            lstIntervals.InvalidateVisual();
        }
    }
}
