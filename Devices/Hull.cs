using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы корпуса.
    /// </summary>
    public struct Hull : IDevice
    {
        /// <summary>
        ///     Функции получения свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Func<Hull, object>> Getters;

        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Armor != 0 || SizeModifier != 0 || CostModifier != 0 || Slots.HasModifiers;
        }

        /// <summary>
        ///     Броня.
        /// </summary>
        [JsonProperty("Броня")]
        public short Armor { get; set; }

        /// <summary>
        ///     Модификаторы слотов.
        /// </summary>
        [JsonProperty("Слоты")]
        public Slots Slots { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <inheritdoc/>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static Hull()
        {
            Dictionary<string, Func<Hull, object>> getters = new Dictionary<string, Func<Hull, object>>(12);
            getters.Add(IDevice.GetParameter<Hull>(nameof(Armor)), (Hull hull) => hull.Armor);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Artefacts)), (Hull hull) => hull.Slots.Artefacts);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Droid)), (Hull hull) => hull.Slots.Droid);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Engine)), (Hull hull) => hull.Slots.Engine);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Gripper)), (Hull hull) => hull.Slots.Gripper);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Guns)), (Hull hull) => hull.Slots.Guns);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Radar)), (Hull hull) => hull.Slots.Radar);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Scanner)), (Hull hull) => hull.Slots.Scanner);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.ShieldGenerator)), (Hull hull) => hull.Slots.ShieldGenerator);
            getters.Add(IDevice.GetParameter<Slots>(nameof(Slots.Tank)), (Hull hull) => hull.Slots.Tank);
            getters.Add(IDevice.GetParameter<Hull>(nameof(SizeModifier)), (Hull hull) => hull.SizeModifier);
            getters.Add(IDevice.GetParameter<Hull>(nameof(CostModifier)), (Hull hull) => hull.CostModifier);
            Getters = getters.AsReadOnly();
        }

        /// <inheritdoc/>
        public object GetParameter(string parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter, nameof(parameter));
            return Getters.TryGetValue(parameter, out Func<Hull, object>? getter) ? getter(this) : throw new ArgumentException(null, nameof(parameter));
        }
    }
}