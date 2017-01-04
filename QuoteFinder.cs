
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 
{
    public class AnalystQuoteFinder
    {
        private List<QuoteSyntaxChecker> _quoteSyntaxCheckers = new List<QuoteSyntaxChecker>();

        public AnalystQuoteFinder()
        {
            int avg = 300;

            _quoteSyntaxCheckers.Add(new SyntaxChecker2 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker1 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker4 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker3 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker6 { Min = avg, Max = avg });

            _quoteSyntaxCheckers.Add(new SyntaxChecker5 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker8 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker7 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker10 { Min = avg, Max = avg });
            _quoteSyntaxCheckers.Add(new SyntaxChecker9 { Min = avg, Max = avg });

            _quoteSyntaxCheckers.Add(new SyntaxCheckerLast { Min = avg, Max = avg });
            // more syntax checkers here...
        }

        public List<string> FindQuotes(string contentText, Dictionary<int, string> analystNameIndexes)
        {
            List<string> foundQuotes = new List<string>();
            foreach (var analystNameIndex in analystNameIndexes)
            {
                foreach (var syntaxChecker in _quoteSyntaxCheckers)
                {
                    string quote;
                    if (syntaxChecker.FindQuote(contentText, analystNameIndex.Key, analystNameIndex.Value, out quote))
                    {
                        if (!foundQuotes.Contains(quote))
                        { 
                            foundQuotes.Add(quote);
                            break;
                        }
                    }
                }
            }
            return foundQuotes;
        }


    }
}
