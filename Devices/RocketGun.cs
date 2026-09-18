using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы ракетных орудий.
    /// </summary>
    public struct RocketGun : IGun
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<RocketGun, object>> Getters;

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
        static RocketGun()
        {
            Dictionary<string, Func<RocketGun, object>> getters = new Dictionary<string, Func<RocketGun, object>>(4);
            getters.Add(IDevice.GetParameter<RocketGun>(nameof(Power)), (RocketGun gun) => gun.Power);
            getters.Add(IDevice.GetParameter<RocketGun>(nameof(Range)), (RocketGun gun) => gun.Range);
            getters.Add(IDevice.GetParameter<RocketGun>(nameof(SizeModifier)), (RocketGun gun) => gun.SizeModifier);
            getters.Add(IDevice.GetParameter<RocketGun>(nameof(CostModifier)), (RocketGun gun) => gun.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<RocketGun, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}