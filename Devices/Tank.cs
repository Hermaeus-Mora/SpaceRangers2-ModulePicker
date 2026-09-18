using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы топливного бака.
    /// </summary>
    public struct Tank : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Tank, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Volume != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Объём.
        /// </summary>
        [JsonProperty("Объём")]
        public short Volume { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Tank()
        {
            Dictionary<string, Func<Tank, object>> getters = new Dictionary<string, Func<Tank, object>>(3);
            getters.Add(IDevice.GetParameter<Tank>(nameof(Volume)), (Tank tank) => tank.Volume);
            getters.Add(IDevice.GetParameter<Tank>(nameof(SizeModifier)), (Tank tank) => tank.SizeModifier);
            getters.Add(IDevice.GetParameter<Tank>(nameof(CostModifier)), (Tank tank) => tank.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Tank, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}