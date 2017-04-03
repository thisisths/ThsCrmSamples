namespace ThsCrmSamples.CrmDevSession1
{
    using Microsoft.Xrm.Sdk.Query;

    using ThsCrmSamples.Core;
    using ThsCrmSamples.CrmDevSession1.Logic;

    public class Sample2Bad : BasePlugin
    {
        public Sample2Bad(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        protected override void ExecutePlugin()
        {
            if (this.TargetEntity.LogicalName != "account")
            {
                this.Logger.Error("Sample2Bad: should run on an account!");
            }

            var query = new QueryExpression("contact")
            {
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("accountid", ConditionOperator.Equal, this.TargetEntity.Id)
                    }
                }
            };
            var contacts = this.OrganizationService.RetrieveMultiple(query);

            foreach (var contact in contacts.Entities)
            {
                var sampleLogic = new SampleLogic(this.OrganizationService);
                sampleLogic.SetAccountNameToContact(contact.Id, this.TargetEntity.Id);
            }
        }
    }
}
