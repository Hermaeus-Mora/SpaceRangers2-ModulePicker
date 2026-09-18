using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ModulePicker.Converters
{
    /// <summary>
    ///     Конвертер расы.
    /// </summary>
    public sealed class RaceConverter : JsonConverter
    {
        /// <summary>
        ///     Словарь (раса, название).
        /// </summary>
        private static readonly ReadOnlyDictionary<Race, string> RaceNameDictionary;
        /// <summary>
        ///     Словарь (название, раса).
        /// </summary>
        private static readonly ReadOnlyDictionary<string, Race> NameRaceDictionary;

        /// <summary>
        ///     Общий экземпляр.
        /// </summary>
        public static RaceConverter Instance
        {
            get => field ??= new RaceConverter();
        }

        /// <summary>
        ///     Инициализатор.
        /// </summary>
        static RaceConverter()
        {
            Dictionary<Race, string> raceNameDictionary = new Dictionary<Race, string>(6);
            raceNameDictionary.Add(Race.All, "Все");
            raceNameDictionary.Add(Race.Faeyans, "Фэяне");
            raceNameDictionary.Add(Race.Gaalians, "Гаальцы");
            raceNameDictionary.Add(Race.Humans, "Люди");
            raceNameDictionary.Add(Race.Maloqs, "Малоки");
            raceNameDictionary.Add(Race.Pelengs, "Пеленги");
            RaceNameDictionary = raceNameDictionary.AsReadOnly();

            Dictionary<string, Race> nameRaceDictionary = new Dictionary<string, Race>(6);
            foreach (KeyValuePair<Race, string> pair in raceNameDictionary)
                nameRaceDictionary.Add(pair.Value, pair.Key);
            NameRaceDictionary = nameRaceDictionary.AsReadOnly();
        }

        /// <summary>
        ///     Определяет, может ли конвертер работать с типом <c><paramref name="objectType"/></c>.
        /// </summary>
        /// <param name="objectType">
        ///     Тип объекта.
        /// </param>
        /// <returns>
        ///     <c>true</c>, если конвертер обрабатывает тип <c><paramref name="objectType"/></c>, иначе – <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return object.Equals(objectType, typeof(Race));
        }

        /// <summary>
        ///     Считывает значение.
        /// </summary>
        /// <param name="reader">
        ///     Оболочка чтения.
        /// </param>
        /// <param name="objectType">
        ///     Тип читаемого объекта.
        /// </param>
        /// <param name="existingValue">
        ///     Существующее значение.
        /// </param>
        /// <param name="serializer">
        ///     Сериализатор.
        /// </param>
        /// <returns>
        ///     Прочитанное значение.
        /// </returns>
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
                return Race.All;

            string str = reader.Value!.ToString()!;
            return NameRaceDictionary.TryGetValue(str, out Race value) ? value : Race.All;
        }
        /// <summary>
        ///     Записывает значение <c><paramref name="value"/></c>.
        /// </summary>
        /// <param name="writer">
        ///     Оболочка записи.
        /// </param>
        /// <param name="value">
        ///     Значение.
        /// </param>
        /// <param name="serializer">
        ///     Сериализатор.
        /// </param>
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(value is Race race ? Convert(race) : string.Empty);
        }

        /// <summary>
        ///     Конвертирует расу в строку.
        /// </summary>
        /// <param name="race">
        ///     Раса.
        /// </param>
        /// <returns>
        ///     Строка.
        /// </returns>
        public static string Convert(Race race)
        {
            return RaceNameDictionary.TryGetValue(race, out string? str) ? str : string.Empty;
        }
    }
}