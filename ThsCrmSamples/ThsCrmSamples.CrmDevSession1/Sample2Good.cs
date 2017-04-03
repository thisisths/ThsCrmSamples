namespace ThsCrmSamples.CrmDevSession1
{
    using Microsoft.Xrm.Sdk.Query;

    using ThsCrmSamples.Core;
    using ThsCrmSamples.CrmDevSession1.Logic;

    public class Sample2Good : BasePlugin
    {
        public Sample2Good(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        protected override void ExecutePlugin()
        {
            if (this.TargetEntity.LogicalName != "account")
            {
                this.Logger.Error("Sample2Bad: should run on an account!");
            }
            
            var sampleLogic = new SampleLogic(this.OrganizationService);
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
            var account = this.OrganizationService.Retrieve("account", this.TargetEntity.Id, new ColumnSet("name"));

            foreach (var contact in contacts.Entities)
            {
                sampleLogic.SetAccountNameToContact(contact.Id, (string)account.Attributes["name"]);
            }
        }
    }
}
