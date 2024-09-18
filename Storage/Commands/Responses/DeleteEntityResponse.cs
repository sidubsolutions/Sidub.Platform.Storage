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

namespace Sidub.Platform.Storage.Commands.Responses
{

    /// <summary>
    /// Represents the response of a delete entity command.
    /// </summary>
    public class DeleteEntityCommandResponse : ICommandResponse
    {

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating whether the delete entity command was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteEntityCommandResponse"/> class.
        /// </summary>
        /// <param name="isSuccessful">A value indicating whether the delete entity command was successful.</param>
        public DeleteEntityCommandResponse(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        #endregion

    }

}
