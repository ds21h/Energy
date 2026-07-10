using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Energy {
    internal class Data {
        private static Data? mData;

        private ExtData mExtData;
        private Resort mResort;
        private Providers mProviders;
        private Situation mSituation;
        private Taxes mTaxes;
        private DisplayData? mDisplayData;

        internal ExtData xExtData {
            get {
                return mExtData;
            }
        }  

        internal Taxes xTaxes {
            get {
                return mTaxes;
            }
        }

        internal Situation xSituation {
            get {
                return mSituation;
            }
        }

        internal Resort xResort {
            get {
                return mResort;
            }
        }

        internal Providers xProviders {
            get {
                return mProviders;
            }
        }

        internal DisplayData xDisplayData {
            get {
                if (mDisplayData == null) {
                    mDisplayData = new DisplayData();
                }
                return mDisplayData;
            }
        }

        internal static Data getInstance {
            get {
                if (mData == null) {
                    mData = new Data();
                }
                return mData;
            }
        }

        private Data() {
            mExtData = new ExtData();
            mResort = new Resort();
            mProviders = new Providers();
            mSituation = new Situation();
            mTaxes = new Taxes();
            mDisplayData = null;
        }

        internal void xLoadSituation(string pName) {
            mSituation = new Situation(pName);
            mExtData = new ExtData(pName);
        }

        internal void xCalculate() {
            mDisplayData = new DisplayData();
        }
    }
}
