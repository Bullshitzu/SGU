using UnityEngine;
using System.Collections;
using System;

namespace FileParser {
    public class ParserExceptions {

        public class UnknownLineTypeException : Exception {
            public UnknownLineTypeException () : base(Comment) { }
            public UnknownLineTypeException (Exception inner) : base(Comment, inner) { }
            static string Comment = "Syntax error, unknown line type.";
        }
        public class WrongLineFormatException : Exception {
            public WrongLineFormatException (string type, string validFormat) : base(GetComment(type, validFormat)) { }
            public WrongLineFormatException (string type, string validFormat, Exception inner) : base(GetComment(type, validFormat), inner) { }
            static string GetComment (string type, string validFormat) {
                return "Wrong format for " + type.Trim() + ". Valid format is " + validFormat;
            }
        }
        public class WrongFileFormatException : Exception {
            public WrongFileFormatException (string path, string validFormats) : base(GetComment(path, validFormats)) { }
            public WrongFileFormatException (string path, string validFormats, Exception inner) : base(GetComment(path, validFormats), inner) { }
            static string GetComment (string path, string validFormats) {
                return "Wrong file format for " + path + ". Valid formats are: " + validFormats;
            }
        }
        public class InvalidMemberException : Exception {
            public InvalidMemberException (string line, params string[] validMembers)
                : base(AddComment(line, validMembers)) { }
            public InvalidMemberException (string line, Exception inner, params string[] validMembers)
                : base(AddComment(line, validMembers), inner) { }
            static string AddComment (string line, string[] validMembers) {
                string validMembersString = "";
                for (int i = 0; i < validMembers.Length; i++) {
                    validMembersString += validMembers[i];
                    if (i + 1 != validMembers.Length) validMembersString += ", ";
                }
                return "Invalid member \"" + line + "\". Valid members for this type are: " + validMembersString;
            }
        }
        public class ParserGenerationException : Exception {
            public ParserGenerationException (string msg, Exception inner) : base(GetComment(msg), inner) { }
            static string GetComment (string msg) {
                return "Error while generating parsed content (" + msg + "): ";
            }
        }
        public class ParserEnumException : Exception {
            public ParserEnumException (string member, params string[] validMembers) : 
                base(AddComment(member, validMembers)) { }
            public ParserEnumException (string member, Exception inner, params string[] validMembers) : 
                base(AddComment(member, validMembers), inner) { }
            static string AddComment (string line, string[] validMembers) {
                string validMembersString = "";
                for (int i = 0; i < validMembers.Length; i++) {
                    validMembersString += validMembers[i];
                    if (i + 1 != validMembers.Length) validMembersString += ", ";
                }
                return "Invalid enum member \"" + line + "\". Valid members are: " + validMembersString;
            }

        }
    }
}
