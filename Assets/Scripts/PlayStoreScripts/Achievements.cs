using UnityEngine;

namespace PlayStoreScripts
{
    public static class Achievements
    {
        public static readonly string BOO = "CgkI06WkmfgFEAIQAQ";

        public static void Unlock(string achievementId)
        {
            if (!PlayServices.usePlayServices)
                return;
            
            Social.ReportProgress(achievementId, 100.0d, success =>
            {
                // Handle success or not
            });
        }
    }
}