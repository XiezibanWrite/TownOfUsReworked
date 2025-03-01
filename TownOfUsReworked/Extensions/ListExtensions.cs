namespace TownOfUsReworked.Extensions
{
    [HarmonyPatch]
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list)
        {
            if (list.Count is 1 or 0)
                return;

            var count = list.Count;

            for (var i = 0; i <= count - 1; ++i)
            {
                var r = URandom.RandomRangeInt(i, count);
                (list[r], list[i]) = (list[i], list[r]);
            }
        }

        public static T TakeFirst<T>(this List<T> list)
        {
            var item = list[0];

            while (item == null)
            {
                list.Shuffle();
                item = list[0];
            }

            list.RemoveAt(0);
            return item;
        }

        public static void RemoveRange<T>(this List<T> list, List<T> list2)
        {
            foreach (var item in list2)
            {
                if (list.Contains(item))
                    list.Remove(item);
            }
        }

        public static void AddRanges<T>(this List<T> main, params List<T>[] items)
        {
            foreach (var list in items)
                main.AddRange(list);
        }

        public static void RemoveRanges<T>(this List<T> main, params List<T>[] items)
        {
            foreach (var list in items)
                main.RemoveRange(list);
        }

        public static bool Replace<T>(this List<T> list, T item1, T item2)
        {
            if (list.Contains(item1))
            {
                var index = list.IndexOf(item1);
                list.Remove(item1);
                list.Insert(index, item2);
            }

            return list.Contains(item1);
        }

        public static List<T> Il2CppToSystem<T>(this Il2CppSystem.Collections.Generic.List<T> list) => list.ToArray().ToList();

        public static Il2CppSystem.Collections.Generic.List<T> SystemToIl2Cpp<T>(this List<T> list)
        {
            var newList = new Il2CppSystem.Collections.Generic.List<T>();

            foreach (var item in list)
                newList.Add(item);

            return newList;
        }

        public static T Random<T>(this List<T> list, T defaultVal = default)
        {
            if (list.Count == 0)
                return defaultVal;
            else if (list.Count == 1)
                return list[0];
            else
                return list[URandom.RandomRangeInt(0, list.Count)];
        }

        public static T Random<T>(this List<T> list, Func<T, bool> predicate, T defaultVal = default) => list.Where(predicate).ToList().Random(defaultVal);

        public static int Count<T>(this Il2CppSystem.Collections.Generic.List<T> list, Func<T, bool> predicate) => list.Il2CppToSystem().Count(predicate);

        public static bool Any<T>(this Il2CppSystem.Collections.Generic.List<T> list, Func<T, bool> predicate) => list.Il2CppToSystem().Any(predicate);

        public static IEnumerable<T> Where<T>(this Il2CppSystem.Collections.Generic.List<T> list, Func<T, bool> predicate) => list.Il2CppToSystem().Where(predicate);

        public static void ForEach<T>(this Il2CppSystem.Collections.Generic.List<T> list, Action<T> action) => list.Il2CppToSystem().ForEach(action);

        public static T Random<T>(this Il2CppSystem.Collections.Generic.List<T> list, T defaultVal = default) => list.Il2CppToSystem().Random(defaultVal);

        public static T Random<T>(this Il2CppSystem.Collections.Generic.List<T> list, Func<T, bool> predicate, T defaultVal = default) => list.Il2CppToSystem().Random(predicate,
            defaultVal);

        public static T Find<T>(this Il2CppSystem.Collections.Generic.List<T> list, Predicate<T> predicate) => list.Il2CppToSystem().Find(predicate);
    }
}