using System;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace PigLatin
{
	class MainClass
	{
		private static string[] Map(string sentence)
		{
			return sentence.Split(' ');
		}
		public static string[] Process(string[] words)
		{
			for (int i = 0; i < words.Length; i++)
			{
				int index = i;
				Task.Factory.StartNew(
					() => words[index] = LowerString(words[index]),
					TaskCreationOptions.AttachedToParent | TaskCreationOptions.LongRunning);
			}
			return words;
		}
		private static string LowerString(string s)
		{
			return s.ToLower();
		}
		private static string Reduce(string[] words)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string word in words)
			{
				string original = word;
				if (word.Length > 0)
				{
					sb.Append(word.Substring(1));
					sb.Append(original.Substring(0, 1));
					sb.Append("ay");
					sb.Append(' ');
				}

			}
			return sb.ToString().Trim();
		}
		// TODO: complete this method - return the sentence as pig latin
		public static string PigLatin(string sentence)
		{
			if (sentence == null)
				return null;
			if (sentence == "")
				return "";
			var task = Task<string[]>.Factory.StartNew(() => Map(sentence))
				.ContinueWith<string[]>(t => Process(t.Result))
					.ContinueWith<string>(t => Reduce(t.Result));

			// display result
			return task.Result;
		}

		public static void Main (string[] args)
		{
			Console.WriteLine("Enter a sentence");
			string sentence = Console.ReadLine();

			// set up map-reduce process
			var task = Task<string[]>.Factory.StartNew (() => Map (sentence))
				.ContinueWith<string[]> (t => Process (t.Result))
					.ContinueWith<string> (t => Reduce (t.Result));

			// display result
			Console.WriteLine ("Result: {0}", task.Result);
			Console.WriteLine("Hit enter to close");
			Console.ReadLine();
		}
	}
}
