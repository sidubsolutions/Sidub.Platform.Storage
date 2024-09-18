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

using Sidub.Platform.Core;

#endregion

namespace Sidub.Platform.Storage.Services
{

    /// <summary>
    /// Represents a data provider service that provides access to a specific data client.
    /// </summary>
    /// <typeparam name="TDataClient">The type of the data client.</typeparam>
    public interface IDataProviderService<out TDataClient>
    {

        #region Interface methods

        /// <summary>
        /// Gets the data client based on the specified service reference context.
        /// </summary>
        /// <param name="serviceReferenceContext">The service reference context.</param>
        /// <returns>The data client.</returns>
        TDataClient GetDataClient(ServiceReference serviceReferenceContext);

        #endregion

    }

}
