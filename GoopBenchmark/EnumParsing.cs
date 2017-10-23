namespace GoopBenchmark
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using BenchmarkDotNet.Attributes;
    using InvokerDelegatePair = System.Tuple<EnumEx.InvokerFunc, System.Delegate>;

    public static class EnumEx
    {
        private static readonly MethodInfo EnumTryParseOpenMethod;
        private static readonly Type TryParseFuncOpenType = typeof(TryParseFunc<>);
        private static readonly MethodInfo InvokeOpenMethod;
        private static readonly Type InvokerFuncType = typeof(InvokerFunc);

        private static ConcurrentDictionary<Type, MethodInfo> TryParseMethods = new ConcurrentDictionary<Type, MethodInfo>();
        private static ConcurrentDictionary<Type, InvokerDelegatePair> TryParsePairs = new ConcurrentDictionary<Type, InvokerDelegatePair>();

        public delegate bool TryParseFunc<T>(string value, bool ignoreCase, out T result);
        public delegate bool InvokerFunc(Delegate tryParseFunc, string value, bool ignoreCase, out object result);

        static EnumEx()
        {
            EnumTryParseOpenMethod = typeof(Enum)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(Enum.TryParse) && m.GetParameters().Length == 3);

            InvokeOpenMethod = typeof(EnumEx).GetMethod(nameof(EnumEx.Invoke), BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static bool TryParseM(Type enumType, string value, bool ignoreCase, out object result)
        {
            VerifyEnumType(enumType);

            MethodInfo method = EnumTryParseOpenMethod.MakeGenericMethod(enumType);
            object[] args = { value, ignoreCase, null };
            bool success = (bool)method.Invoke(null, args);
            result = args[2];
            return success;
        }

        public static bool TryParseM<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            bool success = TryParseM(typeof(TEnum), value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        public static bool TryParseMC(Type enumType, string value, bool ignoreCase, out object result)
        {
            MethodInfo method = TryParseMethods.GetOrAdd(
                enumType,
                type =>
                {
                    VerifyEnumType(type);
                    return EnumTryParseOpenMethod.MakeGenericMethod(type);
                });
            object[] args = { value, ignoreCase, null };
            bool success = (bool)method.Invoke(null, args);
            result = args[2];
            return success;
        }

        public static bool TryParseMC<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            bool success = TryParseMC(typeof(TEnum), value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        private static InvokerDelegatePair GetTryParsePair(Type enumType)
        {
            return TryParsePairs.GetOrAdd(
                enumType,
                type =>
                {
                    VerifyEnumType(type);
                    MethodInfo invokerMethod = InvokeOpenMethod.MakeGenericMethod(type);
                    var i = (InvokerFunc)invokerMethod.CreateDelegate(InvokerFuncType);
                    MethodInfo method = EnumTryParseOpenMethod.MakeGenericMethod(type);
                    Type delegateType = TryParseFuncOpenType.MakeGenericType(type);
                    var d = method.CreateDelegate(delegateType);
                    return new InvokerDelegatePair(i, d);
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

        public static bool TryParseDiDC(Type enumType, string value, bool ignoreCase, out object result)
        {
            Delegate d = GetTryParsePair(enumType).Item2;
            object[] args = { value, ignoreCase, null };
            bool success = (bool)d.DynamicInvoke(args);
            result = args[2];
            return success;
        }

        public static bool TryParseDiDC<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            bool success = TryParseDiDC(typeof(TEnum), value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        public static bool TryParseDC(Type enumType, string value, bool ignoreCase, out object result)
        {
            InvokerDelegatePair pair = GetTryParsePair(enumType);
            return pair.Item1(pair.Item2, value, ignoreCase, out result);
        }

        public static bool TryParseDCSlowest<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            bool success = TryParseDC(typeof(TEnum), value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        public static bool TryParseDCSlow<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            InvokerDelegatePair pair = GetTryParsePair(typeof(TEnum));
            bool success = pair.Item1(pair.Item2, value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        public static bool TryParseDCFast<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            var d = (TryParseFunc<TEnum>)GetTryParsePair(typeof(TEnum)).Item2;
            return d(value, ignoreCase, out result);
        }

        public static bool TryParseE(Type enumType, string value, bool ignoreCase, out object result)
        {
            try
            {
                result = Enum.Parse(enumType, value, ignoreCase);
                return true;
            }
            catch
            {
                result = Activator.CreateInstance(enumType);
                return false;
            }
        }

        public static bool TryParseESlow<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            object parsedValue;
            bool success = TryParseE(typeof(TEnum), value, ignoreCase, out parsedValue);
            result = (TEnum)parsedValue;
            return success;
        }

        public static bool TryParseEFast<TEnum>(string value, bool ignoreCase, out TEnum result)
        {
            try
            {
                result = (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
                return true;
            }
            catch
            {
                result = default(TEnum);
                return false;
            }
        }

        public static bool TryParseEFastest<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
        {
            return Enum.TryParse(value, ignoreCase, out result);
        }

        private static void VerifyEnumType(Type type, [CallerMemberName] string callerName = "")
        {
            if (type.IsEnum == false)
            {
                throw new NotSupportedException($"{callerName} requires an enumerable type. Type \"{type.Name}\" is not supported.");
            }
        }
    }

    public class EnumParsing
    {
        [Benchmark(Description = "using method info")]
        public void UsingMethodInfo()
        {
            Run<Asdf>(EnumEx.TryParseM);
        }

        [Benchmark(Description = "using cached method info")]
        public void UsingMethodInfoCached()
        {
            Run<Asdf>(EnumEx.TryParseMC);
        }

        [Benchmark(Description = "using cached delegate (dynamic invoke)")]
        public void UsingDynamicInvokeDelegateCached()
        {
            Run<Asdf>(EnumEx.TryParseDiDC);
        }

        [Benchmark(Description = "using cached delegate (fast path)")]
        public void UsingCastedDelegateCachedFast()
        {
            Run<Asdf>(EnumEx.TryParseDCFast);
        }

        [Benchmark(Description = "using cached delegate (slow path)")]
        public void UsingCastedDelegateCachedSlow()
        {
            Run<Asdf>(EnumEx.TryParseDCSlow);
        }

        [Benchmark(Description = "using cached delegate (slowest path)")]
        public void UsingCastedDelegateCachedSlowest()
        {
            Run<Asdf>(EnumEx.TryParseDCSlowest);
        }

        [Benchmark(Description = "using Enum.Parse (slow path)")]
        public void UsingFrameworkMethodSlow()
        {
            Run<Asdf>(EnumEx.TryParseESlow);
        }

        [Benchmark(Description = "using Enum.Parse (fast path)")]
        public void UsingFrameworkMethodFast()
        {
            Run<Asdf>(EnumEx.TryParseEFast);
        }

        [Benchmark(Description = "using Enum.TryParse (fast path)")]
        public void UsingFrameworkMethodFastest()
        {
            Run<Asdf>(EnumEx.TryParseEFastest);
        }

        [Benchmark(Description = "using Enum.TryParse (directly)")]
        public void UsingFrameworkMethodDirectly()
        {
            Run<Asdf>(Enum.TryParse);
        }

        private static void Run<T>(EnumEx.TryParseFunc<T> func) where T : struct
        {
            Case(func, "a", true);
            Case(func, "s", true);
            Case(func, "d", true);
            Case(func, "f", true);
            Case(func, "A", true);
            Case(func, "A", false);
            Case(func, "x", true);
            Case(func, "x", false);
        }

        private static void Case<T>(EnumEx.TryParseFunc<T> func, string value, bool ignoreCase) where T : struct
        {
            T result;
            bool success = func(value, ignoreCase, out result);
            Program.ConsoleWriteLine($"success={success}, result={result}");
        }

        private enum Asdf { a, s, d, f };
    }
}
