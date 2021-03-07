using System;
using System.Collections.Generic;

namespace Avitah.Energy.Service.Model
{
    public class MappedData
    {
        public MappedDataKey Key { get; set; }
        public DateTime DateTime { get; set; }
        public double? Value { get; set; }
    }

    public class MappedDataKey
    {
        public string Country { get; set; }
        public string Quantity { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        protected bool Equals(MappedDataKey other)
        {
            return Country == other.Country && Quantity == other.Quantity && Type == other.Type && Category == other.Category && Unit == other.Unit;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MappedDataKey)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Country, Quantity, Type, Category, Unit);
        }
    }

    public class MappedDataGrouping
    {
        public MappedDataKey Key { get; set; }
        public IEnumerable<MappedData> MappedDataTimeSeries { get; set; }
    }
}
