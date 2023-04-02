using Jarvis;
using System.Diagnostics;
using Whisper.net;

TheBot bot = new TheBot();

//Waiting for user to press R to capture voice.
while (Console.ReadKey(true).Key == ConsoleKey.R)
{
    bot.ReadVoice();
}
