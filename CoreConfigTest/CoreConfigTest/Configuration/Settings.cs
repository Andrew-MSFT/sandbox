using System.Collections.Generic;

namespace CoreConfigTest.Configuration
{
    public class Settings
    {
        private Dictionary<string, object> _values;

        public object this[string key]
        {
            get
            {
                return _values[key];
            }
        }
        public Settings()
        {
            _values = new Dictionary<string, object>();
        }

        public Settings(IEnumerable<KeyValuePair<string,object>> values)
        {
            _values = new Dictionary<string, object>();
            AddSettings(values);
        }

        public void AddSetting(string key, object value)
        {
            _values.Add(key, value);
        }

        public void AddSettings(IEnumerable<KeyValuePair<string,object>> values)
        {
            foreach(var val in values)
            {
                AddSetting(val.Key, val.Value);
            }
        }
    }
}
