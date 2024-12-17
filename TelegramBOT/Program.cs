

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;


var client = new TelegramBotClient("7970880429:AAErzZ9G2AKgWx2JmmlLKpd4jWiPE7eY_Ug");
Console.WriteLine($"Сервер начал работу \n");
client.StartReceiving(Update,Error);
Console.ReadLine();
async Task Error(ITelegramBotClient client, Exception exception, HandleErrorSource source, CancellationToken token)
{
    throw new NotImplementedException();
}

async Task Update(ITelegramBotClient client, Update update, CancellationToken token)
{
    var message = update.Message;
    
    if (message.Text != null )
    {
        Console.WriteLine($"{DateTime.UtcNow}  :  {message.Chat.FirstName ?? "Аноним"}  => |   {message.Text} ");
        
        if(message.Text.ToLower().Contains("привет"))
        {
            var answer = await client.SendTextMessageAsync(message.Chat.Id, $"Hello {message.Chat.FirstName}");
            
            Console.WriteLine($"{DateTime.UtcNow}  :  Bot  => | {answer.Text}");
            return;
        }
    }
    if (message.Photo != null)
    {
        await client.SendTextMessageAsync(message.Chat.Id, "Спасибо за фото. Но лучше отправь документом.");
        
        Console.WriteLine($"{DateTime.UtcNow}  :  Bot  => | {message.Photo.GetType().Name} ");
        return;
    }
    if (message.Document != null)
    {
        await client.SendTextMessageAsync(message.Chat.Id, "Спасибо за фото.");

        //var fileId = update.Message.Document.FileId;
        //var fileInfo = await client.GetFileAsync(fileId);
        //var filePath = fileInfo.FilePath;

        //string destinationFilePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\{message.Document.FileName}";

        //await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        //await client.DownloadFile(
        //    filePath: filePath, 
        //    destination: fileStream);
        //fileStream.Close();

        Console.WriteLine($"{DateTime.UtcNow}  :  (Download)Bot  => | {message.Document.FileName} ");
        return;
    }
}

//Console.ReadLine();
