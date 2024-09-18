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
using Sidub.Platform.Filter;

#endregion

namespace Sidub.Platform.Storage.Queries
{

    /// <summary>
    /// Represents a composite enumerable query that implements both <see cref="IRelationQuery"/> and <see cref="IEnumerableQuery{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class CompositeEnumerableQuery<TEntity> : IRelationQuery, IEnumerableQuery<TEntity>
        where TEntity : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets the entity relation.
        /// </summary>
        public IEntityRelation Relation { get; }

        /// <summary>
        /// Gets or sets the entity relation context.
        /// </summary>
        public IEntityRelationContext EntityRelationContext { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeEnumerableQuery{TEntity}"/> class.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        public CompositeEnumerableQuery(IEntityRelation relation)
        {
            Relation = relation;
            EntityRelationContext = new EntityCompositionRelationContext(Relation);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the filter for the query.
        /// </summary>
        /// <returns>The filter for the query.</returns>
        public virtual IFilter? GetFilter()
        {
            return null;
        }

        #endregion

    }

}
