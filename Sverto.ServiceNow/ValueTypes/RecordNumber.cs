using System;
using System.Text.RegularExpressions;
using Sverto.ServiceNow.Models;

namespace Sverto.ServiceNow.ValueTypes
{
    public class RecordNumber
    {
        private static Regex _Matches = new Regex("(SR|CHG|CTASK|PRB)[0-9]{7}");
        private static Regex _Match = new Regex("^(SR|CHG|CTASK|PRB)[0-9]{7}$");

        public string Number { get; private set; }
        public Type RecordType { get; private set; }

        public RecordNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentNullException(nameof(number));

            var match = _Match.Match(number.ToUpper());
            var type = GetRecordType(match);
            if (!match.Success || type == null)
                throw new ArgumentException("Invalid parameter value.", nameof(number));

            Number = match.Value;
            RecordType = type;
        }

        private static Type GetRecordType(Match match)
        {
            switch (match.Groups[1].Value) {
                case "SR": return typeof(SupportRequest);
                case "CHG": return typeof(Change);
                case "CTASK": return typeof(ChangeTask);
                case "PRB": return typeof(Problem);
                default: return null;
            }
        }

        public static bool IsValidNumber(string number)
        {
            return _Match.IsMatch(number);
        }

        public static bool IsValidId(string id)
        {
            return (!string.IsNullOrWhiteSpace(id) && id.Length == 32);
        }

        public static RecordNumber Extract(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return null;
            var result = _Matches.Matches(text);
            if (result.Count == 0) return null;
            return new RecordNumber(result[0].Value);
        }

        public static explicit operator String(RecordNumber recordNumber)
        {
            return recordNumber?.Number;
        }

        public override string ToString()
        {
            return Number;
        }
    }
}
