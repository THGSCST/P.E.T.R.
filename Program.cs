using PETR_Robot;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;

// PETR-Robot Main
Stopwatch runTime = Stopwatch.StartNew();
Console.WriteLine("PETR-Robot started! Time now is: BR {0} US {1}", MarketHours.GetBraTime(), MarketHours.GetUsaTime());

string telegramBotToken = Utils.GetEnvironmentVariable<string>("TELEGRAM_BOT_TOKEN");
long groupChatID = Utils.GetEnvironmentVariable<long>("GROUP_CHAT_ID");
int fixedMessageID = Utils.GetEnvironmentVariable<int>("FIXED_MESSAGE_ID");
bool showLogMessage = Utils.GetEnvironmentVariable<bool>("SHOW_LOG_MESSAGE");

var bot = new TelegramBotClient(telegramBotToken);
var chatID = new ChatId(groupChatID);

if (true)
{
    var b3QuotePETR4 = await ApiCalls.GetB3Quote("PETR4");
    var b3QuotePETR3 = await ApiCalls.GetB3Quote("PETR3");

    if (b3QuotePETR4.Item1 > 0)
    {
        // Choose the circle color based on the stock percentage
        string circlePETR4 = b3QuotePETR4.Item2 < 0 ? Emoji.redCircle : Emoji.greenCircle;
        string circlePETR3 = b3QuotePETR3.Item2 < 0 ? Emoji.redCircle : Emoji.greenCircle;

        string message = String.Format("{0}PETR4: {1} {2}% {3}PETR3: {4} {5}%\n{5} com delay 15 min",
            circlePETR4, b3QuotePETR4.Item1, b3QuotePETR4.Item2, circlePETR3, b3QuotePETR3.Item1, b3QuotePETR3.Item2, DateTime.Now);

        var B3PetrQuoteMessage = await bot.EditMessageTextAsync(chatID, fixedMessageID, message);
    }
}

runTime.Stop();
string finishMessage = "PETR-Robot finish! Total run time is: " + runTime.Elapsed.ToString();
Console.WriteLine(finishMessage);
if (showLogMessage)
    await bot.SendTextMessageAsync(chatID, finishMessage);
