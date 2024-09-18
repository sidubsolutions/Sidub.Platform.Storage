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

using Sidub.Platform.Storage.Commands;
using Sidub.Platform.Storage.Commands.Responses;
using Sidub.Platform.Storage.Connectors;

#endregion

namespace Sidub.Platform.Storage.Handlers
{

    /// <summary>
    /// Represents a factory for creating command handlers.
    /// </summary>
    /// <typeparam name="TStorageConnector">The type of the storage connector.</typeparam>
    public interface ICommandHandlerFactory<in TStorageConnector> where TStorageConnector : IStorageConnector
    {

        #region Interface methods

        /// <summary>
        /// Determines whether the specified command and result types are handled by this factory.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResult">The type of the command result.</typeparam>
        /// <returns><c>true</c> if the command and result types are handled; otherwise, <c>false</c>.</returns>
        bool IsHandled<TCommand, TResult>() where TCommand : ICommand<TResult> where TResult : ICommandResponse;

        /// <summary>
        /// Creates a command handler for the specified command and result types.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TResult">The type of the command result.</typeparam>
        /// <returns>A command handler for the specified command and result types.</returns>
        ICommandHandler<TCommand, TResult> Create<TCommand, TResult>() where TCommand : ICommand<TResult> where TResult : ICommandResponse;

        #endregion

    }

}
