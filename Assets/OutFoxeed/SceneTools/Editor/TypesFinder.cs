using System.Collections.Generic;
using OutFoxeed.Editor;

namespace OutFoxeed.SceneTools.Editor
{
    public static class TypesFinder
    {
        public static System.Type[] GetTypesToFind()
        {
            List<System.Type> tempTypes = new List<System.Type>();

            foreach (string line in GetEachPrefs())
            {
                if (line == "") continue;

                System.Type type = TypeUtilities.StringToType(line);

                if (type != null) tempTypes.Add(type);
            }
            return tempTypes.ToArray();
        }
        static string GetTypesToFindFilePath()
        {
            return OutFoxeed.Editor.FileReader.GetFilePath("typesToFind");
        }

        static string[] GetEachPrefs()
        {
            return OutFoxeed.Editor.FileReader.GetLines(GetTypesToFindFilePath());
        }

        public static void AddToPrefs(string newLine)
        {
            OutFoxeed.Editor.FileReader.AddLine(newLine, GetTypesToFindFilePath());
        }
        public static void RemoveFromPrefs(System.Type typeToRemove)
        {
            OutFoxeed.Editor.FileReader.RemoveFromTextFile(typeToRemove.ToString().Remove(0, "UnityEngine.".Length),GetTypesToFindFilePath());
        }
    }
}
