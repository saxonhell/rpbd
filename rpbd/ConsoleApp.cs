using System;
using System.Linq;
using rpbd.Data;
using rpbd.Models;

namespace rpbd
{
    internal class ConsoleApp
    {
        static TransportCompanyContext context = new TransportCompanyContext();

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Выборка всех данных из таблицы (отношение 'один')");
                Console.WriteLine("2. Выборка данных из таблицы с фильтрацией (отношение 'один')");
                Console.WriteLine("3. Группировка данных с итогом (отношение 'многие')");
                Console.WriteLine("4. Выборка данных из двух таблиц ('один-ко-многим')");
                Console.WriteLine("5. Выборка данных из двух таблиц с фильтрацией ('один-ко-многим')");
                Console.WriteLine("6. Вставка данных в таблицу (отношение 'один')");
                Console.WriteLine("7. Вставка данных в таблицу (отношение 'многие')");
                Console.WriteLine("8. Удаление данных из таблицы (отношение 'один')");
                Console.WriteLine("9. Удаление данных из таблицы (отношение 'многие')");
                Console.WriteLine("10. Обновление данных с условием");
                Console.WriteLine("0. Выход");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SelectAllFromCarBrand();
                        break;
                    case "2":
                        SelectCarBrandWithFilter();
                        break;
                    case "3":
                        GroupByCarsPerBrand();
                        break;
                    case "4":
                        SelectCarsWithBrand();
                        break;
                    case "5":
                        SelectCarsWithBrandFilter();
                        break;
                    case "6":
                        InsertCarBrand();
                        break;
                    case "7":
                        InsertCar();
                        break;
                    case "8":
                        DeleteCarBrand();
                        break;
                    case "9":
                        DeleteCar();
                        break;
                    case "10":
                        UpdateCar();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        // 1. Выборка всех данных из таблицы CarBrand (отношение "один")
        static void SelectAllFromCarBrand()
        {
            var carBrands = context.CarBrands.ToList();
            foreach (var brand in carBrands)
            {
                Console.WriteLine($"ID: {brand.Id}, Название: {brand.Name}");
            }
        }

        // 2. Выборка данных из таблицы CarBrand с фильтрацией
        static void SelectCarBrandWithFilter()
        {
            Console.WriteLine("Введите название бренда для фильтрации:");
            string name = Console.ReadLine();

            var carBrands = context.CarBrands
                .Where(cb => cb.Name.Contains(name))
                .ToList();

            foreach (var brand in carBrands)
            {
                Console.WriteLine($"ID: {brand.Id}, Название: {brand.Name}");
            }
        }

        // 3. Группировка данных по BrandId с подсчетом количества автомобилей (отношение "многие")
        static void GroupByCarsPerBrand()
        {
            var carGroups = context.Cars
                .GroupBy(c => c.BrandId)
                .Select(g => new { BrandId = g.Key, CarCount = g.Count() })
                .ToList();

            foreach (var group in carGroups)
            {
                var brand = context.CarBrands.Find(group.BrandId);
                string brandName = brand != null ? brand.Name : "Неизвестно";

                Console.WriteLine($"Бренд: {brandName} (ID: {group.BrandId}), Количество автомобилей: {group.CarCount}");
            }
        }

        // 4. Выборка данных из двух таблиц Car и CarBrand (отношение "один-ко-многим")
        static void SelectCarsWithBrand()
        {
            var query = context.Cars
                .Select(c => new { c.RegistrationNumber, BrandName = c.Brand.Name })
                .ToList();

            foreach (var item in query)
            {
                Console.WriteLine($"Автомобиль: {item.RegistrationNumber}, Бренд: {item.BrandName}");
            }
        }

        // 5. Выборка данных из двух таблиц с фильтрацией по названию бренда
        static void SelectCarsWithBrandFilter()
        {
            Console.WriteLine("Введите название бренда для фильтрации автомобилей:");
            string brandName = Console.ReadLine();

            var cars = context.Cars
                .Where(c => c.Brand.Name.Contains(brandName))
                .Select(c => new { c.RegistrationNumber, BrandName = c.Brand.Name })
                .ToList();

            foreach (var car in cars)
            {
                Console.WriteLine($"Автомобиль: {car.RegistrationNumber}, Бренд: {car.BrandName}");
            }
        }

        // 6. Вставка данных в таблицу CarBrand (отношение "один")
        static void InsertCarBrand()
        {
            string name = ReadNonEmptyString("Введите название нового бренда: ");

            string specs = ReadNonEmptyString("Введите технические характеристики: ");

            CarBrand newBrand = new CarBrand { Name = name, TechnicalSpecifications = specs };
            context.CarBrands.Add(newBrand);
            context.SaveChanges();

            Console.WriteLine("Бренд добавлен.");
        }

        // 7. Вставка данных в таблицу Car (отношение "многие")
        static void InsertCar()
        {
            Console.WriteLine("Введите данные для нового автомобиля:");

            int brandId = ReadInt("ID бренда: ");
            int carTypeId = ReadInt("ID типа автомобиля: ");
            string regNumber = ReadNonEmptyString("Номер регистрации: ");
            string bodyNumber = ReadNonEmptyString("Номер кузова: ");
            string engineNumber = ReadNonEmptyString("Номер двигателя: ");
            int yearOfManufacture = ReadInt("Год производства: ");
            int driverId = ReadInt("ID водителя: ");
            DateOnly lastMaintenanceDate = ReadDateOnly("Дата последнего техобслуживания (ГГГГ-ММ-ДД): ");
            int mechanicId = ReadInt("ID механика: ");

            Car newCar = new Car
            {
                BrandId = brandId,
                CarTypeId = carTypeId,
                RegistrationNumber = regNumber,
                BodyNumber = bodyNumber,
                EngineNumber = engineNumber,
                YearOfManufacture = yearOfManufacture,
                DriverId = driverId,
                LastMaintenanceDate = lastMaintenanceDate,
                MechanicId = mechanicId
            };

            context.Cars.Add(newCar);
            context.SaveChanges();

            Console.WriteLine("Автомобиль добавлен.");
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                }
            }
        }

        static DateOnly ReadDateOnly(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (DateOnly.TryParse(input, out DateOnly result))
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Некорректная дата. Пожалуйста, введите дату в формате ГГГГ-ММ-ДД.");
                }
            }
        }

        static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Значение не может быть пустым. Попробуйте снова.");
                }
            }
        }


        // 8. Удаление данных из таблицы CarBrand (отношение "один")
        static void DeleteCarBrand()
        {
            Console.Write("Введите ID бренда для удаления: ");
            int brandId = int.Parse(Console.ReadLine());

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var brand = context.CarBrands.Find(brandId);
                    if (brand != null)
                    {
                        // Находим все автомобили, связанные с данным брендом
                        var carsToDelete = context.Cars.Where(c => c.BrandId == brandId).ToList();
                        if (carsToDelete.Any())
                        {
                            // Для каждого автомобиля устанавливаем CarId в null у связанных поездок
                            var carIds = carsToDelete.Select(c => c.Id).ToList();
                            var tripsToUpdate = context.Trips.Where(t => carIds.Contains(t.CarId.Value)).ToList();

                            foreach (var trip in tripsToUpdate)
                            {
                                trip.CarId = null;
                            }

                            // Сохраняем изменения в поездках
                            context.SaveChanges();
                            Console.WriteLine("CarId у связанных поездок установлен в null.");

                            // Удаляем автомобили
                            context.Cars.RemoveRange(carsToDelete);
                            context.SaveChanges();
                            Console.WriteLine("Связанные автомобили удалены.");
                        }

                        // Удаляем бренд
                        context.CarBrands.Remove(brand);
                        context.SaveChanges();
                        transaction.Commit();
                        Console.WriteLine("Бренд удален.");
                    }
                    else
                    {
                        Console.WriteLine("Бренд не найден.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }



        // 9. Удаление данных из таблицы Car (отношение "многие")
        static void DeleteCar()
        {
            Console.Write("Введите ID автомобиля для удаления: ");
            int carId = int.Parse(Console.ReadLine());

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var car = context.Cars.Find(carId);
                    if (car != null)
                    {
                        // Находим все поездки, связанные с данным автомобилем
                        var tripsToUpdate = context.Trips.Where(t => t.CarId == carId).ToList();

                        if (tripsToUpdate.Any())
                        {
                            // Устанавливаем CarId в null для связанных поездок
                            foreach (var trip in tripsToUpdate)
                            {
                                trip.CarId = null;
                            }
                            context.SaveChanges();
                            Console.WriteLine("CarId у связанных поездок установлен в null.");
                        }

                        // Удаляем автомобиль
                        context.Cars.Remove(car);
                        context.SaveChanges();
                        transaction.Commit();
                        Console.WriteLine("Автомобиль удален.");
                    }
                    else
                    {
                        Console.WriteLine("Автомобиль не найден.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }


        // 10. Обновление данных в таблице Car с условием
        static void UpdateCar()
        {
            Console.Write("Введите ID автомобиля для обновления: ");
            int carId = int.Parse(Console.ReadLine());

            var car = context.Cars.Find(carId);
            if (car != null)
            {
                Console.Write("Введите новое значение для поля 'Номер регистрации': ");
                string newRegNumber = Console.ReadLine();

                car.RegistrationNumber = newRegNumber;
                context.SaveChanges();
                Console.WriteLine("Данные автомобиля обновлены.");
            }
            else
            {
                Console.WriteLine("Автомобиль не найден.");
            }
        }
    }
}
