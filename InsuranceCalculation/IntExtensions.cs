namespace InsuranceCalculation
{
    /// <summary>
    /// Integer extensions
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Increase a value by a percentage amount
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int IncreasePercentage(this int value, int percentage)
        {
            return value + (value * percentage / 100);
        }

        /// <summary>
        /// Decrease a value by a percentage amount
        /// </summary>
        /// <param name="value"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int DecreasePercentage(this int value, int percentage)
        {
            return value - (value * percentage / 100);
        }
    }
}