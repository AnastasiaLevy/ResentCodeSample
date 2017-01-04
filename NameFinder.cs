using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ARIBase;

namespace 
{
    public class AnalystNameFinder
    {
        public AnalystNameFinder() { }

        public Dictionary<int, string> FindAnalystName(string text, AnalystName name)
        {
            var results = new Dictionary<int, string>();
            if (name == null || string.IsNullOrEmpty(text))
                return results;  // if no name or text empty, return empty results (don't crash)

            List<string> names = new List<string>();
            IEnumerable<int> indexes;

            names = getNameVars(name);
            if (names == null)
                return results;

            foreach (string s in names)
            {
                indexes = text.IndexOfAll(s);
                if (indexes == null)
                    continue;
                foreach (var i in indexes)
                {
                    if (!results.ContainsKey(i))
                        results.Add(i, s);

                }
            }
            results = findIndexRange(results);

            return results;
        }

        private Dictionary<int, string> findIndexRange(Dictionary<int, string> results)
        {
            for (int i = 0; i < results.Count - 1; i++)
            {
                int index = results.ElementAt(i).Key;
                int min = index - results.ElementAt(i).Value.Length;
                int max = index + results.ElementAt(i).Value.Length;
               results = (from el in results
                                   where el.Key > max || el.Key < min || el.Key == index
                          select el).ToDictionary(x => x.Key, x => x.Value);
            }

            return results;
        }

        private static List<string> getNameVars(AnalystName analystName)
        {
            var generator = new QuoteFinderAnalystNamePermutationGenerator();
            var namePermutations = generator.GetPermutations(analystName.FirstName, analystName.LastName, analystName.MiddleName, analystName.NickName);
            return namePermutations.OrderByDescending(e => e.Length).ToList();
        }

    }
}
