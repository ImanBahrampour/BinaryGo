﻿using JsonGo.Binary.Deserialize;
using JsonGo.Interfaces;
using JsonGo.IO;
using JsonGo.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace JsonGo.Runtime.Variables.Enums
{
    /// <summary>
    /// Enum that inheritance ulong
    /// </summary>
    public class EnumNullableULongVariable<TEnum> : BaseVariable, ISerializationVariable<TEnum?>
         where TEnum : struct, Enum
    {
        /// <summary>
        /// default constructor to initialize
        /// </summary>
        public EnumNullableULongVariable() : base(typeof(TEnum?))
        {

        }

        /// <summary>
        /// Initalizes TypeGo variable
        /// </summary>
        /// <param name="typeGoInfo">TypeGo variable to initialize</param>
        /// <param name="options">Serializer or deserializer options</param>
        public void Initialize(TypeGoInfo<TEnum?> typeGoInfo, ITypeOptions options)
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
        public void JsonSerialize(ref JsonSerializeHandler handler, ref TEnum? value)
        {
            if (value.HasValue)
            {
                var data = value.Value;
                handler.TextWriter.Write(Unsafe.As<TEnum, ulong>(ref data).ToString(CurrentCulture));
            }
            else
            {
                handler.TextWriter.Write(JsonConstantsString.Null);
            }
        }

        /// <summary>
        /// json deserialize
        /// </summary>
        /// <param name="text">json text</param>
        /// <returns>convert text to type</returns>
        public TEnum? JsonDeserialize(ref ReadOnlySpan<char> text)
        {
            if (ulong.TryParse(text, out ulong value))
                return Unsafe.As<ulong, TEnum>(ref value);
            return default;
        }

        /// <summary>
        /// Binary serialize
        /// </summary>
        /// <param name="stream">stream to write</param>
        /// <param name="value">value to serialize</param>
        public void BinarySerialize(ref BufferBuilder stream, ref TEnum? value)
        {
            if (value.HasValue)
            {
                var data = value.Value;
                stream.Write(BitConverter.GetBytes(Unsafe.As<TEnum, ulong>(ref data)));
            }
            else
            {
                stream.Write(0);
            }
        }

        /// <summary>
        /// Binary deserialize
        /// </summary>
        /// <param name="reader">Reader of binary</param>
        public TEnum? BinaryDeserialize(ref BinarySpanReader reader)
        {
            if (reader.Read() == 1)
            {
                var value = BitConverter.ToUInt64(reader.Read(sizeof(ulong)));
                return Unsafe.As<ulong, TEnum>(ref value);
            }
            return default;
        }
    }
}