using ModulePicker.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker
{
    /// <summary>
    ///     Микромодуль.
    /// </summary>
    public struct MicroModule
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<Type, Func<MicroModule, IDevice>> Getters;

        /// <summary>
        ///     Название.
        /// </summary>
        [JsonProperty("Название")]
        public string Name { get; set; }

        /// <summary>
        ///     Описание.
        /// </summary>
        [JsonProperty("Описание")]
        public string Description { get; set; }

        /// <summary>
        ///     Расы, к оборудованию которых применим микромодуль.
        /// </summary>
        [JsonProperty("Расы")]
        public Race[] Races { get; set; }

        /// <summary>
        ///     Модификаторы двигателя.
        /// </summary>
        [JsonProperty("Двигатель")]
        public Engine Engine { get; set; }

        /// <summary>
        ///     Модификаторы бака.
        /// </summary>
        [JsonProperty("Бак")]
        public Tank Tank { get; set; }

        /// <summary>
        ///     Модификаторы сканера.
        /// </summary>
        [JsonProperty("Сканер")]
        public Scanner Scanner { get; set; }

        /// <summary>
        ///     Модификаторы радара.
        /// </summary>
        [JsonProperty("Радар")]
        public Radar Radar { get; set; }

        /// <summary>
        ///     Модификаторы дроида.
        /// </summary>
        [JsonProperty("Дроид")]
        public Droid Droid { get; set; }

        /// <summary>
        ///     Модификаторы захвата.
        /// </summary>
        [JsonProperty("Захват")]
        public Gripper Gripper { get; set; }

        /// <summary>
        ///     Модификаторы генератора защитного поля.
        /// </summary>
        [JsonProperty("Генератор поля")]
        public ShieldGenerator ShieldGenerator { get; set; }

        /// <summary>
        ///     Модификаторы энергетического оружия.
        /// </summary>
        [JsonProperty("Энергетическое оружие")]
        public EnergyGun EnergyGun { get; set; }

        /// <summary>
        ///     Модификаторы ракетного оружия.
        /// </summary>
        [JsonProperty("Ракетное оружие")]
        public RocketGun RocketGun { get; set; }

        /// <summary>
        ///     Модификаторы осколочного оружия.
        /// </summary>
        [JsonProperty("Осколочное оружие")]
        public FragmentationGun FragmentationGun { get; set; }

        /// <summary>
        ///     Модификаторы корпуса.
        /// </summary>
        [JsonProperty("Корпус")]
        public Hull Hull { get; set; }

        /// <summary>
        ///     Инициализатор.
        /// </summary>
        static MicroModule()
        {
            Dictionary<Type, Func<MicroModule, IDevice>> getters = new Dictionary<Type, Func<MicroModule, IDevice>>(11);
            getters.Add(typeof(Engine), (MicroModule module) => module.Engine);
            getters.Add(typeof(Tank), (MicroModule module) => module.Tank);
            getters.Add(typeof(Scanner), (MicroModule module) => module.Scanner);
            getters.Add(typeof(Radar), (MicroModule module) => module.Radar);
            getters.Add(typeof(Droid), (MicroModule module) => module.Droid);
            getters.Add(typeof(Gripper), (MicroModule module) => module.Gripper);
            getters.Add(typeof(ShieldGenerator), (MicroModule module) => module.ShieldGenerator);
            getters.Add(typeof(EnergyGun), (MicroModule module) => module.EnergyGun);
            getters.Add(typeof(RocketGun), (MicroModule module) => module.RocketGun);
            getters.Add(typeof(FragmentationGun), (MicroModule module) => module.FragmentationGun);
            getters.Add(typeof(Hull), (MicroModule module) => module.Hull);
            Getters = getters.AsReadOnly();
        }
        /// <summary>
        ///     Инициализирует новый экземпляр микромодуля.
        /// </summary>
        public MicroModule()
        {
            Name = string.Empty;
            Description = string.Empty;
            Races = Array.Empty<Race>();
        }

        /// <summary>
        ///     Получает модификаторы устройства, которые даёт микромодуль.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип устройства.
        /// </typeparam>
        /// <returns>
        ///     Модификаторы устройства.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Тип устройства некорректен.
        /// </exception>
        public IDevice GetDevice<T>() where T : IDevice
        {
            return GetDevice(typeof(T));
        }
        /// <summary>
        ///     Получает модификаторы устройства, которые даёт микромодуль.
        /// </summary>
        /// <typeparam name="T">
        ///     Тип устройства.
        /// </typeparam>
        /// <returns>
        ///     Модификаторы устройства.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Тип устройства некорректен.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Аргумент равен <c>null</c>.
        /// </exception>
        public IDevice GetDevice(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            return Getters.TryGetValue(type, out Func<MicroModule, IDevice>? getter) ? getter(this) : throw new ArgumentException(null, nameof(type));
        }
    }
}