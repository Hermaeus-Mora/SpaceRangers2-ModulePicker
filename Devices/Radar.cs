using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы радара.
    /// </summary>
    public struct Radar : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Radar, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Radius != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Дальность.
        /// </summary>
        [JsonProperty("Дальность")]
        public short Radius { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Radar()
        {
            Dictionary<string, Func<Radar, object>> getters = new Dictionary<string, Func<Radar, object>>(3);
            getters.Add(IDevice.GetParameter<Radar>(nameof(Radius)), (Radar radar) => radar.Radius);
            getters.Add(IDevice.GetParameter<Radar>(nameof(SizeModifier)), (Radar radar) => radar.SizeModifier);
            getters.Add(IDevice.GetParameter<Radar>(nameof(CostModifier)), (Radar radar) => radar.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Radar, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}