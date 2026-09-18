using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы генератора защитного поля.
    /// </summary>
    public struct ShieldGenerator : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<ShieldGenerator, object>> Getters;

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
        static ShieldGenerator()
        {
            Dictionary<string, Func<ShieldGenerator, object>> getters = new Dictionary<string, Func<ShieldGenerator, object>>(3);
            getters.Add(IDevice.GetParameter<ShieldGenerator>(nameof(Power)), (ShieldGenerator generator) => generator.Power);
            getters.Add(IDevice.GetParameter<ShieldGenerator>(nameof(SizeModifier)), (ShieldGenerator generator) => generator.SizeModifier);
            getters.Add(IDevice.GetParameter<ShieldGenerator>(nameof(CostModifier)), (ShieldGenerator generator) => generator.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<ShieldGenerator, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}