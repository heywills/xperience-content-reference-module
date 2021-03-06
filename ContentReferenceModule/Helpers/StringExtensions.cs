using System;
using System.Collections.Generic;

namespace XperienceCommunity.ContentReferenceModule.Helpers
{
    public static class StringExtensions
    {
		/// <summary>
		/// Takes a string and returns a list of GUIDs using the split character provided. Only valid guids will return. 
		/// Will return null if the string does not contain any valid GUIDs. 
		/// To support legacy behavior if the split character is ";" then it will match both "," and ";" if you do not
		/// want this behavior use the override List(Guid) Guidify(this string value, params char[] separator)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="splitCharacter"></param>
		/// <returns>An emptry List(Guid) if none are found.</returns>
		public static IEnumerable<Guid> Guidify(this string value, string splitCharacter = ";")
		{
			var separator = new[] { char.Parse(splitCharacter) };

			// To support legacy behavior if the split char is ";" then we also match ","
			if (splitCharacter == ";")
			{
				separator = new[] { ',', ';' };
			}

			return value.Guidify(separator);
		}

		/// <summary>
		/// Takes a string and returns a list of GUIDs using the split character provided. Only valid guids will return. 
		/// Will return null if the string does not contain any valid GUIDs.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="separator">List of separator character to split the string on</param>
		/// <returns>NULL if no guids are present or a list of guids.</returns>
		public static IEnumerable<Guid> Guidify(this string value, params char[] separator)
		{
			var list = new List<Guid>(); // no checking for nulls
            if (string.IsNullOrWhiteSpace(value))
            {
                return list;
            }
            var guidList = value.SplitStringAndTrim(separator);
            if (guidList == null)
            {
                return list;
            }
            foreach (var g in guidList)
            {
                if (Guid.TryParse(g, out var guid))
                {
                    list.Add(guid);
                }
            }
            return list;
        }

		public static IEnumerable<string> SplitStringAndTrim(this string helper, params char[] seperator)
		{
            //Get string coll of items
            string[] values = helper.Split(seperator);
            var list = new List<string>();
			foreach (var s in values)
			{
				var thisVal = s.Trim();
				if (!string.IsNullOrWhiteSpace(thisVal))
				{
					list.Add(thisVal);
				}
			}
			return list;
		}
	}
}
