namespace ThsCrmSamples.CrmDevSession1.Logic
{
    using System;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    public class SampleLogic
    {
        private const string NAME_FIELD = "ths_name";

        private readonly IOrganizationService organizationService;

        public SampleLogic(IOrganizationService organizationService)
        {
            this.organizationService = organizationService;
        }

        public void SetNameToHelloWorldIfNullOrWhitespace(Guid recordId, string entityLogicalName)
        {
            var entity = this.organizationService.Retrieve(entityLogicalName, recordId, new ColumnSet(NAME_FIELD));

            if (string.IsNullOrWhiteSpace((string)entity.Attributes[NAME_FIELD]))
            {
                entity.Attributes[NAME_FIELD] = "Hallo Welt";
            }
        }
    }
}
