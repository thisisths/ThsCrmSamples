﻿namespace ThsCrmSamples.CrmDevSession1
{
    using System.Linq;

    using ThsCrmSamples.Core;
    using ThsCrmSamples.CrmDevSession1.Logic;

    public class Sample3Good : BasePlugin
    {
        public Sample3Good(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        protected override void ExecutePlugin()
        {
            if (this.TargetEntity.LogicalName != "account")
            {
                this.Logger.Error("Sample2Bad: should run on an account!");
            }

            var logic = new SampleLogic2(this.OrganizationServiceContext);

            var contacts = logic.GetContactsOfAccount(this.TargetEntity.Id).ToList();

            var count = contacts.Count;

            foreach (var contact in contacts)
            {
                logic.DoSomeThingWithContactAndCount(contact, count);
            }
        }
    }
}
