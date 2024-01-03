﻿using BepInEx.Logging;
using CPPMaterials.Source;
using Photon.Pun;
using System;
using System.Diagnostics;

namespace CPPMaterials.Source.Tools
{
    public static class Logging
    {
        private static ManualLogSource logger;
        public static void Init()
        {
            logger = Logger.CreateLogSource(PluginInfo.Name);
        }

        public static void Exception(Exception e)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            logger.LogWarning($"({methodInfo.ReflectedType.Name}.{methodInfo.Name}()) " + string.Join(" ", e.Message, e.StackTrace));
        }

        public static void Fatal(params object[] content)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            logger.LogFatal($"({methodInfo.ReflectedType.Name}.{methodInfo.Name}()) " + string.Join(" ", content));
        }

        public static void Warning(params object[] content)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            logger.LogWarning($"({methodInfo.ReflectedType.Name}.{methodInfo.Name}()) " + string.Join(" ", content));
        }

        public static void Info(params object[] content)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            logger.LogInfo($"({methodInfo.ReflectedType.Name}.{methodInfo.Name}()) " + string.Join(" ", content));

        }

        public static void Debug(params object[] content)
        {
            var methodInfo = new StackTrace().GetFrame(1).GetMethod();
            logger.LogDebug($"({methodInfo.ReflectedType.Name}.{methodInfo.Name}()) " + string.Join("  ", content));
        }

        public static void Debugger(params object[] content)
        {
            Debug(content);
            //if (MenuController.debugger)
            //{
            //    Plugin.debugText.text = PrependTextToLog(
            //        Plugin.debugText.text,
            //        string.Join(" ", content)
            //    );
            //}
        }

        public static int DebuggerLines = 20;
        public static string PrependTextToLog(string log, string text)
        {
            log = text + "\n" + log;
            string[] lines = log.Split('\n');
            if (lines.Length > DebuggerLines)
            {
                log = string.Join("\n", lines, 0, DebuggerLines);
            }
            return log;
        }
    }
}
