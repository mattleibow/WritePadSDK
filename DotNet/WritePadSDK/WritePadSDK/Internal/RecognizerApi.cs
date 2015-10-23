/* ************************************************************************************* */
/* *    PhatWare WritePad SDK                                                          * */
/* *    Copyright (c) 2008-2015 PhatWare(r) Corp. All rights reserved.                 * */
/* ************************************************************************************* */

/* ************************************************************************************* *
 *
 * Unauthorized distribution of this code is prohibited. For more information
 * refer to the End User Software License Agreement provided with this
 * software.
 *
 * This source code is distributed and supported by PhatWare Corp.
 * http://www.phatware.com
 *
 * THIS SAMPLE CODE CAN BE USED  AS A REFERENCE AND, IN ITS BINARY FORM,
 * IN THE USER'S PROJECT WHICH IS INTEGRATED WITH THE WRITEPAD SDK.
 * ANY OTHER USE OF THIS CODE IS PROHIBITED.
 *
 * THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU "AS-IS"
 * AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR OTHERWISE,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE. IN NO EVENT SHALL PHATWARE CORP.
 * BE LIABLE TO YOU OR ANYONE ELSE FOR ANY DIRECT, SPECIAL, INCIDENTAL,
 * INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY KIND, OR ANY DAMAGES WHATSOEVER,
 * INCLUDING WITHOUT LIMITATION, LOSS OF PROFIT, LOSS OF USE, SAVINGS
 * OR REVENUE, OR THE CLAIMS OF THIRD PARTIES, WHETHER OR NOT PHATWARE CORP.
 * HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH LOSS, HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE
 * POSSESSION, USE OR PERFORMANCE OF THIS SOFTWARE.
 *
 * US Government Users Restricted Rights
 * Use, duplication, or disclosure by the Government is subject to
 * restrictions set forth in EULA and in FAR 52.227.19(c)(2) or subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013 and/or in similar or successor
 * clauses in the FAR or the DOD or NASA FAR Supplement.
 * Unpublished-- rights reserved under the copyright laws of the
 * United States.  Contractor/manufacturer is PhatWare Corp.
 * 1314 S. Grand Blvd. Ste. 2-175 Spokane, WA 99202
 *
 * ************************************************************************************* */

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace PhatWare.WritePad
{
    internal class RecognizerApi
    {
        //typedef int (RECO_ONGOTWORDLIST)( IntPtr szWordFrom, IntPtr szWordTo, unsigned int nFlags, IntPtr pParam );
        //typedef RECO_ONGOTWORDLIST * PRECO_ONGOTWORDLIST;


        // recognizer library language ID

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetLanguageID", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern Language HWR_GetLanguageID(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetLanguageName", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_GetLanguageName(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_IsLanguageSupported", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_IsLanguageSupported(Language langID);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetSupportedLanguages", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetSupportedLanguages(out IntPtr languages);


        // recognition API

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_InitRecognizer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_InitRecognizer(string inDictionaryMain, string inDictionaryCustom, string inLearner, string inAutoCorrect, Language language, out RecognitionFlags pFlags);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_InitRecognizerFromMemory", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_InitRecognizerFromMemory(string inDictionaryMain, string inDictionaryCustom, string inLearner, string inAutoCorrect, Language language, out RecognitionFlags pFlags);


        [DllImport(Constants.LibraryName, EntryPoint = "HWR_FreeRecognizer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void HWR_FreeRecognizer(IntPtr pRecognizer, string inDictionaryCustom, string inLearner, string inWordList);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_RecognizerAddStroke", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_RecognizerAddStroke(IntPtr pRecognizer, InkTracePoint[] pStroke, int nStrokeCnt);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_Recognize", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_Recognize(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_Reset", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_Reset(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_RecognizeInkData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_RecognizeInkData(IntPtr pRecognizer, IntPtr pInkData, int nFirstStroke, int nLastStroke, bool bAsync, bool bFlipY, bool bSort, bool bSelOnly);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_PreRecognizeInkData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_PreRecognizeInkData(IntPtr pRecognizer, IntPtr pInkData, int nDataLen, bool bFlipY);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResult", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_GetResult(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResultWeight", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern ushort HWR_GetResultWeight(IntPtr pRecognizer, int nWord, int nAlternative);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResultWord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_GetResultWord(IntPtr pRecognizer, int nWord, int nAlternative);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResultStrokesNumber", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetResultStrokesNumber(IntPtr pRecognizer, int nWord, int nAlternative);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResultWordCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetResultWordCount(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetResultAlternativeCount", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetResultAlternativeCount(IntPtr pRecognizer, int nWord);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetRecognitionMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RecognitionMode HWR_SetRecognitionMode(IntPtr pRecognizer, RecognitionMode nNewMode);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetRecognitionMode", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RecognitionMode HWR_GetRecognitionMode(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetStrokeIDs", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetStrokeIDs(IntPtr pRecognizer, int word, int altrnative, out IntPtr strokes);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetRecognitionFlags", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RecognitionFlags HWR_SetRecognitionFlags(IntPtr pRecognizer, RecognitionFlags newFlags);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetRecognitionFlags", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern RecognitionFlags HWR_GetRecognitionFlags(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_StopAsyncReco", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void HWR_StopAsyncReco(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetCustomCharset", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void HWR_SetCustomCharset(IntPtr pRecognizer, IntPtr pCustomNum, IntPtr pCustPunct);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_RecognizeSymbol", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_RecognizeSymbol(IntPtr pRecognizer, IntPtr pInkData, int nbase, int charsize);


        // simple calculator functions

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_EnablePhatCalc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_EnablePhatCalc(IntPtr pRecognizer, bool bEnable);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_CalculateString", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_CalculateString(IntPtr pRecognizer, IntPtr pszString);


        // autocorrector functions

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SaveWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SaveWordList(IntPtr pRecognizer, string inWordListFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_EnumWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_EnumWordList(IntPtr pRecognizer, /*TODO: RECO_ONGOTWORDLIST*/IntPtr callback, IntPtr pParam);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_EmptyWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_EmptyWordList(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_AddWordToWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_AddWordToWordList(IntPtr pRecognizer, IntPtr pszWord1, IntPtr pszWord2, int dwFlags, bool bReplace);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ResetAutoCorrector", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ResetAutoCorrector(IntPtr pRecognizer, string inWordListFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_AutocorrectWord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_AutocorrectWord(IntPtr pRecognizer, IntPtr pszWord);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ReloadAutoCorrector", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ReloadAutoCorrector(IntPtr pRecognizer, string inWordListFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ImportWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ImportWordList(IntPtr pRecognizer, string inImportFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ExportWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ExportWordList(IntPtr pRecognizer, string inExportFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetAutocorrectorData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetAutocorrectorData(IntPtr pRecognizer, out IntPtr ppData);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetAutocorrectorData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetAutocorrectorData(IntPtr pRecognizer, byte[] pData);


        // learner functions

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ResetLearner", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ResetLearner(IntPtr pRecognizer, string inLearnerFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_AnalyzeWordList", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_AnalyzeWordList(IntPtr pRecognizer, IntPtr pszWordList, out IntPtr pszResult);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_LearnNewWord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_LearnNewWord(IntPtr pRecognizer, IntPtr pszWord, ushort nWeight);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ReplaceWord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ReplaceWord(IntPtr pRecognizer, IntPtr pszWord1, ushort nWeight1, IntPtr pszWord2, ushort nWeight2);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ReloadLearner", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ReloadLearner(IntPtr pRecognizer, string inDictionaryCustom);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SaveLearner", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SaveLearner(IntPtr pRecognizer, string pszFileName);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetLearnerData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetLearnerData(IntPtr pRecognizer, out IntPtr ppData);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetLearnerData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetLearnerData(IntPtr pRecognizer, byte[] pData);


        // dictionary functions

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_WordFlipCase", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_WordFlipCase(IntPtr pRecognizer, IntPtr pszWord);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_WordEnsureLowerCase", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_WordEnsureLowerCase(IntPtr pRecognizer, IntPtr pszWord);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_EnumUserWords", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_EnumUserWords(IntPtr pRecognizer, /*TODO: PRECO_ONGOTWORD*/IntPtr callback, IntPtr pParam);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_NewUserDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_NewUserDict(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SaveUserDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SaveUserDict(IntPtr pRecognizer, string inDictionaryCustom);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_IsWordInDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_IsWordInDict(IntPtr pRecognizer, IntPtr pszWord);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_AddUserWordToDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_AddUserWordToDict(IntPtr pRecognizer, IntPtr pszWord, bool filter);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SpellCheckWord", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_SpellCheckWord(IntPtr pRecognizer, IntPtr pszWord, out IntPtr pszAnswer, int cbSize, int flags);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetDictionaryData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetDictionaryData(IntPtr pRecognizer, byte[] pData, DictionaryType nDictType);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetDictionaryData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetDictionaryData(IntPtr pRecognizer, out IntPtr ppData, DictionaryType nDictType);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_HasDictionaryChanged", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_HasDictionaryChanged(IntPtr pRecognizer, DictionaryType nDictType);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetDictionaryLength", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int HWR_GetDictionaryLength(IntPtr pRecognizer, DictionaryType nDictType);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ResetUserDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ResetUserDict(IntPtr pRecognizer, string inDictionaryCustom);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ExportUserDictionary", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ExportUserDictionary(IntPtr pRecognizer, string inExportFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ImportUserDictionary", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ImportUserDictionary(IntPtr pRecognizer, string inImportFile);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_ReloadUserDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_ReloadUserDict(IntPtr pRecognizer, string inDictionaryCustom);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_LoadAlternativeDict", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_LoadAlternativeDict(IntPtr pRecognizer, string inDictionaryAlt);


        // letter shapes (added in version 5)

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetDefaultShapes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetDefaultShapes(IntPtr pRecognizer);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetLetterShapes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetLetterShapes(IntPtr pRecognizer, IntPtr pShapes);

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_GetLetterShapes", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr HWR_GetLetterShapes(IntPtr pRecognizer);


        // external resource set function

        [DllImport(Constants.LibraryName, EntryPoint = "HWR_SetExternalResource", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool HWR_SetExternalResource(Language lang, IntPtr data);


        // managed wrappers for IntPtr values

        internal static Language[] HWR_GetSupportedLanguages_Managed()
        {
            IntPtr languagesPtr;
            var size = HWR_GetSupportedLanguages(out languagesPtr);
            int[] languages = new int[size];
            Marshal.Copy(languagesPtr, languages, 0, size);
            // TODO: Marshal.FreeHGlobal(languagesPtr);
            return languages.Select(l => (Language)l).ToArray();
        }

        internal static string HWR_GetLanguageName_Managed(IntPtr pRecognizer)
        {
            var resultPtr = HWR_GetLanguageName(pRecognizer);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static string HWR_RecognizeInkData_Managed(IntPtr pRecognizer, IntPtr pInkData, int nFirstStroke, int nLastStroke, bool bAsync, bool bFlipY, bool bSort, bool bSelOnly)
        {
            var resultPtr = HWR_RecognizeInkData(pRecognizer, pInkData, nFirstStroke, nLastStroke, bAsync, bFlipY, bSort, bSelOnly);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static string HWR_GetResult_Managed(IntPtr pRecognizer)
        {
            var resultPtr = HWR_GetResult(pRecognizer);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static string HWR_GetResultWord_Managed(IntPtr pRecognizer, int column, int row)
        {
            var resultPtr = HWR_GetResultWord(pRecognizer, column, row);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static int[] HWR_GetStrokeIDs_Managed(IntPtr pRecognizer, int word, int altrnative)
        {
            IntPtr strokesPtr;
            var size = HWR_GetStrokeIDs(pRecognizer, word, altrnative, out strokesPtr);
            int[] strokes = new int[size];
            Marshal.Copy(strokesPtr, strokes, 0, size);
            // TODO: Marshal.FreeHGlobal(strokesPtr);
            return strokes;
        }

        internal static void HWR_SetCustomCharset_Managed(IntPtr pRecognizer, string pCustomNum, string pCustPunct)
        {
            var pCustomNumPtr = Marshal.StringToHGlobalUni(pCustomNum);
            var pCustPunctPtr = Marshal.StringToHGlobalUni(pCustPunct);
            HWR_SetCustomCharset(pRecognizer, pCustomNumPtr, pCustPunctPtr);
            Marshal.FreeHGlobal(pCustomNumPtr);
            Marshal.FreeHGlobal(pCustPunctPtr);
        }

        internal static string HWR_CalculateString_Managed(IntPtr pRecognizer, string pszString)
        {
            var pszStringPtr = Marshal.StringToHGlobalUni(pszString);
            var resultPtr = HWR_CalculateString(pRecognizer, pszStringPtr);
            Marshal.FreeHGlobal(pszStringPtr);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static bool HWR_AddWordToWordList_Managed(IntPtr pRecognizer, string pszWord1, string pszWord2, int dwFlags, bool bReplace)
        {
            var pszWord1Ptr = Marshal.StringToHGlobalUni(pszWord1);
            var pszWord2Ptr = Marshal.StringToHGlobalUni(pszWord2);
            var result = HWR_AddWordToWordList(pRecognizer, pszWord1Ptr, pszWord2Ptr, dwFlags, bReplace);
            Marshal.FreeHGlobal(pszWord1Ptr);
            Marshal.FreeHGlobal(pszWord2Ptr);
            return result;
        }

        internal static string HWR_AutocorrectWord_Managed(IntPtr pRecognizer, string pszWord)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var resultPtr = HWR_AutocorrectWord(pRecognizer, pszWordPtr);
            Marshal.FreeHGlobal(pszWordPtr);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static byte[] HWR_GetAutocorrectorData_Managed(IntPtr pRecognizer)
        {
            IntPtr ppDataPtr;
            var size = HWR_GetAutocorrectorData(pRecognizer, out ppDataPtr);
            if (size == -1)
            {
                // TODO (possibly): Marshal.FreeHGlobal(ppDataPtr);
                return null;
            }
            else if (size == 0)
            {
                // TODO (possibly: Marshal.FreeHGlobal(ppDataPtr);
                return new byte[0];
            }
            byte[] ppData = new byte[size];
            Marshal.Copy(ppDataPtr, ppData, 0, size);
            // TODO: Marshal.FreeHGlobal(ppDataPtr);
            return ppData;
        }

        internal static bool HWR_AnalyzeWordList_Managed(IntPtr pRecognizer, string pszWordList, out string pszResult)
        {
            var pszWordListPtr = Marshal.StringToHGlobalUni(pszWordList);
            IntPtr pszResultPtr;
            var result = HWR_AnalyzeWordList(pRecognizer, pszWordListPtr, out pszResultPtr);
            pszResult = Marshal.PtrToStringUni(pszResultPtr);
            // TODO: Marshal.FreeHGlobal(pszResultPtr);
            Marshal.FreeHGlobal(pszWordListPtr);
            return result;
        }

        internal static bool HWR_LearnNewWord_Managed(IntPtr pRecognizer, string pszWord, ushort nWeight)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var result = HWR_LearnNewWord(pRecognizer, pszWordPtr, nWeight);
            Marshal.FreeHGlobal(pszWordPtr);
            return result;
        }

        internal static bool HWR_ReplaceWord_Managed(IntPtr pRecognizer, string pszWord1, ushort nWeight1, string pszWord2, ushort nWeight2)
        {
            var pszWord1Ptr = Marshal.StringToHGlobalUni(pszWord1);
            var pszWord2Ptr = Marshal.StringToHGlobalUni(pszWord2);
            var result = HWR_ReplaceWord(pRecognizer, pszWord1Ptr, nWeight1, pszWord2Ptr, nWeight2);
            Marshal.FreeHGlobal(pszWord1Ptr);
            Marshal.FreeHGlobal(pszWord2Ptr);
            return result;
        }

        internal static byte[] HWR_GetLearnerData_Managed(IntPtr pRecognizer)
        {
            IntPtr ppDataPtr;
            var size = HWR_GetLearnerData(pRecognizer, out ppDataPtr);
            if (size == -1)
            {
                // TODO (possibly): Marshal.FreeHGlobal(ppDataPtr);
                return null;
            }
            else if (size == 0)
            {
                // TODO (possibly: Marshal.FreeHGlobal(ppDataPtr);
                return new byte[0];
            }
            byte[] ppData = new byte[size];
            Marshal.Copy(ppDataPtr, ppData, 0, size);
            // TODO: Marshal.FreeHGlobal(ppDataPtr);
            return ppData;
        }

        internal static string HWR_WordFlipCase_Managed(IntPtr pRecognizer, string pszWord)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var resultPtr = HWR_WordFlipCase(pRecognizer, pszWordPtr);
            Marshal.FreeHGlobal(pszWordPtr);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static string HWR_WordEnsureLowerCase_Managed(IntPtr pRecognizer, string pszWord)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var resultPtr = HWR_WordEnsureLowerCase(pRecognizer, pszWordPtr);
            Marshal.FreeHGlobal(pszWordPtr);
            var result = Marshal.PtrToStringUni(resultPtr);
            // TODO: free resultPtr
            return result;
        }

        internal static bool HWR_IsWordInDict_Managed(IntPtr pRecognizer, string pszWord)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var result = HWR_IsWordInDict(pRecognizer, pszWordPtr);
            Marshal.FreeHGlobal(pszWordPtr);
            return result;
        }

        internal static bool HWR_AddUserWordToDict_Managed(IntPtr pRecognizer, string pszWord, bool filter)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            var result = HWR_AddUserWordToDict(pRecognizer, pszWordPtr, filter);
            Marshal.FreeHGlobal(pszWordPtr);
            return result;
        }

        internal static int HWR_SpellCheckWord_Managed(IntPtr pRecognizer, string pszWord, out string pszAnswer, int cbSize, int flags)
        {
            var pszWordPtr = Marshal.StringToHGlobalUni(pszWord);
            IntPtr pszAnswerPtr;
            var result = HWR_SpellCheckWord(pRecognizer, pszWordPtr, out pszAnswerPtr, cbSize, flags);
            pszAnswer = Marshal.PtrToStringUni(pszAnswerPtr);
            Marshal.FreeHGlobal(pszWordPtr);
            // TODO: Marshal.FreeHGlobal(pszAnswerPtr);
            return result;
        }

        internal static byte[] HWR_GetDictionaryData_Managed(IntPtr pRecognizer, DictionaryType nDictType)
        {
            IntPtr ppDataPtr;
            var size = HWR_GetDictionaryData(pRecognizer, out ppDataPtr, nDictType);
            if (size == -1)
            {
                // TODO (possibly): Marshal.FreeHGlobal(ppDataPtr);
                return null;
            }
            else if (size == 0)
            {
                // TODO (possibly: Marshal.FreeHGlobal(ppDataPtr);
                return new byte[0];
            }
            byte[] ppData = new byte[size];
            Marshal.Copy(ppDataPtr, ppData, 0, size);
            // TODO: Marshal.FreeHGlobal(ppDataPtr);
            return ppData;
        }
    }
}
