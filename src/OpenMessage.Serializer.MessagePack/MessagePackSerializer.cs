using OpenMessage.Serialization;
using System;
using System.Collections.Generic;
using serializer = MessagePack.MessagePackSerializer;

namespace OpenMessage.Serializer.MessagePack
{
    internal sealed class MessagePackSerializer : ISerializer, IDeserializer
    {
        private static readonly string _contentType = "binary/messagepack";

        public string ContentType { get; } = _contentType;

        public IEnumerable<string> SupportedContentTypes { get; } = new[] {_contentType};

        public byte[] AsBytes<T>(T entity)
        {
            if (entity is null)
                Throw.ArgumentNullException(nameof(entity));

            return serializer.Serialize(entity);
        }

        public string AsString<T>(T entity)
        {
            if (entity is null)
                Throw.ArgumentNullException(nameof(entity));

            return Convert.ToBase64String(AsBytes(entity));
        }

        public T? From<T>(string data, Type messageType) where T : class
        {
            if (string.IsNullOrWhiteSpace(data))
                Throw.ArgumentException(nameof(data), "Cannot be null, empty or whitespace");

            return From<T>(Convert.FromBase64String(data), messageType);
        }

        public T? From<T>(byte[] data, Type messageType) where T : class 
        {
            if (data is null || data.Length == 0)
                Throw.ArgumentException(nameof(data), "Cannot be null or empty");

            return (T)serializer.Deserialize(messageType, data);
        }
    }
}