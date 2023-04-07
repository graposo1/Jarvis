using Jarvis;

TheBot bot = new TheBot();

//Waiting for user to press R to capture voice.
while (Console.ReadKey(true).Key == ConsoleKey.R)
{
    bot.ReadVoice();
}

//Api api = new Api();

//while (Console.ReadKey(true).Key == ConsoleKey.R)
//{
//    api.Prompt("Hello");
//}

