﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable CA1716
namespace Microsoft.Shared.Collections;
#pragma warning restore CA1716

#if !SHARED_PROJECT
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif

internal sealed class EmptyReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>
    where TKey : notnull
{
    public static readonly EmptyReadOnlyDictionary<TKey, TValue> Instance = new();

    public int Count => 0;
    public TValue this[TKey key] => throw new KeyNotFoundException();
    public bool ContainsKey(TKey key) => false;
    public IEnumerable<TKey> Keys => EmptyReadOnlyList<TKey>.Instance;
    public IEnumerable<TValue> Values => EmptyReadOnlyList<TValue>.Instance;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => EmptyReadOnlyList<KeyValuePair<TKey, TValue>>.Instance.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => Array.Empty<TKey>();
    ICollection<TValue> IDictionary<TKey, TValue>.Values => Array.Empty<TValue>();
    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;
    TValue IDictionary<TKey, TValue>.this[TKey key]
    {
        get => throw new KeyNotFoundException();
        set => throw new NotSupportedException();
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
#pragma warning disable CS8601 // The recommended implementation: https://learn.microsoft.com/dotnet/api/system.collections.generic.dictionary-2.trygetvalue
        value = default;
#pragma warning restore

        return false;
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
        // nop
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        // nop
    }

    void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException();
    bool IDictionary<TKey, TValue>.Remove(TKey key) => false;
    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => false;
    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => false;
}
