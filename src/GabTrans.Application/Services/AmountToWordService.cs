using GabTrans.Application.Abstractions.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Services
{
    public class AmountToWordService : IAmountToWordService
    {
        private static string ConvertDigit(string myDigit)
        {
            string functionReturnValue = "";
            switch (myDigit)
            {
                case "1":
                    functionReturnValue = "One";
                    break;
                case "2":
                    functionReturnValue = "Two";
                    break;
                case "3":
                    functionReturnValue = "Three";
                    break;
                case "4":
                    functionReturnValue = "Four";
                    break;
                case "5":
                    functionReturnValue = "Five";
                    break;
                case "6":
                    functionReturnValue = "Six";
                    break;
                case "7":
                    functionReturnValue = "Seven";
                    break;
                case "8":
                    functionReturnValue = "Eight";
                    break;
                case "9":
                    functionReturnValue = "Nine";
                    break;
                default:
                    functionReturnValue = string.Empty;
                    break;
            }
            return functionReturnValue;
        }

        private static string ConvertTens(string myTens)
        {
            //Try
            string result = string.Empty;

            // Is value between 10 and 19?
            if (myTens.Substring(0, 1).Equals("1"))
            {
                switch ((myTens))
                {
                    case "10":
                        result = "Ten";
                        break;
                    case "11":
                        result = "Eleven";
                        break;
                    case "12":
                        result = "Twelve";
                        break;
                    case "13":
                        result = "Thirteen";
                        break;
                    case "14":
                        result = "Fourteen";
                        break;
                    case "15":
                        result = "Fifteen";
                        break;
                    case "16":
                        result = "Sixteen";
                        break;
                    case "17":
                        result = "Seventeen";
                        break;
                    case "18":
                        result = "Eighteen";
                        break;
                    case "19":
                        result = "Nineteen";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // .. otherwise it's between 20 and 99.
                switch (myTens.Substring(0, 1))
                {
                    case "2":
                        result = "Twenty ";
                        break;
                    case "3":
                        result = "Thirty ";
                        break;
                    case "4":
                        result = "Forty ";
                        break;
                    case "5":
                        result = "Fifty ";
                        break;
                    case "6":
                        result = "Sixty ";
                        break;
                    case "7":
                        result = "Seventy ";
                        break;
                    case "8":
                        result = "Eighty ";
                        break;
                    case "9":
                        result = "Ninety ";
                        break;
                    default:
                        break;
                }

                // Convert ones place digit.
                result = result + ConvertDigit(myTens.Substring(1, 1));
            }
            return result;
            //Catch conv As Exception
            //    MsgBox(conv.Message)
            //End Try
        }

        private static string ConvertHundreds(string myNumber)
        {
            //Try
            string result = string.Empty;

            // Exit if there is nothing to convert.
            //If Val(MyNumber) = 0 Then
            //    Exit Function
            //End If

            // Append leading zeros to number.
            myNumber = Microsoft.VisualBasic.Strings.Right("000" + myNumber, 3);

            // Do we have a hundreds place digit to convert?
            if (Microsoft.VisualBasic.Strings.Left(myNumber, 1) != "0")
            {
                if (Strings.Mid(myNumber, 2, 1) != "0")
                {
                    result = ConvertDigit(Microsoft.VisualBasic.Strings.Left(myNumber, 1)) + " Hundred and ";

                }
                else
                {
                    result = ConvertDigit(Microsoft.VisualBasic.Strings.Left(myNumber, 1)) + " Hundred ";
                }
            }

            // Do we have a tens place digit to convert?
            if (Strings.Mid(myNumber, 2, 1) != "0")
            {
                result = result + ConvertTens(Strings.Mid(myNumber, 2));
            }
            else
            {
                // If not, then convert the ones place digit.
                result = result + ConvertDigit(Strings.Mid(myNumber, 3));
            }

            return Strings.Trim(result);
            //Catch conv As Exception
            //    MsgBox(conv.Message)
            //End Try
        }

        public string NumberToNaira(string myNumber)
        {
            //Try
            string Temp = null;
            string Naira = string.Empty;
            string Kobo = string.Empty;
            int Decimalplace = 0;
            int Count = 0;

            string[] Place = new string[10];
            Place[2] = " Thousand ";
            Place[3] = " Million ";
            Place[4] = " Billion ";
            Place[5] = " Trillion ";

            // Convert MyNumber to a string, trimming extra spaces.
            myNumber = Strings.Trim(myNumber);

            // Find decimal place.
            Decimalplace = Strings.InStr(myNumber, ".", Microsoft.VisualBasic.CompareMethod.Text);

            // If we find decimal place...
            if (Decimalplace > 0)
            {
                // Convert kobo
                Temp = Microsoft.VisualBasic.Strings.Left(Strings.Mid(myNumber, Decimalplace + 1) + "00", 2);
                Kobo = ConvertTens(Temp);

                // Strip off kobo from remainder to convert.
                myNumber = Strings.Trim(Microsoft.VisualBasic.Strings.Left(myNumber, Decimalplace - 1));
            }

            Count = 1;
            while (myNumber != string.Empty)
            {
                // Convert last 3 digits of MyNumber to English Naira.
                Temp = ConvertHundreds(Microsoft.VisualBasic.Strings.Right(myNumber, 3));
                if (Temp != string.Empty)
                    Naira = Temp + Place[Count] + Naira;
                if (Strings.Len(myNumber) > 3)
                {
                    // Remove last 3 converted digits from MyNumber.
                    myNumber = Microsoft.VisualBasic.Strings.Left(myNumber, Strings.Len(myNumber) - 3);
                }
                else
                {
                    myNumber = string.Empty;
                }
                //Count = Count + 1
                Count += 1;
            }

            // Clean up Naira.
            switch (Naira)
            {
                case "":
                    Naira = "Naira";
                    //"No Naira"
                    break;
                case "One":
                    Naira = "One Naira";
                    break;
                default:
                    Naira = Naira + " Naira";
                    break;
            }

            // Clean up kobo.
            switch (Kobo)
            {
                case "":
                    Kobo = " Only.";
                    break;
                case "One":
                    Kobo = " One Kobo Only.";
                    break;
                default:
                    Kobo = " " + Kobo + " Kobo Only.";
                    break;
            }

            return Naira + Kobo;
            //Catch conv As Exception
            //    MsgBox(conv.Message)
            //End Try
        }

        public string NumberToCedis(string myNumber)
        {
            //Try
            string Temp = null;
            string Cedis = string.Empty;
            string Pesewa = string.Empty;
            int Decimalplace = 0;
            int Count = 0;

            string[] Place = new string[10];
            Place[2] = " Thousand ";
            Place[3] = " Million ";
            Place[4] = " Billion ";
            Place[5] = " Trillion ";

            // Convert MyNumber to a string, trimming extra spaces.
            myNumber = Strings.Trim(myNumber);

            // Find decimal place.
            Decimalplace = Strings.InStr(myNumber, ".", Microsoft.VisualBasic.CompareMethod.Text);

            // If we find decimal place...
            if (Decimalplace > 0)
            {
                // Convert kobo
                Temp = Microsoft.VisualBasic.Strings.Left(Strings.Mid(myNumber, Decimalplace + 1) + "00", 2);
                Pesewa = ConvertTens(Temp);

                // Strip off kobo from remainder to convert.
                myNumber = Strings.Trim(Microsoft.VisualBasic.Strings.Left(myNumber, Decimalplace - 1));
            }

            Count = 1;
            while (myNumber != string.Empty)
            {
                // Convert last 3 digits of MyNumber to English Naira.
                Temp = ConvertHundreds(Microsoft.VisualBasic.Strings.Right(myNumber, 3));
                if (Temp != string.Empty)
                    Cedis = Temp + Place[Count] + Cedis;
                if (Strings.Len(myNumber) > 3)
                {
                    // Remove last 3 converted digits from MyNumber.
                    myNumber = Microsoft.VisualBasic.Strings.Left(myNumber, Strings.Len(myNumber) - 3);
                }
                else
                {
                    myNumber = string.Empty;
                }
                //Count = Count + 1
                Count += 1;
            }

            // Clean up Naira.
            switch (Cedis)
            {
                case "":
                    Cedis = "Cedis";
                    //"No Naira"
                    break;
                case "One":
                    Cedis = "One Cedis";
                    break;
                default:
                    Cedis = Cedis + " Cedis";
                    break;
            }

            // Clean up kobo.
            switch (Pesewa)
            {
                case "":
                    Pesewa = " Only.";
                    break;
                case "One":
                    Pesewa = " One Pesewa Only.";
                    break;
                default:
                    Pesewa = " " + Pesewa + " Pesewa Only.";
                    break;
            }

            return Cedis + Pesewa;
            //Catch conv As Exception
            //    MsgBox(conv.Message)
            //End Try
        }
        public string NumberToDollar(string myNumber)
        {
            //Try
            string Temp = null;
            string Dollar = string.Empty;
            string Cent = string.Empty;
            int Decimalplace = 0;
            int Count = 0;

            string[] Place = new string[10];
            Place[2] = " Thousand ";
            Place[3] = " Million ";
            Place[4] = " Billion ";
            Place[5] = " Trillion ";

            // Convert MyNumber to a string, trimming extra spaces.
            myNumber = Strings.Trim(myNumber);

            // Find decimal place.
            Decimalplace = Strings.InStr(myNumber, ".", Microsoft.VisualBasic.CompareMethod.Text);

            // If we find decimal place...
            if (Decimalplace > 0)
            {
                // Convert kobo
                Temp = Microsoft.VisualBasic.Strings.Left(Strings.Mid(myNumber, Decimalplace + 1) + "00", 2);
                Cent = ConvertTens(Temp);

                // Strip off kobo from remainder to convert.
                myNumber = Strings.Trim(Microsoft.VisualBasic.Strings.Left(myNumber, Decimalplace - 1));
            }

            Count = 1;
            while (myNumber != string.Empty)
            {
                // Convert last 3 digits of MyNumber to English Naira.
                Temp = ConvertHundreds(Microsoft.VisualBasic.Strings.Right(myNumber, 3));
                if (Temp != string.Empty)
                    Dollar = Temp + Place[Count] + Dollar;
                if (Strings.Len(myNumber) > 3)
                {
                    // Remove last 3 converted digits from MyNumber.
                    myNumber = Microsoft.VisualBasic.Strings.Left(myNumber, Strings.Len(myNumber) - 3);
                }
                else
                {
                    myNumber = string.Empty;
                }
                //Count = Count + 1
                Count += 1;
            }

            // Clean up Dollar.
            switch (Dollar)
            {
                case "":
                    Dollar = "Dollar";
                    //"No Naira"
                    break;
                case "One":
                    Dollar = "One Dollar";
                    break;
                default:
                    Dollar = Dollar + " Dollar";
                    break;
            }

            // Clean up Cent.
            switch (Cent)
            {
                case "":
                    Cent = " Only.";
                    break;
                case "One":
                    Cent = " One Cent Only.";
                    break;
                default:
                    Cent = " " + Cent + " Cent Only.";
                    break;
            }

            return Dollar + Cent;
            //Catch conv As Exception
            //    MsgBox(conv.Message)
            //End Try
        }
    }
}
