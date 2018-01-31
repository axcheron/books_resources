#load "parser.csx"

using static System.Console;
using static CANAPE.Cli.ConsoleUtils;

var template = new FixedProxyTemplate();
// Local port of 4444, destination 127.0.0.1:12345
template.LocalPort = 4445;
template.Host = "192.168.56.100";
template.Port = 12346;

var tls = new TlsNetworkLayerFactory();
tls.Config.ServerProtocol = System.Security.Authentication.SslProtocols.Tls12;
CertificateManager.SetRootCert("ca.pfx");
tls.Config.SpecifyServerCert = true;
tls.Config.ServerCertificateSubject = "CN=127.0.0.1";
template.AddLayer(tls);
template.AddLayer<Parser>();

var service = template.Create();
service.Start();

WriteLine("Created {0}", service);
WriteLine("Press Enter to exit...");
ReadLine();
service.Stop();
WriteLine("Writing Outbound Packets to packets.bin");
service.Packets.WriteToFile("packets.bin");
WriteLine("Done");
