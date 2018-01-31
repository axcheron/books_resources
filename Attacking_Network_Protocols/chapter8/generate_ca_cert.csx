using System.IO;

var ca = CertificateUtils.GenerateCACert("CN=MyTestCA", 
    2048, CertificateHashAlgorithm.Sha1);
// Export to PFX with no password
File.WriteAllBytes("ca.pfx", ca.ExportToPFX());
// Export public certificate to a PEM file.
File.WriteAllText("ca.crt", ca.ExportToPEM());
