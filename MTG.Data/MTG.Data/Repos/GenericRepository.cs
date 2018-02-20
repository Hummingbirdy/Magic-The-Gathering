//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dapper;

//namespace MTG.Data.Repos
//{
//    public interface IGenericRepository<TModel> where TModel : new()
//    {
//        TModel Get(int id);
//        Task<TModel> GetAsync(int id);
//        IEnumerable<TModel> GetAll();
//        Task<IEnumerable<TModel>> GetAllAsync();
//        IEnumerable<TModel> GetList(object whereConditions);
//        IEnumerable<TModel> GetList(string whereConditions);
//        Task<IEnumerable<TModel>> GetListAsync(object whereConditions);
//        Task<IEnumerable<TModel>> GetListAsync(string conditions);

//        IEnumerable<TModel> GetListPaged(int pageNumber, int rowsPerPage, string conditions,
//            string orderby);
//        int? Insert(TModel modelToInsert);
//        Task<int?> InsertAsync(TModel modelToInsert);
//        int Update(TModel modelToUpdate);
//        Task<int> UpdateAsync(TModel modelToUpdate);
//        int Delete(int id);
//        Task<int> DeleteAsync(int id);
//        int Delete(TModel entityToDelete);
//        Task<int> DeleteAsync(TModel entityToDelete);
//        int DeleteList(object whereConditions);
//        Task<int> DeleteListAsync(object whereConditions);
//        int DeleteList(string conditions);
//        Task<int> DeleteListAsync(string conditions);
//        int RecordCount(string conditions);
//        int RecordCount(object whereConditions);
//        Task<int> RecordCountAsync(string conditions);
//        Task<int> RecordCountAsync(object whereConditions);
//        IEnumerable<TModel> QuerySproc(string sql, object param);
//        Task<IEnumerable<TModel>> QuerySprocAsync(string sql, object param);
//    }

//    /// <summary>
//    /// Base Repository Class for Database operations.
//    /// This is a generic implementation around Dapper.SimpleCRUD : https://github.com/ericdc1/Dapper.SimpleCRUD
//    /// </summary>
//    /// <typeparam name="TModel"></typeparam>
//    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : new()
//    {
//        protected readonly IDbTransaction Transaction;
//        //private  IDbConnection _dbConnection;
//        //readonly string _connectionString;

//        public GenericRepository(IDbTransaction transaction)
//        {
//            Transaction = transaction;
//        }

//        protected IDbConnection Connection => Transaction.Connection;

//        public virtual TModel Get(int id)
//        {
//            var result = Connection.Get<TModel>(id, Transaction);

//            if (result != null) return result;

//            return new TModel();
//        }

//        public virtual async Task<TModel> GetAsync(int id)
//        {
//            var result = await Connection.GetAsync<TModel>(id, Transaction);

//            if (result != null) return result;

//            return new TModel();
//        }

//        public virtual IEnumerable<TModel> GetAll()
//        {
//            var results = Connection.GetList<TModel>("WHERE 1 = 1", Transaction);
//            return results ?? new List<TModel>();
//        }

//        public virtual IEnumerable<T> GetAll<T>()
//        {
//            var results = Connection.GetList<T>("WHERE 1 = 1", Transaction);
//            return results ?? new List<T>();
//        }

//        public virtual async Task<IEnumerable<TModel>> GetAllAsync()
//        {
//            var results = await Connection.GetListAsync<TModel>("WHERE 1 = 1", Transaction);
//            return results ?? new List<TModel>();
//        }

//        public virtual IEnumerable<TModel> GetList(object whereConditions)
//        {
//            var results = Connection.GetList<TModel>(whereConditions, Transaction);

//            return results ?? new List<TModel>();
//        }

//        public virtual IEnumerable<T> GetList<T>(object whereConditions)
//        {
//            var results = Connection.GetList<T>(whereConditions, Transaction);

//            return results ?? new List<T>();
//        }

//        public virtual IEnumerable<TModel> GetList(string whereConditions)
//        {
//            var results = Connection.GetList<TModel>(whereConditions, Transaction);

//            return results ?? new List<TModel>();
//        }

//        public virtual IEnumerable<T> GetList<T>(string whereConditions)
//        {
//            var results = Connection.GetList<T>(whereConditions, Transaction);

//            return results ?? new List<T>();
//        }

//        public virtual async Task<IEnumerable<TModel>> GetListAsync(object whereConditions)
//        {
//            var results = await Connection.GetListAsync<TModel>(whereConditions, Transaction);

//            return results ?? new List<TModel>();
//        }

//        public virtual async Task<IEnumerable<TModel>> GetListAsync(string conditions)
//        {
//            var results = await Connection.GetListAsync<TModel>(conditions, Transaction);
//            return results ?? new List<TModel>();
//        }

//        public virtual int? Insert(TModel modelToInsert)
//        {
//            return Connection.Insert(modelToInsert, Transaction);
//        }

//        public virtual int? Insert<T>(T modelToInsert)
//        {
//            return Connection.Insert(modelToInsert, Transaction);
//        }

//        public virtual async Task<int?> InsertAsync(TModel modelToInsert)
//        {
//            return await Connection.InsertAsync(modelToInsert, Transaction);
//        }

//        public virtual int Update(TModel modelToUpdate)
//        {
//            return Connection.Update(modelToUpdate, Transaction);
//        }

//        public virtual async Task<int> UpdateAsync(TModel modelToUpdate)
//        {
//            return await Connection.UpdateAsync(modelToUpdate, Transaction);
//        }

//        public virtual int Delete(int id)
//        {
//            return Connection.Delete<TModel>(id, Transaction);
//        }

//        public virtual async Task<int> DeleteAsync(int id)
//        {
//            return await Connection.DeleteAsync<TModel>(id, Transaction);
//        }

//        public virtual int Delete(TModel entityToDelete)
//        {
//            return Connection.Delete<TModel>(entityToDelete, Transaction);
//        }

//        public virtual async Task<int> DeleteAsync(TModel entityToDelete)
//        {
//            return await Connection.DeleteAsync<TModel>(entityToDelete, Transaction);
//        }

//        public virtual int DeleteList(object whereConditions)
//        {
//            return Connection.DeleteList<TModel>(whereConditions, Transaction);
//        }

//        public virtual async Task<int> DeleteListAsync(object whereConditions)
//        {
//            return await Connection.DeleteListAsync<TModel>(whereConditions, Transaction);
//        }

//        public virtual int DeleteList(string conditions)
//        {
//            return Connection.DeleteList<TModel>(conditions, Transaction);
//        }

//        public virtual async Task<int> DeleteListAsync(string conditions)
//        {
//            return await Connection.DeleteListAsync<TModel>(conditions, Transaction);
//        }

//        public virtual int RecordCount(string conditions)
//        {
//            return Connection.RecordCount<TModel>(conditions, Transaction);
//        }

//        public virtual int RecordCount<T>(string conditions)
//        {
//            return Connection.RecordCount<T>(conditions, Transaction);
//        }
//        public virtual int RecordCount(object whereConditions)
//        {
//            return Connection.RecordCount<TModel>(whereConditions, Transaction);
//        }

//        public virtual async Task<int> RecordCountAsync(string conditions)
//        {
//            return await Connection.RecordCountAsync<TModel>(conditions, Transaction);
//        }
//        public virtual async Task<int> RecordCountAsync(object whereConditions)
//        {
//            return await Connection.RecordCountAsync<TModel>(whereConditions, Transaction);
//        }

//        public virtual IEnumerable<TModel> QuerySproc(string sql, object param)
//        {
//            var results = Connection.Query<TModel>(sql, param, commandType: CommandType.StoredProcedure,
//                transaction: Transaction);
//            return results ?? new List<TModel>();
//        }

//        public virtual async Task<IEnumerable<TModel>> QuerySprocAsync(string sql, object param)
//        {
//            var results = await Connection.QueryAsync<TModel>(sql, param, commandType: CommandType.StoredProcedure,
//                transaction: Transaction);
//            return results ?? new List<TModel>();
//        }

//        public IEnumerable<TModel> GetListPaged(int pageNumber, int rowsPerPage, string conditions, string orderby)
//        {
//            var results = Connection.GetListPaged<TModel>(pageNumber, rowsPerPage, conditions, orderby,
//                transaction: Transaction);
//            return results ?? new List<TModel>();
//        }
//    }
//}
