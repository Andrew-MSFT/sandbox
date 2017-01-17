using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConfigTest.Configuration
{
    public class ConfigurationManager
    {
        public static Secrets SecretValues { get; private set; }
        public static Settings SettingValues { get; private set; }

        public static void AddUserSecrets()
        {
            var builder = new SecretsBuilder();
            string filePath = SecretsBuilder.GetUserSecretPath();

            using (var reader = new System.IO.FileStream(filePath, FileMode.Open))
            {
                var text = new StreamReader(reader).ReadToEnd();
                var token = JObject.Parse(text);
                builder.ParseJSON(token);
                //SecretValues = JsonConvert.DeserializeObject<Secrets>(text);
            }
            SecretValues = new Secrets(builder.JsonValuePairs);
        }

        public static void AddJsonFile(string fileName)
        {
            var builder = new SecretsBuilder();
 

            using (var reader = new System.IO.FileStream(fileName, FileMode.Open))
            {
                var text = new StreamReader(reader).ReadToEnd();
                var token = JObject.Parse(text);
                builder.ParseJSON(token);
            }
            SettingValues = new Settings(builder.JsonValuePairs);
        }
        private class SecretsBuilder
        {
            internal static string ScrubUserName(string user)
            {
                var start = user.IndexOf("\\");
                return start > 0 ? user.Substring(start + 1) : user;
            }
            internal static string GetUserSecretPath()
            {
                const string FileBase = @"C:\Users\{0}\AppData\Roaming\Microsoft\UserSecrets\{1}\secrets.json";
                const string BeginTag = "<UserSecretsId>";
                const string EndTag = "</UserSecretsId>";
                string secretsFile;
                var pwd = Directory.GetCurrentDirectory();
                var files = Directory.GetFiles(pwd, "*.csproj");
                using (var reader = new System.IO.FileStream(files[0], FileMode.Open))
                {
                    var text = new StreamReader(reader).ReadToEnd();
                    var bIdx = text.IndexOf(BeginTag, StringComparison.OrdinalIgnoreCase);
                    var eIdx = text.IndexOf(EndTag, StringComparison.OrdinalIgnoreCase);
                    var startIdx = bIdx + BeginTag.Length;
                    var folderName = text.Substring(startIdx, eIdx - startIdx);
                    string userName = ScrubUserName(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                    secretsFile = string.Format(FileBase, userName, folderName);
                }

                return secretsFile;
            }

            internal List<KeyValuePair<string, object>> JsonValuePairs { get; set; } = new List<KeyValuePair<string, object>>();

            internal void ParseJSON(JToken jToken)
            {
                switch (jToken.Type)
                {
                    case JTokenType.Array:
                        JArray jArray = jToken as JArray;
                        int aIndex = 0;
                        foreach (JToken jVal in jArray)
                        {
                            //newChild = parent.AddChild("[" + aIndex + "]", null);
                            ParseJSON(jVal);
                            aIndex++;
                        }
                        break;
                    case JTokenType.Object:
                        JObject jObject = (JObject)jToken;
                        foreach (var child in jObject.Children())
                        {

                        }
                        foreach (KeyValuePair<String, JToken> kvPair in jObject)
                        {
                            Console.Write(kvPair.Key + ":");
                            Console.WriteLine(kvPair.Value);
                            ParseJSON(kvPair.Value);
                        }
                        break;
                    case JTokenType.Integer:
                        JsonValuePairs.Add(new KeyValuePair<string, object>(FormatPath(jToken.Path), int.Parse(jToken.ToString())));
                        break;
                    case JTokenType.Boolean:
                        JsonValuePairs.Add(new KeyValuePair<string, object>(FormatPath(jToken.Path), bool.Parse(jToken.ToString())));
                        break;
                    case JTokenType.String:
                        JsonValuePairs.Add(new KeyValuePair<string, object>(FormatPath(jToken.Path), jToken.ToString()));
                        break;
                    case JTokenType.Property:
                        foreach (var token in jToken.Children())
                        {
                            ParseJSON(token);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            private static string FormatPath(string path)
            {
                return path.Replace(".", ":");
            }

        }
    }
}
