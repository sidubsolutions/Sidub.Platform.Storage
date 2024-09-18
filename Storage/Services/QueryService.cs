/*
 * Sidub Platform - Storage
 * Copyright (C) 2024 Sidub Inc.
 * All rights reserved.
 *
 * This file is part of Sidub Platform - Storage (the "Product").
 *
 * The Product is dual-licensed under:
 * 1. The GNU Affero General Public License version 3 (AGPLv3)
 * 2. Sidub Inc.'s Proprietary Software License Agreement (PSLA)
 *
 * You may choose to use, redistribute, and/or modify the Product under
 * the terms of either license.
 *
 * The Product is provided "AS IS" and "AS AVAILABLE," without any
 * warranties or conditions of any kind, either express or implied, including
 * but not limited to implied warranties or conditions of merchantability and
 * fitness for a particular purpose. See the applicable license for more
 * details.
 *
 * See the LICENSE.txt file for detailed license terms and conditions or
 * visit https://sidub.ca/licensing for a copy of the license texts.
 */

#region Imports

using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Services;
using Sidub.Platform.Storage.Commands;
using Sidub.Platform.Storage.Commands.Responses;
using Sidub.Platform.Storage.Connectors;
using Sidub.Platform.Storage.Handlers;
using Sidub.Platform.Storage.Queries;

#endregion

namespace Sidub.Platform.Storage.Services
{

    /// <summary>
    /// Service responsible for executing queries and commands on storage entities.
    /// </summary>
    public class QueryService : IQueryService
    {

        #region Member variables

        private readonly IServiceRegistry _entityMetadataService;
        private readonly List<IDataHandlerService> _dataHandlerServices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryService"/> class.
        /// </summary>
        /// <param name="entityMetadataService">The entity metadata service.</param>
        /// <param name="queryHandlerFactories">The query handler factories.</param>
        public QueryService(IServiceRegistry entityMetadataService, IEnumerable<IDataHandlerService> queryHandlerFactories)
        {
            _entityMetadataService = entityMetadataService;
            _dataHandlerServices = queryHandlerFactories.ToList();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes a save entity command.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="command">The save entity command.</param>
        /// <returns>The save entity command response.</returns>
        public async Task<SaveEntityCommandResponse<TEntity>> Execute<TEntity>(StorageServiceReference ServiceReference, SaveEntityCommand<TEntity> command) where TEntity : IEntity
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault()
                ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            ICommandHandler<SaveEntityCommand<TEntity>, SaveEntityCommandResponse<TEntity>> handler = handlerFactory.GetSaveCommandHandler<TEntity>(this);
            var result = await handler.Execute(ServiceReference, command, this);

            if (result.IsSuccessful && result.Result is not null)
                result.Result.IsRetrievedFromStorage = true;

            return result;
        }

        /// <summary>
        /// Executes a record query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The record query.</param>
        /// <returns>The entity result of the query.</returns>
        public async Task<TEntity?> Execute<TEntity>(StorageServiceReference ServiceReference, IRecordQuery<TEntity> query) where TEntity : IEntity
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault() ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            IRecordQueryHandler<IRecordQuery<TEntity>, TEntity> handler = handlerFactory.GetEntityQueryHandler<TEntity>(this);
            handler.ServiceReference = ServiceReference;
            handler.Query = query;

            return await handler.Get(this);
        }

        /// <summary>
        /// Executes an enumerable query.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The enumerable query.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The enumerable result of the query.</returns>
        public async IAsyncEnumerable<TEntity> Execute<TEntity>(StorageServiceReference ServiceReference, IEnumerableQuery<TEntity> query, QueryParameters? queryParameters = null) where TEntity : class, IEntity
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault() ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            IEnumerableQueryHandler<IEnumerableQuery<TEntity>, TEntity> handler = handlerFactory.GetEntitiesQueryHandler<TEntity>(this);
            handler.ServiceReference = ServiceReference;
            handler.Query = query;

            await foreach (var i in handler.Get(this, queryParameters))
            {
                yield return i;
            }
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The enumerable result of the query.</returns>
        public async IAsyncEnumerable<TResponse> ExecuteQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query, QueryParameters? queryParameters = null)
           where TQuery : IQuery<TResponse>
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault() ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            var handler = handlerFactory.GetQueryHandler<TQuery, TResponse>(this)
                ?? throw new NotImplementedException("Handler not found.");

            handler.ServiceReference = ServiceReference;
            handler.Query = query;

            await foreach (var i in handler.Get(this, queryParameters))
            {
                yield return i;
            }
        }

        /// <summary>
        /// Executes a record query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query.</param>
        /// <returns>The response of the query.</returns>
        public async Task<TResponse?> ExecuteRecordQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query)
            where TQuery : IRecordQuery<TResponse>
            where TResponse : IEntity
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault() ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            var handler = handlerFactory.GetQueryHandler<TQuery, TResponse>(this)
                ?? throw new NotImplementedException("Handler not found.");

            handler.ServiceReference = ServiceReference;
            handler.Query = query;

            var resultAsync = handler.Get(this);

            await foreach (var i in resultAsync)
            {
                return i;
            }

            return default;
        }

        /// <summary>
        /// Executes an enumerable query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>The enumerable result of the query.</returns>
        public async IAsyncEnumerable<TResponse> ExecuteEnumerableQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query, QueryParameters? queryParameters = null)
            where TQuery : IEnumerableQuery<TResponse>
            where TResponse : IEntity
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault() ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            var handler = handlerFactory.GetQueryHandler<TQuery, TResponse>(this)
                ?? throw new NotImplementedException("Handler not found.");

            handler.ServiceReference = ServiceReference;
            handler.Query = query;

            await foreach (var i in handler.Get(this, queryParameters))
            {
                yield return i;
            }
        }

        /// <summary>
        /// Executes a command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="command">The command.</param>
        /// <returns>The response of the command.</returns>
        public async Task<TResponse> ExecuteCommand<TCommand, TResponse>(StorageServiceReference ServiceReference, TCommand command)
        where TResponse : ICommandResponse
        where TCommand : ICommand<TResponse>
        {
            var storageConnector = _entityMetadataService.GetMetadata<IStorageConnector>(ServiceReference).SingleOrDefault()
                ?? throw new Exception($"Storage connector not found for ServiceReference '{ServiceReference}'.");

            var handlerFactory = _dataHandlerServices.FirstOrDefault(x => x.IsHandled(storageConnector))
                ?? throw new Exception($"Could not find a data handler service for storage connector '{storageConnector.GetType().FullName}'.");

            var handler = handlerFactory.GetCommandHandler<TCommand, TResponse>(this)
                ?? throw new NotImplementedException("Handler not found.");


            var result = await handler.Execute(ServiceReference, command, this);

            return result;
        }

        #endregion

    }

}
