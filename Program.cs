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
            Tools.WriteLineColor("ВВЕДИТЕ КЛЮЧ БОТА",ConsoleColor.Green);
            string key = Tools.ReadLineNoNull("Введите ключ:");
			try
			{
                TelegramBotHelper helper = new TelegramBotHelper(key);
                helper.GetUpdates();
			}
			catch (Exception ex) 
            { 
                Tools.WriteLineColor("ОШИБКА Код ошибки ::",ConsoleColor.Red);
                Tools.WriteLineColor(ex.Message,ConsoleColor.Green);
			}

            Tools.WriteLineColor("ПРОГРАМА ДАЛЬШЕ ОТКАЗАЛАСЬ РАБОТАТЬ (нажмите любую кнопку для завершения программы)", ConsoleColor.Yellow);
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
            Tools.WriteLineColor("ОШИБКА! ТЕЛЕГРАМ БОТ СДОХ!!!", ConsoleColor.Red);
            throw new InvalidOperationException("Ошибка телеграм бота. Бот не существует.");
        }
        else
        {
            Tools.WriteLineColor($"ТЕЛЕГРАМ БОТ '{data1.Username}' АКТИВЕН", ConsoleColor.Green);
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
                    Tools.WriteLineColor($"{DateTime.Now} ИНФОРМАЦИИ НЕ ПОЛУЧЕНО", ConsoleColor.Red);
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

                if (item.Message.Text == "/start")
                {
                    bot.SendTextMessageAsync(item.Message.Chat.Id, "Здравствуй друг! я бот который умеет делать стикерпаки! для создания стикера укажи имя стикерпака и прикрепи фото и я добавлю стикер!");
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
        return $"ВЫПОЛНЕНО {Work} НЕ ОБРАБАТЫВАЕТСЯ {NoWork}";
    }

    private const string photopath = @"c:\photo.jpg";
    public async Task StikerAsync(Update item)
    {
        Console.WriteLine(item.Message.Photo.Length);
        foreach (var item2 in item.Message.Photo)
        {
            Console.WriteLine("a");
            var fileId = item2.FileId;
            var file = await bot.GetFileAsync(fileId);
            var filePath = $"{file.FilePath}";
            FileStream a = new FileStream(filePath, FileMode.Create);

            await bot.DownloadFileAsync(file.FilePath, a);
            Tools.WriteLineColor($"{file.FilePath}", ConsoleColor.Red);

            a.Close();
        }
    }

    
}

static class Tools
{
    static string[] NullStringMessage =
    {
        "ты не можешь оставить эту строку пустую!",
        "ТЫ НЕ МОЖЕШЬ ОСТАВИТЬ ЭТУ СТРОКУ ПУСТУЮ!!!",
        "ХВАТИТ ДОЛБИТЬ ENTER НИЧЕГО НЕ НАПИСАВ!!!",
        "ТЫ ОБЯЗАН НАПИСАТЬ ХОТЬ ЧТОТО!!!",
        "НЕ ИЗДЕВАЙСЯ НАД МНОЙ НАПИШИ ЧТО ТРЕБУЕТСЯ!!!",
        "Я ТЕБЯ ОТКЛЮЧУ ЕСЛИ ТЫ НЕ ВПИШЕШЬ ЧТО Я СКАЗАЛ СЧИТАЮ ДО 3",
        "1!!!",
        "2!!!",
        "2.5!!!",
        "2.8!!!",
        "2.9 С НИТОЧКОЙ!!!!",
        "ДА ИДИ ТЫ В ЖОПУ(В ГТА РП)"
    };

    public static void WriteLineColor(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
    public static string ReadLineNoNull(string Message = "Ввод:")
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
                Tools.WriteLineColor(NullStringMessage[i],ConsoleColor.Red);
            }
            i++;
        } while (string.IsNullOrEmpty(a));

        return a;
    }
}

