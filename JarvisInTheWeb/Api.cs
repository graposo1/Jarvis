using JarvisInTheWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Api
{
    public sealed class Api
    {
        public Process process { get; set; }
        string GPT4All_Exe_Location = AppDomain.CurrentDomain.BaseDirectory + "gpt4all-lora-quantized-win64.exe";
        string GPT4ALL_Model_Location = AppDomain.CurrentDomain.BaseDirectory + "gpt4all-lora-quantized.bin";
        private static Api? _instance;

        private readonly Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _hubContext;

        public static Api Instance(IHubContext<ChatHub> ctx)
        {
            if (_instance == null)
            {
                _instance = new Api(ctx);
            }

            return _instance;
        }

        public Api(IHubContext<ChatHub> ctx)
        {
            _hubContext = ctx;

            var processInfo = new ProcessStartInfo("powershell.exe")
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = @"-Command Start-Process -FilePath " + GPT4All_Exe_Location + " -ArgumentList '-m', '" + GPT4ALL_Model_Location + "' -NoNewWindow"
            };

            process = new Process { StartInfo = processInfo };
            process.OutputDataReceived += Process_OutputDataReceived;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            process.BeginOutputReadLine();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var txt = Regex.Replace(e.Data, @"\e\[(\d+;)*(\d+)?[ABCDHJKfmsu]", "");

                try
                {
                    _hubContext.Clients.All.SendAsync("ReceiveMessageGPT", "GPT", txt);
                }
                catch
                {

                }
            }
            else
            {
            }
        }

        public void Prompt(string text)
        {
            process.StandardInput.WriteLine(text);
        }
    }
}