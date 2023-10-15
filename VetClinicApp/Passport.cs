using System;
using System.Collections.Generic;
using System.Text;

namespace VetClinicApp
{
    public class Passport
    {
        public static string fileName = "Passport";
        public List<string> Keys = new List<string>() {
            "Имя", "Фамилия", "Отчество", "Возраст",
            "В профессии с", "Должность", "Эл.Почта", "Тел."
        };
        public Dictionary<string, string> PassportFields { get; private set; }
        public string Name 
        { 
            get { return PassportFields[Keys[0]]; } 
            private set { PassportFields[Keys[0]] = value; } 
        }
        public string SurName 
        {
            get { return PassportFields[Keys[1]]; }
            private set { PassportFields[Keys[1]] = value; }
        }
        public string FatherName 
        { 
           get { return PassportFields[Keys[2]]; } 
           private set { PassportFields[Keys[2]] = value; }  
        }
        public int Age 
        { 
           get { return int.Parse(PassportFields[Keys[3]]); } 
           private set { PassportFields[Keys[3]] = value.ToString(); } 
        }
        public int WorkFromYear 
        {
            get { return int.Parse(PassportFields[Keys[4]]); }
            private set { PassportFields[Keys[4]] = value.ToString(); }
        }
        public string Position
        {
            get { return PassportFields[Keys[5]]; }
            private set { PassportFields[Keys[5]] = value; }
        }
        public string Email 
        { 
           get { return PassportFields[Keys[6]]; } 
           private set { PassportFields[Keys[6]] = value; } 
        }
        public string Phone 
        { 
           get { return PassportFields[Keys[7]]; } 
           private set { PassportFields[Keys[7]] = value; } 
        }

        public Passport(List<string> fields)
        {
            PassportFields = new Dictionary<string, string>();

            for(int i = 0; i < Keys.Count; i++)
            {
                PassportFields.Add(Keys[i], fields[i]);
            }
        }
    }
}
