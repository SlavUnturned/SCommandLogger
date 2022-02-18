using NUnit.Framework;
using SCommandLogger.API;
using SCommandLogger;
using Rocket.API;
using Rocket.Unturned.Commands;
using System;
using System.IO;

namespace Tests
{
    public static class Tests
    {
        static ICommandLogger logger;
        public static ICommandLogger Logger
        {
            get => logger;
            set
            {
                logger?.Dispose();
                logger = value;
            }
        }
        public static void Main()
        {
            Environment.CurrentDirectory = Directory.GetCurrentDirectory();
            JSONLoggerTest();
            TextLoggerTest();
            Console.ReadKey();
        }
        [SetUp]
        public static void Setup()
        {
        }

        public static void JSONLoggerTest()
        {
            Logger = new JSONCommandLogger(Path.Combine(Environment.CurrentDirectory, "json.txt"));
            TestLogger();
        }
        public static void TextLoggerTest()
        {
            Logger = new TextCommandLogger(Path.Combine(Environment.CurrentDirectory, "text.txt"));
            TestLogger();
        }
        public static void TestLogger()
        {
            for (int i = 0; i < 2; i++)
            {
                var entry = Logger.Produce(new RocketPlayer("0", "Console"), new CommandRocket(), "cmd {name}: "+ i.ToString(), new string[] { "reload" });
                Logger.Insert(entry);
            }
            if (Logger is FileCommandLogger l)
            {
                File.Delete(l.FilePath);
                Logger.Dispose();
                Console.WriteLine(File.ReadAllText(l.FilePath));
            }
        }
    }
}