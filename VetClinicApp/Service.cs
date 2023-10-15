using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VetClinicApp
{
    public class Service: ISerializable
    {
        public string Type;
        public List<(string, string)> Options;
        public List<string> Doctors;
        public decimal Price;

        public string Path => StaticData.servicesFolder;

        public Service(string type, List<(string, string)> options, List<string> doctors) 
        {
            Type = type;
            Options = options;
            Doctors = doctors;
        }

        public void Serialize()
        {
            string servPath = Path + "/" + Type;
            string fileName = string.Empty;

            Directory.CreateDirectory(servPath);
            
            foreach((string, string) option in Options)
            {
                fileName = servPath + "/" + option.Item1;
                File.Create(fileName).Close();

                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine(option.Item1 + ":" + option.Item2);
                }
            }

            File.Create(servPath + "/" + StaticData.serviceEmpls).Close();

            foreach (string doctor in Doctors)
            {
                using (StreamWriter writer = new StreamWriter(servPath + "/" + StaticData.serviceEmpls))
                {
                    writer.WriteLine(doctor);
                }
            }
        }
    }
}
