using System;
using System.Linq;

namespace AndcultureCode.CSharp.Core.Interfaces
{
    public interface IContext : IDisposable
    {
        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        void CreateStructure();

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        void DeleteDatabase();

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        void DetectChanges();

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        void DropStructure();

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        long ExecuteCommand(string commandText);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;
    }
}