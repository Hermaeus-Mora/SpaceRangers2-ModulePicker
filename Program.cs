using ModulePicker.Comparers;
using ModulePicker.Converters;
using ModulePicker.Devices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace ModulePicker
{
    /// <summary>
    ///     Программа.
    /// </summary>
    public partial class Program
    {
        /// <summary>
        ///     Настройки сериализатора.
        /// </summary>
        private static readonly JsonSerializerSettings Settings;
        /// <summary>
        ///     Словарь типов устройств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Type> Devices;
        /// <summary>
        ///     Словарь наборов параметров по устройствам.
        /// </summary>
        private static readonly ReadOnlyDictionary<Type, ReadOnlyMemory<string>> Parameters;

        /// <summary>
        ///     Микромодули.
        /// </summary>
        private static ReadOnlyMemory<MicroModule> Modules;

        /// <summary>
        ///     Инициализатор.
        /// </summary>
        static Program()
        {
            Settings = new JsonSerializerSettings();
            Settings.Converters.Add(RaceConverter.Instance);

            Dictionary<string, Type> devices = new Dictionary<string, Type>(11);
            devices.Add("Бак", typeof(Tank));
            devices.Add("Дроид", typeof(Droid));
            devices.Add("Радар", typeof(Radar));
            devices.Add("Захват", typeof(Gripper));
            devices.Add("Корпус", typeof(Hull));
            devices.Add("Сканер", typeof(Scanner));
            devices.Add("Генератор", typeof(ShieldGenerator));
            devices.Add("Двигатель", typeof(Engine));
            devices.Add("Ракетное оружие", typeof(RocketGun));
            devices.Add("Осколочное оружие", typeof(FragmentationGun));
            devices.Add("Энергетическое оружие", typeof(EnergyGun));
            Devices = devices.AsReadOnly();

            Dictionary<Type, ReadOnlyMemory<string>> parameters = new Dictionary<Type, ReadOnlyMemory<string>>(11);
            parameters.Add(typeof(Tank), GetDeviceParameters<Tank>());
            parameters.Add(typeof(Droid), GetDeviceParameters<Droid>());
            parameters.Add(typeof(Radar), GetDeviceParameters<Radar>());
            parameters.Add(typeof(Gripper), GetDeviceParameters<Gripper>());
            parameters.Add(typeof(Hull), GetDeviceParameters<Hull>());
            parameters.Add(typeof(Scanner), GetDeviceParameters<Scanner>());
            parameters.Add(typeof(ShieldGenerator), GetDeviceParameters<ShieldGenerator>());
            parameters.Add(typeof(Engine), GetDeviceParameters<Engine>());
            parameters.Add(typeof(RocketGun), GetDeviceParameters<RocketGun>());
            parameters.Add(typeof(FragmentationGun), GetDeviceParameters<FragmentationGun>());
            parameters.Add(typeof(EnergyGun), GetDeviceParameters<EnergyGun>());
            Parameters = parameters.AsReadOnly();
        }
        /// <summary>
        ///     Точка входа в программа.
        /// </summary>
        /// <param name="_">
        ///     Аргументы.
        /// </param>
        private static void Main(string[] _)
        {
            // Загрузка данных
            Modules = LoadMicromodules();

            // Настройка консоли
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            ConsoleColor color = Console.ForegroundColor;

            // Цикл по устройствам
            Type? device;
            while (true)
            {
                // Ввод команды
                Console.Write("Устройство: ");
                string? input = Console.ReadLine();
                if (input is null)
                    return;

                // Очистка
                if (input.Equals("clear", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Clear();
                    continue;
                }

                // Помощь
                if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine();
                    Console.WriteLine("Команды:");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("   clear");
                    Console.ForegroundColor = color;

                    Console.WriteLine();
                    Console.WriteLine("Устройства:");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    foreach (KeyValuePair<string, Type> pair in Devices)
                        Console.WriteLine($"   {pair.Key}");
                    Console.ForegroundColor = color;
                    Console.WriteLine();
                    continue;
                }

                // Определение устройства
                device = Devices.FirstOrDefault(x => x.Key.Equals(input.Trim(), StringComparison.OrdinalIgnoreCase)).Value;
                if (device is null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Устройство не найдено, используйте \"help\" для вывода списка.");
                    Console.ForegroundColor = color;
                    Console.WriteLine();
                    continue;
                }

                // Цикл по параметрам
                while (true)
                {
                    Console.Write("Параметр: ");
                    input = Console.ReadLine();
                    if (input is null)
                        return;

                    // Очистка
                    if (input.Equals("clear", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                        continue;
                    }

                    // Помощь
                    if (input.Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Команды:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("   clear");
                        Console.WriteLine("   break");
                        Console.ForegroundColor = color;

                        Console.WriteLine();
                        Console.WriteLine("Параметры устройства:");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        ReadOnlyMemory<string> parameters = Parameters[device];
                        foreach (string param in parameters.Span)
                            Console.WriteLine($"   {param}");
                        Console.ForegroundColor = color;
                        Console.WriteLine();
                        continue;
                    }

                    // Выход
                    if (input.Equals("break", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine();
                        break;
                    }

                    // Определение параметра
                    string? parameter = null;
                    foreach (string param in Parameters[device].Span)
                    {
                        if (param.Trim().Equals(input, StringComparison.OrdinalIgnoreCase))
                        {
                            parameter = param;
                            break;
                        }
                    }
                    if (parameter is null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("   Параметр не найден, используйте \"help\" для вывода списка.");
                        Console.ForegroundColor = color;
                        Console.WriteLine();
                        continue;
                    }

                    // Выборка модификаторов
                    List<MicroModule> modules = new List<MicroModule>();
                    foreach (MicroModule module in Modules.Span)
                    {
                        IDevice mods = module.GetDevice(device);
                        if (mods.HasModifiers)
                            modules.Add(module);
                    }

                    // Сортировка
                    UniformObjectComparer comparer = new UniformObjectComparer(IDevice.GetComparer(Modules.Span[0].GetDevice(device).GetParameter(parameter).GetType()));
                    IEnumerable<IGrouping<object, MicroModule>> groups = modules.GroupBy(x => x.GetDevice(device).GetParameter(parameter)).OrderByDescending(x => x.Key, comparer);
                    ReadOnlySpan<MicroModule> sorted = groups.SelectMany(g => g).ToArray();

                    // Вывод
                    Console.WriteLine();
                    foreach (MicroModule module in sorted)
                        DrawMicroModule(module);

                    Console.WriteLine($"Подходящих микромодулей найдено: {sorted.Length}");
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        ///     Считывает внедрённый файл с параметрами микромодулей.
        /// </summary>
        /// <returns>
        ///     Микромодули.
        /// </returns>
        private static ReadOnlyMemory<MicroModule> LoadMicromodules()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            ReadOnlySpan<string> resources = assembly.GetManifestResourceNames();
            foreach (string resource in resources)
            {
                if (!resource.EndsWith("Modules.json"))
                    continue;

                string json;
                using (Stream stream = assembly.GetManifestResourceStream(resource)!)
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        json = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<MicroModule[]>(json, Settings);
            }

            return Array.Empty<MicroModule>();
        }
        /// <summary>
        ///     Получает набор параметров устроства типа <c><typeparamref name="T"/></c>.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип устройства.
        /// </typeparam>
        /// <returns>
        ///     Набор параметров.
        /// </returns>
        private static ReadOnlyMemory<string> GetDeviceParameters<T>() where T : IDevice
        {
            List<string> parameters = new List<string>();
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                JsonPropertyAttribute? attribute = property.GetCustomAttribute<JsonPropertyAttribute>(true);
                if (attribute is not null && attribute.PropertyName is not null)
                    parameters.Add(attribute.PropertyName);
            }
            if (typeof(T) == typeof(Hull))
            {
                parameters.Remove(IDevice.GetParameter<Hull>(nameof(Hull.Slots)));
                properties = typeof(Slots).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    JsonPropertyAttribute? attribute = property.GetCustomAttribute<JsonPropertyAttribute>(true);
                    if (attribute is not null && attribute.PropertyName is not null)
                        parameters.Add(attribute.PropertyName);
                }
            }

            IEnumerable<string> uniques = parameters.Distinct(StringComparer.OrdinalIgnoreCase);
            IEnumerable<IGrouping<int, string>> groups = uniques.GroupBy(x => x.Length).OrderBy(x => x.Key);
            return groups.SelectMany(g => g.OrderBy(s => s, StringComparer.OrdinalIgnoreCase)).ToArray();
        }

        /// <summary>
        ///     Отображает информацию о микромодуле.
        /// </summary>
        /// <param name="module">
        ///     Микромодуль.
        /// </param>
        private static void DrawMicroModule(MicroModule module)
        {
            // Название
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Микромодуль ");
            if (module.Description.StartsWith("Микромодуль первого уровня", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.DarkRed;
            else if (module.Description.StartsWith("Микромодуль второго уровня", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\"{module.Name}\"");

            // Описание
            Console.ForegroundColor = color;
            Console.WriteLine(module.Description);
            Console.Write("Расы: ");
            DrawRaces(module.Races);
            Console.WriteLine();
            foreach (KeyValuePair<string, Type> pair in Devices)
            {
                // Получение устройства
                IDevice device = module.GetDevice(pair.Value);
                if (!device.HasModifiers)
                    continue;

                // Отображение названия и параметров
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(pair.Key);
                ReadOnlySpan<string> parameters = Parameters[pair.Value].Span;
                foreach (string parameter in parameters)
                {
                    // Получение значения
                    object modifier = device.GetParameter(parameter);

                    // Определения наличия модификатора
                    Type type = modifier.GetType();
                    object? @default = GetDefault(type);
                    IComparer comparer = IDevice.GetComparer(type);
                    if (comparer.Compare(modifier, @default) == 0)
                        continue;

                    // Определение пользы модификатора
                    bool? positive = IsPositive(device, parameter, modifier, comparer);

                    // Отображение
                    Console.Write($"   {parameter}: ");
                    DrawModifier(modifier, @default, comparer, positive);
                    Console.WriteLine();
                }
                Console.ForegroundColor = color;
                Console.WriteLine();
            }
        }
        /// <summary>
        ///     Отображает расы.
        /// </summary>
        /// <param name="races">
        ///     Расы.
        /// </param>
        private static void DrawRaces(ReadOnlySpan<Race> races)
        {
            ConsoleColor color = Console.ForegroundColor;
            for (int i = 0; i < races.Length; ++i)
            {
                Race race = races[i];
                switch (race)
                {
                    case Race.All:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;

                    case Race.Faeyans:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;

                    case Race.Gaalians:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case Race.Humans:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;

                    case Race.Maloqs:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;

                    case Race.Pelengs:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                }
                Console.Write(RaceConverter.Convert(race));
                Console.ForegroundColor = color;
                if (i < races.Length - 1)
                    Console.Write(", ");
            }
        }
        /// <summary>
        ///     Отображает значение модификатора.
        /// </summary>
        /// <param name="modifier">
        ///     Значение модификатора.
        /// </param>
        /// <param name="default">
        ///     Значение по умолчанию.
        /// </param>
        /// <param name="comparer">
        ///     Компаратор параметра.
        /// </param>
        /// <param name="positive">
        ///     Флаг позитивности модификатора.
        /// </param>
        private static void DrawModifier(object modifier, object? @default, IComparer comparer, bool? positive)
        {
            Type type = modifier.GetType();
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = positive.HasValue ? (positive.Value ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed) : ConsoleColor.Cyan;
            Type[] interfaces = type.GetInterfaces();

            // Целое число
            if (interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IBinaryInteger<>)) && comparer.Compare(modifier, @default) > 0)
            {
                Console.Write($"+{modifier}");
                goto Complete;
            }

            // Число с плавающей запятой
            if (interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IBinaryFloatingPointIeee754<>)))
            {
                if (comparer.Compare(modifier, 1f) > 0)
                {
                    int value = (int)(Math.Round(100f * ((float)modifier - 1f), 0));
                    Console.Write($"+{value}%");
                }
                else
                {
                    int value = (int)(Math.Round(100f * (1f - (float)modifier), 0));
                    Console.Write($"-{value}%");
                }
                goto Complete;
            }

            // Флаг
            if (modifier is bool flag)
                Console.Write(flag ? '+' : '-');

        // Завершение
        Complete:
            Console.ForegroundColor = color;
        }

        /// <summary>
        ///     Получает значение по умолчанию для типа параметра.
        /// </summary>
        /// <param name="type">
        ///     Тип параметра.
        /// </param>
        /// <returns>
        ///     Значение по умолчанию.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Аргумент равен <c>null</c>.
        /// </exception>
        private static object? GetDefault(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
        /// <summary>
        ///     Определяет, является ли значение модификатора положительным изменением.
        /// </summary>
        /// <param name="device">
        ///     Устройство.
        /// </param>
        /// <param name="parameter">
        ///     Параметр.
        /// </param>
        /// <param name="modifier">
        ///     Значение модификатора.
        /// </param>
        /// <param name="comparer">
        ///     Компаратор модификатора.
        /// </param>
        /// <returns>
        ///     <c>true</c>, если модификатор полезен, иначе – <c>false</c>.
        /// </returns>
        private static bool? IsPositive(IDevice device, string parameter, object modifier, IComparer comparer)
        {
            // Нейтральные параметры
            if (device is Hull && parameter == IDevice.GetParameter<Hull>(nameof(IDevice.SizeModifier)))
                return null;

            // Определение типов
            Type type = modifier.GetType();
            Type[] interfaces = type.GetInterfaces();

            // Целые числа
            if (interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IBinaryInteger<>)))
            {
                int result = comparer.Compare(modifier, GetDefault(type));
                if (result > 0)
                    return true;
                if (result < 0)
                    return false;
                return null;
            }

            // Числа с плаваюищей запятой
            if (interfaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IBinaryFloatingPointIeee754<>)))
            {
                // Модификатор размера
                if (parameter == IDevice.GetParameter<IDevice>(nameof(IDevice.SizeModifier)))
                {
                    int result = comparer.Compare(modifier, 1f);
                    if (result > 0)
                        return false;
                    if (result < 0)
                        return true;
                    return null;
                }

                // Модификатор стоимости
                if (parameter == IDevice.GetParameter<IDevice>(nameof(IDevice.CostModifier)))
                {
                    int result = comparer.Compare(modifier, 1f);
                    if (result > 0)
                        return true;
                    if (result < 0)
                        return false;
                    return null;
                }
            }

            // Флаг
            if (modifier is bool flag)
                return flag ? true : null;

            // Непредвиденные обстоятельства
            return null;
        }
    }
}