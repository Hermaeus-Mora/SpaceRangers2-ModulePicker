using Newtonsoft.Json;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы слотов.
    /// </summary>
    public struct Slots
    {
        /// <inheritdoc/>
        public bool HasModifiers
        {
            get => Artefacts != 0 || Guns != 0 || Tank || Droid || Radar || Engine || Gripper || Scanner || ShieldGenerator;
        }

        /// <summary>
        ///     Артефакты.
        /// </summary>
        [JsonProperty("Артефакты")]
        public byte Artefacts { get; set; }

        /// <summary>
        ///     Орудия.
        /// </summary>
        [JsonProperty("Орудия")]
        public byte Guns { get; set; }

        /// <summary>
        ///     Бак.
        /// </summary>
        [JsonProperty("Бак")]
        public bool Tank { get; set; }

        /// <summary>
        ///     Дроид.
        /// </summary>
        [JsonProperty("Дроид")]
        public bool Droid { get; set; }

        /// <summary>
        ///     Радар.
        /// </summary>
        [JsonProperty("Радар")]
        public bool Radar { get; set; }

        /// <summary>
        ///     Двигатель.
        /// </summary>
        [JsonProperty("Двигатель")]
        public bool Engine { get; set; }

        /// <summary>
        ///     Захват.
        /// </summary>
        [JsonProperty("Захват")]
        public bool Gripper { get; set; }

        /// <summary>
        ///     Сканер.
        /// </summary>
        [JsonProperty("Сканер")]
        public bool Scanner { get; set; }

        /// <summary>
        ///     Генератор поля.
        /// </summary>
        [JsonProperty("Генератор поля")]
        public bool ShieldGenerator { get; set; }
    }
}