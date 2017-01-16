using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreConfigTest.Configuration
{
    public class ConfigValues
    {

    }

    public class Configuration
    {
        public static SecretValues Secrets { get; private set; }
        public static ConfigValues Config { get; private set; }

        public static void AddUserSecrets()
        {
            string filePath = SecretsBuilder.GetUserSecretPath();

            using (var reader = new System.IO.FileStream(filePath, FileMode.Open))
            {
                var text = new StreamReader(reader).ReadToEnd();
                var secrets = JsonConvert.DeserializeObject<SecretValues>(text);
                //Secrets = secrets;
            }
        }
    }

    internal class SecretsBuilder
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
    }
}
