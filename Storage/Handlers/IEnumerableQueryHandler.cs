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
using Sidub.Platform.Storage.Queries;
using Sidub.Platform.Storage.Services;

#endregion

namespace Sidub.Platform.Storage.Handlers
{

    /// <summary>
    /// Represents a handler for query operations on a collection of entities.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IEnumerableQueryHandler<in TQuery, TEntity> : IQueryHandler<TQuery, TEntity> where TQuery : IEnumerableQuery<TEntity> where TEntity : IEntity
    {

        #region Interface methods

        /// <summary>
        /// Retrieves a collection of entities based on the specified query and query parameters.
        /// </summary>
        /// <param name="queryService">The query service used to execute the query.</param>
        /// <param name="queryParameters">The optional query parameters.</param>
        /// <returns>An asynchronous enumerable of entities.</returns>
        new IAsyncEnumerable<TEntity> Get(IQueryService queryService, QueryParameters? queryParameters);

        #endregion

    }

}
