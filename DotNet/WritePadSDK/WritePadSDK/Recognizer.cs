using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PhatWare.WritePad
{
    [Flags]
    public enum RecognitionFlags : uint
    {
        Default = 0,

        SEPLET = 0x00000001,
        USERDICT = 0x00000002,
        MAINDICT = 0x00000004,
        ONLYDICT = 0x00000008,
        STATICSEGMENT = 0x00000010,
        SINGLEWORDONLY = 0x00000020,
        INTERNATIONAL = 0x00000040,
        SUGGESTONLYDICT = 0x00000080,
        ANALYZER = 0x00000100,
        CORRECTOR = 0x00000200,
        SPELLIGNORENUM = 0x00000400,
        SPELLIGNOREUPPER = 0x00000800,
        NOSINGLELETSPACE = 0x00001000,
        ENABLECALC = 0x00002000,
        NOSPACE = 0x00004000,
        ALTDICT = 0x00008000,
        USECUSTOMPUNCT = 0x00010000,
        SMOOTHSTROKES = 0x00020000,

        FLAG_ERROR = 0xFFFFFFFF
    }

    public enum DictionaryType
    {
        Main = 0,
        Alternative,
        User
    }

    public enum RecognitionMode
    {
        INVALID = -1,

        /// <summary>
        /// Normal recognition -- all symbols allowed
        /// </summary>
        GENERAL = 0,

        /// <summary>
        /// All recognized text converted to capitals
        /// </summary>
        CAPS = 1,

        /// <summary>
        /// Numeric and Lex DB recognition mode
        /// </summary>
        NUM = 2,

        /// <summary>
        /// internet address mode
        /// </summary>
        WWW = 3,

        /// <summary>
        /// pure numeric mode, no alpha or punctuation, recognizes 0123456789 only
        /// </summary>
        NUMBERSPURE = 4,

        /// <summary>
        /// custom charset for numbers and punctuation, no alpha
        /// </summary>
        CUSTOM = 5,

        /// <summary>
        /// Alpha characters only, no punctuation or numbers
        /// </summary>
        ALPHAONLY = 6
    }
    public class Recognizer
    {
        private static readonly Assembly CurrentAssembly = Assembly.GetAssembly(typeof(Recognizer));

        public const string EmbeddedDictionariesPrefix = "PhatWare.WritePad.Dictionaries";

        public const string UserShortcutFile = "usershortcuts.csv";
        public const string DictionaryFileExtension = "dct";
        public const string EmptyWordString = "<--->";

        private readonly IntPtr nativeHandle;

        private readonly string mainDict;
        private readonly string userDict;
        private readonly string learner;
        private readonly string corrector;

        private bool disposed = false;

        public Recognizer(Language language)
            : this(language, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
        {
        }

        public Recognizer(Language language, string userDictionaryRoot)
            : this(language, null, userDictionaryRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Recognizer"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="mainDictionaryPath">The main dictionary path.</param>
        /// <param name="userDictionaryRoot">The user dictionary root.</param>
        public Recognizer(Language language, string mainDictionaryPath, string userDictionaryRoot)
        {
            var languageName = GetDictionaryName(language);

            // set the main dictionary
            mainDict = mainDictionaryPath;

            // get user dictionary paths
            userDict = "WritePad_User_" + languageName + ".dct";
            learner = "WritePad_Stat_" + languageName + ".lrn";
            corrector = "WritePad_Corr_" + languageName + ".cwl";

            // add any user prefix
            if (userDictionaryRoot != null)
            {
                userDict = Path.Combine(userDictionaryRoot, userDict);
                learner = Path.Combine(userDictionaryRoot, learner);
                corrector = Path.Combine(userDictionaryRoot, corrector);
            }

            // create the recognizer instance
            RecognitionFlags flags;
            nativeHandle = RecognizerApi.HWR_InitRecognizer(mainDict, userDict, learner, corrector, language, out flags);

            // load embedded main dictionary if none was specified
            if (mainDictionaryPath == null)
            {
                var resource = string.Format("{0}.{1}.{2}", EmbeddedDictionariesPrefix, languageName, DictionaryFileExtension);
                using (var dictionary = CurrentAssembly.GetManifestResourceStream(resource))
                using (var memory = new MemoryStream())
                {
                    dictionary.CopyTo(memory);
                    RecognizerApi.HWR_SetDictionaryData(nativeHandle, memory.ToArray(), DictionaryType.Main);
                }
            }
        }

        public Recognizer(Language language, string mainDictionaryPath, string userDictionaryPath, string learnerPath, string autoCorrectPath)
        {
            mainDict = mainDictionaryPath;
            userDict = userDictionaryPath;
            learner = learnerPath;
            corrector = autoCorrectPath;

            // create the recognizer
            RecognitionFlags flags;
            nativeHandle = RecognizerApi.HWR_InitRecognizer(mainDictionaryPath, userDictionaryPath, learnerPath, autoCorrectPath, language, out flags);
        }

        public Language Language
        {
            get { return RecognizerApi.HWR_GetLanguageID(nativeHandle); }
        }

        public string LanguageName
        {
            get { return RecognizerApi.HWR_GetLanguageName_Managed(nativeHandle); }
        }

        public static bool IsLanguageSupported(Language language)
        {
            return RecognizerApi.HWR_IsLanguageSupported(language);
        }

        public static Language[] SupportedLanguages
        {
            get { return RecognizerApi.HWR_GetSupportedLanguages_Managed(); }
        }

        public int GetResultWordCount()
        {
            return RecognizerApi.HWR_GetResultWordCount(nativeHandle);
        }

        public int GetResultAlternativeCount(int word)
        {
            return RecognizerApi.HWR_GetResultAlternativeCount(nativeHandle, word);
        }

        public bool LearnNewWord(string word, int weight)
        {
            return RecognizerApi.HWR_LearnNewWord_Managed(nativeHandle, word, (ushort)weight);
        }

        public string GetResultWord(int column, int row)
        {
            return RecognizerApi.HWR_GetResultWord_Managed(nativeHandle, column, row);
        }

        public int GetResultWeight(int word, int alternative)
        {
            return RecognizerApi.HWR_GetResultWeight(nativeHandle, word, alternative);
        }

        public string RecognizeInkData(Ink ink)
        {
            return RecognizeInkData(ink, 0, ink.StrokeCount, false, false, false, false);
        }

        public string RecognizeInkData(Ink ink, int firstStroke, int lastStroke)
        {
            return RecognizeInkData(ink, firstStroke, lastStroke, false, false, false, false);
        }

        public string RecognizeInkData(Ink ink, bool async, bool flipY, bool sort, bool selOnly)
        {
            return RecognizeInkData(ink, 0, ink.StrokeCount, async, flipY, sort, selOnly);
        }

        public string RecognizeInkData(Ink ink, int firstStroke, int lastStroke, bool async, bool flipY, bool sort, bool selOnly)
        {
            return RecognizerApi.HWR_RecognizeInkData_Managed(
                nativeHandle, ink.nativeHandle, firstStroke, lastStroke, async, flipY, sort, selOnly);
        }

        public RecognitionFlags RecognitionFlags
        {
            get { return RecognizerApi.HWR_GetRecognitionFlags(nativeHandle); }
            set { RecognizerApi.HWR_SetRecognitionFlags(nativeHandle, value); }
        }

        ~Recognizer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (nativeHandle != null && !disposed)
            {
                RecognizerApi.HWR_FreeRecognizer(nativeHandle, userDict, learner, corrector);

                disposed = true;
            }
        }

        private static string GetDictionaryName(Language language)
        {
            switch (language)
            {
                default:
                case Language.English:
                    return "English";
                case Language.EnglishUK:
                    return "EnglishUK";
                case Language.German:
                    return "German";
                case Language.French:
                    return "French";
                case Language.Italian:
                    return "Italian";
                case Language.Spanish:
                    return "Spanish";
                case Language.Swedish:
                    return "Swedish";
                case Language.Norwegian:
                    return "Norwegian";
                case Language.Dutch:
                    return "Dutch";
                case Language.Danish:
                    return "Danish";
                case Language.Portuguese:
                    return "Portuguese";
                case Language.PortugueseBrazilian:
                    return "Brazilian";
                case Language.Finnish:
                    return "Finnish";
                case Language.Indonesian:
                    return "Indonesian";
            }
        }
    }
}
