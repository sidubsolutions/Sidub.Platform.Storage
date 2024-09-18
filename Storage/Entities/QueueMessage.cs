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

using Sidub.Platform.Core.Attributes;
using Sidub.Platform.Core.Entity;
using System.Text;

#endregion

namespace Sidub.Platform.Storage.Entities
{

    /// <summary>
    /// Represents a message in a queue.
    /// </summary>
    [Entity("QueueMessage")]
    public class QueueMessage : IEntity
    {

        #region Public properties

        /// <summary>
        /// Gets or sets the unique identifier of the message.
        /// </summary>
        [EntityKey<Guid?>("MessageId")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the data of the message.
        /// </summary>
        [EntityField<byte[]>("MessageText")]
        public byte[] MessageData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is retrieved from storage.
        /// </summary>
        public bool IsRetrievedFromStorage { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueMessage"/> class.
        /// </summary>
        public QueueMessage()
        {
            MessageData = new byte[] { };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueMessage"/> class with the specified message data.
        /// </summary>
        /// <param name="messageData">The data of the message.</param>
        public QueueMessage(byte[] messageData)
        {
            MessageData = messageData;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueMessage"/> class with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public QueueMessage(string message)
        {
            MessageData = Encoding.UTF8.GetBytes(message);
        }

        #endregion

    }

}
