namespace ThsCrmSamples.CrmDevSession1.Logic
{
    using System;
    using System.Linq;
    
    using Microsoft.Xrm.Sdk.Client;

    public class SampleLogic2
    {
        private readonly OrganizationServiceContext serviceContext;

        public SampleLogic2(OrganizationServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        public IQueryable<Contact> GetContactsOfAccount(Guid accountId)
        {
            return this.serviceContext.CreateQuery<Contact>().Where(o => o.AccountId.Id == accountId);
        }

        public void DoSomeThingWithContactAndCount(Contact contact, int count)
        {
            // To something
        }
    }
}
