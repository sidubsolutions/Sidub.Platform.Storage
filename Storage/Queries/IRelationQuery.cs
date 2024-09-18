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

#endregion

namespace Sidub.Platform.Storage.Queries
{

    /// <summary>
    /// Represents a query for retrieving related entities.
    /// </summary>
    public interface IRelationQuery
    {

        #region Interface properties

        /// <summary>
        /// Gets the entity relation context associated with the query.
        /// </summary>
        IEntityRelationContext EntityRelationContext { get; }

        #endregion

    }

    /// <summary>
    /// Provides methods for creating different types of queries related to entity relations.
    /// </summary>
    public static class RelationQuery
    {

        #region Public static methods

        /// <summary>
        /// Creates a record query for the specified entity relation and entity keys.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="entityKeys">The entity keys.</param>
        /// <returns>The record query.</returns>
        public static IRecordQuery CreateRecordQuery(IEntityRelation relation, IDictionary<IEntityField, object> entityKeys)
        {
            IRecordQuery? query = relation.Relationship switch
            {
                EntityRelationshipType.Association => Activator.CreateInstance(typeof(AssociationRecordQuery<>).MakeGenericType(relation.RelatedType), entityKeys, relation) as IRecordQuery,
                _ => Activator.CreateInstance(typeof(AssociationRecordQuery<>).MakeGenericType(relation.RelatedType), entityKeys, relation) as IRecordQuery
            } ?? throw new Exception("Null query encountered in creating entity reference.");

            return query;
        }

        /// <summary>
        /// Creates an enumerable query for the specified entity relation and entity keys.
        /// </summary>
        /// <param name="relation">The entity relation.</param>
        /// <param name="entityKeys">The entity keys.</param>
        /// <returns>The enumerable query.</returns>
        public static IEnumerableQuery CreateEnumerableQuery(IEntityRelation relation, IDictionary<IEntityField, object> entityKeys)
        {
            IEnumerableQuery? query = relation.Relationship switch
            {
                EntityRelationshipType.Association => Activator.CreateInstance(typeof(AssociationEnumerableQuery<>).MakeGenericType(relation.RelatedType), entityKeys, relation) as IEnumerableQuery,
                _ => Activator.CreateInstance(typeof(AssociationEnumerableQuery<>).MakeGenericType(relation.RelatedType), entityKeys, relation) as IEnumerableQuery
            } ?? throw new Exception("Null query encountered in creating entity reference.");

            return query;
        }

        #endregion

    }

}
