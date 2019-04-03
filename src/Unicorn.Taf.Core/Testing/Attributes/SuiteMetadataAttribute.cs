﻿using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MetadataAttribute : Attribute
    {
        public MetadataAttribute(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public string Key { get; protected set; }

        public string Value { get; protected set; }
    }
}