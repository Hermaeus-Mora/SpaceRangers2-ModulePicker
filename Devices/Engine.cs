using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы двигателя.
    /// </summary>
    public struct Engine : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Engine, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Speed != 0 || JumpRange != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Скорость.
        /// </summary>
        [JsonProperty("Скорость")]
        public short Speed { get; set; }

        /// <summary>
        ///     Дальность прыжка.
        /// </summary>
        [JsonProperty("Дальность")]
        public short JumpRange { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Engine()
        {
            Dictionary<string, Func<Engine, object>> getters = new Dictionary<string, Func<Engine, object>>(4);
            getters.Add(IDevice.GetParameter<Engine>(nameof(Speed)), (Engine engine) => engine.Speed);
            getters.Add(IDevice.GetParameter<Engine>(nameof(JumpRange)), (Engine engine) => engine.JumpRange);
            getters.Add(IDevice.GetParameter<Engine>(nameof(SizeModifier)), (Engine engine) => engine.SizeModifier);
            getters.Add(IDevice.GetParameter<Engine>(nameof(CostModifier)), (Engine engine) => engine.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Engine, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}