using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace HtmlDiff
{
    public class HtmlDiff
    {
        /// <summary>
        /// This value defines balance between speed and memory utilization. The higher it is the faster it works and more memory consumes.
        /// </summary>
        private const int MatchGranularityMaximum = 4;

        /// <summary>
        /// Will show debug output
        /// operation.PrintDebugInfo(_oldWords, _newWords);
        /// </summary>
        private const bool ShowDebugOutput = true;

        /// <summary>
        /// Will show debug output
        /// use WrapTextUniqueId method
        /// </summary>
        private const bool WrapUniqueIdText = true;

        /// <summary>
        /// Default Insert Tag value
        /// </summary>
        public string InsertTagValue = "Inserted";

        /// <summary>
        /// Default Delete Tag value
        /// </summary>
        public string DeleteTagValue = "Deleted";

        /// <summary>
        /// Use &nbsp; instead of " " space when insert 
        /// </summary>
        private const bool InsertNonBreakingSpaces = false;

        private StringBuilder _content;
        private string _newText;
        private string _oldText;


        private static Dictionary<string, int> _specialCaseClosingTags = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            {"</strong>", 0},
            {"</em>", 0},
            {"</b>",0},
            {"</i>",0},
            {"</big>",0},
            {"</small>",0},
            {"</u>",0},
            {"</sub>",0},
            {"</sup>",0},
            {"</strike>",0},
            {"</div>",0},
            {"</span>",0},
            {"</li>",0},
            {"</ul>",0},
            {"</ol>",0},
            {"</p>",0},
            {"</th>",0},
            {"</td>",0},
            {"</tr>",0},
            {"</colgroup>",0},
            {"</table>",0},
            {"</tbody>",0},
            {"</s>",0}
        };

        private static readonly Regex _specialCaseOpeningTagRegex = new Regex(
            //"<([a-z]+)[\\>\\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            "<((strong)|(em)|(b)|(i)|(big)|(small)|(u)|(sub)|(sup)|(strike)|(div)|(span)|(li)|(ul)|(ol)|(p)|(th)|(td)|(tr)|(colgroup)|(table)|(tbody)|(s))[\\>\\s]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        /// <summary>
        /// Tracks opening and closing formatting tags to ensure that we don't inadvertently generate invalid html during the diff process.
        /// </summary>
        private Stack<string> _specialTagDiffStack;

        private string[] _newWords;
        private string[] _oldWords;
        private int _matchGranularity;
        private List<Regex> _blockExpressions;

        /// <summary>
        /// Defines how to compare repeating words. Valid values are from 0 to 1.
        /// This value allows to exclude some words from comparison that eventually
        /// reduces the total time of the diff algorithm.
        /// 0 means that all words are excluded so the diff will not find any matching words at all.
        /// 1 (default value) means that all words participate in comparison so this is the most accurate case.
        /// 0.5 means that any word that occurs more than 50% times may be excluded from comparison. This doesn't
        /// mean that such words will definitely be excluded but only gives a permission to exclude them if necessary.
        /// </summary>
        public double RepeatingWordsAccuracy { get; set; }

        /// <summary>
        /// If true all whitespaces are considered as equal
        /// </summary>
        public bool IgnoreWhitespaceDifferences { get; set; }


        /// <summary>
        /// Ignore tag difference
        /// </summary>
        public bool IgnoreTagDifferences
        {
            get; set;
        }

        /// <summary>
        /// If some match is too small and located far from its neighbors then it is considered as orphan
        /// and removed. For example:
        /// <code>
        /// aaaaa bb ccccccccc dddddd ee
        /// 11111 bb 222222222 dddddd ee
        /// </code>
        /// will find two matches <code>bb</code> and <code>dddddd ee</code> but the first will be considered
        /// as orphan and ignored, as result it will consider texts <code>aaaaa bb ccccccccc</code> and
        /// <code>11111 bb 222222222</code> as single replacement:
        /// <code>
        /// &lt;del&gt;aaaaa bb ccccccccc&lt;/del&gt;&lt;ins&gt;11111 bb 222222222&lt;/ins&gt; dddddd ee
        /// </code>
        /// This property defines relative size of the match to be considered as orphan, from 0 to 1.
        /// 1 means that all matches will be considered as orphans.
        /// 0 (default) means that no match will be considered as orphan.
        /// 0.2 means that if match length is less than 20% of distance between its neighbors it is considered as orphan.
        /// </summary>
        public double OrphanMatchThreshold { get; set; }

        /// <summary>
        /// Is require to generate id field
        /// If true, define IDName too
        /// </summary>
        public bool GenerateID { get; set; }

        /// <summary>
        /// Value of Updated-ref attribute
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// IDName for tag, for ex: GEN01
        /// Will be timestamp added
        /// </summary>
        public string IDName { get; set; }

        /// <summary>
        ///     Initializes a new instance of the class.
        /// </summary>
        /// <param name="oldText">The old text.</param>
        /// <param name="newText">The new text.</param>
        public HtmlDiff(string oldText, string newText, bool isXMLIndent = true)
        {
            RepeatingWordsAccuracy = 1d; //by default all repeating words should be compared
            IgnoreWhitespaceDifferences = true;
            IgnoreTagDifferences = true;
            GenerateID = true;
            _oldText = isXMLIndent ? PrettyXml(oldText) : oldText;
            _newText = isXMLIndent ? PrettyXml(newText) : newText;

            _content = new StringBuilder();
            _specialTagDiffStack = new Stack<string>();
            _blockExpressions = new List<Regex>();
        }

        void Reset()
        {
            ClearBlockExpression();
            _content = new StringBuilder();
            _specialTagDiffStack = new Stack<string>();
            _blockExpressions = new List<Regex>();
        }

        public void ClearBlockExpression()
        {
            _blockExpressions.Clear();
        }
        public static string Execute(string oldText, string newText)
        {
            return new HtmlDiff(oldText, newText).Build();
        }

        /// <summary>
        /// Builds the HTML diff output
        /// </summary>
        /// <returns>HTML diff markup</returns>
        public string Build(string Namespace = "")
        {
            // If there is no difference, don't bother checking for differences
            if (_oldText == _newText)
            {
                return _newText;
            }

            SplitInputsToWords();

            _matchGranularity = Math.Min(MatchGranularityMaximum, Math.Min(_oldWords.Length, _newWords.Length));

            List<Operation> operations = Operations();

            foreach (Operation item in operations)
            {
                PerformOperation(item);
            }

            if (Namespace == "")
            {
                return _content.ToString();
            }
            else
            {
                return ReplaceTagsWithNamespaces(_content.ToString(), Namespace);
            }

        }

        public string BuildXml(string Namespace = "")
        {
            //AddBlockExpression(new Regex(@"<e:Graphic-file(.|\n)*?>(.|\n)*?<\/e:Graphic-file>", RegexOptions.IgnoreCase));
            AddBlockExpression(new Regex(@"<e:Figure><e:Graphic-file(.|\n)*?>(.|\n)*?<\/e:Graphic-file><\/e:Figure>", RegexOptions.IgnoreCase));
            AddBlockExpression(new Regex(@"<e:Amendment(.|\n)*?>(.|\n)*?<\/e:Amendment>", RegexOptions.IgnoreCase));
            //AddBlockExpression(new Regex(@"<x:th(.|\n)*?>(.|\n)*?<\/x:th>", RegexOptions.IgnoreCase));
            //AddBlockExpression(new Regex(@"<x:td(.|\n)*?>(.|\n)*?<\/x:td>", RegexOptions.IgnoreCase));
            var xml = Build(Namespace);
            if (xml.IsValidXhtml())
                return xml;

            else
            {
                Reset();
                AddBlockExpression(new Regex(@"<x:table(.|\n)*?>(.|\n)*?<\/x:table>", RegexOptions.IgnoreCase));
                xml = Build(Namespace);
                if (xml.IsValidXhtml())
                {
                    Utils.ErrorMessage.Add("Error fixed using table block expression checking");
                    return xml;
                }
                else
                {
                    Reset();
                    AddBlockExpression(new Regex(@"<x:div(.|\n)*?>(.|\n)*?<\/x:div>", RegexOptions.IgnoreCase));
                    xml = Build(Namespace);
                    if (xml.IsValidXhtml())
                    {
                        Utils.ErrorMessage.Add("Error fixed using div block expression checking");
                        return xml;
                    }
                    else
                    {
                        Utils.ErrorMessage.Add(
                            "Error can`t be fixed! Please contact with technic support regard this issue. Last changes without amendment marks will be shown");
                        return _newText;
                    }
                }
            }
        }

        string PrettyXml(string xml)
        {
            var StringWriter = new Utf8StringWriter();
            var element = XElement.Parse(xml);
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;

            using (var xmlWriter = XmlWriter.Create(StringWriter, settings))
            {
                element.Save(xmlWriter);
            }
            return StringWriter.ToString();
        }

        private string ReplaceTagsWithNamespaces(string data, string NameSpace)
        {
            return data.Replace($@"<{InsertTagValue}", $@"<{NameSpace}:{InsertTagValue}")
                .Replace($@"</{InsertTagValue}", $@"</{NameSpace}:{InsertTagValue}")
                .Replace($@"<{DeleteTagValue}", $@"<{NameSpace}:{DeleteTagValue}")
                .Replace($@"</{DeleteTagValue}", $@"</{NameSpace}:{DeleteTagValue}");
        }

        /// <summary>
        /// Uses <paramref name="expression"/> to group text together so that any change detected within the group is treated as a single block
        /// </summary>
        /// <param name="expression"></param>
        public void AddBlockExpression(Regex expression)
        {
            _blockExpressions.Add(expression);
        }

        private void SplitInputsToWords()
        {
            _oldWords = WordSplitter.ConvertHtmlToListOfWords(_oldText, _blockExpressions);

            //free memory, allow it for GC
            //_oldText = null;

            _newWords = WordSplitter.ConvertHtmlToListOfWords(_newText, _blockExpressions);

            //free memory, allow it for GC
            //_newText = null;
        }

        private void PerformOperation(Operation operation)
        {
#if DEBUG
            if (ShowDebugOutput)
                operation.PrintDebugInfo(_oldWords, _newWords);
#endif

            switch (operation.Action)
            {
                case Action.Equal:
                    ProcessEqualOperation(operation);
                    break;
                case Action.Delete:
                    ProcessDeleteOperation(operation, "diffdel");
                    break;
                case Action.Insert:
                    ProcessInsertOperation(operation, "diffins");
                    break;
                case Action.None:
                    break;
                case Action.Replace:
                    ProcessReplaceOperation(operation);
                    break;
            }
        }

        private void ProcessReplaceOperation(Operation operation)
        {
            ProcessDeleteOperation(operation, "diffmod");
            ProcessInsertOperation(operation, "diffmod");
        }

        private void ProcessInsertOperation(Operation operation, string cssClass)
        {
            List<string> text = _newWords.Where((s, pos) => pos >= operation.StartInNew && pos < operation.EndInNew).ToList();
            InsertTag(InsertTagValue, cssClass, text);
        }

        private void ProcessDeleteOperation(Operation operation, string cssClass)
        {
            List<string> text = _oldWords.Where((s, pos) => pos >= operation.StartInOld && pos < operation.EndInOld).ToList();
            InsertTag(DeleteTagValue, cssClass, text);
        }

        private void ProcessEqualOperation(Operation operation)
        {
            string[] result =
                _newWords.Where((s, pos) => pos >= operation.StartInNew && pos < operation.EndInNew).ToArray();
            _content.Append(String.Join("", result));
        }


        private string GenID()
        {
            string result = IDName;
            result += HiResDateTime.UtcNowTicksString;
            return result;
        }



        /// <summary>
        ///     This method encloses words within a specified tag (ins or del), and adds this into "content",
        ///     with a twist: if there are words contain tags, it actually creates multiple ins or del,
        ///     so that they don't include any ins or del. This handles cases like
        ///     old: '<p>a</p>'
        ///     new: '<p>ab</p>
        ///     <p>
        ///         c</b>'
        ///         diff result: '<p>a<ins>b</ins></p>
        ///         <p>
        ///             <ins>c</ins>
        ///         </p>
        ///         '
        ///         this still doesn't guarantee valid HTML (hint: think about diffing a text containing ins or
        ///         del tags), but handles correctly more cases than the earlier version.
        ///         P.S.: Spare a thought for people who write HTML browsers. They live in this ... every day.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cssClass"></param>
        /// <param name="words"></param>
        private void InsertTag(string tag, string cssClass, List<string> words)
        {
            while (true)
            {
                if (words.Count == 0)
                {
                    break;
                }

                string[] nonTags = ExtractConsecutiveWords(words, x => !Utils.IsTag(x));

                string specialCaseTagInjection = string.Empty;
                bool specialCaseTagInjectionIsBefore = false;

                if (nonTags.Length != 0)
                {
                    string text = "";
                    if (WrapUniqueIdText)
                    {
                        // fixes empty strings

                        if (nonTags.Length == 1 && Utils.IsWhiteSpace(nonTags[0]))
                        {
                            text = string.Join("", nonTags);
                        }
                        else
                            text = Utils.WrapTextUniqueId(string.Join("", nonTags), tag, GenID(), Ref);
                    }
                    else
                    {
                        text = Utils.WrapText(string.Join("", nonTags), tag, cssClass);
                    }

                    _content.Append(text);
                }
                else
                {
                    // Check if the tag is a special case
                    if (_specialCaseOpeningTagRegex.IsMatch(words[0]))
                    {
                        _specialTagDiffStack.Push(words[0]);
                        specialCaseTagInjection = IgnoreTagDifferences ? "" : $@"<{InsertTagValue} class='mod'>";
                        if (tag == DeleteTagValue)
                        {
                            words.RemoveAt(0);

                            // following tags may be formatting tags as well, follow through
                            while (words.Count > 0 && _specialCaseOpeningTagRegex.IsMatch(words[0]))
                            {
                                words.RemoveAt(0);
                            }
                        }
                    }

                    else if (_specialCaseClosingTags.ContainsKey(words[0]))
                    {
                        var openingTag = _specialTagDiffStack.Count == 0 ? null : _specialTagDiffStack.Pop();

                        // If we didn't have an opening tag, and we don't have a match with the previous tag used 
                        if (openingTag == null || openingTag != words.Last().Replace("/", ""))
                        {
                            // do nothing
                        }
                        else
                        {
                            specialCaseTagInjection = IgnoreTagDifferences ? "" : $@"</{InsertTagValue}>";
                            specialCaseTagInjectionIsBefore = true;
                        }

                        if (tag == DeleteTagValue)
                        {
                            words.RemoveAt(0);

                            // following tags may be formatting tags as well, follow through
                            while (words.Count > 0 && _specialCaseClosingTags.ContainsKey(words[0]))
                            {
                                words.RemoveAt(0);
                            }
                        }
                    }
                }

                if (words.Count == 0 && specialCaseTagInjection.Length == 0)
                {
                    break;
                }
                else if (tag == DeleteTagValue && words.Count > 0 && Utils.IsTag(words[0]))
                {
                    words.RemoveAt(0);
                }

                if (specialCaseTagInjectionIsBefore)
                {
                    _content.Append(specialCaseTagInjection + String.Join("", ExtractConsecutiveWords(words, Utils.IsTag)));
                }
                else
                {
                    _content.Append(String.Join("", ExtractConsecutiveWords(words, Utils.IsTag)) + specialCaseTagInjection);
                }
            }
        }

        private string[] ExtractConsecutiveWords(List<string> words, Func<string, bool> condition)
        {
            int? indexOfFirstTag = null;

            for (int i = 0; i < words.Count; i++)
            {
                string word = words[i];

                if (InsertNonBreakingSpaces && i == 0 && word == " ")
                {
                    words[i] = "&nbsp;";
                }

                if (!condition(word))
                {
                    indexOfFirstTag = i;
                    break;
                }
            }

            if (indexOfFirstTag != null)
            {
                string[] items = words.Where((s, pos) => pos >= 0 && pos < indexOfFirstTag).ToArray();
                if (indexOfFirstTag.Value > 0)
                {
                    words.RemoveRange(0, indexOfFirstTag.Value);
                }
                return items;
            }
            else
            {
                string[] items = words.Where((s, pos) => pos >= 0 && pos <= words.Count).ToArray();
                words.RemoveRange(0, words.Count);
                return items;
            }
        }

        private List<Operation> Operations()
        {
            int positionInOld = 0, positionInNew = 0;
            var operations = new List<Operation>();

            var matches = MatchingBlocks();

            matches.Add(new Match(_oldWords.Length, _newWords.Length, 0));

            //Remove orphans from matches.
            //If distance between left and right matches is 4 times longer than length of current match then it is considered as orphan
            var mathesWithoutOrphans = RemoveOrphans(matches);

            foreach (Match match in mathesWithoutOrphans)
            {
                bool matchStartsAtCurrentPositionInOld = (positionInOld == match.StartInOld);
                bool matchStartsAtCurrentPositionInNew = (positionInNew == match.StartInNew);

                Action action;

                if (matchStartsAtCurrentPositionInOld == false
                    && matchStartsAtCurrentPositionInNew == false)
                {
                    action = Action.Replace;
                }
                else if (matchStartsAtCurrentPositionInOld
                         && matchStartsAtCurrentPositionInNew == false)
                {
                    action = Action.Insert;
                }
                else if (matchStartsAtCurrentPositionInOld == false)
                {
                    action = Action.Delete;
                }
                else // This occurs if the first few words are the same in both versions
                {
                    action = Action.None;
                }

                if (action != Action.None)
                {
                    operations.Add(
                        new Operation(action,
                            positionInOld,
                            match.StartInOld,
                            positionInNew,
                            match.StartInNew));
                }

                if (match.Size != 0)
                {
                    operations.Add(new Operation(
                        Action.Equal,
                        match.StartInOld,
                        match.EndInOld,
                        match.StartInNew,
                        match.EndInNew));
                }

                positionInOld = match.EndInOld;
                positionInNew = match.EndInNew;
            }

            return operations;
        }

        private IEnumerable<Match> RemoveOrphans(IEnumerable<Match> matches)
        {
            Match prev = null;
            Match curr = null;
            foreach (var next in matches)
            {
                if (curr == null)
                {
                    prev = new Match(0, 0, 0);
                    curr = next;
                    continue;
                }

                if (prev.EndInOld == curr.StartInOld && prev.EndInNew == curr.StartInNew
                    || curr.EndInOld == next.StartInOld && curr.EndInNew == next.StartInNew)
                //if match has no diff on the left or on the right
                {
                    yield return curr;
                    prev = curr;
                    curr = next;
                    continue;
                }

                var oldDistanceInChars = Enumerable.Range(prev.EndInOld, next.StartInOld - prev.EndInOld)
                    .Sum(i => _oldWords[i].Length);
                var newDistanceInChars = Enumerable.Range(prev.EndInNew, next.StartInNew - prev.EndInNew)
                    .Sum(i => _newWords[i].Length);
                var currMatchLengthInChars = Enumerable.Range(curr.StartInNew, curr.EndInNew - curr.StartInNew)
                    .Sum(i => _newWords[i].Length);
                if (currMatchLengthInChars > Math.Max(oldDistanceInChars, newDistanceInChars) * OrphanMatchThreshold)
                {
                    yield return curr;
                }

                prev = curr;
                curr = next;
            }

            yield return curr; //assume that the last match is always vital
        }

        private List<Match> MatchingBlocks()
        {
            var matchingBlocks = new List<Match>();
            FindMatchingBlocks(0, _oldWords.Length, 0, _newWords.Length, matchingBlocks);
            return matchingBlocks;
        }


        private void FindMatchingBlocks(
            int startInOld,
            int endInOld,
            int startInNew,
            int endInNew,
            List<Match> matchingBlocks)
        {
            Match match = FindMatch(startInOld, endInOld, startInNew, endInNew);

            if (match != null)
            {
                if (startInOld < match.StartInOld && startInNew < match.StartInNew)
                {
                    FindMatchingBlocks(startInOld, match.StartInOld, startInNew, match.StartInNew, matchingBlocks);
                }

                matchingBlocks.Add(match);

                if (match.EndInOld < endInOld && match.EndInNew < endInNew)
                {
                    FindMatchingBlocks(match.EndInOld, endInOld, match.EndInNew, endInNew, matchingBlocks);
                }
            }
        }

        private Match FindMatch(int startInOld, int endInOld, int startInNew, int endInNew)
        {
            // For large texts it is more likely that there is a Match of size bigger than maximum granularity.
            // If not then go down and try to find it with smaller granularity.
            for (int i = _matchGranularity; i > 0; i--)
            {
                var options = new MatchOptions
                {
                    BlockSize = i,
                    RepeatingWordsAccuracy = RepeatingWordsAccuracy,
                    IgnoreWhitespaceDifferences = IgnoreWhitespaceDifferences
                };
                var finder = new MatchFinder(_oldWords, _newWords, startInOld, endInOld, startInNew, endInNew, options);
                var match = finder.FindMatch();
                if (match != null)
                    return match;
            }
            return null;
        }
    }

    public class HiResDateTime
    {
        private static long lastTimeStamp = DateTime.UtcNow.Ticks;
        private static Regex digitsOnly = new Regex(@"[^\d]");
        public static long UtcNowTicks
        {
            get
            {
                long original, newValue;
                do
                {
                    original = lastTimeStamp;
                    long now = DateTime.UtcNow.Ticks;
                    newValue = Math.Max(now, original + 1);
                } while (Interlocked.CompareExchange
                             (ref lastTimeStamp, newValue, original) != original);

                return newValue;
            }
        }

        public static string UtcNowTicksString
        {
            get
            {
                return digitsOnly.Replace(UtcNowTicks.ToString(), "");
            }
        }
    }
}