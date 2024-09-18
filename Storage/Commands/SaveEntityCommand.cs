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
    /// Represents a command to save an entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class SaveEntityCommand<TEntity> : ICommand<SaveEntityCommandResponse<TEntity>> where TEntity : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the entity to be saved.
        /// </summary>
        public TEntity Entity { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveEntityCommand{TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The entity to be saved.</param>
        public SaveEntityCommand(TEntity entity)
        {
            Entity = entity;
        }

        #endregion

    }

    /// <summary>
    /// Provides factory methods to create instances of <see cref="SaveEntityCommand{TEntity}"/>.
    /// </summary>
    public static class SaveEntityCommand
    {

        #region Public static methods

        /// <summary>
        /// Creates a new instance of <see cref="SaveEntityCommand{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to be saved.</param>
        /// <returns>A new instance of <see cref="SaveEntityCommand{TEntity}"/>.</returns>
        public static SaveEntityCommand<TEntity> Create<TEntity>(TEntity entity) where TEntity : IEntity
        {
            return new SaveEntityCommand<TEntity>(entity);
        }

        #endregion

    }

}
