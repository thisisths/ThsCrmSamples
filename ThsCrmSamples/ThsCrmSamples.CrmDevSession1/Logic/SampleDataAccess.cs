using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThsCrmSamples.CrmDevSession1.Logic
{
    using Microsoft.Xrm.Sdk.Client;

    public class SampleDataAccess : ISampleDataAccess
    {
        private readonly OrganizationServiceContext organizationServiceContext;

        public SampleDataAccess(OrganizationServiceContext organizationServiceContext)
        {
            this.organizationServiceContext = organizationServiceContext;
        }

        public IEnumerable<Contact> GetContacsByAccountId(Guid accountId)
        {
            return this.organizationServiceContext.CreateQuery<Contact>().Where(o => o.AccountId.Id == accountId);
        }
    }
}
