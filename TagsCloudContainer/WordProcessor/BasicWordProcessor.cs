﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHunspell;

namespace TagsCloudContainer.WordProcessor
{
    public class BasicWordProcessor : IWordProcessor
    {
        public IEnumerable<WordWithCount> ProcessWords(IEnumerable<string> words)
        {
            var lowerWords = words.Select(w => w.ToLower());
            var directoryInfo = DirectoryMethods.GetProjectDirectoryInfo();
            var directoryName = $@"{directoryInfo.FullName}\Hunspell";
            var hunspell = new Hunspell($@"{directoryName}\ru.aff", $@"{directoryName}\ru.dic");
            var processedWords = new List<string>();
            //Убрать скучные слова
            foreach (var word in lowerWords)
            {
                var stemResult = hunspell.Stem(word);
                processedWords.Add(stemResult.Count > 0 ? stemResult[0] : word);
            }
            return GetWordsWithCount(processedWords);
        }

        private IEnumerable<WordWithCount> GetWordsWithCount(IEnumerable<string> words)
        {
            var frequencyDictionary = new Dictionary<string, int>();
            foreach (var word in words)
            {
                if (!frequencyDictionary.ContainsKey(word))
                    frequencyDictionary[word] = 0;
                frequencyDictionary[word]++;
            }

            return frequencyDictionary
                .OrderByDescending(p => p.Value)
                .Select(p => new WordWithCount(p.Key, p.Value));
        }
    }
}