// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AudioSwitcher.ApplicationModel.Settings
{
    internal class JsonSettingsService : ISettingService
    {
        private readonly TextReader _reader;

        public JsonSettingsService(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _reader = reader;
        }

        public bool GetBoolean(string name)
        {
            return GetValue<bool>(name);
        }

        public string GetString(string name)
        {
            return GetValue<string>(name);
        }

        private T GetValue<T>(string name)
        {
            using (var reader = new JsonTextReader(_reader))
            {
                var settings = JObject.Load(reader);

                JToken token = settings.GetValue(name);

                return token.Value<T>();
            }
        }
    }
}
