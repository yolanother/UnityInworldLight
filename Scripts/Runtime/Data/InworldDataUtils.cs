namespace Inworld.Data
{
    public static class InworldDataUtils
    {
        public static string GetTranscription(this JSONNode response)
        {
            return response["text"]["text"];
        }
        
        public static bool IsInteractionEnd(this JSONNode response)
        {
            return response["type"].AsInt == 5 && response["control"]["type"].AsInt == 3;
        }
    }
}