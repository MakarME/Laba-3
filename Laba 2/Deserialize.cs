using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Laba_2
{
    class Deserialize : Parse
    {
        public List<Dorm> Search(Stream stream)
        {
            List<Dorm> AllResult;
            using (StreamReader sr = new StreamReader(stream))
            {
                string json = sr.ReadToEnd();
                AllResult = JsonSerializer.Deserialize<List<Dorm>>(json);
            }

            return AllResult;
        }
    }
}
