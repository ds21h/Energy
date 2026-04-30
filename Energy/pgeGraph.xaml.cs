using System;
using System.Collections.Generic;
using System.Reflection;
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
    /// Interaction logic for pgeGraph.xaml
    /// </summary>

    public partial class pgeGraph: Page {

        private List<double> mChartData;

        public pgeGraph() {
            int lIndex;

            InitializeComponent();
            mChartData = new List<double>();
            for (lIndex = 0; lIndex < 600; lIndex++) {
                mChartData.Add(Data.getInstance.xLines[lIndex].xConsumed);
            }
            clmValues.Values = mChartData;
        }
    }
}
