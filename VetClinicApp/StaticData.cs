using System;
using System.Collections.Generic;
using System.IO;

namespace VetClinicApp
{
    public class StaticData
    {
        public static string invalidFields = "Проверьте заполнение полей!";
        public static string recordCreationError = "Ошибка создания записи!";
        public static string saved = "Запись сохранена!";
        public static string deleted = "Запись удалена!";
        public static string alreadyExist = "Уже существует!";
        public static string notFillFields = "Обе колонки в строке должны быть заполнены!";
        public static string invalidFormat = "Неверный формат поля!";
        public static string noDoctAdded = "На услугу не назначен ни один сотрудник!";
        public static string noClientSaved = "Сначала сохраните запись клиента!";

        public static string workTimesFile = "WorkTime";
        public static string serviceEmpls = "empls";
        public static string passportfile = "/Passport";
        public static string rolesFile = Directory.GetCurrentDirectory() + "/roles";
        public static string employeesFolder = Directory.GetCurrentDirectory() + "/Employees";
        public static string clientsFolder = Directory.GetCurrentDirectory() + "/Clients";
        public static string callsFile = Directory.GetCurrentDirectory() + "/callLog";
        public static string servicesFolder = Directory.GetCurrentDirectory() + "/Services";
        public static string appointmentFolder = Directory.GetCurrentDirectory() + "/Appointments";
    }
}
