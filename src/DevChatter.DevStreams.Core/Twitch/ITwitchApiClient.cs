using System.Threading.Tasks;

namespace DevChatter.DevStreams.Core.Twitch
{
    public interface ITwitchApiClient
    {
        Task<string> GetJsonData(string url);
    }
}