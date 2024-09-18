﻿/*
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

#endregion

namespace Sidub.Platform.Storage.Commands.Responses
{

    /// <summary>
    /// Represents a response for an action command.
    /// </summary>
    public class ActionCommandResponse : ICommandResponse
    {

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating whether the action command was successful.
        /// </summary>
        public bool IsSuccessful { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents a response for an action command with a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ActionCommandResponse<TResult> : ActionCommandResponse where TResult : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets or sets the result of the action command.
        /// </summary>
        public TResult? Result { get; set; }

        #endregion

    }

}
