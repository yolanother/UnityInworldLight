namespace Inworld.Data
{
    public static class InworldDataUtils
    {
        public static string GetTranscription(this JSONNode response)
        {
            return response["text"]["text"];
        }
        
        public static string GetError(this JSONNode response)
        {
            return response["error"];
        }
        
        public static bool IsInteractionEnd(this JSONNode response)
        {
            if (!response.HasKey("type") || !response.HasKey("control") || !response["control"].HasKey("type"))
            {
                return false;
            }

            return response["type"].AsInt == 5 && response["control"]["type"].AsInt == 3;
        }

        public static string GetSessionId(this JSONNode response)
        {
            return response["sessionId"];
        }
    }
}