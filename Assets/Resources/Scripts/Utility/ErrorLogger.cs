using UnityEngine;
using System.Collections;
using System.IO;

public class ErrorLogger {
	
	static bool enabled=true;
	
	#region Interaction Methods
	public static string CreateErrorLog (string explanation) {
		if(!enabled) return "";
		string filename = CreateDirectoryAndFilename();
		
		StreamWriter writer = File.CreateText("logs/"+filename);
		
		WriteBaseInfo(writer, explanation);
		WriteSystemInfo(writer);
		
		writer.Close();
		return filename;
	}
	public static string CreateErrorLog (string explanation, object[] args) {
		if(!enabled) return "";
		string filename = CreateDirectoryAndFilename();
		
		StreamWriter writer = File.CreateText("logs/"+filename);
		
		WriteBaseInfo(writer, explanation);
		WriteAdditionalInfo(writer, args);
		WriteSystemInfo(writer);
		
		writer.Close();
		return filename;
	}
	public static string CreateErrorLog (string explanation, System.Exception exception) {
		if(!enabled) return "";
		string filename = CreateDirectoryAndFilename();
		
		StreamWriter writer = File.CreateText("logs/"+filename);
		
		WriteBaseInfo(writer, explanation);
		WriteExceptionInfo(writer, exception);
		WriteSystemInfo(writer);
		
		writer.Close();
		return filename;
	}
	public static string CreateErrorLog (string explanation, System.Exception exception, object[] args) {
		if(!enabled) return "";
		string filename = CreateDirectoryAndFilename();
		
		StreamWriter writer = File.CreateText("logs/"+filename);
		
		WriteBaseInfo(writer, explanation);
		WriteExceptionInfo(writer, exception);
		WriteAdditionalInfo(writer, args);
		WriteSystemInfo(writer);
		
		writer.Close();
		
		return filename;
	}
	#endregion
	
	static string CreateDirectoryAndFilename () {
		if(!Directory.Exists("logs")) {
			Directory.CreateDirectory("logs");
		}
		
		int cnt = 1;
		string filename = "error "+System.DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss")+" e"+cnt.ToString();
		while(File.Exists("logs/"+filename)) {
			cnt++;
			filename = filename.Remove(filename.Length-1);
			filename += cnt.ToString();
		}
		filename+=".log";
		
		return filename;
	}
	
	static void WriteBaseInfo (StreamWriter writer, string explanation) {
		writer.WriteLine("----------------------");
		writer.WriteLine("------BASE INFO-------");
		writer.WriteLine("----------------------");
		writer.WriteLine("");
		writer.WriteLine(explanation);
		writer.WriteLine("Game version: "+Version.stage+" "+Version.current);
		// other relevant stuff here
		writer.WriteLine("");
	}
	static void WriteExceptionInfo (StreamWriter writer, System.Exception e) {
		writer.WriteLine("----------------------");
		writer.WriteLine("----EXCEPTION INFO----");
		writer.WriteLine("----------------------");
		writer.WriteLine("");
		writer.WriteLine("Exception: "+e.Message);
		writer.WriteLine("Source: "+e.Source);
		writer.WriteLine("Link: "+e.HelpLink);
		writer.WriteLine("Stack trace:");
		writer.WriteLine(e.StackTrace);
		writer.WriteLine("");
	}
	static void WriteAdditionalInfo (StreamWriter writer, object[] args) {
		writer.WriteLine("----------------------");
		writer.WriteLine("---ADDITIONAL INFO----");
		writer.WriteLine("----------------------");
		writer.WriteLine("");
		for (int i = 0; i < args.Length; i++) {
			writer.WriteLine(args[i].ToString());
		}
		writer.WriteLine("");
	}
	static void WriteSystemInfo (StreamWriter writer) {
		writer.WriteLine("----------------------");
		writer.WriteLine("-----SYSTEM INFO------");
		writer.WriteLine("----------------------");
		writer.WriteLine("");
		
		// use SystemInfo
		writer.WriteLine("Operating System: "+SystemInfo.operatingSystem);
		writer.WriteLine("System Memory: "+SystemInfo.systemMemorySize);
		writer.WriteLine("CPU Type: "+SystemInfo.processorType);
		writer.WriteLine("CPU Count: "+SystemInfo.processorCount);
		
		writer.WriteLine("Device Type: "+SystemInfo.deviceType.ToString());
		writer.WriteLine("Device Model: "+SystemInfo.deviceModel);
		writer.WriteLine("Device Name: "+SystemInfo.deviceName);
		writer.WriteLine("Device ID: "+SystemInfo.deviceUniqueIdentifier);
		
		writer.WriteLine("GPU ID: "+SystemInfo.graphicsDeviceID);
		writer.WriteLine("GPU Name: "+SystemInfo.graphicsDeviceName);
		writer.WriteLine("GPU D3D Version: "+SystemInfo.graphicsDeviceVersion);
		writer.WriteLine("GPU Vendor: "+SystemInfo.graphicsDeviceVendor);
		writer.WriteLine("GPU Vendor ID: "+SystemInfo.graphicsDeviceVendorID);
		writer.WriteLine("GPU Memory: "+SystemInfo.graphicsMemorySize);
		writer.WriteLine("GPU Multithread Support: "+SystemInfo.graphicsMultiThreaded);
		writer.WriteLine("GPU Shader Level Support: "+SystemInfo.graphicsShaderLevel);
		
		writer.WriteLine("Supports Shadows: "+SystemInfo.supportsShadows);
		writer.WriteLine("Supports Render Textures: "+SystemInfo.supportsRenderTextures);
		writer.WriteLine("Supports NPOT Textures: "+SystemInfo.npotSupport);
		writer.WriteLine("Supports Image Effects: "+SystemInfo.supportsImageEffects);
		writer.WriteLine("Supports Compute Shaders: "+SystemInfo.supportsComputeShaders);
		writer.WriteLine("Supports 3D Textures: "+SystemInfo.supports3DTextures);
		
		writer.WriteLine("");
	}
}
