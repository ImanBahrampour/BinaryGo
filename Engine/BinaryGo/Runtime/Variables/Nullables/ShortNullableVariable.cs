﻿using BinaryGo.Binary.Deserialize;
using BinaryGo.Interfaces;
using BinaryGo.IO;
using BinaryGo.Json;
using System;

namespace BinaryGo.Runtime.Variables.Nullables
{
    /// <summary>
    /// Short serializer and deserializer
    /// </summary>
    public class ShortNullableVariable : BaseVariable, ISerializationVariable<short?>
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public ShortNullableVariable() : base(typeof(short?))
        {

        }
        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<short?> typeGoInfo, ITypeOptions options)
        {
            typeGoInfo.IsNoQuotesValueType = false;

            //set the default value of variable
            typeGoInfo.DefaultValue = default;

            //set delegates to access faster and make it pointer directly usage
            typeGoInfo.JsonSerialize = JsonSerialize;

            //set delegates to access faster and make it pointer directly usage for json deserializer
            typeGoInfo.JsonDeserialize = JsonDeserialize;

            //set delegates to access faster and make it pointer directly usage for binary serializer
            typeGoInfo.BinarySerialize = BinarySerialize;

            //set delegates to access faster and make it pointer directly usage for binary deserializer
            typeGoInfo.BinaryDeserialize = BinaryDeserialize;
        }

        /// <summary>
        /// json serialize
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="value"></param>
        public void JsonSerialize(ref JsonSerializeHandler handler, ref short? value)
        {
            if (value.HasValue)
                handler.TextWriter.Write(value.Value.ToString(CurrentCulture));
            else
                handler.TextWriter.Write(JsonConstantsString.Null);
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public short? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (short.TryParse(text, out short value))
                return value;
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref short? value)
        {
            if (value.HasValue)
            {
                stream.Write(1);
                stream.Write(BitConverter.GetBytes(value.Value).AsSpan());
            }
            else
                stream.Write(0);
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public short? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
                return BitConverter.ToInt16(reader.Read(sizeof(short)));
            return default;
        }
    }
}
