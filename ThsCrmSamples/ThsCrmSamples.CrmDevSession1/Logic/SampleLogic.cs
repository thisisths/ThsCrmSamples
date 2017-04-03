namespace ThsCrmSamples.CrmDevSession1.Logic
{
    using System;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    using ThsCrmSamples.CrmDevSession1.Exceptions;

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

            this.organizationService.Update(entity);
        }

        public void SetNameToHelloWorldIfNullOrWhitespace2(Guid recordId, string entityLogicalName)
        {
            var entity = this.organizationService.Retrieve(entityLogicalName, recordId, new ColumnSet(NAME_FIELD));

            if (entity.Attributes[NAME_FIELD].GetType() != typeof(string))
            {
                throw new WrongAttributeTypeException(entityLogicalName, NAME_FIELD, "string");
            }

            if (string.IsNullOrWhiteSpace((string)entity.Attributes[NAME_FIELD]))
            {
                entity.Attributes[NAME_FIELD] = "Hallo Welt";
            }

            this.organizationService.Update(entity);
        }

        public void SetAccountNameToContact(Guid contactId, Guid accountId)
        {
            var account = this.organizationService.Retrieve("account", accountId, new ColumnSet("name"));

            var contact = new Entity("contact", contactId)
            {
                Attributes =
                {
                    ["accountName"] = account.Attributes["name"]
                }
            };
            this.organizationService.Update(contact);
        }

        public void SetAccountNameToContact(Guid contactId, string accountName)
        {
            var contact = new Entity("contact", contactId)
            {
                Attributes =
                {
                    ["accountName"] = accountName
                }
            };
            this.organizationService.Update(contact);
        }
    }
}
