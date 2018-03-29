using System.Data;
using System.Web;
using MTG.App_Start;
using MTG.Infrastructure.Tasks;

namespace MTG.DependencyResolution
{
    public class TransactionPerRequest :
        IRunOnEachRequest, IRunOnError, IRunAfterEachRequest
    {
        private IDbConnection _connection;
        private readonly HttpContextBase _httpContext;

        public TransactionPerRequest(IDbConnection connection, HttpContextBase httpContext)
        {
            _connection = connection;


            _httpContext = httpContext;
        }

        void IRunOnEachRequest.Execute()
        {

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }

            var dbTransaction =
                _connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.Inject(dbTransaction);

        }

        void IRunOnError.Execute()
        {
            _httpContext.Items["_Error"] = true;
        }

        void IRunAfterEachRequest.Execute()
        {
            if (StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer != null)
            {
                var transaction =
                    StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer.GetInstance<IDbTransaction>();

                if (_httpContext.Items["_Error"] != null)
                {
                    transaction.Rollback();
                }
                else
                {
                    transaction.Commit();
                }

                transaction.Dispose();
            }
            _connection.Dispose();
        }
    }
}