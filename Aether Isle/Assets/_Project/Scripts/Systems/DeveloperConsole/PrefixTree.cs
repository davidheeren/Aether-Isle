using System.Collections.Generic;
using System;


namespace DeveloperConsole
{
    public class PrefixTree
    {
        // All keys are lowercase
        // This means you cannot have 'Car' and 'car' at the same time
        private readonly Dictionary<char, Entry> entries = new Dictionary<char, Entry>();

        public void Add(string word)
        {
            if (word == null)
                throw new ArgumentNullException(NullWordMessage);

            if (word.Length == 0)
                return;

            Entry parentEntry;
            if (!entries.TryGetValue(char.ToLower(word[0]), out parentEntry))
            {
                parentEntry = new Entry(word[0]);
                entries.Add(char.ToLower(word[0]), parentEntry);
            }

            for (int i = 1; i < word.Length; i++)
            {
                Entry childEntry;
                if (!parentEntry.children.TryGetValue(char.ToLower(word[i]), out childEntry))
                {
                    childEntry = new Entry(word[i]);
                    parentEntry.children.Add(char.ToLower(word[i]), childEntry);
                }

                parentEntry = childEntry;
            }

            parentEntry.isEndOfWord = true;
        }

        public void Remove(string word, bool caseSensitive = true)
        {
            if (word == null)
                throw new ArgumentNullException(NullWordMessage);

            if (word.Length == 0)
                throw new Exception(NonExistentWordMessage);

            Entry parentEntry;
            if (!TryGetEntryCaseSensitive(entries, word[0], caseSensitive, out parentEntry))
                throw new Exception(NonExistentWordMessage);

            Entry lastBranchEntry = null!;
            int lastBranchChildIndex = -1;

            if (parentEntry.isEndOfWord)
            {
                lastBranchEntry = parentEntry;
                lastBranchChildIndex = 1;
            }

            for (int i = 1; i < word.Length; i++)
            {
                Entry childEntry;
                if (!TryGetEntryCaseSensitive(parentEntry.children, word[i], caseSensitive, out childEntry))
                    throw new Exception(NonExistentWordMessage);

                if (i != word.Length - 1)
                {
                    if (childEntry.isEndOfWord || childEntry.children.Count > 1)
                    {
                        lastBranchEntry = childEntry;
                        lastBranchChildIndex = i + 1;
                    }
                }

                parentEntry = childEntry;
            }

            if (parentEntry.children.Count == 0)
            {
                if (lastBranchEntry == null)
                    entries.Remove(char.ToLower(word[0]));
                else
                    lastBranchEntry.children.Remove(char.ToLower(word[lastBranchChildIndex]));
            }
            else
            {
                if (!parentEntry.isEndOfWord)
                    throw new Exception(NonExistentWordMessage);

                parentEntry.isEndOfWord = false;
            }
        }

        public List<string> GetWordsFromPrefix(string prefix, bool caseSensitive = true)
        {
            if (prefix == null)
                throw new ArgumentNullException("Prefix cannot be null");

            List<string> words = new List<string>();

            // Get all words if empty string
            if (prefix.Length == 0)
            {
                foreach (Entry entry in entries.Values)
                {
                    AddToWordsRecursively(entry, "");
                }

                return words;
            }

            string startWord = "";

            // Find last entry from prefix
            if (!TryGetEntryCaseSensitive(entries, prefix[0], caseSensitive, out Entry parentEntry))
                return words;

            for (int i = 1; i < prefix.Length; i++)
            {
                if (!TryGetEntryCaseSensitive(parentEntry.children, prefix[i], caseSensitive, out Entry childEntry))
                    return words; // empty

                startWord += parentEntry.character;

                parentEntry = childEntry;
            }

            // Parent entry is the last entry in char array
            AddToWordsRecursively(parentEntry, startWord);
            return words;


            void AddToWordsRecursively(Entry entry, string word)
            {
                word += entry.character;

                if (entry.isEndOfWord)
                    words.Add(word);

                foreach (Entry c in entry.children.Values)
                {
                    AddToWordsRecursively(c, word);
                }
            }
        }

        public bool Contains(string word, bool caseSensitive = true)
        {
            if (word == null)
                throw new ArgumentNullException(NullWordMessage);

            if (word.Length == 0)
                return false;

            if (!TryGetEntryCaseSensitive(entries, word[0], caseSensitive, out Entry parentEntry))
                return false;

            for (int i = 1; i < word.Length; i++)
            {
                if (!TryGetEntryCaseSensitive(parentEntry.children, word[i], caseSensitive, out Entry childEntry))
                    return false;

                parentEntry = childEntry;
            }

            if (!parentEntry.isEndOfWord)
                return false;

            return true;
        }

        private bool TryGetEntryCaseSensitive(Dictionary<char, Entry> dict, char c, bool caseSensitive, out Entry entry)
        {
            char cl = char.ToLower(c);

            if (!dict.TryGetValue(cl, out entry))
                return false;

            if (!caseSensitive)
                return true;

            return entry.character == c;

            // If if dict is not all lower
            //if (dict.TryGetValue(c, out entry))
            //    return true;
            //else if (!caseSensitive)
            //    return dict.TryGetValue(char.IsUpper(c) ? char.ToLower(c) : char.ToUpper(c), out entry); // inverse case 
            //return false;
        }

        public void TestPrintEntries()
        {
            foreach (Entry entry in entries.Values)
            {
                PrintTree(entry);
            }

            static void PrintTree(Entry entry, string indent = "", bool isLast = true)
            {
                Console.Write(indent);
                Console.Write(isLast ? "└── " : "├── ");
                Console.Write(entry.character);
                Console.WriteLine(entry.isEndOfWord ? "*" : "");

                indent += isLast ? "    " : "│   ";

                int i = 0;
                foreach (Entry child in entry.children.Values)
                {
                    PrintTree(child, indent, i == entry.children.Count - 1);
                    i++;
                }
            }
        }

        private string NullWordMessage => "Word cannot be null";
        private string NonExistentWordMessage => "Word does not exist";

        private class Entry
        {
            public char character;
            public bool isEndOfWord;
            public Dictionary<char, Entry> children = new Dictionary<char, Entry>();

            public Entry(char character)
            {
                this.character = character;
            }
        }
    }
}
