using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GoldProject
{
    public static class Achievements
    {
        public static readonly string BOO = "CgkI06WkmfgFEAIQAQ";

        public static void Unlock(string achievementId)
        {
            Social.ReportProgress(achievementId, 100.0d, success =>
            {
                // Handle success or not
            });
        }
    }
}