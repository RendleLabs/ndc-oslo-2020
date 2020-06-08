namespace CustomTypes
{
    public partial class DecimalValue
    {
        private const int NanoFactor = 1_000_000_000;

        public static implicit operator DecimalValue(decimal value)
        {
            var units = decimal.Truncate(value);
            var nanos = (value - units) * NanoFactor;
            return new DecimalValue
            {
                Units = (long) units,
                Nanos = (int) nanos
            };
        }

        public static implicit operator decimal(DecimalValue value) =>
            value.Units + (decimal) value.Nanos / NanoFactor;
    }
}