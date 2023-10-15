using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VetClinicApp
{
    public class Client : ISerializable
    {
        private List<string> keys = new List<string>()
        {
            "Имя", "Фамилия", "Отчество", "Адрес", "Тел."
        };
        private Dictionary<string, string> Fields;

        public string Name
        {
            get { return Fields[keys[0]]; }
            private set { Fields[keys[0]] = value; }
        }
        public string SurName
        {
            get { return Fields[keys[1]]; }
            private set { Fields[keys[1]] = value; }
        }
        public string FatherName
        {
            get { return Fields[keys[2]]; }
            private set { Fields[keys[2]] = value; }
        }
        public string Address
        {
            get { return Fields[keys[3]]; }
            private set { Fields[keys[3]] = value; }
        }
        public string Phone
        {
            get { return Fields[keys[4]]; }
            private set { Fields[keys[4]] = value; }
        }

        public string Path => StaticData.clientsFolder;

        public Client(List<string> fields)
        {
            Fields = new Dictionary<string, string>();

            for (int i = 0; i < keys.Count; i++)
            {
                Fields.Add(keys[i], fields[i]);
            }
        }

        public void Serialize()
        {
            string fileName = Path + "/" + Name + "_" + SurName + "_" + FatherName;

            File.Create(fileName).Close();

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (KeyValuePair<string, string> pair in Fields)
                    writer.WriteLine(pair.Key + ":" + pair.Value);
            }
        }
    }
}
