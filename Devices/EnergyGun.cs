using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы энергетический орудий.
    /// </summary>
    public struct EnergyGun : IGun
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<EnergyGun, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Power != 0 || Range != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <inheritdoc/>
        [JsonProperty("Мощность")]
        public short Power { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Дальность")]
        public short Range { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static EnergyGun()
        {
            Dictionary<string, Func<EnergyGun, object>> getters = new Dictionary<string, Func<EnergyGun, object>>(4);
            getters.Add(IDevice.GetParameter<EnergyGun>(nameof(Power)), (EnergyGun gun) => gun.Power);
            getters.Add(IDevice.GetParameter<EnergyGun>(nameof(Range)), (EnergyGun gun) => gun.Range);
            getters.Add(IDevice.GetParameter<EnergyGun>(nameof(SizeModifier)), (EnergyGun gun) => gun.SizeModifier);
            getters.Add(IDevice.GetParameter<EnergyGun>(nameof(CostModifier)), (EnergyGun gun) => gun.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<EnergyGun, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}