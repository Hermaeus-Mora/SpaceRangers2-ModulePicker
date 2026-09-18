using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ModulePicker.Devices
{
    /// <summary>
    ///     Модификаторы устройства.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        ///     Компараторы по типам свойств.
        /// </summary>
        private static readonly ReadOnlyDictionary<Type, IComparer> Comparers;

        /// <summary>
        ///     Флаг наличия модификаторов.
        /// </summary>
        public bool HasModifiers { get; }

        /// <summary>
        ///     Модификатор размера.
        /// </summary>
        [JsonProperty("Модификатор размера")]
        public float SizeModifier { get; set; }

        /// <summary>
        ///     Модификатор стоимости.
        /// </summary>
        [JsonProperty("Модификатор стоимости")]
        public float CostModifier { get; set; }

        /// <summary>
        ///     Инициализаторы.
        /// </summary>
        static IDevice()
        {
            Dictionary<Type, IComparer> comparers = new Dictionary<Type, IComparer>(4);
            comparers.Add(typeof(bool), Comparer<bool>.Default);
            comparers.Add(typeof(byte), Comparer<byte>.Default);
            comparers.Add(typeof(short), Comparer<short>.Default);
            comparers.Add(typeof(float), Comparer<float>.Default);
            Comparers = comparers.AsReadOnly();
        }

        /// <summary>
        ///     Получает значение параметра.
        /// </summary>
        /// <param name="parameter">
        ///     Параметр.
        /// </param>
        /// <returns>
        ///     Значение.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Параметр некорректен.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Аргумент равен <c>null</c>.
        /// </exception>
        public object GetParameter(string parameter);
        /// <summary>
        ///     Получает имя параметра по имени свойства.
        /// </summary>
        /// <param name="property">
        ///     Имя свойства.
        /// </param>
        /// <returns>
        ///     Имя параметра.
        /// </returns>
        public static string GetParameter<T>(string property)
        {
            PropertyInfo? info = typeof(T).GetProperty(property);
            JsonPropertyAttribute? attrubute = info?.GetCustomAttribute<JsonPropertyAttribute>();
            return attrubute?.PropertyName ?? string.Empty;
        }

        /// <summary>
        ///     Получает компаратор для типа свойства.
        /// </summary>
        /// <param name="type">
        ///     Тип свойства.
        /// </param>
        /// <returns>
        ///     Компаратор.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        ///     Тип устройства некорректен.
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        ///     Аргумент равен <c>null</c>.
        /// </exception>
        public static IComparer GetComparer(Type type)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            return Comparers.TryGetValue(type, out IComparer? comparer) ? comparer : throw new ArgumentException(null, nameof(type));
        }
    }
}