﻿namespace Goop
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using InvokerDelegatePair = System.Tuple<EnumEx.InvokeFunc, System.Delegate>;

    public static class EnumEx
    {
        /// <summary>
        /// The open version of the Enum.TryParse{T} method.
        /// </summary>
        private static readonly MethodInfo EnumTryParseOpenMethod;

        /// <summary>
        /// The open version of the EnumEx.Invoke{T} method.
        /// </summary>
        private static readonly MethodInfo InvokeOpenMethod;

        private delegate bool TryParseFunc<T>(string value, bool ignoreCase, out T result);
        public delegate bool InvokeFunc(Delegate tryParseFunc, string value, bool ignoreCase, out object result);

        private static readonly ConcurrentDictionary<Type, InvokerDelegatePair> TryParsePairs = new ConcurrentDictionary<Type, InvokerDelegatePair>();

        static EnumEx()
        {
            EnumTryParseOpenMethod = typeof(Enum)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(Enum.TryParse) && m.GetParameters().Length == 3);

            InvokeOpenMethod = typeof(EnumEx).GetMethod(nameof(EnumEx.Invoke), BindingFlags.NonPublic | BindingFlags.Static);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object. A parameter
        /// specifies whether the operation is case-sensitive. The return value indicates
        /// whether the conversion succeeded.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">
        /// When this method returns, contains an object of the given type whose value is represented by value.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        /// <remarks>It is recommended that you use Enum.TryParse{T} instead of this method when possible.</remarks>
        public static bool TryParse(Type enumType, string value, bool ignoreCase, out object result)
        {
            VerifyEnumType(enumType);
            InvokerDelegatePair pair = GetTryParsePair(enumType);
            return pair.Item1(pair.Item2, value, ignoreCase, out result);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object. A parameter
        /// specifies whether the operation is case-sensitive. The return value indicates
        /// whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case.</param>
        /// <param name="result">
        /// When this method returns, contains an object of the given type whose value is represented by value.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false.</returns>
        /// <remarks>
        /// This method exists because Enum.TryParse{T} requires that the type is known to be a ValueType at compile time.
        /// Here, we do the check at runtime.
        /// It is recommended that you use Enum.TryParse{T} instead of this method when possible.
        /// </remarks>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            var type = typeof(TEnum);
            VerifyEnumType(type);
            var tryParse = (TryParseFunc<TEnum>)GetTryParsePair(type).Item2;
            return tryParse(value, ignoreCase, out result);
        }

        /// <summary>
        /// Gets the next value in an enum, after a given value. After reaching the end, returns to the first value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="enumValue">The initial value of the enum.</param>
        /// <param name="skip">The enum values to skip over when cycling.</param>
        /// <returns>The next value of the enum.</returns>
        public static TEnum CycleNextEnumValue<TEnum>(this TEnum enumValue, params TEnum[] skip) where TEnum : struct
        {
            var type = typeof(TEnum);

            VerifyEnumType(type);

            var values = (TEnum[])Enum.GetValues(type);

            int index = Array.IndexOf(values, enumValue);

            do
            {
                index = (index + 1) % values.Length;
            }
            while (Array.IndexOf(skip, values[index]) != -1);

            return values[index];
        }

        public static IEnumerable<T> GetAttributes<T>(this Enum value) where T : Attribute
        {
            return value.GetType().GetField(value.ToString()).GetCustomAttributes<T>(false);
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            return value.GetAttributes<T>().FirstOrDefault();
        }

        /// <summary>
        /// Throws an exception if the type is not an enum.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="callerName">Name of the caller.</param>
        /// <exception cref="System.NotSupportedException">requires an enumerable type</exception>
        private static void VerifyEnumType(Type type, [CallerMemberName] string callerName = "")
        {
            if (type.IsEnum == false)
            {
                throw new NotSupportedException($"{callerName} requires an enumerable type. Type \"{type.Name}\" is not supported.");
            }
        }

        private static InvokerDelegatePair GetTryParsePair(Type enumType)
        {
            return TryParsePairs.GetOrAdd(
                enumType,
                type =>
                {
                    MethodInfo invokeMethod = InvokeOpenMethod.MakeGenericMethod(type);
                    MethodInfo tryParseMethod = EnumTryParseOpenMethod.MakeGenericMethod(type);
                    Type delegateType = typeof(TryParseFunc<>).MakeGenericType(type);
                    return new InvokerDelegatePair(
                        (InvokeFunc)invokeMethod.CreateDelegate(typeof(InvokeFunc)),
                        tryParseMethod.CreateDelegate(delegateType));
                });
        }

        private static bool Invoke<TEnum>(Delegate tryParse, string value, bool ignoreCase, out object result)
        {
            TEnum parseResult;
            var tryParseT = (TryParseFunc<TEnum>)tryParse;
            bool success = tryParseT(value, ignoreCase, out parseResult);
            result = parseResult;
            return success;
        }
    }
}