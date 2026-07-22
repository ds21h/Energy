using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Energy {
    internal class Situations {
        private List<Situation> mSituations;
        private Situation mSelectedSituation;

        internal List<Situation> xSituations {
            get {
                return mSituations;
            }
        }

        internal Situation xSelectedSituation {
            get {
                return mSelectedSituation;
            }
        }

        public Situations() {
            mSituations = new List<Situation>();
            mSelectedSituation = new Situation();
            sLoadSituations();
        }

        private void sLoadSituations() {
            DirectoryInfo lDirInfo;

            mSituations.Clear();
            lDirInfo = new DirectoryInfo(Parameters.GetInstance.xDataDir);
            if (lDirInfo.Exists) {
                foreach (DirectoryInfo lSubDir in lDirInfo.GetDirectories()) {
                    mSituations.Add(new Situation(lSubDir.Name));
                }
            }
        }

        internal void xSelectSituation(string pName) {
            foreach (Situation bSituation in mSituations) {
                if (bSituation.xName == pName) {
                    mSelectedSituation = bSituation;
                    break;
                }
            }
        }

        internal bool xAddSituation(string pName) {
            bool lResult;

            lResult = false;
            foreach (Situation bSituation in mSituations) {
                if (bSituation.xName == pName) {
                    mSelectedSituation = bSituation;
                    lResult = true;
                    break;
                }
            }
            if (!lResult) {
                mSelectedSituation = new Situation(pName, true);
                mSituations.Add(mSelectedSituation);
                lResult = true;
            }
            return false;
        }
    }
}
