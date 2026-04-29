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
using System.Windows.Shapes;

namespace Energy {
    /// <summary>
    /// Interaction logic for frmLoad.xaml
    /// </summary>
    public partial class frmLoad: Window {
        private DateTime? mStart; 
        private DateTime? mEnd;

        internal DateTime? xStart {
            get {
                return mStart;
            }
        }

        internal DateTime? xEnd {
            get {
                return mEnd;
            }
        }

        public frmLoad(DateTime? pStart, DateTime? pEnd) {
            InitializeComponent();
            mStart = pStart;
            mEnd = pEnd;
            if (mStart != null) {
                dpStart.SelectedDate = mStart;
                dpEnd.SelectedDate = mEnd;
            }
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            if (dpStart.SelectedDate == null) {
                if (mStart != null) {
                    dpStart.SelectedDate = mStart;
                }
            } else {
                mStart = dpStart.SelectedDate;
            }
        }

        private void dpEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {
            if (dpEnd.SelectedDate == null) {
                if (mEnd != null) {
                    dpEnd.SelectedDate = mEnd;
                }
            } else {
                mEnd = dpEnd.SelectedDate;
            }       
        }

        private void ButtonLoad_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            this.Hide();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            this.Hide();
        }
    }
}
