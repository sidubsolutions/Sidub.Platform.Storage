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

using Sidub.Platform.Storage.Connectors;
using Sidub.Platform.Storage.Queries;

#endregion

namespace Sidub.Platform.Storage.Handlers
{

    /// <summary>
    /// Represents a factory for creating query handlers.
    /// </summary>
    /// <typeparam name="TStorageConnector">The type of the storage connector.</typeparam>
    public interface IQueryHandlerFactory<in TStorageConnector> where TStorageConnector : IStorageConnector
    {

        #region Interface methods

        /// <summary>
        /// Determines if the specified query and response types are handled by this factory.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns><c>true</c> if the query and response types are handled; otherwise, <c>false</c>.</returns>
        bool IsHandled<TQuery, TResponse>() where TQuery : IQuery<TResponse>;

        /// <summary>
        /// Creates a query handler for the specified query and response types.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <returns>A query handler for the specified query and response types.</returns>
        IQueryHandler<TQuery, TResponse> Create<TQuery, TResponse>() where TQuery : IQuery<TResponse>;

        #endregion

    }

}
