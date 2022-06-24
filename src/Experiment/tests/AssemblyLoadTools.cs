﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit.Experimental.Tests
{
    internal class AssemblyLoadTools
    {

        internal static Assembly TryLoadAssembly(string filePath)
        {
            // filePath = "C:\\Users\\t-mwolberg\\Documents\\Convert.dll";
            // Get the array of runtime assemblies.
            string[] runtimeAssemblies = Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll");
            // Create the list of assembly paths consisting of runtime assemblies and the inspected assembly.
            var paths = new List<string>(runtimeAssemblies);
            paths.Add(filePath);
            // Create PathAssemblyResolver that can resolve assemblies using the created list.
            var resolver = new PathAssemblyResolver(paths);
            var mlc = new MetadataLoadContext(resolver);
            // Load assembly into MetadataLoadContext.
            Assembly assembly = mlc.LoadFromAssemblyPath(filePath);
            return assembly;
        }

        internal static void MetadataReader(string filename)
        {
            Debug.WriteLine("Using MetadataReader class");

            using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fs);

            MetadataReader mr = peReader.GetMetadataReader();

            Debug.WriteLine("Number of types is " + mr.TypeDefinitions.Count);
            foreach (TypeDefinitionHandle tdefh in mr.TypeDefinitions)
            {
                TypeDefinition tdef = mr.GetTypeDefinition(tdefh);
                string ns = mr.GetString(tdef.Namespace);
                string name = mr.GetString(tdef.Name);
                Debug.WriteLine($"Name of type is {ns}.{name}");
            }

            Debug.WriteLine("Number of methods is " + mr.MethodDefinitions.Count);
            foreach (MethodDefinitionHandle mdefh in mr.MethodDefinitions)
            {
                MethodDefinition mdef = mr.GetMethodDefinition(mdefh);
                string mname = mr.GetString(mdef.Name);
                var owner = mr.GetTypeDefinition(mdef.GetDeclaringType());
                Debug.WriteLine($"Method name: {mname} is owned by {mr.GetString(owner.Name)}.");
            }

            Debug.WriteLine("Ended MetadataReader class");
        }
    }
}
