namespace Inworld.Data
{
    public static class InworldDataUtils
    {
        public static bool IsInteractionEnd(this JSONNode response)
        {
            return response["type"].AsInt == 5 && response["control"]["type"].AsInt == 3;
        }
    }
}