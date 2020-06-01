﻿using JsonGo.Interfaces;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsonGo.Runtime.Variables
{
    /// <summary>
    /// byte[] serializer and deserializer
    /// </summary>
    public class ByteArrayVariable : ISerializationVariable
    {
        /// <summary>
        /// initalize this variable to your typeGo
        /// </summary>
        /// <param name="typeGoInfo">typeGo to initialize variable on it</param>
        /// <param name="options">options of setting of variable serializer or deserializer</param>
        public void Initialize(TypeGoInfo typeGoInfo, ITypeGo options)
        {
            var currentCulture = TypeGoInfo.CurrentCulture;
            typeGoInfo.IsNoQuotesValueType = false;

            //json serialize
            typeGoInfo.JsonSerialize = (JsonSerializeHandler handler, ref object data) =>
            {
                handler.AppendChar(JsonConstantsString.Quotes);
                handler.Append(Convert.ToBase64String((byte[])data));
                handler.AppendChar(JsonConstantsString.Quotes);
            };

            //json deserialize of variable
            typeGoInfo.JsonDeserialize = (deserializer, x) =>
            {
                return Convert.FromBase64String(new string(x));
            };

            //binary serialization
            typeGoInfo.BinarySerialize = (Stream stream, ref object data) =>
            {
                var array = ((byte[])data).AsSpan();
                stream.Write(BitConverter.GetBytes(array.Length));
                stream.Write(array);
            };

            //set the default value of variable
            typeGoInfo.DefaultValue = default(byte[]);
        }
    }
}