using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOSSM.Util
{
    internal class Parallel
    {
        public static async Task All<T>(IEnumerable<T> list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action(t);
                }));

            }

            await Task.WhenAll(result.ToArray());
        }
        public static async Task All<T>(T[] list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action(t);
                }));

            }

            await Task.WhenAll(result.ToArray());
        }

        public static async Task All<T>(Array list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action((T)t);
                }));

            }

            await Task.WhenAll(result.ToArray());
        }

        public static async Task Any<T>(IEnumerable<T> list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action(t);
                }));

            }

            await Task.WhenAny(result.ToArray());
        }

        public static async Task Any<T>(T[] list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action(t);
                }));

            }

            await Task.WhenAny(result.ToArray());
        }

        public static async Task Any<T>(Array list, Action<T> action)
        {
            List<Task> result = new List<Task>();
            foreach (var t in list)
            {
                result.Add(Task.Run(() =>
                {
                    action((T)t);
                }));

            }

            await Task.WhenAny(result.ToArray());
        }

    }
}
