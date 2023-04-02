using NAudio.Wave;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Text.RegularExpressions;
using Whisper.net;

namespace Jarvis
{
    internal class TheBot
    {
        WhisperFactory whisperFactory = WhisperFactory.FromPath(@"location\...\ggml-model-whisper-base.bin");
        string GPT4All_Exe_Location = @"C:\...\gpt4all-lora-quantized-win64.exe";
        string GPT4ALL_Model_Location = @"C:\...\gpt4all-lora-quantized.bin";
        WhisperProcessor processor;
        WaveFileWriter writer;
        WaveInEvent waveInEvt;
        Process process;
        int HowManyZeroes = 0;
        int _maxZeroes = 5;
        int _threshold = 10;

        public TheBot()
        {
            waveInEvt = new WaveInEvent();
            waveInEvt.WaveFormat = new WaveFormat(16000, 2);

            var processInfo = new ProcessStartInfo("powershell.exe")
            {
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = @"-Command Start-Process -FilePath " + GPT4All_Exe_Location + " -ArgumentList '-m', '" + GPT4ALL_Model_Location + "' -NoNewWindow"
            };

            process = new Process { StartInfo = processInfo };
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
        }

        /// <summary>
        /// Process text from GPT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var txt = Regex.Replace(e.Data, @"\e\[(\d+;)*(\d+)?[ABCDHJKfmsu]", "");
                var synthesizer = new SpeechSynthesizer();
                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.Speak(txt);

                Console.WriteLine("received output: {0}", txt);
            } else
            {
                Console.WriteLine("No output");
            }
        }

        #region WHISPERER
        /// <summary>
        /// Read user voice.
        /// </summary>
        public void ReadVoice()
        {
            Console.WriteLine("LISTENING...");

            if (File.Exists("filerecorded.wav"))
                File.Delete("filerecorded.wav");

            waveInEvt.DataAvailable += WaveSourceMic_DataAvailable;

            if (processor != null)
                processor.Dispose();

            processor = whisperFactory.CreateBuilder()
            .WithSegmentEventHandler(OnNewSegment)
            .WithLanguage("auto")
            .Build();

            writer = new WaveFileWriter("filerecorded.wav", waveInEvt.WaveFormat);

            waveInEvt.StartRecording();
        }

        /// <summary>
        /// Sends processed user voice to GPT.
        /// </summary>
        /// <param name="e"></param>
        private void OnNewSegment(SegmentData e)
        {
            Console.WriteLine(e.Text);

            //Send data to GPT
            process.StandardInput.WriteLine("Write-Host " + e.Text);
        }
        
        private void StopRecord()
        {
            waveInEvt.DataAvailable -= WaveSourceMic_DataAvailable;

            HowManyZeroes = 0;
            waveInEvt.StopRecording();
            writer.Dispose();
            processor.Process(new MemoryStream(File.ReadAllBytes("filerecorded.wav")));
        }
        
        private void WaveSourceMic_DataAvailable(object? sender, WaveInEventArgs e)
        {
            try
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
            catch
            {
                return;
            }

            int value = Math.Abs(BitConverter.ToInt16(e.Buffer, (e.BytesRecorded - 2)));

            if (value < _threshold)
            {
                HowManyZeroes++;
            }
            else
            {
                HowManyZeroes = 0;
            }

            if (HowManyZeroes == _maxZeroes)
            {
                StopRecord();
            }
        }
        #endregion
    }
}
