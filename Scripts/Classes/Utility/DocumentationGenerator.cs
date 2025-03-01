using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KirbStomp.StateMachine;
using KirbStomp;
using KirbStomp.Scripts.Projectiles;
using KirbStomp.MainLine;
using KirbStomp.Interfaces;

public class DocumentationGenerator
{
    private Dictionary<string, Type> _typeMap;
    private string _outputDirectory;

    public DocumentationGenerator(Dictionary<string, object> instances, string outputDirectory)
    {
        _typeMap = instances.ToDictionary(
            kvp => kvp.Key, 
            kvp => kvp.Value.GetType()
        );
        _outputDirectory = outputDirectory;
        
        // Ensure output directory exists
        if (!Directory.Exists(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }
    public DocumentationGenerator(string outputDirectory)
    {
         string directoryPath = Path.Combine("bin", "Debug", "net8.0");
         Console.WriteLine(directoryPath);
         
        _typeMap = ClassExtractor.ExtractClassesFromDirectory(directoryPath);
        _typeMap.Add("HitboxRepository", typeof(HitboxRepository));

        _outputDirectory = outputDirectory;
        // Ensure output directory exists
        if (!Directory.Exists(_outputDirectory))
        {
            Directory.CreateDirectory(_outputDirectory);
        }
    }
    public void GenerateAllDocumentation()
    {
        // Generate index page
        GenerateIndexPage();
        
        // Generate individual class pages
        foreach (var kvp in _typeMap)
        {
            GenerateClassDocumentation(kvp.Key, kvp.Value);
        }
    }

    private void GenerateIndexPage()
    {
        StringBuilder index = new StringBuilder();
        index.AppendLine("# Class Documentation Index");
        index.AppendLine();
        index.AppendLine("## Available Classes");
        index.AppendLine();
        
        // Sort classes alphabetically
        var sortedClasses = _typeMap.Keys.OrderBy(k => k).ToList();
        
        foreach (var className in sortedClasses)
        {
            index.AppendLine($"- [{className}]({className}.md)");
        }
        
        File.WriteAllText(Path.Combine(_outputDirectory, "index.md"), index.ToString());
    }

    private void GenerateClassDocumentation(string className, Type type)
    {
        StringBuilder doc = new StringBuilder();
        
        // Class Header
        doc.AppendLine($"# {className}");
        doc.AppendLine();
        
        // Quick Navigation
        doc.AppendLine("## Quick Navigation");
        doc.AppendLine("- [Properties](#properties)");
        doc.AppendLine("- [Static Properties](#static-properties)");
        doc.AppendLine("- [Methods](#methods)");
        doc.AppendLine("- [Static Methods](#static-methods)");
        doc.AppendLine("- [Constructors](#constructors)");
        doc.AppendLine("- [Interfaces](#interfaces)");
        doc.AppendLine("- [Inheritance](#inheritance)");
        doc.AppendLine("- [Back to Index](index.md)");
        doc.AppendLine();
        
        // Properties Section
        doc.AppendLine("<a id='properties'></a>");
        doc.AppendLine("## Properties");
        
        // Public Properties
        doc.AppendLine("### Public Properties");
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        if (properties.Length > 0)
        {
            doc.AppendLine("| Name | Type | Description |");
            doc.AppendLine("|------|------|-------------|");
            foreach (var prop in properties)
            {
                string linkedType = GetTypeLink(prop.PropertyType);
                doc.AppendLine($"| {prop.Name} | {linkedType} | |");
            }
        }
        else
        {
            doc.AppendLine("*No public properties*");
        }
        doc.AppendLine();
        
        // Private Properties (if needed)
        doc.AppendLine("### Private Properties");
        var privateProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
        if (privateProperties.Length > 0)
        {
            doc.AppendLine("| Name | Type |");
            doc.AppendLine("|------|------|");
            foreach (var prop in privateProperties)
            {
                string linkedType = GetTypeLink(prop.PropertyType);
                doc.AppendLine($"| {prop.Name} | {linkedType} |");
            }
        }
        else
        {
            doc.AppendLine("*No private properties*");
        }
        doc.AppendLine();
        
        // Static Properties Section
        doc.AppendLine("<a id='static-properties'></a>");
        doc.AppendLine("## Static Properties");
        
        // Public Static Properties
        doc.AppendLine("### Public Static Properties");
        var staticProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
        if (staticProperties.Length > 0)
        {
            doc.AppendLine("| Name | Type | Description |");
            doc.AppendLine("|------|------|-------------|");
            foreach (var prop in staticProperties)
            {
                string linkedType = GetTypeLink(prop.PropertyType);
                doc.AppendLine($"| {prop.Name} | {linkedType} | |");
            }
        }
        else
        {
            doc.AppendLine("*No public static properties*");
        }
        doc.AppendLine();
        
        // Private Static Properties
        doc.AppendLine("### Private Static Properties");
        var privateStaticProperties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static);
        if (privateStaticProperties.Length > 0)
        {
            doc.AppendLine("| Name | Type |");
            doc.AppendLine("|------|------|");
            foreach (var prop in privateStaticProperties)
            {
                string linkedType = GetTypeLink(prop.PropertyType);
                doc.AppendLine($"| {prop.Name} | {linkedType} |");
            }
        }
        else
        {
            doc.AppendLine("*No private static properties*");
        }
        doc.AppendLine();
        
        // Methods Section
        doc.AppendLine("<a id='methods'></a>");
        doc.AppendLine("## Methods");
        
        // Public Methods
        doc.AppendLine("### Public Methods");
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName) // Filter out property accessors
            .ToArray();
            
        if (methods.Length > 0)
        {
            doc.AppendLine("| Name | Return Type | Parameters |");
            doc.AppendLine("|------|-------------|------------|");
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var paramList = new List<string>();
                foreach (var param in parameters)
                {
                    string linkedParamType = GetTypeLink(param.ParameterType);
                    paramList.Add($"{linkedParamType} {param.Name}");
                }
                string paramString = string.Join(", ", paramList);
                string linkedReturnType = GetTypeLink(method.ReturnType);
                doc.AppendLine($"| {method.Name} | {linkedReturnType} | {paramString} |");
            }
        }
        else
        {
            doc.AppendLine("*No public methods*");
        }
        doc.AppendLine();
        
        // Static Methods Section
        doc.AppendLine("<a id='static-methods'></a>");
        doc.AppendLine("## Static Methods");
        
        // Public Static Methods
        doc.AppendLine("### Public Static Methods");
        var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName) // Filter out property accessors
            .ToArray();
            
        if (staticMethods.Length > 0)
        {
            doc.AppendLine("| Name | Return Type | Parameters |");
            doc.AppendLine("|------|-------------|------------|");
            foreach (var method in staticMethods)
            {
                var parameters = method.GetParameters();
                var paramList = new List<string>();
                foreach (var param in parameters)
                {
                    string linkedParamType = GetTypeLink(param.ParameterType);
                    paramList.Add($"{linkedParamType} {param.Name}");
                }
                string paramString = string.Join(", ", paramList);
                string linkedReturnType = GetTypeLink(method.ReturnType);
                doc.AppendLine($"| {method.Name} | {linkedReturnType} | {paramString} |");
            }
        }
        else
        {
            doc.AppendLine("*No public static methods*");
        }
        doc.AppendLine();
        
        // Private Static Methods (optional)
        doc.AppendLine("### Private Static Methods");
        var privateStaticMethods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName) // Filter out property accessors
            .ToArray();
            
        if (privateStaticMethods.Length > 0)
        {
            doc.AppendLine("| Name | Return Type | Parameters |");
            doc.AppendLine("|------|-------------|------------|");
            foreach (var method in privateStaticMethods)
            {
                var parameters = method.GetParameters();
                var paramList = new List<string>();
                foreach (var param in parameters)
                {
                    string linkedParamType = GetTypeLink(param.ParameterType);
                    paramList.Add($"{linkedParamType} {param.Name}");
                }
                string paramString = string.Join(", ", paramList);
                string linkedReturnType = GetTypeLink(method.ReturnType);
                doc.AppendLine($"| {method.Name} | {linkedReturnType} | {paramString} |");
            }
        }
        else
        {
            doc.AppendLine("*No private static methods*");
        }
        doc.AppendLine();
        
        // Constructors Section
        doc.AppendLine("<a id='constructors'></a>");
        doc.AppendLine("## Constructors");
        var constructors = type.GetConstructors();
        if (constructors.Length > 0)
        {
            doc.AppendLine("| Constructor | Parameters |");
            doc.AppendLine("|------------|------------|");
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                var paramList = new List<string>();
                foreach (var param in parameters)
                {
                    string linkedParamType = GetTypeLink(param.ParameterType);
                    paramList.Add($"{linkedParamType} {param.Name}");
                }
                string paramString = string.Join(", ", paramList);
                doc.AppendLine($"| {type.Name} | {paramString} |");
            }
        }
        else
        {
            doc.AppendLine("*No explicit constructors*");
        }
        doc.AppendLine();
        
        // Interfaces Section
        doc.AppendLine("<a id='interfaces'></a>");
        doc.AppendLine("## Implemented Interfaces");
        var interfaces = type.GetInterfaces();
        if (interfaces.Length > 0)
        {
            foreach (var inter in interfaces)
            {
                string linkedInterface = GetTypeLink(inter);
                doc.AppendLine($"- {linkedInterface}");
            }
        }
        else
        {
            doc.AppendLine("*No implemented interfaces*");
        }
        doc.AppendLine();
        
        // Inheritance Section
        doc.AppendLine("<a id='inheritance'></a>");
        doc.AppendLine("## Inheritance");
        var baseType = type.BaseType;
        if (baseType != null && baseType != typeof(object))
        {
            string linkedBaseType = GetTypeLink(baseType);
            doc.AppendLine($"Inherits from: {linkedBaseType}");
            doc.AppendLine();
            
            doc.AppendLine("### Inheritance Chain");
            doc.AppendLine("```");
            doc.AppendLine(GetInheritanceChain(type));
            doc.AppendLine("```");
        }
        else
        {
            doc.AppendLine($"Inherits from: `{typeof(object).Name}`");
        }
        
        // Static Fields (Optional but useful)
        doc.AppendLine("<a id='static-fields'></a>");
        doc.AppendLine("## Static Fields");
        var staticFields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        if (staticFields.Length > 0)
        {
            doc.AppendLine("| Name | Type | Value |");
            doc.AppendLine("|------|------|-------|");
            foreach (var field in staticFields)
            {
                string linkedType = GetTypeLink(field.FieldType);
                // Note: Getting field values would require an instance, which we might not have for static fields
                doc.AppendLine($"| {field.Name} | {linkedType} | |");
            }
        }
        else
        {
            doc.AppendLine("*No public static fields*");
        }
        
        // Write to file
        File.WriteAllText(Path.Combine(_outputDirectory, $"{className}.md"), doc.ToString());
    }

    private string GetTypeLink(Type type)
    {
        // Handle arrays, generics, etc.
        if (type.IsArray)
        {
            Type elementType = type.GetElementType();
            string elementTypeLink = GetTypeLink(elementType);
            return $"{elementTypeLink}[]";
        }
        
        if (type.IsGenericType)
        {
            Type genericTypeDef = type.GetGenericTypeDefinition();
            string genericName = genericTypeDef.Name.Split('`')[0];
            
            var genericArgs = type.GetGenericArguments();
            var linkedArgs = genericArgs.Select(GetTypeLink);
            
            return $"{genericName}<{string.Join(", ", linkedArgs)}>";
        }
        
        // Check if we have this type in our map
        var typeKey = _typeMap.FirstOrDefault(x => x.Value == type).Key;
        if (typeKey != null)
        {
            // We have this type, create a link
            return $"[`{type.Name}`]({typeKey}.md)";
        }
        
        // Regular type formatting
        return $"`{type.Name}`";
    }

    private string GetInheritanceChain(Type type)
    {
        var baseType = type.BaseType;
        if (baseType == null || baseType == typeof(object))
        {
            return $"{type.Name} → {typeof(object).Name}";
        }
        return $"{type.Name} → {GetInheritanceChain(baseType)}";
    }  
}