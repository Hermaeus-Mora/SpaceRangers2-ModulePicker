using System.Collections;
using System.Collections.Generic;

namespace ModulePicker.Comparers
{
    /// <summary>
    ///     Компаратор однотипных упакованных объектов.
    /// </summary>
    public sealed class UniformObjectComparer : IComparer<object>
    {
        /// <summary>
        ///     Компаратор.
        /// </summary>
        private readonly IComparer Comparer;

        /// <summary>
        ///     Инициализирует новый экземпляр компаратора.
        /// </summary>
        /// <param name="comparer">
        ///     Общий компаратор.
        /// </param>
        public UniformObjectComparer(IComparer comparer)
        {
            Comparer = comparer;
        }

        /// <summary>
        ///     Сравнивает объекты.
        /// </summary>
        /// <param name="x">
        ///     Объект 1.
        /// </param>
        /// <param name="y">
        ///     Объект 2.
        /// </param>
        /// <returns>
        ///     Результат сравнения.
        /// </returns>
        public int Compare(object? x, object? y)
        {
            return Comparer.Compare(x, y);
        }
    }
}