namespace ThsCrmSamples.Core
{
    using System;
    using System.Linq;
    using System.ServiceModel;

    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    using ThsCrmSample.Core;

    public abstract class BasePlugin : IPlugin
    {
        private Lazy<OrganizationServiceContext> organizationServiceContext;

        protected BasePlugin(string unsecureConfiguration, string secureConfiguration)
        {
            this.UnsecureConfiguration = unsecureConfiguration;
            this.SecureConfiguration = secureConfiguration;

        }

        public virtual void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                this.CreateServicesAndContext(serviceProvider);

                if (this.PluginExecutionContext.InputParameters.Contains("Target")
                    && this.PluginExecutionContext.InputParameters["Target"] is Entity)
                {
                    // Obtain the target entity from the input parameters.
                    this.TargetEntity = (Entity)this.PluginExecutionContext.InputParameters["Target"];
                }

                // Call Run method (abstract method)
                this.ExecutePlugin();
            }
            catch (InvalidPluginExecutionException)
            {
                //retain original stack trace
                throw;
            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException($"Fehler in Execute während Service-Aufruf: {ex.Message}\n{ex.StackTrace}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException($"Fehler in Execute: {ex.Message}\n{ex.StackTrace}", ex);
            }
        }

        private void CreateServicesAndContext(IServiceProvider serviceProvider)
        {
            // Create Plugin-Context
            this.PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ////// ReSharper disable once SuspiciousTypeConversion.Global
            ////((IProxyTypesAssemblyProvider)this.PluginExecutionContext).ProxyTypesAssembly = (typeof(TEntity)).Assembly;

            // Create Tracing Service
            this.TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            this.Logger = new Logger(this.TracingService);

            // Create Service and OrgContext
            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            this.OrganizationService = factory.CreateOrganizationService(this.PluginExecutionContext.UserId);
            this.organizationServiceContext = new Lazy<OrganizationServiceContext>(() => new OrganizationServiceContext(this.OrganizationService));
        }

        public string UnsecureConfiguration { get; private set; }

        public string SecureConfiguration { get; private set; }

        public IPluginExecutionContext PluginExecutionContext { get; private set; }

        public ITracingService TracingService { get; private set; }

        public IOrganizationService OrganizationService { get; private set; }

        public ILogger Logger { get; private set; }

        public OrganizationServiceContext OrganizationServiceContext => this.organizationServiceContext.Value;

        public Entity TargetEntity { get; private set; }

        /// <summary>
        /// Method for implementing the doing of the plugin.
        /// </summary>
        protected abstract void ExecutePlugin();
    }

    public abstract class BasePlugin<TEntity> : BasePlugin
        where TEntity : Entity
    {
        private Lazy<TEntity> entityLazy;

        private Lazy<TEntity> contextEntityLazy;

        protected BasePlugin(string unsecureConfiguration, string secureConfiguration)
            : base(unsecureConfiguration, secureConfiguration)
        {
        }

        /// <summary>
        /// Base Method that initialize context etc and implements top-level exception handler.
        /// </summary>
        public override void Execute(IServiceProvider serviceProvider)
        {
            this.CreateLazies();

            // Call Run method (abstract method)
            base.Execute(serviceProvider);
        }

        private void CreateLazies()
        {
            this.entityLazy = new Lazy<TEntity>(() =>
            {
                if (this.PluginExecutionContext.InputParameters.Contains("Target")
                    && this.PluginExecutionContext.InputParameters["Target"] is TEntity)
                {
                    // Obtain the target entity from the input parameters.
                    return (TEntity)this.PluginExecutionContext.InputParameters["Target"];
                }
                return null;
            });

            this.contextEntityLazy = new Lazy<TEntity>(() => this.OrganizationServiceContext.CreateQuery<TEntity>().SingleOrDefault(o => o.Id == this.entityLazy.Value.Id));
        }

        public TEntity Entity => this.entityLazy.Value;

        public TEntity ContextEntity => this.contextEntityLazy.Value;
    }
}