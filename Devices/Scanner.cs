using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы сканера.
    /// </summary>
    public struct Scanner : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Scanner, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Power != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Мощность.
        /// </summary>
        [JsonProperty("Мощность")]
        public short Power { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Scanner()
        {
            Dictionary<string, Func<Scanner, object>> getters = new Dictionary<string, Func<Scanner, object>>(3);
            getters.Add(IDevice.GetParameter<Scanner>(nameof(Power)), (Scanner scanner) => scanner.Power);
            getters.Add(IDevice.GetParameter<Scanner>(nameof(SizeModifier)), (Scanner scanner) => scanner.SizeModifier);
            getters.Add(IDevice.GetParameter<Scanner>(nameof(CostModifier)), (Scanner scanner) => scanner.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Scanner, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}