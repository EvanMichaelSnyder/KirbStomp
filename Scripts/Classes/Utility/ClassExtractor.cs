using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


public class ClassExtractor
{
    public static Dictionary<string, Type> ExtractClassesFromDirectory(string directoryPath)
    {
        var classDictionary = new Dictionary<string, Type>();
        
        // Look for compiled DLL files instead of .cs files
        string[] dllFiles = Directory.GetFiles(directoryPath, "*.dll", SearchOption.TopDirectoryOnly);

        foreach (string dllPath in dllFiles)
        {
            // Load the assembly from the DLL
            Assembly assembly = Assembly.LoadFrom(dllPath);
            
            // Get all types in the assembly
            foreach (Type type in assembly.GetTypes())
            {
                // Check if it's a class and not abstract
                if (type.IsClass)
                {
                    var n = type.Namespace;
                    if(n != null && n.Split(".")[0] == "KirbStomp"){
                        // Console.WriteLine("Namespace: " + n);
                        classDictionary[type.Name] = type;
                    }
                }
            }
        }
        return classDictionary;
    }
}