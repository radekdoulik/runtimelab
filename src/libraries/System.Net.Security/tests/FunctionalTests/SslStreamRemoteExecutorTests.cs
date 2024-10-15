// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.DotNet.RemoteExecutor;
using Microsoft.DotNet.XUnitExtensions;
using Xunit;
using Xunit.Abstractions;

namespace System.Net.Security.Tests
{
    using Configuration = System.Net.Test.Common.Configuration;

    public class SslStreamRemoteExecutorTests
    {
        public SslStreamRemoteExecutorTests()
        { }

        [ConditionalTheory(typeof(RemoteExecutor), nameof(RemoteExecutor.IsSupported))]
        [PlatformSpecific(TestPlatforms.Linux)] // SSLKEYLOGFILE is only supported on Linux for SslStream
        [InlineData(true)]
        [InlineData(false)]
        public async Task SslKeyLogFile_IsCreatedAndFilled(bool enabledBySwitch)
        {
            if (PlatformDetection.IsDebugLibrary(typeof(SslStream).Assembly) && !enabledBySwitch)
            {
                // AppCtxSwitch is not checked for SSLKEYLOGFILE in Debug builds, the same code path
                // will be tested by the enabledBySwitch = true case. Skip it here.
                return;
            }

            var psi = new ProcessStartInfo();
            var tempFile = Path.GetTempFileName();
            psi.Environment.Add("SSLKEYLOGFILE", tempFile);

            await RemoteExecutor.Invoke(async (enabledBySwitch) =>
            {
                if (bool.Parse(enabledBySwitch))
                {
                    AppContext.SetSwitch("System.Net.EnableSslKeyLogging", true);
                }

                (Stream clientStream, Stream serverStream) = TestHelper.GetConnectedStreams();
                using (clientStream)
                using (serverStream)
                using (var client = new SslStream(clientStream))
                using (var server = new SslStream(serverStream))
                using (X509Certificate2 certificate = Configuration.Certificates.GetServerCertificate())
                {
                    SslClientAuthenticationOptions clientOptions = new SslClientAuthenticationOptions();
                    clientOptions.RemoteCertificateValidationCallback = delegate { return true; };

                    SslServerAuthenticationOptions serverOptions = new SslServerAuthenticationOptions();
                    serverOptions.ServerCertificate = certificate;

                    await TestConfiguration.WhenAllOrAnyFailedWithTimeout(
                        client.AuthenticateAsClientAsync(clientOptions),
                        server.AuthenticateAsServerAsync(serverOptions));

                    await TestHelper.PingPong(client, server);
                }
            }, enabledBySwitch.ToString(), new RemoteInvokeOptions { StartInfo = psi }).DisposeAsync();

            if (enabledBySwitch)
            {
                Assert.True(File.ReadAllText(tempFile).Length > 0);
            }
            else
            {
                Assert.True(File.ReadAllText(tempFile).Length == 0);
            }
        }
    }
}
