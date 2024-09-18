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
using Sidub.Platform.Core.Entity.Relations;
using Sidub.Platform.Storage.Commands;
using Sidub.Platform.Storage.Commands.Responses;
using Sidub.Platform.Storage.Queries;

#endregion

namespace Sidub.Platform.Storage.Services
{

    /// <summary>
    /// Represents a service for executing queries and commands related to storage.
    /// </summary>
    public interface IQueryService
    {

        #region Interface methods

        /// <summary>
        /// Executes a command to save an entity in the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="command">The save entity command.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response of the save entity command.</returns>
        Task<SaveEntityCommandResponse<TEntity>> Execute<TEntity>(StorageServiceReference storage, SaveEntityCommand<TEntity> command) where TEntity : IEntity;

        /// <summary>
        /// Executes a query to retrieve an entity from the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="query">The record query.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the retrieved entity or null if not found.</returns>
        Task<TEntity?> Execute<TEntity>(StorageServiceReference storage, IRecordQuery<TEntity> query) where TEntity : IEntity;

        /// <summary>
        /// Executes a query to retrieve multiple entities from the storage.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entities.</typeparam>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="query">The enumerable query.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>An asynchronous enumerable of entities.</returns>
        IAsyncEnumerable<TEntity> Execute<TEntity>(StorageServiceReference storage, IEnumerableQuery<TEntity> query, QueryParameters? queryParameters = null) where TEntity : class, IEntity;

        /// <summary>
        /// Executes a query and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the query response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>An asynchronous enumerable of query responses.</returns>
        IAsyncEnumerable<TResponse> ExecuteQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query, QueryParameters? queryParameters = null)
            where TQuery : IQuery<TResponse>;

        /// <summary>
        /// Executes a record query and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the query response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the query response or null if not found.</returns>
        Task<TResponse?> ExecuteRecordQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query)
            where TQuery : IRecordQuery<TResponse>
            where TResponse : IEntity;

        /// <summary>
        /// Executes an enumerable query and returns the result.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the query response.</typeparam>
        /// <param name="ServiceReference">The storage service reference.</param>
        /// <param name="query">The query to execute.</param>
        /// <param name="queryParameters">The query parameters.</param>
        /// <returns>An asynchronous enumerable of query responses.</returns>
        IAsyncEnumerable<TResponse> ExecuteEnumerableQuery<TQuery, TResponse>(StorageServiceReference ServiceReference, TQuery query, QueryParameters? queryParameters = null)
            where TQuery : IEnumerableQuery<TResponse>
            where TResponse : IEntity;

        /// <summary>
        /// Executes a command and returns the response.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TCommandResponse">The type of the command response.</typeparam>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the command response.</returns>
        Task<TCommandResponse> ExecuteCommand<TCommand, TCommandResponse>(StorageServiceReference storage, TCommand command)
            where TCommand : ICommand<TCommandResponse>
            where TCommandResponse : ICommandResponse;

        #endregion

    }

    /// <summary>
    /// Provides extension methods for the <see cref="IQueryService"/> interface.
    /// </summary>
    public static class IQueryServiceExtensions
    {

        #region Public static methods

        /// <summary>
        /// Executes a save entity relation command and returns the response.
        /// </summary>
        /// <typeparam name="TQueryService">The type of the query service.</typeparam>
        /// <param name="queryService">The query service instance.</param>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="saveRelationCommand">The save entity relation command.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response of the save entity relation command.</returns>
        public static async Task<ISaveEntityRelationCommandResponse> Execute<TQueryService>(this TQueryService queryService, StorageServiceReference storage, ISaveEntityRelationCommand saveRelationCommand)
            where TQueryService : IQueryService
        {
            var saveRelationCommandType = typeof(SaveEntityRelationCommand<,>).MakeGenericType(saveRelationCommand.ParentType, saveRelationCommand.RelatedType);

            var saveExecuteMethod = queryService.GetType()
                .GetMethods()
                .Single(x => x.Name == nameof(IQueryService.ExecuteCommand)
                    && x.GetParameters()[0].ParameterType == typeof(StorageServiceReference))
                .MakeGenericMethod(saveRelationCommandType, typeof(SaveEntityRelationCommandResponse));

            var saveExecuteTask = saveExecuteMethod.Invoke(queryService, new object[] { storage, saveRelationCommand }) as Task
                ?? throw new Exception("Null encountered while invoking query service method.");

            await saveExecuteTask;

            var resultRaw = saveExecuteTask.GetType().GetProperty("Result")?.GetValue(saveExecuteTask);
            var result = resultRaw as ISaveEntityRelationCommandResponse
                ?? throw new Exception("Null result encountered after invoking query service method");

            return result;
        }

        /// <summary>
        /// Executes a delete entity command and returns the response.
        /// </summary>
        /// <typeparam name="TQueryService">The type of the query service.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service instance.</param>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="deleteEntityCommand">The delete entity command.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the delete entity command response.</returns>
        public static async Task<DeleteEntityCommandResponse> Execute<TQueryService, TEntity>(this TQueryService queryService, StorageServiceReference storage, DeleteEntityCommand<TEntity> deleteEntityCommand) where TQueryService : IQueryService where TEntity : IEntity
        {
            var result = await queryService.ExecuteCommand<DeleteEntityCommand<TEntity>, DeleteEntityCommandResponse>(storage, deleteEntityCommand);

            return result;
        }

        /// <summary>
        /// Executes an action command and returns the response.
        /// </summary>
        /// <typeparam name="TQueryService">The type of the query service.</typeparam>
        /// <typeparam name="TRequestEntity">The type of the request entity.</typeparam>
        /// <param name="queryService">The query service instance.</param>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="saveQueueMessage">The action command.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response of the action command.</returns>
        public static async Task<ActionCommandResponse> Execute<TQueryService, TRequestEntity>(this TQueryService queryService, StorageServiceReference storage, IActionCommand<TRequestEntity> saveQueueMessage) where TQueryService : IQueryService where TRequestEntity : IEntity
        {
            var result = await queryService.ExecuteCommand<IActionCommand<TRequestEntity>, ActionCommandResponse>(storage, saveQueueMessage);

            return result;
        }

        /// <summary>
        /// Executes an action command and returns the response.
        /// </summary>
        /// <typeparam name="TQueryService">The type of the query service.</typeparam>
        /// <typeparam name="TRequestEntity">The type of the request entity.</typeparam>
        /// <typeparam name="TResponseEntity">The type of the response entity.</typeparam>
        /// <param name="queryService">The query service instance.</param>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="saveQueueMessage">The action command.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response of the action command.</returns>
        public static async Task<ActionCommandResponse<TResponseEntity>> Execute<TQueryService, TRequestEntity, TResponseEntity>(this TQueryService queryService, StorageServiceReference storage, IActionCommand<TRequestEntity, TResponseEntity> saveQueueMessage) where TQueryService : IQueryService where TRequestEntity : IEntity where TResponseEntity : IEntity
        {
            var result = await queryService.ExecuteCommand<IActionCommand<TRequestEntity, TResponseEntity>, ActionCommandResponse<TResponseEntity>>(storage, saveQueueMessage);

            return result;
        }

        /// <summary>
        /// Executes a blob query and returns the result.
        /// </summary>
        /// <typeparam name="TQueryService">The type of the query service.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service instance.</param>
        /// <param name="storage">The storage service reference.</param>
        /// <param name="blobQuery">The blob query.</param>
        /// <returns>An asynchronous enumerable of entity references.</returns>
        public static async IAsyncEnumerable<EntityReference<TEntity>> Execute<TQueryService, TEntity>(this TQueryService queryService, StorageServiceReference storage, IBlobQuery<TEntity> blobQuery) where TQueryService : IQueryService where TEntity : class, IEntity
        {
            var result = queryService.ExecuteQuery<IBlobQuery<TEntity>, EntityReference<TEntity>>(storage, blobQuery);

            await foreach (var i in result)
            {
                yield return i;
            }
        }

        #endregion

    }

}
