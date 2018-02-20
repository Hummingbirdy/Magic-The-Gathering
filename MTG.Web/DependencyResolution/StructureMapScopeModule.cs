using System;
using MTG.Infrastructure.Tasks;


namespace MTG.DependencyResolution {
    using System.Web;

    using MTG.App_Start;

    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule {
        #region Public Methods and Operators

        public void Dispose() {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnContextOnBeginRequest;
            context.Error += Context_Error;
            context.EndRequest += OnContextOnEndRequest;
        }

        private void OnContextOnBeginRequest(object sender, EventArgs e)
        {
            StructuremapMvc.StructureMapDependencyScope.CreateNestedContainer();

            foreach (var task in StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }

        private void Context_Error(object sender, EventArgs e)
        {
            foreach (var task in StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }

        private void OnContextOnEndRequest(object sender, EventArgs e)
        {
            foreach (var task in
                StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetAllInstances<IRunAfterEachRequest>())
            {
                task.Execute();
            }

            HttpContextLifecycle.DisposeAndClearAll();
            StructuremapMvc.StructureMapDependencyScope.DisposeNestedContainer();

        }

        #endregion
    }
}