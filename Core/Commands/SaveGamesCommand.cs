using Core.Models;
using Newtonsoft.Json;
using System.Text;

namespace Core.Commands
{
    public record SaveGamesCommandContext(GameData GameData, string Directory, string FileName);
    public class SaveGamesCommand : ICommand<EmptyObject>
    {
        private readonly SaveGamesCommandContext context;

        public SaveGamesCommand(SaveGamesCommandContext context)
        {
            this.context = context;
        }

        public EmptyObject Execute()
        {
            if (!Directory.Exists(context.Directory))
            {
                Directory.CreateDirectory(context.Directory);
            }
            using (var stream = File.Create($"{context.Directory}\\{context.FileName}"))
            {
                var json = JsonConvert.SerializeObject(context.GameData);
                stream.Write(new UTF8Encoding(true).GetBytes(json));
            }

            return new EmptyObject();
        }
    }
}
