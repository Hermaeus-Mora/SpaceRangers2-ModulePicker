using Newtonsoft.Json;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы орудий.
    /// </summary>
    public interface IGun : IDevice
    {
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
    }
}