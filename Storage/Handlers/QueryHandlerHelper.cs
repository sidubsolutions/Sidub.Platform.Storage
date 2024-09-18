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
using Sidub.Platform.Core.Entity;
using Sidub.Platform.Core.Entity.Relations;
using Sidub.Platform.Storage.Queries;
using Sidub.Platform.Storage.Services;

#endregion

namespace Sidub.Platform.Storage.Handlers
{

    /// <summary>
    /// Helper class for query handlers.
    /// </summary>
    public static class QueryHandlerHelper
    {

        #region Public static methods

        /// <summary>
        /// Assigns entity reference providers for the given entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="queryService">The query service.</param>
        /// <param name="serviceReference">The storage service reference.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>The entity with assigned entity reference providers.</returns>
        public static async Task<TEntity> AssignEntityReferenceProviders<TEntity>(IQueryService queryService, StorageServiceReference serviceReference, TEntity entity) where TEntity : IEntity
        {
            // NOTE, providers are currently assigned individually per record; in enumerable relationships, we simply generate a
            //  query / provider for each individual record in that enumerable relationship... this means individual queries are
            //  executed 

            var relations = EntityTypeHelper.GetEntityRelations<TEntity>();

            foreach (var relation in relations)
            {
                var action = async Task<IEntity?> (IDictionary<IEntityField, object> entityKeys) =>
                {
                    var query = RelationQuery.CreateRecordQuery(relation, entityKeys);

                    Type queryType;

                    queryType = typeof(IRecordQuery<>);

                    // todo make this more robust...
                    var queryExecuteMethod = typeof(IQueryService).GetMethods().Single(x =>
                            x.Name == nameof(IQueryService.Execute)
                            && x.GetParameters()[0].ParameterType == typeof(StorageServiceReference)
                            && x.GetParameters()[1].ParameterType.IsGenericType
                            && x.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == queryType
                        )
                        .MakeGenericMethod(relation.RelatedType);

                    object?[] parameters;

                    if (queryType == typeof(IEnumerableQuery<>))
                    {
                        parameters = new object?[] { serviceReference, query, null };
                    }
                    else
                        parameters = new object[] { serviceReference, query };

                    var queryExecuteTask = queryExecuteMethod.Invoke(queryService, parameters) as Task ?? throw new Exception("Null task..");

                    await queryExecuteTask;
                    // todo...
                    var result = queryExecuteTask.GetType().GetProperty("Result")?.GetValue(queryExecuteTask) as IEntity;

                    return result;
                };


                if (relation.IsEnumerableRelation)
                {
                    var baseEntityReferences = EntityTypeHelper.GetEntityRelationEnumerable(entity, relation)
                        ?? throw new Exception("Failed to get entity reference list.");

                    foreach (var i in baseEntityReferences)
                    {
                        i.Provider = action;
                    }

                    if (relation.LoadType == EntityRelationLoadType.Eager)
                    {
                        foreach (var i in baseEntityReferences)
                            _ = await i.Get();
                    }

                    EntityTypeHelper.SetEntityRelationReference(entity, relation, baseEntityReferences);
                }
                else
                {
                    var baseEntityReference = EntityTypeHelper.GetEntityRelationRecord(entity, relation);

                    if (baseEntityReference is not null)
                    {
                        baseEntityReference.Provider = action;

                        if (relation.LoadType == EntityRelationLoadType.Eager)
                            _ = await baseEntityReference.Get();
                    }

                    EntityTypeHelper.SetEntityRelationReference(entity, relation, baseEntityReference);
                }

            }

            return entity;
        }

        #endregion

    }

}
