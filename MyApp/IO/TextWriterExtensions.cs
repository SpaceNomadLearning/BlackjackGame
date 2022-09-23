using System;
using System.CodeDom.Compiler;
using System.IO;

namespace MyApp.IO
{
    internal static class TextWriterExtensions
    {
        public static int? GetWindowWidth(this TextWriter writer)
        {
            return writer.IsConsole() ? Console.WindowWidth : null;
        }

        public static bool IsConsole(this TextWriter writer)
        {
            if (writer == Console.Out)
                return !Console.IsOutputRedirected;

            if (writer == Console.Error)
            {
                // Color codes are always output to Console.Out
                return !Console.IsErrorRedirected && !Console.IsOutputRedirected;
            }

            return writer is IndentedTextWriter iw && iw.InnerWriter.IsConsole();
        }

        public static void SetForeground(this TextWriter writer, ConsoleColor color)
        {
            if (writer.IsConsole())
                Console.ForegroundColor = color;
        }

        public static void ResetColor(this TextWriter writer)
        {
            if (writer.IsConsole())
                Console.ResetColor();
        }

        public static void WriteWitColor(this TextWriter writer, string text, ConsoleColor color)
        {
            writer.SetForeground(color);
            writer.Write(text);
            writer.ResetColor();
        }

        public static void WriteText(this TextWriter writer, string text)
        {
            writer.WriteWitColor(text, ConsoleColor.White);
        }

        public static void WriteSuccessText(this TextWriter writer, string text)
        {
            writer.WriteWitColor(text, ConsoleColor.DarkGreen);
        }

        public static void WriteWarningText(this TextWriter writer, string text)
        {
            writer.WriteWitColor(text, ConsoleColor.DarkYellow);
        }

        public static void WriteErrorText(this TextWriter writer, string text)
        {
            writer.WriteWitColor(text, ConsoleColor.DarkRed);
        }
    }
}
