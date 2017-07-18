using System;
using System.Collections.Generic;

namespace InsuranceCalculation
{
    /// <summary>
    /// Represents a driver
    /// </summary>
    public class Driver
    {
        public string Name { get; set; }
        public Occupation Occupation { get; set; }
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<Claim> Claims { get; set; }

        public Driver()
        {
            Claims = new List<Claim>();
        }
    }

    public enum Occupation
    {
        Chauffeur,
        Accountant
    }
}