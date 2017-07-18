using System;
using System.Collections.Generic;
using InsuranceCalculation;
using Machine.Specifications;

namespace InsuranceCalculationTests.DeclineRulesTests
{
    [Subject(typeof(DeclineRules))]
    class Context
    {
        
    }

    class when_I_call_DeclineRules_with_more_than_three_claims_in_total : Context
    {
        static List<Driver> drivers;
        private static bool result;

        Establish context = () =>
        {
            drivers = new List<Driver>
            {
                new Driver
                {
                    Claims = new List<Claim>
                    {
                        new Claim(),
                        new Claim()
                    }
                },
                new Driver
                {
                    Claims = new List<Claim>
                    {
                        new Claim(),
                        new Claim()
                    }
                }
            };
        };

        Because of = () =>
        {
            result = DeclineRules.IsDeclined(DateTime.Today, drivers);
        };

        It should_be_declined = () =>
        {
            result.ShouldBeTrue();
        };
    }
}