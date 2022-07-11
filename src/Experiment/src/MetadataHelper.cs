﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace System.Reflection.Emit.Experimental
{
    //This static helper class adds common entities to a Metadata Builder.
    internal static class MetadataHelper
    {
        internal static AssemblyReferenceHandle AddAssemblyReference(Assembly assembly, MetadataBuilder metadata)
        {
            AssemblyName assemblyName = assembly.GetName();
            if (assemblyName == null || assemblyName.Name == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }
            return AddAssemblyReference(metadata, assemblyName.Name, assemblyName.Version, assemblyName.CultureName, assemblyName.GetPublicKey(), (AssemblyFlags)assemblyName.Flags);
        }

        internal static AssemblyReferenceHandle AddAssemblyReference(MetadataBuilder metadata, string name, Version? version, string? culture, byte[]? publicKey, AssemblyFlags flags)
        {
            return metadata.AddAssemblyReference(
            name: metadata.GetOrAddString(name),
            version: version ?? new Version(0, 0, 0, 0),
            culture: (culture == null) ? default : metadata.GetOrAddString(value: culture),
            publicKeyOrToken: (publicKey == null) ? default : metadata.GetOrAddBlob(publicKey),
            flags: flags,
            hashValue: default); // not sure where to find hashValue.
        }

        internal static TypeDefinitionHandle addTypeDef(TypeBuilder typeBuilder, MetadataBuilder metadata, int methodToken)
        {
            //Add type metadata
            return metadata.AddTypeDefinition(
                attributes: typeBuilder.UserTypeAttribute,
                (typeBuilder.Namespace == null) ? default : metadata.GetOrAddString(typeBuilder.Namespace),
                name: metadata.GetOrAddString(typeBuilder.Name),
                baseType: default,//Inheritance to be added
                fieldList: MetadataTokens.FieldDefinitionHandle(1),//Update once we support fields.
                methodList: MetadataTokens.MethodDefinitionHandle(methodToken));
        }

        internal static TypeReferenceHandle AddTypeReference(MetadataBuilder metadata, Type type, AssemblyReferenceHandle parent)
        {
            return AddTypeReference(metadata, parent, type.Name, type.Namespace);
        }

        internal static TypeReferenceHandle AddTypeReference(MetadataBuilder metadata, AssemblyReferenceHandle parent, string name, string? nameSpace)
        {
            return metadata.AddTypeReference(
                parent,
                (nameSpace == null) ? default : metadata.GetOrAddString(nameSpace),
                metadata.GetOrAddString(name)
                );
        }

        internal static MemberReferenceHandle AddConstructorReference(MetadataBuilder metadata, TypeReferenceHandle parent, MethodBase method)
        {
            var blob = SignatureHelper.MethodSignatureEnconder(method.GetParameters(), null, true);
            return metadata.AddMemberReference(
                parent,
                metadata.GetOrAddString(method.Name),
                metadata.GetOrAddBlob(blob)
                );
        }

        internal static MethodDefinitionHandle AddMethodDefintion(MetadataBuilder metadata, MethodBuilder methodBuilder)
        {
            return metadata.AddMethodDefinition(
                methodBuilder.Attributes,
                MethodImplAttributes.IL,
                metadata.GetOrAddString(methodBuilder.Name),
                metadata.GetOrAddBlob(SignatureHelper.MethodSignatureEnconder(methodBuilder._parameters, methodBuilder._returnType, !methodBuilder.IsStatic)),
                -1, //No body supported
                parameterList: default
                );
        }
    }
}
