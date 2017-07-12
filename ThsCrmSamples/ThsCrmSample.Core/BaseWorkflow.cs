namespace ThsCrmSamples.Core
{
    using System;
    using System.Activities;
    using System.Linq;
    using System.ServiceModel;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Workflow;
    
    using ThsCrmSample.Core;

    public abstract class BaseWorkflow : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                this.CreateServicesAndContext(context);

                // Call Run method (abstract method)
                this.ExecuteStep();
            }
            catch (InvalidPluginExecutionException)
            {
                //retain original stack trace
                throw;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, $"Fehler in Execute während Service-Aufruf: {ex.Message}\n{ex.StackTrace}");
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(OperationStatus.Failed, $"Fehler in Execute: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void CreateServicesAndContext(CodeActivityContext context)
        {
            this.ActivityContext = context;

            this.TracingService = this.ActivityContext.GetExtension<ITracingService>();
            if (this.TracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            this.WorkflowContext = context.GetExtension<IWorkflowContext>();
            if (this.WorkflowContext == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            this.TracingService.Trace("DateSelectorWorkflow.Execute(), Correlation Id: {0}, Initiating User: {1}",
                this.WorkflowContext.CorrelationId,
                this.WorkflowContext.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            this.OrganizationService = serviceFactory.CreateOrganizationService(this.WorkflowContext.UserId);

            this.OrganizationServiceContext = new Lazy<OrganizationServiceContext>(() => new OrganizationServiceContext(this.OrganizationService));
            this.Logger = new Logger(this.TracingService);
        }

        public CodeActivityContext ActivityContext { get; private set; }

        public IWorkflowContext WorkflowContext { get; private set; }

        public ITracingService TracingService { get; private set; }

        public IOrganizationService OrganizationService { get; private set; }

        public Lazy<OrganizationServiceContext> OrganizationServiceContext { get; private set; }

        public ILogger Logger { get; private set; }

        protected abstract void ExecuteStep();

        public TEntity GetContextEntity<TEntity>()
            where TEntity : Entity
        {
            return this.OrganizationServiceContext.Value.CreateQuery<TEntity>().FirstOrDefault(o => o.Id == this.WorkflowContext.PrimaryEntityId);
        }
    }
}
