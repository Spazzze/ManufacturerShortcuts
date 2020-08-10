using System.Collections.Generic;
using System.Linq;

namespace ManufacturerShortcuts.Scripts.ManufacturerShortcuts
{
    public static class VeryLazyGuysList
    {
        private static readonly Dictionary<string, string> VeryLazyGuys = new Dictionary<string, string>
        {
            {"Djekke", "🤖"},
            {"Spaze", "🪐"},
            {"Flaky", "🌠"},
        };

        public static string Tag(string name) => VeryLazyGuys[name];
        public static bool IsLazyGuy(string name) => VeryLazyGuys.Keys.Contains(name);
    }
}