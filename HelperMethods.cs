using ARICommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 
{
    public class QuotesHelperMethods
    {
        public bool GetTextAround(string contentText, int index, string name, out string text)
        {
            text = "";
            int quoteLength = 210;
            int indexOfFirstSentence = 0;
            try
            {

        
                GetSingleBlob(contentText, index, ref text, quoteLength/2);
                int number = CheckLength(text);

                if (number < quoteLength)
                {
                    int adjust = quoteLength - number;
                
                }
                number = CheckLength(text);
                if (number < quoteLength)
                {
                    int adjust = quoteLength - number;
                    if (indexOfFirstSentence - adjust <= 0)
                    {
                        text = contentText.Substring(0, quoteLength);

                    }
                    else
                    {
                        text = contentText.Substring((indexOfFirstSentence - adjust), quoteLength);
                    }
                }
            }
            catch
            {
                Log.Instance.Debug("Error in GetTextAround, contentText: {0}, index: {1}", contentText, index);
            }

            return true;
        }

        private void GetSingleBlob(string contentText, int index, ref string text,  int directionLength)
        {
            if ((index - directionLength > 0) && (contentText.Length > (index + directionLength)))
            {
                text = contentText.Substring(index - directionLength, directionLength * 2);
            }
            else if ((index - directionLength < 0) && (contentText.Length > (index + directionLength)))
            {
                text = contentText.Substring(0, directionLength * 2);
            }
            else if ((index - directionLength > 0) && (contentText.Length < (index + directionLength)))
            {
                text = contentText.Substring(contentText.Length - directionLength * 2, directionLength * 2);
            }
            else
                text = contentText;
            
            
        }

        private static void GetSentence(string contentText, int index, ref string text, ref int indexOfFirstSentence, out int indexOfSecondSentence, out int total)
        {
            try
            {
                string firstPart = contentText.Substring(0, contentText.Length - contentText.Substring(index).Length);
                indexOfFirstSentence = firstPart.LastIndexOf('.') + 1;
            }
            catch
            {
                int one = 1;
            }

            string secondPart = contentText.Substring(index);
            int lastPeriod = secondPart.IndexOf('.');
            int whereSecondPartStarts = index;
            indexOfSecondSentence = secondPart.IndexOf('.') + index;



            total = contentText.Length;

            text = contentText.Substring(indexOfFirstSentence, (indexOfSecondSentence) > total ? total - indexOfFirstSentence : indexOfSecondSentence - indexOfFirstSentence);
        }

        private int CheckLength(string text)
        {
            if (String.IsNullOrEmpty(text))
                return 0;
            else
            {
                return text.Length;
            }

        }

        public bool FindQuoteBack(string text, char openQuote, char closingQuote, out string foundQuote)
        {
            return FindQuoteBack(text, openQuote, closingQuote, false, out foundQuote);
        }

        public bool FindQuoteBack(string text, char openQuote, char closingQuote, bool checkIfPosessive, out string quote)
        {
            quote = null;
            List<int> quotMarksPos = new List<int>();
            char[] arr = text.ToCharArray();
            for (int i = arr.Length - 1; i > -1; i--)
            {
                if (arr[i] == closingQuote)
                {

                    if (checkIfPosessive)
                    {
                        if (!PosessiveCase(arr, i) && !quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);
                    }
                    else
                        quotMarksPos.Add(i);

                }
                if (arr[i] == openQuote)
                {
                    if (checkIfPosessive)
                    {
                        if (!PosessiveCase(arr, i) && !quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);
                    }
                    else
                        if (!quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);

                }
                if (quotMarksPos.Count == 2)
                    break;
            }

            if (quotMarksPos.Count == 2)
            {
                int length = quotMarksPos[0] - quotMarksPos[1] + 1;
                quote = text.Substring(quotMarksPos[1], length);
            }
            else if (quotMarksPos.Count == 1)
            {
                return false;
            }
            return true;
        }

        public bool FindQuoteForward(string text, char p1, char p2, out string foundQuote)
        {
            return FindQuoteForward(text, p1, p2, false, out foundQuote);
        }

        public bool FindQuoteForward(string text, char openQuote, char closingQuote, bool checkIfPosessive, out string quote)
        {
            quote = null;
            int quoteLength = 200;
            List<int> quotMarksPos = new List<int>();
            char[] arr = text.ToCharArray();
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                if (arr[i] == openQuote)
                {
                    if (checkIfPosessive)
                    {
                        if (!PosessiveCase(arr, i, true) && !quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);
                    }
                    else
                        if (!quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);

                }
                //quotMarksPos.Add(i);

                if (arr[i] == closingQuote)
                {
                    if (checkIfPosessive)
                    {
                        if (!PosessiveCase(arr, i, true) && !quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);
                    }
                    else
                        if (!quotMarksPos.Contains(i))
                            quotMarksPos.Add(i);
                }
                if (quotMarksPos.Count == 2)
                    break;
            }
            if (quotMarksPos.Count == 2)
            {
                int length = quotMarksPos[1] - quotMarksPos[0];
                if (length < quoteLength)
                {
                    int difference = quoteLength - length;
                    if ((quotMarksPos[0] + difference) < text.Length)
                        quote = text.Substring(quotMarksPos[0], difference);
                    else
                        quote = text;
                }
                else

                    quote = text.Substring(quotMarksPos[0], length + 1);

            }
            else if ((quotMarksPos.Count == 1))
            {
                return false;

            }
            return true;

        }

        public bool PosessiveCase(char[] arr, int i)
        {
            if (i < 1 || i == arr.Length - 1) return false;

            return (arr[i - 1] == ' ' || arr[i + 1] == '.') ? false : true;
        }

        public bool PosessiveCase(char[] arr, int i, bool lookForward)
        {
            if (i < 1 || i == arr.Length - 1) return false;
            if (char.IsWhiteSpace(arr[i + 1])) return true;
            if (arr[i + 1] == 't' && char.IsWhiteSpace(arr[i + 2])) return true;

            // TODO: JMD this needs to check array bounds, it crashes sometimes
            if (arr[i + 1] == 's' && char.IsWhiteSpace(arr[i + 2])) return true;
            if (!char.IsWhiteSpace(arr[i + 1]) && (!char.IsWhiteSpace(arr[i - 1]))) return true;

            return false;

        }

        /// <summary>
        /// General method to find a quotation in the string
        /// </summary>
        /// <param name="forward">Check if the direction of search is forward</param>
        /// <param name="contentText">Text to check</param>
        /// <param name="index">Index of the name occurence in the text string</param>
        /// <param name="name">Name to look for</param>
        /// <param name="min">Amount of steps to go back from the name</param>
        /// <param name="max">Amount of steps to go forward from the name</param>
        /// <param name="p1">Opening quote</param>
        /// <param name="p2">Closing quote</param>
        /// <param name="isPosessive">check if quote is used to indicate a possevive case</param>
        /// <param name="foundQuote">Return found quotation</param>
        /// <returns>True if the qoute is found</returns>
        public bool FindQuote(bool forward, string contentText, int index, string name, int min, int max, char p1, char p2, bool isPosessive, out string foundQuote)
        {

            string check = "";
            string text = "";
            foundQuote = null;

            int goForward = (index >= min) ? min : index;
            int goBack = (index + max) < contentText.Length ? max : contentText.Length - index;

            if (forward)
            {
                //text =contentText.Substring(index, goBack);
                GetTextAround(contentText, index, name, out text);
                if (isPosessive)
                {
                    if (!FindQuoteForward(text, p1, p2, true, out foundQuote))
                    {
                        text = contentText.Substring(index, goBack);
                        FindQuoteForward(text, p1, p2, true, out foundQuote);
                    }
                }
                else
                    if (!FindQuoteForward(text, p1, p2, out foundQuote))
                    {
                        text = contentText.Substring(index, goBack);
                        FindQuoteForward(text, p1, p2, out foundQuote);

                    }
                try
                {
                    if (foundQuote != null)
                    {
                        int indexOfFoundQuote = contentText.IndexOf(foundQuote);
                        if (indexOfFoundQuote < 40)
                            check = contentText.Substring(0, foundQuote.Length + 30);
                        else

                            check = contentText.Substring(contentText.IndexOf(foundQuote) - 40, foundQuote.Length + 30);
                    }
                }
                catch
                {

                }

            }
            else
            {
       
                GetTextAround(contentText, index, name, out text);
                if (isPosessive)
                {
                    if (!FindQuoteBack(text, p1, p2, true, out foundQuote))
                    {
                        text = contentText.Substring(index - goForward, goForward);
                        FindQuoteBack(text, p1, p2, true, out foundQuote);

                    }
                }
                else
                    if (!FindQuoteBack(text, p1, p2, out foundQuote))
                    {
                        text = contentText.Substring(index - goForward, goForward);
                        FindQuoteBack(text, p1, p2, out foundQuote);
                    }
                try
                {
                    if (foundQuote == null) return false;
                    if (foundQuote.Length < contentText.IndexOf(foundQuote) + foundQuote.Length + name.Length + 30)
                        check = (foundQuote == null) ? check : contentText.Substring(contentText.IndexOf(foundQuote) + foundQuote.Length, name.Length);
                    else


                        check = (foundQuote == null) ? check : contentText.Substring(contentText.IndexOf(foundQuote) + foundQuote.Length, name.Length + 30);
                }
                catch (Exception e)
                {

                }

            }
            return (check.Contains(name)) ? true : false; 
        }
    }
}
