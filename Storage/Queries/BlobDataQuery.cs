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
using Sidub.Platform.Filter;

#endregion

namespace Sidub.Platform.Storage.Queries
{

    /// <summary>
    /// Represents a query for retrieving blob data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class BlobDataQuery<TEntity> : IQuery<TEntity> where TEntity : class, IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the path of the blob.
        /// </summary>
        public string BlobPath { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobDataQuery{TEntity}"/> class.
        /// </summary>
        /// <param name="blobPath">The path of the blob.</param>
        public BlobDataQuery(string blobPath)
        {
            BlobPath = blobPath;
        }

        /// <summary>
        /// Gets the filter associated with the query.
        /// </summary>
        /// <returns>The filter associated with the query.</returns>
        IFilter? IQuery<TEntity>.GetFilter()
        {
            throw new Exception("Blob data queries do not support filters.");
        }

        #endregion

    }

}
