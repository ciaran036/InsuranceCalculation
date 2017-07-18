using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceCalculation
{
    public class PremiumCalculation
    {
        public const int StartingPoint = 500;

        /// <summary>
        /// Calculate an insurance premium according to supplied information
        /// </summary>
        /// <param name="policyStartDate"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        public static int Calculate(DateTime policyStartDate, IEnumerable<Driver> drivers)
        {
            var premium = StartingPoint;
            driverOccupationRules(ref premium, drivers);
            driverAgeRules(ref premium, policyStartDate, drivers);
            driverClaimsRules(ref premium, policyStartDate, drivers);

            return premium;
        }

        /// <summary>
        /// For each claim within 1 year of the start date of the policy, increase the premium by 20%.
        /// For each claim within 2-5 years of the start date of the policy, increase the premium by 10%.
        /// </summary>
        /// <param name="premium"></param>
        /// <param name="startDate"></param>
        /// <param name="drivers"></param>
        private static void driverClaimsRules(ref int premium, DateTime startDate, IEnumerable<Driver> drivers)
        {
            foreach (var driver in drivers.Where(d => d.Claims.Any()))
            {
                foreach (var claim in driver.Claims)
                {
                    if (claim.Date >= startDate.AddYears(-1))
                    {
                        premium = premium.IncreasePercentage(20);
                    }
                    else if (claim.Date >= startDate.AddYears(-2) && claim.Date <= startDate.AddYears(-5))
                    {
                        premium = premium.IncreasePercentage(10);
                    }
                }
            }
        }

        /// <summary>
        /// If youngest driver is aged between 21 and 25 at the start date of the policy, increase premium by 20%.
        /// If the youngest driver is aged between 26 and 75 at the start date of the policy, decrease premium by 10%.
        /// </summary>
        /// <param name="premium"></param>
        /// <param name="startDate"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static void driverAgeRules(ref int premium, DateTime startDate, IEnumerable<Driver> drivers)
        {
            var youngestDriver = drivers.OrderByDescending(d => d.DateOfBirth).First();
            var driverAge = youngestDriver.DateOfBirth.AgeOnDate(startDate);

            if (driverAge >= 21 && driverAge <= 25)
            {
                premium = premium.IncreasePercentage(20);
            }
            else if(driverAge >= 26 && driverAge <= 75)
            {
                premium = premium.DecreasePercentage(10);
            }
        }

        /// <summary>
        /// Apply calculation rules according to driver occupation
        /// </summary>
        /// <param name="premium"></param>
        /// <param name="drivers"></param>
        private static void driverOccupationRules(ref int premium, IEnumerable<Driver> drivers)
        {
            chauffeurRule(ref premium, drivers);
            accountantRule(ref premium, drivers);
        }

        /// <summary>
        /// If there is a driver who is an Accountant on the policy, decrease the premium by 10%.
        /// </summary>
        /// <param name="premium"></param>
        /// <param name="drivers"></param>
        private static void accountantRule(ref int premium, IEnumerable<Driver> drivers)
        {
            if (drivers.Any(d => d.Occupation == Occupation.Accountant))
            {
                premium = premium.DecreasePercentage(10);
            }
        }

        /// <summary>
        /// If there is a driver who is a Chauffeur on the policy, increase the premium by 10%
        /// </summary>
        /// <param name="premium"></param>
        /// <param name="drivers"></param>
        private static void chauffeurRule(ref int premium, IEnumerable<Driver> drivers)
        {
            if (drivers.Any(d => d.Occupation == Occupation.Chauffeur))
            {
                premium = premium.IncreasePercentage(10);
            }
        }
    }
}