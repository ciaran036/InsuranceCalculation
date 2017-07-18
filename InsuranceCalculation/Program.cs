using System;
using System.Collections.Generic;

namespace InsuranceCalculation
{
    /// <summary>
    /// Program which calculates an insurance premium for a user based on their input
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var policyStartDate = getPolicyStartDate();
            var drivers = getDrivers();

            if (!DeclineRules.IsDeclined(policyStartDate, drivers))
            {
                var premium = PremiumCalculation.Calculate(policyStartDate, drivers);
                Console.WriteLine("Your insurance premium will be : £" + premium);
                Console.ReadLine();
            }
            else
            {
                Console.ReadLine();
            }
        }

        #region Private methods

        /// <summary>
        /// Get the policy start date
        /// </summary>
        /// <returns></returns>
        private static DateTime getPolicyStartDate()
        {
            Console.WriteLine("Please enter the start date of the policy in format DD/MM/YYYY, e.g. 06/10/2015'");
            var startDateText = Console.ReadLine();
            return !startDateText.TryParseDate(out DateTime startDate) ? ErrorResponse(getPolicyStartDate) : startDate;
        }

        /// <summary>
        /// Get details of drivers on the policy
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Driver> getDrivers()
        {
            var drivers = new List<Driver>();
            var numberOfDrivers = getNumberOfDriversOnPolicy();

            for (var i = 0; i < numberOfDrivers; i++)
            {
                Console.WriteLine("Please enter the details for driver number " + (i+1) + " : ");
                drivers.Add(getDriverDetails());
            }

            return drivers;
        }

        /// <summary>
        /// Prompt user to enter the number of drivers on the policy
        /// </summary>
        /// <returns></returns>
        private static int getNumberOfDriversOnPolicy()
        {
            Console.WriteLine(
                "How many drivers do you wish to add to the policy? A policy has a minimum of 1 and a maximum of 5 drivers.");
            var numberOfDriversResponse = Console.ReadLine();
            int numberOfDrivers;

            if (int.TryParse(numberOfDriversResponse, out numberOfDrivers))
            {
                if (numberOfDrivers < 0)
                {
                    Console.WriteLine("At least 1 driver must be on the policy. Please try again.");
                    return getNumberOfDriversOnPolicy();
                }
                if (numberOfDrivers > 5)
                {
                    Console.WriteLine("The policy can have a maximum of 5 drivers. Please try again.");
                    return getNumberOfDriversOnPolicy();
                }
                return numberOfDrivers;
            }
            return ErrorResponse(getNumberOfDriversOnPolicy);
        }

        /// <summary>
        /// Prompt user to enter details for a driver
        /// </summary>
        /// <returns></returns>
        private static Driver getDriverDetails()
        {
            return new Driver
            {
                Name = getDriverName(),
                Occupation = getDriverOccupation(),
                DateOfBirth = getDateOfBirth(),
                Claims = getDriverClaims()
            };
        }

        /// <summary>
        /// Prompt for any claims against a driver
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Claim> getDriverClaims()
        {
            var claims = new List<Claim>();
            var anyClaims = areThereAnyClaims();
            if (!anyClaims) return claims;

            var numberOfClaims = howManyClaims();

            for (int i = 0; i < numberOfClaims; i++)
            {
                claims.Add(new Claim {Date = getClaimDate(i+1)});
            }

            return claims;
        }

        /// <summary>
        /// Prompt user for the claim date
        /// </summary>
        /// <param name="claimNumber"></param>
        /// <returns></returns>
        private static DateTime getClaimDate(int claimNumber)
        {
            Console.WriteLine("Please enter the date of claim for claim number " + claimNumber + " . Please enter in following format: 'DD/MM/YYYY'.");
            var dateOfClaim = DateTime.MinValue;
            var claimDateResponse = Console.ReadLine();

            if (!claimDateResponse.TryParseDate(out dateOfClaim))
            {
                Console.WriteLine("Failed to read date. Please try again.");
                getClaimDate(claimNumber);
            }

            return dateOfClaim;
        }

        /// <summary>
        /// Prompt user for number of claims against a driver
        /// </summary>
        /// <returns></returns>
        private static int howManyClaims()
        {
            Console.WriteLine("How many claims are there? A driver can have a maximum of 5 claims.");
            var claimQuantityResponse = Console.ReadLine();
            int claimQuantity;
            bool responseUnderstood = int.TryParse(claimQuantityResponse, out claimQuantity);

            if (!responseUnderstood || claimQuantity < 0)
            {
                return ErrorResponse(howManyClaims);
            }

            if(claimQuantity > 5)
            {
                return ErrorResponse(howManyClaims);
            }

            return claimQuantity;
        }

        /// <summary>
        /// Prompt user to specify if there any claims against a driver
        /// </summary>
        /// <returns></returns>
        private static bool areThereAnyClaims()
        {
            Console.WriteLine("For this driver, has there been any claims? Y for Yes, N for No.");
            var anyClaimsResponse = Console.ReadLine();
            switch (anyClaimsResponse)
            {
                case "Y":
                case "y":
                    return true;
                case "N":
                case "n":
                    return false;
                default:
                    return ErrorResponse(areThereAnyClaims);
            }
        }

        /// <summary>
        /// Prompt user for driver name
        /// </summary>
        /// <returns></returns>
        private static string getDriverName()
        {
            Console.WriteLine("What is the driver's name? ");
            var driverName = Console.ReadLine();
            return !string.IsNullOrEmpty(driverName) ? driverName : ErrorResponse(getDriverName);
        }

        /// <summary>
        /// Prompt user for date of birth
        /// </summary>
        /// <returns></returns>
        private static DateTime getDateOfBirth()
        {
            Console.WriteLine("Please enter the driver's date of birth, using format DD/MM/YYYY.");
            var dateOfBirthText = Console.ReadLine();
            return !dateOfBirthText.TryParseDate(out DateTime dateOfBirth) ? ErrorResponse(getDateOfBirth) : dateOfBirth;
        }

        /// <summary>
        /// Prompt driver for occupation
        /// </summary>
        /// <returns></returns>
        private static Occupation getDriverOccupation()
        {
            Console.WriteLine(
                "Is the driver a Chauffeur, or an Accountant? Enter C for Chauffeur, or A for Accountant. No other occupation is accepted.");

            var occupationResponse = Console.ReadLine();
            switch (occupationResponse)
            {
                case "C":
                    return Occupation.Chauffeur;
                case "A":
                    return Occupation.Accountant;
                default:
                    return ErrorResponse(getDriverOccupation);
            }
        }

        /// <summary>
        /// Inform user that response was not understood and prompt them to try input again
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        private static T ErrorResponse<T>(Func<T> action)
        {
            Console.WriteLine("Response not understood. Please try again.");
            return action.Invoke();
        }

        #endregion
    }
}
