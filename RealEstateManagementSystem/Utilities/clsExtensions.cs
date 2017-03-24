﻿using RealEstateManagementSystem.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RealEstateManagementSystem.Utilities
{
    public static partial class clsExtensions
    {

        //this.Controls.ClearControls();
        //this.Controls.ClearControls<TextBox>();
        private static Dictionary<Type, Action<Control>> controldefaults = new Dictionary<Type, Action<Control>>() {
            {typeof(TextBox), c => ((TextBox)c).Clear()},
            {typeof(CheckBox), c => ((CheckBox)c).Checked = false},
            {typeof(ListBox), c => ((ListBox)c).Items.Clear()},
            {typeof(RadioButton), c => ((RadioButton)c).Checked = false},
            {typeof(GroupBox), c => ((GroupBox)c).Controls.ClearControls()},
            {typeof(Panel), c => ((Panel)c).Controls.ClearControls()}
            };

        private static void FindAndInvoke(Type type, Control control)
        {
            if (controldefaults.ContainsKey(type))
            {
                controldefaults[type].Invoke(control);
            }
        }

        public static void ClearControls(this Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                FindAndInvoke(control.GetType(), control);
            }
        }

        public static void ClearControls<T>(this Control.ControlCollection controls) where T : class
        {
            if (!controldefaults.ContainsKey(typeof(T))) return;

            foreach (Control control in controls)
            {
                if (control.GetType().Equals(typeof(T)))
                {
                    FindAndInvoke(typeof(T), control);
                }
            }
        }
        public static string RemoveCommas(this string String)
        {
            return String.Replace(",", "");
        }
        public static string RemoveBrackets(this string String)
        {
            string str = String;
            str = String.Replace("(", "");
            str = String.Replace(")", "");
            return str;
        }

        public static bool ConvertToBoolean(this string str)
        {
            return str == "0" || str.ToUpper() == "FALSE" ? false : true;
        }

        public static int ConvertToInt32(this string str)
        {
            return Convert.ToInt32(str.RemoveCommas());
        }

        /// <summary>
        /// Format Decimal number as Comma-Seperated string format (xx,xx,xxx)
        /// </summary>
        /// <param name="amount">Value</param>
        /// <param name="addCurrencyMark">Add Tk. in front and /- in end</param>
        /// <returns></returns>
        public static string FormatAsMoney(this decimal amount, bool addCurrencyMark = false, bool useBracketForNegative = false)
        {
            if (useBracketForNegative == true)
            {
                return amount < 0 ?
                    addCurrencyMark == true ? "(Tk." + Spell.SpellAmount.comma(amount * -1) + "/-)" : "(" + Spell.SpellAmount.comma(amount * -1) + ")" :
                    addCurrencyMark == true ? "Tk. " + Spell.SpellAmount.comma(amount) + "/-" : Spell.SpellAmount.comma(amount);
            }
            else
            {
                return amount < 0 ?
                    addCurrencyMark == true ? "Tk. -" + Spell.SpellAmount.comma(amount * -1) + "/-" : "-" + Spell.SpellAmount.comma(amount * -1) + "" :
                    addCurrencyMark == true ? "Tk. " + Spell.SpellAmount.comma(amount) + "/-" : Spell.SpellAmount.comma(amount);
            }

        }

        public static string AmountInWords(this decimal amount)
        {

            return amount < 0 ? Spell.SpellAmount.InWrods(amount * -1) + " (Refund)" : Spell.SpellAmount.InWrods(amount);
        }

        public static string ShowAsStandardDateFormat(this string str, bool showWithTime = false)
        {
            DateTime dt = DateTime.Now;
            string strFormat = showWithTime == true ? "dd-MMM-yyyy hh:mm:ss tt" : "dd-MMM-yyyy";
            return DateTime.TryParse(str, out dt) == true ? Convert.ToDateTime(str.ToString()).ToString(strFormat) : str;

        }

        public static DateTime ConvertToDateTime(this string str)
        {
            DateTime dt = DateTime.Now;
            //string strFormat = showWithTime == true ? "dd-MMM-yyyy hh:mm:ss tt" : "dd-MMM-yyyy";
            return DateTime.TryParse(str, out dt) == true ? Convert.ToDateTime(str.ToString()) : DateTime.Now;

        }

        public static string ReplaceEmptyStringWithNA(this string str)
        {
            return string.IsNullOrEmpty(str) ? "N/A" : str;
        }

        public static decimal ConvertToDecimal(this string str)
        {
            return Convert.ToDecimal(str.RemoveCommas());
        }

        public static int ConverBooleanToSmallInt(this bool bln)
        {
            return bln == true ? 1 : 0;
        }

        public static Exception Log(this Exception ex)
        {
            File.AppendAllText("CaughtExceptions" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("HH:mm:ss") + ": " + ex.Message + "\n" + ex.ToString() + "\n");
            return ex;
        }

        //public static Exception Display(this Exception ex, string msg = null, MessageBoxIcon img = MessageBoxIcon.Error)
        //{
        //    //new ApplicationException("Unable to calculate !", ex).Log().Display();
        //    MessageBox.Show(msg ?? ex.Message, "", MessageBoxButtons.OK , img);
        //    return ex;
        //}

        public static Exception Display(this Exception ex)
        {
            //File.AppendAllText("CaughtExceptions" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("HH:mm:ss") + ": " + ex.Message + "\n" + ex.ToString() + "\n");
            string errMsgInitial = string.Empty;
            if (!ex.GetType().IsAssignableFrom(typeof(ApplicationException)) && !ex.GetType().IsAssignableFrom(typeof(SqlException)))
            {
                clsCommonFunctions.LogError(ex);
                errMsgInitial = Resources.strSystemErrorMessage;
            }
            else
            {
                errMsgInitial = Resources.strFailedMessage;
            }
            MessageBox.Show(errMsgInitial + ex.Message.ToString(), Resources.strFailedCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return ex;
        }


    }
}