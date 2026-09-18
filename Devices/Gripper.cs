using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы захвата.
    /// </summary>
    public struct Gripper : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Gripper, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Power != 0 || Range != 0 || SizeModifier != 0 || CostModifier != 0;
        }

        /// <summary>
        ///     Мощность.
        /// </summary>
        [JsonProperty("Мощность")]
        public short Power { get; set; }

        /// <summary>
        ///     Дальность.
        /// </summary>
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
        static Gripper()
        {
            Dictionary<string, Func<Gripper, object>> getters = new Dictionary<string, Func<Gripper, object>>(4);
            getters.Add(IDevice.GetParameter<Gripper>(nameof(Power)), (Gripper gripper) => gripper.Power);
            getters.Add(IDevice.GetParameter<Gripper>(nameof(Range)), (Gripper gripper) => gripper.Range);
            getters.Add(IDevice.GetParameter<Gripper>(nameof(SizeModifier)), (Gripper gripper) => gripper.SizeModifier);
            getters.Add(IDevice.GetParameter<Gripper>(nameof(CostModifier)), (Gripper gripper) => gripper.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Gripper, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}