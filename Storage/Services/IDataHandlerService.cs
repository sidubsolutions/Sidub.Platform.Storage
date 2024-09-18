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
using Sidub.Platform.Storage.Commands;
using Sidub.Platform.Storage.Commands.Responses;
using Sidub.Platform.Storage.Connectors;
using Sidub.Platform.Storage.Handlers;
using Sidub.Platform.Storage.Queries;

#endregion

namespace Sidub.Platform.Storage.Services
{

    /// <summary>
    /// Represents a service which provides data handlers for a given data source type.
    /// </summary>
    public interface IDataHandlerService//<in TStorageConnector> where TStorageConnector : IStorageConnector
    {

        #region Interface methods

        /// <summary>
        /// Determines whether the specified connector is handled by this service.
        /// </summary>
        /// <param name="connector">The storage connector.</param>
        /// <returns>True if the connector is handled, otherwise false.</returns>
        bool IsHandled(IStorageConnector connector);

        /// <summary>
        /// Gets the command handler for saving an entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <returns>The command handler for saving an entity.</returns>
        ICommandHandler<SaveEntityCommand<TEntity>, SaveEntityCommandResponse<TEntity>> GetSaveCommandHandler<TEntity>(IQueryService queryService) where TEntity : IEntity;

        /// <summary>
        /// Gets the query handler for retrieving a single entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <returns>The query handler for retrieving a single entity.</returns>
        IRecordQueryHandler<IRecordQuery<TEntity>, TEntity> GetEntityQueryHandler<TEntity>(IQueryService queryService) where TEntity : IEntity;

        /// <summary>
        /// Gets the query handler for retrieving multiple entities.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <returns>The query handler for retrieving multiple entities.</returns>
        IEnumerableQueryHandler<IEnumerableQuery<TEntity>, TEntity> GetEntitiesQueryHandler<TEntity>(IQueryService queryService) where TEntity : class, IEntity;

        /// <summary>
        /// Gets the query handler for executing a query.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the query response.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <returns>The query handler for executing a query.</returns>
        IQueryHandler<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>(IQueryService queryService)
            where TQuery : IQuery<TResponse>;

        /// <summary>
        /// Gets the command handler for executing a command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResponse">The type of the command response.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <returns>The command handler for executing a command.</returns>
        ICommandHandler<TCommand, TResponse>? GetCommandHandler<TCommand, TResponse>(IQueryService queryService)
            where TCommand : ICommand<TResponse>
            where TResponse : ICommandResponse;

        #endregion

    }

}
