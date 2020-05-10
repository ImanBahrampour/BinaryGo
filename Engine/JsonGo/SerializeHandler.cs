﻿using JsonGo.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonGo
{
    public class SerializeHandler
    {
        public Func<string, StringBuilder> Append { get; set; }
        public Func<char, StringBuilder> AppendChar { get; set; }
        public Serializer Serializer { get; set; }
        public Action<object, int> AddSerializedObjects { get; set; }
        public TryGetValue<object, int> TryGetValueOfSerializedObjects { get; set; }
    }
}
