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
using Sidub.Platform.Storage.Commands.Responses;

#endregion

namespace Sidub.Platform.Storage.Commands
{

    /// <summary>
    /// Represents an action command with parameters of type <typeparamref name="TParameters"/>.
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters for the action command.</typeparam>
    public interface IActionCommand<TParameters> : ICommand<ActionCommandResponse>
        where TParameters : IEntity
    {

        #region Interface properties

        /// <summary>
        /// Gets the action command type.
        /// </summary>
        ActionCommandType ActionCommand { get; }

        /// <summary>
        /// Gets or sets the parameters for the action command.
        /// </summary>
        TParameters Parameters { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents an action command with parameters of type <typeparamref name="TParameters"/> and response entity of type <typeparamref name="TResponseEntity"/>.
    /// </summary>
    /// <typeparam name="TParameters">The type of parameters for the action command.</typeparam>
    /// <typeparam name="TResponseEntity">The type of response entity for the action command.</typeparam>
    public interface IActionCommand<TParameters, TResponseEntity> : ICommand<ActionCommandResponse<TResponseEntity>>
        where TParameters : IEntity
        where TResponseEntity : IEntity
    {

        #region Interface properties

        /// <summary>
        /// Gets the action command type.
        /// </summary>
        ActionCommandType ActionCommand { get; }

        /// <summary>
        /// Gets or sets the parameters for the action command.
        /// </summary>
        TParameters Parameters { get; set; }

        #endregion

    }

}
