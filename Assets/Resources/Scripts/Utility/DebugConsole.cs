using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugConsole {
	
	public class Message {
		public string message;
		public MessageType type;
		
		public enum MessageType {
			Notification,
			Warning,
			Error
		}
		
		public Message (string message, MessageType type) {
			this.message = message;
			this.type = type;
		}
	}
	
	static DebugConsole () {
		messages = new List<Message>();
	}
	
	public static List<Message> messages;
	
	#region Interaction Methods
	public static void Clear () {
		messages.Clear();
	}
	public static void LogNotification (string message) {
		AddMessage(message, Message.MessageType.Notification);
		Debug.Log("Debug console: "+message);
	}
	public static void LogWarning (string message) {
		AddMessage(message, Message.MessageType.Warning);
		Debug.LogWarning("Debug console: "+message);
	}
	public static void LogError (string message) {
		AddMessage(message, Message.MessageType.Error);
		Debug.LogError("Debug console: "+message);
	}
	#endregion
	
	static void AddMessage (string message, Message.MessageType type) {
		
		//todo: create UI objects etc..
		messages.Add(new Message(message, type));
		
	}
	
	//todo: log methods should also create new UI objects
}
