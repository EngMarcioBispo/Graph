using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Graph
{
    public interface IGraph<T>
    {
        IObservable<IEnumerable<T>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : IGraph<T>
    {
        readonly Dictionary<T, IEnumerable<T>> _dict;

        public Graph(IEnumerable<ILink<T>> links)
        {
            _dict = links.GroupBy(link => link.Source)
                         .ToDictionary(g => g.Key, g => g.Select(link => link.Target));                                       
        }

        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            var route = new List<IEnumerable<T>>();
            var line = new Queue<IEnumerable<T>>();
            line.Enqueue(new List<T> { source });
            while (line.Any())
            {
                var currentRoute = line.Dequeue();
                var currentLastNode = currentRoute.Last();
                if (_dict.TryGetValue(currentLastNode, out var targets))
                {
                    foreach (var newTarget in targets.Where(t => !currentRoute.Contains(t)))
                    {
                        var nextList = new List<T>(currentRoute) { newTarget };

                        if (EqualityComparer<T>.Default.Equals(newTarget, target))
                            route.Add(nextList.AsReadOnly());
                        else
                            line.Enqueue(nextList);
                    }
                }
            }
            return route.ToObservable();
        }
    }
}
