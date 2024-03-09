using PETR_Robot;
using System.Diagnostics;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

// PETR-Robot Main
Stopwatch runTime = Stopwatch.StartNew();
Console.WriteLine("PETR-Robot started! Time now is: " + DateTime.Now.ToString());

string telegramBotToken = Utils.GetEnvironmentVariable<string>("TELEGRAM_BOT_TOKEN");
long groupChatID = Utils.GetEnvironmentVariable<long>("GROUP_CHAT_ID");
int fixedMessageID = Utils.GetEnvironmentVariable<int>("FIXED_MESSAGE_ID");
bool showLogMessage = Utils.GetEnvironmentVariable<bool>("SHOW_LOG_MESSAGE");

var bot = new TelegramBotClient(telegramBotToken);
var chatID = new Telegram.Bot.Types.ChatId(groupChatID);

bool IsB3TradingHours =
    DateTime.Now.DayOfWeek >= DayOfWeek.Monday &&
    DateTime.Now.DayOfWeek <= DayOfWeek.Friday &&
    DateTime.Now.TimeOfDay >= new TimeSpan(10, 19, 0) &&
    DateTime.Now.TimeOfDay <= new TimeSpan(18, 15, 0);

if (IsB3TradingHours)
{
    var b3QuotePETR4 = await ApiCalls.GetB3Quote("PETR4");
    var b3QuotePETR3 = await ApiCalls.GetB3Quote("PETR3");

    if (b3QuotePETR4.Item1 != -1)
    {
        //"🟢PETR4 R$40,57 +1,05%\r\n🔴PETR3 R$41,45 -0,57%\r\n(01/03/24 18h07 *Delay 15 min)");
        string message = String.Format("PETR4: {0} {1}% PETR3: {2} {3}% {4}",
            b3QuotePETR4.Item1, b3QuotePETR4.Item2, b3QuotePETR3.Item1, b3QuotePETR3.Item2, DateTime.Now);

        var B3PetrQuoteMessage = await bot.EditMessageTextAsync(chatID, fixedMessageID, message);
    }
}

runTime.Stop();
string finishMessage = "PETR-Robot finish! Total run time is: " + runTime.Elapsed.ToString();
Console.WriteLine(finishMessage);
if (showLogMessage)
    await bot.SendTextMessageAsync(chatID, finishMessage);
