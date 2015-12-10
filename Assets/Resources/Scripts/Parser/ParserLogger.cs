using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace FileParser {
    public class ParserLogger {

        static public string TempData { get; set; }

        public static string[] CollapseExceptions (Exception e) {

            string[] data = new string[2];

            data[0] = "";
            data[1] = "";

            data = CollapseExceptionsRecursively(data[0], data[1], e);

            return data;
        }
        static string[] CollapseExceptionsRecursively (string message, string stackTrace, Exception e) {
            message += e.Message;
            stackTrace = e.StackTrace + Environment.NewLine + stackTrace;

            string[] data = new string[] { message, stackTrace };
            if (e.InnerException != null) data = CollapseExceptionsRecursively(message, stackTrace, e.InnerException);
            return data;
        }

        public static void LogParserError (Exception e) {
            ErrorLogger.CreateErrorLog("Game data parsing error", e);
            DebugConsole.LogError(CollapseExceptions(e)[0]);
        }
        public static void LogParserError (Exception e, params System.Object[] args) {
            ErrorLogger.CreateErrorLog("Game data parsing error", e, args);
            DebugConsole.LogError(CollapseExceptions(e)[0]);
        }

        public static String[] GetFileData (string path) {

            List<string> logParams = new List<string>();
            FileAttributes attributes = File.GetAttributes(path);

            logParams.Add("Path: " + path);
            logParams.Add(Environment.NewLine);
            logParams.Add("Read Only: " + ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly));
            logParams.Add(Environment.NewLine);
            logParams.Add("File Content:");
            logParams.Add(Environment.NewLine);
            logParams.Add(Environment.NewLine);
            logParams.Add("----BEGIN FILE----");
            logParams.Add(Environment.NewLine);

            try {
                StreamReader reader = new StreamReader(path);
                logParams.Add(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception e) {
                logParams.Add("----ERROR----");
                logParams.Add(e.Message);
            }

            logParams.Add(Environment.NewLine);
            logParams.Add("----END FILE----");

            return logParams.ToArray();
        }

    }
}
