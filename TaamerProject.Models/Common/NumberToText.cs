using System; 
using Microsoft.VisualBasic;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace TaamerProject.Models.Common
{
	/// <summary>
	/// Class for converting integers (up to 15 digits) to
	/// the equivalent text in both English and Arabic.
	/// Created by: Dia Zeki Al-Azzawi
	/// Date: July 31, 2005
	/// </summary>
	public class NumberToText 
	{

		#region Fields
		//Fields for Arabic Numbers
		private string[] onesA = new string[11];
		private string[] TensA = new string[11];
		private string[] HunderdsA = new string[11];
		private string[] ThousandsA = new string[11];
		private string[] MillionsA = new string[11];
		private string[] MilliarsA = new string[11];
		private string[] TrillionsA = new string[11];
		private string TeensA;
		//Fields for English Numbers
		private string[] ones = new string[11];
		private string[] Teens = new string[11];
		private string[] Tens = new string[11];
		private string[] Thousands = new string[6];
		#endregion 

		#region Constructor and Initialization
		/// <summary>
		/// Class Constructor
		/// </summary>
		public NumberToText() 
		{
			InitNumbers();
		}
		private void InitNumbers() 
		{
			//English Numbers
			ones[0] = "صفر";
			ones[1] = "واحد";
			ones[2] = "اثنان";
			ones[3] = "ثلاثة";
			ones[4] = "أربعة";
			ones[5] = "خمسة";
			ones[6] = "ستة";
			ones[7] = "سبعة";
			ones[8] = "ثمانية";
			ones[9] = "تسعة";
			Teens[0] = "عشرة";
			Teens[1] = "أحد عشر";
			Teens[2] = "اثنا عشر";
			Teens[3] = "ثلاثة عشر";
			Teens[4] = "أربعة عشر";
			Teens[5] = "خمسة عشر";
			Teens[6] = "ستة عشر";
			Teens[7] = "سبعة عشر";
			Teens[8] = "ثمانية عشر";
			Teens[9] = "تسعة عشر";
			Tens[0] = "";
			Tens[1] = "عشرة";
			Tens[2] = "عشرون";
			Tens[3] = "ثلاثون";
			Tens[4] = "أربعون";
			Tens[5] = "خمسون";
			Tens[6] = "ستون";
			Tens[7] = "سبعون";
			Tens[8] = "ثمانون";
			Tens[9] = "تسعون";
			Thousands[0] = "";
			Thousands[1] = "ألف"; // US numbering
			Thousands[2] = "مليون";
			Thousands[3] = "بليون";
			Thousands[4] = "تريليون";
			
			//Arabic Numbers
			TeensA = "æä";
			onesA[0] = "ÕÝÑ";
			onesA[1] = "æÇÍÏ";
			onesA[2] = "ÅËäÇä";
			onesA[3] = "ËáÇË";
			onesA[4] = "ÃÑÈÚ";
			onesA[5] = "ÎãÓ";
			onesA[6] = "ÓÊ";
			onesA[7] = "ÓÈÚ";
			onesA[8] = "ËãÇä";
			onesA[9] = "ÊÓÚ";
			TensA[0] = "ÚÔÑ";
			TensA[1] = "ÅÍÏì";
			TensA[2] = "ÅËäì";
			TensA[3] = "ËáÇËÉ";
			TensA[4] = "ÃÑÈÚÉ";
			TensA[5] = "ÎãÓÉ";
			TensA[6] = "ÓÊÉ";
			TensA[7] = "ÓÈÚÉ";
			TensA[8] = "ËãÇäíÉ";
			TensA[9] = "ÊÓÚÉ";
			HunderdsA[0] = "";
			HunderdsA[1] = "ãÇÆÉ";
			HunderdsA[2] = "ãÇÆÊ" + "Çä";
			HunderdsA[3] = onesA[3] + "ãÇÆÉ";
			HunderdsA[4] = onesA[4] + "ãÇÆÉ";
			HunderdsA[5] = onesA[5] + "ãÇÆÉ";
			HunderdsA[6] = onesA[6] + "ãÇÆÉ";
			HunderdsA[7] = onesA[7] + "ãÇÆÉ";
			HunderdsA[8] = onesA[8] + "ãÇÆÉ";
			HunderdsA[9] = onesA[9] + "ãÇÆÉ";
			ThousandsA[0] = "";
			ThousandsA[1] = "ÃáÝ";
			ThousandsA[2] = "ÃáÝ" + "Çä";
			ThousandsA[3] = TensA[3] + " " + "ÂáÇÝ";
			ThousandsA[4] = TensA[4] + " " + "ÂáÇÝ";
			ThousandsA[5] = TensA[5] + " " + "ÂáÇÝ";
			ThousandsA[6] = TensA[6] + " " + "ÂáÇÝ";
			ThousandsA[7] = TensA[7] + " " + "ÂáÇÝ";
			ThousandsA[8] = TensA[8] + " " + "ÂáÇÝ";
			ThousandsA[9] = TensA[9] + " " + "ÂáÇÝ";
			MillionsA[0] = "";
			MillionsA[1] = "ãáíæä";
			MillionsA[2] = "ãáíæäÇä";
			MillionsA[3] = TensA[3] + " " + "ãáÇííä";
			MillionsA[4] = TensA[4] + " " + "ãáÇííä";
			MillionsA[5] = TensA[5] + " " + "ãáÇííä";
			MillionsA[6] = TensA[6] + " " + "ãáÇííä";
			MillionsA[7] = TensA[7] + " " + "ãáÇííä";
			MillionsA[8] = TensA[8] + " " + "ãáÇííä";
			MillionsA[9] = TensA[9] + " " + "ãáÇííä";
			MilliarsA[0] = "";
			MilliarsA[1] = "ãáíÇÑ";
			MilliarsA[2] = "ãáíÇÑÇä";
			MilliarsA[3] = onesA[3] + " " + "ãáíÇÑÇÊ";
			MilliarsA[4] = onesA[4] + " " + "ãáíÇÑÇÊ";
			MilliarsA[5] = onesA[5] + " " + "ãáíÇÑÇÊ";
			MilliarsA[6] = onesA[6] + " " + "ãáíÇÑÇÊ";
			MilliarsA[7] = onesA[7] + " " + "ãáíÇÑÇÊ";
			MilliarsA[8] = onesA[8] + " " + "ãáíÇÑÇÊ";
			MilliarsA[9] = onesA[9] + " " + "ãáíÇÑÇÊ";
			TrillionsA[0] = "";
			TrillionsA[1] = "ÊÑíáíæä";
			TrillionsA[2] = "ÊÑíáíæäÇä";
			TrillionsA[3] = TensA[3] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[4] = TensA[4] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[5] = TensA[5] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[6] = TensA[6] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[7] = TensA[7] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[8] = TensA[8] + " " + "ÊÑíáíæäÇÊ";
			TrillionsA[9] = TensA[9] + " " + "ÊÑíáíæäÇÊ";
		}        
		#endregion 

		#region Number-To-Text Functions

		#region English Number-To-Text
		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <param name="LaggingString">A string to be added to the end of the number</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(long Number, string LeadingString, string LaggingString) 
		{
			int nCol = 0, nChar = 0;
			bool bAllZeros = true; // Non-zero digit not yet encountered
			bool bShowThousands = false;
			string strBuff = null, strTemp = null;
			string strVal = Number.ToString();
			
			if (Strings.Len(strVal) > 15)
			{
				strBuff = "#Error#";
				return strBuff;
			}
			else
			{
				// Trap errors
				try
				{
					// Iterate through string
					for (int i=Strings.Len(strVal); i>=1; i+=-1)
					{
						// Get value of this digit
						nChar = System.Convert.ToInt32(Conversion.Val(Strings.Mid(strVal, i, 1)));
						// Get column position
						nCol = (Strings.Len(strVal) - i) + 1;
						// Action depends on 1's, 10's or 100's column
						switch ((nCol % 3))
						{
							case 1:
								bShowThousands = true;
								if (i == 1)
								{
									// First digit in number (last in the loop)
									strTemp = ones[nChar] + " ";
								}
								else if (Strings.Mid(strVal, i - 1, 1) == "1")
								{
									// This digit is part of "teen" number
									strTemp = Teens[nChar] + " ";
									i = i - 1; // Skip tens position
								} 
								else if (nChar > 0)
								{
									// Any non-zero digit
									strTemp = ones[nChar] + " ";
								}
								else
								{
									// This digit is zero. If digit in tens and hundreds column
									// are also zero, don't show "thousands"
									bShowThousands = false;
									// Test for non-zero digit in this grouping
									if (Strings.Mid(strVal, i - 1, 1) != "0")
									{
										bShowThousands = true;
									}
									else if (i > 2)
									{
										if (Strings.Mid(strVal, i - 2, 1) != "0")
										{
											bShowThousands = true;
										}
									}
									strTemp = "";
								}
								// Show "thousands" if non-zero in grouping
								if (bShowThousands)
								{
									if (nCol > 1)
									{
										strTemp = strTemp + Thousands[nCol / 3];
										if (bAllZeros)
										{
											strTemp = strTemp + " ";
										}
										else
										{
											strTemp = strTemp + ", ";
										}
									}
									// Indicate non-zero digit encountered
									bAllZeros = false;
								}
								strBuff = strTemp + strBuff;
								break;
							case 2:
								if (nChar > 0)
								{
									if (Strings.Mid(strVal, i + 1, 1) != "0")
									{
										strBuff = Tens[nChar] + "-" + strBuff;
									}
									else
									{
										strBuff = Tens[nChar] + " " + strBuff;
									}
								}
								break;
							case 0:
								if (nChar > 0)
								{
									strBuff = ones[nChar] + " مائة " + strBuff;
								}
								break;
						}
					} 
					// Convert first letter to upper case
					strBuff = Strings.UCase( Strings.Left( strBuff, 1 ) ) + Strings.Mid( strBuff, 2 );
					// Return result
					return LeadingString + strBuff.TrimEnd() + LaggingString;
				}
				catch
				{
					//goto NumToTextError;
					strBuff = "#Error#";
					return strBuff;
				}
			}
		} 
        
		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(long Number)
		{
			return EnglishNumToText(Number, "");
		}

		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(long Number, string LeadingString)
		{ 
			return EnglishNumToText(Number, LeadingString, "");
		}        
		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <param name="LaggingString">A string to be added to the end of the number</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(string Number, string LeadingString, string LaggingString)
		{
			return EnglishNumToText(System.Convert.ToInt64(Conversion.Val(Number)), LeadingString, LaggingString);
		}
		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(string Number)
		{
			return EnglishNumToText(Number, "");
		}
		/// <summary>
		/// Returns a string representing the text of a given number in English
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <returns>An English string equivalent to the number</returns>
		public string EnglishNumToText(string Number, string LeadingString)
		{
			return EnglishNumToText(Number, LeadingString, "");
		}
		#endregion

		#region Arabic Number-To-Text
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <param name="LaggingString">A string to be added to the end of the number</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(long Number, string LeadingString, string LaggingString)
		{
			string NumToTextA = null;
			string strVal = Number.ToString();

			if (strVal == "0")
			{
				NumToTextA = aOnes("0");
				return LeadingString + NumToTextA + LaggingString;
			}
			
			switch (Strings.Len(strVal))
			{
				case 1:
					NumToTextA = aOnes(strVal);
					break;
				case 2:
					NumToTextA = aTens(strVal);
					break;
				case 3:
					NumToTextA = aHunderds(strVal);
					break;
				case 4:
					NumToTextA = aThousands(strVal);
					break;
				case 5:
					NumToTextA = aTenThousands(strVal);
					break;
				case 6:
					NumToTextA = aHunderdThousands(strVal);
					break;
				case 7:
					NumToTextA = aMillions(strVal);
					break;
				case 8:
					NumToTextA = aTenMillions(strVal);
					break;
				case 9:
					NumToTextA = aHunderdMillions(strVal);
					break;
				case 10:
					NumToTextA = aMilliars(strVal);
					break;
				case 11:
					NumToTextA = aTenMilliars(strVal);
					break;
				case 12:
					NumToTextA = aHunderdMilliars(strVal);
					break;
				case 13:
					NumToTextA = aTrillions(strVal);
					break;
				case 14:
					NumToTextA = aTenTrillions(strVal);
					break;
				case 15:
					NumToTextA = aHunderdTrillions(strVal);
					break;
				default:
					NumToTextA = "#ÛíÑ ãÚÑøóÝ#";
					break;
			}
			
			return LeadingString + NumToTextA + LaggingString;
		}
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(long Number)
		{
			return ArabicNumToText(Number, "");
		}
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (long) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(long Number, string LeadingString)
		{
			return ArabicNumToText(Number, LeadingString, "");
		}
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <param name="LaggingString">A string to be added to the end of the number</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(string Number, string LeadingString, string LaggingString)
		{
			return ArabicNumToText(System.Convert.ToInt64(Conversion.Val(Number)), LeadingString, LaggingString);
		}
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(string Number)
		{
			return ArabicNumToText(Number, "");
		}
		/// <summary>
		/// Returns a string representing the text of a given number in Arabic
		/// </summary>
		/// <param name="Number">The input number (string) to be translated into text</param>
		/// <param name="LeadingString">A string to be added to the beginning of the number</param>
		/// <returns>An Arabic string equivalent to the number</returns>
		public string ArabicNumToText(string Number, string LeadingString)
		{
			return ArabicNumToText(Number, LeadingString, "");
		}
		#endregion

		#endregion 

		#region Private Functions (Needed by ArabicNumToText)
		private string aOnes(string s)
		{
			string aOnes1;

			switch (System.Convert.ToInt32(Conversion.Val(s)))
			{
				case 0:
				case 1:
				case 2:
					aOnes1 = onesA[System.Convert.ToInt32(Conversion.Val(s))];
					break;
				case 8:
					aOnes1 = onesA[System.Convert.ToInt32(Conversion.Val(s))] + "íÉ";
					break;
				default:
					aOnes1 = onesA[System.Convert.ToInt32(Conversion.Val(s))] + "É";
					break;
			}

			return aOnes1;
		}

		private string aTens(string s)
		{
			string sr = Strings.Right(s, 1);
			int isr = System.Convert.ToInt32(Conversion.Val(sr));
			string sl = Strings.Left(s, 1);
			int isl = System.Convert.ToInt32(Conversion.Val(sl));

			if (sl == "0")
			{
				return aOnes(sr);
			}

			string aTens1;

			if (sr == "0")
			{
				switch (isl)
				{
					case 1:
						aTens1 = TensA[0] + "É";
						break;
					case 2:
						aTens1 = TensA[0] + TeensA;
						break;
					default:
						aTens1 = onesA[isl] + TeensA;
						break;
				}
			}
			else
			{
				if (sl == "1")
				{
					aTens1 = TensA[isr] + " " + TensA[0];
				}
				else
				{
					if (sl == "2")
					{
						aTens1 = TensA[isr] + " æ " + TensA[0] + TeensA;
					}
					else
					{
						aTens1 = onesA[isr] + " æ " + onesA[isl] + TeensA;
					}
				}
			}

			return aTens1;
		}

		private string aHunderds(string s)
		{
			string s1 = Strings.Left(s, 1);
			int is1 = System.Convert.ToInt32(Conversion.Val(s1));
			string s2 = Strings.Mid(s, 2, 1);
			int is2 = System.Convert.ToInt32(Conversion.Val(s2));
			string s3 = Strings.Right(s, 1);
			int is3 = System.Convert.ToInt32(Conversion.Val(s3));
			string s23 = s2 + s3;

			if ((is2 == 0) && (is3 == 0))
			{
				return HunderdsA[is1];
			}
			
			if (is1 == 0)
			{
				return HunderdsA[is1] + ArabicNumToText(s23);
			}
			else
			{
				return HunderdsA[is1] + " æ " + ArabicNumToText(s23);
			}
		}

		private string aThousands(string s)
		{
			string s1 = Strings.Left(s, 1);
			int is1 = System.Convert.ToInt32(Conversion.Val(s1));
			string s234 = Strings.Mid(s, 2);

			if (s234 == "000")
			{
				return ThousandsA[is1];
			}

			if (is1 == 0)
			{
				return ThousandsA[is1] + ArabicNumToText(s234);
			}
			else
			{
				return ThousandsA[is1] + " æ " + ArabicNumToText(s234);
			}
		}
		private string aTenThousands(string s)
		{
			string s1 = Strings.Mid(s, 1, 2);
			string s345 = Strings.Mid(s, 3);

			if (Conversion.Val(s345) == 0)
			{
				if (s1 == "10")
				{
					return ArabicNumToText(s1) + " ÂáÇÝ";
				}
				else
				{
					return ArabicNumToText(s1) + " ÃáÝ";
				}
			}
			
			if (s1 == "10")
			{
				return ArabicNumToText(s1) + " ÂáÇÝ æ " + ArabicNumToText(s345);
			}
			else
			{
				return ArabicNumToText(s1) + " ÃáÝ æ " + ArabicNumToText(s345);
			}

		}
		private string aHunderdThousands(string s)
		{
			string s1 = Strings.Mid(s, 1, 3);
			string s456 = Strings.Mid(s, 4);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (Conversion.Val(s) == 200000)
				{
					return Strings.Left(ArabicNumToText(s1), 5) + " ÃáÝ";
				}
				else
				{
					return ArabicNumToText(s1) + " ÃáÝ";
				}
			}
			
			if (s456 == "000")
			{
				return ArabicNumToText(s1) + " ÃáÝ";
			}

			if (s1 == "200")
			{
				return Strings.Left(ArabicNumToText(s1), 5) + " ÃáÝ æ " + ArabicNumToText(s456);
			}
			else
			{
				return ArabicNumToText(s1) + " ÃáÝ æ " + ArabicNumToText(s456);
			}
		}
		private string aMillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 1);
			string sr = Strings.Mid(s, 2);

			if (Conversion.Val(sr) == 0)
			{
				return MillionsA[System.Convert.ToInt32(s1)];
			}

			return MillionsA[System.Convert.ToInt32(s1)] + " æ " + ArabicNumToText(sr);
		}
		private string aTenMillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 2);
			string sr = Strings.Mid(s, 3);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (s1 == "10")
				{
					return ArabicNumToText(s1) + " ãáÇííä";
				}
				else
				{
					return ArabicNumToText(s1) + " ãáíæä";
				}
			}
			
			if (s1 == "10")
			{
				return ArabicNumToText(s1) + " ãáÇííä æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ãáíæä æ " + ArabicNumToText(sr);
			}
		}
		private string aHunderdMillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 3);
			string sr = Strings.Mid(s, 4);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (Conversion.Val(s) == 200000000)
				{
					return Strings.Left(ArabicNumToText(s1), 5) + " ãáíæä";
				}
				else
				{
					return ArabicNumToText(s1) + " ãáíæä";
				}
			}

			if (sr == "000000")
			{
				return ArabicNumToText(s1) + " ãáíæä";
			}
			if (s1 == "200")
			{
				return Strings.Left(ArabicNumToText(s1), 5) + " ãáíæä æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ãáíæä æ " + ArabicNumToText(sr);
			}
		}
		private string aMilliars(string s)
		{
			string s1 = Strings.Mid(s, 1, 1);
			string sr = Strings.Mid(s, 2);

			if (Conversion.Val(sr) == 0)
			{
				return MilliarsA[System.Convert.ToInt32(s1)];
			}
			return MilliarsA[System.Convert.ToInt32(s1)] + " æ " + ArabicNumToText(sr);
		}
		private string aTenMilliars(string s)
		{
			string s1 = Strings.Mid(s, 1, 2);
			string sr = Strings.Mid(s, 3);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (s1 == "10")
				{
					return ArabicNumToText(s1) + " ãáíÇÑÇÊ";
				}
				else
				{
					return ArabicNumToText(s1) + " ãáíÇÑ";
				}
			}
			
			if (s1 == "10")
			{
				return ArabicNumToText(s1) + " ãáíÇÑÇÊ æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ãáíÇÑ æ " + ArabicNumToText(sr);
			}
		}
		private string aHunderdMilliars(string s)
		{
			string s1 = Strings.Mid(s, 1, 3);
			string sr = Strings.Mid(s, 4);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (Conversion.Val(s) == System.Convert.ToDouble(200000000000))
				{
					return Strings.Left(ArabicNumToText(s1), 5) + " ãáíÇÑ";
				}
				else
				{
					return ArabicNumToText(s1) + " ãáíÇÑ";
				}
			}
			if (sr == "000000000")
			{
				return ArabicNumToText(s1) + " ãáíÇÑ";
			}
			if (s1 == "200")
			{
				return Strings.Left(ArabicNumToText(s1), 5) + " ãáíÇÑ æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ãáíÇÑ æ " + ArabicNumToText(sr);
			}
		}
		private string aTrillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 1);
			string sr = Strings.Mid(s, 2);

			if (Conversion.Val(sr) == 0)
			{
				return TrillionsA[System.Convert.ToInt32(s1)];
			}
			return TrillionsA[System.Convert.ToInt32(s1)] + " æ " + ArabicNumToText(sr);
		}
		private string aTenTrillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 2);
			string sr = Strings.Mid(s, 3);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (s1 == "10")
				{
					return ArabicNumToText(s1) + " ÊÑíáíæäÇÊ";
				}
				else
				{
					return ArabicNumToText(s1) + " ÊÑíáíæä";
				}
			}
			if (s1 == "10")
			{
				return ArabicNumToText(s1) + " ÊÑíáíæäÇÊ æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ÊÑíáíæä æ " + ArabicNumToText(sr);
			}
		}
		private string aHunderdTrillions(string s)
		{
			string s1 = Strings.Mid(s, 1, 3);
			string sr = Strings.Mid(s, 4);

			if (Conversion.Val(Strings.Mid(s, 2)) == 0)
			{
				if (Conversion.Val(s) == System.Convert.ToDouble(200000000000000))
				{
					return Strings.Left(ArabicNumToText(s1), 5) + " ÊÑíáíæä";
				}
				else
				{
					return ArabicNumToText(s1) + " ÊÑíáíæä";
				}
			}
			if (sr == "000000000")
			{
				return ArabicNumToText(s1) + " ÊÑíáíæä";
			}
			if (s1 == "200")
			{
				return Strings.Left(ArabicNumToText(s1), 5) + " ÊÑíáíæä æ " + ArabicNumToText(sr);
			}
			else
			{
				return ArabicNumToText(s1) + " ÊÑíáíæä æ " + ArabicNumToText(sr);
			}
		}

		#endregion

	}
}