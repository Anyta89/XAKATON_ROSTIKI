using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tools.WriteLineColor("ÂÂÅÄÈÒÅ ÊËŞ× ÁÎÒÀ", ConsoleColor.Green);
            string key = Tools.ReadLineNoNull("Ââåäèòå êëş÷:");
            try
            {
                TelegramBotHelper helper = new TelegramBotHelper(key);
                helper.GetUpdates();
            }
            catch (Exception ex)
            {
                Tools.WriteLineColor("ÎØÈÁÊÀ Êîä îøèáêè ::", ConsoleColor.Red);
                Tools.WriteLineColor(ex.Message, ConsoleColor.Green);
            }

            Tools.WriteLineColor("ÏĞÎÃĞÀÌÀ ÄÀËÜØÅ ÎÒÊÀÇÀËÀÑÜ ĞÀÁÎÒÀÒÜ (íàæìèòå ëşáóş êíîïêó äëÿ çàâåğøåíèÿ ïğîãğàììû)", ConsoleColor.Yellow);
            Console.ReadKey();
        }
    }
}

public class TelegramBotHelper
{
    public string token;
    TelegramBotClient bot;
    public TelegramBotHelper(string token)
    {
        this.token = token;
    }

    int offset = 0;
    public void GetUpdates()
    {
        bot = new TelegramBotClient(token);
        var data1 = bot.GetMeAsync().Result;

        if (data1 == null && string.IsNullOrEmpty(data1.Username))
        {
            Tools.WriteLineColor("ÎØÈÁÊÀ! ÒÅËÅÃĞÀÌ ÁÎÒ ÑÄÎÕ!!!", ConsoleColor.Red);
            throw new InvalidOperationException("Îøèáêà òåëåãğàì áîòà. Áîò íå ñóùåñòâóåò.");
        }
        else
        {
            Tools.WriteLineColor($"ÒÅËÅÃĞÀÌ ÁÎÒ '{data1.Username}' ÀÊÒÈÂÅÍ", ConsoleColor.Green);
        }
        while (true)
        {
            try
            {
                var _data = bot.GetUpdatesAsync(offset).Result;

                if (_data != null && _data.Length > 0)
                {
                    Tools.WriteLineColor($"{DateTime.Now} {Updates(_data)}", ConsoleColor.Green);
                }
                else
                {
                    Tools.WriteLineColor($"{DateTime.Now} ÈÍÔÎĞÌÀÖÈÈ ÍÅ ÏÎËÓ×ÅÍÎ", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                Tools.WriteLineColor($"{DateTime.Now} {ex.Message}", ConsoleColor.Red);
            }

            Thread.Sleep(1000);
        }
    }

    public string Updates(Update[] updatedata)
    {
        int NoWork = 0, Work = 0;
        foreach (var item in updatedata)
        {
            if (item.Type == UpdateType.Message)
            {
                if (item.Message.Photo != null)
                {
                    StikerAsync(item);
                }
                if (item.Message.Text == "/testcommand1")
                {
                    bot.SendTextMessageAsync(item.Message.Chat.Id, "Ãòà ğï");
                }

                bot.SendTextMessageAsync(item.Message.Chat.Id, item.Message.Text);
                Work++;
            }
            else
            {
                NoWork++;
            }

            offset = item.Id + 1;
        }
        return $"ÂÛÏÎËÍÅÍÎ {Work} ÍÅ ÎÁĞÀÁÀÒÛÂÀÅÒÑß {NoWork}";
    }

    private const string photopath = @"c:\photo.jpg";
    public async Task StikerAsync(Update item)
    {

    }


}

static class Tools
{
    static string[] NullStringMessage =
    {
        "òû íå ìîæåøü îñòàâèòü ıòó ñòğîêó ïóñòóş!",
        "ÒÛ ÍÅ ÌÎÆÅØÜ ÎÑÒÀÂÈÒÜ İÒÓ ÑÒĞÎÊÓ ÏÓÑÒÓŞ!!!",
        "ÕÂÀÒÈÒ ÄÎËÁÈÒÜ ENTER ÍÈ×ÅÃÎ ÍÅ ÍÀÏÈÑÀÂ!!!",
        "ÒÛ ÎÁßÇÀÍ ÍÀÏÈÑÀÒÜ ÕÎÒÜ ×ÒÎÒÎ!!!",
        "ÍÅ ÈÇÄÅÂÀÉÑß ÍÀÄ ÌÍÎÉ ÍÀÏÈØÈ ×ÒÎ ÒĞÅÁÓÅÒÑß!!!",
        "ß ÒÅÁß ÎÒÊËŞ×Ó ÅÑËÈ ÒÛ ÍÅ ÂÏÈØÅØÜ ×ÒÎ ß ÑÊÀÇÀË Ñ×ÈÒÀŞ ÄÎ 3",
        "1!!!",
        "2!!!",
        "2.5!!!",
        "2.8!!!",
        "2.9 Ñ ÍÈÒÎ×ÊÎÉ!!!!",
        "ÄÀ ÈÄÈ ÒÛ Â ÆÎÏÓ(Â ÃÒÀ ĞÏ)"
    };

    public static void WriteLineColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static string ReadLineNoNull(string Message = "Ââîä:")
    {
        string a;
        int i = 0;
        do
        {
            Console.WriteLine(Message);
            a = Console.ReadLine();

            if (i == NullStringMessage.Length)
                Environment.Exit(0);


            if (string.IsNullOrEmpty(a))
            {
                Console.Clear();
                Tools.WriteLineColor(NullStringMessage[i], ConsoleColor.Red);
            }
            i++;
        } while (string.IsNullOrEmpty(a));

        return a;
    }
}
