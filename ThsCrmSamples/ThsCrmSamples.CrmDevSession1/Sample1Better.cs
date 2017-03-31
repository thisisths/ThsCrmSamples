namespace ThsCrmSamples.CrmDevSession1
{
    using System;

    using ThsCrmSamples.Core;
    using ThsCrmSamples.CrmDevSession1.Exceptions;
    using ThsCrmSamples.CrmDevSession1.Logic;

    public class Sample1Better : BasePlugin
    {
        public Sample1Better(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        protected override void ExecutePlugin()
        {
            var logic = new SampleLogic(this.OrganizationService);

            try
            {
                logic.SetNameToHelloWorldIfNullOrWhitespace2(this.TargetEntity.Id, this.TargetEntity.LogicalName);
            }
            catch (WrongAttributeTypeException ex)
            {
                this.Logger.Error(ex);
            }
            catch (BusinessException ex)
            {
                this.Logger.Error(ex);
                throw;
            }
        }
    }
}
