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
using Sidub.Platform.Core.Entity.Relations;
using Sidub.Platform.Storage.Commands.Responses;

#endregion

namespace Sidub.Platform.Storage.Commands
{

    /// <summary>
    /// Represents a command for saving an entity relation.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    public interface ISaveEntityRelationCommand : ICommand<SaveEntityRelationCommandResponse>
    {

        #region Interface properties

        /// <summary>
        /// Gets the type of the parent entity.
        /// </summary>
        internal Type ParentType { get; }

        /// <summary>
        /// Gets the type of the related entity.
        /// </summary>
        internal Type RelatedType { get; }

        /// <summary>
        /// Gets the entity relation.
        /// </summary>
        public IEntityRelation Relation { get; }

        /// <summary>
        /// Gets a value indicating whether the related entity is deleted.
        /// </summary>
        public bool IsDeleted { get; }

        #endregion

    }

    /// <summary>
    /// Represents a command for saving an entity relation.
    /// </summary>
    /// <typeparam name="TParent">The type of the parent entity.</typeparam>
    /// <typeparam name="TRelated">The type of the related entity.</typeparam>
    public class SaveEntityRelationCommand<TParent, TRelated> : ISaveEntityRelationCommand
        where TParent : IEntity
        where TRelated : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the entity relation.
        /// </summary>
        public IEntityRelation Relation { get; }

        /// <summary>
        /// Gets the parent entity.
        /// </summary>
        public TParent ParentEntity { get; }

        /// <summary>
        /// Gets the related entity reference.
        /// </summary>
        public IEntityReference RelatedEntity { get; }

        /// <summary>
        /// Gets the type of the parent entity.
        /// </summary>
        Type ISaveEntityRelationCommand.ParentType => typeof(TParent);

        /// <summary>
        /// Gets the type of the related entity.
        /// </summary>
        Type ISaveEntityRelationCommand.RelatedType => typeof(TRelated);

        /// <summary>
        /// Gets a value indicating whether the related entity is deleted.
        /// </summary>
        public bool IsDeleted => RelatedEntity.Action == EntityRelationActionType.Delete;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveEntityRelationCommand{TParent, TRelated}"/> class.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="parentEntity">The parent entity.</param>
        /// <param name="relatedEntity">The related entity reference.</param>
        public SaveEntityRelationCommand(IEntityRelation relation, TParent parentEntity, IEntityReference relatedEntity)
        {
            Relation = relation;
            ParentEntity = parentEntity;
            RelatedEntity = relatedEntity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveEntityRelationCommand{TParent, TRelated}"/> class.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="parentEntity">The parent entity.</param>
        /// <param name="relatedEntity">The related entity.</param>
        public SaveEntityRelationCommand(IEntityRelation relation, TParent parentEntity, TRelated relatedEntity)
        {
            Relation = relation;
            ParentEntity = parentEntity;
            RelatedEntity = EntityReference.Create(relatedEntity);
        }

        #endregion

    }

    /// <summary>
    /// Provides a static method for creating an instance of <see cref="ISaveEntityRelationCommand"/>.
    /// </summary>
    public static class SaveEntityRelationCommand
    {

        #region Public static methods

        /// <summary>
        /// Creates an instance of <see cref="ISaveEntityRelationCommand"/>.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent entity.</typeparam>
        /// <param name="relation">The entity relation.</param>
        /// <param name="parentEntity">The parent entity.</param>
        /// <param name="relatedEntity">The related entity reference.</param>
        /// <returns>An instance of <see cref="ISaveEntityRelationCommand"/>.</returns>
        public static ISaveEntityRelationCommand Create<TParent>(IEntityRelation relation, TParent parentEntity, IEntityReference relatedEntity)
            where TParent : IEntity
        {
            var parentType = parentEntity.GetType();
            var relatedType = relatedEntity.ConcreteType?.GetDefinedType()
                ?? relatedEntity.EntityType;

            var t = typeof(SaveEntityRelationCommand<,>).MakeGenericType([parentType, relatedType]);

            var cmdArgs = new object[] { relation, parentEntity, relatedEntity };

            var cmd = Activator.CreateInstance(t, cmdArgs);
            var cmdT = cmd as ISaveEntityRelationCommand
                ?? throw new Exception("Failed to create an instance of ISaveEntityRelationCommand.");

            return cmdT;
        }

        #endregion

    }

}
