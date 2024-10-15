// Generated by `wit-bindgen` 0.32.0. DO NOT EDIT!
// <auto-generated />
#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WasiPollWorld.wit.imports.wasi.clocks.v0_2_0
{
    internal static class MonotonicClockInterop {

        internal static class NowWasmInterop
        {
            [DllImport("wasi:clocks/monotonic-clock@0.2.0", EntryPoint = "now"), WasmImportLinkage]
            internal static extern long wasmImportNow();

        }

        internal  static unsafe ulong Now()
        {
            var result =  NowWasmInterop.wasmImportNow();
            return unchecked((ulong)(result));

            //TODO: free alloc handle (interopString) if exists
        }

        internal static class ResolutionWasmInterop
        {
            [DllImport("wasi:clocks/monotonic-clock@0.2.0", EntryPoint = "resolution"), WasmImportLinkage]
            internal static extern long wasmImportResolution();

        }

        internal  static unsafe ulong Resolution()
        {
            var result =  ResolutionWasmInterop.wasmImportResolution();
            return unchecked((ulong)(result));

            //TODO: free alloc handle (interopString) if exists
        }

        internal static class SubscribeInstantWasmInterop
        {
            [DllImport("wasi:clocks/monotonic-clock@0.2.0", EntryPoint = "subscribe-instant"), WasmImportLinkage]
            internal static extern int wasmImportSubscribeInstant(long p0);

        }

        internal  static unsafe global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable SubscribeInstant(ulong when)
        {
            var result =  SubscribeInstantWasmInterop.wasmImportSubscribeInstant(unchecked((long)(when)));
            var resource = new global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable(new global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable.THandle(result));
            return resource;

            //TODO: free alloc handle (interopString) if exists
        }

        internal static class SubscribeDurationWasmInterop
        {
            [DllImport("wasi:clocks/monotonic-clock@0.2.0", EntryPoint = "subscribe-duration"), WasmImportLinkage]
            internal static extern int wasmImportSubscribeDuration(long p0);

        }

        internal  static unsafe global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable SubscribeDuration(ulong when)
        {
            var result =  SubscribeDurationWasmInterop.wasmImportSubscribeDuration(unchecked((long)(when)));
            var resource = new global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable(new global::WasiPollWorld.wit.imports.wasi.io.v0_2_0.IPoll.Pollable.THandle(result));
            return resource;

            //TODO: free alloc handle (interopString) if exists
        }

    }
}
