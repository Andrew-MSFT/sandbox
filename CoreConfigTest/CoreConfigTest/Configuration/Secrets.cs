﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConfigTest.Configuration
{
    public class Secrets
    {
        private Dictionary<string, object> _values;

        public object this[string key]
        {
            get
            {
                return _values[key];
            }
        }
        public Secrets()
        {
            _values = new Dictionary<string, object>();
        }

        public Secrets(IEnumerable<KeyValuePair<string, object>> values)
        {
            _values = new Dictionary<string, object>();
            AddSecrets(values);
        }

        public void AddSecret(string key, object value)
        {
            _values.Add(key, value);
        }

        public void AddSecrets(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach (var val in values)
            {
                AddSecret(val.Key, val.Value);
            }
        }
    }
}
