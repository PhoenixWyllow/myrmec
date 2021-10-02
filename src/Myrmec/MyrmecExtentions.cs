using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Myrmec
{
	/// <summary>
	/// MyrmecExtentions
	/// </summary>
	public static class MyrmecExtentions
	{
		/// <summary>
		/// Add metadata into metadata list.
		/// </summary>
		/// <param name="list">The target list.</param>
		/// <param name="record">The metadata record need to add.</param>
		public static void Add(this List<Metadata> list, Record record)
		{
			var metadata = new Metadata()
			{
				Extentions = record.Extentions.Split(',', ' ').ToList()
			};
			string hex = record.Hex;
			if (record.Offset > 0)
			{
				hex = Repeat("??", record.Offset, ',') + hex;
			}
			string[] byteStringArray = hex.Split(',', ' ');

			bool lastCharIsQuestionMark = true;

			int start = 0;
			for (int i = 0; i < byteStringArray.Length + 1; i++)
			{
				int count;
				if (i == byteStringArray.Length)
				{
					if (!lastCharIsQuestionMark)
					{
						count = i - start;
						metadata.Offsets.Add(MakeOffset(byteStringArray, start, count));
					}
					break;
				}

				if (byteStringArray[i] == "??")
				{
					if (!lastCharIsQuestionMark)
					{
						count = i - start;
						metadata.Offsets.Add(MakeOffset(byteStringArray, start, count));
					}
					lastCharIsQuestionMark = true;
				}
				else
				{
					if (lastCharIsQuestionMark)
					{
						start = i;
					}
					lastCharIsQuestionMark = false;
				}
			}

			list.Add(metadata);
		}

		/// <summary>
		/// Get byte array from string.
		/// </summary>
		/// <param name="source">byte format string.</param>
		/// <returns>result byte array.</returns>
		public static byte[] GetBytes(this string source)
		{
			string[] array = source.Split(',', ' ');
			byte[] byteArr = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				byteArr[i] = Convert.ToByte(array[i], 16);
			}

			return byteArr;
		}

		/// <summary>
		/// Match result from complex metadata.
		/// </summary>
		/// <param name="list">The complex metadata list.</param>
		/// <param name="data">Data to be match.</param>
		/// <param name="matchAll">Match all result or only first that sniffer matched.</param>
		/// <returns>Match result list.</returns>
		public static List<string> Match(this List<Metadata> list, byte[] data, bool matchAll = false)
		{
			List<string> extentionStore = new List<string>(4);
			foreach (var metatata in list)
			{
				if (metatata.Match(data))
				{
					extentionStore.AddRange(metatata.Extentions);
					if (!matchAll)
					{
						break;
					}
				}
			}
			return extentionStore;
		}

		/// <summary>
		/// Populate matadata tree use record list.
		/// </summary>
		/// <param name="sniffer"></param>
		/// <param name="records">Matadate record list.</param>
		public static void Populate(this Sniffer sniffer, IList<Record> records)
		{
			foreach (var record in records)
			{
				sniffer.Add(record);
			}
			sniffer.SuggestedMaxBufferSize = records.Max(h => h.Hex.Length);
		}

		private static Offset MakeOffset(string[] byteStringArray, int start, int count)
		{
			return new Offset
			{
				Start = start,
				Count = count,
				Value = Encoding.ASCII.GetString(string.Join(",", byteStringArray, start, count).GetBytes())
			};
		}

		[DebuggerStepThrough]
		private static string Repeat(string source, int count, char seprator)
		{
			var sb = new StringBuilder(count);
			for (int i = 0; i < count; i++)
			{
				sb.Append(source).Append(seprator);
			}

			return sb.ToString();
		}
	}
}