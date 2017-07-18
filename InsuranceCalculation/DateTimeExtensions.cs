using System;

namespace InsuranceCalculation
{
    /// <summary>
    /// Extension methods for DateTime's
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Calculates the age of a person for a given date
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int AgeOnDate(this DateTime dateOfBirth, DateTime date)
        {
            var age = date.Year - dateOfBirth.Year;
            if (age > 0)
            {
                age -= Convert.ToInt32(date.Date < dateOfBirth.Date.AddYears(age));
            }
            else
            {
                age = 0;
            }

            return age;
        }
    }
}