using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы осколочных орудий.
    /// </summary>
    public struct FragmentationGun : IGun
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<FragmentationGun, object>> Getters;

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
        static FragmentationGun()
        {
            Dictionary<string, Func<FragmentationGun, object>> getters = new Dictionary<string, Func<FragmentationGun, object>>(4);
            getters.Add(IDevice.GetParameter<FragmentationGun>(nameof(Power)), (FragmentationGun gun) => gun.Power);
            getters.Add(IDevice.GetParameter<FragmentationGun>(nameof(Range)), (FragmentationGun gun) => gun.Range);
            getters.Add(IDevice.GetParameter<FragmentationGun>(nameof(SizeModifier)), (FragmentationGun gun) => gun.SizeModifier);
            getters.Add(IDevice.GetParameter<FragmentationGun>(nameof(CostModifier)), (FragmentationGun gun) => gun.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<FragmentationGun, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}