namespace ThsCrmSamples.CrmDevSession1.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Microsoft.Xrm.Sdk.Client;

    public class SampleLogic2
    {
        private readonly ISampleDataAccess sampleDataAccess;

        public SampleLogic2(ISampleDataAccess sampleDataAccess)
        {
            this.sampleDataAccess = sampleDataAccess;
        }

        public IEnumerable<Contact> GetContactsOfAccount(Guid accountId)
        {


            return this.sampleDataAccess.GetContacsByAccountId(accountId);
        }

        public void DoSomeThingWithContactAndCount(Contact contact, int count)
        {
            // To something
        }
    }
}
