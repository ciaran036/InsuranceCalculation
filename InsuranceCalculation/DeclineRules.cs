using System;
using System.Collections.Generic;
using System.Linq;

namespace InsuranceCalculation
{
    /// <summary>
    /// Rules for declining an insurancy policy
    /// </summary>
    public static class DeclineRules
    {
        /// <summary>
        /// Determines whether the policy has been declined or not according to business rules
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        public static bool IsDeclined(DateTime startDate, IEnumerable<Driver> drivers)
        {
            if (startDateBeforeToday(startDate)) return true;
            if (youngestDriverIsUnderAge(startDate, drivers)) return true;
            if (oldestDriverIsOver75(startDate, drivers)) return true;
            if (totalNumberOfClaimsExceedsThree(drivers)) return true;
            if (anyDriverHasMoreThanTwoClaims(drivers)) return true;

            return false;
        }

        #region Private methods

        /// <summary>
        /// If the total number of claims exceeds 3 then decline with a message, "Policy has more than 3 claims".
        /// </summary>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static bool totalNumberOfClaimsExceedsThree(IEnumerable<Driver> drivers)
        {
            var totalClaims = drivers.SelectMany(d => d.Claims).Count();
            if (totalClaims > 3)
            {
                declineMessage("Policy has more than 3 claims.");
                return true;
            }
            return false;
        }

        /// <summary>
        /// If a driver has more than 2 claims decline with a message 
        /// "Driver has more than 2 claims" - inlude the name of the driver
        /// TODO: What if more than 1 driver has more than 2 claims?
        /// </summary>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static bool anyDriverHasMoreThanTwoClaims(IEnumerable<Driver> drivers)
        {
            var driver = drivers.FirstOrDefault(d => d.Claims.Count() > 2);
            if (driver != null)
            {
                declineMessage("Driver has more than 2 claims - " + driver.Name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// If the oldest driver is over the age of 75 at the start date of the policy,
        /// decline with a message "Age of Oldest Driver" and append the name of the driver
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static bool oldestDriverIsOver75(DateTime startDate, IEnumerable<Driver> drivers)
        {
            var oldestDriver = drivers.OrderBy(d => d.DateOfBirth).First();
            if (oldestDriver.DateOfBirth.AgeOnDate(startDate) > 75)
            {
                declineMessage("Age of Oldest Driver - " + oldestDriver.Name);
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the youngest driver is under the age of 21 at the start date of the policy,
        /// decline with a message, "Age of Youngest Driver" and append the name of the driver.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="drivers"></param>
        /// <returns></returns>
        private static bool youngestDriverIsUnderAge(DateTime startDate, IEnumerable<Driver> drivers)
        {
            var youngestDriver = drivers.OrderByDescending(d => d.DateOfBirth).First();
            if (youngestDriver.DateOfBirth.AgeOnDate(startDate) < 21)
            {
                declineMessage("Age of Youngest Driver");
                return true;
            }
            return false;
        }

        /// <summary>
        /// If the start date of the policy is before today decline with the message "Start Date of Policy".
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private static bool startDateBeforeToday(DateTime startDate)
        {
            if (startDate < DateTime.Today)
            {
                declineMessage("Start Date of Policy");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Output a message to inform a user if a policy has been declined
        /// </summary>
        /// <param name="message"></param>
        private static void declineMessage(string message)
        {
            Console.WriteLine("The policy has been declined for the following reason.");
            Console.WriteLine("Reason: " + message);
        }

        #endregion
    }
}