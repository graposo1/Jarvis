using JarvisOnTheWeb.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Api
{
    public sealed class Api
    {
        public Process process { get; set; }
        string GPT4All_Exe_Location = AppDomain.CurrentDomain.BaseDirectory + "gpt4all-lora-quantized-win64.exe";
        string GPT4ALL_Model_Location = AppDomain.CurrentDomain.BaseDirectory + "gpt4all-lora-quantized.bin";
        private static Api? _instance;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _hubContext;

        /// <summary>
        /// To run the python script
        /// </summary>
        private bool IsCustomPython = true;
        private string Python_Model_Location = AppDomain.CurrentDomain.BaseDirectory + "ggml-model-q4_1.bin";

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
            ProcessStartInfo processInfo;

            //Custom python program or not.
            if (!IsCustomPython)
            {
                processInfo = new ProcessStartInfo("powershell.exe")
                {
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    Arguments = @"-Command Start-Process -FilePath " + GPT4All_Exe_Location + " -ArgumentList '-m', '" + GPT4ALL_Model_Location + "' -NoNewWindow"
                };
            }
            else
            {
                processInfo = new ProcessStartInfo("python.exe")
                {
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    Arguments = @"pythonscripts\chat.py " + Python_Model_Location
                };
            }

            process = new Process { StartInfo = processInfo };
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            //Read line by line.
            //process.OutputDataReceived += Process_OutputDataReceived;
            //process.BeginOutputReadLine();


            //Realtime reading from output buffer.Word by word
            Task.Run(async () =>
            {
                while (true)
                {
                    int byteRead;
                    StringBuilder word = new StringBuilder();

                    try
                    {
                        while ((byteRead = process.StandardOutput.BaseStream.ReadByte()) > -1)
                        {
                            //Space and enter.
                            if (byteRead == 32 || byteRead == 13)
                            {
                                if (word.Length > 0)
                                {
                                    var txt = Regex.Replace(word.ToString(), @"\e\[(\d+;)*(\d+)?[ABCDHJKfmsu]", "");

                                    if (byteRead == 13) //Enter
                                    {
                                        await _hubContext.Clients.All.SendAsync("ReceiveMessageGPTContinue", "GPT", txt + " ");
                                        await _hubContext.Clients.All.SendAsync("ReceiveMessageGPTNewLine", "GPT", "");
                                    }
                                    else
                                    {
                                        await _hubContext.Clients.All.SendAsync("ReceiveMessageGPTContinue", "GPT", txt + " ");
                                    }

                                    word.Clear();
                                }
                            }
                            else
                            {
                                word.Append(Char.ConvertFromUtf32(byteRead));
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }

        /// <summary>
        /// Read line by line.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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