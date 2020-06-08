namespace CustomTypes
{
    public partial class DecimalValue
    {
        private const int NanoFactor = 1_000_000_000;

        public static implicit operator DecimalValue(decimal value) =>
            new DecimalValue
            {
                Units = (long)decimal.Truncate(value),
                Nanos = (int)(decimal.Remainder(1m, value) * NanoFactor)
            };

        public static implicit operator decimal(DecimalValue value) =>
            value.Units + (decimal) value.Nanos / NanoFactor;
    }
}