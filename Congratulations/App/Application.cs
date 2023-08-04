using Congratulations.Data;
using Congratulations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congratulations.App
{
    public class Application
    {
        public Application()
        {
            SelectNearest();
            Menu();
        }

        void Menu()
        {
            while (true)
            {
                Console.WriteLine("\n\nВыберите пункт меню:" +
                                  "\n1. Отобразить список всех дней рождения" +
                                  "\n2. Отобразить ближайшие и сегодняшние дни рождения" +
                                  "\n3. Добавить запись" +
                                  "\n4. Удалить запись" +
                                  "\n5. Редактировать запись" +
                                  "\n0. Закрыть программу");
                
                int userChoice = new int();
                try
                {
                    userChoice = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Некорректное значение");
                    continue;
                }

                switch (userChoice)
                {
                    case 1:
                        SelectAll();
                        break;
                    case 2:
                        SelectNearest();
                        break;
                    case 3:
                        AddValue();
                        break;
                    case 4:
                        RemoveValue();
                        break;
                    case 5:
                        EditValue();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Вы ввели неверный пункт меню.");
                        break;
                }
            }

        }

        void SelectAll()
        {
            using var db = new ApplicationDbContext();

            var user = db.Users.ToList();   
            Console.WriteLine("Список дней рождений:");
            

            foreach (var u in user)
            {
                Console.WriteLine($"{u.Id}.{u}  ");
            }
        }

        void SelectNearest()//2 ближайшие и сегодняшние
        {     
            Console.WriteLine("Сегодняшние и ближайшие дни рождения:");
            
            using ApplicationDbContext db = new ApplicationDbContext();

            var users = db.Users.ToList();
            var result = users.Select(users =>
            {
                users.NearestBirthday =
                    new DateTime(IsDateAfter(users.Date, DateTime.Today) ? DateTime.Today.Year : DateTime.Today.Year + 1,
                        users.Date.Month, users.Date.Day);
                return users;
            })
                .Where(user => user.NearestBirthday <= DateTime.Today.AddDays(5))
                .OrderBy(user => user.NearestBirthday);

            foreach (var user in result)
            {
                Console.WriteLine($"{user}");
            }
        }

        void AddValue()
        {
            Console.WriteLine("Введите имя:");
            var name = Console.ReadLine();
            Console.WriteLine("Введите дату:");

            DateTime isDateValid = new DateTime();
            try
            {
                isDateValid = DateTime.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Вы ввели некорректную дату");
                return;
            }
            using var db = new ApplicationDbContext();

            var newUser = new User { Name = name, Date = isDateValid };
            db.Users.Add(newUser);
            db.SaveChanges();
            Console.WriteLine("Данные успешно сохранены");
        }

        void RemoveValue()
        {
            using var db = new ApplicationDbContext();

            Console.WriteLine("Введите номер");
            int n = new int();
            try
            {
                n = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Вы ввели некорректный номер");
                return;
            }

            var user = db.Users.Find(n);
            if (user == null)
            {
                Console.WriteLine("Такого номера не существует");
                return;
            }
            db.Users.Remove(user);
            db.SaveChanges();
            Console.WriteLine("Данные удалены");
        }

        void EditValue()
        {
            using var db = new ApplicationDbContext();

            Console.WriteLine("Введите номер");
            int n = new int();
            try
            {
                n = int.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine("Вы ввели некорректный номер");
                return;
            }

            var user = db.Users.Find(n);
            if (user == null)
            {
                Console.WriteLine("Такого номера не существует");
                return;
            }

            Console.WriteLine("\nЧто Вы хотите сделать?" +
                              "\n1. Изменить имя" +
                              "\n2. Изменить дату" +
                              "\n3. Изменить имя и дату");
            int userChoice = new int();
            try
            {
                userChoice = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Вы ввели некорректный номер");
                return;
            }

            switch (userChoice)
            {
                case 1:
                    Console.WriteLine("Введите имя");
                    user.Name = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Введите дату");
                    try
                    {
                        user.Date = DateTime.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Вы ввели некорректное значение");
                        return;
                    }
                    break;
                case 3:
                    Console.WriteLine("Введите имя");
                    user.Name = Console.ReadLine();
                    Console.WriteLine("Введите дату");
                    try
                    {
                        user.Date = DateTime.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Вы ввели некорректное значение");
                        return;
                    }
                    break;
            }
            db.SaveChanges();
            Console.WriteLine("Данные успешно изменены");
        }


        bool IsDateAfter(DateTime a, DateTime b)
        {
            if (a.Month > b.Month)
            {
                return true;
            }

            if (a.Month == b.Month)
            {
                return a.Day.CompareTo(b.Day) >= 0;
            }

            return a.Month.CompareTo(b.Month) > 0;
        }
    }
}
