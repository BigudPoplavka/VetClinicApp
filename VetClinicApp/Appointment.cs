using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VetClinicApp
{
    public class Appointment: ISerializable
    {
        public Client Client { get; set; }
        public Service Service { get; set; }
        public string Option { get; set; }
        public string Time { get; set; }

        public string Path => StaticData.appointmentFolder;

        public Appointment(Client client, Service service, string option, string time)
        {
            Client = client;
            Service = service;
            Option = option;
            Time = time;
        }

        public void Serialize()
        {
            string title = Client.Name + " " + Client.SurName + " " + Client.FatherName + " - " + Option.Replace('|', '-') + " - " + Time;
            string filePath = Path + "/" + title;
            File.Create(filePath).Close();
        }
    }
}
