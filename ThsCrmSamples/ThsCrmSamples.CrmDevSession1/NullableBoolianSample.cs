namespace ThsCrmSamples.CrmDevSession1
{
    using System;

    public class NullableBoolianSample
    {
        public void CompareWithBool()
        {
            var entityWithApprovedTrue = new EntityFromService { IsApproved = true };
            if (entityWithApprovedTrue.IsApproved.GetValueOrDefault())
            {
                Console.WriteLine("This line will be written");
            }

            var entityWithApprovedFalse = new EntityFromService { IsApproved = false };
            if (entityWithApprovedFalse.IsApproved.GetValueOrDefault())
            {
                Console.WriteLine("This line will not be written");
            }

            if (!entityWithApprovedFalse.IsApproved.GetValueOrDefault())
            {
                Console.WriteLine("But this line will be written");
            }

            var entityWithApprovedNull = new EntityFromService { IsApproved = false };
            if (entityWithApprovedNull.IsApproved.GetValueOrDefault())
            {
                Console.WriteLine("This line will not be written");
            }

            if (!entityWithApprovedNull.IsApproved.GetValueOrDefault())
            {
                Console.WriteLine("But this line will be written");
            }
        }

        public class EntityFromService
        {
            public bool? IsApproved { get; set; }
        }
    }
}
