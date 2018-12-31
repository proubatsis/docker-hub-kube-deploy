using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace DeployApi.Tests.Util {
    public class TestData {
        public static JObject LoadJson(string name) {
            string filename = name;
            if (!name.EndsWith(".json")) {
                filename = name + ".json";
            }

            filename = "resources/" + filename;

            var reader = new StreamReader(filename);
            var jsonStr = reader.ReadToEnd();
            return JObject.Parse(jsonStr);
        }
    }
}
