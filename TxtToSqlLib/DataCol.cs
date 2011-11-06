using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace TxtToSql {
    public class DataCol {
        private object vals_;

        public DataCol(string fname) {
            Name = fname;
            HasNulls = false;
            ValCount = new Dictionary<string, int>();
            DataType = typeof(int);
        }

        #region simple props
        public string Name { get; private set; }
        public Type DataType { get; private set; }
        public bool HasNulls { get; private set; }
        public Dictionary<string, int> ValCount { get; private set; }
        public int NumVals { get { return ValCount.Count; } }
        public object MaxVal() {
            if (DataType == typeof(int)) return Ints().Max();
            return DataType == typeof(double) ? Doubles().Max() : null;
        }
        public object MinVal() {
            if (DataType == typeof(int)) return Ints().Min();
            return DataType == typeof(double) ? Doubles().Min() : null;
        }
        private IEnumerable<double?> Doubles() { return (IEnumerable<double?>)Vals_; }
        private IEnumerable<int?> Ints() { return (IEnumerable<int?>)Vals_; }
        #endregion

        public object Vals_ {
            get {
                if (DataType == typeof(int)) {
                    if (vals_ == null) vals_ = ValCount.Select(o => o.Key == "<NULL>" ? null : (int?)Convert.ToInt32(o.Key)).ToArray();
                } else if (DataType == typeof(double)) {
                    if (vals_ == null) vals_ = ValCount.Select(o => o.Key == "<NULL>" ? null : (double?)Convert.ToDouble(o.Key)).ToArray();
                }
                return vals_;
            }
        }

        public string CreateSQL {
            get {
                var name = Regex.Replace(Name, "[: ;\"'().]", "_");
                name = Regex.Replace(name, "__*", "_");
                var res = String.Format("{0} {1}", name, SqlType);
                if (!HasNulls) res += " NOT";
                res += " NULL";
                return res;
            }
        }

        protected string SqlType {
            get {
                if (DataType == typeof(String)) return string.Format("TEXT");
                if (DataType == typeof(DateTime)) return string.Format("TIMESTAMP");
                if (DataType == typeof(double)) return string.Format("REAL");
                if (DataType == typeof(int)) return string.Format("INT");
                throw new InvalidOperationException("no type");
            }
        }

        public KeyValuePair<string, int> MostCommon() {
            var top = new KeyValuePair<string, int>(null, 0);
            foreach (var val in ValCount) {
                if (val.Value <= top.Value) continue;
                top = val;
            }
            return top;
        }

        public override string ToString() {
            var com = MostCommon();
            var res = new List<string>
                      {
                          Name,
                          "Nulls:" + (HasNulls ? "Yes" : "No"),
                          "Distinct:" + NumVals,
                          "Top:" + com.Key + "/" + com.Value,
                          DataType.ToString()
                      };
            var max = MaxVal();
            if (max != null) {
                res.Add("Max:" + max);
                res.Add("Min:" + MinVal());
            }
            return String.Join("; ", res.ToArray());
        }

        public void Add(string val) {
            if (String.IsNullOrEmpty(val)) {
                HasNulls = true;
                val = "<NULL>";
            } else if (DataType != typeof(string)) {
                if (DataType == typeof(int)) CheckInt_(val);
                if (DataType == typeof(double)) CheckDbl_(val);
                if (DataType == typeof(DateTime)) CheckDT_(val);
            }

            int curr;
            if (ValCount.TryGetValue(val, out curr)) ValCount[val] = curr + 1;
            else ValCount[val] = 1;
        }

        readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
        const DateTimeStyles styles = DateTimeStyles.None;

        private void CheckDT_(string val) {
            DateTime res;
            if (DateTime.TryParse(val, out res)) return;
            if (DateTime.TryParse(val, culture, styles, out res)) return;

            try {
                Convert.ToDateTime(val);
            } catch {
                DataType = typeof(string);
                return;
            }
        }
        private void CheckDbl_(string val) {
            try {
                Convert.ToDouble(val);
            } catch {
                DataType = typeof(DateTime);
                return;
            }
        }
        private void CheckInt_(string val) {
            try {
                Convert.ToInt32(val);
            } catch {
                DataType = typeof(double);
                return;
            }
        }
    }

    public class DataFile {
        public DataFile(IEnumerable<string> fields) {
            Cols = fields.Select(o => new DataCol(o)).ToArray();
            Count = 0;
        }
        public DataCol[] Cols { get; private set; }
        public int Count { get; private set; }
        public void Add(IEnumerable<string> rec) {
            int i = 0;
            foreach (var r in rec) Cols[i++].Add(r);
            Count++;
        }
    }
}