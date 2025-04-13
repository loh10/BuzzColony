using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace MyUtils
{
    public static class Utils
    {
        /// <summary>
        /// Return a Vector2 from a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Return a Vector2 from value</returns>
        public static Vector2 StringToVector2(string value)
        {
            value = value.Replace("(", "").Replace(")", "");
            string[] vector = value.Split(',');
            Vector2 vector2 = new Vector2(float.Parse(vector[0], CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(vector[1], CultureInfo.InvariantCulture.NumberFormat));
            return vector2;
        }

        /// <summary>
        /// Remove Number from a string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>value without number</returns>
        public static string NumberRemover(string value)
        {
            return value.Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "")
                .Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "").Replace("0", "");
        }

        /// <summary>
        /// Remove the first index of an array
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] RemoveFirstIndex<T>( T[] array)
        {
            T[] newArray = new T[array.Length - 1];
            for (int i = 1; i < array.Length; i++)
            {
                newArray[i - 1] = array[i];
            }
            return newArray;
        }
    }
}