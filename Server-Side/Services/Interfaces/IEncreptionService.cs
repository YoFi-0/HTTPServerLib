using Domain.ReturnsModel;

namespace Server_Side.Services.Interfaces
{
    public interface IEncreptionService
    {
        EncinreptionpoeRes<string> En_Code(string words);
        EncinreptionpoeRes<string> De_Code(string words);
        string Hash(string word);
        bool Compare_Hash(string word, string hash);

    }
}
