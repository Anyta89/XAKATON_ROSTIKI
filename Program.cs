с использованием Система;
с использованием System.IO;
с использованием System.Linq;
с использованием System.Runtime.Remoting.Messaging;
с использованием System.Threading;
с использованием System.Threading.Tasks;
с использованием Telegram.Бот;
с использованием Telegram.Bot.Types;
с использованием Telegram.Bot.Types.Enums;

пространство имен test
{
    внутренний сорт Program
    {
        статический пустота Main(string[] args)
        {
            Инструменты.WriteLineColor("ВВЕДИТЕ КЛЮЧ БОТА", ConsoleColor.Green);
            string key = Tools.ReadLineNoNull("Ââåäèòå êëþ÷:");
            try
            {
                TelegramBotHelper helper = новый TelegramBotHelper(key);
                helper.GetUpdates();
            }
            ловить (Exception ex)
            {
                Tools.WriteLineColor("ÎØÈÁÊÀ Êîä îøèáêè ::", ConsoleColor.Red);
                Tools.WriteLineColor(ex.Message, ConsoleColor.Green);
            }

            Tools.WriteLineColor("ÏÐÎÃÐÀÌÀ ÄÀËÜØÅ ÎÒÊÀÇÀËÀÑÜ ÐÀÁÎÒÀÒÜ (íàæìèòå ëþáóþ êíîïêó äëÿ çàâåðøåíèÿ ïðîãðàììû)", ConsoleColor.Yellow);
            Console.ReadKey();
        }
    }
}

Общественный сорт TelegramBotHelper
{
    Общественный string token;
    TelegramBotClient bot;
    Общественный TelegramBotHelper(string token)
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
            Tools.WriteLineColor("ÎØÈÁÊÀ! ÒÅËÅÃÐÀÌ ÁÎÒ ÑÄÎÕ!!!", ConsoleColor.Red);
            throw new InvalidOperationException("Îøèáêà òåëåãðàì áîòà. Áîò íå ñóùåñòâóåò.");
        }
        else
        {
            Tools.WriteLineColor($"ÒÅËÅÃÐÀÌ ÁÎÒ '{data1.Username}' ÀÊÒÈÂÅÍ", ConsoleColor.Green);
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
                    Tools.WriteLineColor($"{DateTime.Now} ÈÍÔÎÐÌÀÖÈÈ ÍÅ ÏÎËÓ×ÅÍÎ", ConsoleColor.Red);
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
                    bot.SendTextMessageAsync(item.Message.Chat.Id, "Ãòà ðï");
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
        return $"ÂÛÏÎËÍÅÍÎ {Work} ÍÅ ÎÁÐÀÁÀÒÛÂÀÅÒÑß {NoWork}";
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
        "òû íå ìîæåøü îñòàâèòü ýòó ñòðîêó ïóñòóþ!",
        "ÒÛ ÍÅ ÌÎÆÅØÜ ÎÑÒÀÂÈÒÜ ÝÒÓ ÑÒÐÎÊÓ ÏÓÑÒÓÞ!!!",
        "ÕÂÀÒÈÒ ÄÎËÁÈÒÜ ENTER ÍÈ×ÅÃÎ ÍÅ ÍÀÏÈÑÀÂ!!!",
        "ÒÛ ÎÁßÇÀÍ ÍÀÏÈÑÀÒÜ ÕÎÒÜ ×ÒÎÒÎ!!!",
        "ÍÅ ÈÇÄÅÂÀÉÑß ÍÀÄ ÌÍÎÉ ÍÀÏÈØÈ ×ÒÎ ÒÐÅÁÓÅÒÑß!!!",
        "ß ÒÅÁß ÎÒÊËÞ×Ó ÅÑËÈ ÒÛ ÍÅ ÂÏÈØÅØÜ ×ÒÎ ß ÑÊÀÇÀË Ñ×ÈÒÀÞ ÄÎ 3",
        "1!!!",
        "2!!!",
        "2.5!!!",
        "2.8!!!",
        "2.9 Ñ ÍÈÒÎ×ÊÎÉ!!!!",
        "ÄÀ ÈÄÈ ÒÛ Â ÆÎÏÓ(Â ÃÒÀ ÐÏ)"
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
