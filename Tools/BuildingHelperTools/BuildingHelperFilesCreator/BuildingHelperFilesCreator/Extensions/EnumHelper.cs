﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BuildingHelperFilesCreator.Extensions
{
	public static class BuildHelperExtensions
	{
		public static string DescriptionAttr<T>(this T source)
		{
			FieldInfo fi = source.GetType().GetField(source.ToString());

			DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
				typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0) return attributes[0].Description;
			else return source.ToString();
		}
	}

    public static class EnumReflection
    {
        /// <summary>
        /// Retrieves an enum item from a specified string by matching the string to the DescriptionAttribute
        /// elements assigned to each enum item
        /// </summary>
        /// <typeparam name="TEnum">The enum type that should be returned</typeparam>
        /// <param name="description">The description that should be searched</param>
        /// <param name="ignoreCase">Whether string comparison of descriptions should be case-sensitive or not</param>
        /// <returns>The matched enum item</returns>
        /// <exception cref="ArgumentException">Thrown if no enum item could be found with the corresponding description</exception>
        public static TEnum GetEnumByDescription<TEnum>(string description, bool ignoreCase = false)
            // Add a condition to the generic type
            where TEnum : Enum
        {
            // Loop through all the items in the specified enum
            foreach (var item in typeof(TEnum).GetFields())
            {
                // Check to see if the enum item has a description attribute
                if (Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)) is
                    DescriptionAttribute attribute)
                {
                    // If the enum item has a description attribute, then check if
                    // the description matches the given description parameter
                    if (string.Equals(attribute.Description, description,
                        ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                        return (TEnum)item.GetValue(null);
                }
            }

            // If no enum item was found with the specified description, throw an
            // exception
            throw new ArgumentException($"Enum item with description \"{description}\" could not be found",
                nameof(description));
        }

        /// <summary>
        /// Tries to retrieve an enum item from the specified string by matching the string to the DescriptionAttribute
        /// elements assigned to each enum item
        /// </summary>
        /// <typeparam name="TEnum">The enum type that should be retrieved</typeparam>
        /// <param name="description">The description that should be searched</param>
        /// <param name="ignoreCase">Whether string comparison of descriptions should be case-sensitive or not</param>
        /// <param name="result">The matched enum item if it was found</param>
        /// <returns>Whether or not the enum item was found or not</returns>
        public static bool TryGetEnumByDescription<TEnum>(string description, bool ignoreCase, out TEnum result)
            where TEnum : Enum
        {
            try
            {
                // We try to get the enum using our original method
                result = GetEnumByDescription<TEnum>(description, ignoreCase);
                return true;
            }
            catch (ArgumentException)
            {
                // If we cannot retrieve the enum item, set it to the default and
                // return false
                result = default(TEnum);
                return false;
            }
        }

        /// <summary>
        /// Tries to retrieve an enum item from the specified string by matching the string to the DescriptionAttribute
        /// elements assigned to each enum item
        /// </summary>
        /// <typeparam name="TEnum">The enum type that should be retrieved</typeparam>
        /// <param name="description">The description that should be searched</param>
        /// <param name="result">The matched enum item if it was found</param>
        /// <returns>Whether or not the enum item was found or not</returns>
        public static bool TryGetEnumByDescription<TEnum>(string description, out TEnum result)
            where TEnum : Enum
        {
            return TryGetEnumByDescription(description, false, out result);
        }
    }
}
