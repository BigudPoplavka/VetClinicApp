using System;
using System.Collections.Generic;
using System.IO;

namespace VetClinicApp
{
    public class Employee: IEmployee, ISerializable
    {
        private Passport EmployeePassport;
        public string workTime;
        public string Path => StaticData.employeesFolder;
        public Passport Passport => EmployeePassport;

        public Employee(List<string> passportFields, string time)
        {
            EmployeePassport = new Passport(passportFields);
            workTime = time;
        }

        public void Serialize()
        {
            string dirName = EmployeePassport.Name + "_" + EmployeePassport.SurName + "_" + EmployeePassport.FatherName;
            string passportPath = Path + "/" + dirName + "/" + Passport.fileName;
            string workTimesPath = Path + "/" + dirName + "/" + StaticData.workTimesFile;

            Directory.CreateDirectory(Path + "/" + dirName);
            File.Create(passportPath).Close();
            File.Create(workTimesPath).Close();

            using (StreamWriter writer = new StreamWriter(passportPath))
            {
                foreach (KeyValuePair<string, string> pair in EmployeePassport.PassportFields)
                    writer.WriteLine(pair.Key + ":" + pair.Value);
            }

            using (StreamWriter writer = new StreamWriter(workTimesPath))
            {
                writer.WriteLine(workTime);
            }
        }
    }
}
