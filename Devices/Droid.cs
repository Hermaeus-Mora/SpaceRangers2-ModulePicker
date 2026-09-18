using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы ремонтного дроида.
    /// </summary>
    public struct Droid : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Droid, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Efficiency != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Эффективность.
        /// </summary>
        [JsonProperty("Эффективность")]
        public short Efficiency { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Droid()
        {
            Dictionary<string, Func<Droid, object>> getters = new Dictionary<string, Func<Droid, object>>(3);
            getters.Add(IDevice.GetParameter<Droid>(nameof(Efficiency)), (Droid droid) => droid.Efficiency);
            getters.Add(IDevice.GetParameter<Droid>(nameof(SizeModifier)), (Droid droid) => droid.SizeModifier);
            getters.Add(IDevice.GetParameter<Droid>(nameof(CostModifier)), (Droid droid) => droid.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Droid, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}