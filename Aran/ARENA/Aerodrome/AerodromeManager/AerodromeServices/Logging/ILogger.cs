﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AerodromeServices.Logging
{
    public interface ILogger
    {
        string Name { get; }

        void Debug([Localizable(false)] string message);
        void Debug(object value);
        void Debug(string message, byte argument);
        void Debug(string message, bool argument);
        void Debug(string message, decimal argument);
        void Debug(string message, long argument);
        void Debug([Localizable(false)] string message, params object[] args);
        void Debug(string message, ulong argument);
        void Debug(string message, uint argument);
        void Debug(string message, string argument);
        void Debug(string message, sbyte argument);
        void Debug(string message, object argument);
        void Debug(string message, int argument);
        void Debug(string message, float argument);
        void Debug(string message, double argument);
        void Debug(string message, char argument);
        void Debug(IFormatProvider formatProvider, object value);
        void Debug(IFormatProvider formatProvider, string message, byte argument);
        void Debug(string message, object arg1, object arg2);
        void Debug(IFormatProvider formatProvider, string message, decimal argument);
        void Debug(IFormatProvider formatProvider, string message, float argument);
        void Debug(IFormatProvider formatProvider, string message, long argument);
        void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Debug(IFormatProvider formatProvider, string message, string argument);
        void Debug(IFormatProvider formatProvider, string message, ulong argument);
        void Debug(IFormatProvider formatProvider, string message, uint argument);
        void Debug(IFormatProvider formatProvider, string message, sbyte argument);
        void Debug(IFormatProvider formatProvider, string message, object argument);
        void Debug(IFormatProvider formatProvider, string message, int argument);
        void Debug(IFormatProvider formatProvider, string message, double argument);
        void Debug(IFormatProvider formatProvider, string message, char argument);
        void Debug(IFormatProvider formatProvider, string message, bool argument);
        void Debug(Exception exception, [Localizable(false)] string message, params object[] args);
        void Debug(string message, object arg1, object arg2, object arg3);
        void Debug(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Debug<T>(T value);
        void Debug<TArgument>([Localizable(false)] string message, TArgument argument);
        void Debug<T>(IFormatProvider formatProvider, T value);
        void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Debug<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Debug<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Error([Localizable(false)] string message);
        void Error(object value);
        void Error(string message, byte argument);
        void Error(string message, bool argument);
        void Error(string message, char argument);
        void Error(string message, decimal argument);
        void Error(string message, double argument);
        void Error(string message, int argument);
        void Error(string message, long argument);
        void Error(string message, object argument);
        void Error(string message, float argument);
        void Error([Localizable(false)] string message, params object[] args);
        void Error(string message, sbyte argument);
        void Error(string message, uint argument);
        void Error(string message, ulong argument);
        void Error(string message, string argument);
        void Error(IFormatProvider formatProvider, object value);
        void Error(IFormatProvider formatProvider, string message, ulong argument);
        void Error(IFormatProvider formatProvider, string message, float argument);
        void Error(IFormatProvider formatProvider, string message, int argument);
        void Error(IFormatProvider formatProvider, string message, long argument);
        void Error(IFormatProvider formatProvider, string message, object argument);
        void Error(IFormatProvider formatProvider, string message, sbyte argument);
        void Error(IFormatProvider formatProvider, string message, string argument);
        void Error(string message, object arg1, object arg2);
        void Error(IFormatProvider formatProvider, string message, uint argument);
        void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Error(IFormatProvider formatProvider, string message, char argument);
        void Error(IFormatProvider formatProvider, string message, decimal argument);
        void Error(IFormatProvider formatProvider, string message, double argument);
        void Error(IFormatProvider formatProvider, string message, byte argument);
        void Error(IFormatProvider formatProvider, string message, bool argument);
        void Error(Exception exception, [Localizable(false)] string message, params object[] args);
        void ErrorRecursive(Exception exception);
        void Error(string message, object arg1, object arg2, object arg3);
        void Error(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Error<T>(T value);
        void Error<TArgument>([Localizable(false)] string message, TArgument argument);
        void Error<T>(IFormatProvider formatProvider, T value);
        void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Error<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Error<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Fatal([Localizable(false)] string message);
        void Fatal(object value);
        void Fatal(string message, string argument);
        void Fatal(string message, ulong argument);
        void Fatal(string message, uint argument);
        void Fatal(string message, double argument);
        void Fatal(string message, float argument);
        void Fatal(string message, long argument);
        void Fatal(string message, object argument);
        void Fatal(string message, int argument);
        void Fatal([Localizable(false)] string message, params object[] args);
        void Fatal(string message, sbyte argument);
        void Fatal(string message, byte argument);
        void Fatal(string message, char argument);
        void Fatal(string message, decimal argument);
        void Fatal(string message, bool argument);
        void Fatal(IFormatProvider formatProvider, object value);
        void Fatal(IFormatProvider formatProvider, string message, string argument);
        void Fatal(string message, object arg1, object arg2);
        void Fatal(IFormatProvider formatProvider, string message, uint argument);
        void Fatal(IFormatProvider formatProvider, string message, ulong argument);
        void Fatal(IFormatProvider formatProvider, string message, double argument);
        void Fatal(IFormatProvider formatProvider, string message, float argument);
        void Fatal(IFormatProvider formatProvider, string message, long argument);
        void Fatal(IFormatProvider formatProvider, string message, object argument);
        void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Fatal(IFormatProvider formatProvider, string message, sbyte argument);
        void Fatal(IFormatProvider formatProvider, string message, int argument);
        void Fatal(IFormatProvider formatProvider, string message, byte argument);
        void Fatal(IFormatProvider formatProvider, string message, char argument);
        void Fatal(IFormatProvider formatProvider, string message, decimal argument);
        void Fatal(IFormatProvider formatProvider, string message, bool argument);
        void Fatal(Exception exception, [Localizable(false)] string message, params object[] args);
        void Fatal(string message, object arg1, object arg2, object arg3);
        void Fatal(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Fatal<T>(T value);
        void Fatal<TArgument>([Localizable(false)] string message, TArgument argument);
        void Fatal<T>(IFormatProvider formatProvider, T value);
        void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Fatal<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Fatal<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Info([Localizable(false)] string message);
        void InfoWithMemberName([Localizable(false)] string message, [CallerMemberName] string memberName = "");
        void Info(object value);
        void Info(string message, char argument);
        void Info(string message, double argument);
        void Info(string message, float argument);
        void Info(string message, int argument);
        void Info(string message, long argument);
        void Info(string message, object argument);
        void Info(string message, decimal argument);
        void Info(string message, bool argument);
        void Info(string message, sbyte argument);
        void Info(string message, string argument);
        void Info(string message, uint argument);
        void Info(string message, ulong argument);
        void Info([Localizable(false)] string message, params object[] args);
        void Info(string message, byte argument);
        void Info(IFormatProvider formatProvider, object value);
        void Info(IFormatProvider formatProvider, string message, long argument);
        void Info(IFormatProvider formatProvider, string message, object argument);
        void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Info(IFormatProvider formatProvider, string message, sbyte argument);
        void Info(IFormatProvider formatProvider, string message, uint argument);
        void Info(IFormatProvider formatProvider, string message, ulong argument);
        void Info(string message, object arg1, object arg2);
        void Info(IFormatProvider formatProvider, string message, string argument);
        void Info(IFormatProvider formatProvider, string message, decimal argument);
        void Info(IFormatProvider formatProvider, string message, double argument);
        void Info(IFormatProvider formatProvider, string message, float argument);
        void Info(IFormatProvider formatProvider, string message, int argument);
        void Info(IFormatProvider formatProvider, string message, byte argument);
        void Info(IFormatProvider formatProvider, string message, char argument);
        void Info(IFormatProvider formatProvider, string message, bool argument);
        void Info(Exception exception, [Localizable(false)] string message, params object[] args);
        void Info(string message, object arg1, object arg2, object arg3);
        void Info(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Info<T>(T value);
        void Info<TArgument>([Localizable(false)] string message, TArgument argument);
        void Info<T>(IFormatProvider formatProvider, T value);
        void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Info<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Info<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Log(LogLevel level, [Localizable(false)] string message);
        void Log(LogLevel level, object value);
        void Log(LogLevel level, string message, object argument);
        void Log(LogLevel level, string message, char argument);
        void Log(LogLevel level, [Localizable(false)] string message, params object[] args);
        void Log(LogLevel level, string message, sbyte argument);
        void Log(LogLevel level, string message, string argument);
        void Log(LogLevel level, string message, uint argument);
        void Log(LogLevel level, string message, ulong argument);
        void Log(LogLevel level, string message, decimal argument);
        void Log(LogLevel level, string message, double argument);
        void Log(LogLevel level, string message, float argument);
        void Log(LogLevel level, string message, int argument);
        void Log(LogLevel level, string message, long argument);
        void Log(LogLevel level, string message, byte argument);
        void Log(LogLevel level, string message, bool argument);
        void Log(LogLevel level, IFormatProvider formatProvider, object value);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, sbyte argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, string argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, uint argument);
        void Log(LogLevel level, string message, object arg1, object arg2);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, ulong argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, double argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, float argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, int argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, long argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, object argument);
        void Log(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, byte argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, char argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, decimal argument);
        void Log(LogLevel level, IFormatProvider formatProvider, string message, bool argument);
        void Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args);
        void Log(LogLevel level, string message, object arg1, object arg2, object arg3);
        void Log(LogLevel level, Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Log<T>(LogLevel level, T value);
        void Log<TArgument>(LogLevel level, [Localizable(false)] string message, TArgument argument);
        void Log<T>(LogLevel level, IFormatProvider formatProvider, T value);
        void Log<TArgument>(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Log<TArgument1, TArgument2>(LogLevel level, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Log<TArgument1, TArgument2>(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Log<TArgument1, TArgument2, TArgument3>(LogLevel level, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Log<TArgument1, TArgument2, TArgument3>(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Swallow(Action action);
        T Swallow<T>(Func<T> func);
        T Swallow<T>(Func<T> func, T fallback);
        void Trace([Localizable(false)] string message);
        void Trace(object value);
        void Trace(string message, ulong argument);
        void Trace(string message, long argument);
        void Trace(string message, object argument);
        void Trace(string message, decimal argument);
        void Trace(string message, double argument);
        void Trace([Localizable(false)] string message, params object[] args);
        void Trace(string message, sbyte argument);
        void Trace(string message, string argument);
        void Trace(string message, uint argument);
        void Trace(string message, float argument);
        void Trace(string message, int argument);
        void Trace(string message, bool argument);
        void Trace(string message, char argument);
        void Trace(string message, byte argument);
        void Trace(IFormatProvider formatProvider, object value);
        void Trace(string message, object arg1, object arg2);
        void Trace(IFormatProvider formatProvider, string message, ulong argument);
        void Trace(IFormatProvider formatProvider, string message, int argument);
        void Trace(IFormatProvider formatProvider, string message, long argument);
        void Trace(IFormatProvider formatProvider, string message, object argument);
        void Trace(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Trace(IFormatProvider formatProvider, string message, sbyte argument);
        void Trace(IFormatProvider formatProvider, string message, string argument);
        void Trace(IFormatProvider formatProvider, string message, uint argument);
        void Trace(IFormatProvider formatProvider, string message, char argument);
        void Trace(IFormatProvider formatProvider, string message, decimal argument);
        void Trace(IFormatProvider formatProvider, string message, double argument);
        void Trace(IFormatProvider formatProvider, string message, float argument);
        void Trace(IFormatProvider formatProvider, string message, bool argument);
        void Trace(IFormatProvider formatProvider, string message, byte argument);
        void Trace(Exception exception, [Localizable(false)] string message, params object[] args);
        void Trace(string message, object arg1, object arg2, object arg3);
        void Trace(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Trace<T>(T value);
        void Trace<TArgument>([Localizable(false)] string message, TArgument argument);
        void Trace<T>(IFormatProvider formatProvider, T value);
        void Trace<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Trace<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Trace<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Warn([Localizable(false)] string message);
        void Warn(object value);
        void Warn(string message, byte argument);
        void Warn(string message, char argument);
        void Warn(string message, decimal argument);
        void Warn(string message, double argument);
        void Warn(string message, float argument);
        void Warn(string message, long argument);
        void Warn(string message, object argument);
        void Warn(string message, int argument);
        void Warn(string message, bool argument);
        void Warn([Localizable(false)] string message, params object[] args);
        void Warn(string message, sbyte argument);
        void Warn(string message, string argument);
        void Warn(string message, uint argument);
        void Warn(string message, ulong argument);
        void Warn(IFormatProvider formatProvider, object value);
        void Warn(IFormatProvider formatProvider, string message, long argument);
        void Warn(IFormatProvider formatProvider, string message, object argument);
        void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Warn(IFormatProvider formatProvider, string message, sbyte argument);
        void Warn(IFormatProvider formatProvider, string message, string argument);
        void Warn(IFormatProvider formatProvider, string message, uint argument);
        void Warn(string message, object arg1, object arg2);
        void Warn(IFormatProvider formatProvider, string message, ulong argument);
        void Warn(IFormatProvider formatProvider, string message, decimal argument);
        void Warn(IFormatProvider formatProvider, string message, double argument);
        void Warn(IFormatProvider formatProvider, string message, float argument);
        void Warn(IFormatProvider formatProvider, string message, int argument);
        void Warn(IFormatProvider formatProvider, string message, byte argument);
        void Warn(IFormatProvider formatProvider, string message, char argument);
        void Warn(IFormatProvider formatProvider, string message, bool argument);
        void Warn(Exception exception, [Localizable(false)] string message, params object[] args);
        void Warn(string message, object arg1, object arg2, object arg3);
        void Warn(Exception exception, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);
        void Warn<T>(T value);
        void Warn<TArgument>([Localizable(false)] string message, TArgument argument);
        void Warn<T>(IFormatProvider formatProvider, T value);
        void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument);
        void Warn<TArgument1, TArgument2>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2);
        void Warn<TArgument1, TArgument2, TArgument3>([Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);
        void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3);

        #region Extentions
        void Trace(Func<string> func);
        void Debug(Func<string> func);
        #endregion
    }

    public enum LogLevel
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        Trace
    }
}
